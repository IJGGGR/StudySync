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

        internal ActionResult<T> Wrapper<T>(T? arg)
        {
            if (arg == null)
            {
                return NotFound();
            }

            return Ok(arg);
        }

        internal IEnumerable<MdlTimeRecord> GetAll()
        {
            var res = _ctx.TblTimeRecord;

            return res;
        }

        internal MdlTimeRecord? GetOneById(int id)
        {
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
                return [];
            }

            // category = category.Trim().ToLower();

            // if (string.IsNullOrWhiteSpace(category))
            // {
            //     return [];
            // }

            var res = _ctx.TblTimeRecord.Where(e => e.Category == category);

            return res;
        }

        internal IEnumerable<MdlTimeRecord> GetAllByIsProductive(bool b)
        {
            var res = _ctx.TblTimeRecord.Where(e => e.IsProductive == b);

            return res;
        }

        // SET =============================================================================================

        internal ActionResult<MdlTimeRecord> Create(RqtTimeRecord rqt)
        {
            var obj = new MdlTimeRecord {
                Id           = 0,
                UserId       = rqt.UserId,
                Started      = rqt.Started,
                Stopped      = rqt.Stopped,
                Length       = rqt.Stopped - rqt.Started,
                Category     = rqt.Category,
                IsProductive = rqt.IsProductive
            };
            _ctx.Add(obj);
            var num = _ctx.SaveChanges();

            if (num < 1)
            {
                return BadRequest();
            }

            return CreatedAtAction(
                nameof(GetOneById), // * this may want the controller endpoint function instead
                new { Id = obj.Id },
                obj
            );
        }

        // internal ActionResult<MdlBlogItem> Update(MdlBlogItem mdl)
        // {
        //     var ent = GetOneById(mdl.Id);

        //     if (ent == null)
        //     {
        //         return BadRequest();
        //     }

        //     // ent.Id = mdl.Id;
        //     // ent.UserId = mdl.UserId;
        //     ent.PublisherName = mdl.PublisherName;
        //     ent.Title = mdl.Title;
        //     ent.Image = mdl.Image;
        //     ent.Description = mdl.Description;
        //     ent.Date = mdl.Date;
        //     ent.Category = mdl.Category;
        //     ent.Tags = mdl.Tags;
        //     ent.IsPublished = mdl.IsPublished;
        //     ent.IsDeleted = mdl.IsDeleted;
        //     _ctx.Update(ent);
        //     var num = _ctx.SaveChanges();

        //     if (num < 1)
        //     {
        //         return BadRequest();
        //     }

        //     return Ok(ent);
        // }

        // internal ActionResult Delete(MdlBlogItem mdl)
        // {
        //     var ent = GetOneById(mdl.Id);

        //     if (ent == null)
        //     {
        //         return BadRequest();
        //     }

        //     if (ent.IsDeleted)
        //     {
        //         return BadRequest(); // already "deleted"
        //     }

        //     ent.IsDeleted = true;
        //     _ctx.Update(ent);
        //     var num = _ctx.SaveChanges();

        //     if (num < 1)
        //     {
        //         return BadRequest();
        //     }

        //     return NoContent();
        // }
    }
}
