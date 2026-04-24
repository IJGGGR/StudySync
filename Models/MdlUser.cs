using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudySync.Models
{
    public class MdlUser
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Salt { get; set; }
        public required string Hash { get; set; }
        public int Points { get; set; }
        public int[] OutgoingRequests { get; set; } = [];
        public int[] IncomingRequests { get; set; } = [];
        public int[] Friends { get; set; } = [];
    }
}
