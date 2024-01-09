namespace Limbo.Integrations.BorgerDk.Elements;

public class BorgerDkMicroArticle {

    public BorgerDkBlockElement Parent { get; }

    public string Id { get; }

    public string Title { get; }

    public string TitleType { get; }

    public string Content { get; }

    public BorgerDkMicroArticle(BorgerDkBlockElement parent, string id, string title, string titleType, string content) {
        Parent = parent;
        Id = id;
        Title = title;
        TitleType = titleType;
        Content = content;
    }

}