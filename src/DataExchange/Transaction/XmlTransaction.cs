using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Caliburn.Micro;
using Castle.Windsor;
using Library.Extensions;

namespace DataExchange.Transaction
{
    public class XmlTransaction
    {
        private const int RequestTimeout = 5000;
        private const int ResponseTimeout = 7000;

        public string Status { get; set; }



        public async Task<XDocument> PostXmlTransaction(string uri, XDocument xmlRequest)
        {
            //Declare XMLResponse document
            XmlDocument XMLResponse = null;
            XDocument XResponse = null;

            //Declare an HTTP-specific implementation of the WebRequest class.
            HttpWebRequest objHttpWebRequest;

            //Declare an HTTP-specific implementation of the WebResponse class
            HttpWebResponse objHttpWebResponse = null;

            //Declare a generic view of a sequence of bytes
            Stream objRequestStream = null;
            Stream objResponseStream = null;

            //Declare XMLReader
            XmlTextReader objXMLReader;

            //Creates an HttpWebRequest for the specified URL.
            objHttpWebRequest = (HttpWebRequest)WebRequest.Create(uri);

            try
            {
                //---------- Start HttpRequest 

                //Set HttpWebRequest properties
                var bytes = System.Text.Encoding.ASCII.GetBytes(xmlRequest.ToString());
                objHttpWebRequest.Method = "POST";
                objHttpWebRequest.ContentLength = bytes.Length;
                objHttpWebRequest.ContentType = "text/xml; encoding='utf-8'";



                Status = $"Отправка запроса.... на \"{uri}\"";


                //Get Stream object 
                objRequestStream = await objHttpWebRequest.GetRequestStreamAsync().WithTimeout(RequestTimeout);         //блокирующий вызов

                //Writes a sequence of bytes to the current stream 
                await objRequestStream.WriteAsync(bytes, 0, bytes.Length);


                Status = "запись байт в поток....";

                //Close stream
                objRequestStream.Close();

                //---------- End HttpRequest

                //Sends the HttpWebRequest, and waits for a response.
                objHttpWebResponse = (HttpWebResponse)await objHttpWebRequest.GetResponseAsync().WithTimeout(ResponseTimeout);

                //---------- Start HttpResponse
                if (objHttpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    Status = "Ответ получен !!!";

                    //Get response stream 
                    objResponseStream = objHttpWebResponse.GetResponseStream();

                    //Load response stream into XMLReader
                    objXMLReader = new XmlTextReader(objResponseStream);

                    //Declare XMLDocument
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(objXMLReader);                 //EXCEPTION: whereAre multiply root elem


                    //Set XMLResponse object returned from XMLReader
                    XMLResponse = xmldoc;
                    XResponse = XDocument.Parse(XMLResponse.OuterXml);
                    Status = "Считанный XML= " + XResponse;

                    //Close XMLReader
                    objXMLReader.Close();
                }

                //Close HttpWebResponse
                objHttpWebResponse.Close();
            }
            catch (WebException we)
            {
                Status = we.ToString();
                throw new Exception(we.Message);
            }
            catch (Exception ex)
            {
                Status = $"Неизвестное Исключение: {ex.ToString()}";
                throw new Exception(ex.Message);
            }
            finally
            {
                objRequestStream?.Close();
                objResponseStream?.Close();
                objHttpWebResponse?.Close();

                objRequestStream?.Dispose();
                objResponseStream?.Dispose();
                objHttpWebResponse?.Dispose();
            }

            return XResponse;
        }
    }
}