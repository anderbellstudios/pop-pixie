require 'tty-prompt'
require 'fileutils'

prompt = TTY::Prompt.new

environments = Dir.glob('./environments/*').select { |f| File.directory?(f) }.map { |f| File.basename(f) }
previous_environment = File.read('./environments/current').strip rescue nil

new_environment = prompt.select('Switch to environment:', environments, default: previous_environment, filter: true)

environment_files = Dir.glob("./environments/#{new_environment}/**/*", File::FNM_DOTMATCH)
  .select { |f| File.file?(f) }
  .map { |f| f.sub("./environments/#{new_environment}/", '') }

unless previous_environment.nil?
  environment_files.each do |file|
    system("diff -q ./#{file} ./environments/#{previous_environment}/#{file}")

    if $?.exitstatus == 1
      $stderr.puts "Manually merge #{file} before switching environments"
      exit 1
    end
  end
end

environment_files.each do |file|
  FileUtils.cp("./environments/#{new_environment}/#{file}", "./#{file}")
end

File.write('./environments/current', new_environment)
