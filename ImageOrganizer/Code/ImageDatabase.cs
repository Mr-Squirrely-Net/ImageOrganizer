using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace ImageOrganizer.Code {
    internal class ImageDatabase {

        private readonly LiteDatabase? _db;
        private readonly ILiteCollection<ParsedImage> _images;

        internal ImageDatabase(string databaseLocation) {
            _db = new LiteDatabase(databaseLocation);
            _images = _db.GetCollection<ParsedImage>("images");
        }

        internal void AddImage(ParsedImage parsedImage) => _ = _images.Insert(parsedImage);

    }
}
