using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace Oficina
{
    internal class Program
    {
        public class Clientes
        {
            public int ID { get; set; }
            public string Nome { get; set; }
            public string Email { get; set; }
            public string Telemovel { get; set; }
        }

        public class Automoveis
        {
            public int IDAuto { get; set; }
            public int IDCliente { get; set; }
            public string Marca { get; set; }
            public string Modelo { get; set; }
            public int Ano { get; set; }
            public string num_chassi { get; set; }
        }

        static string FichaClientes = "clientes.csv";
        static string FichaAutomoveis = "automoveis.csv";
        static int id = 0;
        static int idAutomovel = 0;

        public static void Digite()
        {
            Console.WriteLine("Digite qualquer coisa para avançar...");
            Console.ReadKey();
            Console.Clear();
        }

        static void InicializarFicheiros()
        {
            if (!File.Exists(FichaClientes))
            {
                File.Create(FichaClientes).Close();
                Console.WriteLine("O ficheiro 'clientes.csv' não existia e foi criado automaticamente!");
            }

            if (!File.Exists(FichaAutomoveis))
            {
                File.Create(FichaAutomoveis).Close();
                Console.WriteLine("O ficheiro 'automoveis.csv' não existia e foi criado automaticamente!");
            }

            if (File.Exists(FichaClientes))
            {
                using (StreamReader sr = new StreamReader(FichaClientes))
                {
                    while (!sr.EndOfStream)
                    {
                        string linha = sr.ReadLine();
                        if (!string.IsNullOrWhiteSpace(linha))
                        {
                            id++;
                        }
                    }
                }
            }

            if (File.Exists(FichaAutomoveis))
            {
                using (StreamReader sr = new StreamReader(FichaAutomoveis))
                {
                    while (!sr.EndOfStream)
                    {
                        string linha = sr.ReadLine();
                        if (!string.IsNullOrWhiteSpace(linha))
                        {
                            idAutomovel++;
                        }
                    }
                }
            }
        }

        static void InserirNovoCliente()
        {
            Clientes NovoCliente = new Clientes();
            string telemovel;
            id++;

            NovoCliente.ID = id;
            Console.WriteLine("======= Novo Cliente =======");
            Console.Write("\nDigite o Nome: ");
            NovoCliente.Nome = Console.ReadLine();
            Console.Write("\nDigite o Email: ");
            NovoCliente.Email = Console.ReadLine();
            Console.Write("\nDigite o Telemóvel: ");
            telemovel = Console.ReadLine();

            while (true)
            {
                if (telemovel.Length == 9 && long.TryParse(telemovel, out _))
                {
                    NovoCliente.Telemovel = telemovel;
                    break;
                }
                Console.WriteLine("O número de telemóvel deve ter 9 dígitos (todos números)");
                Console.Write("\nDigite novamente o número de telemóvel: ");
                telemovel = Console.ReadLine();
            }

            using (StreamWriter sw = new StreamWriter(FichaClientes, true))
            {
                sw.WriteLine($"{NovoCliente.ID}   {NovoCliente.Nome}   {NovoCliente.Email}   {NovoCliente.Telemovel}");
            }
            Console.Clear();

            Console.WriteLine("======= Dados do Cliente =======");
            Console.WriteLine($"ID: {NovoCliente.ID}\nNome: {NovoCliente.Nome}\nEmail: {NovoCliente.Email}\nTelemóvel: {NovoCliente.Telemovel}");
            Digite();
        }

        static void VerClientes()
        {
            Console.WriteLine("======= Mostrar Dados dos Clientes =======");
            if (id != 0)
            {
                using (StreamReader sr = new StreamReader(FichaClientes))
                {
                    while (!sr.EndOfStream)
                    {
                        string linha = sr.ReadLine();
                        Console.WriteLine("Lido do ficheiro: " + linha);
                    }
                }
            }
            else
            {
                Console.WriteLine("Não existe nenhum cliente ainda");
            }
            Digite();
        }

        static void MostrarClientePorID()
        {
            Console.WriteLine("======= Mostrar Dados de um Cliente (Por ID) =======");
            Console.Write("Digite o ID do cliente que procura: ");
            int idProcurado;

            while (!int.TryParse(Console.ReadLine(), out idProcurado))
            {
                Console.Write("Por favor, digite um ID válido (número inteiro): ");
            }

            bool clienteEncontrado = false;

            using (StreamReader sr = new StreamReader(FichaClientes))
            {
                while (!sr.EndOfStream)
                {
                    string linha = sr.ReadLine();

                    if (!string.IsNullOrWhiteSpace(linha))
                    {
                        string[] partes = linha.Split(';');

                        if (partes.Length >= 4 && int.TryParse(partes[0], out int idLinha))
                        {
                            if (idLinha == idProcurado)
                            {
                                Console.Clear();
                                Console.WriteLine("======= Cliente Encontrado =======");
                                Console.WriteLine($"ID: {partes[0]}");
                                Console.WriteLine($"Nome: {partes[1]}");
                                Console.WriteLine($"Email: {partes[2]}");
                                Console.WriteLine($"Telemóvel: {partes[3]}");

                                clienteEncontrado = true;
                                break;
                            }
                        }
                    }
                }
            }

            if (!clienteEncontrado)
            {
                Console.WriteLine($"\nNão foi encontrado nenhum cliente com o ID {idProcurado}.");
            }
            Digite();
        }

        static void MenuClientes()
        {
            int opcao;
            Console.WriteLine("======= Menu Clientes =======");
            Console.Write("\nDigite\n [1] - Inserir Novo Cliente\n [2] - Ver Clientes\n [3] - Mostrar Dados de um Cliente (Por ID)\n [0] - Voltar ao Menu Principal \nOpção: ");
            do
            {
                while (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    Console.WriteLine("Digite apenas números inteiros");
                }
                if (opcao > 3 || opcao < 0)
                {
                    Console.WriteLine("Digite apenas números entre [0-3]");
                }
            } while (opcao > 3 || opcao < 0);
            Console.Clear();

            switch (opcao)
            {
                case 1:
                    InserirNovoCliente();
                    break;
                case 2:
                    VerClientes();
                    break;
                case 3:
                    MostrarClientePorID();
                    break;
            }
        }

        static void InserirNovoAutomovel()
        {
            idAutomovel++;
            string nomeCliente;
            int idcliente, ano;
            string num_chassi;
            Automoveis NovoAutomovel = new Automoveis();
            Console.WriteLine("===== Novo automóvel =====");
            NovoAutomovel.IDAuto = idAutomovel;
            Console.WriteLine("Qual o nome do cliente: ");
            nomeCliente = Console.ReadLine();

            string[] procura = new string[id];
            int contador = 0;
            using (StreamReader sr = new StreamReader(FichaClientes))
            {
                while (!sr.EndOfStream)
                {
                    procura[contador] = sr.ReadLine();
                    contador++;
                }
            }

            foreach (string item in procura)
            {
                if (item != null && item.ToLower().Contains(nomeCliente.ToLower()))
                {
                    Console.WriteLine($"{item}");
                }
            }

            Console.WriteLine("Qual o ID do dono: ");
            do
            {
                while (!int.TryParse(Console.ReadLine(), out idcliente))
                {
                    Console.WriteLine("Digite apenas números inteiros");
                }
                if (idcliente > id || idcliente < 0)
                {
                    Console.WriteLine($"Digite apenas números entre [1-{id}]");
                }
            } while (idcliente > id || idcliente < 0);

            NovoAutomovel.IDCliente = idcliente;

            Console.WriteLine("Digite a marca do veículo: ");
            NovoAutomovel.Marca = Console.ReadLine();
            Console.WriteLine("Digite o modelo do veículo: ");
            NovoAutomovel.Modelo = Console.ReadLine();
            Console.WriteLine("Digite o ano do veículo: ");
            do
            {
                while (!int.TryParse(Console.ReadLine(), out ano))
                {
                    Console.WriteLine("Digite apenas números inteiros");
                }
                if (ano > 2026 || ano < 1885)
                {
                    Console.WriteLine($"Digite apenas números entre [1885-2026]");
                }
            } while (ano > 2026 || ano < 1885);
            NovoAutomovel.Ano = ano;
            Console.WriteLine("Digite o número do chassi do veículo: ");
            num_chassi = Console.ReadLine();
            while (num_chassi.Length != 5)
            {
                Console.WriteLine("Digite apenas 5 dígitos");
                num_chassi = Console.ReadLine();
            }
            NovoAutomovel.num_chassi = num_chassi;

            using (StreamWriter sw = new StreamWriter(FichaAutomoveis, true))
            {
                sw.WriteLine($"{NovoAutomovel.IDAuto};{NovoAutomovel.IDCliente};{NovoAutomovel.Marca};{NovoAutomovel.Modelo};{NovoAutomovel.Ano};{NovoAutomovel.num_chassi}");
            }

            Digite();
        }

        static void VerTodosAutomoveis()
        {
            Console.WriteLine("======= Mostrar Dados dos Automóveis =======");
            if (idAutomovel != 0)
            {
                using (StreamReader sr = new StreamReader(FichaAutomoveis))
                {
                    while (!sr.EndOfStream)
                    {
                        string linha = sr.ReadLine();
                        Console.WriteLine("Lido do ficheiro: " + linha);
                    }
                }
            }
            else
            {
                Console.WriteLine("Não existe nenhum automóvel ainda");
            }

            Digite();
        }

        static void MostrarAutomovelPorID()
        {
            Console.WriteLine("======= Mostrar Dados de um Automóvel (Por ID) =======");
            Console.Write("Digite o ID do automóvel que procura: ");
            int idAutoProcurado;

            while (!int.TryParse(Console.ReadLine(), out idAutoProcurado))
            {
                Console.Write("Por favor, digite um ID válido (número inteiro): ");
            }

            bool automovelEncontrado = false;

            using (StreamReader sr = new StreamReader(FichaAutomoveis))
            {
                while (!sr.EndOfStream)
                {
                    string linha = sr.ReadLine();

                    if (!string.IsNullOrWhiteSpace(linha))
                    {
                        string[] partes = linha.Split(';');

                        if (partes.Length >= 6 && int.TryParse(partes[0], out int idLinha))
                        {
                            if (idLinha == idAutoProcurado)
                            {
                                Console.Clear();
                                Console.WriteLine("======= Automóvel Encontrado =======");
                                Console.WriteLine($"ID Automóvel: {partes[0]}");
                                Console.WriteLine($"ID Cliente:    {partes[1]}");
                                Console.WriteLine($"Marca:         {partes[2]}");
                                Console.WriteLine($"Modelo:        {partes[3]}");
                                Console.WriteLine($"Ano:           {partes[4]}");
                                Console.WriteLine($"Número do chassi: {partes[5]}");

                                automovelEncontrado = true;
                                break;
                            }
                        }
                    }
                }
            }

            if (!automovelEncontrado)
            {
                Console.WriteLine($"\nNão foi encontrado nenhum automóvel com o ID {idAutoProcurado}.");
            }

            Digite();
        }

        static void MenuAutomoveis()
        {
            int opcao;
            Console.WriteLine("======= Menu Automóveis =======");
            Console.Write("\nDigite\n [1] - Inserir Novo Automóvel\n [2] - Ver Todos os Automóveis\n [3] - Mostrar Dados de um Automóvel (por ID)\n [0] - Voltar ao Menu Principal \nOpção: ");
            do
            {
                while (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    Console.WriteLine("Digite apenas números inteiros");
                }
                if (opcao > 3 || opcao < 0)
                {
                    Console.WriteLine("Digite apenas números entre [0-3]");
                }
            } while (opcao > 3 || opcao < 0);
            Console.Clear();
            switch (opcao)
            {
                case 1:
                    InserirNovoAutomovel();
                    break;
                case 2:
                    VerTodosAutomoveis();
                    break;
                case 3:
                    MostrarAutomovelPorID();
                    break;
            }
        }

        static void PesquisarAutomovelCliente()
        {
            string nomeCliente;
            string[] procura = new string[id];
            int contador = 0, idcliente;
            Console.WriteLine("======= Pesquisar Automóvel de um Cliente =======");
            Console.WriteLine("Qual o nome do cliente: ");
            nomeCliente = Console.ReadLine();

            using (StreamReader sr = new StreamReader(FichaClientes))
            {
                while (!sr.EndOfStream)
                {
                    procura[contador] = sr.ReadLine();
                    contador++;
                }
            }

            foreach (string item in procura)
            {
                if (item != null && item.ToLower().Contains(nomeCliente.ToLower()))
                {
                    Console.WriteLine($"{item}");
                }
            }
            Console.WriteLine("Qual o ID do dono: ");
            do
            {
                while (!int.TryParse(Console.ReadLine(), out idcliente))
                {
                    Console.WriteLine("Digite apenas números inteiros");
                }
                if (idcliente > id || idcliente < 0)
                {
                    Console.WriteLine($"Digite apenas números entre [1-{id}]");
                }
            } while (idcliente > id || idcliente < 0);
            Console.Clear();
            Console.WriteLine($"======= Automóveis do Cliente (ID: {idcliente}) =======");
            bool encontrouCarro = false;

            using (StreamReader sr = new StreamReader(FichaAutomoveis))
            {
                while (!sr.EndOfStream)
                {
                    string linha = sr.ReadLine();
                    if (!string.IsNullOrWhiteSpace(linha))
                    {
                        string[] partes = linha.Split(';');

                        if (partes.Length >= 6 && int.TryParse(partes[1], out int idClienteFicheiro))
                        {
                            if (idClienteFicheiro == idcliente)
                            {
                                Console.WriteLine("---------------------------------------------");
                                Console.WriteLine($"ID Automóvel: {partes[0]}");
                                Console.WriteLine($"Marca:        {partes[2]}");
                                Console.WriteLine($"Modelo:       {partes[3]}");
                                Console.WriteLine($"Ano:          {partes[4]}");
                                Console.WriteLine($"Chassi:       {partes[5]}");
                                encontrouCarro = true;
                            }
                        }
                    }
                }
            }

            if (!encontrouCarro)
            {
                Console.WriteLine("Este cliente não tem nenhum automóvel registado.");
            }
            else
            {
                Console.WriteLine("---------------------------------------------");
            }
            Digite();
        }

        static void MenuPesquisas()
        {
            int opcao;
            Console.WriteLine("======= Pesquisas =======");
            Console.Write("\nDigite \n [1] - Listar Automóvel de um Cliente\n [0] - Voltar ao Menu Principal \nOpção: ");
            do
            {
                while (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    Console.WriteLine("Digite apenas números inteiros");
                }
                if (opcao > 1 || opcao < 0)
                {
                    Console.WriteLine("Digite apenas números entre [0-1]");
                }
            } while (opcao > 1 || opcao < 0);

            switch (opcao)
            {
                case 1:
                    PesquisarAutomovelCliente();
                    break;
            }
        }

        static void Main(string[] args)
        {
            int Pescolha;
            List<Clientes> ListaClientes = new List<Clientes>();
            List<Clientes> ListaAutomoveis = new List<Clientes>();

            InicializarFicheiros();

            do
            {
                Console.Clear();
                Console.WriteLine("======= Oficina =======");
                Console.Write("\nDigite\n [1] - Menu clientes\n [2] - Menu automóveis\n [3] - Pesquisas \n [0] - Sair \nOpção: ");
                do
                {
                    while (!int.TryParse(Console.ReadLine(), out Pescolha))
                    {
                        Console.WriteLine("Digite apenas números inteiros");
                    }
                    if (Pescolha > 3 || Pescolha < 0)
                    {
                        Console.WriteLine("Digite apenas números entre [0-3]");
                    }
                } while (Pescolha > 3 || Pescolha < 0);
                Console.Clear();

                switch (Pescolha)
                {
                    case 1:
                        MenuClientes();
                        break;

                    case 2:
                        MenuAutomoveis();
                        break;

                    case 3:
                        MenuPesquisas();
                        break;
                }
            } while (Pescolha != 0);
        }
    }
}