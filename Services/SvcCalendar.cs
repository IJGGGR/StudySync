using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudySync.Contexts;
using StudySync.Models;
using StudySync.Models.DTOs;

namespace StudySync.Services
{
    public class SvcCalendar
    {
        private readonly AppDbCtx _ctx;

        public SvcCalendar(AppDbCtx ctx)
        {
            _ctx = ctx;
            _ctx.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;
        }

        // * GET ===========================================================================================

        internal IEnumerable<MdlCalendarEvent> GetAll()
        {
            var res = _ctx.TblCalendarEvent;

            return res;
        }

        internal MdlCalendarEvent? GetOneById(int id)
        {
            if (id < 1)
            {
                throw new ArgumentException("Invalid Id.");
            }

            var res = _ctx.TblCalendarEvent.FirstOrDefault(e => e.Id == id);

            return res;
        }

        internal IEnumerable<MdlCalendarEvent> GetAllByUserId(int id)
        {
            var res = _ctx.TblCalendarEvent.Where(e => e.UserId == id);

            return res;
        }

        // * SET ===========================================================================================

        internal MdlCalendarEvent Create(RqtCalendarEvent rqt)
        {
            var obj = new MdlCalendarEvent(rqt);
            _ctx.Add(obj);
            var num = _ctx.SaveChanges();

            if (num != 1)
            {
                throw new Exception($"DATABASE SAVE ERROR. {num} OUT OF 1 CHANGES WERE SAVED.");
            }

            return obj;
        }

        internal MdlCalendarEvent Update(int id, RqtCalendarEvent rqt)
        {
            var ent = GetOneById(id);

            if (ent == null)
            {
                throw new KeyNotFoundException($"Id '{id}' not found.");
            }

            ent.Update(rqt);
            _ctx.Update(ent);
            var num = _ctx.SaveChanges();

            if (num != 1)
            {
                throw new Exception($"DATABASE SAVE ERROR. {num} OUT OF 1 CHANGES WERE SAVED.");
            }

            return ent;
        }

        internal bool Delete(int id)
        {
            var ent = GetOneById(id);

            if (ent == null)
            {
                throw new KeyNotFoundException($"Id '{id}' not found.");
            }

            if (ent.IsDeleted)
            {
                return false;
            }

            ent.IsDeleted = true;
            _ctx.Update(ent);
            var num = _ctx.SaveChanges();

            if (num != 1)
            {
                throw new Exception($"DATABASE SAVE ERROR. {num} OUT OF 1 CHANGES WERE SAVED.");
            }

            return true;
        }
    }
}
