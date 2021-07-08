using System.Collections.Generic;

namespace SoftTradeTestAvicom.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Price { get; set; }

        public string Type { get; set; }

        public string SubscriptionPeriod { get; set; }

        public ICollection<Client> Clients { get; set; }
    }
}
