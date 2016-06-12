Push-Location $PSScriptRoot
Try
{
	.\make\make.ps1 -t build
}
Finally
{
	Pop-Location
}