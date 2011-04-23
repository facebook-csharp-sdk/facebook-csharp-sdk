require File.join(File.dirname(__FILE__), 'Build/albacore/albacore.rb')
require File.join(File.dirname(__FILE__), 'Build/albacore/dacopier.rb')

require 'find'
require 'fileutils'

task :default => [:libs]

PROJECT_NAME      = "Facebook C# SDK"
PROJECT_NAME_SAFE = "FacebookSDK"
LOG               = false                # TODO: enable albacore logging from ENV
ENV['NIGHTLY']    = 'false'

build_config = nil
nuspec_config = nil

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
            :doc     => "#{root_path}Doc/",
            :packages=> '',
            :nuget   => '',
            :xunit   => {
                :x86_console_path => ''
            }
       },
       :version => {
            :base => "#{base_version}",
            :full => "#{base_version}",
            :long => "#{base_version}"
        },
       :vcs => { # version control system
            :name         => '',        # version control name
            :rev_id       => '',        # revision number
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
            :net40         => '',
            :net35         => '',
            :shfb        => '', # sandcastle help file builder doc project
       },
	   :nuspec => {
            :authors                 => "Jim Zimmerman, Nathan Totten, Prabir Shrestha",
       }
   }
   
   nuspec_config = {
        "Facebook" => {
            :description => "The Facebook C# SDK core.",
            :dependencies => [
               { :id => "CodeContracts.Unofficial", :version => "1.0.0.2" }
            ]
        },
        "FacebookWeb" => {
            :description => "The Facebook C# SDK web component.",
            :dependencies => [
                { :id => "Facebook", :version => "#{build_config[:version][:full]}" }
            ]
        },
        "FacebookWebMvc" => {
            :description => "The Facebook C# SDK MVC component.",
            :dependencies => [ 
                { :id => "FacebookWeb", :version => "#{build_config[:version][:full]}" }
            ]
        },
        "Facebook.JavascriptMvcWebsite" => {
            :description => "Facebook.JavascriptMvcWebsite"
        },
        "Facebook.Sample" => {
            :description => "This package contains samples that demonstrate the use of the Facebook library.",
            :dependencies => [ 
                { :id => "Facebook", :version => "#{build_config[:version][:full]}" }
            ]
        },
        "Facebook.Sample.Dynamic" => {
            :description => "This package contains samples that demonstrate the use of the Facebook library using dynamic.",
            :dependencies => [ 
                { :id => "Facebook", :version => "#{build_config[:version][:full]}" }
            ]
        },
        "Facebook.Sample.Winforms.Login" => {
            :description => "This package contains samples that demonstrate the use of the Facebook Login in WinForms.",
            :dependencies => [ 
                { :id => "Facebook", :version => "#{build_config[:version][:full]}" }
            ]
        }
    }
   
   build_config[:paths][:packages]  = "#{build_config[:paths][:src]}packages/"
   build_config[:paths][:nuget]  = "#{build_config[:paths][:packages]}NuGet.CommandLine.1.2.20325.10/Tools/NuGet.exe"
   
   build_config[:paths][:xunit][:x86_console_path]  = "#{build_config[:paths][:tools]}xunit-1.7/xunit.console.clr4.exe"
   
   build_config[:sln][:wp7]         = "#{build_config[:paths][:src]}Facebook-WP7.sln"
   build_config[:sln][:sl4]         = "#{build_config[:paths][:src]}Facebook-SL4.sln"
   build_config[:sln][:net40]         = "#{build_config[:paths][:src]}Facebook-Net40.sln"
   build_config[:sln][:net35]         = "#{build_config[:paths][:src]}Facebook-Net35.sln"
   build_config[:sln][:shfb]        = "#{build_config[:paths][:doc]}doc.shfbproj"
   
    begin
        build_config[:vcs][:rev_id] = `hg id -i`.chomp.chop # remove the +
        build_config[:vcs][:name] = 'hg'
        build_config[:vcs][:short_rev_id] = build_config[:vcs][:rev_id]
    rescue
    end
   
   if(build_config[:vcs][:rev_id].length==0) then
       # if mercurial fails try git
       begin
           build_config[:vcs][:rev_id]    = `git log -1 --pretty=format:%H`.chomp
            build_config[:vcs][:name] = 'git'
           build_config[:vcs][:short_rev_id] = build_config[:vcs][:rev_id][0..7]
       rescue
       end
   end
   
   build_config[:ci][:is_nightly]   = ENV['NIGHTLY'].nil? ? true : ENV['NIGHTLY'].match(/(true|1)$/i) != nil
   build_config[:ci][:build_number] = ENV[build_config[:ci][:build_number_param_name]] || 0
   
   build_config[:version][:full] = "#{build_config[:version][:base]}.#{build_config[:ci][:build_number]}"
   
    if(build_config[:ci][:is_nightly])
        build_config[:version][:long] = "#{build_config[:version][:full]}-nightly-#{build_config[:vcs][:short_rev_id]}"
    else
        build_config[:version][:long] = "#{build_config[:version][:full]}-#{build_config[:vcs][:short_rev_id]}"        
    end
   
    puts build_config if build_config[:log]
    puts
    puts  "     Project Name: #{PROJECT_NAME}"
    puts  "Safe Project Name: #{PROJECT_NAME_SAFE}"
    puts  "          Version: #{build_config[:version][:full]} (#{build_config[:version][:long]})"
    puts  "     Base Version: #{build_config[:version][:base]}"
    print "  CI Build Number: #{build_config[:ci][:build_number]}"
    print " (not running under CI mode)" if build_config[:ci][:build_number] == 0
    puts
    puts  "        Root Path: #{build_config[:paths][:root]}"
    puts
    puts  "              VCS: #{build_config[:vcs][:name]}"
    print "      Revision ID: #{build_config[:vcs][:rev_id]}"
    print "  (#{build_config[:vcs][:short_rev_id]})" if build_config[:vcs][:name] == 'git'
    puts    
    puts   
