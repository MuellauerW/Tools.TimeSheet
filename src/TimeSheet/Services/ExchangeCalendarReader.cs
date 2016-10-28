namespace TimeSheet.Services
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.IO;
  using System.Net;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using Caliburn.Micro;
  using Independentsoft.Exchange;
  using TimeSheet.Models;


  /// <summary>
  /// Die Klasse ExchangeCalendarReader ist mit dem Cascade
  /// Pattern implementiert und liest Termine aus dem 
  /// Exchange Server.
  /// </summary>
  public class ExchangeCalendarReader : IExchangeCalendarReader
  {

    public ExchangeCalendarReader()
    {
      this._username = (System.Windows.Application.Current as App)?._currentConfiguration?.Username;
      this._password = (System.Windows.Application.Current as App)?._currentConfiguration?.Password;
      this._exchangeServerURI = (System.Windows.Application.Current as App)?._currentConfiguration?.ExchangeServerUri;
      this._appointmentSelectPredicate = AcceptAppointmentUnconditionally;
    }



    private string _exchangeServerURI;
    public IExchangeCalendarReader SetURI(string uri)
    {
      this._exchangeServerURI = uri;
      return this;
    }

    private string _username;
    private string _password;
    public IExchangeCalendarReader SetCredentials(string username, string password)
    {
      this._username = username;
      this._password = password;
      return this;
    }

    private DateTime _startInterval;
    private DateTime _endInterval;
    public IExchangeCalendarReader SetInterval(DateTime start, DateTime end)
    {
      this._startInterval = start;
      this._endInterval = end;
      return this;
    }

    private string _appointmentSubject;
    public IExchangeCalendarReader SetAppointmentSubject(string subj)
    {
      this._appointmentSubject = subj;
      return this;
    }

    private Predicate<Appointment> _appointmentSelectPredicate;
    private ExchangeCalendarItemCompareStrategy _compareStrategy;
    public IExchangeCalendarReader SetItemCompareStrategy(ExchangeCalendarItemCompareStrategy strategy)
    {
      this._compareStrategy = strategy;
      switch (strategy) {
        case ExchangeCalendarItemCompareStrategy.AcceptAll:
          this._appointmentSelectPredicate = AcceptAppointmentUnconditionally;
          break;

        case ExchangeCalendarItemCompareStrategy.Contains:
          this._appointmentSelectPredicate = AcceptAppointmentOnlyWhenSubjectContains;
          break;

        case ExchangeCalendarItemCompareStrategy.Equals:
          this._appointmentSelectPredicate = AcceptAppointmentOnlyWhenSubjectEquals;
          break;

        case ExchangeCalendarItemCompareStrategy.StartsWith:
          this._appointmentSelectPredicate = AcceptAppointmentOnlyWhenSubjectStartsWith;
          break;

        default:
          break;
      }

      return this;
    }

    /// <summary>
    /// Das ist der Kern der Routine, hier wird alles aufgerollt und dann 
    /// geht es los.
    /// </summary>
    public IList<WorkReportItemDTO> RetrieveAppointments()
    {
      //Aufruf nur sinnvoll, wenn Objekt konfiguriert
      if (String.IsNullOrEmpty(_username) || String.IsNullOrEmpty(_password)) {
        throw new ArgumentException("Username / Password not set!");
      }
      if (String.IsNullOrEmpty(_exchangeServerURI)) {
        throw new ArgumentException("Exchange Server URI not set!");
      }

      List<WorkReportItemDTO> workReportItems = new List<WorkReportItemDTO>();

      NetworkCredential credential = new NetworkCredential(_username, _password);
      Service service = new Service(_exchangeServerURI, credential);

      try {
        FindFolderResponse allCalendarFolders = service.FindFolder(StandardFolder.Calendar, FolderQueryTraversal.Deep);
        Folder rootCalendarFolder = service.GetFolder(StandardFolder.Calendar);
        allCalendarFolders.Folders.Add(rootCalendarFolder);
        
        IList<Independentsoft.Exchange.FolderId> calenderFoldersToBeSearched = new List<Independentsoft.Exchange.FolderId>();
        string[] defaultCalendarSearchList = (System.Windows.Application.Current as App)._currentConfiguration.CalendarList;


        for (int i = 0; i < allCalendarFolders.Folders.Count; i++) {
          string folderName = allCalendarFolders.Folders[i].DisplayName;
          if (defaultCalendarSearchList == null || defaultCalendarSearchList.Contains(folderName)) {
            Trace.WriteLine($"Accepted FolderName: {allCalendarFolders.Folders[i].DisplayName}");
            FolderId f = allCalendarFolders.Folders[i].FolderId;
            calenderFoldersToBeSearched.Add(f);
          } else {
            Trace.WriteLine($"Ignored FolderName: {allCalendarFolders.Folders[i].DisplayName}");
          }
        }

        //wenn ich keine calendar folders habe, brauche ich gar nicht suchen. das ist sinnlos und würde
        //nur das web service mit einem Fehler aussteigen lassen
        if (calenderFoldersToBeSearched.Count == 0) {
          return workReportItems;
        }
        
        IsGreaterThanOrEqualTo restrictionStartInterval = new IsGreaterThanOrEqualTo(AppointmentPropertyPath.StartTime, _startInterval);
        IsLessThan restrictionEndInterval = new IsLessThan(AppointmentPropertyPath.StartTime, _endInterval);
        And restrictionInterval = new And(restrictionStartInterval, restrictionEndInterval);
        IList<FindItemResponse> responses = service.FindItem(calenderFoldersToBeSearched, AppointmentPropertyPath.AllPropertyPaths, restrictionInterval);

        foreach (FindItemResponse response in responses) {
          for (int i = 0; i < response.Items.Count; i++) {
            if (response.Items[i] is Appointment) {
              if (_appointmentSelectPredicate(response.Items[i] as Appointment) == true) {
                WorkReportItemDTO wre = new WorkReportItemDTO();
                Appointment appointment = (Appointment)response.Items[i];
                wre.CustomerName = appointment.Subject;
                wre.StartTime = appointment.StartTime;
                wre.EndTime = appointment.EndTime;
                wre.WorkDescription = appointment.BodyPlainText;
                string s = appointment.BodyHtmlText;
                workReportItems.Add(wre);
              }
            }
          }

        }


        workReportItems.Sort();
      } catch (Exception ex) {
        Trace.WriteLine(ex.Message);
        throw;
      }
      return workReportItems;
    }


    /// <summary>
    /// Dies ist das default Prädikat für die Auswahl eines Appointmens.
    /// Dieses Prädikat sagt einfach immer ja
    /// </summary>
    /// <param name="appointment"></param>
    /// <returns></returns>
    private bool AcceptAppointmentUnconditionally(Appointment appointment)
    {
      return true;
    }


    /// <summary>
    /// Dieses Prädikat selektiert termine, wo das subject gleich ist, aber case sensitiv ist 
    /// es nicht. wir müssen es ja nicht übertreiben
    /// </summary>
    /// <param name="appointment"></param>
    /// <returns></returns>
    private bool AcceptAppointmentOnlyWhenSubjectEquals(Appointment appointment)
    {
      return (appointment.Subject.Equals(this._appointmentSubject, StringComparison.CurrentCultureIgnoreCase));
    }

    /// <summary>
    /// Dieses Prädikat zieht, wenn der Anfang des Strings des Appointments übereinstimmt
    /// </summary>
    /// <param name="appointment"></param>
    /// <returns></returns>
    private bool AcceptAppointmentOnlyWhenSubjectStartsWith(Appointment appointment)
    {
      return (appointment.Subject.StartsWith(_appointmentSubject, StringComparison.CurrentCultureIgnoreCase));
    }


    /// <summary>
    /// Dieses Prädikat zieht, wenn der string case insensitive enthalten ist.
    /// </summary>
    /// <param name="appointment"></param>
    /// <returns></returns>
    private bool AcceptAppointmentOnlyWhenSubjectContains(Appointment appointment)
    {
      return (appointment.Subject.ToLower().Contains(_appointmentSubject.ToLower()));
    }


  } //end public class ExchangeCalendarReader


  /// <summary>
  /// Dieser Enum legt die Suchstrategie für die Auswahl der Items fest.
  /// Es kann also nicht nur nach Items gesucht werden, wo der Text gleich ist,
  /// sondern die Suche kann aufgeweicht werden.
  /// </summary>
  public enum ExchangeCalendarItemCompareStrategy
  {
    AcceptAll, Contains, StartsWith, Equals 
  }


} //end namespace TimeSheet.Services

