using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Caliburn.Micro;
using Castle.Windsor;
using Domain.Entities;

namespace DataExchange.XmlGetter
{
    public class GetterXmlFromDisk : IGetterXml
    {
        private readonly string _path;

        public Station OwnerStation { get; }            //станция для которой полученн XML





        public GetterXmlFromDisk(string path, Station ownerStation)
        {
            _path = path;
            OwnerStation = ownerStation;
        }


     

        public async Task<XDocument> Get(XDocument request = null)
        {
            var extension = Path.GetExtension(_path);
            if (extension != null && (File.Exists(_path) && extension.ToLower() == ".xml"))
            {
                return await Task.Factory.StartNew(() => XDocument.Load(_path));
            }

            return null;
        }
    }
}