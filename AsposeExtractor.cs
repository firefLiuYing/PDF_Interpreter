using Aspose.Pdf;
using Aspose.Pdf.Drawing;
using Aspose.Pdf.Text;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
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
        public static async Task<List<string>> ExtractTextFromPdf(string input)
        {
            Document document = new(input);
            List<string> paragraphs = [];
            foreach (var page in document.Pages)
            {
                var absorber = new ParagraphAbsorber();
                absorber.Visit(page);
                foreach (var markup in absorber.PageMarkups)
                {
                    foreach (MarkupSection section in markup.Sections)
                    {
                        DrawRectangleOnPage(section.Rectangle, page);
                        TextBuilder textBuilder = new(page);
                        TextParagraph textParagraph = new();
                        textParagraph.Rectangle = section.Rectangle;
                        textParagraph.FormattingOptions.WrapMode = TextFormattingOptions.WordWrapMode.ByWords;
                        string toBeTranslated = string.Empty;
                        TextFragmentState curTextState = section.Paragraphs[0].Lines[0][0].TextState;
                        foreach (var paragraph in section.Paragraphs)
                        {
                            if (IsTable(paragraph)) continue;
                            if (HaveUrl(paragraph)) continue;
                            foreach (var line in paragraph.Lines)
                            {
                                foreach(var fragment in line)
                                {
                                    if(fragment.TextState.FontStyle!=FontStyles.Bold)
                                    {
                                        curTextState.FontStyle=FontStyles.Regular;
                                    }
                                    toBeTranslated += fragment.Text;
                                    fragment.TextState.ForegroundColor=Aspose.Pdf.Color.FromArgb(1,1,1,1) ;
                                }
                            }
                        }
                        string finalText = await Interpreter.FreeInterpretAsync(toBeTranslated);
                        MyDebug.Log(finalText);
                        var newFragment = GetTextFragment(finalText, curTextState);
                        textParagraph.AppendLine(newFragment);
                        textBuilder.AppendParagraph(textParagraph);
                    }
                }
                document.Save("D:\\UserResource\\DeskTop\\Output\\Test.pdf");
            }
            return paragraphs;
        }
        private static bool IsUrl(string url)
        {
            string pattern = @"^(https?://)?([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}(/.*)?$";
            Regex regex=new Regex(pattern);
            return regex.IsMatch(url);
        }
        private static void AddText(string text,Rectangle rectangle, Page page,TextFragmentState textState)
        {
            TextParagraph textParagraph = new();
            TextBuilder textBuilder = new(page);
            textParagraph.Rectangle = rectangle;
            textParagraph.FormattingOptions.WrapMode = TextFormattingOptions.WordWrapMode.ByWords;
            TextFragment fragment = new(text);
            MyDebug.Log(text);
            fragment.TextState.FontSize = textState.FontSize;
            fragment.TextState.FontStyle = textState.FontStyle;
            fragment.TextState.CharacterSpacing = textState.CharacterSpacing;
            fragment.TextState.LineSpacing = textState.LineSpacing;
            fragment.TextState.Rotation = textState.Rotation;
            textParagraph.AppendLine(fragment);
            textBuilder.AppendParagraph(textParagraph);
        }
        private static TextFragment GetTextFragment(string text,TextFragmentState textState)
        {
            TextFragment fragment = new(text);
            fragment.TextState.FontSize = textState.FontSize;
            fragment.TextState.FontStyle = textState.FontStyle;
            fragment.TextState.CharacterSpacing = textState.CharacterSpacing;
            fragment.TextState.LineSpacing = textState.LineSpacing;
            fragment.TextState.Rotation = textState.Rotation;
            return fragment;
        }
        private static bool IsTable(MarkupParagraph paragraph)
        {
            Dictionary<int, int> yValueMap = [];
            foreach (var point in paragraph.Points)
            {
                int yValue = (int)point.Y;
                if (yValueMap.ContainsKey(yValue))
                {
                    yValueMap[yValue]++;
                    if (yValueMap[yValue]>=3) return true;
                }
                else
                {
                    yValueMap[yValue] = 1;
                }
            }
            return false;
        }
        private static bool HaveUrl(MarkupParagraph paragraph)
        {
            foreach(var line in  paragraph.Lines)
            {
                foreach (var framgent in line)
                {
                    if (IsUrl(framgent.Text)) return true;
                }
            }
            return false;
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