end

Rake::Task["configure"].invoke

desc "Build .NET 4 binaries"
msbuild :net40 => [:clean_net40,:assemblyinfo_facebook,:assemblyinfo_facebookweb,:assemblyinfo_facebookwebmvc] do |msb|
   msb.properties :configuration => build_config[:configuration]
   msb.solution = build_config[:sln][:net40]
   msb.targets :Build
end

msbuild :clean_net40 do |msb|
   msb.properties :configuration => build_config[:configuration]
   msb.solution = build_config[:sln][:net40]
   msb.targets :Clean
end

task :net40_tests => [:net40_facebook_tests, :net40_facebookweb_tests]

xunit :net40_facebook_tests => [:net40] do |xunit|
    output_path = "#{build_config[:paths][:output]}Tests/Release/"
    xunit.command = build_config[:paths][:xunit][:x86_console_path]
    xunit.assembly = "#{output_path}Facebook.Tests.dll"
    xunit.html_output = "#{output_path}"
    xunit.options = '/nunit ' + output_path + 'Facebook.Tests.nUnit.xml', '/xml ' + output_path + 'Facebook.Tests.xUnit.xml'
end

xunit :net40_facebookweb_tests => [:net40] do |xunit|
    output_path = "#{build_config[:paths][:output]}Tests/Release/"
    xunit.command = build_config[:paths][:xunit][:x86_console_path]
    xunit.assembly = "#{output_path}Facebook.Web.Tests.dll"
    xunit.html_output = "#{output_path}"
    xunit.options = '/nunit ' + output_path + 'Facebook.Web.Tests.nUnit.xml', '/xml ' + output_path + 'Facebook.Web.Tests.xUnit.xml'
end

desc "Build .NET 3.5 binaries"
msbuild :net35 => [:clean_net35,:assemblyinfo_facebook,:assemblyinfo_facebookweb,:assemblyinfo_facebookwebmvc] do |msb|
   msb.properties :configuration => build_config[:configuration]
   msb.solution = build_config[:sln][:net35]
   msb.targets :Build
   #msb.use :net35
end

# compile .net 3.5 libraries using msbuild 4.0 in order to generate the code contract libraries.
# seems like if we use .net 3.5, it does not generate the code contracts.
msbuild :clean_net35 do |msb|
   msb.properties :configuration => build_config[:configuration]
   msb.solution = build_config[:sln][:net35]
   msb.targets :Clean
   #msb.use :net35
