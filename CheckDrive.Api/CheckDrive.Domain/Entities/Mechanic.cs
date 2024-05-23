﻿using CheckDrive.Domain.Common;

namespace CheckDrive.Domain.Entities
{
    public class Mechanic : EntityBase
    {
        public int AccountId { get; set; }
        public Account Account { get; set; }

        public virtual ICollection<DispatcherReview> DispetcherReviews { get; set; }
        public virtual ICollection<MechanicHandover> MechanicHandovers { get; set; }
        public virtual ICollection<MechanicAcceptance> MechanicAcceptance { get; set; }
    }
}
