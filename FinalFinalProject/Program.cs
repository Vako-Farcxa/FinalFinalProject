using Repositories;
using Models;


namespace FinalFinalProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var userManager = new UserManager();
            var quizManager = new QuizManager();
            var quizAttemptManager = new QuizAttemptManager();

            User loggedInUser = null;

            while (true)
            {
                Console.Clear();
                if (loggedInUser == null)
                {
                    Console.WriteLine("1. Login");
                    Console.WriteLine("2. Register");
                    Console.WriteLine("0. Exit");
                    Console.Write("Choose an option: ");
                    var choice = Console.ReadLine();

                    if (choice == "1")
                    {
                        Console.Write("Username: ");
                        var username = Console.ReadLine();
                        Console.Write("Password: ");
                        var password = Console.ReadLine();

                        if (userManager.ValidateUser(username, password))
                        {
                            loggedInUser = userManager.GetUser(username);
                            Console.WriteLine("Login successful!");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("Invalid credentials.");
                            Console.ReadKey();
                        }
                    }
                    else if (choice == "2")
                    {
                        Console.Write("Username: ");
                        var username = Console.ReadLine();
                        Console.Write("Password: ");
                        var password = Console.ReadLine();

                        try
                        {
                            userManager.AddUser(new User { Username = username, Password = password });
                            Console.WriteLine("Registration successful!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        Console.ReadKey();
                    }
                    else if (choice == "0")
                    {
                        break;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine($"Welcome, {loggedInUser.Username}!");
                    Console.WriteLine("1. Create a Quiz");
                    Console.WriteLine("2. Attempt a Quiz");
                    Console.WriteLine("3. View Personal Record");
                    Console.WriteLine("4. View Leaderboard");
                    Console.WriteLine("5. Manage My Quizzes");
                    Console.WriteLine("0. Logout");
                    Console.Write("Choose an option: ");
                    var choice = Console.ReadLine();

                    if (choice == "1")
                    {
                        Console.Clear();
                        Console.Write("Enter Quiz Title: ");
                        var title = Console.ReadLine();
                        var questions = new List<Question>();

                        for (int i = 1; i <= 5; i++)
                        {
                            Console.Clear();
                            Console.Write($"Enter Question {i}: ");
                            var questionText = Console.ReadLine();

                            var options = new List<string>();
                            for (int j = 1; j <= 4; j++)
                            {
                                Console.Write($"Option {j}: ");
                                options.Add(Console.ReadLine());
                            }

                            Console.Write("Enter the index (1-4) of the correct option: ");
                            int correctIndex = int.Parse(Console.ReadLine()) - 1;

                            questions.Add(new Question
                            {
                                QuestionText = questionText,
                                Options = options,
                                CorrectOptionIndex = correctIndex
                            });
                        }

                        quizManager.AddQuiz(new Quiz
                        {
                            Id = (quizManager.GetQuizzes().Count + 1).ToString(),
                            Title = title,
                            Questions = questions,
                            Creator = loggedInUser.Username
                        });

                        Console.WriteLine("Quiz created successfully!");
                        Console.ReadKey();
                    }
                    else if (choice == "2")
                    {
                        Console.Clear();
                        var availableQuizzes = quizManager.GetQuizzesNotCreatedBy(loggedInUser.Username);

                        if (availableQuizzes.Count == 0)
                        {
                            Console.WriteLine("No quizzes available to attempt.");
                            Console.ReadKey();
                            continue;
                        }

                        Console.WriteLine("Available Quizzes:");
                        for (int i = 0; i < availableQuizzes.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {availableQuizzes[i].Title}");
                        }

                        Console.Write("Choose a quiz to attempt (number): ");
                        int quizChoice = int.Parse(Console.ReadLine()) - 1;

                        if (quizChoice < 0 || quizChoice >= availableQuizzes.Count)
                        {
                            Console.WriteLine("Invalid choice.");
                            Console.ReadKey();
                            continue;
                        }

                        var selectedQuiz = availableQuizzes[quizChoice];
                        int score = 0;
                        var startTime = DateTime.Now;

                        foreach (var question in selectedQuiz.Questions)
                        {
                            Console.Clear();
                            Console.WriteLine(question.QuestionText);
                            for (int i = 0; i < question.Options.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {question.Options[i]}");
                            }

                            Console.Write("Your answer (1-4): ");
                            int answer;
                            if (!int.TryParse(Console.ReadLine(), out answer) || answer < 1 || answer > 4)
                            {
                                Console.WriteLine("Invalid answer.");
                                continue;
                            }

                            if ((DateTime.Now - startTime).TotalMinutes > 2)
                            {
                                Console.WriteLine("Time's up! You failed the quiz.");
                                score = 0;
                                break;
                            }

                            if (answer - 1 == question.CorrectOptionIndex)
                                score += 20;
                            else
                                score -= 20;
                        }

                        if (score > 0)
                        {
                            quizAttemptManager.AddResult(new UserResult
                            {
                                Username = loggedInUser.Username,
                                QuizId = selectedQuiz.Id,
                                QuizTitle = selectedQuiz.Title,
                                Score = score,
                                AttemptDate = DateTime.Now
                            });

                            Console.WriteLine($"Quiz completed! Your score: {score}");
                        }
                        else
                        {
                            Console.WriteLine("You failed the quiz.");
                        }
                        Console.ReadKey();
                    }
                    else if (choice == "3")
                    {
                        Console.Clear();
                        var userResults = quizAttemptManager.GetUserResults(loggedInUser.Username);

                        if (userResults.Count == 0)
                        {
                            Console.WriteLine("No records found.");
                        }
                        else
                        {
                            foreach (var result in userResults)
                            {
                                Console.WriteLine($"Quiz: {result.QuizTitle}, Score: {result.Score}, Date: {result.AttemptDate}");
                            }
                        }
                        Console.ReadKey();
                    }
                    else if (choice == "4")
                    {
                        Console.Clear();
                        Console.WriteLine("Leaderboard:");
                        var leaderboard = quizAttemptManager.GetLeaderboard();

                        foreach (var entry in leaderboard)
                        {
                            Console.WriteLine($"User: {entry.Username}, Total Score: {entry.TotalScore}");
                        }
                        Console.ReadKey();
                    }
                    else if (choice == "5")
                    {
                        Console.Clear();
                        var userQuizzes = quizManager.GetQuizzes()
                            .Where(q => q.Creator == loggedInUser.Username).ToList();

                        if (userQuizzes.Count == 0)
                        {
                            Console.WriteLine("You have no quizzes.");
                            Console.ReadKey();
                            continue;
                        }

                        Console.WriteLine("Your Quizzes:");
                        for (int i = 0; i < userQuizzes.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {userQuizzes[i].Title}");
                        }

                        Console.Write("Choose a quiz to edit/delete (number): ");
                        int quizChoice = int.Parse(Console.ReadLine()) - 1;

                        if (quizChoice < 0 || quizChoice >= userQuizzes.Count)
                        {
                            Console.WriteLine("Invalid choice.");
                            Console.ReadKey();
                            continue;
                        }

                        Console.WriteLine("1. Edit Quiz");
                        Console.WriteLine("2. Delete Quiz");
                        Console.Write("Choose an option: ");
                        var action = Console.ReadLine();

                        if (action == "1")
                        {
                            var quizToEdit = userQuizzes[quizChoice];
                            Console.Clear();
                            Console.WriteLine($"Editing Quiz: {quizToEdit.Title}");

                            quizManager.UpdateQuiz(userQuizzes[quizChoice].Creator, userQuizzes[quizChoice].Id);

                            Console.WriteLine("Quiz updated successfully!");
                        }
                        else if (action == "2")
                        {
                            quizManager.DeleteQuiz(userQuizzes[quizChoice].Creator, userQuizzes[quizChoice].Id);
                            Console.WriteLine("Quiz deleted successfully!");
                        }

                        Console.ReadKey();
                    }
                    else if (choice == "0")
                    {
                        loggedInUser = null;
                        Console.WriteLine("Logged out.");
                        Console.ReadKey();
                    }
                }
            }
        }
    }
}
