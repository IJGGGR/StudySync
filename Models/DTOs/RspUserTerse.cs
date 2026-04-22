using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudySync.Models.DTOs
{
    public class RspUserTerse
    {
        public int Id { get; set; }
        public required string Username { get; set; }
    }
}
