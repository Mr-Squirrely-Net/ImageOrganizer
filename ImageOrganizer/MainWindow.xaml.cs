using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using LiteDB;
using WinRT.Interop;
using Microsoft.Toolkit.Uwp.Notifications;
using SquirrelUtils.Sizer;

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



            //new ToastContentBuilder().AddArgument("action", "viewConversation").AddArgument("conversationID", 9813)
            //    .AddText("Andrew sent you a picture").AddText("Check this out, The Enchantments in Washington").Show();

            //Notification.Show(new Controls.AppNotification(), ShowAnimation.Fade,true);

            //PopupWindow popup = new() {
            //    MinWidth = 400,
            //    Title = "Title",
            //    WindowStartupLocation = WindowStartupLocation.CenterScreen,
            //    AllowsTransparency = true,
            //    WindowStyle = WindowStyle.None
            //};
            //HandyControl.Controls.TextBox text = new();
            //popup.PopupElement = text;
            //popup.ShowDialog();

            DirectoryInfo info = new((string)DirLabel.Content);
            ILiteCollection<DatabaseInformation> images = Reference.Database.GetCollection<DatabaseInformation>(info.Name);

            //int width = 0;
            //int flexOrder = 0;

            //UniformSpacingPanel panel = new();
            
            foreach (DatabaseInformation dbInfo in images.FindAll()) {
                Debug.WriteLine(dbInfo.Name);
                Debug.WriteLine(dbInfo.IsDirectory);

                if (dbInfo.IsDirectory != false) continue;
                
                CardModel cardModel = new() {
                    Header = dbInfo.Name,
                    Footer = Sizer.SuffixName($"{dbInfo.Directory}/{dbInfo.Name}"),
                    Content = new BitmapImage(new Uri($"{dbInfo.Directory}/{dbInfo.Name}"))
                };
                Cards.Add(cardModel);
            }

            ImagePanel.ItemsSource = Cards;
            ImagePanel.UpdateLayout();

            //DirectoryInfo info = new((string)DirLabel.Content);
            //ILiteCollection<DatabaseInformation> images = Reference.Database.GetCollection<DatabaseInformation>(info.Name);

            ////int width = 0;
            ////int flexOrder = 0;

            ////UniformSpacingPanel panel = new();

            //foreach (DatabaseInformation dbInfo in images.FindAll()) {

            //    Image image2 = new() {
            //        Width = 100,
            //        Height = 100,
            //        Source = new BitmapImage(new Uri($"{dbInfo.Directory}/{dbInfo.Name}"))
            //    };

            //    CoverViewItem item = new() {
            //        Header = image2
            //    };

            //    CoverPanel.Items.Add(item);
            //}
        }

        private void MainWindow_OnClosing(object? sender, CancelEventArgs e) {
            Reference.Database.Dispose();
            ToastNotificationManagerCompat.Uninstall();
        }
    }
}
