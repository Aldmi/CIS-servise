using System.Runtime.Serialization;

namespace WCFCis2AvtodictorContract.DataContract
{
    [DataContract]
    public class DiagnosticData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int DeviceId { get; set; }   //Числовой Уникальный номер, присвоенный устройству в системе информирования.

        [DataMember]
        public int Status { get; set; }     //Код общего статуса технического состояния устройства: исправен - неисправен

        [DataMember]
        public string Fault { get; set; }   //Строковый Описание ошибки по устройству, 
    }
}