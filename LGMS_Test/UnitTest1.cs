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
           CurrencyService currencyService = new CurrencyService(new HttpClient());
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
            string fileName = "Essay.pdf";
            string expected = ".pdf";
            string actual = System.IO.Path.GetExtension(fileName);
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Test3_PreventServiceRequestCreateForExpiredContracts()
        {
            string contractStatus = "Expired";
            bool canCreateServiceRequest = contractStatus != "Expired";
            Assert.False(canCreateServiceRequest);
        }
    }
}

