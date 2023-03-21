require 'yaml'

$SEARCH_DIRS = Dir.glob(
  File.expand_path('./Assets/*', __dir__)
).filter { |f| File.directory?(f) && File.basename(f) != 'vendor' }

def find_matching_pattern(pattern)
  $SEARCH_DIRS.flat_map do |search_dir|
    Dir.glob(File.join(search_dir, '**', pattern))
  end
end

$GUID_REFERENCING_FILE_PATHS = find_matching_pattern('*.{unity,prefab,asset,mat}')

$REFENECED_GUIDS = $GUID_REFERENCING_FILE_PATHS.flat_map do |file_path|
  File.read(file_path).scan(/guid: ([0-9a-f]+)/).flatten
end.uniq

$CS_FILE_PATHS = find_matching_pattern('*.cs')

$IDENTIFIERS_REFERENCED_IN_FILES = Hash.new { |hash, key| hash[key] = [] }

$CS_FILE_PATHS.each do |file_path|
  File.read(file_path).scan(/\b[A-Z][A-Za-z]+\b/).flatten.uniq.each do |identifier|
    $IDENTIFIERS_REFERENCED_IN_FILES[identifier] << file_path
  end
end

$NOT_UNUSED_FILES = File.readlines(File.expand_path('./.notunused', __dir__)).map(&:strip)

class SourceFile
  attr_reader :path, :guid

  def initialize(path)
    @path = path
    @meta_path = path + '.meta'
    @guid = YAML.load_file(@meta_path).fetch('guid')
  end

  def used?
    in_not_unusd? || unity_file? || guid_referenced? || (cs_file? && class_name_referenced?)
  end

  def basename
    File.basename(@path)
  end

  def in_not_unusd?
    $NOT_UNUSED_FILES.include?(basename)
  end

  def guid_referenced?
    $REFENECED_GUIDS.include?(@guid)
  end

  def unity_file?
    @path.end_with?('.unity')
  end

  def cs_file?
    @path.end_with?('.cs')
  end

  def class_name_referenced?
    $IDENTIFIERS_REFERENCED_IN_FILES[class_name].any? { |file| file != path }
  end

  def class_name
    basename.split('.').first
  end
end

$SOURCE_FILES = find_matching_pattern('*[!.meta]')
  .filter { |f| File.file?(f) }
  .map { |f| SourceFile.new(f) }

$UNUSED_FILES = $SOURCE_FILES.reject(&:used?)

puts "Found #{$UNUSED_FILES.count} unused files:"
$UNUSED_FILES.each do |file|
  puts "  #{file.basename}"
end
