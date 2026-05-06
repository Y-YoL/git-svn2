using System.CommandLine;
using YoL.GitSvn2.Commands;

var root = new RootCommand("git svn2 is a simple conduit for changesets between Subversion and Git.")
{
	Subcommands = {
		new InitCommand(),
	},
};

if (args is [])
{
	args = ["--help"];
}

var parseResult = root.Parse(args);
return await parseResult.InvokeAsync();
