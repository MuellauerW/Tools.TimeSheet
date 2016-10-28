namespace TimeSheet.Services
{
  using System;
  using System.IO;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using TimeSheet.Models;


  public class CalendarHtmlWriter : ICalendarHtmlWriter
  {
    /// <summary>
    /// Diese Methode schreibt die Terminliste in ein HTML File
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="htmlHeader"></param>
    /// <param name="appointmentList"></param>
    /// <returns></returns>
    public bool WriteToFile(string filename, string htmlHeader, IEnumerable<WorkReportItemDTO> appointmentList)
    {
      StringBuilder htmlFile = new StringBuilder();
      htmlFile.AppendLine("<!doctype html>");
      htmlFile.AppendLine("<html lang=\"en\">");
      htmlFile.AppendLine("<head>");
      htmlFile.AppendLine("<meta charset=\"utf-8\">");
      htmlFile.AppendLine("<style type=\"text/css\">");
      htmlFile.AppendLine("  body {       font-family: Arial, Helvetica, sans-serif;     }");
      htmlFile.AppendLine("  table, th, td { border: 1px solid black; }");
      htmlFile.AppendLine("</style>");
      htmlFile.AppendFormat("<title>{0}</title>", "mdc edv-beratung stundenliste");
      htmlFile.AppendLine("</head>");
      htmlFile.AppendLine("<body>");
      htmlFile.AppendFormat("<h1>{0}</h1>", htmlHeader);

      //Beginn enclosing Div of table      
      htmlFile.AppendLine("<div>");
      htmlFile.AppendLine("<table style=\"width:960px\">");

      foreach (WorkReportItemDTO wi in appointmentList) {
        htmlFile.AppendLine("<tr>");
        htmlFile.AppendFormat("<td>Beginn: {0}</td>", wi.StartTime.ToString("yyyy-MM-dd hh:mm"));
        htmlFile.AppendFormat("<td>Ende: {0}</td>", wi.EndTime.ToString("yyyy-MM-dd hh:mm"));
        htmlFile.AppendFormat("<td>Dauer/h: {0}</td>", wi.Duration.TotalHours);
        htmlFile.AppendLine("</tr>");

        htmlFile.AppendLine("<tr>");
        htmlFile.AppendFormat("<td colspan=\"3\">Beschreibung: {0}</td>", wi.WorkDescription);
        htmlFile.AppendLine("</tr>");
        htmlFile.AppendLine("<tr>");
        htmlFile.AppendFormat("<td colspan=\"3\">  </td>");
        htmlFile.AppendLine("</tr>");
      }
      htmlFile.AppendLine("</table>");
      htmlFile.AppendLine("</div>");
      //End Enclosing div of table
      htmlFile.AppendLine("</body>");
      htmlFile.AppendLine("</html>");

      //Aufbau des html Files fertig, jetz nich in ein File stopfen..
      using (StreamWriter sw = File.CreateText(filename)) {
        sw.Write(htmlFile.ToString());
      }    


      return true;
    }

  } //end public class CalendarHtmlWriter : ICalendarHtmlWriter

} //end namespace TimeSheet.Services
