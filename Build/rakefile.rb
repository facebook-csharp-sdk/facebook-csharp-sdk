root_path = File.dirname(File.dirname(__FILE__)) + "/"

require 'erb'
require 'yaml'
require File.join(root_path, 'Build/nuget.rb')
require File.join(root_path, 'Build/albacore/albacore.rb')

#ENV['nightly']    = 'false'
#ENV['nuget_api_key'] = ''
config = nil

task :configure do

	b = binding
	# read VERSION from file
	base_version = nil
	tag_version = nil
	File.open("#{root_path}VERSION",'r') do |f|
		version = f.gets.split('-')
		base_version = version[0].chomp
		tag_version = version[1] if version.length == 2
	end

	build_number = ENV['BUILD_NUMBER'] || "0"
	version_full = "#{base_version}.#{build_number}"

	# load config.yaml
	config = YAML.load(ERB.new(File.read(File.join(root_path, 'Build/config.yml'))).result(b))

	begin
        config["vcs"]["rev_id"] = `hg id -i`.chomp.chop # remove the +
        config["vcs"]["name"] = 'hg'
        config["vcs"]["short_rev_id"] = config["vcs"]["rev_id"]
    rescue
    end
	
	if(config["vcs"]["rev_id"].length == 0) then
		# if mercurial fails try git
		begin
			config["vcs"]["rev_id"]    = `git log -1 --pretty=format:%H`.chomp
			config["vcs"]["name"] = 'git'
			config["vcs"]["short_rev_id"] = config["vcs"]["rev_id"][0..7]
		rescue
		end
	end
	
    if(config["version"]["is_nightly"])
        config["version"]["long"] = "#{config["version"]["tag_full"]}-nightly-#{config["vcs"]["short_rev_id"]}"
    else
	config["version"]["long"] = "#{config["version"]["tag_full"]}-#{config["vcs"]["short_rev_id"]}"
    end
	
    puts
    puts  "     Project Name: #{config["project"]["name"]}"
    puts  "Safe Project Name: #{config["project"]["safe_name"]}"
	puts  "          Version: #{config["version"]["tag_full"]} (#{config["version"]["long"]})"
	puts  "     Base Version: #{config["version"]["base"]}"
	print "  CI Build Number: #{config["version"]["build_number"]}"
	print " (not running under CI mode)" if config["version"]["build_number"] == "0"
	puts
    puts  "        Root Path: #{config["path"]["root"]}"
    puts
	puts  "              VCS: #{config["vcs"]["name"]}"
    print "      Revision ID: #{config["vcs"]["rev_id"]}"
    print "  (#{config["vcs"]["short_rev_id"]})" if config["vcs"]["name"] == 'git'
    puts    
    puts   
	
end

Rake::Task["configure"].invoke

directory "#{config['path']['working']}"
directory "#{config['path']['working']}NuGet/"
directory "#{config['path']['working']}SymbolSource/"
directory "#{config['path']['dist']}"
directory "#{config['path']['dist']}NuGet/"
directory "#{config['path']['dist']}SymbolSource/"

