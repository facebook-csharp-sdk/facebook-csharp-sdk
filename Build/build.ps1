properties { 
  $version = '4.1.0'
  $zipFileName = "FacebookSDK_V$version.zip"
  $buildPackage = $true
  
  $baseDir  = resolve-path ..
  $buildDir = "$baseDir\Build"
  $sourceDir = "$baseDir\Source"
  $toolsDir = "$baseDir\Tools"
  $docDir = "$baseDir\Doc"
  $workingDir = "$baseDir\Working"
  $builds = @(
    @{Name = "Facebook-Net40"; TestsName = $null; Constants=""; FinalDir="Net40"; Framework="net-4.0"}
    @{Name = "Facebook-Net40Client"; TestsName = $null; Constants="CLIENTPROFILE"; FinalDir="Net40Client"; Framework="net-4.0"}
    @{Name = "Facebook-Net35"; TestsName = $null; Constants="NET35"; FinalDir="Net35"; Framework="net-2.0"}
    @{Name = "Facebook-Net35Client"; TestsName = $null; Constants="NET35;CLIENTPROFILE"; FinalDir="Net35Client"; Framework="net-2.0"}
    @{Name = "Facebook-SL4"; TestsName = $null; Constants="SILVERLIGHT"; FinalDir="SL4"; Framework="net-4.0"}
    @{Name = "Facebook-WP7"; TestsName = $null; Constants="SILVERLIGHT;WINDOWS_PHONE"; FinalDir="WP7"; Framework="net-4.0"}
  )
}

$framework = '4.0x86'

task default -depends Test

# Ensure a clean working directory
task Clean {
  Set-Location $baseDir
  
  if (Test-Path -path $workingDir)
  {
    Write-Output "Deleting Working Directory"
    
    del $workingDir -Recurse -Force
  }
  
  New-Item -Path $workingDir -ItemType Directory
}

# Build each solution, optionally signed
task Build -depends Clean { 
  
  foreach ($build in $builds)
  {
    $name = $build.Name
    $finalDir = $build.FinalDir

    Write-Host -ForegroundColor Green "Building " $name
    Write-Host
    exec { msbuild "/t:Clean;Rebuild" /p:Configuration=Release (GetConstants $build.Constants $signAssemblies) ".\Source\$name.sln" } "Error building $name"
  }
}

# Create Merged Versions of the builds that use the DLR
 task Merge -depends Build {
  #$binaryDir = "$sourceDir\Facebook\bin\Release\DotNet35"  
  #MergeAssembly "$binaryDir\Facebook.dll" "v2" $false $signKeyPath @("""$binaryDir\Microsoft.Dynamic.dll""", """$binaryDir\Microsoft.Scripting.dll""", """$binaryDir\Microsoft.Scripting.Core.dll""", """$binaryDir\Microsoft.Scripting.ExtensionAttribute.dll""")

  #$binaryDir = "$sourceDir\Newtonsoft.Json.Tests\bin\Release\DotNet20"
  #MergeAssembly "$binaryDir\Newtonsoft.Json.Tests.Net20.dll" $signKeyPath "$binaryDir\LinqBridge.dll"
  #MergeAssembly "$binaryDir\Newtonsoft.Json.Net20.dll" $signKeyPath "$binaryDir\LinqBridge.dll"
  #del $binaryDir\LinqBridge.dll
 }

