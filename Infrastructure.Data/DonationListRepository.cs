using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Domain;

namespace Infrastructure.Data
{
    public sealed class DonationRepositoryList : IDonationRepository
    {
        private static readonly List<Donation> _donations = new List<Donation>();

        public void Insert(Donation donation)
        {
            var count = _donations.Any() ? _donations.Max(x => x.Id) + 1 : 1;
            donation.Id = count;
            _donations.Add(donation);
        }

        public void Update(Donation donation)
        {
            var result = _donations.FirstOrDefault(x => x.Id == donation.Id);
            if (result != null)
            {
                _donations.Remove(result);
                _donations.Add(donation);
            }
        }

        public void Delete(Donation donation)
        {
            var result = GetForId(donation.Id);

            if (result != null)
            {
                _donations.Remove(result);

            }
        }

        public IEnumerable<Donation> GetDonations()
        {
            return _donations;
        }

        public Donation GetForId(int id)
        {
            return _donations.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Donation> GetName(string name)
        {

            return _donations.Where(x => Regex.IsMatch(
                x.Name, name, RegexOptions.IgnoreCase) || Regex.IsMatch(
                x.Description, name, RegexOptions.IgnoreCase));
        }

        public IList<Donation> GetFiveLast()
        {
            return _donations.OrderByDescending(x => x.GetDataRegister()).Take(5).ToList();
        }
    }
}