using System;

namespace JokesAPI.Data
{
    public class Joke
    {
        public int JokeId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? Modified { get; set; }

    }
}
