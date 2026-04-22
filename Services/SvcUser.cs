using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudySync.Contexts;
using StudySync.Models;
using StudySync.Models.DTOs;

namespace StudySync.Services
{
    public class SvcUser
    {
        private readonly AppDbCtx _ctx;
        private readonly IConfiguration _cfg;

        public SvcUser(AppDbCtx ctx, IConfiguration cfg)
        {
            _ctx = ctx;
            _cfg = cfg;
            _ctx.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;
        }

        internal IEnumerable<MdlUser> GetAll()
        {
            var res = _ctx.TblUser; // _ctx.TblUser.AsNoTracking().AsEnumerable()

            return res;
        }

        /// <exception cref="ArgumentException"></exception>
        internal MdlUser? GetOneById(int id)
        {
            if (id < 1)
            {
                throw new ArgumentException("Invalid Id.");
            }

            var res = _ctx.TblUser.FirstOrDefault(e => e.Id == id);

            return res;
        }

        /// <exception cref="ArgumentException"></exception>
        internal MdlUser? GetOneByUsername(string? username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Invalid Username.");
            }

            username = username.Trim().ToLower();
            var res = _ctx.TblUser.FirstOrDefault(e => e.Username == username);

            return res;
        }

        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentException"></exception>
        internal MdlUser Create(RqtAccount rqt)
        {
            // check username validity and availability
            if (GetOneByUsername(rqt.Username) != null)
            {
                throw new InvalidOperationException("Username unavailable.");
            }

            if (string.IsNullOrWhiteSpace(rqt.Password))
            {
                throw new ArgumentException("Invalid Password.");
            }

            var un = rqt.Username.ToLower();
            var pw = rqt.Password;

            if (char.IsWhiteSpace(un[0]) | char.IsWhiteSpace(un[^1]))
            {
                throw new ArgumentException("Invalid Username. No leading or trailing spaces.");
            }

            if (!un.All(c => char.IsAsciiLetterOrDigit(c) | c == '.' | c == '-' | c == '_'))
            {
                throw new ArgumentException("Invalid Username. Alphanumeric, '.', '-', or '_' characters only.");
            }

            if (char.IsWhiteSpace(pw[0]) | char.IsWhiteSpace(pw[^1]))
            {
                throw new ArgumentException("Invalid Password. No leading or trailing spaces.");
            }

            // db add
            var psh = FnGenSaltHash(pw);
            var obj = new MdlUser() {
                Username = un,
                Salt = psh.Salt,
                Hash = psh.Hash
            };
            _ctx.TblUser.Add(obj);
            var num = _ctx.SaveChanges();

            if (num != 1)
            {
                throw new Exception($"DATABASE SAVE ERROR. {num} OUT OF 1 CHANGES WERE SAVED.");
            }

            return obj;
        }

        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="Exception"></exception>
        internal string Login(RqtAccount rqt)
        {
            var ent = GetOneByUsername(rqt.Username);

            if (ent == null)
            {
                throw new KeyNotFoundException($"Username '{rqt.Username}' not found.");
            }

            if (!FnChkPassword(rqt.Password, ent.Salt, ent.Hash))
            {
                throw new Exception("Incorrect Password.");
            }

            var jwt = FnGenJWT([]);

            return jwt;
        }

