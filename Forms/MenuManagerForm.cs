namespace FastFoodPOS.Forms;

public class MenuManagerForm : Form
{
    public MenuManagerForm()
    {
        Text = "Menu manager";
        Font = UiHelper.Body;
        BackColor = UiHelper.Surface;
        Padding = new Padding(16);

        var tabs = new TabControl { Dock = DockStyle.Fill, Padding = new Point(18, 6) };
        AddTab(tabs, "Menu items", BuildItemsPage());
        AddTab(tabs, "Categories", BuildCategoriesPage());
        AddTab(tabs, "Add-ons", BuildAddOnsPage());
        Controls.Add(tabs);
    }

    private static void AddTab(TabControl tabs, string title, Control page)
    {
        var tp = new TabPage(title) { BackColor = UiHelper.Surface, UseVisualStyleBackColor = false, Padding = new Padding(12) };
        page.Dock = DockStyle.Fill;
        tp.Controls.Add(page);
        tabs.TabPages.Add(tp);
    }

    private static TableLayoutPanel Split()
    {
        var t = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
        t.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        t.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 380));
        t.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        return t;
    }

    private static TableLayoutPanel FieldStack()
    {
        var t = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1 };
        t.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        return t;
    }

    private static void AddField(TableLayoutPanel t, string caption, Control field)
    {
        int row = t.RowCount;
        t.RowCount = row + 2;
        t.RowStyles.Add(new RowStyle(SizeType.Absolute, 26));
        t.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
        t.Controls.Add(UiHelper.Caption(caption), 0, row);
        field.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        t.Controls.Add(field, 0, row + 1);
    }

    private static void AddRow(TableLayoutPanel t, Control row, int height)
    {
        int r = t.RowCount;
        t.RowCount = r + 1;
        t.RowStyles.Add(new RowStyle(SizeType.Absolute, height));
        row.Dock = DockStyle.Fill;
        t.Controls.Add(row, 0, r);
    }

    private static TableLayoutPanel ButtonGrid(int columns, params Button[] buttons)
    {
        int rows = (buttons.Length + columns - 1) / columns;
        var t = new TableLayoutPanel { ColumnCount = columns, RowCount = rows, Margin = Padding.Empty };
        for (int c = 0; c < columns; c++) t.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / columns));
        for (int r = 0; r < rows; r++) t.RowStyles.Add(new RowStyle(SizeType.Absolute, 46));
        for (int i = 0; i < buttons.Length; i++)
        {
            var b = buttons[i];
            b.Dock = DockStyle.Fill;
            b.Margin = new Padding(0, 0, 8, 8);
            t.Controls.Add(b, i % columns, i / columns);
        }
        return t;
    }

    private static Control EditorCard(TableLayoutPanel stack, int height)
    {
        var card = new SurfacePanel { Dock = DockStyle.Top, Height = height, Padding = new Padding(16) };
        card.Controls.Add(stack);
        var column = new Panel { Dock = DockStyle.Fill, Padding = new Padding(16, 0, 0, 0) };
        column.Controls.Add(card);
        return column;
    }

    private static Control BuildItemsPage()
    {
        var grid = new DataGridView();
        Sample.Fill(grid,
            new[] { "Name", "Price", "Category", "Available" },
            new[]
            {
                new[] { "Cheese Burger", "60.00", "Burgers", "Yes" },
                new[] { "Chicken Burger", "65.00", "Burgers", "Yes" },
                new[] { "Crispy Chicken", "70.00", "Burgers", "Yes" },
                new[] { "Double Burger", "85.00", "Burgers", "Yes" },
                new[] { "Brownie", "28.00", "Desserts", "Yes" },
                new[] { "Ice Cream", "22.00", "Desserts", "No" },
                new[] { "Cola", "15.00", "Drinks", "Yes" },
                new[] { "Orange Juice", "20.00", "Drinks", "Yes" },
                new[] { "Water", "8.00", "Drinks", "Yes" },
                new[] { "Coleslaw", "18.00", "Sides", "Yes" },
                new[] { "French Fries", "25.00", "Sides", "Yes" },
                new[] { "Onion Rings", "30.00", "Sides", "Yes" }
            },
            1);

        var txtSearch = new TextBox { Width = 220, PlaceholderText = "Search items", Margin = new Padding(0, 4, 12, 0) };
        var cmbFilter = new ComboBox { Width = 160, DropDownStyle = ComboBoxStyle.DropDownList, Margin = new Padding(0, 4, 0, 0) };
        cmbFilter.Items.AddRange(new object[] { "All", "Burgers", "Sides", "Drinks", "Desserts" });
        cmbFilter.SelectedIndex = 0;
        var filter = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 46, WrapContents = false };
        filter.Controls.Add(txtSearch);
        filter.Controls.Add(cmbFilter);

        var list = new Panel { Dock = DockStyle.Fill };
        list.Controls.Add(UiHelper.Wrap(grid));
        list.Controls.Add(filter);

        var cmbCat = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        cmbCat.Items.AddRange(new object[] { "Burgers", "Desserts", "Drinks", "Sides" });
        cmbCat.SelectedIndex = 0;

        var stack = FieldStack();
        AddField(stack, "Name", new TextBox());
        AddField(stack, "Price", new TextBox());
        AddField(stack, "Category", cmbCat);
        AddRow(stack, new CheckBox { Text = "Available on the order screen", Checked = true, AutoSize = true }, 36);
        AddRow(stack, ButtonGrid(2,
            UiHelper.Btn("Add", BtnStyle.Primary, 70),
            UiHelper.Btn("Update", BtnStyle.Secondary, 70),
            UiHelper.Btn("Delete", BtnStyle.Danger, 70),
            UiHelper.Btn("Clear", BtnStyle.Secondary, 70)), 100);

        var split = Split();
        split.Controls.Add(list, 0, 0);
        split.Controls.Add(EditorCard(stack, 408), 1, 0);
        return split;
    }

    private static Control BuildCategoriesPage()
    {
        var grid = new DataGridView();
        Sample.Fill(grid,
            new[] { "Name" },
            new[]
            {
                new[] { "Burgers" },
                new[] { "Desserts" },
                new[] { "Drinks" },
                new[] { "Sides" }
            });

        var stack = FieldStack();
        AddField(stack, "Name", new TextBox());
        AddRow(stack, ButtonGrid(3,
            UiHelper.Btn("Add", BtnStyle.Primary, 70),
            UiHelper.Btn("Rename", BtnStyle.Secondary, 70),
            UiHelper.Btn("Delete", BtnStyle.Danger, 70)), 50);

        var split = Split();
        split.Controls.Add(UiHelper.Wrap(grid), 0, 0);
        split.Controls.Add(EditorCard(stack, 170), 1, 0);
        return split;
    }

    private static Control BuildAddOnsPage()
    {
        var grid = new DataGridView();
        Sample.Fill(grid,
            new[] { "Name", "Price" },
            new[]
            {
                new[] { "Bacon", "12.00" },
                new[] { "Extra cheese", "10.00" },
                new[] { "Extra sauce", "5.00" },
                new[] { "Large size", "15.00" }
            },
            1);

        var stack = FieldStack();
        AddField(stack, "Name", new TextBox());
        AddField(stack, "Price", new TextBox());
        AddRow(stack, ButtonGrid(3,
            UiHelper.Btn("Add", BtnStyle.Primary, 70),
            UiHelper.Btn("Update", BtnStyle.Secondary, 70),
            UiHelper.Btn("Delete", BtnStyle.Danger, 70)), 50);

        var split = Split();
        split.Controls.Add(UiHelper.Wrap(grid), 0, 0);
        split.Controls.Add(EditorCard(stack, 250), 1, 0);
        return split;
    }
}
