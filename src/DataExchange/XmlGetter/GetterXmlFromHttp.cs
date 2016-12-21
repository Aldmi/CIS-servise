using System.Xml.Linq;
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


        public XDocument Get(XDocument request = null)
        {
            throw new System.NotImplementedException();
        }
    }
}