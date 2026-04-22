// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using StudySync.Contexts;
// using StudySync.Models;

// namespace StudySync.Services
// {
//     public class SvcBlogItem : ControllerBase
//     {
//         private readonly AppDbCtx _ctx;

//         public SvcBlogItem(AppDbCtx ctx)
//         {
//             _ctx = ctx;
//             _ctx.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;
//         }

//         internal ActionResult<T> Wrapper<T>(T? arg)
//         {
//             if (arg == null)
//             {
//                 return NotFound();
//             }

//             return Ok(arg);
//         }

//         internal IEnumerable<MdlBlogItem> GetAll()
//         {
//             var res = _ctx.TblBlogItem;

//             return res;
//         }

//         internal IEnumerable<MdlBlogItem> GetAllByDate(string date)
//         {
//             var res = _ctx.TblBlogItem.Where(e => e.Date == date);

//             return res;
//         }

//         internal IEnumerable<MdlBlogItem> GetAllByCategory(string category)
//         {
//             var res = _ctx.TblBlogItem.Where(e => e.Category == category);

//             return res;
//         }

//         internal IEnumerable<MdlBlogItem> GetAllByTag(string tag)
//         {
//             var res = _ctx.TblBlogItem.Where(e => (e.Tags ?? "").Split(',', StringSplitOptions.None).Any(e => e == tag));

//             return res;
//         }

//         internal IEnumerable<MdlBlogItem> GetAllByIsPublished()
//         {
//             var res = _ctx.TblBlogItem.Where(e => e.IsPublished);

//             return res;
//         }

//         internal MdlBlogItem? GetOneById(int id)
//         {
//             var res = _ctx.TblBlogItem.FirstOrDefault(e => e.Id == id);

//             return res;
//         }

//         internal ActionResult<MdlBlogItem> Create(MdlBlogItem mdl)
//         {
//             mdl.Id = 0;
//             _ctx.Add(mdl);
//             var num = _ctx.SaveChanges();

//             if (num < 1)
//             {
//                 return BadRequest();
//             }

//             return CreatedAtAction(
//                 nameof(GetOneById),
//                 new { id = mdl.Id },
//                 mdl
//             );
//         }

//         internal ActionResult<MdlBlogItem> Update(MdlBlogItem mdl)
//         {
//             var ent = GetOneById(mdl.Id);

//             if (ent == null)
//             {
//                 return BadRequest();
//             }

//             // ent.Id = mdl.Id;
//             // ent.UserId = mdl.UserId;
//             ent.PublisherName = mdl.PublisherName;
//             ent.Title = mdl.Title;
//             ent.Image = mdl.Image;
//             ent.Description = mdl.Description;
//             ent.Date = mdl.Date;
//             ent.Category = mdl.Category;
//             ent.Tags = mdl.Tags;
//             ent.IsPublished = mdl.IsPublished;
//             ent.IsDeleted = mdl.IsDeleted;
//             _ctx.Update(ent);
//             var num = _ctx.SaveChanges();

//             if (num < 1)
//             {
//                 return BadRequest();
//             }

//             return Ok(ent);
//         }

//         internal ActionResult Delete(MdlBlogItem mdl)
//         {
//             var ent = GetOneById(mdl.Id);

//             if (ent == null)
//             {
//                 return BadRequest();
//             }

//             if (ent.IsDeleted)
//             {
//                 return BadRequest(); // already "deleted"
//             }

//             ent.IsDeleted = true;
//             _ctx.Update(ent);
//             var num = _ctx.SaveChanges();

//             if (num < 1)
//             {
//                 return BadRequest();
//             }

//             return NoContent();
//         }
//     }
// }
