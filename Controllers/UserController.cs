using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class UserController(SvcUser svc) : ControllerBase
    {
        private readonly SvcUser _svc = svc;

        [HttpGet("GetAll")]
        public ActionResult<IEnumerable<RspUser>> GetAll()
        {
            try
            {
                var rsp = _svc.GetAll().Select(e => new RspUser
                {
                    Id = e.Id,
                    Username = e.Username,
                    OutgoingRequests = e.OutgoingRequests,
                    IncomingRequests = e.IncomingRequests,
                    Friends = e.Friends,
                });

                return Ok(rsp);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
        }

        [HttpGet("GetOneById")]
        public ActionResult<RspUser> GetOneById(int id)
        {
            try
            {
                var ent = _svc.GetOneById(id);

                if (ent == null)
                {
                    return NotFound($"Id '{id}' not found.");
                }

                var rsp = new RspUser
                {
                    Id = ent.Id,
                    Username = ent.Username,
                    OutgoingRequests = ent.OutgoingRequests,
                    IncomingRequests = ent.IncomingRequests,
                    Friends = ent.Friends,
                };

                return Ok(rsp);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
        }

        [HttpGet("GetOneByUsername")]
        public ActionResult<RspUser> GetOneByUsername(string username)
        {
            try
            {
                var ent = _svc.GetOneByUsername(username);

                if (ent == null)
                {
                    return NotFound($"Username '{username}' not found.");
                }

                var rsp = new RspUser
                {
                    Id = ent.Id,
                    Username = ent.Username,
                    OutgoingRequests = ent.OutgoingRequests,
                    IncomingRequests = ent.IncomingRequests,
                    Friends = ent.Friends,
                };

                return Ok(rsp);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
        }

        [HttpGet("Verbose/GetOneById/{Id}")]
        public ActionResult<RspUserVerbose> VerboseGetOneById(int Id)
        {
            try
            {
                var ent = _svc.GetOneById(Id);

                if (ent == null)
                {
                    return NotFound($"Id '{Id}' not found.");
                }

                var rsp = new RspUserVerbose
                {
                    Id = ent.Id,
                    Username = ent.Username,
                    OutgoingRequests = [ .. ent.OutgoingRequests.Select(e => _svc.TerseGetOneById(e)) ],
                    IncomingRequests = [ .. ent.IncomingRequests.Select(e => _svc.TerseGetOneById(e)) ],
                    Friends = [ .. ent.Friends.Select(e => _svc.TerseGetOneById(e)) ],
                };

                return Ok(rsp);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
        }

        [HttpGet("Verbose/GetOneByUsername/{Username}")]
        public ActionResult<RspUserVerbose> VerboseGetOneByUsername(string Username)
        {
            try
            {
                var ent = _svc.GetOneByUsername(Username);

                if (ent == null)
                {
                    return NotFound($"Username '{Username}' not found.");
                }

                var rsp = new RspUserVerbose
                {
                    Id = ent.Id,
                    Username = ent.Username,
                    OutgoingRequests = [ .. ent.OutgoingRequests.Select(e => _svc.TerseGetOneById(e)) ],
                    IncomingRequests = [ .. ent.IncomingRequests.Select(e => _svc.TerseGetOneById(e)) ],
                    Friends = [ .. ent.Friends.Select(e => _svc.TerseGetOneById(e)) ],
                };

                return Ok(rsp);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
        }

        [HttpPost("Login")]
        public ActionResult<string> Login([FromBody] RqtAccount rqt)
        {
            try
            {
                var jwt = _svc.Login(rqt);

                return Ok(jwt);
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

        [HttpPost("Create")]
        public ActionResult<RspUser> Create([FromBody] RqtAccount rqt)
        {
            try
            {
                var ent = _svc.Create(rqt);

                var rsp = new RspUser
                {
                    Id = ent.Id,
                    Username = ent.Username,
                };

                return CreatedAtAction(nameof(GetOneById), new { rsp.Id }, rsp);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
        }

        [HttpPut("Update")]
        public ActionResult<RspUser> Update(int id, string username)
        {
            try
            {
                var ent = _svc.Update(id, username);

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

        [HttpDelete("Delete")]
        public ActionResult Delete(int id)
        {
            try
            {
                var res = _svc.Delete(id);

                if (!res)
                {
                    throw new UnreachableException("UNREACHABLE.");
                }

                return NoContent();
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
