using System.Collections.Generic;

namespace SoftTradeTestAvicom.Models
{
    public class Manager
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Client> Clients { get; set; }
    }
}
