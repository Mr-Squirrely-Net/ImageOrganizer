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
        public static List<string> ImageTypes = new() { ".jpeg", ".jpg", ".gif", ".png", ".webp" };
        public static LiteDatabase Database = new($"{Environment.CurrentDirectory}/Main.db");
        //internal static ILiteCollection<Settings> SettingsCollection = Database.GetCollection<Settings>("settings");
        public static ImageHashes ImageHasher = new(new ImageMagickTransformer());
		
        public static bool IsImage(this FileInfo info) => ImageTypes.Contains(info.Extension);

        public delegate void FinishedMessage(string message);
        
        public static void ScanDirectory(string directory) {
            DirectoryScanning scanning = new(directory, new FinishedMessage(MessageCallback));
            Thread scanThread = new(new ThreadStart(scanning.ScanDirectory));
            scanThread.Start();
        }

        public static void MessageCallback(string message) => new ToastContentBuilder().AddText("Finished").AddText(message).Show();

        public static bool AddToCollection(string collectionName, string itemID, DatabaseInformation databaseInformation) => 
            Database.GetCollection<DatabaseInformation>(collectionName).Upsert(itemID, databaseInformation);
    }
}
