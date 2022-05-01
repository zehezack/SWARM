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
    public class EnrollmentController : BaseController<Enrollment>, IBaseController<Enrollment>
    {

        public EnrollmentController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<Enrollment> lstEnrollment = await _context.Enrollments.OrderBy(x => x.StudentId).ToListAsync();
                return Ok(lstEnrollment);
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
                Enrollment lstEnrollment = await _context.Enrollments.Where(x => x.StudentId == KeyValue).FirstOrDefaultAsync();
                return Ok(lstEnrollment);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        [Route("Get/{KeyValue1}/{KeyValue2}")]
        public async Task<IActionResult> Get(int KeyValue1, int KeyValue2)
        {
            try
            {
                Enrollment itmEnrollmente = await _context.Enrollments.Where(x => x.StudentId == KeyValue1 && x.SectionId == KeyValue2).FirstOrDefaultAsync();
                return Ok(itmEnrollmente);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete/{KeyValue1}")]
        public async Task<IActionResult> Delete(int KeyValue1)
        {
            try
            {
                Enrollment itmEnrollmente = await _context.Enrollments.Where(x => x.StudentId == KeyValue1).FirstOrDefaultAsync();
                _context.Remove(itmEnrollmente);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpDelete]
        [Route("Delete/{KeyValue1}/{KeyValue2}")]
        public async Task<IActionResult> Delete(int KeyValue1, int KeyValue2)
        {
            try
            {
                Enrollment itmEnrollmente = await _context.Enrollments.Where(x => x.StudentId == KeyValue1 && x.SectionId == KeyValue2).FirstOrDefaultAsync();
                _context.Remove(itmEnrollmente);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }


        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Enrollment _Enrollment)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var context = await _context.Enrollments.Where(x => x.StudentId == _Enrollment.StudentId && x.SectionId == _Enrollment.SectionId).FirstOrDefaultAsync();

                if (context != null)
                {
                    await Post(_Enrollment);
                    return Ok();
                }

                context.StudentId = _Enrollment.StudentId;
                context.SectionId = _Enrollment.SectionId;
                context.EnrollDate = _Enrollment.EnrollDate;
                context.FinalGrade = _Enrollment.FinalGrade;
                context.SchoolId = _Enrollment.SchoolId;
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
        public async Task<IActionResult> Post([FromBody] Enrollment _Enrollment)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var context = await _context.Enrollments.Where(x => x.StudentId == _Enrollment.StudentId).FirstOrDefaultAsync();

                if (context != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                context = new Enrollment();
                context.StudentId = _Enrollment.StudentId;
                context.SectionId = _Enrollment.SectionId;
                context.EnrollDate = _Enrollment.EnrollDate;
                context.FinalGrade = _Enrollment.FinalGrade;
                context.SchoolId = _Enrollment.SchoolId;

                _context.Enrollments.Add(context);

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