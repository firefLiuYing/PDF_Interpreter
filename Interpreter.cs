using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PDF_Interpreter
{
    internal static class Interpreter
    {
        private const string appId= "";
        private const string secretKey= "";
        private const string url= "http://api.fanyi.baidu.com/api/trans/vip/translate?";
        private static readonly HttpClient httpClient = new();
        /// <summary>
        /// 语言代码:
        /// 中文:zh   英文:en   日语:jp   文言文:wyw
        /// </summary>
        /// <param name="source">原文</param>
        /// <param name="from">原语言</param>
        /// <param name="to">译文语言</param>
        /// <returns></returns>
        public static async Task<string> InterpretAsync(string source,string from="en",string to="zh")
        {
            try
            {
                Random random = new();
                string salt = random.Next(100000).ToString();
                string sign = GenerateMd5(appId + source + salt + secretKey);
                string requestUrl = url;
                requestUrl+= "q=" + HttpUtility.UrlEncode(source);
                requestUrl+= "&from=" + from;
                requestUrl += "&to=" + to;
                requestUrl += "&appid=" + appId;
                requestUrl += "&salt=" + salt;
                requestUrl += "&sign=" + sign;
                using HttpResponseMessage messgae= await httpClient.GetAsync(requestUrl);
                string responseJson = await messgae.Content.ReadAsStringAsync();
                MyDebug.Log(requestUrl);
                MyDebug.Log(responseJson);
                var result=JsonConvert.DeserializeObject<BaiDuInterpretResult>(responseJson);
                if(result!=null)
                {
                    if(result.trans_result!=null&& result.trans_result.Length > 0)
                    {
                        MyDebug.Log("\n原文："+source+"\n译文："+result.trans_result[0].dst);
                        return result.trans_result[0].dst;
                    }
                    else return string.Empty;
                }
                else return string.Empty;
            }
            catch (Exception ex)
            {
                MyDebug.Log($"翻译失败：{ex.Message}");
                return string.Empty;
            }
        }
        private static string GenerateMd5(string input)
        {
            MD5 md5 = MD5.Create();
            // 将字符串转换成字节数组
            byte[] byteOld = Encoding.UTF8.GetBytes(input);
            // 调用加密方法
            byte[] byteNew = md5.ComputeHash(byteOld);
            // 将加密结果转换为字符串
            StringBuilder sb = new StringBuilder();
            foreach (byte b in byteNew)
            {
                // 将字节转换成16进制表示的字符串，
                sb.Append(b.ToString("x2"));
            }
            // 返回加密的字符串
            return sb.ToString();
        }
        private class BaiDuInterpretResult
        {
            public string from;
            public string to;
            public TransResult[] trans_result;
        }
        private class TransResult
        {
            public string src { get; set; }
            public string dst { get; set; }
        }
    }
}
