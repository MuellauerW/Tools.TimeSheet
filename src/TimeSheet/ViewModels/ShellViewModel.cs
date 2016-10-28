namespace TimeSheet.ViewModels
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Reflection;
  using System.Runtime.CompilerServices;
  using System.Runtime.ExceptionServices;
  using System.Runtime.InteropServices.WindowsRuntime;
  using System.Security.Cryptography.X509Certificates;
  using System.Windows;
  using Microsoft.Win32;

  using Caliburn.Micro;
  using Independentsoft.Exchange;
  using TimeSheet.Framework;
  using TimeSheet.Models;
  using TimeSheet.Services;


  /// <summary>
  /// In dieser Klasse sind alle Elemente für das Management der View 
  /// mit den Abrechnungsdaten
  /// </summary>
  public class ShellViewModel : Caliburn.Micro.PropertyChangedBase, IShell
  {
    private IExchangeCalendarReader _ExchangeCalendarReader = new ExchangeCalendarReader();
    private ICalendarHtmlWriter _CalendarHtmlWriter = new CalendarHtmlWriter();
    private IList<WorkReportItemDTO> _originalWorkReportItemDTOList = null; 

    public ShellViewModel()
    {
      DateTime fistDayOfCurrentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);


      _startDate = fistDayOfCurrentMonth.AddMonths(-1);
      _endDate =   fistDayOfCurrentMonth;
      _totalWorktime = TimeSpan.Zero;
      _workItemsList = new BindableCollection<WorkReportItemAdapter>();
      _kundenListe = new BindableCollection<string>();
      foreach (string dk in  (System.Windows.Application.Current as App)?._currentConfiguration?.ProjectList) {
        _kundenListe.Add(dk);
      }
      //_selectedKundenListe = _kundenListe[0];
      _kundenListeText = _kundenListe[0];
      Version v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
      _mainViewTitle = $"MuellauerW.Stundenlisten v{v.Major}.{v.Minor:00}.{v.Build}";
    }



    private string _mainViewTitle;
    public string MainViewTitle {
      get { return _mainViewTitle; }
      set {
        _mainViewTitle = value;
        NotifyOfPropertyChange(() => MainViewTitle);
      }
    }


    private BindableCollection<string> _kundenListe;
    public BindableCollection<string> KundenListe {
      get { return _kundenListe;}
      set {
        _kundenListe = value;
        NotifyOfPropertyChange(() => KundenListe);
      }
    }

    //private string _selectedKundenListe;
    //public string SelectedKundenListe {
    //  get { return _selectedKundenListe;}
    //  set {
    //    _selectedKundenListe = value;
    //    NotifyOfPropertyChange(() => SelectedKundenListe);
    //  }
    //}

    private string _kundenListeText;
    public string KundenListeText
    {
      get { return _kundenListeText; }
      set
      {
        _kundenListeText = value;
        NotifyOfPropertyChange(() => KundenListeText);
      }
    }



    private DateTime _startDate;
    public DateTime StartDate {
      get { return _startDate; }
      set {
        _startDate = value;
        NotifyOfPropertyChange(() => StartDate);
      }
    }


    private DateTime _endDate;
    public DateTime EndDate {
      get { return _endDate; }
      set {
        _endDate = value;
        NotifyOfPropertyChange(() => EndDate);
      }
    }


    private TimeSpan _totalWorktime;
    public string TotalWorktime {
      get { return string.Format("{0:#00}:{1:00}", ((_totalWorktime.Days * 24) + _totalWorktime.Hours), _totalWorktime.Minutes); }
    }


    private BindableCollection<WorkReportItemAdapter> _workItemsList;
    public BindableCollection<WorkReportItemAdapter> WorkItemsList {
      get { return _workItemsList; }
      set {
        _workItemsList = value;
        NotifyOfPropertyChange(() => WorkItemsList);
      }
    }


    /// <summary>
    /// Button Handler, hier wird das Auslesen der Daten vom Exchange Server gestartet
    /// </summary>
    public void RetrieveData()
    {
      IList<WorkReportItemDTO> foundItems = _ExchangeCalendarReader
        .SetCredentials((System.Windows.Application.Current as App)?._currentConfiguration?.Username, (System.Windows.Application.Current as App)?._currentConfiguration?.Password )
        .SetURI( (System.Windows.Application.Current as App)?._currentConfiguration?.ExchangeServerUri)
        .SetAppointmentSubject(KundenListeText)
        .SetInterval(this._startDate, this._endDate)
        .SetItemCompareStrategy((System.Windows.Application.Current as App)._currentConfiguration.Strategy)
        .RetrieveAppointments();

      _originalWorkReportItemDTOList = foundItems;
      _totalWorktime = TimeSpan.Zero;
      _workItemsList.Clear();
      foreach (WorkReportItemDTO wri in foundItems) {
        _workItemsList.Add(new WorkReportItemAdapter(wri));
        _totalWorktime = _totalWorktime.Add(wri.EndTime.Subtract(wri.StartTime));
      }
      NotifyOfPropertyChange(() => TotalWorktime);
      NotifyOfPropertyChange(() => WorkItemsList);
      NotifyOfPropertyChange(() => CanSaveAs);
    }


    /// <summary>
    /// Klassische Methode, die den Content in ein Html File schreibt
    /// Da die meisten Stundenlisten für Haydn angefertigt werden, schlage ich gleich dieses
    /// Directory vor
    /// </summary>
    public void SaveAs()
    {
      string filePath = (System.Windows.Application.Current as App)?._currentConfiguration?.Path;
      SaveFileDialog sfd = new SaveFileDialog();
      sfd.InitialDirectory = filePath;
      sfd.FileName = string.Format($"{DateTime.Now.Year:0000}_{DateTime.Now.Month:00}_{DateTime.Now.Day:00}_mdc_{KundenListeText}_stundenliste_{_startDate.Year:0000}_{_startDate.Month:00}");
      sfd.DefaultExt = ".html";
      Nullable<bool> retval;
      if ((retval = sfd.ShowDialog()) == true) {
        _CalendarHtmlWriter.WriteToFile(
          sfd.FileName,
          string.Format($"Kunde: {KundenListeText} Abrechnung {_startDate.Year:0000}-{_startDate.Month:00} Stunden: {TotalWorktime}"),
          _originalWorkReportItemDTOList );
      } else {
        MessageBox.Show("Die Daten werden NICHT gespeichert");
      }
    }


    /// <summary>
    /// Diese Property dient als Prädikat, ob der SaveAs Button von Runtime System aktiviert
    /// werden muss.
    /// </summary>
    public bool CanSaveAs {
      get { return _workItemsList.Count > 0; }
    }


  } //end   public class ShellViewModel : Caliburn.Micro.PropertyChangedBase, IShell


} //end namespace TimeSheet.ViewModels
