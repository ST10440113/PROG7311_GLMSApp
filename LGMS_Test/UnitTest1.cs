using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using PROG7311_GLMSApp.Controllers;
using PROG7311_GLMSApp.Models;
using PROG7311_GLMSApp.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace LGMS_Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1_USD_to_ZAR_Conversion()
        {
            //Arrange
            CurrencyService currencyService = new CurrencyService(null);
            double amount = 100;
            double exchangeRate = 16.36;
            double expected = 1636;

            // Act
            double actual = currencyService.ConvertToZar(amount, exchangeRate);
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test2_FileTypeValidation()
        {
            //Arrange
            ContractService contractService = new ContractService(null, null, null);
            IFormFile file = new FormFile(null, 0, 0, null, "agreement.exe");
            bool isFileValid = false;

            //Act
            try
            {
                contractService.CheckFileExtension(file);
            }
            catch (ArgumentException ex)
            {
                isFileValid = ex.Message.Contains("Invalid file type. Only PDF files are allowed.");
            }

            //Assert
            Assert.True(isFileValid);
        }


        [Fact]
        public void Test3_PreventServiceRequestCreateForExpiredContracts()
        {
            // Arrange
            ContractContext contractContext = new ContractContext();
            // Act
            contractContext.SetState(new ConcreteContract.Expired());
            bool canCreateServiceRequest = contractContext.ChangeState("Expired");
            // Assert
            Assert.False(canCreateServiceRequest);
        }

        

        private ClientService GetClientService()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7256/")
            };
            return new ClientService(httpClient);
        }


        
        [Fact]
        public async Task Test4_CreateClientAndVerifyExistance()
        {
            // Arrange
            ClientService clientService = GetClientService();
            Client newClient = new Client
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@email.com",
                PhoneNumber = "1234567890",
                Region = "Africa"

            };

            // Act
            var createdClient = await clientService.CreateAsync(newClient);
            var fetchedClient = await clientService.GetClientByIdAsync(createdClient.ClientId);
            

            // Assert
            Assert.NotNull(fetchedClient);
            Assert.Equal(newClient.FirstName, fetchedClient.FirstName);
            Assert.Equal(newClient.LastName, fetchedClient.LastName);
            Assert.Equal(newClient.Email, fetchedClient.Email);
            Assert.Equal(newClient.PhoneNumber, fetchedClient.PhoneNumber);
            Assert.Equal(newClient.Region, fetchedClient.Region);
        }


        [Fact]
        public async Task Test5_GetAllClients()
        {
            //Arrange
            ClientService clientService = GetClientService();

            // Act
            var allClients = await clientService.GetAllClientsAsync();

            // Assert
            Assert.NotNull(allClients);
            Assert.IsType<List<Client>>(allClients);
        }

      
        [Fact]
        public async Task Test6_DeleteClient()
        {
            // Arrange
            ClientService clientService = GetClientService();
            Client clientToDelete = new Client
            {   
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@email.com",
                PhoneNumber = "1234567890",
                Region = "Africa"
            };
            var createdClientToDelete = await clientService.CreateAsync(clientToDelete);

            // Act
            bool deleteResult = await clientService.Delete(createdClientToDelete);
            var deletedClient = await clientService.GetClientByIdAsync(createdClientToDelete.ClientId);
            // Assert
            Assert.True(deleteResult);
            Assert.Null(deletedClient);
        }
    }
}


