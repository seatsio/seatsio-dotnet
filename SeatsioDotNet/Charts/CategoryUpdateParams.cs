namespace SeatsioDotNet.Charts;

public class CategoryUpdateParams
{
    public string Label { get; set; }
    public string Color { get; set; }
    public bool? Accessible { get; set; }

    public CategoryUpdateParams()
    {
    }

    public CategoryUpdateParams(string label, string color, bool? accessible)
    {
        Label = label;
        Color = color;
        Accessible = accessible;
    }

    public CategoryUpdateParams WithLabel(string label)
    {
        this.Label = label;
        return this;
    }

    public CategoryUpdateParams WithColor(string color)
    {
        this.Color = color;
        return this;
    }

    public CategoryUpdateParams WithAccessible(bool accessible)
    {
        this.Accessible = accessible;
        return this;
    }
}