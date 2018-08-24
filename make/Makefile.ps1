Define-Step -Name 'Update Assembly Info' -Target 'build' -Body {
	. (require 'psmake.mod.update-version-info')
	Update-VersionInFile AssemblyVersion.cs $VERSION 'Version("%")'
}

Define-Step -Name 'Build' -Target 'build' -Body {
	call dotnet build OctopusProjectBuilder.sln --configuration Release
}

Define-Step -Name 'Tests' -Target 'build' -Body {
	. (require 'psmake.mod.testing')

	$dotCover = Fetch-Package JetBrains.dotCover.CommandLineTools 2018.1.4
	$reportGenerator = Fetch-Package "ReportGenerator" 3.1.2
	$dotnet = get-command dotnet.exe | Select-Object -ExpandProperty Definition

	$reportDirectory="reports"
	function Run-TestsWithCoverage($csprojFileInfo)
	{
		Write-ShortStatus "Testing $($csprojFileInfo.Name)..."
		$output = "$reportDirectory\$($csprojFileInfo.Name).dcvr"
		call $dotCover\tools\dotCover.exe cover /TargetExecutable=$dotnet /TargetArguments="test $($csprojFileInfo.FullName) --configuration Release --no-build --no-restore" /Output=$output /Filters="+:module=OctopusProjectBuilder*;-:module=*Tests*"
		return $output
	}

	function Merge-Reports
	{
		param(
		[Parameter(Mandatory=$true,ValueFromPipeline=$true)]
		$DcvrFiles
		)
		begin { $results=@() }
		process { $results += $_ }
		end {
			$output = "$reportDirectory\unit-tests.dcvr"
			$source = $results -join ';'
			call $dotCover\tools\dotCover.exe merge /Source=$source /Output=$output
			return $output;
		}
	}

	function Convert-Report($dcvrFile)
	{
		$output="$reportDirectory\unit-tests-coverage.xml"
		call $dotCover\tools\dotCover.exe report /Source=$dcvrFile /ReportType=DetailedXML /Output=$output
		return Create-Object @{ ReportDirectory=$reportDirectory; CoverageReports=$output; }
	}

	Remove-Item $reportDirectory -Force -Recurse -ErrorAction SilentlyContinue

	Get-ChildItem -Recurse . -Filter *.Tests.csproj `
		| %{ Run-TestsWithCoverage $_ } `
		| Merge-Reports `
		| %{ Convert-Report $_} `
		| Generate-CoverageSummary -ReportGeneratorVersion 3.1.2 `
		| Check-AcceptableCoverage -AcceptableCoverage 89
}

Define-Step -Name 'Documentation generation' -Target 'build' -Body {
	& dotnet run --project OctopusProjectBuilder.DocGen\OctopusProjectBuilder.DocGen.csproj --configuration Release | out-file Manual.md -Encoding utf8
	if ($LastExitCode -ne 0) { throw "A program execution was not successful (Exit code: $LASTEXITCODE)." }
}

Define-Step -Name 'Packaging' -Target 'build' -Body {
	. (require 'psmake.mod.packaging')
	call dotnet publish OctopusProjectBuilder.Console\OctopusProjectBuilder.Console.csproj --configuration Release --no-build
	call dotnet pack OctopusProjectBuilder.Console/OctopusProjectBuilder.Console.csproj --configuration Release --output . /p:PackageVersion=$VERSION
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
