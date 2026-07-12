using iTextSharp.text;

namespace Timeless.PdfTools.Lib.ITextSharp;

public class RectangleJ
{
    private float _x;
    private float _y;
    private float _width;
    private float _height;
        
    public RectangleJ(Rectangle rectangle)
    {
        rectangle.Normalize();
        _x = rectangle.Left;
        _y = rectangle.Bottom;
        _width = rectangle.Width;
        _height = rectangle.Height;
    }

    public virtual float X
    {
        get => _x;
        set => _x = value;
    }

    public virtual float Y
    {
        get => _y;
        set => _y = value;
    }

    public virtual float Width
    {
        get => _width;
        set => _width = value;
    }

    public virtual float Height
    {
        get => _height;
        set => _height = value;
    }
}