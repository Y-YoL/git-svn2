using System.CommandLine;

var root = new RootCommand("git svn2 is a simple conduit for changesets between Subversion and Git.");

if(args is []){
	args = ["--help"];
}

var parseResult = root.Parse(args);
return await parseResult.InvokeAsync();
