namespace FastFoodPOS.CustomControls;

public class AppButton : Button
{
    private BtnStyle _kind;

    public AppButton(string text, BtnStyle kind = BtnStyle.Secondary, int minWidth = 100)
    {
        Text = text;
        Font = UiHelper.Semibold;
        FlatStyle = FlatStyle.Flat;
        UseVisualStyleBackColor = false;
        Cursor = Cursors.Hand;
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        MinimumSize = new Size(minWidth, 34);
        Padding = new Padding(10, 0, 10, 0);
        Margin = new Padding(0, 0, 8, 0);
        _kind = kind;
        ApplyLook();
    }

    public BtnStyle Kind
    {
        get => _kind;
        set
        {
            _kind = value;
            ApplyLook();
        }
    }

    protected override void OnEnabledChanged(EventArgs e)
    {
        base.OnEnabledChanged(e);
        ApplyLook();
    }

    private void ApplyLook()
    {
        switch (_kind)
        {
            case BtnStyle.Primary:
                BackColor = Enabled ? UiHelper.Brand : Color.FromArgb(200, 198, 193);
                ForeColor = Color.White;
                FlatAppearance.BorderSize = 0;
                FlatAppearance.MouseOverBackColor = UiHelper.BrandDark;
                FlatAppearance.MouseDownBackColor = UiHelper.BrandDark;
                break;
            case BtnStyle.Danger:
                BackColor = UiHelper.PanelBack;
                ForeColor = UiHelper.Danger;
                FlatAppearance.BorderSize = 1;
                FlatAppearance.BorderColor = UiHelper.Danger;
                FlatAppearance.MouseOverBackColor = Color.FromArgb(252, 238, 238);
                FlatAppearance.MouseDownBackColor = Color.FromArgb(247, 222, 222);
                break;
            default:
                BackColor = UiHelper.PanelBack;
                ForeColor = UiHelper.Ink;
                FlatAppearance.BorderSize = 1;
                FlatAppearance.BorderColor = UiHelper.Border;
                FlatAppearance.MouseOverBackColor = UiHelper.Hover;
                FlatAppearance.MouseDownBackColor = UiHelper.Border;
                break;
        }
    }
}
