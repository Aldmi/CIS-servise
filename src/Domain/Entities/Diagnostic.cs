using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// Диагностика оборудования автодиктора
    /// Данные о техническом состоянии устройств системы информирования пассажиров.
    /// </summary>
    public class Diagnostic : IEntitie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DeviceNumber  { get; set; }   //Числовой Уникальный номер, присвоенный устройству в системе информирования.

        public string DeviceName { get; set; }   //Числовой Уникальный номер, присвоенный устройству в системе информирования.

        [Required]
        public int Status { get; set; }          //Код общего статуса технического состояния устройства: исправен - неисправен

        [Column(TypeName = "datetime2")]
        [Required]
        public DateTime Date { get; set; }

        public string Fault { get; set; }        //Строковый Описание ошибки по устройству, если оно неисправно
    }
}