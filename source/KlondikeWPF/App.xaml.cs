using Data;
using Logic;
using System.Windows;

namespace KlondikeWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            new MainWindow(new CardLogic(new CardData())).Show();
        }
    }
}
