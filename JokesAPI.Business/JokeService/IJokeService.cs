using JokesAPI.Common.Models;
using JokesAPI.Common.Models.DTO;
using JokesAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JokesAPI.Business.JokeService
{
    public interface IJokeService
    {
        public IEnumerable<Joke> GetAllJokes();
        public OperationResult<List<Joke>> GetJokesBySearch(string search);
        public bool IsJokeQuestionExist(string jokeQuestion);
        public Joke GetJokeById(int jokeId);
        public Joke GetLatestJoke();
        public OperationResult<Joke> CreateJoke(JokeCreateDTO joke);
        public OperationResult<Joke> UpdateJoke(Joke joke);
        public OperationResult<Joke> DeleteJoke(int jokeId);


    }
}
