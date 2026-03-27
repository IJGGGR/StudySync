using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudySync.Models;
using StudySync.Models.DTOs;
using StudySync.Services;

namespace StudySync.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimeRecordController(SvcTimeRecord svc) : ControllerBase
    {
        private readonly SvcTimeRecord _svc = svc;

        [HttpGet("GetAll")]
        public ActionResult<IEnumerable<MdlTimeRecord>> GetAll()
        {
            return Ok(_svc.GetAll());
        }

        [HttpGet("GetOneById/{Id}")]
        public ActionResult<MdlTimeRecord> GetOneById(int Id)
        {
            return _svc.Wrapper(_svc.GetOneById(Id));
        }

        [HttpGet("GetAllByUserId/{Id}")]
        public ActionResult<MdlTimeRecord> GetAllByUserId(int Id)
        {
            return Ok(_svc.GetAllByUserId(Id));
        }

        // [HttpGet("GetAllByDate/{date}")]
        // public ActionResult<IEnumerable<MdlBlogItem>> GetAllByDate(string date)
        // {
        //     return Ok(_svc.GetAllByDate(date));
        // }

        [HttpGet("GetAllByCategory/{Category}")]
        public ActionResult<IEnumerable<MdlTimeRecord>> GetAllByCategory(string Category)
        {
            return Ok(_svc.GetAllByCategory(Category));
        }

        [HttpGet("GetAllByIsProductive/{IsProductive}")]
        public ActionResult<IEnumerable<MdlBlogItem>> GetAllByIsProductive(bool IsProductive)
        {
            return Ok(_svc.GetAllByIsProductive(IsProductive));
        }

        [HttpPost("Create")]
        public ActionResult<MdlTimeRecord> Create(
            [FromBody]
            DtoTimeRecord dto
        )
        {
            return _svc.Create(dto);
        }

        // [HttpPut("Update")]
        // public ActionResult<MdlTimeRecord> Update(
        //     [FromBody]
        //     MdlTimeRecord mdl
        // )
        // {
        //     return _svc.Update(mdl);
        // }

        // [HttpDelete("Delete")]
        // public ActionResult Delete(
        //     [FromBody]
        //     MdlTimeRecord mdl
        // )
        // {
        //     return _svc.Delete(mdl);
        // }
    }
}
