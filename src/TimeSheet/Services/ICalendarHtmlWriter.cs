namespace TimeSheet.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using TimeSheet.Models;

  public interface ICalendarHtmlWriter
  {
    bool WriteToFile(string filename, string htmlHeader, IEnumerable<WorkReportItemDTO> appointmentList );

  } //end public interface ICalendarHtmlWriter


} //end namespace TimeSheet.Services
