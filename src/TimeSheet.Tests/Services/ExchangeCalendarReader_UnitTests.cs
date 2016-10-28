namespace TimeSheet.Unit_Tests.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using NUnit.Framework;
  using TimeSheet.Models;
  using TimeSheet.Services;
  using NUnit.Framework.Constraints;



  [TestFixture]
  class ExchangeCalendarReader_UnitTests
  {

    [Test]
    public void ExchangeCalendarReader_CanBeIninstantiated()
    {
      //Arrange
      IExchangeCalendarReader sut;

      //Act
      sut = new ExchangeCalendarReader();
      
      //Assert
      Assert.That(sut, Is.Not.Null);
    }


    [Test]
    public void ExchangeCalendarReader_ThrowsExcheption_When_not_initialised()
    {
      //Arrange
      IExchangeCalendarReader sut;

      //Act
      sut = new ExchangeCalendarReader();

      //Assert
      Assert.Throws<ArgumentException>(delegate { sut.RetrieveAppointments(); });
    }


    [Test]
    [Explicit]
    public void ExchangeCalendarReader_AcceptsConfiguration()
    {

      //NetworkCredential credential = new NetworkCredential("ad\\wolfgang.muellauer", "WTfsK6FkVh");
      //Service service = new Service("https://srvexchange.mdc.at/ews/Exchange.asmx", credential);
      //List<WorkReportItem> workReportItems = new List<WorkReportItem>();

      //Arrange
      IExchangeCalendarReader sut;
      IList<WorkReportItemDTO> sut_List; 
      
      //Act
      sut = new ExchangeCalendarReader();
      sut_List = sut
        .SetCredentials(@"ad\wolfgang.muellauer", @"WTfsK6FkVh")
        .SetAppointmentSubject("HAYDN")
        .SetURI(@"https://srvexchange.mdc.at/ews/Exchange.asmx")
        .SetInterval(new DateTime(2015, 01, 01), new DateTime(2015, 12, 31))
        .RetrieveAppointments();

      //Assert
      Assert.That(sut_List, Is.Not.Null);
      Assert.That(sut_List.Count, Is.GreaterThan(30));
    }

    [Test]
    public void ExchangeCalendarReaderMock_CanBeIninstantiated()
    {
      //Arrange
      IExchangeCalendarReader sut;

      //Act
      sut = new ExchangeCalendarReaderMock();

      //Assert
      Assert.That(sut, Is.Not.Null);
    }



    [Test]
    public void ExchangeCalendarReaderMock_AcceptsConfiguration()
    {

      //NetworkCredential credential = new NetworkCredential("ad\\wolfgang.muellauer", "WTfsK6FkVh");
      //Service service = new Service("https://srvexchange.mdc.at/ews/Exchange.asmx", credential);
      //List<WorkReportItem> workReportItems = new List<WorkReportItem>();

      //Arrange
      IExchangeCalendarReader sut;
      IList<WorkReportItemDTO> sut_List;

      //Act
      sut = new ExchangeCalendarReaderMock();
      sut_List = sut
        .SetAppointmentSubject("HAYDN")
        .SetInterval(new DateTime(2015, 01, 01), new DateTime(2015, 12, 31))
        .RetrieveAppointments();

      //Assert
      Assert.That(sut_List, Is.Not.Null);
      Assert.That(sut_List.Count, Is.EqualTo(4));
      Assert.That(sut_List[0].CustomerName, Is.EqualTo("HAYDN"));
    }



  } //end   class ExchangeCalendarReader_UnitTests

} //end namespace Stundenlisten.Tests.Services

