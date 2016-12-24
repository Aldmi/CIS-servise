using System.Threading.Tasks;
using System.Xml.Linq;
using Domain.Entities;

namespace DataExchange.XmlGetter
{
    public interface IGetterXml
    {
        Station OwnerStation { get; } 

        Task<XDocument> Get(XDocument request = null);
    }
}