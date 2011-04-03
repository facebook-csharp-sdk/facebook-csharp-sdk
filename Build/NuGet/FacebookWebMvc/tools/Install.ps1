param($installPath, $toolsPath, $package, $project)
	$project.Object.References.Add("System.Web.Mvc")
    $project.Object.References | Where-Object { $_.Name -eq 'Facebook.Web.Mvc.Contracts' } | ForEach-Object { $_.Remove() }