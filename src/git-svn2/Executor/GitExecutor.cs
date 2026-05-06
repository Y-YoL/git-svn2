namespace YoL.GitSvn2.Executor;

using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public sealed class GitExecutor : IGitExecutor
{
        public async ValueTask ExecuteAsync(
            string command,
            string arguments,
            string? workingDirectory = null,
            CancellationToken cancellationToken = default)
        {
            var outputBuffer = new StringBuilder();
            var errorBuffer = new StringBuilder();

            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = string.IsNullOrWhiteSpace(arguments)
                        ? command
                        : $"{command} {arguments}",
                    WorkingDirectory = workingDirectory ?? string.Empty,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                },
                EnableRaisingEvents = true,
            };

            process.OutputDataReceived += (_, e) =>
            {
                if (e.Data is not null)
                {
                    outputBuffer.AppendLine(e.Data);
                }
            };

            process.ErrorDataReceived += (_, e) =>
            {
                if (e.Data is not null)
                {
                    errorBuffer.AppendLine(e.Data);
                }
            };

            using var registration = cancellationToken.Register(() =>
            {
                try
                {
                    if (!process.HasExited)
                    {
                        process.Kill(entireProcessTree: true);
                    }
                }
                catch
                {
                }
            });

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync(cancellationToken);

            if (process.ExitCode != 0)
            {
                throw new GitExecutionException(
                    command,
                    arguments,
                    process.ExitCode,
                    outputBuffer.ToString(),
                    errorBuffer.ToString());
            }
        }
    }
}
