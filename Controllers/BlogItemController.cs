using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudySync.Models;
using StudySync.Services;

namespace StudySync.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogItemController(SvcBlogItem svc) : ControllerBase
    {
        private readonly SvcBlogItem _svc = svc;

        [HttpPost("Create")]
        public bool Create(MdlBlogItem mdl)
        {
            return _svc.Create(mdl);
        }

        [HttpGet("GetAll")]
        public IEnumerable<MdlBlogItem> GetAll()
        {
            return _svc.GetAll();
        }

        [HttpGet("GetAllByCategory")]
        public IEnumerable<MdlBlogItem> GetAllByCategory(string category)
        {
            return _svc.GetAllByCategory(category);
        }

        [HttpGet("GetAllByTag")]
        public IEnumerable<MdlBlogItem> GetAllByTag(string tag)
        {
            return _svc.GetAllByTag(tag);
        }

        [HttpGet("GetAllByDate")]
        public IEnumerable<MdlBlogItem> GetAllByDate(string date)
        {
            return _svc.GetAllByDate(date);
        }

        [HttpGet("GetAllByIsPublished")]
        public IEnumerable<MdlBlogItem> GetAllByIsPublished()
        {
            return _svc.GetAllByIsPublished();
        }

        [HttpPost("Update")]
        public bool Update(MdlBlogItem mdl)
        {
            return _svc.Update(mdl);
        }

        [HttpPost("Delete")]
        public bool Delete(MdlBlogItem mdl)
        {
            return _svc.Delete(mdl);
        }
    }
}
