param($installPath, $toolsPath, $package, $project)
    $project.Object.References | Where-Object { $_.Name -eq 'Facebook.Contracts' } | ForEach-Object { $_.Remove() }