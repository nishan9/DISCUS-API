using System;
using Newtonsoft.Json;

namespace DISCUS_API.Models
{
    public class SchoolDepartment
    {
        [JsonProperty("user_metadata")]
        public EduMetada User_metadata { get; set; }
    }
    public class EduMetada
    {
        [JsonProperty("education")]
        public DepartSchool Education { get; set; }

    }
    public class DepartSchool
    {
        [JsonProperty("school")]
        public string School { get; set; }

        [JsonProperty("Department")]
        public string Department { get; set; }

    }

}
