using System.Threading.Tasks;
using System.Xml.Linq;
using Caliburn.Micro;
using Castle.Windsor;
using DataExchange.Transaction;
using Domain.Entities;

namespace DataExchange.XmlGetter
{
    public class GetterXmlFromHttp : IGetterXml
    {
        private readonly string _uri;
        public Station OwnerStation { get; }



        public GetterXmlFromHttp(string uri, Station ownerStation)
        {
            _uri = uri;
            OwnerStation = ownerStation;
        }


        public async Task<XDocument> Get(XDocument request = null)
        {
            var xmlTransaction = new XmlTransaction();
            var xmlResp = await xmlTransaction.PostXmlTransaction(_uri, request);

            return xmlResp;
        }
    }
}