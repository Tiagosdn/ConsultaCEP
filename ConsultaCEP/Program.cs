using Correios;
using Correios.CorreiosServiceReference;
using System;
using System.Collections.Generic;
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

            if (string.IsNullOrEmpty(cep))
            {
                Console.WriteLine("Por favor, informe o CEP desejado!");
                Console.Read();
                return;
            }

            ConsultaCEPTiago(cep);
            


            Console.Read();
        }

        private static object ConsultaCEPTiago(string cep)
        {
            HttpClient client = new HttpClient();
            //chamando a api pela url
            HttpResponseMessage response = client.GetAsync(string.Format("{0}{1}", urlApiCep, cep)).Result;

            //se retornar com sucesso busca os dados
            if (response.IsSuccessStatusCode)
            {
                //Pegando os dados do Rest e armazenando na variável usuários
                var retorno = response.Content.ReadAsStringAsync().Result;

                Console.WriteLine(retorno);
            }
            else
            {
                Console.WriteLine("CEP não encontrado!");
            }
            return null;
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
    }
}
