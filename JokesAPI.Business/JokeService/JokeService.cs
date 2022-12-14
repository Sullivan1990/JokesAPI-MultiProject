using JokesAPI.Common.Models;
using JokesAPI.Common.Models.DTO;
using JokesAPI.Data;
using JokesAPI.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace JokesAPI.Business.JokeService
{
    public class JokeService : IJokeService
    {
        protected readonly JokesContext _context;

        public JokeService(JokesContext context)
        {
            _context = context;
        }

        public OperationResult<Joke> CreateJoke(JokeCreateDTO joke)
        {

            try
            {
                Joke newJoke = new Joke
                {
                    Question = joke.Question,
                    Answer = joke.Answer
                };

                _context.Jokes.Add(newJoke);
                _context.SaveChanges();

                return new OperationResult<Joke>
                {
                    Succeeded = true,
                    ErrorMessage = "",
                    Result = newJoke
                };
            }
            catch (Exception e)
            {
                return new OperationResult<Joke>
                {
                    Succeeded = false,
                    ErrorMessage = e.Message
                };
            }

        }

        public OperationResult<Joke> DeleteJoke(int jokeId)
        {
            try
            {
                var joke = _context.Jokes.Find(jokeId);
                if (joke == null)
                {
                    return new OperationResult<Joke>
                    {
                        Succeeded = false,
                        ErrorMessage = "Joke was not found in database"
                    };
                }
                _context.Jokes.Remove(joke);
                _context.SaveChanges();

                return new OperationResult<Joke>
                {
                    Succeeded = true,
                    ErrorMessage = "",
                    Result = joke
                };
            }
            catch (Exception e)
            {
                return new OperationResult<Joke>
                {
                    Succeeded = false,
                    ErrorMessage = e.Message
                };
            }
        }

        public IEnumerable<Joke> GetAllJokes()
        {
            return _context.Jokes.ToList();
        }

        public Joke GetJokeById(int jokeId)
        {
            return _context.Jokes.Find(jokeId);
        }

        public Joke FindPredicate(Expression<Func<Joke, bool>> predicate)
        {
            return _context.Jokes.Where(predicate).FirstOrDefault();
        }

        public List<Joke> FindAllPredicate(Expression<Func<Joke, bool>> predicate)
        {
            return _context.Jokes.Where(predicate).ToList();
        }

        public OperationResult<List<Joke>> GetJokesBySearch(string search)
        {
            try
            {
                var result = _context.Jokes.Where(c => c.Question.Contains(search) || c.Answer.Contains(search)).ToList();

                return new OperationResult<List<Joke>>
                {
                    Result = result
                };
            }
            catch (Exception e)
            {
                return new OperationResult<List<Joke>>
                {
                    Succeeded = false,
                    ErrorMessage = e.Message
                };
            }

        }

        public Joke GetLatestJoke()
        {
            return _context.Jokes.OrderByDescending(c => c.Created).FirstOrDefault();
        }

        public bool IsJokeQuestionExist(string jokeQuestion)
        {
            return _context.Jokes.Any(c => c.Question.Equals(jokeQuestion));
        }

        public OperationResult<Joke> UpdateJoke(Joke joke)
        {
            try
            {
                joke.Modified = DateTime.Now;
                _context.Jokes.Update(joke);
                _context.SaveChanges();
                return new OperationResult<Joke>
                {
                    Result = joke
                };
            }
            catch (Exception e)
            {
                return new OperationResult<Joke>
                {
                    Succeeded = false,
                    ErrorMessage = e.Message
                };
            }
        }
    }
}
