using System.CommandLine;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YoL.GitSvn2.Commands.Remote;

namespace GitSvn2.Tests
{
    public class RemoteAddCommandTests
    {
        [Fact]
        public async Task RemoteAddCommand_AddsRemoteConfigWithTagsByDefault()
        {
            var tempPath = CreateTemporaryRepository();

            try
            {
                var root = new RootCommand();
                root.Add(new RemoteAddCommand());

                var args = new[] { "add", "origin", "https://example.com/svn" };
                var previousDirectory = Directory.GetCurrentDirectory();
                Directory.SetCurrentDirectory(tempPath);

                try
                {
                    var parseResult = root.Parse(args);
                    var exitCode = await parseResult.InvokeAsync();
                    Assert.Equal(0, exitCode);
                }
                finally
                {
                    Directory.SetCurrentDirectory(previousDirectory);
                }

                Assert.Equal("https://example.com/svn", GetGitConfig(tempPath, "svn2-remote.origin.url"));

                var fetchValues = GetGitConfigAll(tempPath, "svn2-remote.origin.fetch");
                Assert.Contains("+trunk:refs/remotes/origin/trunk", fetchValues);
                Assert.Contains("+branches/*:refs/remotes/origin/*", fetchValues);

                Assert.False(TryGetGitConfig(tempPath, "svn2-remote.origin.tagOpt", out _));
            }
            finally
            {
                DeleteDirectory(tempPath);
            }
        }

        [Fact]
        public async Task RemoteAddCommand_AddsNoTagsOptionWhenSpecified()
        {
            var tempPath = CreateTemporaryRepository();

            try
            {
                var root = new RootCommand();
                root.Add(new RemoteAddCommand());

                var args = new[] { "add", "origin", "https://example.com/svn", "--no-tags" };
                var previousDirectory = Directory.GetCurrentDirectory();
                Directory.SetCurrentDirectory(tempPath);

                try
                {
                    var parseResult = root.Parse(args);
                    var exitCode = await parseResult.InvokeAsync();
                    Assert.Equal(0, exitCode);
                }
                finally
                {
                    Directory.SetCurrentDirectory(previousDirectory);
                }

                Assert.Equal("https://example.com/svn", GetGitConfig(tempPath, "svn2-remote.origin.url"));
                Assert.Equal("--no-tags", GetGitConfig(tempPath, "svn2-remote.origin.tagOpt"));
            }
            finally
            {
                DeleteDirectory(tempPath);
            }
        }

        private static string CreateTemporaryRepository()
        {
            var tempPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempPath);
            RunGitCommand(tempPath, "init -q");
            return tempPath;
        }

        private static string GetGitConfig(string workingDirectory, string key)
        {
            var result = RunGitCommand(workingDirectory, $"config --local --get {key}");
            if (result.ExitCode is not 0)
            {
                throw new InvalidOperationException($"Could not read git config key '{key}'. Output: {result.Output} Error: {result.Error}");
            }

            return result.Output.Trim();
        }

        private static string[] GetGitConfigAll(string workingDirectory, string key)
        {
            var result = RunGitCommand(workingDirectory, $"config --local --get-all {key}");
            if (result.ExitCode is not 0)
            {
                throw new InvalidOperationException($"Could not read git config key '{key}'. Output: {result.Output} Error: {result.Error}");
            }

            return result.Output
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(value => value.Trim())
                .ToArray();
        }

        private static bool TryGetGitConfig(string workingDirectory, string key, out string value)
        {
            var result = RunGitCommand(workingDirectory, $"config --local --get {key}");
            if (result.ExitCode is 0)
            {
                value = result.Output.Trim();
                return true;
            }

            value = string.Empty;
            return false;
        }

        private static void DeleteDirectory(string directory)
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, recursive: true);
            }
        }

        private static GitResult RunGitCommand(string workingDirectory, string arguments)
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = arguments,
                    WorkingDirectory = workingDirectory,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            return new GitResult(process.ExitCode, output, error);
        }

        private sealed record GitResult(int ExitCode, string Output, string Error);
    }
}
