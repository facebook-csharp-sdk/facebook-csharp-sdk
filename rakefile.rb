require File.join(File.dirname(__FILE__), 'Build/albacore/albacore.rb')

task :default => [:rebuild]

PROJECT_NAME      = "Facebook C# SDK"
PROJECT_NAME_SAFE = PROJECT_NAME
LOG               = false                # TODO: enable albacore logging from ENV

build_config = nil

task :configure do
    # do configuration stuffs here
    Albacore.configure do |config|
        config.log_level = :verbose if LOG
    end
    
    root_path    = "#{File.dirname(__FILE__)}/"
    base_version = 0
    
    File.open("#{root_path}VERSION",'r') do |f|
        base_version = f.gets.chomp
    end
    
    build_config = {
        :log    => LOG,
        :paths  => {
            :root    => root_path,
            :src     => "#{root_path}Source/",
            :output  => "#{root_path}Bin/",
            :build   => "#{root_path}Build/",
            :dist    => "#{root_path}Dist/",
            :tools   => "#{root_path}Tools/",
            :working => "#{root_path}Working/",
            :doc     => "#{root_path}Doc/"
        },
        :version => {
			:base => "#{base_version}",
            :full => "#{base_version}",
            :long => "#{base_version}"
		},
        :vcs => { # version control system
			:name         => '',		# version control name
			:rev_id       => '',		# revision number
            :short_rev_id => ''         # short revision id
		},        
        :ci => {
			:build_number_param_name => 'BUILD_NUMBER',
            :is_nightly              => true,
            :build_number            => 0
		},
        :configuration => :Release,
        :sln => {
            :wp7         => '',
            :sl4         => '',
            :net40client => '',
            :net40full   => '',
            :net35client => '',
            :net35full   => '',
            :shfb        => '', # sandcastle help file builder doc project
        }
    }
    
    build_config[:sln][:wp7]         = "#{build_config[:paths][:src]}Facebook-WP7.sln"
    build_config[:sln][:sl4]         = "#{build_config[:paths][:src]}Facebook-SL4.sln"
    build_config[:sln][:net40client] = "#{build_config[:paths][:src]}Facebook-Net40Client.sln"
    build_config[:sln][:net40full]   = "#{build_config[:paths][:src]}Facebook-Net40.sln"
    build_config[:sln][:net35client] = "#{build_config[:paths][:src]}Facebook-Net35Client.sln"
    build_config[:sln][:net35full]   = "#{build_config[:paths][:src]}Facebook-Net35.sln"
    build_config[:sln][:shfb]        = "#{build_config[:paths][:doc]}doc.shfbproj"
    
    begin
        # TODO: support mercurial and svn
		build_config[:vcs][:rev_id]	= `git log -1 --pretty=format:%H`.chomp
		build_config[:vcs][:name] = 'git'
        build_config[:vcs][:short_rev_id] = build_config[:vcs][:rev_id][0..7]
	rescue
	end
    
    build_config[:ci][:is_nightly]   = ENV['NIGHTLY'].nil? ? true : Boolean(ENV['NIGHTLY'].downcase)
    build_config[:ci][:build_number] = ENV[build_config[:ci][:build_number_param_name]] || 0
    
    build_config[:version][:full] = "#{build_config[:version][:base]}.#{build_config[:ci][:build_number]}"
    
    if(build_config[:ci][:is_nightly])
        build_config[:version][:long] = "#{build_config[:version][:full]}-nightly-#{build_config[:vcs][:short_rev_id]}"
    else
        build_config[:version][:long] = "#{build_config[:version][:full]}-#{build_config[:version][:short_rev_id]}"        
    end
    
	puts build_config if build_config[:log]
	puts
    puts "     Project Name: #{PROJECT_NAME}"
	puts "Safe Project Name: #{PROJECT_NAME_SAFE}"
    puts "          Version: #{build_config[:version][:full]} (#{build_config[:version][:long]})"
    puts "     Base Version: #{build_config[:version][:base]}"
    print "  CI Build Number: #{build_config[:ci][:build_number]}"
    print " (not running under CI mode)" if build_config[:ci][:build_number] == 0
    puts
	puts "        Root Path: #{build_config[:paths][:root]}"
    puts
	puts "              VCS: #{build_config[:vcs][:name]}"
	print "     Revision ID: #{build_config[:vcs][:rev_id]}"
	print "  (#{build_config[:vcs][:short_rev_id]})" if build_config[:vcs][:name] == 'git'
	puts	
	puts
    
end

Rake::Task["configure"].invoke

msbuild :net40full do |msb|
    msb.properties :configuration => build_config[:configuration]
    msb.solution = build_config[:sln][:net40full]
    msb.targets :Build
end

msbuild :clean_net40full do |msb|
    msb.properties :configuration => build_config[:configuration]
    msb.solution = build_config[:sln][:net40full]
    msb.targets :Clean
end

msbuild :net40client do |msb|
    msb.properties :configuration => build_config[:configuration]
    msb.solution = build_config[:sln][:net40client]
    msb.targets :Build
end

