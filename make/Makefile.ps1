Define-Step -Name 'Build' -Target 'build' -Body {
	call "${env:ProgramFiles(x86)}\MSBuild\14.0\Bin\msbuild.exe" OctopusProjectBuilder.sln /t:"Clean,Build" /p:Configuration=Release /m /verbosity:m /nologo /p:TreatWarningsAsErrors=true
}