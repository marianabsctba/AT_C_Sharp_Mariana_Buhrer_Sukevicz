using Domain;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly IDonationRepository _repository;
        private const string pressButtons = "Pressione qualquer tecla para exibir o menu principal ...";

        public Worker(IDonationRepository repository)
        {
            _repository = repository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("pt-BR");

            string option;

            
            do
            {
                Console.WriteLine("");
                ShowMenu();

                option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        AddDonation();
                        break;
                    case "2":
                        SearchDonation();
                        break;
                    case "3":
                        UpdateDonation();
                        break;
                    case "4":
                        DeleteDonation();
                        break;
                    case "5":
                        ListDonation();
                        break;
                    case "6":
                        Console.WriteLine("\nSair...Tem certeza que você quer sair do aplicativo? (digite 'sim' ou 'não'):");
                        var answer = Console.ReadLine().ToLower();

                        if (Regex.IsMatch(answer, "sim", RegexOptions.IgnoreCase))
                        {
                            Environment.Exit(0);
                        }
                        break;
                    default:
                        Console.Write("\nOpcao inválida! Escolha uma opção válida. ");
                        break;
                }
                Console.WriteLine(pressButtons);
                Console.ReadKey();
            }
            while (option != "6");
        }

        void ListDonation()
        {
            var resultList = _repository.GetDonations();

            if (resultList.Any())
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("==== Lista de doações ==");
                Console.ResetColor();
                foreach (var donation in resultList)
                {
                    Console.WriteLine($"\n{donation.GetResumeData()} - Dias de registro: {donation.GetTotalDays()}");
                }
            }
            else
            {
                Console.WriteLine("===================================");
                Console.WriteLine("\nNão foi encontrada nenhuma doação!");
            }
        }

        void ShowFiveRegister()
        {
            var resultList = _repository.GetFiveLast().ToList();

            if (resultList.Any())
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("==== Últimas doações registradas====");
                Console.ResetColor();
                foreach (var donation in resultList)
                {
                    Console.WriteLine($"{donation.GetResumeData()}");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("===================================");
                Console.WriteLine("\nNão foi encontrada nenhuma doação!");
                Console.WriteLine("===================================");
                Console.ResetColor();
            }
        }
        void SearchDonation()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("==== Pesquisar doação ==== ");
            Console.ResetColor();
            Console.WriteLine("\n== Digite o termo de busca: ");
            var partialName = Console.ReadLine();

            var resultList = _repository.GetName(partialName);

            if (!resultList.Any())
            {
                Console.WriteLine("==========");
                Console.WriteLine("\nNenhum registro encontrado!");
                return;
            }

            Console.Write("==========");
            Console.WriteLine("\nResultado da busca: ");

            foreach (var item in resultList)
            {
                Console.WriteLine($"\t {item}");
            }
        }

        void AddDonation()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("===== Adicionar doação ====");
            Console.ResetColor();
            Console.WriteLine("\t==== Digite: ====");

            Console.WriteLine("[ id ]: ");
            var id = int.Parse(Console.ReadLine());

            Console.WriteLine("[ Tipo ] digite 'D' (doacao) ou  'T' para troca':");
            var gender = Console.ReadLine().ToUpper();
            var boolGender = gender == "D" ? true : false;

            Console.WriteLine("[ Nome ]: ");
            var name = Console.ReadLine();

            Console.WriteLine("[ Breve descrição ]: ");
            var description = Console.ReadLine();

            Console.Write("[ Quantidade ]:");
            var quantity = int.Parse(Console.ReadLine());

            Console.Write("[ Frete ]: ");
            var courier = double.Parse(Console.ReadLine());

            Console.WriteLine("[ Estado ] A doação é nova ou usada ( digite 'nova' ou 'usada'):");
            var state = Console.ReadLine().ToLower();
            var boolStatus = state == "nova" ? true : false;

            Console.WriteLine("[ Data de Registro ] Formato dd/MM/aaaa: ");
            string registerDate = Console.ReadLine();

            DateTime date;

            if (DateTime.TryParseExact(registerDate, "dd/MM/yyyy", null, DateTimeStyles.None, out date))
            {
                var donation = new Donation(id, boolStatus, boolGender, name, description, quantity, courier, date);
                _repository.Insert(donation);

                Console.WriteLine("=====================");
                Console.WriteLine("\nRegistro adicionado com sucesso!");
            }
        }
    
        void UpdateDonation()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("==== Editar Doação ====");
            Console.ResetColor();
            Console.WriteLine("\tPrimeiro busque a doação a ser editada: ");
            var partialName = Console.ReadLine();

            var resultList = _repository.GetName(partialName);

            if (!resultList.Any())
            {
                Console.WriteLine("=====================");
                Console.WriteLine("\nResultado da busca: ");
                Console.WriteLine("\nNenhum resultado encontrado");
                return;
            }

            Console.WriteLine("=====================");
            Console.WriteLine("\nResultado da busca: ");

            foreach (var item in resultList)
            {
                Console.WriteLine($"{item.GetResumeData()}");
            }

            Console.WriteLine("\n[ id para alteração ]:");
            int.TryParse(Console.ReadLine(), out int id);

            var resultDonation = resultList.FirstOrDefault(p => p.Id == id);

            if (resultDonation == null)
            {
                Console.WriteLine("=====================");
                Console.WriteLine("\nNenhum resultado encontrado");
                return;
            }

            Console.WriteLine("\n[Tipo para alteração] ( digite 'D' (doacao) ou  'T' para troca'):");
            var gender = Console.ReadLine().ToUpper();
            var boolGender = gender == "D" ? true : false;

            Console.WriteLine("\n[Nome para alteração]: ");
            var name = Console.ReadLine();

            Console.WriteLine("\n[Descrição para alteração]: ");
            var description = Console.ReadLine();

            Console.Write("\n[Quantidade para alteração]: ");
            var quantity = int.Parse(Console.ReadLine());

            Console.Write("\n[Frete para alteração]: ");
            var courier = double.Parse(Console.ReadLine());

            Console.WriteLine("\n[Estado para alteração] ( digite 'nova' ou 'usada'):");
            var state = Console.ReadLine();
            var boolState = state == "nova" ? true : false;

            Console.WriteLine("\n[Data de registro para alteração] formato dd/MM/aaaa:");
            string registerDate = Console.ReadLine();

            if (DateTime.TryParseExact(registerDate, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime date))
            {
                resultDonation.BoolGender = boolGender;
                resultDonation.Name = name;
                resultDonation.Description = description;
                resultDonation.Quantity = quantity;
                resultDonation.Courier = courier;
                resultDonation.BoolStatus = boolState;
                resultDonation.RegisterDate = date;
                _repository.Update(resultDonation);

                Console.WriteLine("=====================");
                Console.WriteLine("\nRegistro alterado com sucesso!");
            }

        }
        void DeleteDonation()
        {

            var resultList = _repository.GetDonations();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("====Excluir doação====");
            Console.ResetColor();
            Console.WriteLine("\n[Id do registro que deseja excluir]: ");

            int.TryParse(Console.ReadLine(), out int id);

            var resultDonation = resultList.FirstOrDefault(p => p.Id == id);

            if (resultDonation == null)
            {
                Console.WriteLine("===========================");
                Console.WriteLine("\nNenhum resultado encontrado");
                return;
            }

            Console.WriteLine($"\nTem certeza que deseja excluir o registro abaixo?(Digite [sim] ou [não])\n{resultDonation}");

            var answer = Console.ReadLine()?.ToLower();

            if (Regex.IsMatch(answer, "sim", RegexOptions.IgnoreCase))
            {

                _repository.Delete(resultDonation);
                Console.WriteLine("===================");
                Console.WriteLine("\nRegistro excluído com sucesso!");
            }
        }
        void ShowMenu()
        {
            Console.Clear();
            ShowFiveRegister();
            Console.Title = "AT - Mariana Bührer Sukevicz";
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("======================================");
            Console.WriteLine("--- Gerenciador de Doações e Trocas---");
            Console.WriteLine("======================================");
            Console.ResetColor();
            Console.WriteLine("[ 1 ] Adicionar doações");
            Console.WriteLine("[ 2 ] Pesquisar e Detalhar doações");
            Console.WriteLine("[ 3 ] Editar doações");
            Console.WriteLine("[ 4 ] Excluir doações");
            Console.WriteLine("[ 5 ] Listar doações");
            Console.WriteLine("[ 6 ] Sair");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("--------------------------------------");
            Console.ResetColor();
            Console.WriteLine("\nEscolha uma das opções acima: ");
        }

    }
}


