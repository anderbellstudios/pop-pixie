require_relative 'usage_data'

input_path = ARGV[0] || raise('Usage: ruby used_by.rb <path>')
raise "error: #{input_path} does not exist" unless File.exist?(input_path)
basename = File.basename(input_path)

UsageData.register_source_files

file = UsageData::SourceFile.all.find { |f| f.basename == basename }
raise "error: #{input_path} is not a registered source file" unless file

used_by = file.used_by
puts "Found #{used_by.count} files using #{basename}:"

used_by.each do |file|
  puts "  #{file.basename}"
end
