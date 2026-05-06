namespace YoL.GitSvn2.Executor;

using System.Threading;
using System.Threading.Tasks;

public interface IGitExecutor
{
        /// <summary>
        /// Executes a git command.
        /// </summary>
        /// <param name="command">The git subcommand, such as "config" or "init".</param>
        /// <param name="arguments">The arguments to the git command.</param>
        /// <param name="workingDirectory">The working directory for the git process, or null to use the current directory.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        ValueTask ExecuteAsync(
            string command,
            string arguments,
            string? workingDirectory = null,
            CancellationToken cancellationToken = default);
    }
}
