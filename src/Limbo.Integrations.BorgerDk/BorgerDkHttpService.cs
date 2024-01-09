using System;
using System.Linq;
using Limbo.Integrations.BorgerDk.Exceptions;
using Limbo.Integrations.BorgerDk.WebService;

namespace Limbo.Integrations.BorgerDk;

public class BorgerDkHttpService {

    private readonly ArticleExportClient _client;

    /// <summary>
    /// A reference to the endpoint (web service).
    /// </summary>
    public BorgerDkEndpoint Endpoint { get; set; }

    #region Constructors

    public BorgerDkHttpService() {
        Endpoint = BorgerDkEndpoint.Default;
        _client = Endpoint.GetClient();
    }

    public BorgerDkHttpService(BorgerDkEndpoint endpoint) {
        Endpoint = endpoint;
        _client = Endpoint.GetClient();
    }

    #endregion

    #region Member methods

    public BorgerDkArticleShortDescription GetArticleIdFromUrl(string url) {

        try {

            // Make the request to the Borger.dk endpoint
            ArticleShortDescription description = _client.GetArticleIDByUrl(url);

            // Wrap the description
            return new BorgerDkArticleShortDescription(description);

        } catch (Exception ex) {

            if (ex.Message.EndsWith(" has been marked as not exportable.")) throw new BorgerDkNotExportableException(url, ex);

            if (ex.Message.StartsWith("No article found with url ")) throw new BorgerDkNotFoundException(url, ex);

            throw;

        }

    }

    public BorgerDkArticle GetArticleFromId(int articleId, BorgerDkMunicipality municipality) {

        return BorgerDkArticle.GetFromArticle(
            this,
            _client.GetArticleByID(articleId, municipality.Code == 0 ? null : municipality.Code),
            municipality
        );

    }

    public BorgerDkMunicipality[] GetMunicipalityList() {
        return _client.GetMunicipalityList().Select(x => new BorgerDkMunicipality(x.MunicipalityCode, x.MunicipalityName)).ToArray();
    }

    public BorgerDkArticleDescription[] GetArticleList() {
        return _client.GetAllArticles().Select(x => new BorgerDkArticleDescription(x)).ToArray();
    }

    #endregion

    #region Static methods

    public static bool IsValidUrl(string url) {
        return BorgerDkEndpoint.Values.Any(x => x.IsValidUrl(url));
    }

    public static bool IsValidDomain(string domain) {
        return BorgerDkEndpoint.Values.Any(x => x.Domain == domain);
    }

    #endregion

}