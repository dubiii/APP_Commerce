using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace APP_Commerce.Models
{
    public class Category
    {
        [PrimaryKey]
        public int CategoryId { get; set; }

        public string Description { get; set; }

        public int CompanyId { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Product> Products { get; set; }

        public override int GetHashCode()
        {
            return CategoryId;
        }
    }
}
