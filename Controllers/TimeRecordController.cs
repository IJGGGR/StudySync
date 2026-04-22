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
        // public ActionResult<IEnumerable<MdlTimeRecord>> GetAllByDate(string date)
        // {
        //     return Ok(_svc.GetAllByDate(date));
        // }

        [HttpGet("GetAllByCategory/{Category}")]
        public ActionResult<IEnumerable<MdlTimeRecord>> GetAllByCategory(string Category)
        {
            return Ok(_svc.GetAllByCategory(Category));
        }

        [HttpGet("GetAllByIsProductive/{IsProductive}")]
        public ActionResult<IEnumerable<MdlTimeRecord>> GetAllByIsProductive(bool IsProductive)
        {
            return Ok(_svc.GetAllByIsProductive(IsProductive));
        }

        [HttpPost("Create")]
        public ActionResult<MdlTimeRecord> Create([FromBody] RqtTimeRecord rqt)
        {
            return _svc.Create(rqt);
        }

        // [HttpPut("Update")]
        // public ActionResult<MdlTimeRecord> Update([FromBody] RqtTimeRecord rqt)
        // {
        //     return _svc.Update(rqt);
        // }

        // [HttpDelete("Delete")]
        // public ActionResult Delete([FromBody] RqtTimeRecord rqt)
        // {
        //     return _svc.Delete(rqt);
        // }
    }
}
