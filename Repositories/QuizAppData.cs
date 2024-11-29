using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Repositories
{
    public class QuizAppData
    {
        public static class FilePaths
        {
            public static string UsersFile => @"C:\Users\vako\Desktop\final\FinalFinalProject\Repositories\users.json";
            public static string QuizzesFile => @"C:\\Users\\vako\\Desktop\\final\\FinalFinalProject\\Repositories\\quizzes.json";
            public static string ResultsFile => @"C:\Users\vako\Desktop\final\FinalFinalProject\Repositories\results.json";

            public static void EnsureFilesExist()
            {
                if (!File.Exists(UsersFile)) File.WriteAllText(UsersFile, "[]");
                if (!File.Exists(QuizzesFile)) File.WriteAllText(QuizzesFile, "[]");
                if (!File.Exists(ResultsFile)) File.WriteAllText(ResultsFile, "[]");
            }
        }

        public static class JsonDataHandler
        {
            public static List<T> ReadData<T>(string filePath)
            {
                if (!File.Exists(filePath)) return new List<T>();
                var jsonData = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<T>>(jsonData) ?? new List<T>();
            }

            public static void WriteData<T>(string filePath, List<T> data)
            {
                var jsonData = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(filePath, jsonData);
            }
        }
    }
}
