using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;

public class ProgressReportModel
{
    public List<Tool> DownloadPercentagesList = new List<Tool>();

    public int DownloadCount => DownloadPercentagesList.Count;

    public int DownloadCompleted = 0;

    public int TotalPercentageCompleted()
    {
        int result = 0;

        foreach (Tool tool in DownloadPercentagesList)
        {
            result += tool.PercentageComplete;
        }

        return result != 0 ? result / DownloadCount : 0;
    }

    //public void UpdateToolPercentage(Tool tool, int percentage)
    //{
    //    int index = DownloadPercentagesList.IndexOf(tool);
    //    if (index > -1) {
    //        DownloadPercentagesList[index].PercentageComplete = percentage;
    //    }
    //}

}