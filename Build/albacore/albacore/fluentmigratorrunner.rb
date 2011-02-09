require 'albacore/albacoretask'

class FluentMigratorRunner
	TaskName = :fluentmigrator
	include Albacore::Task
	include Albacore::RunCommand

	attr_accessor :target, :provider, :connection, :namespace, :output, :preview, :steps, :task, :version, :verbose, :script_directory, :profile, :timeout

	def initialize(command=nil)
		super()
		update_attributes Albacore.configuration.fluentmigrator.to_hash
		@command = command unless command.nil?
	end

  def get_command_line
    commandline = "#{@command}"
    commandline << get_command_parameters
    @logger.debug "Build FuentMigrator Test Runner Command Line: " + commandline
    commandline
  end

	def get_command_parameters
    params = " /target \"#{@target}\""
    params << " /provider #{@provider}"
    params << " /connection \"#{@connection}\""
    params << " /ns #{@namespace}" unless @namespace.nil?
    params << " /out #{@output}" unless @output.nil?
    params << " /preview #{@preview}" unless @preview.nil?
    params << " /steps #{@steps}" unless @steps.nil? || @steps == 0
    params << " /task #{@task}" unless @task.nil?
    params << " /version #{@version}" unless @version.nil? || @version == 0
    params << " /verbose #{@verbose}" unless @verbose.nil?
    params << " /wd \"#{@script_directory}\"" unless @script_directory.nil?
    params << " /profile #{@profile}" unless @profile.nil?
    params << " /timeout #{@timeout}" unless @timeout.nil?
    params
	end

	def execute()
    result = run_command "FluentMigrator", get_command_parameters

    failure_message = "FluentMigrator failed. See build log for detail."
    fail_with_message failure_message if !result
	end
end
