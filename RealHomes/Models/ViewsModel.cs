using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Umbraco.Core.Persistence;

namespace RealHomes.Models
{
    [TableName("RHViews")]
    [PrimaryKey("id", autoIncrement =true)]
    public class ViewsModel
    {
        [Column("id")]
        public long ViewId { get; set; }

        [Column("Name")]
        public string ViewName { get; set; }
    }
}