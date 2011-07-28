COPYRIGHT = 'Copyright 2009-2011 Chris Canal. All rights reserved.'

include FileTest
require 'albacore'

BUILD_NUMBER_BASE = '0.1.0'
BUILD_NUMBER = ENV["BUILD_NUMBER"].nil? ? 0 : ENV["BUILD_NUMBER"]
PRODUCT = 'FluentMvc'
CLR_TOOLS_VERSION = 'v4.0.30319'

BUILD_CONFIG = ENV['BUILD_CONFIG'] || 'Release'
BUILD_CONFIG_KEY = ENV['BUILD_CONFIG_KEY'] || 'NET40'
TARGET_FRAMEWORK_VERSION = (BUILD_CONFIG_KEY == 'NET40' ? 'v4.0' : 'v3.5')
MSB_USE = (BUILD_CONFIG_KEY == "NET40" ? :net4 : :net35)

props = { 
  :src => File.expand_path("src"),
  :lib => File.expand_path("lib"),
  :stage => File.expand_path("build_output"),
  :artifacts => File.expand_path("build_artifacts"),
}

puts "Building for .NET Framework #{TARGET_FRAMEWORK_VERSION} in #{BUILD_CONFIG}-mode."
 
desc "Cleans, compiles, unit tests, and packages zip"
task :all => [:default, :create_nuget_package]

desc "**Default**, Cleans, compiles, and runs tests"
task :default => [:clean, :compile, :unit_tests]

desc "Prepares the working directory for a new build"
task :clean do
	FileUtils.rm_rf props[:artifacts]
	FileUtils.rm_rf props[:stage]
	
	# work around latency issue where folder still exists for a short while after it is removed
	waitfor { !exists?(props[:stage]) }
	waitfor { !exists?(props[:artifacts]) }
	
	Dir.mkdir props[:stage]
	Dir.mkdir props[:artifacts]
end

desc "Update the common version information for the build. You can call this task without building."
assemblyinfo :global_version do |asm|
  asm_version = BUILD_NUMBER_BASE + ".0"
  commit_data = get_commit_hash_and_date
  commit = commit_data[0]
  commit_date = commit_data[1]
  build_number = "#{BUILD_NUMBER_BASE}.#{Date.today.strftime('%y%j')}"
  tc_build_number = ENV["BUILD_NUMBER"]
  puts "##teamcity[buildNumber '#{build_number}-#{tc_build_number}']" unless tc_build_number.nil?
  
  # Assembly file config
  asm.product_name = PRODUCT
  asm.description = "Git commit hash: #{commit} - #{commit_date} - FluentMvc is a library for adding conventions to ASP.NET MVC using a fluent configuration API."
  asm.version = asm_version
  asm.file_version = build_number
  asm.custom_attributes :AssemblyInformationalVersion => "#{asm_version}", :ComVisibleAttribute => false
  asm.copyright = COPYRIGHT
  asm.output_file = 'src/CommonAssemblyInfo.cs'
  asm.namespaces "System", "System.Reflection", "System.Runtime.InteropServices", "System.Security"
end

desc "Cleans, versions, compiles the application and generates build_output/."
task :compile => [:global_version, :build]

desc "Only compiles the application."
msbuild :build do |msb|
	msb.properties :Configuration => BUILD_CONFIG, 
	    :BuildConfigKey => BUILD_CONFIG_KEY,
		:TargetFrameworkVersion => TARGET_FRAMEWORK_VERSION,
		:Platform => 'Any CPU'
	msb.use MSB_USE
	msb.targets :Clean, :Build
	msb.solution = 'src/FluentMvc.sln'
end

desc "Runs unit tests"
nunit :unit_tests => :build do |nunit|
  Dir.mkdir props[:artifacts] unless exists?(props[:artifacts])
  nunit.command = "lib/NUnit/net-2.0/nunit-console.exe"
  nunit.options '/nothread', '/nologo', "/xml=#{File.join(props[:artifacts], 'nunit-test-results.xml')}"
  nunit.assemblies File.join(props[:src], "FluentMvc.Spec/bin/#{BUILD_CONFIG}/FluentMvc.Spec.dll")
end

desc "ZIPs up the build results"
zip :package => :unit_tests do |zip|
    Dir.mkdir props[:stage] unless exists?(props[:stage])
    Dir.mkdir props[:artifacts] unless exists?(props[:artifacts])
    copy(File.join(props[:src], "FluentMvc/bin/#{BUILD_CONFIG}/FluentMvc.dll"), File.join(props[:stage], 'FluentMvc.dll'))
	zip.directories_to_zip = [props[:stage]]
	zip.output_file = "FluentMvc-#{BUILD_NUMBER_BASE}.zip"
	zip.output_path = [props[:artifacts]]
end

# desc "Create the nuspec"
# nuspec :create_nuget_spec => :package do |nuspec|
   # nuspec.id = "FluentMvc"
   # nuspec.version = BUILD_NUMBER_BASE + ".0"
   # nuspec.authors = "Chris Canal"
   # nuspec.owners = "Chris Canal"
   # nuspec.description = "FluentMvc is a library for adding conventions to ASP.NET MVC using a fluent configuration API."
   # nuspec.summary = "FluentMvc is a library for adding conventions to ASP.NET MVC using a fluent configuration API."
   # nuspec.language = "en-US"
   # nuspec.licenseUrl = "https://github.com/carcer/FluentMvc/blob/master/LICENSE"
   # nuspec.projectUrl = "https://github.com/carcer/FluentMvc"
   # nuspec.working_directory = [props[:artifacts]]
   # nuspec.output_file = "FluentMvc.nuspec"
   # nuspec.tags = "mvc"
# end

# desc "Create the nuget package"
# nugetpack :create_nuget_package => :create_nuget_spec do |nuget|
   # nuget.command = File.join(props[:lib], "nuget.exe")
   # nuget.nuspec = File.join(props[:artifacts], "FluentMvc.nuspec")
   # nuget.base_folder = props[:stage]
   # nuget.output = props[:artifacts]
# end

def get_commit_hash_and_date
	begin
		commit = `git log -1 --pretty=format:%H`
		git_date = `git log -1 --date=iso --pretty=format:%ad`
		commit_date = DateTime.parse( git_date ).strftime("%Y-%m-%d %H%M%S")
	rescue
		commit = "git unavailable"
	end

	[commit, commit_date]
end

def waitfor(&block)
	checks = 0
	
	until block.call || checks >10 
		sleep 0.5
		checks += 1
	end
	
	raise 'waitfor timeout expired. make sure that you aren\'t running something from the build output folders' if checks > 10
end