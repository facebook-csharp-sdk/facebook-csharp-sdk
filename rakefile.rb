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
            :dist    => "#{root_path}Dist/",
            :tools   => "#{root_path}Tools/",
            :working => "#{root_path}Working/"
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
            :net35full   => ''
        }
    }
    
    build_config[:sln][:wp7]         = "#{build_config[:paths][:src]}Facebook-WP7.sln"
    build_config[:sln][:sl4]         = "#{build_config[:paths][:src]}Facebook-SL4.sln"
    build_config[:sln][:net40client] = "#{build_config[:paths][:src]}Facebook-Net40Client.sln"
    build_config[:sln][:net40full]   = "#{build_config[:paths][:src]}Facebook-Net40.sln"
    build_config[:sln][:net35client] = "#{build_config[:paths][:src]}Facebook-Net35Client.sln"
    build_config[:sln][:net35full]   = "#{build_config[:paths][:src]}Facebook-Net35.sln"    
    
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
    
    puts
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

task :create_working_dir do
    mkdir build_config[:paths][:working] unless File.exists?(build_config[:paths][:working])
    
    nuget_dir = "#{build_config[:paths][:working]}NuGet/"
    FileUtils.rm_rf nuget_dir
    mkdir nuget_dir
    
    mkdir "#{nuget_dir}Facebook/"
    mkdir "#{nuget_dir}FacebookWeb/"
    mkdir "#{nuget_dir}FacebookWebMvc/"
end

task :prepare_facebook_nuget => [:create_working_dir] do
end

desc "Build All"
task :all => [:net35full, :net35client, :net40full, :net40client,:sl4,:wp7]

desc "Clean and Rebuild All (default)"
task :rebuild => [:clean,:all]

desc "Clean All"
task :clean => [:clean_net35full, :clean_net35client, :clean_net40full, :clean_net40client, :clean_sl4, :clean_wp7] do
    FileUtils.rm_rf build_config[:paths][:output]
    FileUtils.rm_rf build_config[:paths][:working]
end