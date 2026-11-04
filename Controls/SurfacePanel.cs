namespace FastFoodPOS.CustomControls;

public class SurfacePanel : Panel
{
    public SurfacePanel()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;
        BackColor = UiHelper.PanelBack;
        Padding = new Padding(1);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        using var pen = new Pen(UiHelper.Border);
        e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
    }
}
