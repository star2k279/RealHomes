
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;
namespace RealHomes.Models
{
    [TableName("RHPropertyType")]
    [PrimaryKey("TypeId", autoIncrement = true)]
    public class PropertyTypeModel
    {
        [Column("TypeId")]
        public long TypeId { get; set; }

        [Column("TypeName")]
        public string TypeName { get; set; }

        [Column("TypeCode")]
        public string TypeCode { get; set; }

        [Column("CategoryId")]
        public long CategoryId { get; set; }
    }
}