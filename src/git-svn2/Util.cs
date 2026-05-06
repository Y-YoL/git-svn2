using System.Diagnostics;

namespace YoL.GitSvn2
{
	internal static class Util
	{
		public static ValueTask SetConfigAsync(string key, string value) =>
			InvokeGitAsync("config", $"--local {key} {value}");

		public static ValueTask InvokeGitAsync(string command) =>
			InvokeGitAsync(command, string.Empty);

		public static async ValueTask InvokeGitAsync(string command, string arguments)
		{
			using var process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = "git",
					Arguments = $"{command} {arguments}",
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					CreateNoWindow = true,
				}
			};

			process.OutputDataReceived += (_, e) =>
			{
				if (!string.IsNullOrEmpty(e.Data))
				{
					Console.WriteLine(e.Data);
				}
			};

			process.ErrorDataReceived += (_, e) =>
			{
				if (!string.IsNullOrEmpty(e.Data))
				{
					Console.Error.WriteLine(e.Data);
				}
			};

			process.Start();
			await process.WaitForExitAsync();
			if (process.ExitCode is not 0)
			{
				throw new InvalidOperationException($"git ${command} is return ${process.ExitCode}.");
			}
		}
	}
}
