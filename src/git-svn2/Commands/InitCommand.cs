using System.CommandLine;
using YoL.GitSvn2.Executor;

namespace YoL.GitSvn2.Commands;

internal class InitCommand : Command
{
	private readonly IGitExecutor executor;

	public InitCommand(IGitExecutor executor)
		: base(
			"init",
			"Initializes an empty Git repository with additional metadata directories for git svn.")
	{
		this.executor = executor;
		this.SetAction(this.ExecuteAsync);
	}

	private async Task<int> ExecuteAsync(ParseResult result, CancellationToken cancellationToken)
	{
		await this.executor.ExecuteAsync("init", "-q", cancellationToken: cancellationToken);

		return 0;
	}
}
