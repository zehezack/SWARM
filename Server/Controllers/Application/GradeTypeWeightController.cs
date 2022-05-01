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
    public class GradeTypeWeightController : BaseController<GradeTypeWeight>, IBaseController<GradeTypeWeight>
    {

        public GradeTypeWeightController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }



        [HttpGet]
        [Route("{KeyValue}")]
        Task<IActionResult> IBaseController<GradeTypeWeight>.Get(int KeyValue)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("{KeyValue}")]
        Task<IActionResult> IBaseController<GradeTypeWeight>.Delete(int KeyValue)
        {
            throw new NotImplementedException();
        }




        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<GradeTypeWeight> lstGradeTypeWeights = await _context.GradeTypeWeights.OrderBy(x => x.SchoolId).ToListAsync();
                return Ok(lstGradeTypeWeights);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("Get/{KeyValue1}/{KeyValue2}/{KeyValue3}/")]
        public async Task<IActionResult> Get(int KeyValue1, int KeyValue2, int KeyValue3)
        {
            try
            {
                GradeTypeWeight itmGradeTypeCodeWeight = await _context.GradeTypeWeights.Where(x => x.SchoolId == KeyValue1 && x.SectionId == KeyValue2 && x.GradeTypeCode == KeyValue3.ToString()).FirstOrDefaultAsync();
                return Ok(itmGradeTypeCodeWeight);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete/{KeyValue1}/{KeyValue2}/{KeyValue3}/")]
        public async Task<IActionResult> Delete(int KeyValue1, int KeyValue2, int KeyValue3)
        {
            try
            {
                GradeTypeWeight itmGradeTypeCodeWeight = await _context.GradeTypeWeights.Where(x => x.SchoolId == KeyValue1 && x.SectionId == KeyValue2 && x.GradeTypeCode == KeyValue3.ToString()).FirstOrDefaultAsync();
                _context.Remove(itmGradeTypeCodeWeight);
                await _context.SaveChangesAsync();
                return Ok(itmGradeTypeCodeWeight);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeTypeWeight _GradeTypeWeight) //overwrite
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var context = await _context.GradeTypeWeights.Where(x => x.SchoolId == _GradeTypeWeight.SchoolId && x.SectionId == _GradeTypeWeight.SectionId && x.GradeTypeCode == _GradeTypeWeight.GradeTypeCode).FirstOrDefaultAsync();

                if (context == null)
                {
                    await Post(_GradeTypeWeight);
                    return Ok();
                }

                context.SchoolId = _GradeTypeWeight.SchoolId;
                context.SectionId = _GradeTypeWeight.SectionId;
                context.GradeTypeCode = _GradeTypeWeight.GradeTypeCode;
                context.NumberPerSection = _GradeTypeWeight.NumberPerSection;
                context.PercentOfFinalGrade = _GradeTypeWeight.PercentOfFinalGrade;
                context.DropLowest = _GradeTypeWeight.DropLowest;


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
        public async Task<IActionResult> Post([FromBody] GradeTypeWeight _GradeTypeWeight) //insert
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var context = await _context.GradeTypeWeights.Where(x => x.SchoolId == _GradeTypeWeight.SchoolId && x.SectionId == _GradeTypeWeight.SectionId && x.GradeTypeCode == _GradeTypeWeight.GradeTypeCode).FirstOrDefaultAsync();

                if (context != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                context = new GradeTypeWeight();
                context.SchoolId = _GradeTypeWeight.SchoolId;
                context.SectionId = _GradeTypeWeight.SectionId;
                context.GradeTypeCode = _GradeTypeWeight.GradeTypeCode;
                context.NumberPerSection = _GradeTypeWeight.NumberPerSection;
                context.PercentOfFinalGrade = _GradeTypeWeight.PercentOfFinalGrade;
                context.DropLowest = _GradeTypeWeight.DropLowest;


                _context.GradeTypeWeights.Add(context);

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
