using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models.Property
{
      public class InzPost
        {
            public int PostId { get; set; }
            public string Title { get; set; }
            public string SubDescription { get; set; }
            public string Category { get; set; }
            public int IsApproved { get; set; }
            public int IsPublic { get; set; }
            public string ReferenceURL { get; set; }
            public int IsDeleted { get; set; }
            public string Description { get; set; }
            public int UserId { get; set; }

            public string FileName { get; set; }
            public List<InzPost> ListPost { get; set; }

            public bool NoPost { get; set; }

            public List<string> Categories { get; set; }


        }
    
}