using System.Drawing.Drawing2D;

namespace FastFoodPOS.Forms;

public class AboutForm : Form
{
    public AboutForm()
    {
        Text = "About";
        Font = UiHelper.Body;
        BackColor = UiHelper.PanelBack;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(420, 300);

        var logo = new Panel { Dock = DockStyle.Top, Height = 110 };
        logo.Paint += (s, e) =>
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var disc = new Rectangle(logo.Width / 2 - 34, 18, 68, 68);
            using (var brush = new LinearGradientBrush(disc, UiHelper.Brand, UiHelper.BrandDark, 45f))
                g.FillEllipse(brush, disc);
            TextRenderer.DrawText(g, "SH", UiHelper.Figure, disc, Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        };

        var info = new Label
        {
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.TopCenter,
            Text = AppSettings.ShopName + " POS  -  GUI version\n\n" +
                   "CS 312 - Modern Development Tools\n" +
                   "C# Windows Forms\n\n" +
                   "This version shows the screens only."
        };

        var ok = UiHelper.Btn("OK", BtnStyle.Primary, 90);
        ok.DialogResult = DialogResult.OK;
        var bottom = new Panel { Dock = DockStyle.Bottom, Height = 56 };
        ok.Location = new Point((ClientSize.Width - ok.Width) / 2, 10);
        ok.Anchor = AnchorStyles.Top;
        bottom.Controls.Add(ok);

        Controls.Add(info);
        Controls.Add(bottom);
        Controls.Add(logo);
        AcceptButton = ok;
        CancelButton = ok;
    }
}
