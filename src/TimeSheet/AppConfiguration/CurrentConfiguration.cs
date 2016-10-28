namespace TimeSheet.AppConfiguration
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Dynamic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using Nett;
  using TimeSheet.Services;


  /// <summary>
  /// Diese Klasse ist die Root Klasse der Konfiguration und die Konfiguration wird aus dem File
  /// Stundenlisten.AppConfiguration.private.toml gelesen.
  /// </summary>
  public class CurrentConfiguration
  {
    public string ExchangeServerUri { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string[] ProjectList { get; set; }
    public string[] CalendarList { get; set; }
    public string Path { get; set; }
    public ExchangeCalendarItemCompareStrategy Strategy { get; set; }

    /// <summary>
    /// Mit dieser utiltiy Klasse, kamm man immer ein leeres Template erzeugen, das man dann
    /// mit kundenspezifischen Daten befüllen kann. So sehe ich dann auch, ob wie das ding zu handlen ist
    /// </summary>
    /// <param name="filename"></param>
    public static void CreateTemplateConfig(string filename)
    {
      CurrentConfiguration cfg = new CurrentConfiguration() {
        ExchangeServerUri = @"https://outlook.office365.com/EWS/Exchange.asmx",
        Username = "john@doe.com",
        Password = "secrtepwd",
        ProjectList = new string[] { "Project1", "Project2", "Project3" },
        CalendarList = new string[] { "Calendar", "MyPersonalCalendar" },
        Strategy = ExchangeCalendarItemCompareStrategy.Equals
      };
      Toml.WriteFile(cfg, filename);
    } //end  public static void CreateTemplateConfig(string filename)


  } //end   public class CurrentConfiguration


} //end namespace using TimeSheet.AppConfiguration



