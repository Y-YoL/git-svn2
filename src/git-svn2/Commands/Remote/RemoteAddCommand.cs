using System.CommandLine;

namespace YoL.GitSvn2.Commands.Remote;

internal class RemoteAddCommand : Command
{
	private static readonly Argument<string> NameArgument = new("name");
	private static readonly Argument<string> UrlArgument = new("url");

	private static readonly Option<bool> NoTagsOption = new("--no-tags")
	{

		DefaultValueFactory = _ => false,
	};

	public RemoteAddCommand() :
		base(
			"add",
			$$"""Add a remote named <name> for the repository at <URL>.""")
	{
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

		await Util.SetConfigAsync($"svn2-remote.{name}.url", url);

		var key = $"svn2-remote.{name}.fetch";
		await Util.SetConfigAsync(key, $"+trunk:refs/remotes/{name}/trunk");
		await Util.AddConfigAsync(key, $"+branches/*:refs/remotes/{name}/*");

		if (!tags)
		{
			await Util.AddConfigAsync($"svn2-remote.{name}.tagOpt", "--no-tags");
		}

		return 0;
	}
}
