using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace ImageOrganizer {
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
