﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using Limbo.Integrations.BorgerDk.Elements;
using Limbo.Integrations.BorgerDk.WebService;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Extensions;
using Skybrud.Essentials.Strings;
using Skybrud.Essentials.Time;

namespace Limbo.Integrations.BorgerDk;

/// <summary>
/// Class representing an article received from the Borger.dk web service.
/// </summary>
public class BorgerDkArticle {

    #region Properties

    /// <summary>
    /// The ID of the article.
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; }

    /// <summary>
    /// The domain for the article
    /// </summary>
    [JsonProperty("domain")]
    public string Domain { get; }

    /// <summary>
    /// The url for the article
    /// </summary>
    [JsonProperty("url")]
    public string Url { get; }

    /// <summary>
    /// Gets a reference to the municipality of the article.
    /// </summary>
    [JsonProperty("municipality")]
    public BorgerDkMunicipality Municipality { get; }

    /// <summary>
    /// The title of the article.
    /// </summary>
    [JsonProperty("title")]
    public string Title { get; }

    /// <summary>
    /// The header of the article.
    /// </summary>
    [JsonProperty("header")]
    public string Header { get; }

    /// <summary>
    /// Gets the byline with information about who has written the article.
    /// </summary>
    [JsonProperty("byline")]
    public string ByLine { get; }

    /// <summary>
    /// The date for when the article was published
    /// </summary>
    [JsonProperty("publishDate")]
    public EssentialsTime PublishDate { get; }

    /// <summary>
    /// The date for when the article was last updated.
    /// </summary>
    [JsonProperty("updateDate")]
    public EssentialsTime UpdateDate { get; }

    /// <summary>
    /// Gets the raw HTML making up the content of the article.
    /// </summary>
    [JsonProperty("content")]
    public string Content { get; }

    /// <summary>
    /// Gets an array of all elements parsed from the article content.
    /// </summary>
    [JsonIgnore]
    public IReadOnlyList<BorgerDkElement> Elements { get; }

    #endregion

    #region Constructors

    public BorgerDkArticle(JObject obj) {

        Id = obj.GetInt32("id");
        Domain = obj.GetString("domain")!;
        Url = obj.GetString("url")!;
        Municipality = obj.GetInt32("municipality", BorgerDkMunicipality.GetFromCode)!;
        Title = obj.GetString("title")!;
        Header = obj.GetString("header")!;
        ByLine = obj.GetString("byline")!;
        PublishDate = obj.GetString("publishDate", EssentialsTime.Parse)!;
        UpdateDate = obj.GetString("updateDate", EssentialsTime.Parse)!;
        Content = obj.GetString("content")!;

        Elements = ParseElements(Content);

    }

    private BorgerDkArticle(BorgerDkHttpService service, Article article, BorgerDkMunicipality municipality) {

        if (municipality == null) throw new ArgumentNullException(nameof(municipality));

        // Check if "service" or "article" is null
        if (service == null) throw new ArgumentNullException(nameof(service));
        if (article == null) throw new ArgumentNullException(nameof(article));

        // Get the Danish time zone
        TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");

        // Assume the timstamp are specified according to the Danish time zone
        DateTimeOffset published = new(article.PublishingDate, tz.GetUtcOffset(article.PublishingDate));
        DateTimeOffset updated = new(article.LastUpdated, tz.GetUtcOffset(article.LastUpdated));

        // Populate basic properties
        Id = article.ArticleID;
        Domain = service.Endpoint.Domain;
        Url = article.ArticleUrl.Split('?')[0];
        Municipality = municipality;
        Title = HttpUtility.HtmlDecode(article.ArticleTitle);
        Header = HttpUtility.HtmlDecode(article.ArticleHeader);
        PublishDate = new EssentialsTime(published, tz);
        UpdateDate = new EssentialsTime(updated, tz);
        Content = article.Content;

        Elements = ParseElements(Content);

        ByLine = StringUtils.StripHtml(Elements.OfType<BorgerDkTextElement>().FirstOrDefault(x => x.Id == "byline")?.Content) ?? string.Empty;

    }

    #endregion

    private IReadOnlyList<BorgerDkElement> ParseElements(string content) {

        HtmlDocument htmlDocument = new();
        htmlDocument.LoadHtml(content);
        htmlDocument.OptionOutputAsXml = false;

        List<BorgerDkElement> elements = new();

        foreach (HtmlNode node in htmlDocument.DocumentNode.ChildNodes) {

            if (node is HtmlTextNode || node.Attributes["id"] == null) continue;

            string id = node.Attributes["id"].Value;

            if (id == "kernetekst") {

                List<BorgerDkMicroArticle> microArticles = new();

                // Initialize a new block element
                BorgerDkBlockElement block = new(id, microArticles);

                foreach (HtmlNode child in node.ChildNodes) {

                    if (child is HtmlTextNode || child.Attributes["id"] == null) continue;

                    // Get the ID of the micro article
                    string microId = child.Attributes["id"].Value.Replace("microArticle_", "");

                    HtmlNode[] children = GetNonTextChildren(child);

                    // Trigger exception if empty
                    if (children.Length == 0) throw new Exception("What's happening? #1 (" + microId + ")");

                    // Trigger exception if no <h2> (or <h3> - thanks for that change)
                    if (children[0].Name != "h2" && children[0].Name != "h3") throw new Exception("What's happening? #2 (" + microId + ")");

                    // Get the title from the <h2>
                    string title = children[0].InnerText.Trim();

                    // Parse the text content
                    string text = FixSimpleErrors(child.InnerHtml.Trim());

                    // Initialize a new micro article
                    BorgerDkMicroArticle micro = new(block, microId, title, children[0].Name, text);

                    // Append the micro article
                    microArticles.Add(micro);

                }

                // Append the element
                elements.Add(block);

            } else if (id == "byline") {

                // Initialize a new text element
                BorgerDkTextElement element = new(id, "Skrevet af", node.InnerText.Trim());

                // Append the element
                elements.Add(element);

            } else {

                HtmlNode[] children = GetNonTextChildren(node);

                // Handle if empty
                if (children.Length == 0) {
                    // throw new Exception("What's happening? #1 (" + id + ")");
                    continue;
                }

                // Handle if no <h3>
                if (children[0].Name != "h3") {
                    //throw new Exception("What's happening? #2 (" + id + ")");
                    continue;
                }

                // Get the title from the <h3>
                string title = children[0].InnerText;

                // Parse the text content
                string text = FixSimpleErrors(node.InnerHtml);

                // Initialize a new text element
                BorgerDkTextElement element = new(id, title, text);

                // Append the element
                elements.Add(element);

            }

        }

        return elements.ToArray();

    }

    #region Static methods

    private static HtmlNode[] GetNonTextChildren(HtmlNode node) {
        return node.ChildNodes.Where(child => child is not HtmlTextNode).ToArray();
    }

    private static string FixSimpleErrors(string str) {

        // Replace non-breaking spaces as they typically appear to be inserted by mistake
        str = str.Replace((char) 160, ' ');

        return str;

    }

    public static BorgerDkArticle GetFromArticle(BorgerDkHttpService service, Article article) {
        return GetFromArticle(service, article, BorgerDkMunicipality.NoMunicipality);
    }

    public static BorgerDkArticle GetFromArticle(BorgerDkHttpService service, Article article, BorgerDkMunicipality municipality) {

        if (municipality == null) throw new ArgumentNullException(nameof(municipality));

        // Check if "service" or "article" is null
        if (service == null) throw new ArgumentNullException(nameof(service));
        if (article == null) throw new ArgumentNullException(nameof(article));

        return new BorgerDkArticle(service, article, municipality);

    }

    #endregion

}