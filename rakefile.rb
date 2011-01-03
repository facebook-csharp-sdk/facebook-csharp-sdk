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
        }
    }
    
    puts
	puts build_config if build_config[:log]
	puts
    puts "     Project Name: #{PROJECT_NAME}"
	puts "Safe Project Name: #{PROJECT_NAME_SAFE}"
    
end

Rake::Task["configure"].invoke