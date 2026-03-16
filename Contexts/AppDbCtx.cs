using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudySync.Models;

namespace StudySync.Contexts
{
    public class AppDbCtx(DbContextOptions<AppDbCtx> opt) : DbContext(opt)
    {
        public DbSet<MdlUser> TblUser { get; set; }
        public DbSet<MdlBlogItem> TblBlogItem { get; set; }
    }
}
