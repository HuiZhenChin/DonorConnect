using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DonorConnect.Class
{
    public class PageEventHelper : PdfPageEventHelper
    {
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            PdfPTable pageFooter = new PdfPTable(1);
            pageFooter.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            pageFooter.DefaultCell.Border = Rectangle.NO_BORDER;

            PdfPCell cell = new PdfPCell(new Phrase("Page " + writer.PageNumber, new Font(Font.FontFamily.HELVETICA, 10, Font.ITALIC)))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Border = Rectangle.NO_BORDER,
                PaddingRight = 10
            };

            pageFooter.AddCell(cell);
            pageFooter.WriteSelectedRows(0, -1, document.LeftMargin, document.BottomMargin - 10, writer.DirectContent);
        }
    }
}