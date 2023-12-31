using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace ImageOrganizer.Code {
    public class DatabaseInformation {
        public string? Name { get; set; }
        public Uri? Content { get; set; }
        public string? Footer { get; set; }
        public ulong? Hash { get; set; }
        public string? Directory { get; set; }
        public bool? IsDirectory { get; set; }
        public bool? IsComic { get; set; }
        public int? PageNumber { get; set; }
        public float Rating { get; set; }
        public List<string>? Tags { get; set; }
    }
}