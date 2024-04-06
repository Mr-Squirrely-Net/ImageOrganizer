using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HandyControl.Data;
using ImageOrganizer.Code;
using ImageOrganizer.Views;

namespace ImageOrganizer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        public MainWindow() {
            InitializeComponent();

            Debug.WriteLine(Reference.Properties.FirstRun);

            if (Reference.Properties.FirstRun) {
                SettingsWindow settingsWindow = new();
                settingsWindow.Show();
            }
            else {
                
            }
        }

        private void TitleSearchBar_OnSearchStarted(object? sender, FunctionEventArgs<string> e) {
            Debug.WriteLine(e.Info);
        }
        
    }
}