using Microsoft.EntityFrameworkCore;
using NuGet.Frameworks;

namespace JokesAPI.Tests
{
    public class JokeServiceTests
    {
        DbContextOptions _options;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder().UseInMemoryDatabase(databaseName: "JokeTestChange1").Options;
        }

        [Test]
        public void AddingNewJokeReturnsOPResultAndIncreasedNumberOfJokes()
        {
            // Arrange

            // set up the Database
            using (var context = new JokesContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            JokeCreateDTO newJoke = new JokeCreateDTO { Question = "What kind of tea is hard to swallow?", Answer = "Reality" };



            JokesContext _context = new JokesContext(_options);

            JokeService _jokeService = new JokeService(_context);

            // Act

            int initialJokeCount = _context.Jokes.Count();

            var result = _jokeService.CreateJoke(newJoke);

            int newJokeCount = _context.Jokes.Count();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Succeeded == true);
            Assert.That(result.Result.Question == newJoke.Question &&
                result.Result.Answer == newJoke.Answer);
            Assert.That(newJokeCount == initialJokeCount + 1);
            _context.Dispose();

        }

        [Test]
        public void RemovingJokeFromDBWithID_OPResultAndDescreasedNumberOfJokes()
        {
            // Arrange

            // set up the Database
            using (var context = new JokesContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            int idToRemove = 1;

            JokesContext _context = new JokesContext(_options);

            JokeService _jokeService = new JokeService(_context);

            // Act

            int initialJokeCount = _context.Jokes.Count();

            var result = _jokeService.DeleteJoke(idToRemove);

            int newJokeCount = _context.Jokes.Count();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Succeeded == true);
            Assert.That(newJokeCount == initialJokeCount - 1);
            _context.Dispose();

        }

        [Test]
        public void RemovingJokeFromDBWithIncorrectID_OPResultWithError()
        {
            // Arrange

            // set up the Database
            using (var context = new JokesContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            int idToRemove = 9999;

            JokesContext _context = new JokesContext(_options);

            JokeService _jokeService = new JokeService(_context);

            // Act

            int initialJokeCount = _context.Jokes.Count();

            var result = _jokeService.DeleteJoke(idToRemove);

            int newJokeCount = _context.Jokes.Count();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Succeeded == false);
            Assert.That(newJokeCount == initialJokeCount);
            Assert.That(result.ErrorMessage.Length > 0);
            _context.Dispose();

        }

