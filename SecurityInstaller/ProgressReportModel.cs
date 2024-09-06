using System.Collections.Generic;
using System.Diagnostics;

public class ProgressReportModel
{
    public List<Tool> DownloadPercentagesList = new List<Tool>();

    public int DownloaderCount => DownloadPercentagesList.Count;

    public int DownloadCompleted { get; set; } = 0;

    public int PercentageComplete { get; set; } = 0;

    public int TotalPercentageCompleted()
    {
        int result = 0;

        foreach (Tool tool in DownloadPercentagesList)
        {
            Debug.WriteLine(tool);
            result += tool.PercentageComplete;
        }

        return result != 0 ? result / DownloaderCount : 0;
    }
}