using System.CommandLine;
using YoL.GitSvn2.Commands;
using YoL.GitSvn2.Executor;

var executor = new GitExecutor();
var root = new RootCommand("git svn2 is a simple conduit for changesets between Subversion and Git.")
{
	Subcommands = {
		new InitCommand(executor),
		new RemoteCommand(executor),
	},
};

if (args is [])
{
	args = ["--help"];
}

var parseResult = root.Parse(args);
return await parseResult.InvokeAsync();
