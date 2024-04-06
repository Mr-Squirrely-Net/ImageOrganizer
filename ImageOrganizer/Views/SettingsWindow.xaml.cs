using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using ImageOrganizer.Code;
using WinRT.Interop;
using System.Diagnostics;

namespace ImageOrganizer.Views {
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow {

        private string MainDirectoryString { get; set; }
        private string TempDirectoryString { get; set; }

        public SettingsWindow() {
            InitializeComponent();
            //PropertyGrid.SelectedObject = Reference.Properties;
            MainDirectoryString = Reference.Properties.OrganizerDirectory;
            TempDirectoryString = Reference.Properties.OrganizerDirectory;
            UpdateText();
        }

        private void UpdateText() {
            MainDirectory.Text = MainDirectoryString;
            TempDirectory.Text = TempDirectoryString;
        }

        private async void BrowseMainFolder_Click(object sender, RoutedEventArgs e) {
            StorageFolder folder = await GetFolder();
            if (folder == null) return;
            MainDirectoryString = folder.Path;
            UpdateText();
        }

        private async void BrowseTempFolder_Click(object sender, RoutedEventArgs e) {
            StorageFolder folder = await GetFolder();
            if (folder == null) return;
            TempDirectoryString = folder.Path;
            UpdateText();
        }

        private IAsyncOperation<StorageFolder> GetFolder() {
            FolderPicker picker = new() {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                FileTypeFilter = { "*" }
            };
            InitializeWithWindow.Initialize(picker, this.GetHandle());
            return picker.PickSingleFolderAsync();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e) {
            Reference.Properties.OrganizerDirectory = MainDirectoryString;
            Reference.Properties.TempDirectory = TempDirectoryString;
            Reference.Properties.FirstRun = false;
            Reference.Properties.Save();
        }
    }
}