namespace :build do

	desc "Build Windows RT binaries"
	msbuild :winrt => ['clean:winrt','assemblyinfo:facebook'] do |msb|
	   msb.properties :configuration => config['version']['configuration']
	   msb.solution = config['path']['sln']['winrt']
	   msb.targets :Build
	end

	desc "Build .NET 4.5 binaries"
	msbuild :net45 => ['clean:net45','assemblyinfo:facebook','assemblyinfo:facebookweb','assemblyinfo:facebookwebmvc'] do |msb|
	   msb.properties :configuration => config['version']['configuration']
	   msb.solution = config['path']['sln']['net45']
	   msb.targets :Build
	end
	
	desc "Build .NET 4 binaries"
	msbuild :net40 => ['clean:net40','assemblyinfo:facebook','assemblyinfo:facebookweb','assemblyinfo:facebookwebmvc'] do |msb|
	   msb.properties :configuration => config['version']['configuration']
	   msb.solution = config['path']['sln']['net40']
	   msb.targets :Build
	end
	
	desc "Build .NET 3.5 binaries"
	msbuild :net35 => ['clean:net35','assemblyinfo:facebook','assemblyinfo:facebookweb','assemblyinfo:facebookwebmvc'] do |msb|
	   msb.properties :configuration => config['version']['configuration']
	   msb.solution = config['path']['sln']['net35']
	   msb.targets :Build
	   #msb.use :net35
	end
	
	desc "Build Silverlight 4 binaries"
	msbuild :sl4 => ['clean:sl4','assemblyinfo:facebook'] do |msb|
	   # temporary hack for bug caused by code contracts
	   FileUtils.rm_rf "#{config["path"]["working"]}obj/Facebook/sl4/"
	   FileUtils.rm_rf "#{config["path"]["working"]}obj/Facebook.Tests/sl4/"
	   
	   msb.properties :configuration => config['version']['configuration']
	   msb.solution = config['path']['sln']['sl4']
	   msb.targets :Build
	end	
    
    desc "Build Silverlight 5 binaries"
	msbuild :sl5 => ['clean:sl5','assemblyinfo:facebook'] do |msb|
	   # temporary hack for bug caused by code contracts
	   FileUtils.rm_rf "#{config["path"]["working"]}obj/Facebook/sl5/"
	   
	   msb.properties :configuration => config['version']['configuration']
	   msb.solution = config['path']['sln']['sl5']
	   msb.targets :Build
	end
	
	desc "Build Windows Phone 7 binaries"
	msbuild :wp7 => ['clean:wp7','assemblyinfo:facebook'] do |msb|
	  msb.properties :configuration => config['version']['configuration']
	  msb.solution = config['path']['sln']['wp7']
	  msb.targets :Build
	end
	
    desc "Build documentation files"
	msbuild :docs => ['build:net40'] do |msb|
		msb.properties :configuration => config['version']['configuration']
		msb.properties :DocumentationSourcePath => "#{ config['path']['output']}Release/Net40/"
		msb.solution = config['path']['sln']['shfb']
		msb.targets [:Clean,:Rebuild]
		msb.properties
	end
	
	multitask :all => ['build:net40','build:net35','build:sl4', 'build:sl5', 'build:wp7']
	#task :all => ['build:parallel','build:sl4']
	
end

namespace :zip do
	
	zip :libs => ['build:all',"#{config['path']['dist']}"] do |zip|
		zip.directories_to_zip "#{config['path']['output']}Release/"
		zip.exclusions = [/.+xml.old/]
		zip.output_file = "#{config['project']['safe_name']}-#{config['version']['long']}.bin.zip"
		zip.output_path = "#{config['path']['dist']}"
		zip.additional_files = [ 
									"#{config['path']['root']}LICENSE.txt",
									"#{config['path']['root']}VERSION",
									"#{config['path']['root']}CHANGES.txt"
							   ]
	end
	
	desc "Create zip archive of the source files"
	task :source => ["#{config['path']['dist']}"] do
		src_archive_name = "#{config['path']['dist']}#{config['project']['safe_name']}-#{config['version']['long']}.src.zip"
		if (config["vcs"]["name"] == 'git') then
			sh "git archive HEAD --format=zip > \"#{src_archive_name}\""
		elsif (config["vcs"]["name"] == 'hg') then
			sh "hg archive -tzip \"#{src_archive_name}\" -p \"#{config['project']['safe_name']}\""
		end
	end
	
	multitask :all => ['zip:libs','zip:source']
	
end

desc "Build All"
task :build => ['build:all']

