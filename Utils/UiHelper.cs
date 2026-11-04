namespace FastFoodPOS.Utils;

public enum BtnStyle { Secondary, Primary, Danger }

public static class UiHelper
{
    public static readonly Color Surface = Color.FromArgb(240, 240, 238);
    public static readonly Color PanelBack = Color.White;
    public static readonly Color Hover = Color.FromArgb(245, 244, 241);
    public static readonly Color Border = Color.FromArgb(217, 216, 212);
    public static readonly Color Ink = Color.FromArgb(30, 30, 28);
    public static readonly Color Muted = Color.FromArgb(110, 109, 104);
    public static readonly Color Brand = Color.FromArgb(209, 59, 38);
    public static readonly Color BrandDark = Color.FromArgb(176, 47, 29);
    public static readonly Color BrandTint = Color.FromArgb(249, 232, 228);
    public static readonly Color Ok = Color.FromArgb(46, 125, 79);
    public static readonly Color Danger = Color.FromArgb(158, 27, 27);

    public static readonly Font Body = new("Segoe UI", 9.5f);
    public static readonly Font Semibold = new("Segoe UI Semibold", 9.5f);
    public static readonly Font Small = new("Segoe UI", 8.5f);
    public static readonly Font Heading = new("Segoe UI Semibold", 12.5f);
    public static readonly Font Large = new("Segoe UI Semibold", 11f);
    public static readonly Font Figure = new("Segoe UI Semibold", 18f);
    public static readonly Font Struck = new("Segoe UI", 9.5f, FontStyle.Strikeout);

    private static readonly Color[] CategoryPalette =
    {
        Color.FromArgb(181, 101, 29), Color.FromArgb(91, 140, 58), Color.FromArgb(47, 111, 159),
        Color.FromArgb(142, 91, 166), Color.FromArgb(199, 154, 27), Color.FromArgb(63, 143, 139)
    };

    public static Color CategoryColor(int categoryId) =>
        CategoryPalette[Math.Abs(categoryId) % CategoryPalette.Length];

    public static Button Btn(string text, BtnStyle style = BtnStyle.Secondary, int width = 100) =>
        new AppButton(text, style, width);

    public static Label Lbl(string text) =>
        new() { Text = text, AutoSize = true, ForeColor = Muted, Margin = new Padding(0, 8, 6, 0) };

    public static Label Caption(string text) => new()
    {
        Text = text,
        Dock = DockStyle.Fill,
        ForeColor = Muted,
        TextAlign = ContentAlignment.MiddleLeft,
        Margin = Padding.Empty
    };

    public static SurfacePanel Wrap(Control inner)
    {
        var p = new SurfacePanel { Dock = DockStyle.Fill, Margin = Padding.Empty };
        inner.Dock = DockStyle.Fill;
        p.Controls.Add(inner);
        return p;
    }

    public static void StyleGrid(DataGridView g)
    {
        g.Dock = DockStyle.Fill;
        g.ReadOnly = true;
        g.AllowUserToAddRows = false;
        g.AllowUserToDeleteRows = false;
        g.AllowUserToResizeRows = false;
        g.MultiSelect = false;
        g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        g.RowHeadersVisible = false;
        g.BackgroundColor = PanelBack;
        g.BorderStyle = BorderStyle.None;
        g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        g.GridColor = Color.FromArgb(232, 231, 227);
        g.EnableHeadersVisualStyles = false;
        g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        g.ColumnHeadersHeight = 34;
        g.RowTemplate.Height = 30;

        var head = g.ColumnHeadersDefaultCellStyle;
        head.BackColor = Hover;
        head.ForeColor = Muted;
        head.Font = Semibold;
        head.SelectionBackColor = Hover;
        head.SelectionForeColor = Muted;
        head.Padding = new Padding(8, 0, 8, 0);

        var cell = g.DefaultCellStyle;
        cell.Font = Body;
        cell.BackColor = PanelBack;
        cell.ForeColor = Ink;
        cell.SelectionBackColor = BrandTint;
        cell.SelectionForeColor = Ink;
        cell.Padding = new Padding(8, 0, 8, 0);
    }

    public static void HideColumns(DataGridView g, params string[] names)
    {
        foreach (var n in names)
            if (g.Columns[n] is DataGridViewColumn c) c.Visible = false;
    }

    public static void Order(DataGridView g, params string[] names)
    {
        int i = 0;
        foreach (var n in names)
            if (g.Columns[n] is DataGridViewColumn c) c.DisplayIndex = i++;
    }

    public static void FormatColumns(DataGridView g, string format, params string[] names)
    {
        foreach (var n in names)
        {
            if (g.Columns[n] is not DataGridViewColumn c) continue;
            c.DefaultCellStyle.Format = format;
            if (format.StartsWith('0')) AlignRight(c);
        }
    }

    public static void RightAlign(DataGridView g, params string[] names)
    {
        foreach (var n in names)
            if (g.Columns[n] is DataGridViewColumn c) AlignRight(c);
    }

    private static void AlignRight(DataGridViewColumn c)
    {
        c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
    }

    public static void Header(DataGridView g, string name, string text)
    {
        if (g.Columns[name] is DataGridViewColumn c) c.HeaderText = text;
    }

    public static void Weight(DataGridView g, string name, float weight)
    {
        if (g.Columns[name] is DataGridViewColumn c) c.FillWeight = weight;
    }

    public static void Info(string message) =>
        MessageBox.Show(message, AppSettings.ShopName + " POS", MessageBoxButtons.OK, MessageBoxIcon.Information);

    public static void Warn(string message) =>
        MessageBox.Show(message, AppSettings.ShopName + " POS", MessageBoxButtons.OK, MessageBoxIcon.Warning);

    public static void Error(string message) =>
        MessageBox.Show(message, AppSettings.ShopName + " POS", MessageBoxButtons.OK, MessageBoxIcon.Error);

    public static bool Confirm(string message) =>
        MessageBox.Show(message, AppSettings.ShopName + " POS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
}
