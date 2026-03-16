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
    public class UserController(SvcUser svc) : ControllerBase
    {
        private readonly SvcUser _svc = svc;

        [HttpPost("Create")]
        public bool Create(DtoAccount UserToAdd)
        {
            return _svc.Create(UserToAdd);
        }

        [HttpGet("GetAll")]
        public IEnumerable<MdlUser> GetAll()
        {
            return _svc.GetAll();
        }

        [HttpGet("GetOneById")]
        public MdlUser? GetOneById(int id)
        {
            return _svc.GetOneById(id);
        }

        [HttpGet("GetOneByUsername")]
        public MdlUser? GetOneByUsername(string username)
        {
            return _svc.GetOneByUsername(username);
        }

        [HttpPost("Login")]
        public IActionResult Login(DtoAccount dto)
        {
            return _svc.Login(dto);
        }

        [HttpPost("Update")]
        public bool Update(int id, string username)
        {
            return _svc.Update(id, username);
        }

        [HttpPost("Delete")]
        public bool Delete(string username)
        {
            return _svc.Delete(username);
        }
    }
}
