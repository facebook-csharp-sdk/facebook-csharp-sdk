require 'albacore/albacoretask'
require 'albacore/assemblyinfolanguages/csharpengine'
require 'albacore/assemblyinfolanguages/vbnetengine'

class AssemblyInfo
  include Albacore::Task
  
  attr_accessor :version, :title, :description, :output_file, :custom_attributes
  attr_accessor :copyright, :com_visible, :com_guid, :company_name, :product_name
  attr_accessor :file_version, :trademark, :lang_engine, :custom_data
  
  attr_array :namespaces
  attr_hash :custom_attributes
  attr_array :custom_data
  
  def initialize
    @namespaces = []
    super()
    update_attributes Albacore.configuration.assemblyinfo.to_hash
  end
  
  def execute
    @lang_engine = CSharpEngine.new unless check_lang_engine
    write_assemblyinfo @output_file
  end
  
  def write_assemblyinfo(assemblyinfo_file)
    valid = check_output_file assemblyinfo_file
    return if !valid
    
    asm_data = build_assembly_info_data

    @logger.info "Generating Assembly Info File At: " + File.expand_path(assemblyinfo_file)
    File.open(assemblyinfo_file, 'w') do |f|      
      f.write asm_data
    end
  end
  
  def check_output_file(file)
    return true if file
    fail_with_message 'output_file cannot be nil'
    return false
  end
  
  def check_lang_engine
    return !@lang_engine.nil?
  end

  def build_assembly_info_data
    asm_data = build_using_statements + "\n"
    
    asm_data << build_attribute("AssemblyTitle", @title) if @title != nil
    asm_data << build_attribute("AssemblyDescription", @description) if @description != nil
    asm_data << build_attribute("AssemblyCompany", @company_name) if @company_name != nil
    asm_data << build_attribute("AssemblyProduct", @product_name) if @product_name != nil
    
    asm_data << build_attribute("AssemblyCopyright", @copyright) if @copyright != nil
    asm_data << build_attribute("AssemblyTrademark", @trademark) if @trademark != nil
    
    asm_data << build_attribute("ComVisible", @com_visible) if @com_visible != nil
    asm_data << build_attribute("Guid", @com_guid) if @com_guid != nil
    
    asm_data << build_attribute("AssemblyVersion", @version) if @version != nil
    asm_data << build_attribute("AssemblyFileVersion", @file_version) if @file_version != nil
    
    asm_data << "\n"
    if @custom_attributes != nil
      attributes = build_custom_attributes()
      asm_data << attributes.join
      asm_data << "\n"
    end
    
    if @custom_data != nil
      @custom_data.each{|data| asm_data << data + "\n"}
    end
    
    asm_data
  end
  
  def build_using_statements
    @namespaces = [] if @namespaces.nil?
    
    @namespaces << "System.Reflection"
    @namespaces << "System.Runtime.InteropServices"
    @namespaces.uniq!
    
    ns = ''
    @namespaces.each do |n|
      ns << @lang_engine.build_using_statement(n)
    end
    
    ns
  end  

  def build_attribute(attr_name, attr_data)
    attribute = @lang_engine.build_attribute(attr_name, attr_data)
    @logger.debug "Build Assembly Info Attribute: " + attribute
    attribute 
  end
  
  def build_custom_attributes()
    attributes = []
    @custom_attributes.each do |key, value|
      attributes << build_attribute(key, value)
    end
    attributes
  end
  
end
