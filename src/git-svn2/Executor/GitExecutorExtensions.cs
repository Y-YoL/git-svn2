namespace YoL.GitSvn2.Executor;

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

public static class GitExecutorExtensions
{
	public static ValueTask SetConfigAsync(
		this IGitExecutor executor,
		string key,
		string value,
		string? workingDirectory = null,
		CancellationToken cancellationToken = default) =>
		executor.ExecuteAsync("config", $"--local {key} {value}", workingDirectory, cancellationToken);

	public static ValueTask AddConfigAsync(
		this IGitExecutor executor,
		string key,
		string value,
		string? workingDirectory = null,
		CancellationToken cancellationToken = default) =>
		executor.ExecuteAsync("config", $"--local --add {key} {value}", workingDirectory, cancellationToken);

	public static ValueTask InvokeGitAsync(
		this IGitExecutor executor,
		string command,
		string? workingDirectory = null,
		CancellationToken cancellationToken = default) =>
		executor.ExecuteAsync(command, string.Empty, workingDirectory, cancellationToken);

	public static ValueTask InvokeGitAsync(
		this IGitExecutor executor,
		string command,
		string arguments,
		string? workingDirectory = null,
		CancellationToken cancellationToken = default) =>
		executor.ExecuteAsync(command, arguments, workingDirectory, cancellationToken);
}
