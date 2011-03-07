rem Run both the 32-bit and 64-bit InstallUtil
IF EXIST %SystemRoot%\Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe %SystemRoot%\Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe Microsoft.WindowsAzure.ServiceRuntime.Commands.dll
IF EXIST %SystemRoot%\Microsoft.NET\Framework64\v2.0.50727\InstallUtil.exe %SystemRoot%\Microsoft.NET\Framework64\v2.0.50727\InstallUtil.exe Microsoft.WindowsAzure.ServiceRuntime.Commands.dll

rem Add the snapin to the default profile
echo Add-PSSnapIn Microsoft.WindowsAzure.ServiceRuntime >> %SystemRoot%\system32\WindowsPowerShell\Profile.ps1

powershell -command set-executionpolicy allsigned