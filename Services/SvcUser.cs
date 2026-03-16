using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudySync.Contexts;
using StudySync.Models;
using StudySync.Models.DTOs;

namespace StudySync.Services
{
    public class SvcUser(AppDbCtx ctx) : ControllerBase
    {
        private readonly AppDbCtx _ctx = ctx;

        public IEnumerable<MdlUser> GetAll()
        {
            return _ctx.TblUser;
            // return _ctx.TblUser.AsNoTracking().AsEnumerable();
        }

        public MdlUser? GetOneById(int id)
        {
            return _ctx.TblUser.FirstOrDefault(e => e.Id == id);
        }

        public MdlUser? GetOneByUsername(string username)
        {
            return _ctx.TblUser.FirstOrDefault(e => e.Username == username);
        }

        public bool DoesUserExist(string? username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return false;
            }

            username = username.ToLower();
            var res = GetOneByUsername(username) != null;

            return res;
        }

        public bool Create(DtoAccount dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) ||
                string.IsNullOrWhiteSpace(dto.Password))
            {
                return false;
            }

            var un = dto.Username.ToLower();
            var pw = dto.Password;

            if (char.IsWhiteSpace(un[0]) || char.IsWhiteSpace(un[^1]) ||
                !un.All(c => char.IsAsciiLetterOrDigit(c) || c == '.' || c == '-' || c == '_') ||
                char.IsWhiteSpace(pw[0]) || char.IsWhiteSpace(pw[^1]))
            {
                return false;
            }

            // db check
            if (DoesUserExist(un))
            {
                return false;
            }

            // db add
            var sec = SaltHashPassword(pw);
            var obj = new MdlUser() { Id = 0, Username = un, Salt = sec.Salt, Hash = sec.Hash };
            _ctx.Add(obj);

            return _ctx.SaveChanges() > 0;
        }

        public DtoSaltHash SaltHashPassword(string password)
        {
            var buf = new byte[64];
            RandomNumberGenerator.Create().GetNonZeroBytes(buf);

            var salt = Convert.ToBase64String(buf);
            var hash = Convert.ToBase64String(new Rfc2898DeriveBytes(password, buf, 10_000, HashAlgorithmName.SHA256).GetBytes(256));

            return new() { Salt = salt, Hash = hash };
        }

        public bool VerifyPassword(string? password, string? dbSalt, string? dbHash)
        {
            if (string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(dbSalt) ||
                string.IsNullOrWhiteSpace(dbHash))
            {
                return false;
            }

            var salt = Convert.FromBase64String(dbSalt);
            var hash = Convert.ToBase64String(new Rfc2898DeriveBytes(password, salt, 10_000, HashAlgorithmName.SHA256).GetBytes(256));

            return hash == dbHash;
        }

        public IActionResult Login(DtoAccount dto)
        {
            var ent = GetOneByUsername(dto.Username ?? "");

            if (ent == null)
            {
                return BadRequest();
            }

            if (!VerifyPassword(dto.Password, ent.Salt, ent.Hash))
            {
                return Unauthorized();
            }

            // TODO: fix hard-coded secret key (must be 256-bits at minimum)

            var buf = Encoding.UTF8.GetBytes("0123456789ABCDEF0123456789ABCDEF");
            var opt = new JwtSecurityToken(
                issuer: "https://csa-2526-studysync-api-b6bue3aue8hka0ea.westus3-01.azurewebsites.net/",
                audience: "https://csa-2526-studysync-api-b6bue3aue8hka0ea.westus3-01.azurewebsites.net/",
                claims: [],
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(buf), SecurityAlgorithms.HmacSha256)
            );
            var tkn = new JwtSecurityTokenHandler().WriteToken(opt);
            var obj = new { Token = tkn };

            return Ok(obj);
        }

        public bool Update(int id, string username)
        {
            var ent = GetOneById(id);

            if (ent == null)
            {
                return false;
            }

            ent.Username = username;
            _ctx.Update(ent);
            var res = _ctx.SaveChanges() > 0;

            return res;
        }

        public bool Delete(string username)
        {
            var ent = GetOneByUsername(username);

            if (ent == null)
            {
                return false;
            }

            _ctx.Remove(ent);
            var res = _ctx.SaveChanges() > 0;

            return res;
        }
    }
}
