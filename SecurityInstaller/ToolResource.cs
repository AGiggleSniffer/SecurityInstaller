using System;
using System.IO;

public static class ToolResource
{
    public static class Support
    {
        public static Uri toolUrl { get; } = new Uri("https://s3-us-west-2.amazonaws.com/nerdtools/remote.msi");

        public static string toolName { get; } = "Remote.msi";

        public static string toolLocation { get; } = Path.Combine(Directory.GetCurrentDirectory(), "Remote.msi");

        public static string toolSwitch { get; } = "/qn";
    }

    //

    public static class Adw 
    {
        public static Uri toolUrl { get; } = new Uri("https://adwcleaner.malwarebytes.com/adwcleaner?channel=release");

        public static string toolName { get; } = "ADWCleaner.exe";

        public static string toolLocation { get; } = @"cmd.exe";

        public static string toolSwitch { get; } = $@"/k {Path.Combine(Directory.GetCurrentDirectory(), toolName)} /eula /clean /noreboot";
    }

    //

    public static class Malwarebytes
    {
        public static Uri toolUrl { get; } = new Uri("https://www.malwarebytes.com/api/downloads/mb-windows?filename=MBSetup.exe");

        public static string toolName { get; } = "MBSetup.exe";

        public static string toolLocation { get; } = @"C:\Program Files\Malwarebytes\Anti-Malware\mbam.exe";

        public static string toolSwitch { get; } = "/verysilent /noreboot";
    }

    //

    public static class Glary
    {
        public static Uri toolUrl { get; } = new Uri("https://www.glarysoft.com/aff/download.php?s=GU");

        public static string toolName { get; } = "GUSetup.exe";

        public static string toolLocation { get; } = @"C:\Program Files (x86)\Glary Utilities\OneClickMaintenance.exe";

        public static string shortcutLocation { get; } = @"C:\Program Files (x86)\Glary Utilities\Integrator.exe";

        public static string toolSwitch { get; } = "/S";
    }

    //

    public static class Ccleaner
    {
        public static Uri toolUrl { get; } = new Uri("https://bits.avcdn.net/productfamily_CCLEANER/insttype_FREE/platform_WIN_PIR/installertype_ONLINE/build_RELEASE");

        public static string toolName { get; } = "CCSetup.exe";

        public static string toolLocation { get; } = @"C:\Program Files\CCleaner\CCleaner64.exe";

        public static string toolSwitch { get; } = "/S";
    }
}

