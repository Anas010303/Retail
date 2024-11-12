using Azure;
using Azure.Data.Tables;

namespace ABC_Retail.Models
{
    public class CustomerProfile : ITableEntity
    {
        public string Id { get; set; } // Unique identifier for the customer profile
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; } // Ensure this matches the property used in your view

        // ITableEntity implementation
        public string PartitionKey { get; set; } = "Customer"; // Define a partition key
        public string RowKey { get; set; } // Row key should match the unique identifier, e.g., Id
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        // Constructor
        public CustomerProfile()
        {
            // Initialize RowKey with Id if needed
            RowKey = Id;
        }
    }
}
