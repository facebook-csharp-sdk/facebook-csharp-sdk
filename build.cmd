@echo off

PUSHD "%~dp0"

echo WARNING!!!
echo.
echo This build.cmd is only meant for making the samples easier to run.
echo build.cmd does not run any tests, so do not use the output of the compiled
echo libraries in production using this method.
echo.
echo.
echo For more information on how to build this Facebook C# SDK correctly, refer to
echo http://facebooksdk.codeplex.com/wikipage?title=Building%20Facebook%20C%23%20SDK
echo.

pause

SET MSBuild=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe
IF NOT EXIST "%MSBuild%" (
	ECHO Installation of .NET Framework 4.0 is required to build this project
	ECHO http://www.microsoft.com/downloads/details.aspx?FamilyID=0a391abd-25c1-4fc0-919f-b21f31ab88b7
	START /d "~\iexplore.exe" http://www.microsoft.com/downloads/details.aspx?FamilyID=0a391abd-25c1-4fc0-919f-b21f31ab88b7
	EXIT /b 1
	GOTO END
)

"%MSBuild%" "%~dp0Source/Facebook-Net40.sln" /target:rebuild /property:TargetFrameworkVersion=v4.0;Configuration=Release
"%MSBuild%" "%~dp0Source/Facebook-Net35.sln" /target:rebuild /property:TargetFrameworkVersion=v3.5;Configuration=Release
"%MSBuild%" "%~dp0Source/Facebook-SL5.sln" /target:rebuild /property:Configuration=Release
"%MSBuild%" "%~dp0Source/Facebook-SL4.sln" /target:rebuild /property:Configuration=Release
"%MSBuild%" "%~dp0Source/Facebook-WP7.sln" /target:rebuild /property:Configuration=Release

:END
POPD

pause
