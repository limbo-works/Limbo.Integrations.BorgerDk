using Limbo.Integrations.BorgerDk;
using Limbo.Integrations.BorgerDk.Elements;
using Skybrud.Essentials.Json.Newtonsoft;

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

    [TestMethod]
    public void ParseArticle() {

        string dir = Path.GetDirectoryName(typeof(UnitTest1).Assembly.Location)!;

        string path1 = Path.Combine(dir, "flytning.json");

        BorgerDkArticle article = new(JsonUtils.LoadJsonObject(path1));

        Assert.AreEqual(645, article.Id);
        Assert.AreEqual("www.borger.dk", article.Domain);
        Assert.AreEqual("https://www.borger.dk/sider/Naar-du-skal-flytte.aspx", article.Url);
        Assert.AreEqual(0, article.Municipality.Code);
        Assert.AreEqual("Flytning i Danmark", article.Title);
        Assert.AreEqual("Du skal melde flytning til kommunen senest 5 dage, efter du har skiftet bopæl", article.Header);
        Assert.AreEqual("Skrevet af borger.dk", article.ByLine);

        IReadOnlyList<BorgerDkMicroArticle>? microArticles = article.Elements
            .OfType<BorgerDkBlockElement>()
            .FirstOrDefault()?
            .MicroArticles;

        Assert.IsNotNull(microArticles);
        Assert.AreEqual(13, microArticles.Count);

        const string expectedContent = """
            <h2>Oplys kommunen om ny adresse og vælg læge</h2><div><p>Du har pligt til at oplyse din nye adresse senest fem dage, efter du har skiftet bopæl. Flyttedatoen er den dag, du faktisk er flyttet ind. Det vil sige, når du har flyttet dine ejendele og overnatter på adressen.</p>
            <ul>
                <li>Du kan melde flytning for dig selv og for de øvrige beboere i husstanden.</li>
                <li>Du kan tidligst anmelde flytning fire uger, før du skifter adresse. </li>
                <li>Du skal melde din flytning digitalt, og du vælger samtidigt din læge. </li>
                <li> PostNord får automatisk besked om din flytning, når den træder i kraft. (Dog ikke, hvis du har navne- og adressebeskyttelse.)</li>
                <li>Husk at sætte navn på din nye brevkasse. Mindst efternavnene på de postmodtagere, der anvender brevkassen, men også gerne fornavnene, for det mindsker risikoen for fejl.</li>
            </ul>
            <h5>Midlertidig adresseflytning</h5>
            <p>Har du i en afgrænset periode brug for at få din post sendt til en anden adresse end din folkeregisteradresse, skal du give dine afsendere besked.</p>
            <h5>Navne- og adressebeskyttelse</h5>
            <p>Hvis du ønsker navne- og adressebeskyttelse, skal du huske at søge om det i forbindelse med din flytteanmeldelse.</p>
            <p>Bemærk også, at du selv skal give besked til PostNord, hvis du har eller vil have navne- og adressebeskyttelse hos dem. Dermed frabeder du dig videregivelse af information til andre distributionsselskaber end PostNord.</p>
            <ul>
                <li><a href="https://kundeservice.postnord.dk/s/article/%C3%A6ndre-adresse-jeg-skal-flytte-da-PrivateExternal?language=da" title="PostNord om flytning og eftersendelse af post" target="_blank" aria-label="Flytning og eftersendelse af post hos PostNord (nyt vindue) åbner i nyt vindue" data-personalized="false">Flytning og eftersendelse af post hos PostNord</a></li>
            </ul>
            <h5>Ingen MitID </h5>
            <p>Kommunens borgerservice kan hjælpe dig, hvis du ikke har MitID</p>
            <h5>Bøde
            </h5>
            <p>Hvis du ikke anmelder din flytning i tide, kan du få en bøde.</p>
            <h5>Hvad er en c/o-adresse?</h5>
            <p>Hvis du flytter ind hos en ejer eller lejer, og dit eget navn ikke står på døren, skal du angive ejerens eller lejerens navn i feltet 'Hos (c/o er en forkortelse for det engelske udtryk "Care of")'. Du kan ikke registreres i CPR på en c/o-adresse, hvis du ikke rent faktisk også bor der. </p>
            <p>Kommunen kan kræve, at ejeren/lejeren med en logiværtserklæring bekræfter, at du bor hos ham eller hende.</p></div>
            """;

        // Validate the micro article title
        Assert.AreEqual("Oplys kommunen om ny adresse og vælg læge", microArticles[0].Title);

        // Validate the micro article content (which should include the title/heading as well)
        Assert.AreEqual(expectedContent.Replace("\r\n", "\n"), microArticles[0].Content.Replace("\r\n", "\n"));

    }

}