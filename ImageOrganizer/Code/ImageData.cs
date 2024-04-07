using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace ImageOrganizer.Code {
    public class ImageData {

        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Resolution { get; set; }
        public long Size { get; set; }
        public bool IsDuplicate { get; set; }

    }
}
