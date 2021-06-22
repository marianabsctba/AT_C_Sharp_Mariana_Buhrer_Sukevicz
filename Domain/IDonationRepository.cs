using System;
using System.Collections.Generic;

namespace Domain
{
    public interface IDonationRepository
    {
        IEnumerable<Donation> GetDonations();
        IList<Donation> GetFiveLast();
        Donation GetForId(int id);
        IEnumerable<Donation> GetName(string name);
        void Insert(Donation donation);
        void Update(Donation donation);
        void Delete(Donation donation);
    }
}