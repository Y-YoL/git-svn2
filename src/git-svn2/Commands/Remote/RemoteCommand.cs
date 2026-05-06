using System.CommandLine;
using YoL.GitSvn2.Commands.Remote;

namespace YoL.GitSvn2.Commands;

internal class RemoteCommand : Command
{
	public RemoteCommand()
		: base(
			"remote",
			$$"""Manage the set of repositories ("remotes") whose branches you track.""")
	{
		this.Subcommands.Add(new RemoteAddCommand());
	}
}