end

desc "Build Silverlight 4 binaries"
msbuild :sl4 => [:clean_sl4,:assemblyinfo_facebook] do |msb|
   # temporary hack for bug caused by code contracts
   FileUtils.rm_rf "#{build_config[:paths][:src]}Facebook/obj/"
   
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
msbuild :wp7 => [:clean_wp7,:assemblyinfo_facebook] do |msb|
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
directory "#{build_config[:paths][:working]}NuGet/"
directory "#{build_config[:paths][:dist]}"
directory "#{build_config[:paths][:dist]}NuGet"

msbuild :docs => [:net40] do |msb|
   msb.properties :configuration => build_config[:configuration]
   msb.properties :DocumentationSourcePath => "#{build_config[:paths][:output]}Release/Net40/" if build_config[:configuration] = :Release
   #msb.properties :DocumentationSourcePath => "#{build_config[:paths][:output]}Debug/Net40/" if build_config[:configuration] = :Debug
   msb.solution = build_config[:sln][:shfb]
   msb.targets [:Clean,:Rebuild]
   msb.properties
end

msbuild :clean_docs do |msb|
   msb.properties :configuration => build_config[:configuration]
   msb.properties :DocumentationSourcePath => "#{build_config[:paths][:output]}Release/Net40/" if build_config[:configuration] = :Release
   #msb.properties :DocumentationSourcePath => "#{build_config[:paths][:output]}Debug/Net40/" if build_config[:configuration] = :Debug                   
   msb.solution = build_config[:sln][:shfb]
   msb.targets [:Clean]
   msb.properties
end

desc "Build All Libraries and run tests (default)"
task :libs => [:net35, :net40, :sl4, :wp7, :tests]

desc "Run all tests"
task :tests => [:net40_tests]

desc "Clean All"
task :clean => [:clean_net35, :clean_net40, :clean_sl4, :clean_wp7] do
   FileUtils.rm_rf build_config[:paths][:output]
   FileUtils.rm_rf build_config[:paths][:working]
   FileUtils.rm_rf build_config[:paths][:dist]    
end

task :dist_prepare do
	rm_rf "#{build_config[:paths][:dist]}"
    mkdir "#{build_config[:paths][:dist]}"

	rm_rf "#{build_config[:paths][:working]}"
	mkdir "#{build_config[:paths][:working]}"
end

desc "Create zip archive of the source files"
task :dist_source => [:dist_prepare] do
   src_archive_name = "#{build_config[:paths][:dist]}#{PROJECT_NAME_SAFE}-#{build_config[:version][:long]}.src.zip"
   if (build_config[:vcs][:name] == 'git') then
    sh "git archive HEAD --format=zip > \"#{src_archive_name}\""
   elsif (build_config[:vcs][:name] == 'hg') then
    sh "hg archive -tzip \"#{src_archive_name}\" -p \"#{PROJECT_NAME_SAFE}\""
   end
end

desc "Create distribution packages for libraries."
task :dist_libs => [:dist_prepare, :nuget] do
    # copy nuget outputs
    mkdir "#{build_config[:paths][:dist]}NuGet/"
    mkdir "#{build_config[:paths][:dist]}SymbolSource/"
    cp Dir["#{build_config[:paths][:working]}NuGet/*.nupkg"], "#{build_config[:paths][:dist]}NuGet/"
    cp Dir["#{build_config[:paths][:working]}SymbolSource/*.nupkg"], "#{build_config[:paths][:dist]}SymbolSource/"
    
    # copy binary .dll files
    mkdir "#{build_config[:paths][:working]}Bin/"
    FileUtils.cp_r "#{build_config[:paths][:working]}NuGet/Facebook/lib/.", "#{build_config[:paths][:working]}Bin/Facebook"
    FileUtils.cp_r "#{build_config[:paths][:working]}NuGet/FacebookWeb/lib/.", "#{build_config[:paths][:working]}Bin/FacebookWeb"
    FileUtils.cp_r "#{build_config[:paths][:working]}NuGet/FacebookWebMvc/lib/.", "#{build_config[:paths][:working]}Bin/FacebookWebMvc"
    
    zip :dist_libs do |zip|
        zip.directories_to_zip "#{build_config[:paths][:working]}Bin/"
        zip.output_file = "#{PROJECT_NAME_SAFE}-#{build_config[:version][:long]}.bin.zip"
        zip.output_path = "#{build_config[:paths][:dist]}"
        zip.additional_files = ["#{build_config[:paths][:root]}LICENSE.txt"]
    end
 end

