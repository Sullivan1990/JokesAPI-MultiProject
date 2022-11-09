using JokesAPI.Business.JokeService;
using JokesAPI.Common.Models.DTO;
using JokesAPI.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JokesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JokeController : ControllerBase
    {
        protected readonly IJokeService _jokeService;
        public JokeController(IJokeService jokeService)
        {
            _jokeService = jokeService;
        }

        // GET: api/<JokeController>
        [HttpGet]
        public IEnumerable<Joke> Get()
        {
            return _jokeService.GetAllJokes();    
        }

        // GET api/<JokeController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _jokeService.GetJokeById(id);
            return result != null ? Ok(result) : NotFound();
        }

        // POST api/<JokeController>
        [HttpPost]
        public IActionResult Post([FromBody] JokeCreateDTO joke)
        {
            var result = _jokeService.CreateJoke(joke);
            return result.Succeeded ? Ok(result.Result) : BadRequest(result.ErrorMessage);
        }

        // PUT api/<JokeController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Joke joke)
        {
            var result = _jokeService.UpdateJoke(joke);
            return result.Succeeded ? Ok(result.Result) : BadRequest(result.ErrorMessage);
        }

        // DELETE api/<JokeController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _jokeService.DeleteJoke(id);
            return result.Succeeded ? NoContent() : BadRequest(result.ErrorMessage);
        }
    }
}
