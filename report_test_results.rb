require 'nokogiri'

def green(str)
  "\e[32m#{str}\e[0m"
end

def red(str)
  "\e[31m#{str}\e[0m"
end

def yellow(str)
  "\e[33m#{str}\e[0m"
end

results_path = File.expand_path('./tests.xml', __dir__)
results = Nokogiri::XML(File.open(results_path))

failure_messages = []

def get_failure_message(test_case)
  full_name = test_case['fullname']
  failure = test_case.xpath('failure').first
  message = failure.xpath('message').first.text
  stack_trace = failure.xpath('stack-trace').first.text
  "#{full_name}\n#{message}\n#{stack_trace}"
end

test_cases = results.xpath('//test-case')

test_cases.each do |test_case|
  result = test_case['result']

  case result
  when 'Passed'
    print green('.')
  when 'Failed'
    print red('F')
    failure_messages << get_failure_message(test_case)
  when 'Skipped'
    print yellow('S')
  else
    print yellow('?')
  end
end

puts

failure_messages.each do |message|
  puts
  puts red(message)
end

File.delete(results_path)

exit 1 if failure_messages.any?
