using JokesAPI.Business.JokeService;
using JokesAPI.Common.Models.DTO;
using JokesAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

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

        [HttpGet("GetByQuestion/{question}")]
        public IActionResult GetByQuestion(string question, bool exactMatch = false)
        {

            // Expressions can be defined ahead of time - in these examples below we are using a Func<>
            // expression that takes in a Joke, and returns a bool.
            // The boolean result is determined by the result of the expression provided to the
            // right hand side of the =

            Expression<Func<Joke, bool>> exact = c => c.Question.Equals(question);
            Expression<Func<Joke, bool>> partial = c => c.Question.Contains(question);

            // Depending on the state of the exactMatch bool, one of the expressions above will be passed in
            Joke joke = _jokeService.FindPredicate(exactMatch == true ? exact : partial);            

            return joke != null ? Ok(joke) : NotFound();
        }

        [HttpGet("GetByAnswer/{answer}")]
        public IActionResult GetByAnswer(string answer, bool exactMatch = false)
        {
            string q = "Question";


            Expression<Func<Joke, bool>> exact = c => c.Answer.Equals(answer);
            Expression<Func<Joke, bool>> partial = c => c.Answer.Contains(answer);

            // Depending on the state of the exactMatch bool, one of the expressions above will be passed in
            Joke joke = _jokeService.FindPredicate(exactMatch == true ? exact : partial);

            // Expressions can be passed in directly, as the input parameter is expecting an expression
            Joke jokeExact = _jokeService.FindPredicate(c => c.Answer.Equals(answer));
            Joke jokePartial = _jokeService.FindPredicate(c => c.Answer.Contains(answer));

            return joke != null ? Ok(joke) : NotFound();
        }

        /// <summary>
        /// Should not be exposed publicly for security reason - a map of the model could be created, and PK's could be leaked
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet("GetSingleByProperty")]
        public IActionResult GetSingleByProperty(string property, string value)
        {
            try
            {
                // Define the type of expression (the type it is based on)
                var paramExpression = Expression.Parameter(typeof(Joke));
                // define the property to base the expression on, as plain text
                var propertyExpression = Expression.Property(paramExpression, typeof(Joke), property);
                // The execution fo the operation - this will result in (c => c.property == value)
                var bodyExpression = Expression.Equal(propertyExpression, Expression.Constant(value));
                // build the expression into a format that is acceptable for a .Where()
                var lambda = Expression.Lambda<Func<Joke, bool>>(bodyExpression, paramExpression);


                // pass the custom expression into the method
                Joke joke = _jokeService.FindPredicate(lambda);


                return joke != null ? Ok(joke) : NotFound();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpGet("GetListByProperty")]
        public IActionResult GetAllByProperty(string property, string value)
        {
            try
            {
                // Define the type of expression (the type it is based on)
                var paramExpression = Expression.Parameter(typeof(Joke));
                // define the property to base the expression on, as plain text
                var propertyExpression = Expression.Property(paramExpression, typeof(Joke), property);
                // The execution fo the operation - this will result in (c => c.property == value)
                var bodyExpression = Expression.Equal(propertyExpression, Expression.Constant(value));
                // build the expression into a format that is acceptable for a .Where()
                var lambda = Expression.Lambda<Func<Joke, bool>>(bodyExpression, paramExpression);


                // pass the custom expression into the method
                List<Joke> jokes = _jokeService.FindAllPredicate(lambda);


                return jokes != null && jokes.Count > 0 ? Ok(jokes) : NotFound();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

        }


        [HttpGet("GetListByPropertyPartial")]
        public IActionResult GetAllByPropertPartial(string property, string partialValue)
        {
            try
            {
                // Define the type of expression (the type it is based on)
                var paramExpression = Expression.Parameter(typeof(Joke));
                // define the property to base the expression on, as plain text
                var propertyExpression = Expression.Property(paramExpression, typeof(Joke), property);

                // create a reference to the 'String.Contains' method - to be used in an expression
                MethodInfo stringContainsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

                // combine the property, the intended method and the value to be passed in
                var containsExpression = Expression.Call(propertyExpression, stringContainsMethod, Expression.Constant(partialValue));

                // build the expression into a format that is acceptable for a .Where()
                var lambda = Expression.Lambda<Func<Joke, bool>>(containsExpression, paramExpression);

                // pass the custom expression into the method
                List<Joke> jokes = _jokeService.FindAllPredicate(lambda);


                return jokes != null && jokes.Count > 0 ? Ok(jokes) : NotFound();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

        }


    }
}
