using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudySync.Contexts;
using StudySync.Models;

namespace StudySync.Services
{
    public class SvcBlogItem(AppDbCtx ctx) : ControllerBase
    {
        private readonly AppDbCtx _ctx = ctx;

        internal bool Create(MdlBlogItem mdl)
        {
            _ctx.Add(mdl);
            var res = _ctx.SaveChanges() > 0;

            return res;
        }

        internal IEnumerable<MdlBlogItem> GetAll()
        {
            return _ctx.TblBlogItem;
            // return _ctx.TblBlogItem.AsNoTracking().AsEnumerable();
        }

        internal IEnumerable<MdlBlogItem> GetAllByCategory(string category)
        {
            return _ctx.TblBlogItem.Where(e => e.Category == category);
        }

        internal IEnumerable<MdlBlogItem> GetAllByDate(string date)
        {
            return _ctx.TblBlogItem.Where(e => e.Date == date);
        }

        internal IEnumerable<MdlBlogItem> GetAllByTag(string tag)
        {
            return _ctx.TblBlogItem.Where(e => (e.Tags ?? "").Split(',', StringSplitOptions.None).Any(e => e == tag));
        }

        internal IEnumerable<MdlBlogItem> GetAllByIsPublished()
        {
            return _ctx.TblBlogItem.Where(e => e.IsPublished);
        }

        internal bool Update(MdlBlogItem mdl)
        {
            _ctx.Update(mdl);
            var res = _ctx.SaveChanges() > 0;

            return res;
        }

        internal bool Delete(MdlBlogItem mdl)
        {
            mdl.IsDeleted = true;
            _ctx.Update(mdl);
            var res = _ctx.SaveChanges() > 0;

            return res;
        }
    }
}
