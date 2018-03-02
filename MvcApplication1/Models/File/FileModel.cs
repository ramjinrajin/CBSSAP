using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models.File
{
    public class FileModel
    {
        public int FileId { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

    }

    public class PropertiesViewModel
    {
        public int RequestId { get; set; }
    }
}