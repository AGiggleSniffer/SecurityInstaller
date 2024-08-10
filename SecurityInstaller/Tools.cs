using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using System;
using IWshRuntimeLibrary;
using System.Windows.Resources;
using static ToolResource;

public class Tools 
{
    // Start DISM/SFC
    public static bool FileChecker()
    {
        Process process = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.UseShellExecute = true;
        startInfo.FileName = "cmd.exe";
        startInfo.Arguments = "pause | /k dism /online /cleanup-image /restorehealth&sfc /scannow";
        process.StartInfo = startInfo;
        process.Start();

        return true;
    }

    // Create Shortcuts
    private static void Shortcut(string shortcutName, string targetFileLocation)
    {
        // Initialize shortcuts
        string shortcutLocation = Path.Combine(@"C:\Users\Public\Desktop\Nerds On Call 800-919NERD", shortcutName + ".lnk");
        WshShell shell = new WshShell();
        IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

        shortcut.TargetPath = targetFileLocation; // The path of the file that will launch when the shortcut is run
        shortcut.Save();
    }

    // Make NOC Folder
    public static async Task<bool> MakeNOC()
    {
        string dir = @"C:\Users\Public\Desktop\Nerds On Call 800-919NERD";
        // If directory does not exist, create it
        if (!Directory.Exists(dir))
        {
            DirectoryInfo folder = Directory.CreateDirectory(dir);

            // Create desktop.ini file
            string deskIni = @"C:\Users\Public\Desktop\Nerds On Call 800-919NERD\desktop.ini";
            using (StreamWriter sw = new StreamWriter(deskIni))
            {
                sw.WriteLine("[.ShellClassInfo]");
                sw.WriteLine("ConfirmFileOp=0");
                sw.WriteLine("IconFile=nerd.ico");
                sw.WriteLine("IconIndex=0");
                sw.WriteLine("InfoTip=Contains the Nerds On Call Security Suite");
                sw.Close();
            }

            string place = @"C:\Users\Public\Desktop\Nerds On Call 800-919NERD\nerd.ico";
            StreamResourceInfo sri = Application.GetResourceStream(new Uri("/nerd.ico", UriKind.Relative));

            if (sri != null)
            {

                using (Stream stream = sri.Stream)
                {
                    using (var file = System.IO.File.Create(place))
                    {
                        await stream.CopyToAsync(file);
                    }
                }
            }


            // Copy Nerds icon then set Attributes
            System.IO.File.SetAttributes(place, FileAttributes.Hidden);

            // Hide icon and desktop.ini then set folder as a system folder
            System.IO.File.SetAttributes(deskIni, FileAttributes.Hidden);
            folder.Attributes |= FileAttributes.System;
            folder.Attributes |= FileAttributes.ReadOnly;
            folder.Attributes |= FileAttributes.Directory;
        }

        // Create Shortcutsss

        // MB Shortcut
        if (System.IO.File.Exists(Malwarebytes.toolLocation))
        {
            Shortcut("Malwarebytes", Malwarebytes.toolLocation);
        }

        // CC Shortcut
        if (System.IO.File.Exists(Ccleaner.toolLocation))
        {
            Shortcut("CCleaner", Ccleaner.toolLocation);
        }

        // GU Shortcut
        if (System.IO.File.Exists(Glary.shortcutLocation))
        {
            Shortcut("Glary Utilities", Glary.shortcutLocation);
        }

        // ADW Shortcut
        if (System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "ADWCleaner.exe")))
        {
            Shortcut("ADWCleaner", Path.Combine(Directory.GetCurrentDirectory(), "ADWCleaner.exe"));
        }

        return true;
    }

    // Adds Registry keys so next time Chrome or Edge opens, it updates or asks to install UBlock Origin
    public static async Task<bool> InstallUB()
    {

        string valueName = "update_url";

        var GC = await Task.Run<bool>(() =>
        {
            // Write to Google Chrome
            string value = "https://clients2.google.com/service/update2/crx";
            string key = @"HKEY_LOCAL_MACHINE\Software\Wow6432Node\Google\Chrome\Extensions\cjpalhdlnbpafiamejdnhcphjbkeiagm";
            Registry.SetValue(key, valueName, value, RegistryValueKind.String);
            return true;
        });

        var edge = await Task.Run<bool>(() =>
        {
            /// Write to MS Edge
            string eValue = "https://edge.microsoft.com/extensionwebstorebase/v1/crx";
            string eKey = @"HKEY_LOCAL_MACHINE\Software\Wow6432Node\Microsoft\Edge\Extensions\odfafepnkmbhccpbejgmiehpchacaeak";
            Registry.SetValue(eKey, valueName, eValue, RegistryValueKind.String);
            return true;
        });

        return true;
    }
}

