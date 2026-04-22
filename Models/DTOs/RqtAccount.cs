using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudySync.Models.DTOs
{
    public class RqtAccount
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
