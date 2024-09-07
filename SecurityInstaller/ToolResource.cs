using System;
using System.Collections.Generic;
using System.IO;

public class Tool
{
    public Uri ToolUrl { get; private set; }

    public string ToolName { get; private set; }

    public string ToolLocation { get; private set; }

    public string ToolCliSwitch { get; private set; }

    public string ShortcutLocation { get; private set; }

    public int PercentageComplete { get; set; }

    public Tool(string url, string name, string location, string cliSwitch, string shortcutLoc = null)
    {
        ToolUrl = new Uri(url);
        ToolName = name;
        ToolLocation = location;
        ToolCliSwitch = cliSwitch;
        ShortcutLocation = shortcutLoc;
        PercentageComplete = 0;
    }
}

public class ToolResource
{
    public Tool Malwarebytes { get; private set; } = new Tool("https://downloads.malwarebytes.com/file/mb5_offline", "MBSetup.exe", @"C:\Program Files\Malwarebytes\Anti-Malware\mbam.exe", "/verysilent /noreboot");
    public Tool Glary { get; private set; } = new Tool("https://www.glarysoft.com/aff/download.php?s=GU", "GUSetup.exe", @"C:\Program Files (x86)\Glary Utilities\OneClickMaintenance.exe", "/S", @"C:\Program Files (x86)\Glary Utilities\Integrator.exe");
    public Tool CCleaner { get; private set; } = new Tool("https://bits.avcdn.net/productfamily_CCLEANER/insttype_FREE/platform_WIN_PIR/installertype_ONLINE/build_RELEASE", "CCSetup.exe", @"C:\Program Files\CCleaner\CCleaner64.exe", "/S");
    
    public Tool Support { get; private set; } = new Tool("https://s3-us-west-2.amazonaws.com/nerdtools/remote.msi", "Remote.msi", $@"""{Path.Combine(Directory.GetCurrentDirectory(), "Remote.msi")}""", "/qn");
    public Tool Adw { get; private set; } = new Tool("https://adwcleaner.malwarebytes.com/adwcleaner?channel=release", "ADWCleaner.exe", @"cmd.exe", $@"/k ""{Path.Combine(Directory.GetCurrentDirectory(), "ADWCleaner.exe")}"" /eula /clean /noreboot");
    public Tool Chrome { get; private set; } = new Tool("https://dl.google.com/chrome/install/chrome_installer.exe", "ChromeSetup.exe", @"C:\Program Files\Google\Chrome\Application", "/silent");
    public Tool FireFox { get; private set; } = new Tool("https://www.mozilla.org/de/firefox/all/#product-desktop-release", "FFSettup.exe", @"C:\Program Files\Mozilla Firefox\firefox.exe", "/S");
    public Tool LibreOffice { get; private set; } = new Tool("https://www.libreoffice.org/donate/dl/win-x86_64/24.8.0/en-US/LibreOffice_24.8.0_Win_x86-64.msi", "LibreSetup.exe", @"C:\Program Files\LibreOffice\program\soffice.exe", "/S");
    public Tool SevenZip { get; private set; } = new Tool("https://7-zip.org/a/7z2408-x64.exe", "7zipSetup.exe", @"C:\Program Files\7-Zip\7zFM.exe", "/S");
    public Tool Steam { get; private set; } = new Tool("https://cdn.akamai.steamstatic.com/client/installer/SteamSetup.exe", "SteamSetup.exe", @"C:\Program Files (x86)\Steam\steam.exe", "/S");
}

