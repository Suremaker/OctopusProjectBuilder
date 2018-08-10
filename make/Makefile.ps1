Define-Step -Name 'Update Assembly Info' -Target 'build' -Body {
	. (require 'psmake.mod.update-version-info')
	Update-VersionInFile AssemblyVersion.cs $VERSION 'Version("%")'
}

Define-Step -Name 'Build' -Target 'build' -Body {
	call $Context.NugetExe restore OctopusProjectBuilder.sln
	#call "${env:ProgramFiles(x86)}\MSBuild\14.0\Bin\msbuild.exe" OctopusProjectBuilder.sln /t:"Clean,Build" /p:Configuration=Release /m /verbosity:m /nologo /p:TreatWarningsAsErrors=true
	call dotnet build --configuration Release /p:DebugType=Full
}

Define-Step -Name 'Tests' -Target 'build' -Body {
	. (require 'psmake.mod.testing')
	
	Write-ShortStatus "Preparing OpenCover"
	$NugetPath = $env:USERPROFILE + "\.nuget"
	$openCoverConsole = $NugetPath + "\packages\OpenCover\4.6.519\tools\OpenCover.Console.exe"

	Write-ShortStatus "Running tests with OpenCover"
	$RunnerArgs = "test", "OctopusProjectBuilder.YamlReader.Tests", "--no-build", "-f netcoreapp2.0", "-c Release", "-l:trx;LogFileName=..\..\reports\unit-test-results1.xml"
	call "$openCoverConsole" "-log:Error" "-showunvisited" "-oldStyle" "-register:user" "-target:dotnet.exe" "-targetargs:`"$RunnerArgs`"" "`"-filter:+[OctopusProjectBuilder*]*`"" "-coverbytest:*.Tests.dll" "-output:$PSScriptRoot\..\reports\opencover1.xml"

	$RunnerArgs = "test", "OctopusProjectBuilder.Uploader.Tests", "--no-build", "-f netcoreapp2.0", "-c Release", "-l:trx;LogFileName=..\..\reports\unit-test-results2.xml"
	call "$openCoverConsole" "-log:Error" "-showunvisited" "-oldStyle" "-register:user" "-target:dotnet.exe" "-targetargs:`"$RunnerArgs`"" "`"-filter:+[OctopusProjectBuilder*]*`"" "-coverbytest:*.Tests.dll" "-output:$PSScriptRoot\..\reports\opencover2.xml"

	$tests = @()
	$tests += Create-Object @{CoverageReports = "reports\opencover1.xml", "reports\opencover2.xml";
							  ReportDirectory = $PSScriptRoot + "\..\reports"
							  TestResult = "..\..\reports\unit-test-results.xml"}

	$tests `
        | Generate-CoverageSummary `
        | Check-AcceptableCoverage -AcceptableCoverage 75
}

Define-Step -Name 'Documentation generation' -Target 'build' -Body {
	& dotnet .\OctopusProjectBuilder.DocGen\bin\Release\netcoreapp2.0\OctopusProjectBuilder.DocGen.dll | out-file Manual.md -Encoding utf8
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