using System;
using System.Collections.Generic;

namespace BikeDealersProject.Models;

public partial class Dealer
{
    public int DealerId { get; set; }

    public string DealerName { get; set; } = null!;

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public int? ZipCode { get; set; }

    public int StorageCapacity { get; set; }

    public int? Inventory { get; set; }

    public virtual ICollection<DealerMaster> DealerMasters { get; set; } = new List<DealerMaster>();
}
