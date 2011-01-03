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
    
    build_config = {
    }
    
end

Rake::Task["configure"].invoke