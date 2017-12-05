using System;

using System.ComponentModel.DataAnnotations;

using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace RealHomes.Models
{
    [TableName("RHApplicationErrors")]
    [PrimaryKey("ID", autoIncrement = true)]
    public class ApplicationErrorModel
    {
        [Column("errorId")]
        [KeyAttribute]
        public long ErrorId { get; set; }

        [Column("Message")]
        public string Message { get; set; }

        [Column("OccuredOn")]
        public DateTime OccuredOn { get; set; }

        [Column("MachineID")]
        public string MachineID { get; set; }


    }
}