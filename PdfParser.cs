using System.Diagnostics;
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

        public string FindPdf(string inputPath,string outputPath)
        {
            //检测路径是否正确
            //if (!File.Exists(inputPath))  MessageBox.Show("文件不存在!", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            //if (!File.Exists(outputPath)) MessageBox.Show("输出路径不存在！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);

            try
            {
                PdfWriter pdfWriter = new(outputPath);
                PdfDocument pdfDocument=new(pdfWriter);
                Document document = new(pdfDocument);
                document.Add(new Paragraph("Hello World!"));
                document.Close();
                
            }
            catch(Exception ex)
            {
                MyDebug.Log(ex.Message);
            }
            return ExtractTextFromPdf(inputPath);

        }

        public static string ExtractTextFromPdf(string path)
        {
            return "哈哈哈";
        }
    }
}
