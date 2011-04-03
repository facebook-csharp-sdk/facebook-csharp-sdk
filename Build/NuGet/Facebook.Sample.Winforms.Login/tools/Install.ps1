param($installPath, $toolsPath, $package, $project)
    $project.Object.References.Add("System.Drawing")
	$project.Object.References.Add("System.Windows.Forms")