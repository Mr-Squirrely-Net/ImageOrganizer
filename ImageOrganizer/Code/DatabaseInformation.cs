using System.Collections.Generic;

namespace ImageOrganizer.Code {
    public class DatabaseInformation {

        public string? Name { get; set; }
        public ulong? Hash { get; set; }
        public string? Directory { get; set; }
        public bool? IsDirectory { get; set; }
        public bool? IsComic { get; set; }
        public int? PageNumber { get; set; }
        public List<string>? Tags { get; set; }


    }
}