        internal static RspSaltHash FnGenSaltHash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(64);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                600_000,
                HashAlgorithmName.SHA256,
                32
            );
            var res = new RspSaltHash
            {
                Salt = Convert.ToBase64String(salt),
                Hash = Convert.ToBase64String(hash)
            };

            return res;
        }

        internal static bool FnChkPassword(string password, string dbSalt, string dbHash)
        {
            var salt = Convert.FromBase64String(dbSalt);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                600_000,
                HashAlgorithmName.SHA256,
                32
            );
            var res = Convert.ToBase64String(hash) == dbHash;

            return res;
        }

        internal string FnGenJWT(IEnumerable<Claim> claims)
        {
            var key = _cfg["JWT:Key"] ?? throw new Exception("NULL JWT KEY.");
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

        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        internal MdlUser Update(int id, string username)
        {
            var ent = GetOneById(id);

            if (ent == null)
            {
                throw new KeyNotFoundException($"Id '{id}' not found.");
            }

            if (GetOneByUsername(username) != null)
            {
                throw new InvalidOperationException("Username unavailable.");
            }

            var un = username.ToLower();

            if (char.IsWhiteSpace(un[0]) | char.IsWhiteSpace(un[^1]))
            {
                throw new ArgumentException("Invalid Username. No leading or trailing spaces.");
            }

            if (!un.All(c => char.IsAsciiLetterOrDigit(c) | c == '.' | c == '-' | c == '_'))
            {
                throw new ArgumentException("Invalid Username. Alphanumeric, '.', '-', or '_' characters only.");
            }

            ent.Username = un;
            _ctx.TblUser.Update(ent);
            var num = _ctx.SaveChanges();

            if (num != 1)
            {
                throw new Exception($"DATABASE SAVE ERROR. {num} OUT OF 1 CHANGES WERE SAVED.");
            }

            return ent;
        }

        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="Exception"></exception>
        internal bool Delete(int id)
        {
            var ent = GetOneById(id);

            if (ent == null)
            {
                throw new KeyNotFoundException($"Id '{id}' not found.");
            }

            _ctx.Remove(ent);
            var num = _ctx.SaveChanges();

            if (num != 1)
            {
                throw new Exception($"DATABASE SAVE ERROR. {num} OUT OF 1 CHANGES WERE SAVED.");
            }

            return true;
        }

        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="Exception"></exception>
        internal bool FriendAcceptOrCreate(RqtFriend rqt)
        {
            var self = GetOneByUsername(rqt.UsernameSelf);

            if (self == null)
            {
                throw new KeyNotFoundException($"Username '{rqt.UsernameSelf}' not found.");
            }

            var them = GetOneByUsername(rqt.UsernameThem);

            if (them == null)
            {
                throw new KeyNotFoundException($"Username '{rqt.UsernameThem}' not found.");
            }

            var sID = self.Id;
            var tID = them.Id;

            if (sID == tID)
            {
                throw new InvalidOperationException("Both Usernames are the same.");
            }

            if (self.Friends.Contains(tID) & them.Friends.Contains(sID))
            {
                throw new InvalidOperationException("Users are already friends.");
            }

            var blnSelfOut = self.OutgoingRequests.Contains(tID);
            var blnThemOut = them.OutgoingRequests.Contains(sID);
            var blnSelfInc = self.IncomingRequests.Contains(tID);
            var blnThemInc = them.IncomingRequests.Contains(sID);

            // * ACCEPT AN INCOMING REQUEST FROM THEM
            if (blnSelfInc & blnThemOut)
            {
                self.IncomingRequests = [ .. self.IncomingRequests.Where(e => e != tID) ];
                them.OutgoingRequests = [ .. them.OutgoingRequests.Where(e => e != sID) ];
                self.Friends = [ .. self.Friends, tID ];
                them.Friends = [ .. them.Friends, sID ];
                _ctx.TblUser.UpdateRange([ self, them ]);
                var num = _ctx.SaveChanges();

                if (num != 2)
                {
                    throw new Exception($"DATABASE SAVE ERROR. {num} OUT OF 2 CHANGES WERE SAVED.");
                }

                return true;
            }

            // * CREATE AN OUTGOING REQUEST FROM SELF
            if (!blnSelfOut & !blnThemInc)
            {
                self.OutgoingRequests = [ .. self.OutgoingRequests, tID ];
                them.IncomingRequests = [ .. them.IncomingRequests, sID ];
                _ctx.TblUser.UpdateRange([ self, them ]);
                var num = _ctx.SaveChanges();

                if (num != 2)
                {
                    throw new Exception($"DATABASE SAVE ERROR. {num} OUT OF 2 CHANGES WERE SAVED.");
                }

                return false;
            }

            // * ACTIVE OUTGOING REQUEST FROM SELF
            if (blnSelfOut & blnThemInc)
            {
                throw new InvalidOperationException("Active outgoing request.");
            }

            // * INVALID STATE CATCH-ALL =====================

            // RESET STATE - PURGE BOTH USER IDS
            self.OutgoingRequests = [ .. self.OutgoingRequests.Where(e => e != sID & e != tID) ];
            them.OutgoingRequests = [ .. them.OutgoingRequests.Where(e => e != sID & e != tID) ];
            self.IncomingRequests = [ .. self.IncomingRequests.Where(e => e != sID & e != tID) ];
            them.IncomingRequests = [ .. them.IncomingRequests.Where(e => e != sID & e != tID) ];
            self.Friends = [ .. self.Friends.Where(e => e != sID & e != tID) ];
            them.Friends = [ .. them.Friends.Where(e => e != sID & e != tID) ];
            _ctx.TblUser.UpdateRange([ self, them ]);
            var fix = _ctx.SaveChanges();

            throw fix switch
            {
                2 => new Exception("An invalid state was repaired, please try again."),
                _ => new Exception($"FAILED REPAIRING INVALID STATE. {fix} OUT OF 2 CHANGES WERE SAVED."),
            };
        }

        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="Exception"></exception>
        internal bool FriendRejectOrDelete(RqtFriend rqt)
        {
            var self = GetOneByUsername(rqt.UsernameSelf);

            if (self == null)
            {
                throw new KeyNotFoundException($"Username '{rqt.UsernameSelf}' not found.");
            }

            var them = GetOneByUsername(rqt.UsernameThem);

            if (them == null)
            {
                throw new KeyNotFoundException($"Username '{rqt.UsernameThem}' not found.");
            }

            var sID = self.Id;
            var tID = them.Id;

            if (sID == tID)
            {
                throw new InvalidOperationException("Both Usernames are the same.");
            }

            var blnSelfOut = self.OutgoingRequests.Contains(tID);
            var blnThemOut = them.OutgoingRequests.Contains(sID);
            var blnSelfInc = self.IncomingRequests.Contains(tID);
            var blnThemInc = them.IncomingRequests.Contains(sID);
            var blnSelfFri = self.Friends.Contains(tID);
            var blnThemFri = them.Friends.Contains(sID);

            // * REJECT AN INCOMING REQUEST FROM THEM
            if (blnSelfInc & blnThemOut)
            {
                self.IncomingRequests = [ .. self.IncomingRequests.Where(e => e != tID) ];
                them.OutgoingRequests = [ .. them.OutgoingRequests.Where(e => e != sID) ];
                _ctx.TblUser.UpdateRange([ self, them ]);
                var num = _ctx.SaveChanges();

                if (num != 2)
                {
                    throw new Exception($"DATABASE SAVE ERROR. {num} OUT OF 2 CHANGES WERE SAVED.");
                }

                return true;
            }

            // * DELETE AN OUTGOING REQUEST FROM SELF
            if (blnSelfOut & blnThemInc)
            {
                self.OutgoingRequests = [ .. self.OutgoingRequests.Where(e => e != tID) ];
                them.IncomingRequests = [ .. them.IncomingRequests.Where(e => e != sID) ];
                _ctx.TblUser.UpdateRange([ self, them ]);
                var num = _ctx.SaveChanges();

                if (num != 2)
                {
                    throw new Exception($"DATABASE SAVE ERROR. {num} OUT OF 2 CHANGES WERE SAVED.");
                }

                return false;
            }

            // * DELETE FRIEND
            if (blnSelfFri & blnThemFri)
            {
                self.Friends = [ .. self.Friends.Where(e => e != tID) ];
                them.Friends = [ .. them.Friends.Where(e => e != sID) ];
                _ctx.TblUser.UpdateRange([ self, them ]);
                var num = _ctx.SaveChanges();

                if (num != 2)
                {
                    throw new Exception($"DATABASE SAVE ERROR. {num} OUT OF 2 CHANGES WERE SAVED.");
                }

                return false;
            }

            // * NOT FRIENDS / NO PENDING REQUESTS
            if (!blnSelfFri & !blnThemFri)
            {
                throw new InvalidOperationException("Users are not friends.");
            }

            // * INVALID STATE CATCH-ALL =====================

            // RESET STATE - PURGE BOTH USER IDS
            self.OutgoingRequests = [ .. self.OutgoingRequests.Where(e => e != sID & e != tID) ];
            them.OutgoingRequests = [ .. them.OutgoingRequests.Where(e => e != sID & e != tID) ];
            self.IncomingRequests = [ .. self.IncomingRequests.Where(e => e != sID & e != tID) ];
            them.IncomingRequests = [ .. them.IncomingRequests.Where(e => e != sID & e != tID) ];
            self.Friends = [ .. self.Friends.Where(e => e != sID & e != tID) ];
            them.Friends = [ .. them.Friends.Where(e => e != sID & e != tID) ];
            _ctx.TblUser.UpdateRange([ self, them ]);
            var fix = _ctx.SaveChanges();

            throw fix switch
            {
                2 => new Exception("An invalid state was repaired, please try again."),
                _ => new Exception($"FAILED REPAIRING INVALID STATE. {fix} OUT OF 2 CHANGES WERE SAVED."),
            };
        }
    }
}
