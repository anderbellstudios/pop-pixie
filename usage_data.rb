require 'yaml'

module UsageData
  GUID_REFERENCING_EXTENSIONS = %w[.unity .prefab .mat .asset .controller]
  IDENTIFIER_REFERENCING_EXTENSIONS = %w[.cs]
  IGNORE_UNUSED_EXTENSIONS = %w[.unity .preset .otf .ttf .asset .txt .asmdef]
  IGNORE_UNUSED_DIRS = %w[Assets/test/]

  class SourceFile
    @@by_guid = {}
    @@all = []

    def self.all
      @@all
    end

    attr_reader :path

    def initialize(path)
      @path = path
      @@by_guid[guid] = self
      @@all << self
    end

    def guid
      @guid ||= YAML.load_file(@path + '.meta').fetch('guid')
    end

    def basename
      File.basename(@path)
    end

    def class_name
      basename.split('.').first
    end

    def contents
      @contents ||= File.read(@path)
    end

    def referenced_guids
      return [] unless path.end_with?(*GUID_REFERENCING_EXTENSIONS)
      @referenced_guids ||= contents.scan(/guid: ([0-9a-f]+)/).flatten.uniq
    end

    def referenced_identifiers
      return [] unless path.end_with?(*IDENTIFIER_REFERENCING_EXTENSIONS)
      @referenced_identifiers ||= contents.scan(/\b[A-Z][A-Za-z]+\b/).flatten.uniq
    end

    def uses?(file)
      referenced_guids.include?(file.guid) || (
        file.path.end_with?('.cs') &&
        referenced_identifiers.include?(file.class_name)
      )
    end

    def all_other_files
      @all_other_files ||= @@all.reject { |f| f == self }
    end

    def used_by
      all_other_files.filter { |f| f.uses?(self) }
    end

    def used?
      path.end_with?(*IGNORE_UNUSED_EXTENSIONS) ||
        path.include?(*IGNORE_UNUSED_DIRS) ||
        used_by.any?
    end
  end

  def self.search_dirs
    @@search_dirs ||= Dir.glob(
      File.expand_path('./Assets/*', __dir__)
    ).filter { |f| File.directory?(f) && File.basename(f) != 'vendor' }
  end

  def self.find_matching_pattern(pattern)
    search_dirs.flat_map do |search_dir|
      Dir.glob(File.join(search_dir, '**', pattern))
    end
  end

  def self.register_source_files
    find_matching_pattern('*')
      .filter { |f| File.file?(f) && !f.end_with?('.meta') }
      .each { |f| SourceFile.new(f) }
  end
end
