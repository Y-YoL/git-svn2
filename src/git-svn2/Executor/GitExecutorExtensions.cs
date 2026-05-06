namespace YoL.GitSvn2.Executor;

using System.Threading;
using System.Threading.Tasks;

public static class GitExecutorExtensions
{
	extension(IGitExecutor executor)
	{
		public ValueTask SetConfigAsync(
			string key,
			string value,
			string? workingDirectory = null,
			CancellationToken cancellationToken = default) =>
			executor.ExecuteAsync("config", $"--local {key} {value}", workingDirectory, cancellationToken);

		public ValueTask AddConfigAsync(
			string key,
			string value,
			string? workingDirectory = null,
			CancellationToken cancellationToken = default) =>
			executor.ExecuteAsync("config", $"--local --add {key} {value}", workingDirectory, cancellationToken);
	}
}
