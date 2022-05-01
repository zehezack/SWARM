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
    public class GradeConversionController : BaseController<GradeConversion>, IBaseController<GradeConversion>
    {

        public GradeConversionController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }


        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<GradeConversion> lstGradeConversions = await _context.GradeConversions.OrderBy(x => x.SchoolId).ToListAsync();
                return Ok(lstGradeConversions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("Get/{KeyValue1}")]
        public async Task<IActionResult> Get(int KeyValue1)
        {
            try
            {
                List<GradeConversion> lstGradeConversions = await _context.GradeConversions.Where(x => x.SchoolId == KeyValue1).ToListAsync();
                return Ok(lstGradeConversions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("{KeyValue}")]
        Task<IActionResult> IBaseController<GradeConversion>.Delete(int KeyValue)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("Get/{KeyValue1}/{KeyValue2}")]
        public async Task<IActionResult> Get(int KeyValue1, int KeyValue2)
        {
            try
            {
                GradeConversion itmGradeConversions = await _context.GradeConversions.Where(x => x.SchoolId == KeyValue1 && x.LetterGrade == KeyValue2.ToString()).FirstOrDefaultAsync();
                return Ok(itmGradeConversions);
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
                GradeConversion itmGradeConversions = await _context.GradeConversions.Where(x => x.SchoolId == KeyValue1 && x.LetterGrade == KeyValue2.ToString()).FirstOrDefaultAsync();
                _context.Remove(itmGradeConversions);
                await _context.SaveChangesAsync();
                return Ok(itmGradeConversions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeConversion _GradeConversion) //overwrite
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var context = await _context.GradeConversions.Where(x => x.SchoolId == _GradeConversion.SchoolId && x.LetterGrade == _GradeConversion.LetterGrade).FirstOrDefaultAsync();

                if (context == null)
                {
                    await Post(_GradeConversion);
                    return Ok();
                }

                context.SchoolId = _GradeConversion.SchoolId;
                context.LetterGrade = _GradeConversion.LetterGrade;
                context.GradePoint = _GradeConversion.GradePoint;
                context.MaxGrade = _GradeConversion.MaxGrade;
                context.MinGrade = _GradeConversion.MinGrade;
                context.School = _GradeConversion.School;


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
        public async Task<IActionResult> Post([FromBody] GradeConversion _GradeConversion) //insert
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var context = await _context.GradeConversions.Where(x => x.SchoolId == _GradeConversion.SchoolId && x.LetterGrade == _GradeConversion.LetterGrade).FirstOrDefaultAsync();

                if (context != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                context = new GradeConversion();
                context.SchoolId = _GradeConversion.SchoolId;
                context.LetterGrade = _GradeConversion.LetterGrade;
                context.GradePoint = _GradeConversion.GradePoint;
                context.MaxGrade = _GradeConversion.MaxGrade;
                context.MinGrade = _GradeConversion.MinGrade;
                context.School = _GradeConversion.School;



                _context.GradeConversions.Add(context);

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
