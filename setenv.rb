require 'tty-prompt'
require 'fileutils'

prompt = TTY::Prompt.new

environments = Dir.glob('./environments/*').select { |f| File.directory?(f) }.map { |f| File.basename(f) }
previous_environment = File.read('./environments/current').strip rescue nil

new_environment = prompt.select('Switch to environment:', environments, default: previous_environment, filter: true)

environment_files = Dir.glob("./environments/#{new_environment}/**/*", File::FNM_DOTMATCH)
  .select { |f| File.file?(f) }
  .map { |f| f.sub("./environments/#{new_environment}/", '') }

environment_files.each do |file|
  puts "Linking #{file}"
  FileUtils.rm("./#{file}")
  FileUtils.ln("./environments/#{new_environment}/#{file}", "./#{file}")
end

File.write('./environments/current', new_environment)
