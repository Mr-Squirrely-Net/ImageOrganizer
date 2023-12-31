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
using Image = System.Drawing.Image;
using ScrollViewer = HandyControl.Controls.ScrollViewer;

namespace ImageOrganizer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {

        private static readonly List<CardModel> ImageCards = new();
        private static readonly List<CardModel> DirectoryCards = new();
        private static readonly LinkedList<CardModel> Cards = new();
        private static ILiteCollection<DatabaseInformation>? Images;

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
            //DirectoryInfo info = new(directory);
            //Images = Reference.Database.GetCollection<DatabaseInformation>(info.Name);

            //foreach (DatabaseInformation dbInfo in Images.FindAll()) {
            //    CardModel cardModel = dbInfo.IsDirectory == true
            //        ? new CardModel {
            //            Header = dbInfo.Name,
            //            Footer = "Directory",
            //            Content = new BitmapImage(new Uri($"{Environment.CurrentDirectory}/folder_icon.png")),
            //            IsDirectory = true,
            //            IsComic = false
            //        }
            //        : new CardModel {
            //            Header = dbInfo.Name,
            //            Footer = Sizer.SuffixName($"{dbInfo.Directory}/{dbInfo.Name}"),
            //            Content = new BitmapImage(new Uri($"{dbInfo.Directory}/{dbInfo.Name}")),
            //            IsDirectory = false,
            //            IsComic = false
            //        };

            //    switch (dbInfo.IsDirectory) {
            //        case true:
            //            DirectoryCards.Add(cardModel);
            //            break;
            //        case false:
            //            ImageCards.Add(cardModel);
            //            break;
            //        case null:
            //            throw new NullReferenceException("You Done Messed Up! It's Null!");
            //    }
            //}

            //foreach (CardModel directoryCard in DirectoryCards) {
            //    Cards.AddLast(directoryCard);
            //}

            //foreach (CardModel imageCard in ImageCards) {
            //    Cards.AddLast(imageCard);
            //}
            DirectoryInfo info = new(directory);
            //Images = Reference.Database.GetCollection<DatabaseInformation>(info.Name);
            ImagePanel.ItemsSource = Reference.Database.GetCollection<DatabaseInformation>(info.Name).FindAll().ToList();
            ImagePanel.UpdateLayout();
            //ImagePanel.SelectedIndex = 0;
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
            DatabaseInformation info = (DatabaseInformation)((ListBox)sender).SelectedItem;
            InfoDrawer.IsOpen = !InfoDrawer.IsOpen;
            InfoDrawerGrid.Children.Clear();
            InfoDrawerGrid.Children.Add(
                new ItemInfo {
                    TempName = {
                        Content = $"{info.Name}"
                    }
                });
        }

        private void HandelClick(CardModel model, bool doubleClick = false) {
            if (doubleClick) {
                Debug.WriteLine($"Double Click {model.Content}");
            }
            else {
                InfoDrawer.IsOpen = !InfoDrawer.IsOpen;
                InfoDrawerGrid.Children.Clear();
                InfoDrawerGrid.Children.Add(
                    new ItemInfo {
                        TempName = {
                            Content = $"{model.Header}"
                        }
                    });
            }
        }

        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
           // Debug.WriteLine($"Double Clicked {Model.Content}");
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e) {
            //if(Model == null) return;

            Debug.WriteLine(ImagePanel.SelectedItem);
            Debug.WriteLine(ImagePanel.SelectedIndex);

            InfoDrawer.IsOpen = !InfoDrawer.IsOpen;
            InfoDrawerGrid.Children.Clear();
            CardModel model = (CardModel)ImagePanel.SelectedItem;
            InfoDrawerGrid.Children.Add(
                new ItemInfo {
                    TempName = {
                        Content = $"{model.Header}"
                    }
                });
            Debug.WriteLine(ImagePanel.SelectedIndex);
        }
    }
}
