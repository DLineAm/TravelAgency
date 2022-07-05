using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace TravelAgency.Services
{
    public class PdfBuilder : IBuilder
    {
        private readonly PdfDocument _doc;
        private readonly PdfPage _page;
        private readonly XGraphics _gfx;
        public PdfBuilder()
        {
            _doc = new PdfDocument();
            _page = _doc.AddPage();
            _gfx = XGraphics.FromPdfPage(_page);
        }

        public PdfBuilder SetTitle(string title)
        {
            _doc.Info.Title = title;
            return this;
        }

        public PdfBuilder DrawString(string text, XFont font, int x, int y)
        {
            _gfx.DrawString(text, font, XBrushes.Black,
                new XRect(x,y, _page.Width,y),
                XStringFormats.Center);
            return this;
        }

        public PdfBuilder DrawString(string text, XFont font, int x, int y, XStringFormat stringFormat)
        {
            _gfx.DrawString(text, font, XBrushes.Black,
                new XRect(x,y, _page.Width,y),
                stringFormat);
            return this;
        }

        public PdfBuilder DrawLine(XColor color, int width, XPoint p1, XPoint p2)
        {
            _gfx.DrawLine(new XPen(color, width), p1, p2);
            return this;
        }

        public IBuilder NewBuilder()
        {
            return new PdfBuilder();
        }

        public object Build()
        {
            return _doc;
        }
    }
}