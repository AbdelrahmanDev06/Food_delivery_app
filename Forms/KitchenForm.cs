namespace FastFoodPOS.Forms;

public class KitchenForm : Form
{
    private static readonly Color Floor = Color.FromArgb(38, 37, 35);
    private static readonly Color TopBar = Color.FromArgb(28, 27, 25);
    private static readonly Color Fresh = Color.FromArgb(232, 231, 227);
    private static readonly Color Late = Color.FromArgb(226, 166, 39);
    private static readonly Color Overdue = Color.FromArgb(209, 59, 38);
    private static readonly Font BodyFont = new("Segoe UI", 11.5f);
    private static readonly Font TitleFont = new("Segoe UI Semibold", 13f);
    private static readonly Font BarFont = new("Segoe UI Semibold", 12f);

    public KitchenForm()
    {
        Text = "Kitchen display";
        Size = new Size(960, 560);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = Floor;

        var board = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(12), BackColor = Floor };
        board.Controls.Add(Card(41, "just now", Fresh, "1 x Cheese Burger\n   + Extra cheese\n2 x French Fries\n1 x Cola"));
        board.Controls.Add(Card(40, "6 min", Late, "2 x Double Burger\n1 x Onion Rings\n2 x Orange Juice"));
        board.Controls.Add(Card(39, "14 min", Overdue, "1 x Crispy Chicken\n   + Bacon\n1 x Brownie"));

        var bar = new Label
        {
            Text = "Kitchen display  -  3 open tickets",
            Dock = DockStyle.Top,
            Height = 46,
            ForeColor = Color.White,
            BackColor = TopBar,
            Font = BarFont,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(16, 0, 0, 0)
        };

        Controls.Add(board);
        Controls.Add(bar);
    }

    private static Control Card(int orderId, string age, Color headColor, string lines)
    {
        var card = new Panel { Width = 280, Height = 230, BackColor = Color.White, Margin = new Padding(8) };

        var head = new Panel { Dock = DockStyle.Top, Height = 42, BackColor = headColor };
        var title = new Label
        {
            Text = $"Order #{orderId}",
            Dock = DockStyle.Fill,
            Font = TitleFont,
            ForeColor = UiHelper.Ink,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(12, 0, 0, 0)
        };
        var time = new Label
        {
            Text = age,
            Dock = DockStyle.Right,
            Width = 110,
            Font = BodyFont,
            ForeColor = UiHelper.Ink,
            TextAlign = ContentAlignment.MiddleRight,
            Padding = new Padding(0, 0, 12, 0)
        };
        head.Controls.Add(title);
        head.Controls.Add(time);

        var body = new Label { Text = lines, Dock = DockStyle.Fill, Font = BodyFont, ForeColor = UiHelper.Ink, Padding = new Padding(12, 10, 12, 6) };

        var done = new Button
        {
            Text = "Done",
            Dock = DockStyle.Bottom,
            Height = 42,
            Font = BarFont,
            BackColor = UiHelper.Ok,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            UseVisualStyleBackColor = false
        };
        done.FlatAppearance.BorderSize = 0;

        card.Controls.Add(body);
        card.Controls.Add(done);
        card.Controls.Add(head);
        return card;
    }
}
