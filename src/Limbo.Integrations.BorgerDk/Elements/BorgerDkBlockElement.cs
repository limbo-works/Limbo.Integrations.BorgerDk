using System.Collections.Generic;

namespace Limbo.Integrations.BorgerDk.Elements;

public class BorgerDkBlockElement : BorgerDkElement {

    public IReadOnlyList<BorgerDkMicroArticle> MicroArticles { get; }

    public BorgerDkBlockElement(string id, IReadOnlyList<BorgerDkMicroArticle> microArticles) : base(id)  {
        MicroArticles = microArticles;
    }

}