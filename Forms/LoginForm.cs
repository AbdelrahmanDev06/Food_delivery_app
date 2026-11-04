namespace FastFoodPOS.Forms;

public class LoginForm : Form
{
    private readonly TextBox _txtUsername = new() { Dock = DockStyle.Fill, MaxLength = 30 };
    private readonly TextBox _txtPassword = new() { Dock = DockStyle.Fill, UseSystemPasswordChar = true, MaxLength = 50 };
    private readonly CheckBox _chkShow = new() { Text = "Show password", AutoSize = true, Margin = new Padding(0, 4, 0, 4) };
    private readonly Button _btnLogin = UiHelper.Btn("Log in", BtnStyle.Primary, 110);
    private readonly Button _btnCancel = UiHelper.Btn("Cancel", BtnStyle.Secondary, 90);

    public LoginForm()
    {
        Text = AppSettings.ShopName + " POS - Log in";
        Font = UiHelper.Body;
        BackColor = UiHelper.Surface;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        Padding = new Padding(24);

        var title = new Label
        {
            Text = AppSettings.ShopName,
            Font = UiHelper.Figure,
            ForeColor = UiHelper.Brand,
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 2)
        };
        var subtitle = new Label
        {
            Text = "Log in to start taking orders",
            ForeColor = UiHelper.Muted,
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 16)
        };

        var root = new TableLayoutPanel { AutoSize = true, ColumnCount = 1, RowCount = 0 };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 320));
        AddRow(root, title);
        AddRow(root, subtitle);
        AddRow(root, UiHelper.Caption("Username"), 24);
        AddRow(root, _txtUsername, 34);
        AddRow(root, UiHelper.Caption("Password"), 24);
        AddRow(root, _txtPassword, 34);
        AddRow(root, _chkShow);

        var buttons = new FlowLayoutPanel { AutoSize = true, WrapContents = false, Margin = new Padding(0, 12, 0, 0) };
        buttons.Controls.Add(_btnLogin);
        buttons.Controls.Add(_btnCancel);
        AddRow(root, buttons);
        Controls.Add(root);

        _btnLogin.DialogResult = DialogResult.OK;
        _btnCancel.DialogResult = DialogResult.Cancel;
        AcceptButton = _btnLogin;
        CancelButton = _btnCancel;
    }

    private static void AddRow(TableLayoutPanel t, Control c, int height = 0)
    {
        int row = t.RowCount;
        t.RowCount = row + 1;
        t.RowStyles.Add(height > 0 ? new RowStyle(SizeType.Absolute, height) : new RowStyle(SizeType.AutoSize));
        t.Controls.Add(c, 0, row);
    }
}
