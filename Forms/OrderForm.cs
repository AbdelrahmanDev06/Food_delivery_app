namespace FastFoodPOS.Forms;

public class OrderForm : Form
{
    private readonly DataGridView _grid = new();

    public OrderForm()
    {
        Text = "New order";
        Font = UiHelper.Body;
        BackColor = UiHelper.Surface;
        Padding = new Padding(16);

        var main = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
        main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        main.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 420));
        main.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        main.Controls.Add(BuildMenuSide(), 0, 0);
        main.Controls.Add(BuildCartSide(), 1, 0);
        Controls.Add(main);
    }

    private static Label TotalLabel(string text, Font font) => new()
    {
        Text = text,
        Dock = DockStyle.Fill,
        Font = font,
        ForeColor = UiHelper.Ink,
        TextAlign = ContentAlignment.MiddleRight,
        Margin = Padding.Empty
    };

    private Control BuildMenuSide()
    {
        var left = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            Margin = new Padding(0, 0, 16, 0)
        };
        left.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
        left.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        left.RowStyles.Add(new RowStyle(SizeType.Absolute, 150));

        var txtSearch = new TextBox { Width = 220, PlaceholderText = "Search the menu (F2)", Margin = new Padding(0, 4, 16, 0) };
        var segCat = new SegmentedControl { Margin = new Padding(0, 2, 0, 0) };
        segCat.SetItems(new[] { "All", "Burgers", "Sides", "Drinks", "Desserts" });
        var bar = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, Margin = Padding.Empty };
        bar.Controls.Add(txtSearch);
        bar.Controls.Add(segCat);

        var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = UiHelper.Surface };
        AddTile(flow, "Cheese Burger", 60, 1);
        AddTile(flow, "Double Burger", 85, 1);
        AddTile(flow, "Chicken Burger", 65, 1);
        AddTile(flow, "Crispy Chicken", 70, 1);
        AddTile(flow, "French Fries", 25, 2);
        AddTile(flow, "Onion Rings", 30, 2);
        AddTile(flow, "Coleslaw", 18, 2);
        AddTile(flow, "Cola", 15, 3);
        AddTile(flow, "Orange Juice", 20, 3);
        AddTile(flow, "Water", 8, 3);
        AddTile(flow, "Ice Cream", 22, 4);
        AddTile(flow, "Brownie", 28, 4);

        var flowAddOns = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, WrapContents = true, BackColor = UiHelper.PanelBack };
        foreach (var text in new[] { "Extra cheese (+10.00)", "Large size (+15.00)", "Extra sauce (+5.00)", "Bacon (+12.00)" })
            flowAddOns.Controls.Add(new CheckBox { Text = text, AutoSize = true, Margin = new Padding(0, 4, 20, 4) });
        var numQty = new NumericUpDown { Minimum = 1, Maximum = 99, Value = 1, Width = 70 };

        var extras = new SurfacePanel { Dock = DockStyle.Fill, Padding = new Padding(14, 8, 14, 8), Margin = Padding.Empty };
        var grid = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 2 };
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
        grid.RowStyles.Add(new RowStyle(SizeType.Absolute, 26));
        grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        grid.Controls.Add(UiHelper.Caption("Extras for the selected line (tick to apply)"), 0, 0);
        grid.Controls.Add(flowAddOns, 0, 1);
        grid.Controls.Add(UiHelper.Caption("Quantity"), 1, 0);
        grid.Controls.Add(numQty, 1, 1);
        extras.Controls.Add(grid);

        left.Controls.Add(bar, 0, 0);
        left.Controls.Add(flow, 0, 1);
        left.Controls.Add(extras, 0, 2);
        return left;
    }

    private static void AddTile(FlowLayoutPanel flow, string name, decimal price, int categoryId) =>
        flow.Controls.Add(new MenuTile(name, price, UiHelper.CategoryColor(categoryId)));

    private Control BuildCartSide()
    {
        var card = new SurfacePanel { Dock = DockStyle.Fill, Padding = new Padding(16), Margin = Padding.Empty };

        var t = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 7 };
        t.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        t.RowStyles.Add(new RowStyle(SizeType.Absolute, 46));
        t.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        t.RowStyles.Add(new RowStyle(SizeType.Absolute, 130));
        t.RowStyles.Add(new RowStyle(SizeType.Absolute, 72));
        t.RowStyles.Add(new RowStyle(SizeType.Absolute, 54));
        t.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));

        var title = UiHelper.Caption("Current order");
        title.Font = UiHelper.Heading;
        title.ForeColor = UiHelper.Ink;

        var btnExtras = UiHelper.Btn("Extras...", BtnStyle.Secondary, 90);
        var btnRemove = UiHelper.Btn("Remove", BtnStyle.Secondary, 80);
        var btnClear = UiHelper.Btn("Clear", BtnStyle.Secondary, 70);
        btnExtras.Margin = new Padding(0, 4, 8, 0);
        btnRemove.Margin = new Padding(0, 4, 8, 0);
        btnClear.Margin = new Padding(0, 4, 0, 0);
        var tools = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, Margin = Padding.Empty };
        tools.Controls.Add(btnExtras);
        tools.Controls.Add(btnRemove);
        tools.Controls.Add(btnClear);

        Sample.Fill(_grid,
            new[] { "Item", "Extras", "Qty", "Price", "Total" },
            new[]
            {
                new[] { "Cheese Burger", "Extra cheese", "2", "70.00", "140.00" },
                new[] { "French Fries", "", "1", "25.00", "25.00" },
                new[] { "Cola", "", "2", "15.00", "30.00" }
            },
            2, 3, 4);
        UiHelper.Weight(_grid, "col0", 36);
        UiHelper.Weight(_grid, "col1", 28);
        UiHelper.Weight(_grid, "col2", 11);
        UiHelper.Weight(_grid, "col3", 17);
        UiHelper.Weight(_grid, "col4", 17);

        var totals = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 3,
            Padding = new Padding(0, 10, 0, 0),
            Margin = Padding.Empty
        };
        totals.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        totals.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        totals.RowStyles.Add(new RowStyle(SizeType.Absolute, 26));
        totals.RowStyles.Add(new RowStyle(SizeType.Absolute, 26));
        totals.RowStyles.Add(new RowStyle(SizeType.Absolute, 58));
        totals.Controls.Add(UiHelper.Caption("Subtotal"), 0, 0);
        totals.Controls.Add(TotalLabel("195.00", UiHelper.Body), 1, 0);
        totals.Controls.Add(UiHelper.Caption("Tax (14%)"), 0, 1);
        totals.Controls.Add(TotalLabel("27.30", UiHelper.Body), 1, 1);
        var totalCaption = UiHelper.Caption("Total");
        totalCaption.Font = UiHelper.Semibold;
        totalCaption.ForeColor = UiHelper.Ink;
        totals.Controls.Add(totalCaption, 0, 2);
        totals.Controls.Add(TotalLabel("222.30", UiHelper.Figure), 1, 2);
        totals.Paint += (s, e) =>
        {
            using var pen = new Pen(UiHelper.Border);
            e.Graphics.DrawLine(pen, 0, 0, totals.Width, 0);
        };

        var grpPay = new GroupBox { Text = "Payment", Dock = DockStyle.Fill };
        var payFlow = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, Padding = new Padding(8, 4, 0, 0) };
        payFlow.Controls.Add(new RadioButton { Text = "Cash", Checked = true, AutoSize = true, Margin = new Padding(0, 0, 24, 0) });
        payFlow.Controls.Add(new RadioButton { Text = "Card", AutoSize = true });
        grpPay.Controls.Add(payFlow);

        var btnCheckout = UiHelper.Btn("Checkout (Ctrl+Enter)", BtnStyle.Primary);
        btnCheckout.Dock = DockStyle.Fill;
        btnCheckout.Font = UiHelper.Large;
        btnCheckout.Margin = new Padding(0, 6, 0, 0);

        var info = new Label { Text = "Sample order - nothing is saved in this version.", Dock = DockStyle.Fill, ForeColor = UiHelper.Muted, Margin = Padding.Empty };

        t.Controls.Add(title, 0, 0);
        t.Controls.Add(tools, 0, 1);
        t.Controls.Add(_grid, 0, 2);
        t.Controls.Add(totals, 0, 3);
        t.Controls.Add(grpPay, 0, 4);
        t.Controls.Add(btnCheckout, 0, 5);
        t.Controls.Add(info, 0, 6);
        card.Controls.Add(t);
        return card;
    }
}
