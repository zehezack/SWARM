using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWARM.EF.Data;
using SWARM.EF.Models;
using SWARM.Server.Controllers;
using SWARM.Server.Models;
using SWARM.Shared;
using SWARM.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;

namespace SWARM.Server.Controllers.Application
{
    public class GradeController : BaseController<Grade>, IBaseController<Grade>
    {

        public GradeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }


        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<Grade> lstGrades = await _context.Grades.OrderBy(x => x.SchoolId).ToListAsync();
                return Ok(lstGrades);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("Get/{KeyValue}/")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            try
            {
                List<Grade> lstGrades = await _context.Grades.Where(x => x.StudentId == KeyValue).ToListAsync();
                return Ok(lstGrades);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpDelete]
        [Route("{KeyValue}")]
        Task<IActionResult> IBaseController<Grade>.Delete(int KeyValue)
        {
            throw new NotImplementedException();
        }


        [HttpDelete]
        [Route("Delete/{KeyValue1}/{KeyValue2}/{KeyValue3}/{KeyValue4}")]
        public async Task<IActionResult> Delete(int KeyValue1, int KeyValue2, int KeyValue3, string KeyValue4)
        {
            try
            {
                Grade itmGrades = await _context.Grades.Where(x => x.SchoolId == KeyValue1 && x.StudentId == KeyValue2 && x.SectionId == KeyValue3 && x.GradeTypeCode == KeyValue4).FirstOrDefaultAsync();
                _context.Remove(itmGrades);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Grade _Grade) //overwrite
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var context = await _context.Grades.Where(x => x.SchoolId == _Grade.SchoolId && x.StudentId == _Grade.StudentId && x.SectionId == _Grade.SectionId && x.GradeTypeCode == _Grade.GradeTypeCode).FirstOrDefaultAsync();

                if (context == null)
                {
                    await Post(_Grade);
                    return Ok();
                }

                context.SchoolId = _Grade.SchoolId;
                context.StudentId = _Grade.StudentId;
                context.SectionId = _Grade.SectionId;
                context.GradeTypeCode = _Grade.GradeTypeCode;
                context.GradeCodeOccurrence = _Grade.GradeCodeOccurrence;
                context.NumericGrade = _Grade.NumericGrade;
                context.Comments = _Grade.Comments;


                _context.Update(context);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(context);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Grade _Grade) //insert
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var context = await _context.Grades.Where(x => x.SchoolId == _Grade.SchoolId && x.StudentId == _Grade.StudentId && x.SectionId == _Grade.SectionId && x.GradeTypeCode == _Grade.GradeTypeCode).FirstOrDefaultAsync();

                if (context != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                context = new Grade();

                context.SchoolId = _Grade.SchoolId;
                context.StudentId = _Grade.StudentId;
                context.SectionId = _Grade.SectionId;
                context.GradeTypeCode = _Grade.GradeTypeCode;
                context.GradeCodeOccurrence = _Grade.GradeCodeOccurrence;
                context.NumericGrade = _Grade.NumericGrade;
                context.Comments = _Grade.Comments;


                _context.Grades.Add(context);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(context);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
