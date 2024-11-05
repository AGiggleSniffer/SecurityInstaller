﻿using System.Collections.Generic;

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
}