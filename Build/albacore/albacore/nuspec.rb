require 'albacore/albacoretask'
require 'nokogiri'

class NuspecFile
  def initialize(src, target) 
    @src = src
	@target = target
  end
  
  def render(xml) 
    node = xml.file
	node['src'] = @src
	node['target'] = @target if !@target.nil?
  end
end

class NuspecDependency
  def initialize(id, version)
    @id = id
	@version = version
  end
  
  def render( xml )
    node = xml.dependency
	node['id'] = @id
	node['version'] = @version if !@version.nil?
  end
end

class Nuspec
  include Albacore::Task
  
  attr_accessor :id, :version, :authors, :description, :language, :licenseUrl, :projectUrl, :output_file,
                :owners, :summary, :iconUrl, :requireLicenseAcceptance, :tags, :working_directory
				
  def initialize()
	@dependencies = Array.new
	@files = Array.new
    super()
  end

  def dependency(id, version=nil)
	@dependencies.push NuspecDependency.new(id, version)
  end
  
  def file(src, target=nil)
    @files.push NuspecFile.new(src, target)
  end
  
  def execute
    valid = check_output_file @output_file
    check_required_field(@id, "id")
    check_required_field(@version, "version")
    check_required_field(@authors, "authors")
    check_required_field(@description, "description")
    
    if(! @working_directory.nil?)
      @working_output_file = File.join(@working_directory, @output_file)
    else
      @working_output_file = @output_file
    end

    builder = Nokogiri::XML::Builder.new do |xml|
      build(xml)
    end

    File.open(@working_output_file, 'w') {|f| f.write(builder.to_xml) }
  end

  def build(xml)
    xml.package{
      xml.metadata{
	    xml.id @id
        xml.version @version
        xml.authors @authors
        xml.description @description
        xml.language @language if !@language.nil?
        xml.licenseUrl @licenseUrl if !@licenseUrl.nil?
        xml.projectUrl @projectUrl if !@projectUrl.nil?
        xml.owners @owners if !@owners.nil?
        xml.summary @summary if !@summary.nil?
        xml.iconUrl @iconUrl if !@iconUrl.nil?
        xml.requireLicenseAcceptance @requireLicenseAcceptance if !@requireLicenseAcceptance.nil?
        xml.tags @tags if !@tags.nil?
        if @dependencies.length > 0
          xml.dependencies{
            @dependencies.each {|x| x.render(xml)}
          }
        end
        if @files.length > 0
          xml.files{
            @files.each {|x| x.render(xml)}
          }
        end
     }
   }
  end

  def check_output_file(file)
    return true if file
    fail_with_message 'output_file cannot be nil'
    return false
  end

  def check_required_field(field, fieldname)
    return true if !field.nil?
    raise "Nuget: required field '#{fieldname}' is not defined"
  end
end