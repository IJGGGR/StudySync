using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudySync.Models.DTOs
{
    public class DtoSaltHash
    {
        public string? Salt { get; set; }
        public string? Hash { get; set; }
    }
}
