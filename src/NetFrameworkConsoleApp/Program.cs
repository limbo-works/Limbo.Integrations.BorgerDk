using System;
using Limbo.Integrations.BorgerDk;

namespace NetFrameworkConsoleApp {

    internal class Program {

        static void Main(string[] args) {

            Console.WriteLine("Hello World!");

            BorgerDkHttpService http = new BorgerDkHttpService(BorgerDkEndpoint.Default);

            var articles = http.GetArticleList();

            Console.WriteLine("Number of articles: " + articles.Length);
            Console.WriteLine();

            var article = http.GetArticleFromId(articles[0].Id, BorgerDkMunicipality.VejleKommune);

            Console.WriteLine("ID:      " + article.Id);
            Console.WriteLine("Title:   " + article.Title);
            Console.WriteLine("Header:  " + article.Header);
            Console.WriteLine("By line: " + article.ByLine);
            Console.WriteLine();

        }

    }

}