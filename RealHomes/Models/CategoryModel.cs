
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace RealHomes.Models
{
    [TableName("RHCategory")]
    [PrimaryKey("CatId", autoIncrement = true)]
    public class CategoryModel
    {

        [Column("CatId")]
        [System.ComponentModel.DataAnnotations.KeyAttribute]
        public long CategoryId { get; set; }


        [Column("Name")]
        public string CategoryName { get; set; }

    }
}