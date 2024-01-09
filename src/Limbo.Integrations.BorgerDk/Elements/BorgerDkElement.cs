namespace Limbo.Integrations.BorgerDk.Elements;

public abstract class BorgerDkElement {

    public string Id { get; }

    protected BorgerDkElement(string id) {
        Id = id;
    }

}