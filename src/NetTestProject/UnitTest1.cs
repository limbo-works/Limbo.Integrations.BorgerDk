using Limbo.Integrations.BorgerDk;

namespace NetTestProject;

[TestClass]
public class UnitTest1 {

    [TestMethod]
    public void GetArticleList1() {

        // Initialize a new HTTP service
        BorgerDkHttpService http = new(BorgerDkEndpoint.Default);

        // Get the article list
        IReadOnlyList<BorgerDkArticleDescription> articles = http.GetArticleList();

        // Should have more than 300 articles
        Assert.IsTrue(articles.Count > 300);

    }

    [TestMethod]
    public void GetArticleList2() {

        // Initialize a new HTTP service
        BorgerDkHttpService http = new(BorgerDkEndpoint.LifeInDenmark);

        // Get the article list
        IReadOnlyList<BorgerDkArticleDescription> articles = http.GetArticleList();

        // Should have more than 150 articles
        Assert.IsTrue(articles.Count > 150);

    }

    [TestMethod]
    public void GetById() {

        // Initialize a new HTTP service
        BorgerDkHttpService http = new(BorgerDkEndpoint.Default);

        // Get the article with ID "11203"
        BorgerDkArticle article = http.GetArticleFromId(11203, BorgerDkMunicipality.VejleKommune);

        // Validate the article
        Assert.AreEqual(11203, article.Id);
        Assert.AreEqual("Skattefri præmie", article.Title);
        Assert.AreEqual("Hvis du arbejder i efterlønsperioden, kan du eventuelt opnå ret til en skattefri præmie", article.Header);
        Assert.AreEqual("Skrevet af Styrelsen for Arbejdsmarked og Rekruttering", article.ByLine);
        Assert.AreEqual("https://www.borger.dk/pension-og-efterloen/Efterloen-fleksydelse-delpension/efterloen/skattefri-praemie", article.Url);

    }

    [TestMethod]
    public void GetByUrl() {

        // Initialize a new HTTP service
        BorgerDkHttpService http = new(BorgerDkEndpoint.Default);

        // Get the article with ID "11203"
        BorgerDkArticleShortDescription article = http.GetArticleIdFromUrl("https://www.borger.dk/pension-og-efterloen/Efterloen-fleksydelse-delpension/efterloen/skattefri-praemie");

        // Validate the article item
        Assert.AreEqual(11203, article.Id);
        Assert.AreEqual("Skattefri præmie", article.Title);

    }

}