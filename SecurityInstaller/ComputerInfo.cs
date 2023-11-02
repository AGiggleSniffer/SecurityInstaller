using System;
using System.IO;
using System.Management;

public static class ComputerInfo
{
    // Drive Partitions
    private static string GetDriveInfo()
    {
        string dInfo = string.Empty;

        try
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in allDrives)
            {
                double freeSpace = drive.TotalFreeSpace / 1073741824;
                double totalSpace = drive.TotalSize / 1073741824;
                string format = drive.DriveFormat;
                string name = drive.Name;

                dInfo += ($"\t{name} {format} - {freeSpace}GB free of:\t{totalSpace}GB\n");
            }
        }
        catch (Exception ex) { return ex.Message; }

        return dInfo;
    }

    // ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive")
    private static string GetDriveInfo2()
    {
        string dInfo = string.Empty;

        try
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive"))
            {
                foreach (ManagementObject wmi in searcher.Get())
                {
                    object size = wmi["Size"];
                    object tSize = Convert.ToInt64(size) / 1073741824;

                    dInfo += $"\t{(string)wmi["Model"]}\t{tSize}GB\n";

                    wmi.Dispose();
                }
            }
        }
        catch (Exception ex) { return ex.Message; }


        return dInfo;
    }

    // ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController")
    private static string GetGPUInfo()
    {
        string gpuInfo = string.Empty;

        try
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController"))
            {
                foreach (ManagementObject wmi in searcher.Get())
                {
                    gpuInfo += $"{(string)wmi["Name"]}\n\t";

                    wmi.Dispose();
                }
            }
        }
        catch (Exception ex) { return ex.Message; }

        return gpuInfo;
    }

    // ManagementClass("Win32_NetworkAdapterConfiguration")
    private static string GetMacAddress()
    {
        string MACAddress = string.Empty;

        try
        {
            using (ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration"))
            {
                using (ManagementObjectCollection moc = mc.GetInstances())
                {
                    foreach (ManagementObject mo in moc)
                    {
                        if ((bool)mo["IPEnabled"] == true)
                        {
                            object desc = mo["Description"];
                            object mac = mo["MACAddress"];

                            MACAddress += $"\tName:\t{desc}\n\tMac:\t{mac}\n";
                        }

                        mo.Dispose();
                    }
                }
            }
        }
        catch (Exception ex) { return ex.Message; }

        return MACAddress;
    }

    // ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard")
    private static string[] GetBoardInfo()
    {
        string boardMaker = string.Empty;
        string boardInfo = string.Empty;

        try
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard"))
            {
                foreach (ManagementObject wmi in searcher.Get())
                {
                    boardMaker = (string)wmi.GetPropertyValue("Manufacturer");
                    boardInfo = (string)wmi.GetPropertyValue("Product");

                    wmi.Dispose();
                }
            }
        }
        catch (Exception ex) { return new String[] { ex.Message, ex.Message }; }

        return new string[] { boardMaker, boardInfo };
    }

    // ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS")
    private static string[] GetBiosInfo()
    {
        string biosMaker = string.Empty;
        string biosSerial = string.Empty;
        string biosCaption = string.Empty;

        try
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS"))
            {
                foreach (ManagementObject wmi in searcher.Get())
                {
                    biosMaker = (string)wmi.GetPropertyValue("Manufacturer");
                    biosSerial = (string)wmi.GetPropertyValue("SerialNumber");
                    biosCaption = (string)wmi.GetPropertyValue("Caption");

                    wmi.Dispose();
                }
            }
        }
        catch (Exception ex) { return new string[] { ex.Message, ex.Message, ex.Message }; }

        return new string[] { biosMaker, biosSerial, biosCaption };
    }

    // ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_ComputerSystemProduct")
    private static string GetProductName()
    {
        string productName = string.Empty;

        try
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_ComputerSystemProduct"))
            {
                foreach (ManagementObject wmi in searcher.Get())
                {
                    productName = (string)wmi.GetPropertyValue("Name");

                    wmi.Dispose();
                }
            }
        }
        catch (Exception ex) { return ex.Message; }

        return productName;
    }

    // ObjectQuery("SELECT Capacity FROM Win32_PhysicalMemory")
    private static string GetMemory()
    {
        string memSizestring = string.Empty;
        ManagementScope oMs = new ManagementScope();
        ObjectQuery oQuery = new ObjectQuery("SELECT Capacity FROM Win32_PhysicalMemory");

        try
        {
            using (ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oMs, oQuery))
            {
                using (ManagementObjectCollection oCollection = oSearcher.Get())
                {
                    long memorySize = 0;
                    long mCap = 0;

                    // In case more than one Memory sticks are installed
                    foreach (ManagementObject obj in oCollection)
                    {
                        mCap = Convert.ToInt64(obj["Capacity"]);
                        memorySize += mCap;

                        obj.Dispose();
                    }
                    memorySize = memorySize / 1073741824;
                    memSizestring = memorySize.ToString();
                }
            }
        }
        catch (Exception ex) { return ex.Message; }

        return memSizestring;
    }

    // ObjectQuery oQuery2 = new ObjectQuery("SELECT MemoryDevices FROM Win32_PhysicalMemoryArray")
    private static string GetRamSlots()
    {
        string memSlotsString = string.Empty;
        ManagementScope oMs = new ManagementScope();
        ObjectQuery oQuery2 = new ObjectQuery("SELECT MemoryDevices FROM Win32_PhysicalMemoryArray");

        try
        {
            using (ManagementObjectSearcher oSearcher2 = new ManagementObjectSearcher(oMs, oQuery2))
            {
                using (ManagementObjectCollection oCollection2 = oSearcher2.Get())
                {
                    int memSlots = 0;
                    foreach (ManagementObject obj in oCollection2)
                    {
                        memSlots = Convert.ToInt32(obj["MemoryDevices"]);

                        obj.Dispose();
                    }
                    memSlotsString = memSlots.ToString();
                }
            }
        }
        catch (Exception ex) { return ex.Message; }

        return memSlotsString;
    }

    // ManagementClass mc = new ManagementClass("Win32_Processor")
    private static string GetCpuInfo()
    {
        string cpuMan = string.Empty;
        string info = string.Empty;
        double? GHz = 0;

        try
        {
            using (ManagementClass mc = new ManagementClass("Win32_Processor"))
            {
                foreach (ManagementObject mo in mc.GetInstances())
                {
                    GHz = 0.001 * (UInt32)mo.Properties["MaxClockSpeed"].Value;

                    string name = (string)mo["Name"];
                    name = name.Replace("(TM)", "™")
                        .Replace("(tm)", "™")
                        .Replace("(R)", "®")
                        .Replace("(r)", "®")
                        .Replace("(C)", "©")
                        .Replace("(c)", "©")
                        .Replace("    ", " ")
                        .Replace("  ", " ");

                    info = name;

                    mo.Dispose();
                    break;
                }

                using (ManagementObjectCollection objCol = mc.GetInstances())
                {
                    foreach (ManagementObject obj in objCol)
                    {
                        if (cpuMan == String.Empty)
                        {
                            cpuMan = (string)obj.Properties["Manufacturer"].Value;
                        }

                        obj.Dispose();
                    }
                }
            }

        }
        catch (Exception ex) { return ex.Message; }

        return $"{cpuMan}\n\t{info}\n\t{GHz}";
    }

    // ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem")
    private static string GetOSInformation()
    {
        string osInfo = string.Empty;

        try
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
            {
                foreach (ManagementObject wmi in searcher.Get())
                {
                    osInfo = $"{((string)wmi["Caption"]).Trim()}\n\tVersion: {(string)wmi["Version"]}\n\tArchitecture: {(string)wmi["OSArchitecture"]}";

                    wmi.Dispose();
                }
            }
        }
        catch (Exception ex) { return ex.Message; }

        return osInfo;
    }

    // ManagementClass("Win32_ComputerSystem")
    private static string GetSystemSKU()
    {
        string info = string.Empty;

        try
        {
            using (ManagementClass mc = new ManagementClass("Win32_ComputerSystem"))
            {
                using (ManagementObjectCollection moc = mc.GetInstances())
                {
                    foreach (ManagementObject mo in moc)
                    {
                        info = (string)mo["SystemSKUNumber"];
                    }
                }
            }
        }
        catch (Exception ex) { return ex.Message; }

        return info;
    }

    ///
    /// Variables
    ///

    private static readonly string OsInfo = GetOSInformation();
    private static readonly string[] BoardInfo = GetBoardInfo(); // { boardMaker, boardInfo }
    private static readonly string[] BiosInfo = GetBiosInfo(); // { biosMaker, biosSerial, biosCaption }
    private static readonly string ProductName = GetProductName();
    private static readonly string SystemSku = GetSystemSKU();
    private static readonly string RamSlots = GetRamSlots();
    private static readonly string MaxMemory = GetMemory();
    private static readonly string CPU = GetCpuInfo();
    private static readonly string GPU = GetGPUInfo();
    private static readonly string Partitions = GetDriveInfo();
    private static readonly string Drives = GetDriveInfo2();
    private static readonly string Mac = GetMacAddress();

    ///
    /// Tidy up
    ///

    public static string asset = $@"
