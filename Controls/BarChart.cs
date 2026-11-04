namespace FastFoodPOS.CustomControls;

public class BarChart : Control
{
    private List<KeyValuePair<string, decimal>> _data = new();

    public string Title { get; set; } = "";
    public Color BarColor { get; set; } = Color.FromArgb(74, 72, 67);
    public Color PeakColor { get; set; } = UiHelper.Brand;

    public BarChart()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;
        BackColor = UiHelper.PanelBack;
        MinimumSize = new Size(220, 160);
    }

    public void SetData(IEnumerable<KeyValuePair<string, decimal>> data)
    {
        _data = data.ToList();
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;

        using var border = new Pen(UiHelper.Border);
        g.DrawRectangle(border, 0, 0, Width - 1, Height - 1);
        TextRenderer.DrawText(g, Title, UiHelper.Semibold, new Point(14, 10), UiHelper.Ink, TextFormatFlags.NoPadding);

        if (_data.Count == 0)
        {
            TextRenderer.DrawText(g, "No sales in this period", UiHelper.Body, ClientRectangle, UiHelper.Muted,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            return;
        }

        const int left = 52, right = 16, top = 48, bottom = 30;
        var plot = new Rectangle(left, top, Width - left - right, Height - top - bottom);
        if (plot.Width <= 40 || plot.Height <= 40) return;

        decimal max = NiceMax(_data.Max(d => d.Value));

        using var gridPen = new Pen(Color.FromArgb(232, 231, 227));
        for (int i = 0; i <= 4; i++)
        {
            int y = plot.Bottom - plot.Height * i / 4;
            g.DrawLine(gridPen, plot.Left, y, plot.Right, y);
            TextRenderer.DrawText(g, (max * i / 4).ToString("0"), UiHelper.Small,
                new Rectangle(0, y - 8, left - 8, 16), UiHelper.Muted,
                TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        }

        float slot = (float)plot.Width / _data.Count;
        float barW = Math.Min(slot * 0.6f, 56f);
        int peak = IndexOfPeak();
        int step = Math.Max(1, (int)Math.Ceiling(34f / slot));
        bool labelEveryBar = slot >= 40f;

        for (int i = 0; i < _data.Count; i++)
        {
            float h = (float)(_data[i].Value / max) * plot.Height;
            float x = plot.Left + i * slot + (slot - barW) / 2f;
            var rect = new RectangleF(x, plot.Bottom - h, barW, h);

            using (var brush = new SolidBrush(i == peak ? PeakColor : BarColor))
                g.FillRectangle(brush, rect);

            if (labelEveryBar || i == peak)
            {
                var valueRect = new Rectangle((int)(plot.Left + i * slot), (int)rect.Y - 18, (int)slot, 16);
                TextRenderer.DrawText(g, _data[i].Value.ToString("0"), UiHelper.Small, valueRect, UiHelper.Ink,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.Bottom | TextFormatFlags.NoPadding);
            }

            if (i % step == 0)
            {
                int cx = (int)(plot.Left + i * slot + slot / 2f);
                int w = (int)(slot * step);
                TextRenderer.DrawText(g, _data[i].Key, UiHelper.Small,
                    new Rectangle(cx - w / 2, plot.Bottom + 6, w, 16), UiHelper.Muted,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.Top |
                    TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis);
            }
        }
    }

    private int IndexOfPeak()
    {
        int best = 0;
        for (int i = 1; i < _data.Count; i++)
            if (_data[i].Value > _data[best].Value) best = i;
        return best;
    }

    private static decimal NiceMax(decimal value)
    {
        if (value <= 0) return 1;
        double v = (double)value;
        double mag = Math.Pow(10, Math.Floor(Math.Log10(v)));
        double n = v / mag;
        double step = n <= 1 ? 1 : n <= 2 ? 2 : n <= 4 ? 4 : n <= 8 ? 8 : 10;
        return (decimal)(step * mag);
    }
}
