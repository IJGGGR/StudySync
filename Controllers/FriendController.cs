using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudySync.Models.DTOs;
using StudySync.Services;

namespace StudySync.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FriendController(SvcUser svc) : ControllerBase
    {
        private readonly SvcUser _svc = svc;

        [HttpPost("AcceptOrCreate/{SelfUsername}/{ThemUsername}")]
        public ActionResult FriendAcceptOrCreate(string SelfUsername, string ThemUsername)
        {
            try
            {
                var res = _svc.FriendAcceptOrCreate(SelfUsername, ThemUsername);

                return Ok(res ? "Accept" : "Create");
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

        [HttpPost("RejectOrDelete/{SelfUsername}/{ThemUsername}")]
        public ActionResult FriendRejectOrDelete(string SelfUsername, string ThemUsername)
        {
            try
            {
                var res = _svc.FriendRejectOrDelete(SelfUsername, ThemUsername);

                return Ok(res ? "Reject" : "Delete");
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
    }
}
