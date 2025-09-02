using System;
using System.Collections.Generic;

namespace BikeDealersProject.Models;

public partial class DealerMaster
{
    public int DealerMasterId { get; set; }

    public int DealerId { get; set; }

    public int BikeId { get; set; }

    public int? BikesDelivered { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public virtual BikeStore? Bike { get; set; }

    public virtual Dealer? Dealer { get; set; }
}
