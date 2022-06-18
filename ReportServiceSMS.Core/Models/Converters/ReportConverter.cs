using ReportService.Core.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Core.Models.Converters
{
    public static class ReportConverter
    {
        public static string ToSmsContent(this Report report)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Raport {report.Title} z dnia {report.Date.ToString("dd-MM-yyyy")}.Environment.NewLine");

            foreach (ReportPosition position in report.Positions)
            {
                sb.Append($"Pozycja: {position.Title}{Environment.NewLine} Opis: {position.Description}{Environment.NewLine} Wartość: {position.Value.ToString("0.00")} zł{Environment.NewLine}");
            }
            return sb.ToString();
        }
    }
}
