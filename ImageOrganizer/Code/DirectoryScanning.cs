using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace ImageOrganizer.Code {
    public class DirectoryScanning {

        private readonly string _directory;
        private readonly Reference.FinishedMessage _finishedMessage;

        public DirectoryScanning(string directory, Reference.FinishedMessage finishedMessage) {
            _directory = directory;
            _finishedMessage = finishedMessage;
        }

        public void ScanDirectory() {
            try {
                DirectoryInfo directoryInfo = new(_directory);

                foreach (DirectoryInfo enumerateDirectory in directoryInfo.EnumerateDirectories()) {
                    DatabaseInformation dbInformation = new() {
                        Name = enumerateDirectory.Name,
                        Directory = enumerateDirectory.FullName,
                        IsComic = false,
                        IsDirectory = true
                    };
                    _ = Reference.AddToCollection(directoryInfo.Name, enumerateDirectory.Name, dbInformation);
                    Reference.ScanDirectory(enumerateDirectory.FullName);
                }

                foreach (FileInfo enumerateFile in directoryInfo.EnumerateFiles()) {
                    if (!enumerateFile.IsImage()) continue;
                    DatabaseInformation dbInformation = new() {
                        Name = enumerateFile.Name,
                        Directory = enumerateFile.DirectoryName,
                        IsDirectory = false,
                        Hash = Reference.ImageHasher.CalculateDctHash(enumerateFile.FullName),
                        IsComic = false
                    };
                    _ = Reference.AddToCollection(directoryInfo.Name, enumerateFile.Name, dbInformation);
                }

                _finishedMessage("Finished scanning images");
            } catch (LiteException e) {
                Debug.WriteLine(e.Message);
                _finishedMessage("We had an issue and couldn't finish");

            }
        }




    }
}
