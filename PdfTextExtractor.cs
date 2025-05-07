using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfInterpreter
{
    public class PdfTextExtractor
    {
        public async Task<List<TextBlock>> ExtractTextAsync(string pdfPath,Func<string ,Task<string>> translateFunc)
        {
            PdfDocument pdfDocument = new(new PdfReader(pdfPath));
            TextExtractorListener listener = new();
            PdfCanvasProcessor processor=new(listener);

            await Task.Run(() =>
            {
                for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
                {
                    processor.ProcessPageContent(pdfDocument.GetPage(i));
                }
            });
            listener.ExtractEnd();
            List<TextBlock> translatedBlocks = new();
            foreach (var block in listener.TextBlocks)
            {
                string translatedText = await translateFunc(block.Text);
                translatedBlocks.Add(new TextBlock
                {
                    Text = translatedText,
                    FontSize = block.FontSize,
                    X = block.X,
                    Y = block.Y
                });
            }
            return translatedBlocks;
        }
    }
}
