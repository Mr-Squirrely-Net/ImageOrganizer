using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.Storage.Pickers;
using Windows.Storage;
using DuplaImage.Lib;
using DuplaImage.Lib.ImageMagick;
using HandyControl.Controls;
using ImageOrganizer.Code;
using LiteDB;
using SixLabors.ImageSharp.Processing;
using WinRT.Interop;

namespace ImageOrganizer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        public MainWindow() {
            InitializeComponent();
        }

        private async void BrowseButton_OnClick(object sender, RoutedEventArgs e) {
            FolderPicker folderPicker = new() {
                SuggestedStartLocation = PickerLocationId.Desktop,
                FileTypeFilter = { "*" }
            };

            InitializeWithWindow.Initialize(folderPicker, App.Hwnd);

            StorageFolder storageFolder = await folderPicker.PickSingleFolderAsync();

            DirLabel.Content = storageFolder != null ? $"{storageFolder.Path}" : "Canceled";

        }

        private void ScanButton_OnClick(object sender, RoutedEventArgs e) {
            _ = Reference.ScanForImages((string)DirLabel.Content);
        }

        private void PopulateButton_OnClick(object sender, RoutedEventArgs e) {
            DirectoryInfo info = new((string)DirLabel.Content);
            ILiteCollection<DatabaseInformation> images = Reference.Database.GetCollection<DatabaseInformation>(info.Name);

            //int width = 0;
            //int flexOrder = 0;

            //UniformSpacingPanel panel = new();

            foreach (DatabaseInformation dbInfo in images.FindAll()) {

                Image image2 = new() {
                    Width = 100,
                    Height = 100,
                    Source = new BitmapImage(new Uri($"{dbInfo.Directory}/{dbInfo.Name}"))
                };

                CoverViewItem item = new() {
                    Header = image2
                };

                CoverPanel.Items.Add(item);

                //if (width == 6) {
                //	FlexPanel.SetOrder(panel, flexOrder);
                //	ImagePanel.Children.Add(panel);
                //	panel = new UniformSpacingPanel();
                //	flexOrder++;
                //	width = 0;
                //}
                //System.Windows.Controls.Image image2 = new() {
                //	Width = 100,
                //	Height = 100,
                //	Source = new BitmapImage(new Uri($"{image.Directory}/{image.Name}"))
                //};

                //panel.Children.Add(image2);
                //width++;
            }
        }
    }
}
