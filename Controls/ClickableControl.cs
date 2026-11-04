namespace FastFoodPOS.CustomControls;

public abstract class ClickableControl : Control
{
    protected bool Hot { get; private set; }
    protected bool Pressed { get; private set; }

    protected ClickableControl()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                 ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                 ControlStyles.Selectable, true);
        TabStop = true;
        Cursor = Cursors.Hand;
    }

    protected void ResetPress()
    {
        Pressed = false;
        Invalidate();
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        Hot = true;
        Invalidate();
        base.OnMouseEnter(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        Hot = false;
        Pressed = false;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            Pressed = true;
            Invalidate();
        }
        base.OnMouseDown(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        Pressed = false;
        Invalidate();
        base.OnMouseUp(e);
    }

    protected override void OnGotFocus(EventArgs e)
    {
        Invalidate();
        base.OnGotFocus(e);
    }

    protected override void OnLostFocus(EventArgs e)
    {
        Invalidate();
        base.OnLostFocus(e);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.KeyCode is Keys.Space or Keys.Enter)
        {
            e.Handled = true;
            OnClick(EventArgs.Empty);
        }
        base.OnKeyDown(e);
    }

    protected void DrawFocus(Graphics g, Rectangle bounds)
    {
        if (Focused && ShowFocusCues) ControlPaint.DrawFocusRectangle(g, bounds);
    }
}
