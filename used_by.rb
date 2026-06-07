require_relative 'usage_data'

path = ARGV[0] || raise('Usage: ruby used_by.rb <path> [method]')
method_name = ARGV[1]
raise "error: #{path} does not exist" unless File.exist?(path)
basename = File.basename(path)

UsageData.register_source_files

file = UsageData::SourceFile.all.find { |f| f.basename == basename }
raise "error: #{path} is not a registered source file" unless file

used_by = file.used_by

filtered_used_by =
  if method_name
    used_by.filter { |f| f.referenced_identifiers.include? method_name }
  else
    used_by
  end

qualifier =
  if method_name
    "the #{method_name} method of "
  else
    ''
  end

puts "Found #{filtered_used_by.count} file(s) using #{qualifier + basename}:"

filtered_used_by.each do |file|
  puts "  #{file.basename}"
end