namespace :clean do

	msbuild :winrt do |msb|
	   msb.properties :configuration => config["version"]["configuration"]
	   msb.solution = config["path"]["sln"]["winrt"]
	   msb.targets :Clean
	end

	msbuild :net45 do |msb|
	   msb.properties :configuration => config["version"]["configuration"]
	   msb.solution = config["path"]["sln"]["net45"]
	   msb.targets :Clean
	end

	msbuild :net40 do |msb|
	   msb.properties :configuration => config["version"]["configuration"]
	   msb.solution = config["path"]["sln"]["net40"]
	   msb.targets :Clean
	end
	
	# compile .net 3.5 libraries using msbuild 4.0 in order to generate the code contract libraries.
	# seems like if we use .net 3.5, it does not generate the code contracts.
	msbuild :net35 do |msb|
	   msb.properties :configuration => config['version']['configuration']
	   msb.solution = config['path']['sln']['net35']
	   msb.targets :Clean
	   #msb.use :net35
	end
	
	msbuild :sl4 do |msb|
	   msb.properties :configuration => config['version']['configuration']
	   msb.solution = config['path']['sln']['sl4']
	   msb.targets :Clean    
	end
    
    msbuild :sl5 do |msb|
	   msb.properties :configuration => config['version']['configuration']
	   msb.solution = config['path']['sln']['sl5']
	   msb.targets :Clean    
	end
	
	msbuild :wp7 do |msb|
		msb.properties :configuration => config['version']['configuration']
		msb.solution = config['path']['sln']['wp7']
		msb.use :net40
		msb.targets :Clean
	end
	
	multitask :libs => ['clean:net35','clean:net40','clean:sl4','clean:sl5', 'clean:wp7']
	
	multitask :all => ['clean:libs'] do
		FileUtils.rm_rf config["path"]["output"]
		FileUtils.rm_rf config["path"]["working"]
		FileUtils.rm_rf config["path"]["dist"]
	end
	
end

desc "Clean All"
task :clean => ['clean:all']

namespace :tests do

	namespace :net40 do
	
		xunit :facebook => ['build:net40'] do |xunit|
			output_path = "#{config["path"]["output"]}Tests/Release/"
			xunit.command = config["path"]["xunit"]["console"]["x86"]
			xunit.assembly = "#{output_path}Facebook.Tests.dll"
			xunit.html_output = "#{output_path}"
			xunit.options = '/nunit ' + output_path + 'Facebook.Tests.nUnit.xml', '/xml ' + output_path + 'Facebook.Tests.xUnit.xml'
		end
		
		xunit :facebookweb => ['build:net40'] do |xunit|
			output_path = "#{config["path"]["output"]}Tests/Release/"
			xunit.command = config["path"]["xunit"]["console"]["x86"]
			xunit.assembly = "#{output_path}Facebook.Web.Tests.dll"
			xunit.html_output = "#{output_path}"
			xunit.options = '/nunit ' + output_path + 'Facebook.Web.Tests.nUnit.xml', '/xml ' + output_path + 'Facebook.Web.Tests.xUnit.xml'
		end
		
		multitask :all => ['tests:net40:facebook','tests:net40:facebookweb']
	
	end

	namespace :sl4 do

		exec :facebook => ['build:sl4'] do |cmd|
			cmd.command = config["path"]["stat_light"]
			cmd.parameters = "-x\"#{config["path"]["output"]}Tests/sl4/Release/Facebook.Tests-SL4.xap\""
		end

		multitask :all => ['tests:sl4:facebook']

	end
	
	multitask :net40 => ['tests:net40:all']

	multitask :sl4 => ['tests:sl4:all']
	
	multitask :all => ['tests:net40', 'tests:sl4']
end

desc "Run tests"
multitask :tests => ['tests:all']

desc "Build All Libraries and run tests (default)"
multitask :libs => ['clean:libs','build','tests']

task :default => ['libs']

