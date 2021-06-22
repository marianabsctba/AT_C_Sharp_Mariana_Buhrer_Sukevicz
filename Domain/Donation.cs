using System;

namespace Domain
{
    public class Donation
    {
        public int Id { get; set; }
        public bool BoolStatus { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public double Courier { get; set; }
        public bool BoolGender { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime Today { get; set; }

        public Donation(int id, 
            bool boolStatus, bool boolGender, 
            string name, string description, int quantity,
            double courier, DateTime registerDate)
        {

            Id = id;
            Name = name ?? throw new ArgumentException("O nome da doação não foi preenchida");
            Description = description ?? throw new ArgumentException("Descrição não foi preenchida");
            RegisterDate = registerDate;
            Quantity = quantity;
            Courier = courier;
            BoolGender = boolGender;
            BoolStatus = boolStatus;
            Today = DateTime.Now.Date;
        }

        public int GetTotalDays()
        {
            var today = DateTime.Now.Date;
            var registryDay = RegisterDate.Date;
            var differenceOfDays = today - registryDay;
            var totalDaysInRegister = differenceOfDays.TotalDays;

            return (int)totalDaysInRegister;
        }

        public DateTime GetDataRegister()
        {
            return RegisterDate;
        }

        public string GetResumeData() => string.Format("Id: {0} Nome: {1} Data de Registro: {2}", Id, Name, RegisterDate.ToString("dd/MM/yyyy"));

        public override string ToString()
        {
            return $"[Id]:{Id} " +
                   $"[Nome]:{Name} " +
                   $"[Tipo [True/Doação][False/Troca]]:{BoolGender} " +
                   $"[Descrição]:{Description} " +
                   $"[Status [True/Novo][False/Usado]:{BoolStatus} " +
                   $"[Data de Registro]:{RegisterDate:dd/MM/yyyy}" +
                   $"[Quantidade]:{Quantity} " +
                   $"[Frete]:{Courier}";
        }

        public string ToCsv()
        {
            return $"{Id};" +
                   $"{Name};" +
                   $"{BoolGender};" +
                   $"{Description};" +
                   $"{BoolStatus};" +
                   $"{RegisterDate:dd/MM/yyyy};" +
                   $"{Quantity};" +
                   $"{Courier};";

        }
    }

}