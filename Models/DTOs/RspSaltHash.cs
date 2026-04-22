using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudySync.Models.DTOs
{
    public class RspSaltHash
    {
        public required string Salt { get; set; }
        public required string Hash { get; set; }
    }
}
