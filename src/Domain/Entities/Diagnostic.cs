using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    /// <summary>
    /// Диагностика оборудования автодиктора
    /// Данные о техническом состоянии устройств системы информирования пассажиров.
    /// </summary>
    public class Diagnostic
    {
        [Key]
        public int Id { get; set; }

        public int DeviceId { get; set; }   //Числовой Уникальный номер, присвоенный устройству в системе информирования.
        public int Status { get; set; }     //Код общего статуса технического состояния устройства: исправен - неисправен
        public string Fault { get; set; }   //Строковый Описание ошибки по устройству, если оно неисправно
    }
}