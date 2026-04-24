using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudySync.Contexts;
using StudySync.Models;
using StudySync.Models.DTOs;

namespace StudySync.Services
{
    public class SvcTimeRecord : ControllerBase
    {
        private readonly AppDbCtx _ctx;

        public SvcTimeRecord(AppDbCtx ctx)
        {
            _ctx = ctx;
            _ctx.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;
        }

        // GET =============================================================================================

        // TODO: create a complex getter so that i don't have a bunch of disjointed ones

        internal IEnumerable<MdlTimeRecord> GetAll()
        {
            var res = _ctx.TblTimeRecord;

            return res;
        }

        internal MdlTimeRecord? GetOneById(int id)
        {
            if (id < 1)
            {
                throw new ArgumentException("Invalid Id.");
            }

            var res = _ctx.TblTimeRecord.FirstOrDefault(e => e.Id == id);

            return res;
        }

        internal IEnumerable<MdlTimeRecord> GetAllByUserId(int id)
        {
            var res = _ctx.TblTimeRecord.Where(e => e.UserId == id);

            return res;
        }

        // internal IEnumerable<MdlBlogItem> GetAllByDate(string date)
        // {
        //     var res = _ctx.TblBlogItem.Where(e => e.Date == date);

        //     return res;
        // }

        internal IEnumerable<MdlTimeRecord> GetAllByCategory(string? category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentException("Invalid category.");
            }

            // category = category.Trim().ToLower();

            // if (string.IsNullOrWhiteSpace(category))
            // {
            //     return [];
            // }

            var res = _ctx.TblTimeRecord.Where(e => e.Category == category);

            return res;
        }

        internal IEnumerable<MdlTimeRecord> GetAllByTag(string? tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                throw new ArgumentException("Invalid tag.");
            }

            // category = category.Trim().ToLower();

            // if (string.IsNullOrWhiteSpace(category))
            // {
            //     return [];
            // }

            var res = _ctx.TblTimeRecord.Where(e => e.Tags.Any(e => e == tag));

            return res;
        }

        internal IEnumerable<MdlTimeRecord> GetAllByIsProductive(bool b)
        {
            var res = _ctx.TblTimeRecord.Where(e => e.IsProductive == b);

            return res;
        }

        // SET =============================================================================================

        internal MdlTimeRecord Create(RqtTimeRecord rqt)
        {
            var obj = new MdlTimeRecord(rqt);
            _ctx.TblTimeRecord.Add(obj);
            var num = _ctx.SaveChanges();

            if (num != 1)
            {
                throw new Exception($"DATABASE SAVE ERROR. {num} OUT OF 1 CHANGES WERE SAVED.");
            }

            return obj;
        }

        internal MdlTimeRecord Update(int id, RqtTimeRecord rqt)
        {
            var ent = GetOneById(id);

            if (ent == null)
            {
                throw new KeyNotFoundException($"Id '{id}' not found.");
            }

            ent.Update(rqt);
            _ctx.TblTimeRecord.Update(ent);
            var num = _ctx.SaveChanges();

            if (num != 1)
            {
                throw new Exception($"DATABASE SAVE ERROR. {num} OUT OF 1 CHANGES WERE SAVED.");
            }

            return ent;
        }

        internal bool Delete(int id)
        {
            var ent = GetOneById(id);

            if (ent == null)
            {
                throw new KeyNotFoundException($"Id '{id}' not found.");
            }

            if (ent.IsDeleted)
            {
                return false;
            }

            ent.IsDeleted = true;
            _ctx.TblTimeRecord.Update(ent);
            var num = _ctx.SaveChanges();

            if (num != 1)
            {
                throw new Exception($"DATABASE SAVE ERROR. {num} OUT OF 1 CHANGES WERE SAVED.");
            }

            return true;
        }
    }
}
