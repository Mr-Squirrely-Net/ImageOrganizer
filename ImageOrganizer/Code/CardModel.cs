using System.Windows.Media.Imaging;

namespace ImageOrganizer.Code {
    public class CardModel {
        public string Header { get; set; }
        public BitmapImage Content { get; set; }
        public string Footer { get; set; }
        public bool IsDirectory { get; set; }
        public bool IsComic { get; set; }
    }

}
