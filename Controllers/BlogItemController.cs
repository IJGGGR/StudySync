// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using StudySync.Models;
// using StudySync.Services;

// namespace StudySync.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class BlogItemController(SvcBlogItem svc) : ControllerBase
//     {
//         private readonly SvcBlogItem _svc = svc;

//         [HttpGet("GetAll")]
//         public ActionResult<IEnumerable<MdlBlogItem>> GetAll()
//         {
//             return Ok(_svc.GetAll());
//         }

//         [HttpGet("GetAllByDate/{date}")]
//         public ActionResult<IEnumerable<MdlBlogItem>> GetAllByDate(string date)
//         {
//             return Ok(_svc.GetAllByDate(date));
//         }

//         [HttpGet("GetAllByCategory/{category}")]
//         public ActionResult<IEnumerable<MdlBlogItem>> GetAllByCategory(string category)
//         {
//             return Ok(_svc.GetAllByCategory(category));
//         }

//         [HttpGet("GetAllByTag/{tag}")]
//         public ActionResult<IEnumerable<MdlBlogItem>> GetAllByTag(string tag)
//         {
//             return Ok(_svc.GetAllByTag(tag));
//         }

//         [HttpGet("GetAllByIsPublished")]
//         public ActionResult<IEnumerable<MdlBlogItem>> GetAllByIsPublished()
//         {
//             return Ok(_svc.GetAllByIsPublished());
//         }

//         [HttpGet("GetOneById/{id}")]
//         public ActionResult<MdlBlogItem> GetOneById(int id)
//         {
//             return _svc.Wrapper(_svc.GetOneById(id));
//         }

//         [HttpPost("Create")]
//         public ActionResult<MdlBlogItem> Create(
//             [FromBody]
//             MdlBlogItem mdl
//         )
//         {
//             return _svc.Create(mdl);
//         }

//         [HttpPut("Update")]
//         public ActionResult<MdlBlogItem> Update(
//             [FromBody]
//             MdlBlogItem mdl
//         )
//         {
//             return _svc.Update(mdl);
//         }

//         [HttpDelete("Delete")]
//         public ActionResult Delete(
//             [FromBody]
//             MdlBlogItem mdl
//         )
//         {
//             return _svc.Delete(mdl);
//         }
//     }
// }