desc "Create distribution package for documentations."
task :dist_docs => [:dist_prepare, :docs] do
   FileUtils.cp_r "#{build_config[:paths][:working]}Documentation/.", "#{build_config[:paths][:dist]}Documentation/"
end

desc "Create distribution packages"
task :dist => [:dist_prepare, :dist_libs, :dist_source, :dist_docs] do
end

assemblyinfo :assemblyinfo_facebook do |asm|
    asm.output_file = "#{build_config[:paths][:src]}Facebook/Properties/AssemblyInfo.cs"
    asm.version = build_config[:version][:full]
    asm.title = "Facebook"
    asm.description = "Facebook C\# SDK"
    asm.product_name = "Facebook C\# SDK"
    asm.company_name = "Thuzi"
    asm.copyright = "Microsoft Public License (Ms-PL)"
    asm.com_visible = false
end

assemblyinfo :assemblyinfo_facebookweb do |asm|
    asm.output_file = "#{build_config[:paths][:src]}Facebook.Web/Properties/AssemblyInfo.cs"
    asm.version = build_config[:version][:full]
    asm.title = "Facebook.Web"
    asm.description = "Facebook C\# SDK"
    asm.product_name = "Facebook C\# SDK"
    asm.company_name = "Thuzi"
    asm.copyright = "Microsoft Public License (Ms-PL)"
    asm.com_visible = false
end

assemblyinfo :assemblyinfo_facebookwebmvc do |asm|
    asm.output_file = "#{build_config[:paths][:src]}Facebook.Web.Mvc/Properties/AssemblyInfo.cs"
    asm.version = build_config[:version][:full]
    asm.title = "Facebook.Web.Mvc"
    asm.description = "Facebook C\# SDK"
    asm.product_name = "Facebook C\# SDK"
    asm.company_name = "Thuzi"
    asm.copyright = "Microsoft Public License (Ms-PL)"
    asm.com_visible = false
end

