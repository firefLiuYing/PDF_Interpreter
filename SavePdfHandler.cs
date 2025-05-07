using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfInterpreter
{
    public class SavePdfHandler
    {
        public async Task SavePdfAsync(string inputPath, string outputPath, Func<string, Task<string>> translateFunc)
        {
            PdfTextExtractor extractor = new();
            List<TextBlock> translatedBlocks= await extractor.ExtractTextAsync(inputPath, translateFunc);
            PdfTextReplacer replacer = new();
            await replacer.CreateTranslatedPdfAsync(inputPath, outputPath, translatedBlocks);
            MyDebug.Log("PDF翻译完成,已保存至"+outputPath);
        }
    }
}