--------------------------------------------------------
        Host Name
--------------------------------------------------------
    
{"\tDevice:\t" + Environment.MachineName}
{"\tUser:\t" + Environment.UserName}

            
--------------------------------------------------------
        Operating System
--------------------------------------------------------
    
{"\t" + OsInfo}


--------------------------------------------------------
        Device Brand & Model
--------------------------------------------------------
    
{"\t" + BoardInfo[0]}
{"\t" + ProductName}


--------------------------------------------------------
        Bios Make & Model
--------------------------------------------------------
    
{"\t" + BiosInfo[0]}
{"\t" + BoardInfo[1]}
{"\tVer: " + BiosInfo[2]}


--------------------------------------------------------
        Serial Number
--------------------------------------------------------
    
{"\tSerial:\t" + BiosInfo[1]}
{"\tSKU:\t" + SystemSku}


--------------------------------------------------------
        Memory
--------------------------------------------------------
    
{"\tSticks: " + RamSlots}
{"\tTotal: " + MaxMemory}

            
--------------------------------------------------------
        CPU
--------------------------------------------------------
    
{"\t" + CPU}


--------------------------------------------------------
        GPU
--------------------------------------------------------
    
{"\t" + GPU}

--------------------------------------------------------
        DiskInfo
--------------------------------------------------------
    
    Partitions:
{Partitions}
    Local / Physical Drives:
{Drives}

--------------------------------------------------------
        Mac Address
--------------------------------------------------------

{Mac}

";
}
