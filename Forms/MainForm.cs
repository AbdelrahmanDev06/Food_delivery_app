namespace FastFoodPOS.Forms;

public class MainForm : Form
{
    private readonly Panel _content = new() { Dock = DockStyle.Fill, BackColor = UiHelper.Surface };
    private readonly ToolStripStatusLabel _lblScreen = new() { Spring = true, TextAlign = ContentAlignment.MiddleLeft };
    private readonly ToolStripStatusLabel _lblUser = new() { Text = "User: admin (Admin)" };
    private readonly ToolStripStatusLabel _lblClock = new() { Text = DateTime.Now.ToString("ddd d MMM, HH:mm") };

    private readonly List<NavButton> _nav = new();
    private readonly List<Func<Form>> _screens = new();
    private Form? _current;

    public bool LoggedOut { get; private set; }

    public MainForm()
    {
        Text = AppSettings.ShopName + " POS";
        Font = UiHelper.Body;
        BackColor = UiHelper.Surface;
        Width = 1280;
        Height = 800;
        MinimumSize = new Size(1100, 700);
        StartPosition = FormStartPosition.CenterScreen;

        var side = BuildSidebar();
        var menu = BuildMenuStrip();
        var tools = BuildToolStrip();

        var status = new StatusStrip { BackColor = UiHelper.PanelBack, ForeColor = UiHelper.Muted, SizingGrip = false };
        status.Items.Add(_lblScreen);
        status.Items.Add(new ToolStripSeparator());
        status.Items.Add(_lblUser);
        status.Items.Add(new ToolStripSeparator());
        status.Items.Add(_lblClock);

        Controls.Add(_content);
        Controls.Add(side);
        Controls.Add(status);
        Controls.Add(tools);
        Controls.Add(menu);
        MainMenuStrip = menu;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        Navigate(0);
    }

    private MenuStrip BuildMenuStrip()
    {
        var menu = new MenuStrip
        {
            BackColor = UiHelper.PanelBack,
            ForeColor = UiHelper.Ink,
            Renderer = new ToolStripProfessionalRenderer(new MenuColors()),
            Padding = new Padding(6, 2, 0, 2)
        };

        var file = new ToolStripMenuItem("&File");
        file.DropDownItems.Add(Item("Log out", Keys.Control | Keys.L, (s, e) => LogOut()));
        file.DropDownItems.Add(Item("Exit", Keys.Control | Keys.Q, (s, e) => Close()));

        var go = new ToolStripMenuItem("&Go to");
        go.DropDownItems.Add(Item("New order", Keys.Control | Keys.N, (s, e) => Navigate(0)));
        go.DropDownItems.Add(Item("Menu manager", Keys.Control | Keys.M, (s, e) => Navigate(1)));
        go.DropDownItems.Add(Item("Order history", Keys.Control | Keys.H, (s, e) => Navigate(2)));
        go.DropDownItems.Add(Item("Reports", Keys.Control | Keys.R, (s, e) => Navigate(3)));
        go.DropDownItems.Add(new ToolStripSeparator());
        go.DropDownItems.Add(Item("Kitchen display", Keys.Control | Keys.K, (s, e) => new KitchenForm().Show(this)));

        var help = new ToolStripMenuItem("&Help");
        help.DropDownItems.Add(Item("About", null, (s, e) =>
        {
            using var about = new AboutForm();
            about.ShowDialog(this);
        }));

        menu.Items.Add(file);
        menu.Items.Add(go);
        menu.Items.Add(help);
        return menu;
    }

    private static ToolStripMenuItem Item(string text, Keys? shortcut, EventHandler onClick)
    {
        var item = new ToolStripMenuItem(text);
        if (shortcut.HasValue) item.ShortcutKeys = shortcut.Value;
        item.Click += onClick;
        return item;
    }

    private ToolStrip BuildToolStrip()
    {
        var tools = new ToolStrip
        {
            BackColor = UiHelper.PanelBack,
            GripStyle = ToolStripGripStyle.Hidden,
            Renderer = new ToolStripProfessionalRenderer(new MenuColors()),
            Padding = new Padding(8, 3, 0, 3)
        };

        var btnNew = new ToolStripButton("New order") { DisplayStyle = ToolStripItemDisplayStyle.Text };
        var btnKitchen = new ToolStripButton("Kitchen display") { DisplayStyle = ToolStripItemDisplayStyle.Text };
        var btnLogout = new ToolStripButton("Log out") { DisplayStyle = ToolStripItemDisplayStyle.Text };
        btnNew.Click += (s, e) => Navigate(0);
        btnKitchen.Click += (s, e) => new KitchenForm().Show(this);
        btnLogout.Click += (s, e) => LogOut();

        tools.Items.Add(btnNew);
        tools.Items.Add(btnKitchen);
        tools.Items.Add(new ToolStripSeparator());
        tools.Items.Add(btnLogout);
        return tools;
    }

