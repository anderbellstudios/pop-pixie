require_relative 'usage_data'

UsageData.register_source_files

exclude_files = File.readlines(File.expand_path('./.notunused', __dir__)).map(&:strip)
unused_files = UsageData::SourceFile.all.reject(&:used?).reject { |f| exclude_files.include?(f.basename) }

puts "Found #{unused_files.count} unused files:"

unused_files.each do |file|
  puts "  #{file.basename}"
end
