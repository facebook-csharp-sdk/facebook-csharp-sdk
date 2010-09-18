powershell -Command "& {Import-Module ..\Lib\PSake\psake.psm1; Invoke-psake .\build.ps1 %*}"
pause