msbuild :clean_net40client do |msb|
    msb.properties :configuration => build_config[:configuration]
    msb.solution = build_config[:sln][:net40client]
    msb.targets :Clean
end

desc "Build .NET 4 binaries (client and full profile)"
task :net40 => [:net40full, :net40client]

task :clean_net40 => [:clean_net40full, :clean_net40client]

msbuild :net35full do |msb|
    msb.properties :configuration => build_config[:configuration]
    msb.solution = build_config[:sln][:net35full]
    msb.targets :Build
    msb.use :net35
end

msbuild :clean_net35full do |msb|
    msb.properties :configuration => build_config[:configuration]
    msb.solution = build_config[:sln][:net35full]
    msb.targets :Clean
    msb.use :net35
end

msbuild :net35client do |msb|
    msb.properties :configuration => build_config[:configuration]
    msb.solution = build_config[:sln][:net35client]
    msb.targets :Build
    msb.use :net35
end

msbuild :clean_net35client do |msb|
    msb.properties :configuration => build_config[:configuration]
    msb.solution = build_config[:sln][:net35client]
    msb.targets :Clean
    msb.use :net35
end

desc "Build .NET 3.5 binaries (client and full profile)"
task :net35 => [:net35full, :net35client]

task :clean_net35 => [:clean_net35full, :clean_net35client]

desc "Build Silverlight 4 binaries"
msbuild :sl4 do |msb|
    msb.properties :configuration => build_config[:configuration]
    msb.solution = build_config[:sln][:sl4]
    msb.targets :Build
end

msbuild :clean_sl4 do |msb|
    msb.properties :configuration => build_config[:configuration]
    msb.solution = build_config[:sln][:sl4]
    msb.targets :Clean
end

desc "Build Windows Phone 7 binaries"
msbuild :wp7 do |msb|
   msb.properties :configuration => build_config[:configuration]
   msb.solution = build_config[:sln][:wp7]
   msb.use :net40
   msb.targets :Build
end

msbuild :clean_wp7 do |msb|
   msb.properties :configuration => build_config[:configuration]
   msb.solution = build_config[:sln][:wp7]
   msb.use :net40
   msb.targets :Clean
end

directory "#{build_config[:paths][:working]}"
directory "#{build_config[:paths][:working]}NuGet/Facebook"
directory "#{build_config[:paths][:working]}NuGet/FacebookWeb"

desc "Create NuGet package for Facebook.dll"
exec :nuget_facebook => [:net35full, :net35client, :net40full, :net40client,:sl4,:wp7,"#{build_config[:paths][:working]}NuGet/Facebook"] do |cmd|
working_dir = build_config[:paths][:working]
    nuget_working_dir = "#{working_dir}NuGet/Facebook/"
    
    FileUtils.rm_rf "#{nuget_working_dir}lib/"
    mkdir "#{nuget_working_dir}lib/"
    
    nuget_dirs = [ "lib/Net35/",
                   "lib/Net35Client/",
                   "lib/Net40/",
                   "lib/Net40Client/",
                   "lib/SL4/",
                   "lib/WP7/" ]
        
    nuget_dirs.each do |d|
        mkdir "#{nuget_working_dir + d}"
        mkdir "#{nuget_working_dir + d}CodeContracts/"
    end
    
    output_path = "#{build_config[:paths][:output]}Release/" if build_config[:configuration] == :Release
    output_path = "#{build_config[:paths][:output]}Debug/"   if build_config[:configuration] == :Debug
    
    [ "Facebook.dll", "Facebook.pdb", "Facebook.XML" ].each do |f|
        # copy these 3 files of each different framework
        cp "#{output_path}Net35/#{f}", "#{nuget_working_dir}lib/Net35/"
        cp "#{output_path}Net35Client/#{f}", "#{nuget_working_dir}lib/Net35Client/"
        cp "#{output_path}Net40/#{f}", "#{nuget_working_dir}lib/Net40/"
        cp "#{output_path}Net40Client/#{f}", "#{nuget_working_dir}lib/Net40Client/"
        cp "#{output_path}SL4/#{f}", "#{nuget_working_dir}lib/SL4/"
        cp "#{output_path}WP7/#{f}", "#{nuget_working_dir}lib/WP7/"
    end
    
    # temporarily copy Json.Net for SL and WP7
    cp "#{output_path}SL4/Newtonsoft.Json.Silverlight.dll", "#{nuget_working_dir}lib/SL4/"
    cp "#{output_path}SL4/Newtonsoft.Json.Silverlight.pdb", "#{nuget_working_dir}lib/SL4/"
    cp "#{output_path}SL4/Newtonsoft.Json.Silverlight.xml", "#{nuget_working_dir}lib/SL4/"
    cp "#{output_path}WP7/Newtonsoft.Json.WindowsPhone.dll", "#{nuget_working_dir}lib/WP7/"
    cp "#{output_path}WP7/Newtonsoft.Json.WindowsPhone.pdb", "#{nuget_working_dir}lib/WP7/"
    cp "#{output_path}WP7/Newtonsoft.Json.WindowsPhone.xml", "#{nuget_working_dir}lib/WP7/"
    
    [ "Facebook.Contracts.dll", "Facebook.Contracts.pdb" ].each do |f|
        # copy code contracts of each different framework
        # TODO .net 3.5 code contracts
        cp "#{output_path}Net40/CodeContracts/#{f}", "#{nuget_working_dir}lib/Net40/CodeContracts/"
        cp "#{output_path}Net40Client/CodeContracts/#{f}", "#{nuget_working_dir}lib/Net40Client/CodeContracts/"
        cp "#{output_path}SL4/CodeContracts/#{f}", "#{nuget_working_dir}lib/SL4/CodeContracts/"
        cp "#{output_path}WP7/CodeContracts/#{f}", "#{nuget_working_dir}lib/WP7/CodeContracts/"
    end
    
    version = build_config[:version][:full] #[:full]
    File.open("#{nuget_working_dir}Facebook.nuspec",'w+') do |f|
        f.puts File.read("#{build_config[:paths][:build]}Facebook.nuspec").gsub(/{version}/,version)
    end
    
    cmd.command = "#{build_config[:paths][:tools]}/NuGet/NuGet.exe"
    cmd.parameters = "pack \"#{build_config[:paths][:working]}NuGet/Facebook/Facebook.nuspec\" -o \"#{build_config[:paths][:working]}NuGet\""
