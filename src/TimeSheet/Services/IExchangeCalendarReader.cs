namespace TimeSheet.Services
{
  using System;
  using System.Collections.Generic;
  using TimeSheet.Models;

  public interface IExchangeCalendarReader 
  {
    IExchangeCalendarReader SetURI(string uri);
    IExchangeCalendarReader SetCredentials(string username, string password);
    IExchangeCalendarReader SetInterval(DateTime start, DateTime end);
    IExchangeCalendarReader SetAppointmentSubject(string subj);
    IExchangeCalendarReader SetItemCompareStrategy(ExchangeCalendarItemCompareStrategy strategy);
    IList<WorkReportItemDTO> RetrieveAppointments();

  } //end   public interface IExchangeCalendarReader 

} //end namespace TimeSheet.Services
