using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PROG7311_GLMSApp.Controllers;
using PROG7311_GLMSApp.Services;
using PROG7311_GLMSApp.Data;
using PROG7311_GLMSApp.Models;
using System.Collections.Generic;
using System;
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
            ContractService contractService = new ContractService(null,null,null);          
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
    }
}

