namespace TimeSheet.Models
{
  using Independentsoft.Exchange;

  using System;

  /// <summary>
  /// Dieser DTO liefert die Daten aus dem Repository an die App.
  /// Meine ursprüngliche Idee mit der Vererbung klappt nicht so ganz,
  /// da ich in der View-Klasse nicht vom DTO und der Caliburn.Micro Base
  /// erben kann. Multiple Inheritance lässt grüssen...
  /// </summary>
  public class WorkReportItemDTO : IComparable<WorkReportItemDTO>
  {

    public WorkReportItemDTO()
    {
    }

    public WorkReportItemDTO(string customerName, DateTime startTime, DateTime endTime, string workDescription)
    {
      this.CustomerName = customerName;
      this.StartTime = startTime;
      this.EndTime = endTime;
      this.WorkDescription = workDescription;
    }

    public string CustomerName { get;   set; }

    public DateTime StartTime { get;  set; }

    public DateTime EndTime { get;  set; }

    public TimeSpan Duration { get { return EndTime - StartTime; } }

    public String WorkDescription { get;  set; }

    /// <summary>
    /// Sortierkriterium nach StartTime
    /// </summary>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public int CompareTo(WorkReportItemDTO rhs)
    {
      return this.StartTime.CompareTo(rhs.StartTime);
    }


  } //end   public class WorkReportItemDTO

} //end namespace TimeSheet.Models