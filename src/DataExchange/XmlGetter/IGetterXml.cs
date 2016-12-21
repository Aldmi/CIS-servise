using System.Xml.Linq;
using Domain.Entities;

namespace DataExchange.XmlGetter
{
    public interface IGetterXml
    {
        Station OwnerStation { get; } 

        XDocument Get(XDocument request = null);
    }
}