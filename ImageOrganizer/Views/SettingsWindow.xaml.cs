using System.Windows;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using ImageOrganizer.Code;
using WinRT.Interop;

namespace ImageOrganizer {
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
