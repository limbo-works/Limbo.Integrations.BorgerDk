namespace Limbo.Integrations.BorgerDk.Elements;

public class BorgerDkTextElement : BorgerDkElement {

    public string Title { get; }

    public string Content { get; }

    public BorgerDkTextElement(string id, string title, string content) : base(id) {
        Title = title;
        Content = content;
    }

}