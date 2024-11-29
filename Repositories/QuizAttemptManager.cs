using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using static Repositories.QuizAppData;

namespace Repositories
{
    public class QuizAttemptManager
    {
        private List<UserResult> _results;

        public QuizAttemptManager()
        {
            FilePaths.EnsureFilesExist();
            _results = JsonDataHandler.ReadData<UserResult>(FilePaths.ResultsFile);
        }

        public List<UserResult> GetUserResults(string username)
        {
            return _results.Where(result => result.Username == username).ToList();
        }

        public void AddResult(UserResult result)
        {
            _results.Add(result);
            SaveResults();
        }

        public List<UserResult> GetResults() => _results;

        public List<(string Username, int TotalScore)> GetLeaderboard()
        {
            return _results
                .GroupBy(r => r.Username)
                .Select(group => new
                {
                    Username = group.Key,
                    TotalScore = group.Sum(r => r.Score)
                })
                .OrderByDescending(entry => entry.TotalScore)
                .Take(10)
                .Select(entry => (entry.Username, entry.TotalScore))
                .ToList();
        }

        private void SaveResults()
        {
            JsonDataHandler.WriteData(FilePaths.ResultsFile, _results);
        }
    }
}
