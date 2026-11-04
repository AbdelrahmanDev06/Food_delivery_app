using System.Drawing.Drawing2D;

namespace FastFoodPOS.CustomControls;

public class PieChart : Control
{
    private static readonly Color[] Palette =
    {
        Color.FromArgb(209, 59, 38), Color.FromArgb(47, 111, 159), Color.FromArgb(91, 140, 58),
        Color.FromArgb(199, 154, 27), Color.FromArgb(142, 91, 166), Color.FromArgb(63, 143, 139)
    };

    private List<KeyValuePair<string, decimal>> _data = new();

    public string Title { get; set; } = "";

    public PieChart()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;
        BackColor = UiHelper.PanelBack;
        MinimumSize = new Size(220, 160);
    }

    public void SetData(IEnumerable<KeyValuePair<string, decimal>> data)
    {
        _data = data.Where(d => d.Value > 0).ToList();
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using (var border = new Pen(UiHelper.Border))
            g.DrawRectangle(border, 0, 0, Width - 1, Height - 1);
        TextRenderer.DrawText(g, Title, UiHelper.Semibold, new Point(14, 10), UiHelper.Ink, TextFormatFlags.NoPadding);

        decimal total = _data.Sum(d => d.Value);
        if (_data.Count == 0 || total <= 0)
        {
            TextRenderer.DrawText(g, "No sales in this period", UiHelper.Body, ClientRectangle, UiHelper.Muted,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            return;
        }

        const int top = 46;
        int size = Math.Min(Height - top - 16, (int)(Width * 0.46f));
        if (size < 40) return;

        var pie = new Rectangle(16, top, size, size);
        float start = -90f;
        for (int i = 0; i < _data.Count; i++)
        {
            float sweep = (float)(_data[i].Value / total) * 360f;
            using var brush = new SolidBrush(Palette[i % Palette.Length]);
            g.FillPie(brush, pie, start, sweep);
            if (_data.Count > 1)
            {
                using var gap = new Pen(UiHelper.PanelBack, 2f);
                g.DrawPie(gap, pie, start, sweep);
            }
            start += sweep;
        }

        int x = pie.Right + 24;
        int y = top + 4;
        for (int i = 0; i < _data.Count && y + 20 < Height; i++)
        {
            using (var brush = new SolidBrush(Palette[i % Palette.Length]))
                g.FillRectangle(brush, x, y + 3, 12, 12);

            decimal percent = _data[i].Value / total;
            string text = $"{_data[i].Key}   {percent:P0}   ({_data[i].Value:0})";
            TextRenderer.DrawText(g, text, UiHelper.Body, new Rectangle(x + 20, y, Width - x - 28, 18), UiHelper.Ink,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis |
                TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
            y += 26;
        }
    }
}
