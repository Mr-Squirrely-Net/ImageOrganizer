using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.Storage;
using DuplaImage.Lib;
using DuplaImage.Lib.ImageMagick;
using HandyControl.Controls;
using HandyControl.Data;
using ImageOrganizer.Code;
using ImageOrganizer.Controls;
using LiteDB;
using WinRT.Interop;
using Microsoft.Toolkit.Uwp.Notifications;
using SquirrelUtils.Sizer;
using ScrollViewer = HandyControl.Controls.ScrollViewer;

namespace ImageOrganizer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {

        private static readonly List<CardModel> Cards = new();

        public MainWindow() {
            InitializeComponent();

            ToastNotificationManagerCompat.OnActivated += toastArgs => {
                ToastArguments args = ToastArguments.Parse(toastArgs.Argument);
                ValueSet userInput = toastArgs.UserInput;
                Debug.WriteLine(toastArgs.Argument);
                Debug.WriteLine(userInput);
            };

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
            Reference.ScanDirectory((string)DirLabel.Content);
        }

        private void PopulateButton_OnClick(object sender, RoutedEventArgs e) {
            PopulateView((string)DirLabel.Content);
        }

        private void PopulateView(string directory) {
            DirectoryInfo info = new(directory);
            ILiteCollection<DatabaseInformation> images = Reference.Database.GetCollection<DatabaseInformation>(info.Name);

            foreach (DatabaseInformation dbInfo in images.FindAll()) {
                CardModel cardModel = dbInfo.IsDirectory == true
                    ? new CardModel {
                        Header = dbInfo.Name,
                        Footer = "Directory",
                        Content = new BitmapImage(new Uri($"{Environment.CurrentDirectory}/folder_icon.png")),
                        IsDirectory = true,
                        IsComic = false
                    }
                    : new CardModel {
                        Header = dbInfo.Name,
                        Footer = Sizer.SuffixName($"{dbInfo.Directory}/{dbInfo.Name}"),
                        Content = new BitmapImage(new Uri($"{dbInfo.Directory}/{dbInfo.Name}")),
                        IsDirectory = false,
                        IsComic = false
                    };
                Cards.Add(cardModel);
            }

            ImagePanel.ItemsSource = Cards;
            ImagePanel.UpdateLayout();
        }

        private void MainWindow_OnClosing(object? sender, CancelEventArgs e) {
            Reference.Database.Dispose();
            ToastNotificationManagerCompat.Uninstall();
        }

        private void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e) {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffsetWithAnimation(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void ImagePanel_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            InfoDrawer.IsOpen = !InfoDrawer.IsOpen;
            InfoDrawerGrid.Children.Clear();
            InfoDrawerGrid.Children.Add(
                new ItemInfo {
                    TempName = {
                        Content = $"{((CardModel)((ListBox)sender).SelectedItem).Header}"
                    }
                });
        }
    }
}
