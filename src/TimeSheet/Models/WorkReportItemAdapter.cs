namespace TimeSheet.Models
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using Caliburn.Micro;


  /// <summary>
  /// Domain Klasse, die einen Kalendereintrag kapselt
  /// </summary>
  public class WorkReportItemAdapter : PropertyChangedBase, IComparable<WorkReportItemAdapter>
  {

    private WorkReportItemDTO _wridto;

    /// <summary>
    /// default ctor
    /// </summary>
    public WorkReportItemAdapter()
    {
      this._wridto = new WorkReportItemDTO() {
        CustomerName = "Internal",
        StartTime = DateTime.Now,
        EndTime = DateTime.Now,
        WorkDescription =  "No work done"
      };
    }

    public WorkReportItemAdapter(WorkReportItemDTO workReportItem)
    {
      this._wridto = workReportItem;
    }


    public string CustomerName {
      get { return _wridto.CustomerName; }
      set {
        _wridto.CustomerName = value;
        NotifyOfPropertyChange(() => CustomerName);
      }
    }

    public DateTime StartTime {
      get { return _wridto.StartTime; }
      set {
        _wridto.StartTime = value;
        NotifyOfPropertyChange(()=> StartTime);
      }
    }

    public string StartTimeAsString {
      get { return _wridto.StartTime.ToString("yyyy-MM-dd HH:mm"); }
    }


    public DateTime EndTime {
      get { return _wridto.EndTime; }
      set {
        _wridto.EndTime = value;
        NotifyOfPropertyChange(() => EndTime);
      }
    }

    public string EndTimeAsString {
      get { return _wridto.EndTime.ToString("yyyy-MM-dd HH:mm"); }
    }


    public string DurationAsString {
      get { return _wridto.Duration.ToString(@"hh\:mm"); }
    }

    public string WorkDescription {
      get { return _wridto.WorkDescription; }
      set {
        _wridto.WorkDescription = value;
        NotifyOfPropertyChange(() => WorkDescription);
      }
    }


    public int CompareTo(WorkReportItemAdapter other)
    {
      return this.StartTime.CompareTo(other.StartTime);
    }
  
  } //end class

} //end namespace TimeSheet.Models



