Define-Step -Name 'Update Assembly Info' -Target 'build' -Body {
	. (require 'psmake.mod.update-version-info')
	Update-VersionInFile AssemblyVersion.cs $VERSION 'Version("%")'
}

Define-Step -Name 'Build' -Target 'build' -Body {
	call $Context.NugetExe restore OctopusProjectBuilder.sln
	call "${env:ProgramFiles(x86)}\MSBuild\14.0\Bin\msbuild.exe" OctopusProjectBuilder.sln /t:"Clean,Build" /p:Configuration=Release /m /verbosity:m /nologo /p:TreatWarningsAsErrors=true
}

Define-Step -Name 'Tests' -Target 'build' -Body {
	. (require 'psmake.mod.testing')

	$tests = @()
	$tests += Define-NUnit3Tests -GroupName 'Unit Tests' -TestAssembly "*.Tests\bin\Release\*.Tests.dll"

	$tests `
        | Run-Tests -EraseReportDirectory -Cover -CodeFilter '+[OctopusProjectBuilder*]* -[*.Tests*]*' -TestFilter '*.Tests.dll' `
        | Generate-CoverageSummary `
        | Check-AcceptableCoverage -AcceptableCoverage 89
}

Define-Step -Name 'Documentation generation' -Target 'build' -Body {
	& OctopusProjectBuilder.DocGen\bin\Release\OctopusProjectBuilder.DocGen.exe | out-file Manual.md -Encoding utf8
	if ($LastExitCode -ne 0) { throw "A program execution was not successful (Exit code: $LASTEXITCODE)." }
}

Define-Step -Name 'Packaging' -Target 'build' -Body {
	. (require 'psmake.mod.packaging')
	
	Package-DeployableNuSpec -Package 'OctopusProjectBuilder.Console.nuspec' -version $VERSION
}

Define-Step -Name 'Update Wiki' -Target 'update-wiki' -Body {
	$wikiUrl = & git config --get remote.origin.url
	$wikiUrl = $wikiUrl -replace '\.git$','.wiki.git'
	call git clone $wikiUrl wiki

	cp .\Manual.md wiki\Manual.md -Force
	try {
		pushd wiki
		call git commit '-a' '--allow-empty' '-m' "wiki commit $VERSION"
		call git push origin master
	}
	finally {
		popd
	}
}