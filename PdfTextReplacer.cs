using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfInterpreter
{
    internal class PdfTextReplacer
    {
        public async Task CreateTranslatedPdfAsync(string inputPath, string outputPath,List<TextBlock> translatedTextBlocks)
        {
            await Task.Run(() =>
            {
                try
                {
                    PdfDocument inputPdf=new(new PdfReader(inputPath));
                    PdfDocument outputPdf=new(new PdfWriter(outputPath));
                    for(int i = 1; i <= inputPdf.GetNumberOfPages(); i++)
                    {
                        PdfPage page=inputPdf.GetPage(i);
                        PageSize pageSize=new(page.GetPageSize());
                        PdfPage newPage=outputPdf.AddNewPage(pageSize);
                        PdfCanvas canvas=new(newPage);
                        foreach(var block in translatedTextBlocks)
                        {
                            canvas.BeginText()
                            .SetFontAndSize(PdfFontFactory.CreateFont(), block.FontSize)
                            .MoveText(block.X, block.Y)
                            .ShowText(block.Text)
                            .EndText();
                        }
                    }
                    outputPdf.Close();
                }
                catch (Exception ex)
                {
                    MyDebug.Log(ex.Message);
                }
            });
        }
    }
}
