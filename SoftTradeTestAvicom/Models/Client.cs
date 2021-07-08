using System.Collections.Generic;

namespace SoftTradeTestAvicom.Models
{
    public class Client
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Status { get; set; }

        public int ManagerId { get; set; }

        public Manager Manager { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
