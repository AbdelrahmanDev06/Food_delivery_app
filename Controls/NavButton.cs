namespace FastFoodPOS.CustomControls;

public class NavButton : ClickableControl
{
    private bool _selected;

    public bool Selected
    {
        get => _selected;
        set
        {
            _selected = value;
            Invalidate();
        }
    }

    public NavButton()
    {
        Dock = DockStyle.Top;
        Height = 44;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(_selected ? UiHelper.BrandTint : Hot ? UiHelper.Hover : UiHelper.PanelBack);

        if (_selected)
        {
            using var bar = new SolidBrush(UiHelper.Brand);
            g.FillRectangle(bar, 0, 0, 3, Height);
        }

        TextRenderer.DrawText(g, Text, _selected ? UiHelper.Semibold : UiHelper.Body,
            new Rectangle(20, 0, Width - 24, Height), UiHelper.Ink,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis |
            TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);

        DrawFocus(g, new Rectangle(4, 4, Width - 8, Height - 8));
    }
}
