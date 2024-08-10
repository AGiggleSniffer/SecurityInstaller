using System;
using System.IO;

public static class ToolResource
{
    public readonly struct Support
    {
        public static Uri toolUrl { get; } = new Uri("https://s3-us-west-2.amazonaws.com/nerdtools/remote.msi");

        public static string toolName { get; } = "Remote.msi";

        public static string toolLocation { get; } = $@"""{Path.Combine(Directory.GetCurrentDirectory(), toolName)}""";

        public static string toolSwitch { get; } = "/qn";
    }

    //

    public readonly struct Adw 
    {
        public static Uri toolUrl { get; } = new Uri("https://adwcleaner.malwarebytes.com/adwcleaner?channel=release");

        public static string toolName { get; } = "ADWCleaner.exe";

        public static string toolLocation { get; } = @"cmd.exe";

        public static string toolSwitch { get; } = $@"/k ""{Path.Combine(Directory.GetCurrentDirectory(), toolName)}"" /eula /clean /noreboot";
    }

    //

    public readonly struct Malwarebytes
    {
        public static Uri toolUrl { get; } = new Uri("https://downloads.malwarebytes.com/file/mb-windows");

        public static string toolName { get; } = "MBSetup.exe";

        public static string toolLocation { get; } = @"C:\Program Files\Malwarebytes\Anti-Malware\mbam.exe";

        public static string toolSwitch { get; } = "/verysilent /noreboot";
    }

    //

    public readonly struct Glary
    {
        public static Uri toolUrl { get; } = new Uri("https://www.glarysoft.com/aff/download.php?s=GU");

        public static string toolName { get; } = "GUSetup.exe";

        public static string toolLocation { get; } = @"C:\Program Files (x86)\Glary Utilities\OneClickMaintenance.exe";

        public static string shortcutLocation { get; } = @"C:\Program Files (x86)\Glary Utilities\Integrator.exe";

        public static string toolSwitch { get; } = "/S";
    }

    //

    public readonly struct Ccleaner
    {
        public static Uri toolUrl { get; } = new Uri("https://bits.avcdn.net/productfamily_CCLEANER/insttype_FREE/platform_WIN_PIR/installertype_ONLINE/build_RELEASE");

        public static string toolName { get; } = "CCSetup.exe";

        public static string toolLocation { get; } = @"C:\Program Files\CCleaner\CCleaner64.exe";

        public static string toolSwitch { get; } = "/S";
    }
}

