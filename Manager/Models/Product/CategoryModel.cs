using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Manager.Models.Product
{
    public class CategoryModel
    {
        public string categorycode1 { get; set; }
        public string categoryname1 { get; set; }
        public string categorycode2 { get; set; }
        public string categoryname2 { get; set; }
        public string displayflag { get; set; }
        public int displaynum { get; set; }        
        public int productcnt { get; set; }
        public int productsalecnt { get; set; }
    }

    public class CategoryListModel
    {
        public List<CategoryModel> Category1List { get; set; }
        public List<CategoryModel> Category2List { get; set; }
    }
}