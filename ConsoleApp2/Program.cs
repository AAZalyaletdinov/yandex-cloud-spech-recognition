using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Collections;

namespace ConsoleApp2
{
    class Program
    {
        static string KEY = "keyFromHere_https://developer.tech.yandex.ru/keys/";
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World! ");
            var f = new FileStream("speech.wav", FileMode.Open, FileAccess.Read);
            var g= PostMethod(f);

            Console.Write(g);
            Console.ReadKey();            
        }

        //код взял отсюда http://www.cyberforum.ru/web-services-wcf/thread1310503.html
        static public string PostMethod(Stream fs)
        {
            string postUrl = "https://asr.yandex.net/asr_xml?" +
            "uuid=" + System.Guid.NewGuid().ToString() + "&" +
            "key=" + KEY + "&" +
            "topic=queries";
            postUrl = "https://asr.yandex.net/asr_xml?uuid=c3d106c898d0433c80e7a791b790357d&key=41060ed1-38cf-4953-ba62-b36c1b9cbf52&topic=queries";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl);
            request.Method = "POST";
            request.Host = "asr.yandex.net";
            request.SendChunked = true;      
            request.ContentType = "audio/x-wav";//"audio/x-pcm;bit=16;rate=16000";
            request.ContentLength = fs.Length;

            using (var newStream = request.GetRequestStream())
            {
                fs.CopyTo(newStream);
                //newStream.Write(bytes, 0, bytes.Length); if by bytes
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string responseToString = "";
            if (response != null)
            {
                var strreader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                responseToString = strreader.ReadToEnd();
            }

            int index = responseToString.IndexOf("<variant confidence=\"1\">");

            responseToString = responseToString.Substring(index + 24, responseToString.Length - index - 24);

            int index2 = responseToString.IndexOf("</variant>");

            responseToString = responseToString.Substring(0, index2);

            return responseToString;
        }
    }
}
