using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using DuplaImage.Lib;
using DuplaImage.Lib.ImageMagick;

using LiteDB;

namespace ImageOrganizer.Code {
    internal static class Reference {

        //Todo: add more image types
        public static List<string> ImageTypes = new() { ".jpeg", ".jpg", ".gif", ".png", ".webp" };
        internal static LiteDatabase Database = new($"{Environment.CurrentDirectory}/Main.db");
        //internal static ILiteCollection<Settings> SettingsCollection = Database.GetCollection<Settings>("settings");
        internal static ImageHashes ImageHasher = new(new ImageMagickTransformer());
		
        public static bool IsImage(this FileInfo info) { return ImageTypes.Contains(info.Extension); }
		
        internal static bool ScanForImages(string directory) {
			try {
                DirectoryInfo directoryInfo = new(directory);

				foreach (DirectoryInfo enumerateDirectory in directoryInfo.EnumerateDirectories()) {
					DatabaseInformation dbInformation = new() {
						Name = enumerateDirectory.Name,
						Directory = enumerateDirectory.FullName,
						IsComic = false,
						IsDirectory = true
                    };
                    _ = AddToCollection(directoryInfo.Name, enumerateDirectory.Name, dbInformation);
                    ScanForImages(enumerateDirectory.FullName);
                }

                foreach (FileInfo enumerateFile in directoryInfo.EnumerateFiles()) {
					if (!enumerateFile.IsImage()) continue;
					DatabaseInformation dbInformation = new() {
						Name = enumerateFile.Name,
						Directory = enumerateFile.DirectoryName,
						Hash = ImageHasher.CalculateDctHash(enumerateFile.FullName),
						IsComic = false
					};
					_ = AddToCollection(directoryInfo.Name, enumerateFile.Name, dbInformation);
				}
				return true;
            }
			catch (LiteException e) {
                Debug.WriteLine(e.Message);
				return false;
			}
		}

		private static bool AddToCollection(string collectionName, string itemID, DatabaseInformation databaseInformation) => 
            Database.GetCollection<DatabaseInformation>(collectionName).Upsert(itemID, databaseInformation);
    }
}
