using System.Diagnostics;
using System.Net.Http.Headers;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace PdfInterpreter
{
    internal class PdfParser
    {
        public void Parser()
        {
            Debug.WriteLine("测试Debug功能");
        }

        public async Task<string> FindPdf(string inputPath,string outputPath)
        {
            await Task.Delay(1);
            List<string> paragraphs = await AsposeExtractor.ExtractTextFromPdf(inputPath);
            /*List<string> translateds = [];
            foreach (string par in paragraphs)
            {
                string translated = await Interpreter.InterpretAsync(par);
                translateds.Add(translated);
            }*/
            return "翻译完成";
        }
    }
}
