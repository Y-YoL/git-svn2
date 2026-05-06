namespace YoL.GitSvn2.Executor;

using System;

public sealed class GitExecutionException : Exception
{
        public string Command { get; }
        public string Arguments { get; }
        public int ExitCode { get; }
        public string StandardOutput { get; }
        public string StandardError { get; }

        public GitExecutionException(
            string command,
            string arguments,
            int exitCode,
            string standardOutput,
            string standardError)
            : base($"git {command} {arguments} failed with exit code {exitCode}.")
        {
            Command = command;
            Arguments = arguments;
            ExitCode = exitCode;
            StandardOutput = standardOutput;
            StandardError = standardError;
        }
    }
}
