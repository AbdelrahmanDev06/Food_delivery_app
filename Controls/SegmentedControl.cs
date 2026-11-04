namespace FastFoodPOS.CustomControls;

public class SegmentedControl : Control
{
    private readonly List<string> _items = new();
    private int _selected;
    private int _hot = -1;

    public event EventHandler? SelectedIndexChanged;

    public SegmentedControl()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                 ControlStyles.Selectable, true);
        TabStop = true;
        Cursor = Cursors.Hand;
        Height = 34;
        Margin = new Padding(0, 0, 8, 0);
    }

    public int SelectedIndex
    {
        get => _selected;
        set
        {
            if (value == _selected || value < 0 || value >= _items.Count) return;
            _selected = value;
            Invalidate();
            SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public string SelectedText => _selected >= 0 && _selected < _items.Count ? _items[_selected] : "";

    public void SetItems(IEnumerable<string> items, int selected = 0)
    {
        _items.Clear();
        _items.AddRange(items);
        _selected = _items.Count == 0 ? -1 : Math.Clamp(selected, 0, _items.Count - 1);
        Width = _items.Sum(i => SegmentWidth(i)) + 1;
        Invalidate();
    }

    private static int SegmentWidth(string text) =>
        TextRenderer.MeasureText(text, UiHelper.Semibold).Width + 28;

    private int IndexAt(Point p)
    {
        int x = 0;
        for (int i = 0; i < _items.Count; i++)
        {
            int w = SegmentWidth(_items[i]);
            if (p.X >= x && p.X < x + w) return i;
            x += w;
        }
        return -1;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(UiHelper.PanelBack);
        using var line = new Pen(UiHelper.Border);

        int x = 0;
        for (int i = 0; i < _items.Count; i++)
        {
            int w = SegmentWidth(_items[i]);
            var r = new Rectangle(x, 0, w, Height - 1);
            bool on = i == _selected;

            using (var fill = new SolidBrush(on ? UiHelper.Ink : i == _hot ? UiHelper.Hover : UiHelper.PanelBack))
                g.FillRectangle(fill, r);

            TextRenderer.DrawText(g, _items[i], UiHelper.Semibold, r, on ? Color.White : UiHelper.Ink,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);

            if (i > 0) g.DrawLine(line, x, 0, x, Height - 1);
            if (on && Focused && ShowFocusCues)
                ControlPaint.DrawFocusRectangle(g, Rectangle.Inflate(r, -3, -3), Color.White, UiHelper.Ink);
            x += w;
        }
        g.DrawRectangle(line, 0, 0, x, Height - 1);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        int idx = IndexAt(e.Location);
        if (idx != _hot)
        {
            _hot = idx;
            Invalidate();
        }
        base.OnMouseMove(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        _hot = -1;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            int idx = IndexAt(e.Location);
            if (idx >= 0) SelectedIndex = idx;
        }
        base.OnMouseDown(e);
    }

    protected override bool IsInputKey(Keys keyData) =>
        (keyData is Keys.Left or Keys.Right) || base.IsInputKey(keyData);

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Left) { SelectedIndex = _selected - 1; e.Handled = true; }
        else if (e.KeyCode == Keys.Right) { SelectedIndex = _selected + 1; e.Handled = true; }
        base.OnKeyDown(e);
    }

    protected override void OnGotFocus(EventArgs e)
    {
        Invalidate();
        base.OnGotFocus(e);
    }

    protected override void OnLostFocus(EventArgs e)
    {
        Invalidate();
        base.OnLostFocus(e);
    }
}
