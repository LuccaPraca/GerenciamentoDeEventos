using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using static GerenciadorEventosMongoDB.Program;

namespace GerenciadorEventosMongoDB
{

    class Program
    {
        static MongoClient cliente;
        static IMongoCollection<Evento> eventos;

        


        public class Evento
        {
            public ObjectId Id { get; set; }
            public string Nome { get; set; }
            public DateTime Data { get; set; }
            public string Local { get; set; }
            public string Descricao { get; set; }
        }
        public class GerenciadorDeEventos
        {
            private IMongoCollection<Evento> _eventos;




            static void Main(string[] args)
            {
                GerenciadorEventos _gerenciador = new GerenciadorEventos(eventos);
                cliente = new MongoClient("mongodb+srv://usuario:senha@cluster0.yu1abtv.mongodb.net/?retryWrites=true&w=majority");
                var database = cliente.GetDatabase("GerenciamentoEventos");
                eventos = database.GetCollection<Evento>("Eventos");

                while (true)
                {
                    Console.WriteLine(@"  ______               _            ");
                    Console.WriteLine(@" |  ____|             | |           ");
                    Console.WriteLine(@" | |____   _____ _ __ | |_ ___  ___ ");
                    Console.WriteLine(@" |  __\ \ / / _ \ '_ \| __/ _ \/ __|");
                    Console.WriteLine(@" | |___\ V /  __/ | | | || (_) \__ \");
                    Console.WriteLine(@" |______\_/ \___|_| |_|\__\___/|___/ by: Lucca Peixoto Praça");
                    Console.WriteLine("------ MENU ------");
                    Console.WriteLine("1 - Listar eventos");
                    Console.WriteLine("2 - Adicionar evento");
                    Console.WriteLine("3 - Atualizar evento");
                    Console.WriteLine("4 - Remover evento");
                    Console.WriteLine("5 - Sair");
                    Console.WriteLine("-------------------");

                    var escolha = Console.ReadLine();

                    switch (escolha)
                    {
                        case "1":
                            Console.Clear();
                            ListarEventos();
                            break;
                        case "2":
                            Console.Clear();
                            AdicionarEvento();
                            break;
                        case "3":
                            Console.Clear();
                            AtualizarEvento();
                            break;
                        case "4":
                            Console.Clear();
                            RemoverEvento();
                            break;
                        case "5":
                            return;
                        default:
                            Console.Clear();
                            Console.WriteLine("Opção inválida!");
                            break;
                    }

                    Console.WriteLine("Pressione qualquer tecla para continuar...");
                    Console.ReadKey();
                    Console.Clear();

                }
            }

            public static void ListarEventos()
            {
                var listaEventos = eventos.Find(_ => true).ToList();

                if (listaEventos.Count > 0)
                {
                    Console.WriteLine("Lista de Eventos:");
                    foreach (var evento in listaEventos)
                    {
                        Console.WriteLine("ID: " + evento.Id);
                        Console.WriteLine("Nome: " + evento.Nome);
                        Console.WriteLine("Data: " + evento.Data);
                        Console.WriteLine("Local: " + evento.Local);
                        Console.WriteLine("Descrição: " + evento.Descricao);
                        Console.WriteLine("--------------------------");
                    }
                }
                else
                {
                    Console.WriteLine("Não há eventos cadastrados.");
                }
            }
            public class GerenciadorEventos
            {
                private readonly IMongoCollection<Evento> _eventos;

                public GerenciadorEventos(IMongoCollection<Evento> eventos)
                {
                    _eventos = eventos;
                }


                //public List<Evento> ListarEventos()
                //{
                //    return _eventos.Find(e => true).ToList();
                //}

                public void AdicionarEvento(Evento evento)
                {
                    _eventos.InsertOne(evento);
                }

                public void AtualizarEvento(Evento evento)
                {
                    var filtro = Builders<Evento>.Filter.Eq(e => e.Id, evento.Id);
                    _eventos.ReplaceOne(filtro, evento);
                }

                public void RemoverEvento(ObjectId id)
                {
                    var filtro = Builders<Evento>.Filter.Eq(e => e.Id, id);
                    _eventos.DeleteOne(filtro);
                }

                public void ListarEventos()
                {
                    Console.WriteLine("------ LISTAR EVENTOS ------");

                    var eventos = _eventos.Find(e => true).ToList();

                    if (eventos.Count > 0)
                    {
                        foreach (var evento in eventos)
                        {
                            Console.WriteLine($"Nome: {evento.Nome}");
                            Console.WriteLine($"Data: {evento.Data.ToShortDateString()}");
                            Console.WriteLine($"Local: {evento.Local}");
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Não há eventos cadastrados!");
                    }
                }
            }


            static void AdicionarEvento()
                {
                    Console.WriteLine("------ ADICIONAR EVENTO ------");

                    Console.Write("Nome: ");
                    string nome = Console.ReadLine();

                    Console.Write("Data (DD/MM/AAAA): ");
                    DateTime data = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", null);

                    Console.Write("Local: ");
                    string local = Console.ReadLine();

                    Console.Write("Descrição: ");
                    string descricao = Console.ReadLine();

                    var evento = new Evento
                    {
                        Nome = nome,
                        Data = data,
                        Local = local,
                        Descricao = descricao
                    };

                    eventos.InsertOne(evento);

                    Console.WriteLine("Evento adicionado com sucesso!");
                }


            }
            static void AtualizarEvento()
            {
                Console.WriteLine("------ ATUALIZAR EVENTO ------");

                Console.Write("Informe o nome do evento a ser atualizado: ");
                string nome = Console.ReadLine();

                var filtro = Builders<Evento>.Filter.Eq(e => e.Nome, nome);
                var evento = eventos.Find(filtro).FirstOrDefault();

                if (evento != null)
                {
                    Console.Write("Novo nome (Deixe em branco para manter o mesmo): ");
                    string novoNome = Console.ReadLine();

                    Console.Write("Nova data (DD/MM/AAAA) (Deixe em branco para manter a mesma): ");
                    string novaDataStr = Console.ReadLine();
                    DateTime novaData;
                    if (!string.IsNullOrWhiteSpace(novaDataStr) && DateTime.TryParseExact(novaDataStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out novaData))
                    {
                        evento.Data = novaData;
                    }

                    Console.Write("Novo local (Deixe em branco para manter o mesmo): ");
                    string novoLocal = Console.ReadLine();

                    Console.Write("Nova descrição (Deixe em branco para manter a mesma): ");
                    string novaDescricao = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(novoNome))
                    {
                        evento.Nome = novoNome;
                    }

                    if (!string.IsNullOrWhiteSpace(novoLocal))
                    {
                        evento.Local = novoLocal;
                    }

                    if (!string.IsNullOrWhiteSpace(novaDescricao))
                    {
                        evento.Descricao = novaDescricao;
                    }

                    eventos.ReplaceOne(filtro, evento);

                    Console.WriteLine("Evento atualizado com sucesso!");
                }
                else
                {
                    Console.WriteLine("Evento não encontrado!");
                }
            }

            static void RemoverEvento()
            {
                Console.WriteLine("------ REMOVER EVENTO ------");

                Console.Write("Informe o nome do evento a ser removido: ");
                string nome = Console.ReadLine();

                var filtro = Builders<Evento>.Filter.Eq(e => e.Nome, nome);
                var resultado = eventos.DeleteOne(filtro);

                if (resultado.DeletedCount > 0)
                {
                    Console.WriteLine("Evento removido com sucesso!");
                }
                else
                {
                    Console.WriteLine("Evento não encontrado!");
                }
            }
        }

    }
