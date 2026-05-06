using System.CommandLine;

namespace YoL.GitSvn2.Commands;

internal class InitCommand : Command
{
	public InitCommand()
		: base(
			"init",
			"Initializes an empty Git repository with additional metadata directories for git svn.")
	{
		this.SetAction(this.ExecuteAsync);
	}

	private async Task<int> ExecuteAsync(ParseResult result, CancellationToken cancellationToken)
	{
		await Util.InvokeGitAsync("init -q");

		return 0;
	}
}
