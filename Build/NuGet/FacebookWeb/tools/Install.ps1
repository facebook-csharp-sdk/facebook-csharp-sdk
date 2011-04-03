param($installPath, $toolsPath, $package, $project)
	$project.Object.References.Add("System.Web")
    $project.Object.References | Where-Object { $_.Name -eq 'Facebook.Web.Contracts' } | ForEach-Object { $_.Remove() }