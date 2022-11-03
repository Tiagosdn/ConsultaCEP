using Correios;
using Correios.CorreiosServiceReference;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace ConsultaCEP
{
    public class Program
    {
        private static string urlApiCep = "https://cep.awesomeapi.com.br/json/";
        static void Main(string[] args)
        {
            Console.WriteLine("Digite o CEP desejado: ");
            string cep = Console.ReadLine();

            while (string.IsNullOrEmpty(cep))
            {
                Console.WriteLine("\n");
                Console.WriteLine("Por favor, informe novamente o CEP desejado:");
                cep = Console.ReadLine();
            }

            while (cep.Length != 8)
            {
                Console.WriteLine("\n");
                Console.WriteLine("Por favor, informe um CEP valido:");
                cep = Console.ReadLine();
            }

            ConsultaCEP(cep);
            Console.Read();
        }

        private static void ConsultaCEP(string cep)
        {
            HttpClient client = new HttpClient();
            //chamando a api pela url
            HttpResponseMessage response = client.GetAsync(string.Format("{0}{1}", urlApiCep, cep)).Result;

            //se retornar com sucesso busca os dados
            if (HttpStatusCode.OK == response.StatusCode)
            {
                Console.WriteLine("\n");
                //Pegando os dados do Rest e armazenando na variável usuários
                string retorno = response.Content.ReadAsStringAsync().Result;

                EnderecoAPI endAPI = JsonConvert.DeserializeObject<EnderecoAPI>(retorno);

                if (endAPI == null)
                {
                    Console.WriteLine("Problemas ao converter o valor retornado:\n");
                    return;
                }
                
                Console.WriteLine("O endereço encontrado para o CEP {0} foi:\n", cep);
                Console.WriteLine("Logradouro: {0}\nBairro: {1}\nCidade: {2}\nEstado: {3}\nDDD: {4}\n", endAPI.address, endAPI.district, endAPI.city, endAPI.state, endAPI.ddd);
                Console.WriteLine("Deseja pesquisar um novo CEP? (digite S para Sim e N para Não)");
                string pesquisarNovamente = Console.ReadLine();
                while (string.IsNullOrEmpty(pesquisarNovamente))
                {
                    Console.WriteLine("Deseja pesquisar um novo CEP? (digite S para Sim e N para Não)");
                    pesquisarNovamente = Console.ReadLine();
                }

                while (pesquisarNovamente.ToLower() != "s" && pesquisarNovamente.ToLower() != "n")
                {
                    Console.WriteLine("Deseja pesquisar um novo CEP? (digite S para Sim e N para Não)");
                    pesquisarNovamente = Console.ReadLine();
                }

                if (pesquisarNovamente.ToLower() == "s")
                {
                    Console.WriteLine("Digite o CEP desejado: ");
                    string newCep = Console.ReadLine();

                    while (string.IsNullOrEmpty(newCep))
                    {
                        Console.WriteLine("\n");
                        Console.WriteLine("Por favor, informe novamente o CEP desejado:");
                        newCep = Console.ReadLine();
                    }

                    while (newCep.Length != 8)
                    {
                        Console.WriteLine("\n");
                        Console.WriteLine("Por favor, informe um CEP valido:");
                        newCep = Console.ReadLine();
                    }

                    ConsultaCEP(newCep);
                }
            }
            else if (HttpStatusCode.BadRequest == response.StatusCode)
            {
                Console.WriteLine("\n");
                Console.WriteLine("O CEP {0} informado é invalido. Por favor, informe um CEP valido:", cep);
            }
            else if (HttpStatusCode.NotFound == response.StatusCode)
            {
                Console.WriteLine("\n");
                Console.WriteLine("O CEP {0} informado não foi encontrado.", cep);
            }
            else
            {
                Console.WriteLine("\n");
                Console.WriteLine("Problemas ao procurar o CEP {0}", cep);
            }
        } 

        private static void ConsultaCEPNuget()
        {
            CorreiosApi correiosApi = new CorreiosApi();
            string cep = Console.ReadLine();

            if (cep.Length == 8)
            {
                enderecoERP retorno = correiosApi.consultaCEP(cep);


                var cepRet = retorno.cep;
                var bairro = retorno.bairro;
                var cidade = retorno.cidade;

                Console.WriteLine("CEP:" + cepRet + "\r\nBairro:" + bairro + "\r\nCidade:" + cidade);
                Console.ReadKey();
            }
            else
            {
                //Console.WriteLine("CEP INVALIDO");

                Console.ReadKey();
            }


            //else if (cep == "000")
            //{
            //    Console.WriteLine("CEP INESISTENTE");
            //}
        }

        private class EnderecoAPI
        {
            public string cep { get; set; }
            public string address_type { get; set; }
            public string address_name { get; set; }
            public string address { get; set; }
            public string state { get; set; }
            public string district { get; set; }
            public string lat { get; set; }
            public string lng { get; set; }
            public string city { get; set; }
            public string city_ibge { get; set; }
            public string ddd { get; set; }
        }
    }
}
