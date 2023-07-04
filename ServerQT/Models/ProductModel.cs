using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;

namespace ServerQT.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int CategoryID { get; set; }
        public int BranchID { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public IFormFile File { get; set; }
        public string Path { get; set; }

    }
}