        [Test]
        public void EditingJokeWithNewQuestion_OPResultWithUpdatedJoke()
        {
            // Arrange

            // set up the Database
            using (var context = new JokesContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.Jokes.Add(new Joke
                {
                    JokeId = 1000,
                    Question = "I don't trust stairs.",
                    Answer = "They're always upto something"
                });
                context.SaveChanges();
            }

            var jokeUpdate = new Joke
            {
                JokeId = 1000,
                Question = "I don't trust stairs.",
                Answer = "They're always up to something"
            };

            JokesContext _context = new JokesContext(_options);

            JokeService _jokeService = new JokeService(_context);

            // Act

            var result = _jokeService.UpdateJoke(jokeUpdate);


            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Succeeded == true);
            Assert.That(result.Result.Question == jokeUpdate.Question);
            Assert.That(result.Result.Answer == jokeUpdate.Answer);
            Assert.IsNotNull(result.Result.Modified);
            Assert.That(result.ErrorMessage.Length == 0);
            _context.Dispose();

        }

        [Test]
        public void SearchingForExistingJokeByExactQuestion_TrueBoolean()
        {
            // Arrange

            // set up the Database
            using (var context = new JokesContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.Jokes.Add(new Joke
                {
                    JokeId = 1000,
                    Question = "I don't trust stairs.",
                    Answer = "They're always up to something"
                });
                context.SaveChanges();
            }

            JokesContext _context = new JokesContext(_options);

            JokeService _jokeService = new JokeService(_context);

            string jokeQuestion = "I don't trust stairs.";

            // Act

            var result = _jokeService.IsJokeQuestionExist(jokeQuestion);

            // Assert

            Assert.IsTrue(result);
            _context.Dispose();

        }

        [Test]
        public void SearchingForExistingJokeByInExactQuestion_FalseBoolean()
        {
            // Arrange

            // set up the Database
            using (var context = new JokesContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.Jokes.Add(new Joke
                {
                    JokeId = 1000,
                    Question = "I don't trust stairs.",
                    Answer = "They're always up to something"
                });
                context.SaveChanges();
            }

            JokesContext _context = new JokesContext(_options);

            JokeService _jokeService = new JokeService(_context);

            string jokeQuestion = "I don't trust stair.";

            // Act

            var result = _jokeService.IsJokeQuestionExist(jokeQuestion);

            // Assert

            Assert.IsFalse(result);
            _context.Dispose();

        }

        [Test]
        public void GetLatestJoke_LatestCreatedJoke()
        {
            // Arrange

            // set up the Database
            using (var context = new JokesContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.Jokes.Add(new Joke
                {
                    JokeId = 1000,
                    Question = "I don't trust stairs.",
                    Answer = "They're always up to something"
                });
                context.SaveChanges();

                context.Jokes.Add(new Joke
                {
                    JokeId = 1001,
                    Question = "Three fish are in a tank",
                    Answer = "One asks the others, 'How do you drive this thing?'"
                });
                context.SaveChanges();
            }

            JokesContext _context = new JokesContext(_options);

            JokeService _jokeService = new JokeService(_context);

            int latestJokeId = 1001;

            // Act

            var result = _jokeService.GetLatestJoke();

            // Assert

            Assert.That(result.JokeId == latestJokeId);
            _context.Dispose();

        }

        [Test]
        public void GetJokesByPartialSearchViaQuestion_TwoMatchingJokes()
        {
            // Arrange

            // set up the Database
            using (var context = new JokesContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.Jokes.Add(new Joke
                {
                    JokeId = 1000,
                    Question = "I don't trust stairs.",
                    Answer = "They're always up to something"
                });
                context.SaveChanges();

                context.Jokes.Add(new Joke
                {
                    JokeId = 1001,
                    Question = "Three fish are in a tank",
                    Answer = "One asks the others, 'How do you drive this thing?'"
                });
                context.SaveChanges();
            }

            JokesContext _context = new JokesContext(_options);

            JokeService _jokeService = new JokeService(_context);

            string questionSearchCriteria = "ta";

            // Act

            var result = _jokeService.GetJokesBySearch(questionSearchCriteria);

            // Assert

            Assert.That(result.Result.Count == 2);
            Assert.That(result.Result[0].JokeId == 1000);
            Assert.That(result.Result[1].JokeId == 1001);
            _context.Dispose();

        }

        [Test]
        public void GetJokesByPartialSearchViaAnswer_OneMatchingJoke()
        {
            // Arrange

            // set up the Database
            using (var context = new JokesContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.Jokes.Add(new Joke
                {
                    JokeId = 1000,
                    Question = "I don't trust stairs.",
                    Answer = "They're always up to something"
                });
                context.SaveChanges();

                context.Jokes.Add(new Joke
                {
                    JokeId = 1001,
                    Question = "Three fish are in a tank",
                    Answer = "One asks the others, 'How do you drive this thing?'"
                });
                context.SaveChanges();
            }

            JokesContext _context = new JokesContext(_options);

            JokeService _jokeService = new JokeService(_context);

            string answerSearchCriteria = "always";

            // Act

            var result = _jokeService.GetJokesBySearch(answerSearchCriteria);

            // Assert

            Assert.That(result.Result.Count == 1);
            Assert.That(result.Result[0].JokeId == 1000);
            _context.Dispose();

        }

        [Test]
        public void GetAllJokes_TwoTotalJokesWithMatchingJokeTableCount()
        {
            // Arrange

            // set up the Database
            using (var context = new JokesContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.Jokes.Add(new Joke
                {
                    JokeId = 1000,
                    Question = "I don't trust stairs.",
                    Answer = "They're always up to something"
                });
                context.SaveChanges();
            }

            JokesContext _context = new JokesContext(_options);

            JokeService _jokeService = new JokeService(_context);

            int expectedJokeCount = 2;

            // Act

            var result = _jokeService.GetAllJokes();

            // Assert

            Assert.That(result.Count() == 2);
            Assert.That(_context.Jokes.Count() == result.Count());

            _context.Dispose();
        }

        [Test]
        public void GetSingleJokeByID_OneJokeReturnedMatchingID()
        {
            // Arrange

            // set up the Database
            using (var context = new JokesContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.Jokes.Add(new Joke
                {
                    JokeId = 1000,
                    Question = "I don't trust stairs.",
                    Answer = "They're always up to something"
                });
                context.SaveChanges();
            }

            JokesContext _context = new JokesContext(_options);

            JokeService _jokeService = new JokeService(_context);

            string expectedJokeQuestion = "I don't trust stairs.";
            int expectedJokeID = 1000;

            // Act

            var result = _jokeService.GetJokeById(expectedJokeID);

            // Assert

            Assert.NotNull(result);
            Assert.That(result.Question.Equals(expectedJokeQuestion));
            _context.Dispose();

        }
    }
}