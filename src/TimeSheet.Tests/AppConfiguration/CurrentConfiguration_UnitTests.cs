namespace TimeSheet.Unit_Tests.AppConfiguration
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using NUnit.Framework;
  using NUnit.Framework.Constraints;
  using TimeSheet.AppConfiguration;

  [TestFixture]
  class CurrentConfiguration_UnitTests
  {

    [Test]
    public void AppConfiguration_Can_Create_Template_Config_File()
    {
      //Arrange
      string templateFilePath = "Stundenlisten.AppConfiguration.template.toml";

      //Act
      CurrentConfiguration.CreateTemplateConfig(templateFilePath);

      //Assert
      Assert.IsTrue(File.Exists(templateFilePath));

    }

  } //end   class CurrentConfiguration_UnitTests


} //end namespace Stundenlisten.Tests.AppConfiguration

