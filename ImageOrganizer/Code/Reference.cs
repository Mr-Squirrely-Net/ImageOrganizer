using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Interop;

using DuplaImage.Lib;
using DuplaImage.Lib.ImageMagick;
using HandyControl.Controls;
using ImageMagick;
using LiteDB;
using Microsoft.Win32;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ImageOrganizer.Code {
    public static class Reference {

        internal static ImageHashes ImageHasher = new(new ImageMagickTransformer());
        internal static Settings Properties = JsonSerializer.Deserialize<Settings>(new StreamReader("settings.json").ReadToEnd()) ?? throw new InvalidOperationException();

        internal static ObservableCollection<string> _directoryCollection = new();
        internal static ObservableCollection<LiteDatabase> _databaseCollection = new();

        public static ulong GetHash(string image) => ImageHasher.CalculateDctHash(image);
        
        public static bool IsSimilarTo(this ParsedImage value, ParsedImage toCompare) => ImageHasher.CompareHashes(value.ImageHash, toCompare.ImageHash) == 1.0f;

        internal static void Save(this Settings value) => File.WriteAllText("settings.json", JsonSerializer.Serialize(value));
        internal static IntPtr GetHandle(this Window window) => new WindowInteropHelper(window ?? throw new InvalidOperationException()).Handle;
    }
}
