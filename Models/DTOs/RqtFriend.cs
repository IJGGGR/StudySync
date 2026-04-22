using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudySync.Models.DTOs
{
    public class RqtFriend
    {
        public required string UsernameSelf { get; set; }
        public required string UsernameThem { get; set; }
    }
}