namespace :assemblyinfo do

	assemblyinfo :facebook do |asm|
		asm.output_file = "#{config["path"]["src"]}Facebook/Properties/AssemblyInfo.cs"
		asm.version = config["version"]["full"]
		asm.title = "Facebook"
		asm.description = "Facebook C\# SDK"
		asm.product_name = "Facebook C\# SDK"
		asm.company_name = "Thuzi"
		asm.copyright = "Microsoft Public License (Ms-PL)"
		asm.com_visible = false
	end
	
	assemblyinfo :facebookweb do |asm|
		asm.output_file = "#{config["path"]["src"]}Facebook.Web/Properties/AssemblyInfo.cs"
		asm.version = config["version"]["full"]
		asm.title = "Facebook.Web"
		asm.description = "Facebook C\# SDK"
		asm.product_name = "Facebook C\# SDK"
		asm.company_name = "Thuzi"
		asm.copyright = "Microsoft Public License (Ms-PL)"
		asm.com_visible = false
	end
	
	assemblyinfo :facebookwebmvc do |asm|
		asm.output_file = "#{config["path"]["src"]}Facebook.Web.Mvc/Properties/AssemblyInfo.cs"
		asm.version = config["version"]["full"]
		asm.title = "Facebook.Web.Mvc"
		asm.description = "Facebook C\# SDK"
		asm.product_name = "Facebook C\# SDK"
		asm.company_name = "Thuzi"
		asm.copyright = "Microsoft Public License (Ms-PL)"
		asm.com_visible = false
	end
	
	multitask :all => ["assemblyinfo:facebook","assemblyinfo:facebookweb","assemblyinfo:facebookwebmvc"]
	
end

task :assemblyinfo => ['assemblyinfo:all']

namespace :nuget do

	task :nuspec => ["#{config['path']['working']}","#{config['path']['working']}NuGet/","#{config['path']['working']}SymbolSource/"] do

		FileList.new("#{config["path"]["build"]}*/*/*.nuspec").each do |path|
			outfile = "#{config['path']['working']}" +
                 (path.start_with?("#{config['path']['build']}NuGet") ? "NuGet/" : "SymbolSource/") +
				 File.basename(path)
			File.open("#{outfile}","w") do |f|
				b = binding
				version_full = config['version']['tag_full']
				f.write(ERB.new(File.read(path)).result(b))
				f.close()
				puts "Created #{outfile}"
			end
		end
	
	end
	
	multitask :pack => ['nuget:nuspec','libs',"#{config['path']['dist']}","#{config['path']['dist']}NuGet/","#{config['path']['dist']}SymbolSource/"] do
	
		FileList.new("#{config["path"]["working"]}*/*.nuspec").each do |path|
			nugetpack 'nuget:pack' do |nuget|
				nuget.command = "#{config['path']['nuget']}"
				nuget.nuspec  = path            
				nuget.output = "#{config['path']['dist']}" +
				(path.start_with?("#{config['path']['working']}NuGet") ? "NuGet/" : "SymbolSource/")
			end
		end
	
	end

	desc "Push .nupkg to symbol source & publish"
	task :push_source do		
		FileList.new("#{config["path"]["build"]}SymbolSource/*").each do |path|
			nugetpush ['nuget:push_source'] do |nuget|
				nuget.command = "#{config['path']['nuget']}"
				nuget.package = "#{config['path']['dist']}SymbolSource/#{File.basename(path)}.#{config['version']['tag_full']}.nupkg"
				nuget.apikey = ENV['nuget_api_key']
				nuget.source = "http://nuget.gw.symbolsource.org/Public/NuGet"
			end
		end
	end
	
	desc "Push .nupkg to nuget.org but don't publish"
	task :push do
		FileList.new("#{config["path"]["build"]}NuGet/*").each do |path|
			nugetpush ['nuget:push'] do |nuget|
				nuget.command = "#{config['path']['nuget']}"
				nuget.package = "#{config['path']['dist']}NuGet/#{File.basename(path)}.#{config['version']['tag_full']}.nupkg"
				nuget.apikey = ENV['nuget_api_key']
				nuget.create_only = true
			end
		end
	end
	
	desc "Publish .nupkg to nuget.org live feed"
	task :publish do
		FileList.new("#{config["path"]["build"]}NuGet/*").each do |path|
			nugetpublish ['nuget:publish'] do |nuget|
				nuget.command = "#{config['path']['nuget']}"
				nuget.id = "#{File.basename(path)}"
				nuget.version = "#{config['version']['tag_full']}"
				nuget.apikey = ENV['nuget_api_key']
			end
		end
	end
	
end

desc "Build NuGet packages"
task :nuget => ['nuget:pack']

desc "Create distribution packages"
multitask :dist => ['zip:all','nuget','build:docs']
