using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using DuplaImage.Lib;
using DuplaImage.Lib.ImageMagick;

using LiteDB;
using Microsoft.Toolkit.Uwp.Notifications;
using SquirrelUtils.Sizer;

namespace ImageOrganizer.Code {
    public static class Reference {

        //Todo: add more image types
        internal static List<string> ImageTypes = new() { ".jpeg", ".jpg", ".gif", ".png", ".webp" };
        internal static LiteDatabase Database = new($"{Environment.CurrentDirectory}/Main.db");
        //internal static ILiteCollection<Settings> SettingsCollection = Database.GetCollection<Settings>("settings");
        internal static ImageHashes ImageHasher = new(new ImageMagickTransformer());

        internal static string Title(string dirName) => $"Image Organizer : {dirName}";

        public static bool IsImage(this FileInfo info) => ImageTypes.Contains(info.Extension);

        public delegate void FinishedMessage(string message);

        internal static void ScanDirectory(string directory) {
            DirectoryScanning scanning = new(directory, new FinishedMessage(MessageCallback));
            Thread scanThread = new(new ThreadStart(scanning.ScanDirectory));
            scanThread.Start();
        }

        public static void MessageCallback(string message) => new ToastContentBuilder().AddText("Finished").AddText(message).Show();

        internal static bool AddToCollection(string collectionName, string itemID, DatabaseInformation databaseInformation) => 
            Database.GetCollection<DatabaseInformation>(collectionName).Upsert(itemID, databaseInformation);
    }
}
