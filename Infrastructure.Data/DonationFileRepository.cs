using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using Domain;

namespace Infrastructure.Data
{
    public class DonationFileRepository : IDonationRepository
    {
        public List<Donation> _donations = new List<Donation>();

        private readonly string _direc;
        private readonly string _fil = "DonationRegister.txt";

        public DonationFileRepository()
        {

            _direc = Directory.GetCurrentDirectory();
            CreateFiles();
            ReadFiles();
        }


        public void ReadFiles()
        {
            var donations = new List<Donation>();

            var path = $@"{_direc}\{_fil}";
            if (File.Exists(path))
            {
                using (
                    var openFile = File.OpenRead(path))
                {
                    using (var readText = new StreamReader(openFile))
                    {
                        while (readText.EndOfStream == false)
                        {
                            var read = readText.ReadLine();

                            if (read != null)
                            {
                                string[] aux = read.Split(';');
                                DateTime date = DateTime.ParseExact(aux[5], "dd/MM/yyyy", null);
                                donations.Add(
                                    new Donation(
                                        int.Parse(aux[0]), 
                                        bool.Parse(aux[4]),
                                        bool.Parse(aux[2]),
                                        aux[1],
                                        aux[3],
                                        int.Parse(aux[6]),
                                        double.Parse(aux[7]),
                                        date
                                    )
                                    );
                            }
                        }

                        _donations = donations;
                    }
                }
            }
        }
        private void CreateFiles()
        {
            var path = $@"{_direc}\{_fil}";

            if (!File.Exists(path))
            {
                File.Create(path);
            }
        }


        public void Save()
        {
            string fileRoute = $@"{_direc}\{_fil}";
            var way = new List<string>();

            foreach (Donation donation in _donations)
            {
                way.Add(donation.ToCsv());
            }

            if (File.Exists(fileRoute))
            {
                File.WriteAllLines(fileRoute, way, Encoding.UTF8);
                ReadFiles();
            }
        }


        public void Insert(Donation donation)
        {
            var count = _donations.Any() ? _donations.Max(x => x.Id) + 1 : 1;
            donation.Id = count;
            _donations.Add(donation);
            Save();

        }

        public void Update(Donation donation)
        {
            var result = _donations.FirstOrDefault(x => x.Id == donation.Id);
            if (result != null)
            {
                _donations.Remove(result);
                _donations.Add(donation);
                Save();
            }
        }

        public void Delete(Donation donation)
        {
            var result = GetForId(donation.Id);

            if (result != null)
            {
                _donations.Remove(result);
                Save();
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
