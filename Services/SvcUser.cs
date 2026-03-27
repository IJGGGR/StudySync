using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
    public class SvcUser(AppDbCtx ctx, IConfiguration cfg) : ControllerBase
    {
        private readonly AppDbCtx _ctx = ctx;
        private readonly IConfiguration _cfg = cfg;

        internal IEnumerable<MdlUser> GetAll()
        {
            var res = _ctx.TblUser; // _ctx.TblUser.AsNoTracking().AsEnumerable()

            return res;
        }

        internal MdlUser? GetOneById(int id)
        {
            var res = _ctx.TblUser.FirstOrDefault(e => e.Id == id);

            return res;
        }

        internal MdlUser? GetOneByUsername(string? username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return null;
            }

            username = username.ToLower();
            var res = _ctx.TblUser.FirstOrDefault(e => e.Username == username);

            return res;
        }

        internal ActionResult<bool> Create(DtoAccount dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) ||
                string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(false);
            }

            var un = dto.Username.ToLower();
            var pw = dto.Password;

            if (char.IsWhiteSpace(un[0]) || char.IsWhiteSpace(un[^1]) ||
                !un.All(c => char.IsAsciiLetterOrDigit(c) || c == '.' || c == '-' || c == '_') ||
                char.IsWhiteSpace(pw[0]) || char.IsWhiteSpace(pw[^1]))
            {
                return BadRequest(false);
            }

            // is username taken
            if (GetOneByUsername(un) != null)
            {
                return Forbid();
            }

            // db add
            var psh = FnGenSaltHash(pw);
            var obj = new MdlUser() { Id = 0, Username = un, Salt = psh.Salt, Hash = psh.Hash };
            _ctx.Add(obj);
            var num = _ctx.SaveChanges();

            if (num < 1)
            {
                return BadRequest(false);
            }

            return Ok(true);
        }

        internal ActionResult<string> Login(DtoAccount dto)
        {
            var ent = GetOneByUsername(dto.Username);

            if (ent == null)
            {
                return BadRequest();
            }

            if (!FnChkPassword(dto.Password, ent.Salt, ent.Hash))
            {
                return Unauthorized();
            }

            var jwt = FnGenJWT([]);

            return Ok(jwt);
        }

        internal static DtoSaltHash FnGenSaltHash(string password)
        {
            // todo: cap password size to 512 bits, longer passwords are pre-hashed
            // 10_000 -> 600_000
            // 256    -> 32

            var salt = RandomNumberGenerator.GetBytes(64);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                10_000,
                HashAlgorithmName.SHA256,
                256
            );
            var res = new DtoSaltHash
            {
                Salt = Convert.ToBase64String(salt),
                Hash = Convert.ToBase64String(hash)
            };

            return res;
        }

        internal static bool FnChkPassword(string? password, string? dbSalt, string? dbHash)
        {
            // todo: cap password size to 512 bits, longer passwords are pre-hashed
            // 10_000 -> 600_000
            // 256    -> 32

            if (string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(dbSalt) ||
                string.IsNullOrWhiteSpace(dbHash))
            {
                return false;
            }

            var salt = Convert.FromBase64String(dbSalt);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                10_000,
                HashAlgorithmName.SHA256,
                256
            );
            var res = Convert.ToBase64String(hash) == dbHash;

            return res;
        }

        internal string FnGenJWT(IEnumerable<Claim> claims)
        {
            var key = _cfg["JWT:Key"] ?? throw new Exception();
            var buf = Encoding.UTF8.GetBytes(key);
            var opt = new JwtSecurityToken(
                issuer: Constants.API_URL,
                audience: Constants.API_URL,
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(buf), SecurityAlgorithms.HmacSha256)
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(opt);

            return jwt;
        }

        internal bool Update(int id, string username)
        {
            var ent = GetOneById(id);

            if (ent == null)
            {
                return false;
            }

            // todo: check if legal username

            ent.Username = username;
            _ctx.Update(ent);
            var res = _ctx.SaveChanges() > 0;

            return res;
        }

        internal bool Delete(int id)
        {
            var ent = GetOneById(id);

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
