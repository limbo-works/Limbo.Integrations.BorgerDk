# Skybrud.Integrations.BorgerDk

.NET library for communicating with the Borger.dk web service.


<br /><br /><br />

## Installation

### NuGet

The package can be installed via [**NuGet**](https://www.nuget.org/packages/Limbo.Integrations.BorgerDk):

Either via the .NET CLI:

```
dotnet add package Limbo.Integrations.BorgerDk
```

or the NuGet Package Manager:

```
Install-Package Limbo.Integrations.BorgerDk
```

### Target Frameworks

The package is build against the following .NET versions:

- .NET Framework 4.5
- .NET Framework 4.6
- .NET Framework 4.7
- .NET Standard 2.0




<br /><br /><br />

## Usage

Below you'll find various examples on how to use the package:

<br /><br />

### Endpoints

Borger.dk currently has two different endpoints. The main endpoint for the articles at [www.borger.dk](https://www.borger.dk/), and then a secondary endpoint with articles targeting an English audience at [lifeindenmark.borger.dk](https://lifeindenmark.borger.dk/).

The endpoint are represented in this package by the `BorgerDkEndpoint` class and the two `BorgerDkEndpoint.Default` and `BorgerDkEndpoint.LifeInDenmark` fields.



<br /><br />

### HTTP Service

The `BorgerDkHttpService` class serves as the main component in wrapping the Borger.dk API/web service. You might create a new instance like this:

```csharp
BorgerDkHttpService http = new BorgerDkHttpService(BorgerDkEndpoint.Default);
```



<br /><br />

### Getting all articles

Borger.dk contains articles that live at a global level, meaning they are shared across Denmarks municipalities. Each article will consist of mostly global information, but may have some local content that describe a specific municipality.

When only looking at the article list, we don't look at the content, so for this part, the municipality is not relevant. As such, the article list can be retrieved as:

```csharp
// Initialize a new HTTP service instance from the default endpoint
BorgerDkHttpService http = new BorgerDkHttpService(BorgerDkEndpoint.Default);

// Fetch the article list from the web service
IReadOnlyList<BorgerDkArticleDescription> articles = http.GetArticleList();

Console.WriteLine("Number of articles: " + articles.Length);
Console.WriteLine();

// Iterate through the articles
foreach (BorgerDkArticleDescription article in articles) {
    
    Console.WriteLine("ID:           " + article.Id);
    Console.WriteLine("Title:        " + article.Title);
    Console.WriteLine("URL:          " + article.Url);
    Console.WriteLine("Publish date: " + article.PublishDate);
    Console.WriteLine("Update date:  " + article.UpdateDate);
    Console.WriteLine();

}
```



<br /><br />

### Lookup by URL

The Borger.dk web services lets you look up the ID of an given article from it's website URL. Similar to when getting the article list, we're not working with the article content, so we don't need to specify a municipality for this.

```csharp
// Initialize a new HTTP service instance from the default endpoint
BorgerDkHttpService http = new BorgerDkHttpService(BorgerDkEndpoint.Default);

// Look up the URL in the web service
BorgerDkArticleShortDescription article = http.GetArticleIdFromUrl("https://www.borger.dk/miljoe-og-energi/Affald-og-genbrug/Affaldsordninger");

Console.WriteLine("ID:           " + article.Id);
Console.WriteLine("Title:        " + article.Title);
```

The `GetArticleIdFromUrl` method will return an instance of `BorgerDkArticleShortDescription`, which contains properties for the `Id` and `Title` of the article.




<br /><br />

### Lookup by ID

Looking up an article via it's ID returns the full article, including it's content. And as the content may contain elements that are specific to your given municipality, you should ideally specify one as the second parameter for the `GetArticleFromId` method.

The example below specified `BorgerDkMunicipality.VejleKommune` for Vejle Kommune. If no municipality in particular apply to your case, you may specify `BorgerDkMunicipality.NoMunicipality` instead.

```csharp
// Initialize a new HTTP service instance from the default endpoint
BorgerDkHttpService http = new BorgerDkHttpService(BorgerDkEndpoint.Default);

// Look up the article from it's ID
BorgerDkArticle article = http.GetArticleFromId(486, BorgerDkMunicipality.VejleKommune);

Console.WriteLine("ID:           " + article.Id);
Console.WriteLine("Title:        " + article.Title);
Console.WriteLine("Title:        " + article.Header);
Console.WriteLine("Title:        " + article.PublishDate);
Console.WriteLine("Title:        " + article.UpdateDate);
Console.WriteLine();

// Iterate through the elements making up the content
foreach (BorgerDkElement element in article.Elements) {

    switch (element) {

        case BorgerDkTextElement text:
            Console.WriteLine(text.Id);
            Console.WriteLine(text.Title);
            Console.WriteLine(text.Content);
            Console.WriteLine();
            break;

        case BorgerDkBlockElement block:

            // Iterate through the micro articles of the main block element
            foreach (BorgerDkMicroArticle microArticle in block.MicroArticles) {
                Console.WriteLine(microArticle.Id);
                Console.WriteLine(microArticle.Title);
                Console.WriteLine(microArticle.Content);
                Console.WriteLine();
            }
            break;
        
    }

}
```            

Articles typically consist of some info boxes (`BorgerDkTextElement`) and then a single block element (`BorgerDkBlockElement`) consisting of multiple micro articles (`BorgerDkTextElement`).



<br /><br />

### Exceptions

When trying to look up an article by a URL or ID that does not exist, the package will throw an exception of type `BorgerDkNotFoundException`.

You should also be aware that some articles are protected from export (typically articles for areas that are not handled by the individual municipalities, but by the state). If you try looking up such an article, an exception of type `BorgerDkNotExportableException` will be thrown.



<br /><br /><br />

## Development

The package contains a SOAP client for the communication with the Borger.dk web service. The client can be updated by running the following command:

```
svcutil.exe https://www.borger.dk/_vti_bin/borger/ArticleExport.svc?wsdl
```

Calling `svcutil.exe` with the above URL will generate the necessary classes for making object oriented calls to the web service.