task :nuspec => ["#{build_config[:paths][:working]}",:libs] do
    rm_rf "#{build_config[:paths][:working]}NuGet/"
    mkdir "#{build_config[:paths][:working]}NuGet/"
    
    Dir.entries(base_dir = "#{build_config[:paths][:build]}NuGet/").each do |name|
        path = "#{base_dir}#{name}/"
        dest_path = "#{build_config[:paths][:working]}NuGet/#{name}/"
        if name == '.' or name == '..' then
            next
        end
        FileUtils.cp_r path, dest_path
        
        nuspec do |nuspec|
            config = nuspec_config[name]
            nuspec.id = name
            nuspec.version = "#{build_config[:version][:full]}"
            nuspec.authors = "#{build_config[:nuspec][:authors]}"
            nuspec.description = config[:description]
            nuspec.language = "en-US"
            nuspec.licenseUrl = "http://facebooksdk.codeplex.com/license"
            nuspec.requireLicenseAcceptance = "false"
            nuspec.projectUrl = "http://facebooksdk.codeplex.com"
            nuspec.iconUrl = "http://bit.ly/facebooksdkicon"
            nuspec.tags = "Facebook"
            nuspec.output_file = "#{dest_path}/#{name}.nuspec"
        
            if !config[:dependencies].nil? then
                config[:dependencies].each do |d|
                    nuspec.dependency d[:id], d[:version]
                end
            end
        end
    end
    
    # copy libs for Facebook.dll, Facebook.Web.dll and Facebook.Web.Mvc.dll
    DaCopier.new(["net35(?!-)","net40(?!-)",".xml.old"]).copy "#{build_config[:paths][:output]}Release", "#{build_config[:paths][:working]}NuGet/Facebook/lib/"    
    DaCopier.new(["net35-client","net40-client","sl4","sl3-wp","Facebook.Web.Mvc",".xml.old"]).copy "#{build_config[:paths][:output]}Release", "#{build_config[:paths][:working]}NuGet/FacebookWeb/lib/"
    DaCopier.new(["net35-client","net40-client","sl4","sl3-wp","Facebook.Web.dll","Facebook.Web.xml","Facebook.Web.pdb","Facebook.Web.Contracts.dll",".xml.old"]).copy "#{build_config[:paths][:output]}Release", "#{build_config[:paths][:working]}NuGet/FacebookWebMvc/lib/"
    
    # duplicate to SymbolSource folder
    rm_rf "#{build_config[:paths][:working]}SymbolSource/"
    mkdir "#{build_config[:paths][:working]}SymbolSource/" 
    FileUtils.cp_r "#{build_config[:paths][:working]}NuGet/Facebook", "#{build_config[:paths][:working]}SymbolSource/Facebook"
    FileUtils.cp_r "#{build_config[:paths][:working]}NuGet/FacebookWeb", "#{build_config[:paths][:working]}SymbolSource/FacebookWeb"
    FileUtils.cp_r "#{build_config[:paths][:working]}NuGet/FacebookWebMvc", "#{build_config[:paths][:working]}SymbolSource/FacebookWebMvc"
    
    # remove pdb files from original NuGetFolder as it is present in SymbolSource folder instead
    FileUtils.rm Dir.glob("#{build_config[:paths][:working]}NuGet/*/**/*.pdb")   
    
    # prepare to copy src to SymbolSource folder
    mkdir "#{build_config[:paths][:working]}SymbolSource/Facebook/src"
    mkdir "#{build_config[:paths][:working]}SymbolSource/FacebookWeb/src"
    mkdir "#{build_config[:paths][:working]}SymbolSource/FacebookWebMvc/src"
    
    # copy the source codes
    DaCopier.new(["^obj$","packages.config",".cd",".user"]).copy "#{build_config[:paths][:src]}Facebook/", "#{build_config[:paths][:working]}SymbolSource/Facebook/src/"
    cp "#{build_config[:paths][:src]}GlobalAssemblyInfo.cs", "#{build_config[:paths][:working]}SymbolSource/Facebook/src/Properties"
    DaCopier.new(["^obj$","packages.config",".cd",".user"]).copy "#{build_config[:paths][:src]}Facebook.Web/", "#{build_config[:paths][:working]}SymbolSource/FacebookWeb/src/"
    cp "#{build_config[:paths][:src]}GlobalAssemblyInfo.cs", "#{build_config[:paths][:working]}SymbolSource/FacebookWeb/src/Properties"
    DaCopier.new(["^obj$","packages.config",".cd",".user"]).copy "#{build_config[:paths][:src]}Facebook.Web.Mvc/", "#{build_config[:paths][:working]}SymbolSource/FacebookWebMvc/src/"
    cp "#{build_config[:paths][:src]}GlobalAssemblyInfo.cs", "#{build_config[:paths][:working]}SymbolSource/FacebookWebMvc/src/Properties"
end

task :nuget => [:nuspec] do
    # copy nuspec files from NuGet to SymbolSource
    cp "#{build_config[:paths][:working]}NuGet/Facebook/Facebook.nuspec", "#{build_config[:paths][:working]}SymbolSource/Facebook/"
    cp "#{build_config[:paths][:working]}NuGet/FacebookWeb/FacebookWeb.nuspec", "#{build_config[:paths][:working]}SymbolSource/FacebookWeb/"
    cp "#{build_config[:paths][:working]}NuGet/FacebookWebMvc/FacebookWebMvc.nuspec", "#{build_config[:paths][:working]}SymbolSource/FacebookWebMvc/"

    Dir["#{build_config[:paths][:working]}*/*/*.nuspec"].each do |name|
        nugetpack :nuget do |nuget|
            nuget.command = "#{build_config[:paths][:nuget]}"
            nuget.nuspec  = name            
            nuget.output = "#{build_config[:paths][:working]}" +
                 (name.start_with?("#{build_config[:paths][:working]}NuGet") ? "NuGet/" : "SymbolSource")
        end
    end
end