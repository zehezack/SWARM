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
    public class GradeTypeController : BaseController<GradeType>, IBaseController<GradeType>
    {

        public GradeTypeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {

        }


        [HttpGet]
        [Route("{KeyValue}")]
        Task<IActionResult> IBaseController<GradeType>.Get(int KeyValue)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                List<GradeType> lstGradeTypes = await _context.GradeTypes.OrderBy(x => x.SchoolId).ToListAsync();
                return Ok(lstGradeTypes);
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
                GradeType itmGradeTypeCode = await _context.GradeTypes.Where(x => x.SchoolId == KeyValue1 && x.GradeTypeCode == KeyValue2.ToString()).FirstOrDefaultAsync();
                return Ok(itmGradeTypeCode);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("{KeyValue}")]
        Task<IActionResult> IBaseController<GradeType>.Delete(int KeyValue)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("Delete/{KeyValue1}/{KeyValue2}")]
        public async Task<IActionResult> Delete(int KeyValue1, int KeyValue2)
        {
            try
            {
                GradeType itmGradeTypeCode = await _context.GradeTypes.Where(x => x.SchoolId == KeyValue1 && x.GradeTypeCode == KeyValue2.ToString()).FirstOrDefaultAsync();
                _context.Remove(itmGradeTypeCode);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeType _GradeType) //overwrite
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var context = await _context.GradeTypes.Where(x => x.SchoolId == _GradeType.SchoolId && x.GradeTypeCode == _GradeType.GradeTypeCode).FirstOrDefaultAsync();

                if (context == null)
                {
                    await Post(_GradeType);
                    return Ok();
                }

                context.SchoolId = _GradeType.SchoolId;
                context.GradeTypeCode = _GradeType.GradeTypeCode;
                context.Description = _GradeType.Description;



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
        public async Task<IActionResult> Post([FromBody] GradeType _GradeType) //insert
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var context = await _context.GradeTypes.Where(x => x.SchoolId == _GradeType.SchoolId && x.GradeTypeCode == _GradeType.GradeTypeCode).FirstOrDefaultAsync();

                if (context != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                context = new GradeType();

                context.SchoolId = _GradeType.SchoolId;
                context.GradeTypeCode = _GradeType.GradeTypeCode;
                context.Description = _GradeType.Description;

                _context.GradeTypes.Add(context);

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
