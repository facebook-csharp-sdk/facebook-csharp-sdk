
# http://lostechies.com/joshuaflanagan/2011/06/24/how-to-use-a-tool-installed-by-nuget-in-your-build-scripts/?utm_source=feedburner&utm_medium=feed&utm_campaign=Feed%3A+LosTechies+%28LosTechies%29
def nuget_tool(package_root, package, tool)
	File.join(Dir.glob(File.join(package_root,"#{package}.*")).sort.last, "tools", tool)
end
