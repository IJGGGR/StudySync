using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudySync.Models.DTOs
{
    public class RspUserVerbose
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public RspUserTerse[] OutgoingRequests { get; set; } = [];
        public RspUserTerse[] IncomingRequests { get; set; } = [];
        public RspUserTerse[] Friends { get; set; } = [];
    }
}
