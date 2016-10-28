namespace TimeSheet
{
  using System.Reflection;
  using System.Windows;
  using Nett;

  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    public AppConfiguration.CurrentConfiguration _currentConfiguration;
    
    /// <summary>
    /// Initialisierung der ganzen wpf geschichte und einlesen der AppConfiguration.private.toml und
    /// deserialiserung in das Objekt _currentConfiguration
    /// </summary>
    public App()
    {
      AssemblyName myAppName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
      _currentConfiguration = Toml.ReadFile<AppConfiguration.CurrentConfiguration>($"{myAppName.Name}.AppConfiguration.private.toml");
      InitializeComponent();
    }


  } //end   public partial class App : Application


} //end namespace TimeSheet

