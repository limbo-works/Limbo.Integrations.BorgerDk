using System;
using Limbo.Integrations.BorgerDk.WebService;
using Skybrud.Essentials.Time;

namespace Limbo.Integrations.BorgerDk;

public class BorgerDkArticleDescription {

    public int Id { get; }

    public string Title { get; }

    public string Url { get; }

    public EssentialsTime PublishDate { get; }

    public EssentialsTime UpdateDate { get; }

    internal BorgerDkArticleDescription(ArticleDescription article) {

        // Get the Danish time zone
        TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");

        // Assume the timstamp are specified according to the Danish time zone
        DateTimeOffset published = new DateTimeOffset(article.PublishingDate, tz.GetUtcOffset(article.PublishingDate));
        DateTimeOffset updated = new DateTimeOffset(article.LastUpdated, tz.GetUtcOffset(article.LastUpdated));

        Id = article.ArticleID;
        Title = article.ArticleTitle;
        Url = article.ArticleUrl;
        PublishDate = new EssentialsTime(published, tz);
        UpdateDate = new EssentialsTime(updated, tz);

    }

}