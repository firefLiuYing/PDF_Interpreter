﻿using Aspose.Pdf;
using Aspose.Pdf.Drawing;
using Aspose.Pdf.Text;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageFormat = System.Drawing.Imaging.ImageFormat;
using Rectangle = Aspose.Pdf.Rectangle;

namespace PdfInterpreter
{
    internal class AsposeExtractor
    {
        public static void ExtractImagesFromPdf(string input, string output)
        {
            Document document = new(input);
            for (int pageIndex = 1; pageIndex <= document.Pages.Count; pageIndex++)
            {
                Page page = document.Pages[pageIndex];
                foreach (XImage xImage in page.Resources.Images)
                {
                    var outputImage = new FileStream(output + pageIndex + "_" + xImage.GetHashCode() + ".jpg", FileMode.Create);
                    xImage.Save(outputImage, ImageFormat.Jpeg);
                }
            }
        }
        public static List<string> ExtractTextFromPdf(string input)
        {
            Document document = new(input);
            List<string> paragraphs = [];
            foreach(var page in document.Pages)
            {
                var absorber = new Aspose.Pdf.Text.ParagraphAbsorber();
                absorber.Visit(page);
                foreach(var markup in absorber.PageMarkups)
                {
                    foreach(MarkupSection section in markup.Sections)
                    {
                        DrawRectangleOnPage(section.Rectangle, page);
                        foreach (var paragraph in section.Paragraphs)
                        {
                            paragraphs.Add(paragraph.Text);
                        }
                    }
                }
            }
            foreach(string par in paragraphs)
            {
                MyDebug.Log(par);
            }
            document.Save("D:\\UserResource\\DeskTop\\Output\\Test.pdf");
            return paragraphs;
        }
        private static void DrawRectangleOnPage(Rectangle rectangle, Page page)
        {
            page.Contents.Add(new Aspose.Pdf.Operators.GSave());
            page.Contents.Add(new Aspose.Pdf.Operators.ConcatenateMatrix(1,0,0,1,0,0));
            page.Contents.Add(new Aspose.Pdf.Operators.SetRGBColorStroke(0,1,0));
            page.Contents.Add(new Aspose.Pdf.Operators.SetLineWidth(2));
            page.Contents.Add(
                new Aspose.Pdf.Operators.Re(rectangle.LLX,
                rectangle.LLY,rectangle.Width,rectangle.Height));
            page.Contents.Add(new Aspose.Pdf.Operators.ClosePathStroke());
            page.Contents.Add(new Aspose.Pdf.Operators.GRestore());
        }
        public static void RebuildPdf(string input, string output, Dictionary<string, string> translations)
        {
            Document document = new(input);
            foreach (var page in document.Pages)
            {
                TextFragmentAbsorber textFragmentAbsorber = new();
                page.Accept(textFragmentAbsorber);
                foreach (TextFragment textFragment in textFragmentAbsorber.TextFragments)
                {
                    string originalText = textFragment.Text;
                    if (translations.ContainsKey(originalText))
                    {
                        textFragment.Text = translations[originalText];
                    }
                }
            }
            document.Save(output);
        }
    }
}
