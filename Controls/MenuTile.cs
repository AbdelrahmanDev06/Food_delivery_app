namespace FastFoodPOS.CustomControls;

public class MenuTile : ClickableControl
{
    public const string DragPrefix = "SMASH:";

    private readonly Color _stripe;
    private readonly string _price;
    private Point _mouseDown;
    private bool _dragging;

    public int ItemId { get; set; }

    public MenuTile(string name, decimal price, Color stripe)
    {
        Text = name;
        _price = price.ToString("0.00");
        _stripe = stripe;
        Size = new Size(160, 92);
        Margin = new Padding(0, 0, 10, 10);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        _mouseDown = e.Location;
        _dragging = false;
        base.OnMouseDown(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (e.Button != MouseButtons.Left || _dragging || ItemId <= 0) return;

        Size limit = SystemInformation.DragSize;
        if (Math.Abs(e.X - _mouseDown.X) < limit.Width && Math.Abs(e.Y - _mouseDown.Y) < limit.Height) return;

        _dragging = true;
        DoDragDrop(DragPrefix + ItemId.ToString(CultureInfo.InvariantCulture), DragDropEffects.Copy);
        _dragging = false;
        ResetPress();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(Pressed ? UiHelper.BrandTint : UiHelper.PanelBack);

        using (var pen = new Pen(Hot || Focused ? UiHelper.Brand : UiHelper.Border))
            g.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
        using (var stripe = new SolidBrush(_stripe))
            g.FillRectangle(stripe, 1, 1, 5, Height - 2);

        TextRenderer.DrawText(g, Text, UiHelper.Semibold, new Rectangle(16, 9, Width - 26, 44), UiHelper.Ink,
            TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.WordBreak |
            TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);

        TextRenderer.DrawText(g, _price, UiHelper.Body, new Rectangle(16, Height - 28, Width - 26, 20), UiHelper.Muted,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
    }
}
