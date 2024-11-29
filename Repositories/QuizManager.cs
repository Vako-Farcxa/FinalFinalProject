using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using static Repositories.QuizAppData;

namespace Repositories
{
    public class QuizManager
    {
        private List<Quiz> _quizzes;

        public QuizManager()
        {
            FilePaths.EnsureFilesExist();
            _quizzes = JsonDataHandler.ReadData<Quiz>(FilePaths.QuizzesFile);
        }

        public List<Quiz> GetQuizzesNotCreatedBy(string username)
        {
            return _quizzes.Where(quiz => quiz.Creator != username).ToList();
        }

        public void AddQuiz(Quiz quiz)
        {
            _quizzes.Add(quiz);
            SaveQuizzes();
        }

        public List<Quiz> GetQuizzes() => _quizzes;

        private void SaveQuizzes()
        {
            JsonDataHandler.WriteData(FilePaths.QuizzesFile, _quizzes);
        }

        public void UpdateQuiz(string username, string quizId)
        {
            var quiz = _quizzes.FirstOrDefault(q => q.Id == quizId);

            if (quiz == null)
            {
                Console.WriteLine("Quiz not found.");
                return;
            }

            if (quiz.Creator != username)
            {
                Console.WriteLine("You can only update quizzes you have created.");
                return;
            }

            Console.Write("Enter the new title for the quiz: ");
            string newTitle = Console.ReadLine();

            List<Question> newQuestions = new List<Question>();
            for (int i = 1; i <= 5; i++) 
            {
                Console.WriteLine($"Enter details for Question {i}:");
                Console.Write("Question text: ");
                string questionText = Console.ReadLine();

                List<string> answers = new List<string>();
                for (int j = 1; j <= 4; j++) 
                {
                    Console.Write($"Answer {j}: ");
                    answers.Add(Console.ReadLine());
                }

                Console.Write("Enter the correct answer (1, 2, 3, or 4): ");
                int correctAnswer = int.Parse(Console.ReadLine());

                newQuestions.Add(new Question
                {
                    QuestionText = questionText,
                    Options = answers,
                    CorrectOptionIndex = correctAnswer - 1
                });
            }

            quiz.Title = newTitle;
            quiz.Questions = newQuestions;

            Console.WriteLine("Quiz updated successfully.");
        }

        public void DeleteQuiz(string username, string quizId)
        {
            var quiz = _quizzes.FirstOrDefault(q => q.Id == quizId);

            if (quiz == null)
            {
                Console.WriteLine("Quiz not found.");
                return;
            }

            if (quiz.Creator != username)
            {
                Console.WriteLine("You can only delete quizzes you have created.");
                return;
            }

            _quizzes.Remove(quiz);

            Console.WriteLine("Quiz deleted successfully.");
        }
    }
}
