class CSharpEngine
  def build_attribute(attr_name, attr_data)
    attribute = "[assembly: #{attr_name}("
    attribute << "#{attr_data.inspect}" if attr_data != nil
    attribute << ")]\n"
    
    attribute
  end

  def build_using_statement(namespace)
    "using #{namespace};\n"
  end
    
end
