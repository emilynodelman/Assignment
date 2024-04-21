using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly ActorService _actorService;

        public ActorsController(ActorService actorService)
        {
            _actorService = actorService;
        }

        [HttpGet]
        public IActionResult GetActors(string? name = null, int? minRank = null, int? maxRank = null, int page = 1, int pageSize = 10)
        {
            ResponseModel<List<Actor>> response = new ResponseModel<List<Actor>>();

            try
            {
                response = _actorService.GetActors(name, minRank, maxRank, page, pageSize);
                if(response != null)
                {
                    return Ok(response);
                }
                else return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Content = null;
                var error = new ErrorModel(HttpStatusCode.InternalServerError.ToString(), "General Error", ex.Message);
                response.Errors.Add(error);
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                return (IActionResult)response;
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetActor(string id)
        {
            try
            {
                var actor = _actorService.GetActorById(id);
                if (actor == null)
                    return NotFound();

                return Ok(actor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult AddActor([FromBody] Actor actor)
        {
            try
            {
                _actorService.AddActor(actor);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateActor(string id, [FromBody] Actor actor)
        {
            try
            {
                actor.Id = id;
                _actorService.UpdateActor(actor);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteActor(string id)
        {
            try
            {
                _actorService.DeleteActor(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
