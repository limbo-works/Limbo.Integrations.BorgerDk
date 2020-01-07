﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using HtmlAgilityPack;
using Skybrud.Essentials.Time;
using Skybrud.Integrations.BorgerDk.Elements;
using Skybrud.Integrations.BorgerDk.WebService;

namespace Skybrud.Integrations.BorgerDk {

    /// <summary>
    /// Class representing an article received from the Borger.dk web service.
    /// </summary>
    public class BorgerDkArticle {

        #region Properties

        /// <summary>
        /// The ID of the article.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// The domain for the article
        /// </summary>
        public string Domain { get; }

        /// <summary>
        /// The url for the article
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// Gets a reference to the municipality of the article.
        /// </summary>
        public BorgerDkMunicipality Municipality { get; }

        /// <summary>
        /// The title of the article.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// The header of the article.
        /// </summary>
        public string Header { get; }

        /// <summary>
        /// The date for when the article was published
        /// </summary>
        public EssentialsTime Published { get; }

        /// <summary>
        /// The date for when the article was last modified
        /// </summary>
        public EssentialsTime Modified { get; }

        /// <summary>
        /// Gets the raw HTML making up the content of the article.
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// Gets an array of all elements parsed from the article content.
        /// </summary>
        public BorgerDkElement[] Elements { get; }

        /// <summary>
        /// Gets the byline of the article.
        /// </summary>
        public string ByLine { get; }
        
        #endregion

        #region Constructors

        private BorgerDkArticle(BorgerDkService service, Article article, BorgerDkMunicipality municipality) {

            // Check if "service" or "article" is null
            if (service == null) throw new ArgumentNullException(nameof(service));
            if (article == null) throw new ArgumentNullException(nameof(article));

            municipality = municipality ?? BorgerDkMunicipality.NoMunicipality;

            // Get the Danish time zone
            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");

            // Assume the timstamp are specified according to the Danish time zone
            DateTimeOffset published = new DateTimeOffset(article.PublishingDate, tz.GetUtcOffset(article.PublishingDate));
            DateTimeOffset updated = new DateTimeOffset(article.LastUpdated, tz.GetUtcOffset(article.LastUpdated));

            // Populate basic properties
            Id = article.ArticleID;
            Domain = service.Endpoint.Domain;
            Url = article.ArticleUrl.Split('?')[0];
            Municipality = municipality;
            Title = HttpUtility.HtmlDecode(article.ArticleTitle);
            Header = HttpUtility.HtmlDecode(article.ArticleHeader);
            Published = new EssentialsTime(published, tz);
            Modified = new EssentialsTime(updated, tz);
            Content = article.Content;

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(article.Content);
            htmlDocument.OptionOutputAsXml = false;

            List<BorgerDkElement> elements = new List<BorgerDkElement>();

            foreach (HtmlNode node in htmlDocument.DocumentNode.ChildNodes) {

                if (node is HtmlTextNode || node.Attributes["id"] == null) continue;

                string id = node.Attributes["id"].Value;

                if (id == "kernetekst") {

                    BorgerDkBlockElement block = new BorgerDkBlockElement {
                        Type = "kernetekst"                         
                    };

                    List<BorgerDkMicroArticle> microArticles = new List<BorgerDkMicroArticle>();

                    foreach (HtmlNode child in node.ChildNodes) {

                        if (child is HtmlTextNode || child.Attributes["id"] == null) continue;

                        // Get the ID of the micro article
                        string microId = child.Attributes["id"].Value.Replace("microArticle_", "");

                        HtmlNode[] children = BorgerDkHelpers.GetNonTextChildren(child);

                        // Trigger exception if empty
                        if (children.Length == 0) throw new Exception("What's happening? #1 (" + microId + ")");

                        // Trigger exception if no <h2> (or <h3> - thanks for that change)
                        if (children[0].Name != "h2" && children[0].Name != "h3") throw new Exception("What's happening? #2 (" + microId + ")");

                        // Get the title from the <h2>
                        string title = children[0].InnerText;

                        BorgerDkMicroArticle micro = new BorgerDkMicroArticle {
                            Parent = block,
                            Id = microId,
                            Title = title.Trim(),
                            TitleType = children[0].Name,
                            Content = child.InnerHtml.Trim(),
                            Children = (
                                from n in children
                                let e = BorgerDkHelpers.ToXElement(n)
                                where e.Attributes().Count() >= 0 && e.Value != ""
                                select BorgerDkHelpers.CleanMicroArticle(e)
                            )
                        };

                        microArticles.Add(micro);

                    }

                    block.MicroArticles = microArticles.ToArray();

                    elements.Add(block);

                } else if (id == "byline") {

                    XElement xChild = BorgerDkHelpers.ToXElement(node);

                    if (xChild.Elements().Count() == 1) {
                        XElement xDiv = xChild.Element("div");
                        if (xDiv != null && !xDiv.Elements().Any()) {
                            xDiv.Remove();
                            xChild.Add(xDiv.Value);
                        }
                    }

                    ByLine = node.InnerHtml.Trim();

                    BorgerDkTextElement element = new BorgerDkTextElement {
                        Type = id,
                        Title = "Skrevet af",
                        Content = ByLine,
                        Children = new [] { xChild }
                    };

                    // Add the element
                    elements.Add(element);

                } else {

                    HtmlNode[] children = BorgerDkHelpers.GetNonTextChildren(node);

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

                    BorgerDkTextElement element = new BorgerDkTextElement {
                        Type = id,
                        Title = title,
                        Content = node.InnerHtml,
                        Children = (
                            from child in children
                            //where child.Name != "h3"
                            let e = BorgerDkHelpers.ToXElement(child)
                            where e.Attributes().Count() >= 0 && e.Value != ""
                            select BorgerDkHelpers.CleanLists(e)
                        )
                    };

                    // Add the element
                    elements.Add(element);

                }

            }

            Elements = elements.ToArray();


        }

        #endregion

        #region Static methods

        public static BorgerDkArticle GetFromArticle(BorgerDkService service, Article article) {
            return GetFromArticle(service, article, null);
        }

        public static BorgerDkArticle GetFromArticle(BorgerDkService service, Article article, BorgerDkMunicipality municipality) {

            // Check if "service" or "article" is null
            if (service == null) throw new ArgumentNullException(nameof(service));
            if (article == null) throw new ArgumentNullException(nameof(article));

            return new BorgerDkArticle(service, article, municipality ?? BorgerDkMunicipality.NoMunicipality);

        }

        #endregion

    }

}