using ClinicServiceNamespace;
using Grpc.Net.Client;
using System;
using static ClinicServiceNamespace.ClinicService;

namespace ClinicClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Контекст незащищенного соединения
            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            using var channel = GrpcChannel.ForAddress("http://localhost:5001");
            ClinicServiceClient clinicServiceClient = new ClinicServiceClient(channel);

            var createClientResponse = clinicServiceClient.CreateClient(new CreateClientRequest
            {
                Document = "DOC34 555",
                FirstName = "Александр",
                Patronymic = "Валентинович",
                Surname = "Дымченко"
            });

            if (createClientResponse.ErrCode == 0)
            {
                Console.WriteLine($"Client #{createClientResponse.ClientId} created successfully");
            }
            else
            {
                Console.WriteLine($"Create client error.\nErrorCode: {createClientResponse.ErrCode}");
            }

            var getClientResponse = clinicServiceClient.GetClients(new GetClientsRequest());

            if (getClientResponse.ErrCode == 0)
            {
                Console.WriteLine("Clients\n==========\n");

                foreach(var client in getClientResponse.Clients)
                {
                    Console.WriteLine($"#{client.ClientId}  {client.Document}  {client.FirstName}  {client.Surname}");
                }
            }
            else
            {
                Console.WriteLine($"Get clients error.\nErrorCode: {getClientResponse.ErrCode}");
            }
            Console.ReadKey();
            
        }
    }
}