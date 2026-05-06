using System.CommandLine;
using YoL.GitSvn2.Executor;

namespace YoL.GitSvn2.Commands.Remote;

internal class RemoteAddCommand : Command
{
	private static readonly Argument<string> NameArgument = new("name");
	private static readonly Argument<string> UrlArgument = new("url");

	private static readonly Option<bool> NoTagsOption = new("--no-tags")
	{

		DefaultValueFactory = _ => false,
	};

	private readonly IGitExecutor executor;

	public RemoteAddCommand(IGitExecutor executor) :
		base(
			"add",
			$$"""Add a remote named <name> for the repository at <URL>.""")
	{
		this.executor = executor;
		this.Options.Add(NoTagsOption);

		this.Arguments.Add(NameArgument);
		this.Arguments.Add(UrlArgument);

		this.SetAction(this.ExecuteAsync);
	}

	private async Task<int> ExecuteAsync(ParseResult result, CancellationToken cancellationToken)
	{
		var name = result.GetRequiredValue(NameArgument);
		var url = result.GetRequiredValue(UrlArgument);

		var tags = !result.GetValue(NoTagsOption);

		await this.executor.SetConfigAsync($"svn2-remote.{name}.url", url, cancellationToken: cancellationToken);

		var key = $"svn2-remote.{name}.fetch";
		await this.executor.SetConfigAsync(key, $"+trunk:refs/remotes/{name}/trunk", cancellationToken: cancellationToken);
		await this.executor.AddConfigAsync(key, $"+branches/*:refs/remotes/{name}/*", cancellationToken: cancellationToken);

		if (!tags)
		{
			await this.executor.AddConfigAsync($"svn2-remote.{name}.tagOpt", "--no-tags", cancellationToken: cancellationToken);
		}

		return 0;
	}
}
