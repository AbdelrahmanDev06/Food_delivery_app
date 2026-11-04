namespace FastFoodPOS.Forms;

public class HistoryForm : Form
{
    private readonly DataGridView _grid = new();
    private readonly DataGridView _gridLines = new();

    public HistoryForm()
    {
        Text = "Order history";
        Font = UiHelper.Body;
        BackColor = UiHelper.Surface;
        Padding = new Padding(16);

        Sample.Fill(_grid,
            new[] { "Order #", "Date", "Subtotal", "Tax", "Total", "Payment", "Status" },
            new[]
            {
                new[] { "112", "04/11/2026 14:20", "195.00", "27.30", "222.30", "Cash", "Paid" },
                new[] { "111", "04/11/2026 13:05", "85.00", "11.90", "96.90", "Card", "Paid" },
                new[] { "110", "04/11/2026 12:40", "140.00", "19.60", "159.60", "Cash", "Voided" },
                new[] { "109", "03/11/2026 20:15", "63.00", "8.82", "71.82", "Card", "Paid" },
                new[] { "108", "03/11/2026 19:30", "230.00", "32.20", "262.20", "Cash", "Paid" },
                new[] { "107", "03/11/2026 18:02", "48.00", "6.72", "54.72", "Cash", "Paid" }
            },
            2, 3, 4);
        _grid.Rows[2].DefaultCellStyle.ForeColor = UiHelper.Muted;
        _grid.Rows[2].DefaultCellStyle.Font = UiHelper.Struck;

        Sample.Fill(_gridLines,
            new[] { "Item", "Extras", "Qty", "Price", "Total" },
            new[]
            {
                new[] { "Cheese Burger", "Extra cheese", "2", "70.00", "140.00" },
                new[] { "French Fries", "", "1", "25.00", "25.00" },
                new[] { "Cola", "", "2", "15.00", "30.00" }
            },
            2, 3, 4);

        var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4 };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 58));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 58));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 42));
        root.Controls.Add(BuildFilterBar(), 0, 0);
        root.Controls.Add(UiHelper.Wrap(_grid), 0, 1);
        root.Controls.Add(BuildActionRow(), 0, 2);
        root.Controls.Add(BuildLinesPanel(), 0, 3);
        Controls.Add(root);
    }

    private static Control BuildFilterBar()
    {
        var dtFrom = new DateTimePicker { Format = DateTimePickerFormat.Short, Width = 110, Value = DateTime.Today.AddDays(-30), Margin = new Padding(0, 4, 16, 0) };
        var dtTo = new DateTimePicker { Format = DateTimePickerFormat.Short, Width = 110, Value = DateTime.Today, Margin = new Padding(0, 4, 16, 0) };
        var segPay = new SegmentedControl { Margin = new Padding(0, 2, 16, 0) };
        segPay.SetItems(new[] { "All", "Cash", "Card" });
        var txtId = new TextBox { Width = 90, PlaceholderText = "Order #", Margin = new Padding(0, 4, 16, 0) };
        var btnSearch = UiHelper.Btn("Search", BtnStyle.Primary, 90);
        btnSearch.Margin = new Padding(0, 2, 0, 0);

        var bar = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, Margin = Padding.Empty };
        bar.Controls.Add(UiHelper.Lbl("From"));
        bar.Controls.Add(dtFrom);
        bar.Controls.Add(UiHelper.Lbl("To"));
        bar.Controls.Add(dtTo);
        bar.Controls.Add(segPay);
        bar.Controls.Add(txtId);
        bar.Controls.Add(btnSearch);
        return bar;
    }

    private static Control BuildActionRow()
    {
        var row = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, Margin = Padding.Empty };
        row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
        row.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));

        var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, Padding = new Padding(0, 8, 0, 0), Margin = Padding.Empty };
        buttons.Controls.Add(UiHelper.Btn("Reprint receipt", BtnStyle.Secondary, 130));
        buttons.Controls.Add(UiHelper.Btn("Void / Restore", BtnStyle.Secondary, 120));
        buttons.Controls.Add(UiHelper.Btn("Delete order", BtnStyle.Danger, 120));

        var summary = new Label
        {
            Text = "5 paid orders   |   Revenue 707.94",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleRight,
            ForeColor = UiHelper.Muted
        };

        row.Controls.Add(buttons, 0, 0);
        row.Controls.Add(summary, 1, 0);
        return row;
    }

    private Control BuildLinesPanel()
    {
        var card = new SurfacePanel { Dock = DockStyle.Fill, Margin = Padding.Empty };
        var title = new Label
        {
            Text = "Items in the selected order",
            Dock = DockStyle.Top,
            Height = 32,
            Font = UiHelper.Semibold,
            Padding = new Padding(10, 0, 0, 0),
            TextAlign = ContentAlignment.MiddleLeft
        };
        card.Controls.Add(_gridLines);
        card.Controls.Add(title);
        return card;
    }
}
