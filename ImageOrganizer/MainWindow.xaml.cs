using System.Diagnostics;
using System.IO;
using System.IO.Compression;
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

using LiteDB;
using static System.Windows.Application;

namespace ImageOrganizer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        public MainWindow() {
            InitializeComponent();

            Current.Exit += delegate(object sender, ExitEventArgs args) {
                foreach (LiteDatabase liteDatabase in Reference._databaseCollection) {
                    liteDatabase.Dispose();
                }
            };

            if (Reference.Properties.FirstRun) {
                SettingsWindow settingsWindow = new();
                settingsWindow.Show();
            }
            else {
                Reference._directoryCollection.Add(Reference.Properties.OrganizerDirectory);
                ScanForDirectory(Reference.Properties.OrganizerDirectory);
                
                foreach (string enumerateDirectory in Reference._directoryCollection) {
                    ScanForImages(enumerateDirectory);
                }

                //foreach (string enumerateDirectory in Directory.EnumerateDirectories(Reference.Properties.OrganizerDirectory)) {
                //    Reference._directoryCollection.Add(enumerateDirectory);
                //}

                //foreach (string directory in Reference._directoryCollection) {
                //    Reference._databaseCollection.Add(new LiteDatabase($"{directory}/images.db"));
                //}
            }
        }

        private static void ScanForDirectory(string directory) {
            foreach (string enumerateDirectory in Directory.EnumerateDirectories(directory)) {
                Reference._directoryCollection.Add(enumerateDirectory);
                ScanForDirectory(enumerateDirectory);
            }
        }

        private static void ScanForImages(string directory) {
            using LiteDatabase database = new($"{directory}/images.db");
            ILiteCollection<ImageData>? col = database.GetCollection<ImageData>("imageData");
            DirectoryInfo dirInfo = new(directory);
            foreach (FileInfo enumerateFile in dirInfo.EnumerateFiles()) {
                if (enumerateFile.Extension is not (".jpg" or ".png" or ".jpeg" or ".webp")) continue;
                ImageData data = new() {
                    Name = enumerateFile.Name,
                    Location = enumerateFile.DirectoryName,
                    IsDuplicate = false,
                    Resolution = "test res",
                    Size = 0
                };
                
                col.Upsert(enumerateFile.Name ,data);
            }
        }

        private void TitleSearchBar_OnSearchStarted(object? sender, FunctionEventArgs<string> e) {
            Debug.WriteLine(e.Info);
        }
        
    }
}