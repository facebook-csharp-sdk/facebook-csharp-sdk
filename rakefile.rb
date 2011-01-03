require File.join(File.dirname(__FILE__), 'Build/albacore/albacore.rb')

PROJECT_NAME      = "Facebook C# SDK"
PROJECT_NAME_SAFE = PROJECT_NAME
LOG               = true                # TODO: enable albacore logging from ENV

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
            :root   => root_path,
            :src    => "#{root_path}Source/",
            :output => "#{root_path}Bin/",
            :dist   => "#{root_path}Dist/",
            :tools  => "#{root_path}Tools/"
        },
        :version => {
			:base =>   "#{base_version}",
            :full => "#{base_version}",
            :long => "#{base_version}"
		},
        :vcs => { 				# version control system
			:name         => '',		# version control name
			:rev_id       => '',		# revision number
            :short_rev_id => ''         # short revision id
		},        
        :ci => {
			:build_number_param_name => 'BUILD_NUMBER',
            :is_nightly              => true,
            :build_number            => 0
		}
    }
    
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
	puts "        Root Path: #{build_config[:paths][:root]}"
    puts
	puts "              VCS: #{build_config[:vcs][:name]}"
	print "     Revision ID: #{build_config[:vcs][:rev_id]}"
	print "  (#{build_config[:vcs][:short_rev_id]})" if build_config[:vcs][:name] == 'git'
	puts	
	puts
    
end

Rake::Task["configure"].invoke