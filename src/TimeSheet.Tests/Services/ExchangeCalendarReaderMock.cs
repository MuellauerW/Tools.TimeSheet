namespace TimeSheet.Unit_Tests.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using TimeSheet.Models;
  using TimeSheet.Services;

  /// <summary>
  /// Implementation eines klassischen Mocks zum Testen und für Design
  /// </summary>
  public class ExchangeCalendarReaderMock : IExchangeCalendarReader
  {
    private string _CustomerName;
    private DateTime _From;
    private DateTime _To;


    public IExchangeCalendarReader SetURI(string uri)
    {
      return this;
    }

    public IExchangeCalendarReader SetCredentials(string username, string password)
    {
      return this;
    }

    public IExchangeCalendarReader SetInterval(DateTime start, DateTime end)
    {
      this._From = start;
      this._To = end;
      return this;
    }

    public IExchangeCalendarReader SetAppointmentSubject(string subj)
    {
      this._CustomerName = subj;
      return this;
    }

    public IExchangeCalendarReader SetItemCompareStrategy(ExchangeCalendarItemCompareStrategy strategy)
    {
      return this;
    }


    public IList<WorkReportItemDTO> RetrieveAppointments()
    {
      IList<WorkReportItemDTO> mockItems = new List<WorkReportItemDTO>();
      Random rnd = new Random();
      TimeSpan interval = _To.Subtract(_From);
      int intervalAdDays = interval.Days;

      mockItems.Add(new WorkReportItemDTO() {
        CustomerName = this._CustomerName, 
        EndTime = this._To.Subtract(new TimeSpan(rnd.Next(0, intervalAdDays / 2) , 0, 0, 0, 0)),
        StartTime = this._From.Add(new TimeSpan(rnd.Next(0, intervalAdDays / 2), 0, 0, 0)),
        WorkDescription = "Dummy Text der Appointment" + Environment.NewLine + "Zeile 2" 
      });

      mockItems.Add(new WorkReportItemDTO() {
        CustomerName = this._CustomerName,
        EndTime = this._To.Subtract(new TimeSpan(rnd.Next(0, intervalAdDays / 2), 0, 0, 0, 0)),
        StartTime = this._From.Add(new TimeSpan(rnd.Next(0, intervalAdDays / 2), 0, 0, 0)),
        WorkDescription = "Ein seltsamer Termin" + Environment.NewLine + "Zeile 2"
      });

      mockItems.Add(new WorkReportItemDTO() {
        CustomerName = this._CustomerName,
        EndTime = this._To.Subtract(new TimeSpan(rnd.Next(0, intervalAdDays / 2), 0, 0, 0, 0)),
        StartTime = this._From.Add(new TimeSpan(rnd.Next(0, intervalAdDays / 2), 0, 0, 0)),
        WorkDescription = "Alles lief gut" + Environment.NewLine + "Zeile 2"
      });

      mockItems.Add(new WorkReportItemDTO() {
        CustomerName = this._CustomerName,
        EndTime = this._To.Subtract(new TimeSpan(rnd.Next(0, intervalAdDays / 2), 0, 0, 0, 0)),
        StartTime = this._From.Add(new TimeSpan(rnd.Next(0, intervalAdDays / 2), 0, 0, 0)),
        WorkDescription = "Wasted time" + Environment.NewLine + "Zeile 2"
      });
      return mockItems;
    }

  } //end class ExchangeCalendarReaderMock : IExchangeCalendarReader
  
}  //end namespace Stundenlisten.Services

