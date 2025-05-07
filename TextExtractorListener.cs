using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace PdfInterpreter
{
    public class TextExtractorListener : IEventListener
    {
        public List<TextBlock> TextBlocks { get; private set; }
        private string rowText = string.Empty;
        private string mainText=string.Empty;
        private float fontSize = 0;
        private float x = 0;
        private float y = 0;
        public void EventOccurred(IEventData data, EventType type)
        {
            if (type == EventType.BEGIN_TEXT) rowText = string.Empty;
            else if (type == EventType.END_TEXT)
            {
                if (rowText.Length == 1 && rowText[0] == ' ') PushMainText();
                else mainText += rowText;
            }
            else if (type == EventType.RENDER_TEXT)
            {
                var renderInfo = (TextRenderInfo)data;
                string text = renderInfo.GetText();
                if (text.Length == 1 && text[0] == 1) text = text.Replace(text[0], ' ');
                else if (rowText.Length <= 0)
                {
                    fontSize = renderInfo.GetFontSize();
                    x = renderInfo.GetBaseline().GetStartPoint().Get(0);
                    y = renderInfo.GetBaseline().GetStartPoint().Get(1);
                    

                    MyDebug.Log("x: " + x + " y: " + y + "  " + renderInfo.GetText());
                }
                rowText += text;
            }
        }
        public void ExtractEnd() => PushMainText();
        private void PushMainText()
        {
            TextBlock textBlock = new()
            {
                Text = mainText,
                FontSize = fontSize,
                X = x,
                Y = y
            };
            TextBlocks.Add(textBlock);
            mainText = string.Empty;
        }
        public ICollection<EventType> GetSupportedEvents()=> 
            [EventType.BEGIN_TEXT,EventType.RENDER_TEXT,EventType.END_TEXT,EventType.RENDER_IMAGE,EventType.RENDER_PATH];
        public TextExtractorListener()=> TextBlocks = new();
    }
    public class TextBlock
    {
        public string Text { get; set; }
        public float FontSize { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
    }
}
