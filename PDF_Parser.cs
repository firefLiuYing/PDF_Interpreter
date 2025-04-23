using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;

namespace PDF_Interpreter
{
    internal class PDF_Parser
    {
        public void Parser()
        {
            Debug.WriteLine("测试Debug功能");
        }

        public string FindPDF(string Input_Path,string Output_Path)
        {
            //检测路径是否正确
            if (!File.Exists(Input_Path))  MessageBox.Show("文件不存在!", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            if (!File.Exists(Output_Path)) MessageBox.Show("输出路径不存在！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);

            return ExtractTextFromPdf(Input_Path);

        }

        public static string ExtractTextFromPdf(string path)
        {
            using (PdfReader reader = new PdfReader(path))
            {
                string text = string.Empty;
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    text += PdfTextExtractor.GetTextFromPage(reader, i);
                }
                return text;
            }
        }
    }
}
