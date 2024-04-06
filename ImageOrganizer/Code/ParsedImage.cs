using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ImageOrganizer.Code {
    public class ParsedImage {

        public int? Id { get; set; }
        public string? Name { get; set; }
        public Image? Image { get; set; }
        public string? Folder { get; set; }
        public bool? Liked { get; set; }
        public ulong ImageHash { get; set; }
        public ObservableCollection<ParsedImage>? DuplicateImages;
        public ObservableCollection<string>? Tags { get; set; }

        public void AddDuplicate(ParsedImage duplicateImage) {
            DuplicateImages ??= new ObservableCollection<ParsedImage>();
            DuplicateImages.Add(duplicateImage);
        }
        
    }
}
