//using Freelance_bot.Application.Feature.Projects.Queries.Models;
//using Freelance_bot.Application.Feature.Projects.Requests;
//using Freelance_bot.Application.IServieces;
//using MediatR;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace Freelance_Bot.Api.Controllers
//{
//    [Route("api/projects")]
//    public abstract class BaseController : ControllerBase
//    {
//        get
//     {
//        return Guid.Parse("11111111-1111-1111-1111-111111111111");
//     }

//    public class ProjectsController(IProjectService projectService) : BaseController
//    {
//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//            => Ok(await projectService.GetAllAsync(CurrentUserId));

//        [HttpGet("{id:guid}")]
//        public async Task<IActionResult> GetById(Guid id)
//        {
//            var project = await projectService.GetByIdAsync(id, CurrentUserId);
//            return project == null ? NotFound() : Ok(project);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Create([FromBody] CreateProjectRequest request)
//        {
//            var project = await projectService.CreateAsync(CurrentUserId, request);
//            return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
//        }

//        [HttpPatch("{id:guid}")]
//        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProjectRequest request)
//        {
//            try
//            {
//                var project = await projectService.UpdateAsync(id, CurrentUserId, request);
//                return Ok(project);
//            }
//            catch (KeyNotFoundException) { return NotFound(); }
//            catch (UnauthorizedAccessException) { return Forbid(); }
//        }

//        [HttpDelete("{id:guid}")]
//        public async Task<IActionResult> Delete(Guid id)
//        {
//            try { await projectService.DeleteAsync(id, CurrentUserId); return NoContent(); }
//            catch (KeyNotFoundException) { return NotFound(); }
//            catch (UnauthorizedAccessException) { return Forbid(); }
//        }

//        // For n8n daily cron — returns raw analytics data
//        //[HttpGet("analytics-data")]
//        //public async Task<IActionResult> GetAnalyticsData()
//        //    => Ok(await projectService.GetAnalyticsDataAsync(CurrentUserId));

//        //[HttpGet("dashboard")]
//        //public async Task<IActionResult> GetDashboard()
//        //    => Ok(await projectService.GetDashboardSummaryAsync(CurrentUserId));
//    }
//}
using Freelance_bot.Application.Feature.Projects.Requests;
using Freelance_bot.Application.IServieces;
using Microsoft.AspNetCore.Mvc;

namespace Freelance_Bot.Api.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectsController(IProjectService projectService) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await projectService.GetAllAsync(CurrentUserId));

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var project = await projectService.GetByIdAsync(id, CurrentUserId);
            return project == null ? NotFound() : Ok(project);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectRequest request)
        {
            var project = await projectService.CreateAsync(CurrentUserId, request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = project.Id },
                project);
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateProjectRequest request)
        {
            try
            {
                var project =
                    await projectService.UpdateAsync(id, CurrentUserId, request);

                return Ok(project);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await projectService.DeleteAsync(id, CurrentUserId);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }
    }

    public abstract class BaseController : ControllerBase
    {
        protected Guid CurrentUserId =>
            Guid.Parse("11111111-1111-1111-1111-111111111111");
    }
}
