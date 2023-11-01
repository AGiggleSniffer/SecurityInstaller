using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityInstaller
{
    public class ProgressReportModel
    {
        public int SupportTotal { get; set; } = 0;

        public int AdwTotal { get; set; } = 0;

        public int MbTotal { get; set; } = 0;

        public int GuTotal { get; set; } = 0;

        public int CcTotal { get; set; } = 0;

        public int DownloaderCount { get; set; } = 0;

        public int DownloadCompleted { get; set; } = 0;

        public int PercentageComplete { get; set; } = 0;
    }
}
