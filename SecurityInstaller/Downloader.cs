using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http.Handlers;
using System.Net.Http;
using System.Threading.Tasks;

namespace SecurityInstaller
{
    public static class Downloader
    {
        private static readonly HttpClientHandler handler = new HttpClientHandler() { AllowAutoRedirect = true };
        private static readonly ProgressMessageHandler ph = new ProgressMessageHandler(handler);
        private static readonly HttpClient client = new HttpClient(ph);

        public static async Task<bool> StartDownload(Uri url, string fileName, string location, string installSwitch, IProgress<int> progress, IProgress<int> progressBar2, IProgress<string> results, bool download, bool install, bool run)
        {
            var watch = Stopwatch.StartNew();

            ph.HttpReceiveProgress += (_, args) =>
            {
                progress.Report(args.ProgressPercentage);
            };

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            if (download == true)
            {
                var rez = await Download(url, filePath);

                results.Report($"\n{fileName} download {rez}\nElapsed Time: {watch.ElapsedMilliseconds}ms");
            }

            if (install == true && !(fileName == "ADWCleaner.exe" || fileName == "Remote.msi"))
            {
                var rez = await Task.Run(() => Install(fileName, installSwitch, results));

                results.Report($"{fileName} install {rez}\nElapsed Time: {watch.ElapsedMilliseconds}ms");
            }
            else if (install == true && run == false && fileName == "Remote.msi")
            {
                await Task.Run(() => Process.Start(location, installSwitch));

                results.Report($"\n{fileName} opened in: {watch.ElapsedMilliseconds}ms");
            }

            if (run == true)
            {
                try
                {
                    if (fileName == "ADWCleaner.exe" || fileName == "Remote.msi")
                    {
                        await Task.Run(() => Process.Start(location, installSwitch));

                        results.Report($"\n{fileName} opened in: {watch.ElapsedMilliseconds}ms");
                    }
                    else
                    {
                        await Task.Run(() => Process.Start(location));

                        results.Report($"\n{fileName} opened in: {watch.ElapsedMilliseconds}ms");
                    }
                }
                catch (Exception ex)
                {
                    results.Report($"\nError Opening:\n{ex.Message}");
                }
            }

            watch.Reset();

            progressBar2.Report(1);

            return true;
        }

        private static async Task<string> Download(Uri url, string filePath)
        {
            try
            {
                using (var rez = await client.GetStreamAsync(url))
                {
                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        await rez.CopyToAsync(fs);
                    }
                }
                return "complete.";
            }
            catch (Exception ex)
            {
                return $"not complete.\nReason: {ex.Message}";
            }
        }

        private static string Install(string fileName, string installSwitch, IProgress<string> results)
        {
            try
            {
                string output = string.Empty;
                using (Process process = new Process())
                {
                    // Install Silently
                    process.StartInfo.FileName = fileName;
                    process.StartInfo.Arguments = installSwitch;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.Start();

                    StreamReader reader = process.StandardOutput;
                    output = reader.ReadToEnd();

                    results.Report(output);

                    process.WaitForExit();
                }

                return $"complete.\nResults:{output}";
            }
            catch (Exception ex)
            {
                return $"not complete.\nReason: {ex.Message}";
            }
        }
    }
}
