namespace FastFoodPOS.Forms;

public class ReportsForm : Form
{
    private readonly DataGridView _gridBest = new();

    public ReportsForm()
    {
        Text = "Reports";
        Font = UiHelper.Body;
        BackColor = UiHelper.Surface;
        Padding = new Padding(16);

        Sample.Fill(_gridBest,
            new[] { "Item", "Quantity sold", "Revenue" },
            new[]
            {
                new[] { "Cheese Burger", "48", "2,880.00" },
                new[] { "French Fries", "41", "1,025.00" },
                new[] { "Cola", "37", "555.00" },
                new[] { "Double Burger", "29", "2,465.00" },
                new[] { "Brownie", "22", "616.00" }
            },
            1, 2);

        var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4 };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 112));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 62));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 38));
        root.Controls.Add(BuildToolbar(), 0, 0);
        root.Controls.Add(BuildSummary(), 0, 1);
        root.Controls.Add(BuildCharts(), 0, 2);
        root.Controls.Add(BuildBestSellers(), 0, 3);
        Controls.Add(root);
    }

    private static Control BuildToolbar()
    {
        var dtFrom = new DateTimePicker { Format = DateTimePickerFormat.Short, Width = 110, Value = DateTime.Today.AddDays(-13), Margin = new Padding(0, 4, 16, 0) };
        var dtTo = new DateTimePicker { Format = DateTimePickerFormat.Short, Width = 110, Value = DateTime.Today, Margin = new Padding(0, 4, 16, 0) };
        var btnGenerate = UiHelper.Btn("Generate", BtnStyle.Primary, 100);
        var btnExport = UiHelper.Btn("Export CSV", BtnStyle.Secondary, 100);
        btnGenerate.Margin = new Padding(0, 2, 8, 0);
        btnExport.Margin = new Padding(0, 2, 8, 0);

        var bar = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, Margin = Padding.Empty };
        bar.Controls.Add(UiHelper.Lbl("From"));
        bar.Controls.Add(dtFrom);
        bar.Controls.Add(UiHelper.Lbl("To"));
        bar.Controls.Add(dtTo);
        bar.Controls.Add(btnGenerate);
        bar.Controls.Add(btnExport);
        return bar;
    }

    private static Control BuildSummary()
    {
        var strip = new SurfacePanel { Dock = DockStyle.Fill, Margin = new Padding(0, 0, 0, 12) };
        var cells = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 1 };
        for (int i = 0; i < 4; i++) cells.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        cells.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        cells.Controls.Add(MakeStat("Orders", "86", false), 0, 0);
        cells.Controls.Add(MakeStat("Revenue", "9,842.50", true), 1, 0);
        cells.Controls.Add(MakeStat("Average order", "114.45", true), 2, 0);
        cells.Controls.Add(MakeStat("Items sold", "231", true), 3, 0);
        strip.Controls.Add(cells);
        return strip;
    }

    private static Control MakeStat(string caption, string number, bool divider)
    {
        var cell = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20, 12, 10, 8), Margin = Padding.Empty };
        var value = new Label
        {
            Text = number,
            Dock = DockStyle.Top,
            Height = 52,
            Font = UiHelper.Figure,
            TextAlign = ContentAlignment.MiddleLeft
        };
        var label = new Label { Text = caption, Dock = DockStyle.Fill, ForeColor = UiHelper.Muted };
        cell.Controls.Add(label);
        cell.Controls.Add(value);
        if (divider)
        {
            cell.Paint += (s, e) =>
            {
                using var pen = new Pen(UiHelper.Border);
                e.Graphics.DrawLine(pen, 0, 14, 0, cell.Height - 14);
            };
        }
        return cell;
    }

    private static Control BuildCharts()
    {
        var daily = new BarChart { Title = "Daily sales", Dock = DockStyle.Fill, Margin = new Padding(0, 0, 12, 12) };
        daily.SetData(new[]
        {
            Pair("22/10", 540m), Pair("23/10", 720m), Pair("24/10", 610m), Pair("25/10", 880m),
            Pair("26/10", 790m), Pair("27/10", 450m), Pair("28/10", 530m), Pair("29/10", 690m),
            Pair("30/10", 760m), Pair("31/10", 940m), Pair("01/11", 820m), Pair("02/11", 610m),
            Pair("03/11", 710m), Pair("04/11", 520m)
        });

        var byCategory = new PieChart { Title = "Revenue by category", Dock = DockStyle.Fill, Margin = new Padding(0, 0, 0, 12) };
        byCategory.SetData(new[]
        {
            Pair("Burgers", 5200m), Pair("Sides", 1900m), Pair("Drinks", 1500m), Pair("Desserts", 1242.5m)
        });

        var charts = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, Margin = Padding.Empty };
        charts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        charts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        charts.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        charts.Controls.Add(daily, 0, 0);
        charts.Controls.Add(byCategory, 1, 0);
        return charts;
    }

    private static KeyValuePair<string, decimal> Pair(string name, decimal value) => new(name, value);

    private Control BuildBestSellers()
    {
        var card = new SurfacePanel { Dock = DockStyle.Fill, Margin = Padding.Empty };
        var title = new Label
        {
            Text = "Best sellers",
            Dock = DockStyle.Top,
            Height = 32,
            Font = UiHelper.Semibold,
            Padding = new Padding(10, 0, 0, 0),
            TextAlign = ContentAlignment.MiddleLeft
        };
        card.Controls.Add(_gridBest);
        card.Controls.Add(title);
        return card;
    }
}
