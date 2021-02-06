using System;
using System.Collections.Generic;

namespace DISCUS_API.Models
{
    public class TagSystem
    {
        public List<SubjectType> Subject { get; set; }
    }

    public class SubjectType
    {
        public string Subject { get; set; }
        public dynamic Children { get; set; }
    }
}