end

desc "Create NuGet package for Facebook.Web.dll" #:net35full,:net40full,
exec :nuget_facebookweb => ["#{build_config[:paths][:working]}NuGet/FacebookWeb"] do |cmd|
    working_dir = build_config[:paths][:working]
    nuget_working_dir = "#{working_dir}NuGet/FacebookWeb/"
    
    FileUtils.rm_rf "#{nuget_working_dir}lib/"
    mkdir "#{nuget_working_dir}lib/"
    
    nuget_dirs = [ "lib/Net35/",
                   "lib/Net40/" ]
                   
    nuget_dirs.each do |d|
        mkdir "#{nuget_working_dir + d}"
        mkdir "#{nuget_working_dir + d}CodeContracts/"
    end
    
     output_path = "#{build_config[:paths][:output]}Release/" if build_config[:configuration] == :Release
     #output_path = "#{build_config[:paths][:output]}Debug/"   if build_config[:configuration] == :Debug
                   
    [ "Facebook.Web.dll", "Facebook.Web.pdb", "Facebook.Web.XML" ].each do |f|
        # copy these 3 files of each different framework
        cp "#{output_path}Net35/#{f}", "#{nuget_working_dir}lib/Net35/"
        cp "#{output_path}Net40/#{f}", "#{nuget_working_dir}lib/Net40/"
    end
    
    [ "Facebook.Web.Contracts.dll", "Facebook.Web.Contracts.pdb" ].each do |f|
        # copy code contracts of each different framework
        # TODO .net 3.5 code contracts
        cp "#{output_path}Net40/CodeContracts/#{f}", "#{nuget_working_dir}lib/Net40/CodeContracts/"
    end
    
    version = build_config[:version][:full] #[:full]
    File.open("#{nuget_working_dir}FacebookWeb.nuspec",'w+') do |f|
        f.puts File.read("#{build_config[:paths][:build]}FacebookWeb.nuspec").gsub(/{version}/,version)
    end
    
    cmd.command = "#{build_config[:paths][:tools]}/NuGet/NuGet.exe"
    cmd.parameters = "pack \"#{build_config[:paths][:working]}NuGet/FacebookWeb/FacebookWeb.nuspec\" -o \"#{build_config[:paths][:working]}NuGet\""
    
end

desc "Build help documentation"
msbuild :docs => [:net40full] do |msb|
    msb.properties :configuration => build_config[:configuration]
    msb.properties :DocumentationSourcePath => "#{build_config[:paths][:output]}Release/Net40/" if build_config[:configuration] = :Release
    msb.properties :DocumentationSourcePath => "#{build_config[:paths][:output]}Debug/Net40/" if build_config[:configuration] = :Debug                   
    msb.solution = build_config[:sln][:shfb]
    msb.targets [:Clean,:Rebuild]
    msb.properties
end

msbuild :clean_docs do |msb|
    msb.properties :configuration => build_config[:configuration]
    msb.properties :DocumentationSourcePath => "#{build_config[:paths][:output]}Release/Net40/" if build_config[:configuration] = :Release
    msb.properties :DocumentationSourcePath => "#{build_config[:paths][:output]}Debug/Net40/" if build_config[:configuration] = :Debug                   
    msb.solution = build_config[:sln][:shfb]
    msb.targets [:Clean]
    msb.properties
end

desc "Build All"
task :all => [:net35full, :net35client, :net40full, :net40client,:sl4,:wp7,:nuget_facebook,:nuget_facebookweb]

desc "Clean and Rebuild All (default)"
task :rebuild => [:clean,:all]

desc "Clean All"
task :clean => [:clean_net35full, :clean_net35client, :clean_net40full, :clean_net40client, :clean_sl4, :clean_wp7] do
    FileUtils.rm_rf build_config[:paths][:output]
    FileUtils.rm_rf build_config[:paths][:working]
end