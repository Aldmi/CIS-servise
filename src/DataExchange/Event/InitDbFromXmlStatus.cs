using System;
using DataExchange.InitDb;
using Domain.Entities;

namespace DataExchange.Event
{
    public class InitDbFromXmlStatus
    {
        public Station OwnerStation { get; set; }
        public Status Status { get; set; }
        public string StatusString { get; set; }
        public DateTime Time { get; set; } = DateTime.Now; 
    }
}