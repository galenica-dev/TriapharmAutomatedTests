using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractServiceSanityCheck
{
    public class CodeReport
    {
        private readonly List<string> _CodeForErrorReport;
        private List<string> _ReportResult;

        public CodeReport(List<string> codeForErrorReport)
        {
            _CodeForErrorReport = codeForErrorReport;
            _ReportResult = new List<string>();
        }

        public List<string> GenReport()
        {
            foreach (var report in _CodeForErrorReport)
            {
                switch (report)
                {
                    //apswsRobotInterface
                    case "apswsRobotInterface":
                        _ReportResult.Add($"Robot is not working");
                        break;
                    //apswsCentralService
                    case "apswsCentralService":
                        _ReportResult.Add($"Link to Central Service is broken");
                        break;
                    //apswsCentralService
                    case "apswsMasterCentralService":
                        _ReportResult.Add($"Link to Master Central Service is broken");
                        break;
                    default:
                        continue; // Skip to the next iteration for unknown cases
                }
            }

            return _ReportResult;
        }
    }
}
