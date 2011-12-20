@echo off

PUSHD "%~dp0"

SET MSBuild=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe
IF NOT EXIST "%MSBuild%" (
	ECHO Installation of .NET Framework 4.0 is required to build this project
	ECHO http://www.microsoft.com/downloads/details.aspx?FamilyID=0a391abd-25c1-4fc0-919f-b21f31ab88b7
	START /d "~\iexplore.exe" http://www.microsoft.com/downloads/details.aspx?FamilyID=0a391abd-25c1-4fc0-919f-b21f31ab88b7
	EXIT /b 1
	GOTO END
)

"%MSBuild%" "%~dp0Source/Facebook-Net40.sln" /target:Clean /property:TargetFrameworkVersion=v4.0;Configuration=Release
"%MSBuild%" "%~dp0Source/Facebook-Net35.sln" /target:Clean /property:TargetFrameworkVersion=v3.5;Configuration=Release
"%MSBuild%" "%~dp0Source/Facebook-SL5.sln" /target:Clean /property:Configuration=Release
"%MSBuild%" "%~dp0Source/Facebook-SL4.sln" /target:Clean /property:Configuration=Release
"%MSBuild%" "%~dp0Source/Facebook-WP7.sln" /target:Clean /property:Configuration=Release
RMDIR bin /s /q

:END
POPD

pause
