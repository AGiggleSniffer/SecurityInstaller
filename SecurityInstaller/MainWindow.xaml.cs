using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.IO;
using System.Diagnostics;
using static MainWindow;
using System.Windows.Interop;
using WpfPoShGUI;
using System.Windows.Threading;
using System.Windows.Navigation;
using System.Net.Http.Handlers;
using System.Net.Http;
using static SecurityInstaller.ToolResource;

namespace SecurityInstaller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                // Call Windows 11 API to Round Corners
                IntPtr hWnd = new WindowInteropHelper(GetWindow(this)).EnsureHandle();
                var attribute = DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
                var preference = DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
                DwmSetWindowAttribute(hWnd, attribute, ref preference, sizeof(uint));
            }
            catch
            {
            }
        }

        // When window is loaded
        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            // Start Global Timer with event
            var timer = new DispatcherTimer();
            // Change frame tick
            int tickRate = 50;
            timer.Interval = TimeSpan.FromMilliseconds(tickRate);
            // Update animations
            timer.Tick += timer_Tick;
            timer.Start();

            // Put hardware string into the correct box
            string info = await Task<string>.Run(() => ComputerInfo.asset);
            AssetOutput.Text = info;
        }

        /// <summary>
        /// Main Funtion on Button Click
        /// </summary>

        private async void StartBtn_Click(object sender, EventArgs e)
        {
            StartBtn.IsEnabled = false;

            // Reset
            this.ProgressBar1.Value = 0;
            this.ProgressBar2.Value = 0;

            // Initialize list for tasks for counting / parallel async
            var tasks = new List<Task<bool>>();

            // Initialize custom Progress model and event
            Progress<ProgressReportModel> progress = new Progress<ProgressReportModel>();
            progress.ProgressChanged += ReportProgress;

            // Progress reporter for task completion / error messages
            var resultsProgress = new Progress<string>(str => 
            {
                ScriptOutput.Text += $"\n{str}";
            });

            // Add tasks to previous list with progress reporters
            AddTasks(tasks, progress, resultsProgress);

            // Set botttom progress bar to amount of tasks to-do
            int count = tasks.Count;
            this.ProgressBar2.Maximum = count;

            // Wait for all tasks to complete
            await Task.WhenAll(tasks);

            // If folder is checked make folder
            if (noc.IsChecked == true)
            {
                try
                {
                    await Tools.MakeNOC();
                    ScriptOutput.AppendText("\n\nNOC Folder Created");
                    this.ProgressBar2.Value += 1;
                }
                catch (Exception ex)
                {
                    ScriptOutput.AppendText($"\n\nError making NOC Folder. Reason:\n{ex.Message}");
                }
            }

            // Cleanup
            // Delete MB shortcut from installer
            if (File.Exists(@"C:\Users\Public\Desktop\Malwarebytes.lnk"))
            {
                File.Delete(@"C:\Users\Public\Desktop\Malwarebytes.lnk");
            }

            // Delete CC shortcut from installer
            if (File.Exists(@"C:\Users\Public\Desktop\CCleaner.lnk"))
            {
                File.Delete(@"C:\Users\Public\Desktop\CCleaner.lnk");
            }

            // Delete GU shortcut from installer
            if (File.Exists(@"C:\Users\Public\Desktop\Glary Utilities.lnk"))
            {
                File.Delete(@"C:\Users\Public\Desktop\Glary Utilities.lnk");
            }

            StartBtn.IsEnabled = true;

            ScriptOutput.AppendText("\n\nDone!");
        }

        // Add tasks to a list
        private void AddTasks(List<Task<bool>> tasks, IProgress<ProgressReportModel> progress, IProgress<string> results)
        {
            // Check if we are downloading / installing / running our tasks
            bool downloadBool = (bool)download.IsChecked;
            bool installBool = (bool)install.IsChecked;
            bool runBool = (bool)run.IsChecked;

            // Initialize progress report model
            ProgressReportModel report = new ProgressReportModel();

            // Progress reporter for how many tasks have completed
            var progressBar2 = new Progress<int>(value =>
            {
                this.ProgressBar2.Value += value;
                report.DownloadCompleted++;
                progress.Report(report);
            });

            // if checked
            if (remote.IsChecked == true)
            {
                // create progress reporter to update specific value
                var progressVal = new Progress<int>(value =>
                {
                    // update report model
                    report.SupportTotal = value;
                    // send updated report model to event
                    progress.Report(report);
                });
                // Count how many downloads we are doing
                report.DownloaderCount++;
                // add task with reporters
                tasks.Add(Downloader.StartDownload(Support.toolUrl, Support.toolName, Support.toolLocation, Support.toolSwitch, progressVal, progressBar2, results, downloadBool, installBool, runBool));
            }

            if (adw.IsChecked == true)
            {
                var progressVal = new Progress<int>(value => 
                {
                    report.AdwTotal = value;
                    progress.Report(report);
                });
                report.DownloaderCount++;
                tasks.Add(Downloader.StartDownload(Adw.toolUrl, Adw.toolName, Adw.toolLocation, Adw.toolSwitch, progressVal, progressBar2, results, downloadBool, installBool, runBool));
            }

            if (mb.IsChecked == true)
            {
                var progressVal = new Progress<int>(value => 
                { 
                    report.MbTotal = value; 
                    progress.Report(report); 
                });
                report.DownloaderCount++;
                tasks.Add(Downloader.StartDownload(Malwarebytes.toolUrl, Malwarebytes.toolName, Malwarebytes.toolLocation, Malwarebytes.toolSwitch, progressVal, progressBar2, results, downloadBool, installBool, runBool));
            }

            if (gu.IsChecked == true)
            {
                var progressVal = new Progress<int>(value => 
                { 
                    report.GuTotal = value; 
                    progress.Report(report); 
                });
                report.DownloaderCount++;
                tasks.Add(Downloader.StartDownload(Glary.toolUrl, Glary.toolName, Glary.toolLocation, Glary.toolSwitch, progressVal, progressBar2, results, downloadBool, installBool, runBool));
            }

            if (cc.IsChecked == true)
            {
                var progressVal = new Progress<int>(value => 
                { 
                    report.CcTotal = value; 
                    progress.Report(report); 
                });
                report.DownloaderCount++;
                tasks.Add(Downloader.StartDownload(Ccleaner.toolUrl, Ccleaner.toolName, Ccleaner.toolLocation, Ccleaner.toolSwitch, progressVal, progressBar2, results, downloadBool, installBool, runBool));
            }

            // if checked
            if (sfc.IsChecked == true)
            {
                // add task
                tasks.Add(Task.Run(() => Tools.FileChecker()));
                // report results
                results.Report("\nDism/SFC Started");
                // report progress
                ProgressBar2.Value += 1;
            }

            if (uBlock.IsChecked == true)
            {
                tasks.Add(Tools.InstallUB());
                results.Report("\nUBlock Origin added to Edge and Chrome");
                ProgressBar2.Value += 1;
            }
        }

        /// <summary>
        /// UI Functionality
        /// </summary>

        // make top bar able to drag window
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        // close window
        private void ExitBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // minimize window
        private void Minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // copy asset info to clipboard
        private void CopyBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(ComputerInfo.asset);
            }
            catch
            {
                ProgressText.Text = "Stop Pressing the Copy Button!";
            }
        }

        // if checked enable child checkboxes else disable children
        private void CB4_Click(object sender, RoutedEventArgs e)
        {
            if (CB4.IsChecked == false)
            {
                adw.IsEnabled = false;
                adw.IsChecked = false;
                adw.Opacity = .1;

                mb.IsEnabled = false;
                mb.IsChecked = false;
                mb.Opacity = .1;

                gu.IsEnabled = false;
                gu.IsChecked = false;
                gu.Opacity = .1;

                cc.IsEnabled = false;
                cc.IsChecked = false;
                cc.Opacity = .1;
            }
            else if (CB4.IsChecked == true)
            {
                adw.IsEnabled = true;
                adw.Opacity = 1;

                mb.IsEnabled = true;
                mb.Opacity = 1;

                gu.IsEnabled = true;
                gu.Opacity = 1;

                cc.IsEnabled = true;
                cc.Opacity = 1;
            }
        }

        /// <summary>
        /// Main Window Animation
        /// </summary>

        // on tick specified in "OnLoad" func
        private void timer_Tick(object sender, EventArgs e)
        {

        }

        // on event call update top progress bar
        private void ReportProgress(object sender, ProgressReportModel e)
        {
            if (this.ProgressBar1.Value < (e.SupportTotal + e.AdwTotal + e.MbTotal + e.GuTotal + e.CcTotal) / e.DownloaderCount)
            {
                e.PercentageComplete = (e.SupportTotal + e.AdwTotal + e.MbTotal + e.GuTotal + e.CcTotal) / e.DownloaderCount;
            }
            this.ProgressBar1.Value = e.PercentageComplete;
            this.ProgressText.Text = $"Downloading {e.DownloadCompleted}/{e.DownloaderCount} items - Download {e.PercentageComplete}% complete";
        }
    }
}