    private Panel BuildSidebar()
    {
        var side = new Panel { Dock = DockStyle.Left, Width = 200, BackColor = UiHelper.PanelBack };

        var brand = new Panel { Dock = DockStyle.Top, Height = 72 };
        brand.Paint += (s, e) =>
        {
            var g = e.Graphics;
            using (var mark = new SolidBrush(UiHelper.Brand))
                g.FillRectangle(mark, 20, 21, 30, 30);
            TextRenderer.DrawText(g, "S", UiHelper.Heading, new Rectangle(20, 21, 30, 30), Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            TextRenderer.DrawText(g, AppSettings.ShopName, UiHelper.Heading, new Rectangle(60, 21, 136, 30),
                UiHelper.Ink, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        };

        var edge = new Panel { Dock = DockStyle.Right, Width = 1, BackColor = UiHelper.Border };

        _nav.Add(MakeNav("New order", () => new OrderForm()));
        _nav.Add(MakeNav("Menu manager", () => new MenuManagerForm()));
        _nav.Add(MakeNav("Order history", () => new HistoryForm()));
        _nav.Add(MakeNav("Reports", () => new ReportsForm()));

        for (int i = _nav.Count - 1; i >= 0; i--) side.Controls.Add(_nav[i]);
        side.Controls.Add(brand);
        side.Controls.Add(edge);
        return side;
    }

    private NavButton MakeNav(string text, Func<Form> create)
    {
        var b = new NavButton { Text = text };
        int index = _nav.Count;
        b.Click += (s, e) => Navigate(index);
        _screens.Add(create);
        return b;
    }

    private void Navigate(int index)
    {
        if (index < 0 || index >= _screens.Count) return;

        if (_current != null)
        {
            _content.Controls.Remove(_current);
            _current.Dispose();
        }

        var f = _screens[index]();
        _current = f;
        f.TopLevel = false;
        f.FormBorderStyle = FormBorderStyle.None;
        f.Dock = DockStyle.Fill;
        _content.Controls.Add(f);
        f.Show();

        _lblScreen.Text = f.Text;
        for (int i = 0; i < _nav.Count; i++) _nav[i].Selected = i == index;
    }

    private void LogOut()
    {
        LoggedOut = true;
        Close();
    }

    private sealed class MenuColors : ProfessionalColorTable
    {
        public override Color MenuStripGradientBegin => UiHelper.PanelBack;
        public override Color MenuStripGradientEnd => UiHelper.PanelBack;
        public override Color ToolStripGradientBegin => UiHelper.PanelBack;
        public override Color ToolStripGradientMiddle => UiHelper.PanelBack;
        public override Color ToolStripGradientEnd => UiHelper.PanelBack;
        public override Color ToolStripBorder => UiHelper.Border;
        public override Color ButtonSelectedGradientBegin => UiHelper.Hover;
        public override Color ButtonSelectedGradientMiddle => UiHelper.Hover;
        public override Color ButtonSelectedGradientEnd => UiHelper.Hover;
        public override Color ButtonSelectedBorder => UiHelper.Border;
        public override Color ButtonPressedGradientBegin => UiHelper.Border;
        public override Color ButtonPressedGradientMiddle => UiHelper.Border;
        public override Color ButtonPressedGradientEnd => UiHelper.Border;
        public override Color ToolStripDropDownBackground => UiHelper.PanelBack;
        public override Color ImageMarginGradientBegin => UiHelper.PanelBack;
        public override Color ImageMarginGradientMiddle => UiHelper.PanelBack;
        public override Color ImageMarginGradientEnd => UiHelper.PanelBack;
        public override Color MenuItemSelected => UiHelper.Hover;
        public override Color MenuItemSelectedGradientBegin => UiHelper.Hover;
        public override Color MenuItemSelectedGradientEnd => UiHelper.Hover;
        public override Color MenuItemPressedGradientBegin => UiHelper.Hover;
        public override Color MenuItemPressedGradientMiddle => UiHelper.Hover;
        public override Color MenuItemPressedGradientEnd => UiHelper.Hover;
        public override Color MenuItemBorder => UiHelper.Border;
        public override Color MenuBorder => UiHelper.Border;
        public override Color SeparatorDark => UiHelper.Border;
    }
}