# Optional build documentation, add files to final zip
task Package -depends Merge {
   if($buildPackage) {
        foreach ($build in $builds)
        {
            $name = $build.TestsName
            $finalDir = $build.FinalDir

            robocopy $baseDir\bin\Release\$finalDir $workingDir\Package\Bin\$finalDir /S /NP /XF *.sdf *.old
        }
          
        exec { msbuild "/t:Clean;Rebuild" /p:Configuration=Release "/p:DocumentationSourcePath=$workingDir\Package\Bin\DotNet40" $docDir\doc.shfbproj } "Error building documentation. Check that you have Sandcastle, Sandcastle Help File Builder and HTML Help Workshop installed."

        move -Path $workingDir\Documentation\Documentation.chm -Destination $workingDir\Package\Documentation.chm
        move -Path $workingDir\Documentation\LastBuild.log -Destination $workingDir\Documentation.log

        New-Item -Path $workingDir\NuPack\Facebook\$version\ -ItemType Directory
        New-Item -Path $workingDir\NuPack\FacebookWeb\$version\ -ItemType Directory
        New-Item -Path $workingDir\NuPack\FacebookWebMvc\$version\ -ItemType Directory

        Copy-Item -Path "$buildDir\Facebook.nuspec" -Destination $workingDir\NuPack\Facebook\$version\Facebook.nuspec -recurse
        (Get-Content $workingDir\NuPack\Facebook\$version\Facebook.nuspec) | 
        Foreach-Object {$_ -replace "{version}", $version} | 
        Set-Content $workingDir\NuPack\Facebook\$version\Facebook.nuspec

        Copy-Item -Path "$buildDir\FacebookWeb.nuspec" -Destination $workingDir\NuPack\FacebookWeb\$version\FacebookWeb.nuspec -recurse
        (Get-Content $workingDir\NuPack\FacebookWeb\$version\FacebookWeb.nuspec) | 
        Foreach-Object {$_ -replace "{version}", $version} | 
        Set-Content $workingDir\NuPack\FacebookWeb\$version\FacebookWeb.nuspec


        Copy-Item -Path "$buildDir\FacebookWebMvc.nuspec" -Destination $workingDir\NuPack\FacebookWebMvc\$version\FacebookWebMvc.nuspec -recurse
        (Get-Content $workingDir\NuPack\FacebookWebMvc\$version\FacebookWebMvc.nuspec) | 
        Foreach-Object {$_ -replace "{version}", $version} | 
        Set-Content $workingDir\NuPack\FacebookWebMvc\$version\FacebookWebMvc.nuspec


        foreach ($build in $builds)
        {
            $name = $build.TestsName
            $finalDir = $build.FinalDir

            Copy-Item -Path "$baseDir\bin\Release\$finalDir" -Destination $workingDir\NuPack\Facebook\$version\lib\$finalDir -recurse
            get-childitem $workingDir\NuPack\Facebook\$version\lib\$finalDir\*.* -include *.old,*.sdf -recurse | remove-item

            if (Test-Path -Path "$baseDir\bin\Release\$finalDir") {
                Copy-Item -Path "$baseDir\bin\Release\$finalDir" -Destination $workingDir\NuPack\FacebookWeb\$version\lib\$finalDir -recurse
                get-childitem $workingDir\NuPack\FacebookWeb\$version\lib\$finalDir\*.* -include *.old,*.sdf -recurse | remove-item
            }

            if (Test-Path -Path "$sourceDir\bin\Release\$finalDir") {
                Copy-Item -Path "$sourceDir\bin\Release\$finalDir" -Destination $workingDir\NuPack\FacebookWebMvc\$version\lib\$finalDir -recurse
                get-childitem $workingDir\NuPack\FacebookWebMvc\$version\lib\$finalDir\*.* -include *.old,*.sdf -recurse | remove-item
            }
        }
          
        exec { .\Tools\NuPack\NuPack.exe $workingDir\NuPack\Facebook\$version\Facebook.nuspec }
        exec { .\Tools\NuPack\NuPack.exe $workingDir\NuPack\FacebookWeb\$version\FacebookWeb.nuspec }
        exec { .\Tools\NuPack\NuPack.exe $workingDir\NuPack\FacebookWebMvc\$version\FacebookWebMvc.nuspec }
        move -Path .\*.nupkg -Destination $workingDir\NuPack

        Copy-Item -Path $docDir\readme.txt -Destination $workingDir\Package\
        #Copy-Item -Path $docDir\versions.txt -Destination $workingDir\Package\Bin\

        robocopy $sourceDir $workingDir\Package\Source\Source /MIR /NP /XD .svn bin obj TestResults /XF *.suo *.user
        robocopy $buildDir $workingDir\Package\Source\Build /MIR /NP /XD .svn
        robocopy $docDir $workingDir\Package\Source\Doc /MIR /NP /XD .svn
        robocopy $toolsDir $workingDir\Package\Source\Tools /MIR /NP /XD .svn
          
        exec { .\Tools\7-zip\7za.exe a -tzip $workingDir\$zipFileName $workingDir\Package\* } "Error zipping"
  }
}

# Run tests on deployed files
task Test -depends Package {
  #foreach ($build in $builds)
  #{
  #  $name = $build.TestsName
  #  if ($name -ne $null)
  #  {
  #      $finalDir = $build.FinalDir
  #      $framework = $build.Framework
  #      
  #      Write-Host -ForegroundColor Green "Copying test assembly $name to deployed directory"
  #      Write-Host
  #      robocopy ".\Source\Newtonsoft.Json.Tests\bin\Release\$finalDir" $workingDir\Deployed\Bin\$finalDir /NP /XO /XF LinqBridge.dll
  #      
  #      Copy-Item -Path ".\Source\Newtonsoft.Json.Tests\bin\Release\$finalDir\$name.dll" -Destination $workingDir\Deployed\Bin\$finalDir\

  #      Write-Host -ForegroundColor Green "Running tests " $name
  #      Write-Host
  #      exec { .\Tools\NUnit\nunit-console.exe "$workingDir\Deployed\Bin\$finalDir\$name.dll" /framework=$framework /xml:$workingDir\$name.xml } "Error running $name tests"
  #  }
  #}
}

function MergeAssembly($dllPrimaryAssembly, $targetPlatform, $internalize, $signKey, [string[]]$mergedAssemlies)
{
  $mergeAssemblyPaths = [String]::Join(" ", $mergedAssemlies)
  
  $primary = Get-Item $dllPrimaryAssembly
  $mergedAssemblyName = $primary.Name
  $mergedDir = $primary.DirectoryName + "\Merged"
  
  Remove-Item $mergedDir -recurse
  New-Item $mergedDir -ItemType Directory
  
  $ilMergeKeyFile = switch($signAssemblies) { $true { "/keyfile:$signKeyPath" } default { "" } }
  $internalizeSwitch = switch($internalize) { $true { "/internalize" } default { "" } }
  
  exec { .\Tools\ILMerge\ilmerge.exe "/targetplatform:$targetPlatform" $internalizeSwitch "/closed" "/log:$workingDir\$mergedAssemblyName.MergeLog.txt" $ilMergeKeyFile "/out:$mergedDir\$mergedAssemblyName" $dllPrimaryAssembly $mergeAssemblyPaths } "Error executing ILMerge"
}

function GetConstants($constants, $includeSigned)
{
  $signed = switch($includeSigned) { $true { ";SIGNED" } default { "" } }

  return "/p:DefineConstants=`"TRACE;$constants$signed`""
}