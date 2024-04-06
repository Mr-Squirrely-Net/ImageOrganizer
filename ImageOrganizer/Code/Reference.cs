using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Interop;

using DuplaImage.Lib;
using DuplaImage.Lib.ImageMagick;
using HandyControl.Controls;
using ImageMagick;
using Microsoft.Win32;

namespace ImageOrganizer.Code {
    public static class Reference {

        internal static ImageHashes ImageHasher = new(new ImageMagickTransformer());
        internal static Settings Properties = JsonSerializer.Deserialize<Settings>(new StreamReader("settings.json").ReadToEnd()) ?? throw new InvalidOperationException();

        public static ulong GetHash(string image) => ImageHasher.CalculateDctHash(image);
        
        public static bool IsSimilarTo(this ParsedImage value, ParsedImage toCompare) => ImageHasher.CompareHashes(value.ImageHash, toCompare.ImageHash) == 1.0f;

        internal static void Save(this Settings value) => File.WriteAllText("settings.json", JsonSerializer.Serialize(value));
        internal static IntPtr GetHandle(this Window window) => new WindowInteropHelper(window ?? throw new InvalidOperationException()).Handle;
    }
}
