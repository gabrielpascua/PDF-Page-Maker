using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text.html.simpleparser;

public partial class maker : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string html = GetHTMLOutput("~/render.aspx?id=1");

        if (!string.IsNullOrEmpty(html))
        {
            string file = Server.MapPath("~/pdf") + "\\using.pdf";

            using (Document doc = new Document(PageSize.LETTER, 25, 25, 25, 25))
            {
                using (PdfWriter pdf = PdfWriter.GetInstance(doc, new FileStream(file, FileMode.Create)))
                {
                    List<IElement> code = HTMLWorker.ParseToList(new StringReader(html), null);

                    doc.Open();

                    foreach (IElement el in code)
                    {
                        doc.Add(el);
                    }

                    doc.Close();
                }
            }
        }
    }

    private string GetHTMLOutput(string relativeUrl)
    {
        string html = string.Empty;

        StringWriter sw = new StringWriter();

        try
        {
            Server.Execute(relativeUrl, sw);
            html = sw.ToString();
        }
        catch (Exception ex)
        {
            //log error here
        }

        return html;
    }
}