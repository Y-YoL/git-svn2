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
			CancellationToken cancellationToken = default) =>
			executor.ExecuteAsync("config", $"--local {key} {value}", cancellationToken);

		public ValueTask AddConfigAsync(
			string key,
			string value,
			CancellationToken cancellationToken = default) =>
			executor.ExecuteAsync("config", $"--local --add {key} {value}", cancellationToken);
	}
}
