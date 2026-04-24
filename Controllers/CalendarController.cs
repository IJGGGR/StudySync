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
    public class CalendarController(SvcCalendar svc) : ControllerBase
    {
        public readonly SvcCalendar _svc = svc;

        [HttpGet("GetAll")]
        public ActionResult<IEnumerable<MdlCalendarEvent>> GetAll()
        {
            try
            {
                var res = _svc.GetAll();

                return Ok(res);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
        }

        [HttpGet("GetOneById/{Id}")]
        public ActionResult<MdlCalendarEvent> GetOneById(int Id)
        {
            try
            {
                var ent = _svc.GetOneById(Id);

                if (ent == null)
                {
                    return NotFound($"Id '{Id}' not found.");
                }

                return Ok(ent);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
        }

        [HttpGet("GetAllByUserId/{UserId}")]
        public ActionResult<MdlCalendarEvent> GetAllByUserId(int UserId)
        {
            try
            {
                var ent = _svc.GetAllByUserId(UserId);

                if (ent == null)
                {
                    return NotFound($"UserId '{UserId}' not found.");
                }

                return Ok(ent);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
        }

        [HttpPost("Create")]
        public ActionResult<MdlCalendarEvent> Create([FromBody] RqtCalendarEvent rqt)
        {
            try
            {
                var ent = _svc.Create(rqt);

                return CreatedAtAction(nameof(GetOneById), new { ent.Id }, ent);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
        }

        [HttpPut("Update/{Id}")]
        public ActionResult<MdlCalendarEvent> Update(int Id, [FromBody] RqtCalendarEvent rqt)
        {
            try
            {
                var ent = _svc.Update(Id, rqt);

                return Ok(ent);
            }
            catch (KeyNotFoundException exc)
            {
                return NotFound(exc.Message);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
        }

        [HttpDelete("Delete/{Id}")]
        public ActionResult Delete(int Id)
        {
            try
            {
                var res = _svc.Delete(Id);

                if (!res)
                {
                    return BadRequest("Already deleted.");
                }

                return NoContent();
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
        }
    }
}
