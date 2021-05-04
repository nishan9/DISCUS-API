using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DISCUS_API.Controllers;
using DISCUS_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace DISCUS_API.Tests
{
    public class Tests
    {
        private DbContextOptions<DataContext> dbContextOptions = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase(databaseName: "TestDB").Options;
        private IOptions<Auth0Settings> auth0settings;
        private Mock<HttpMessageHandler> mockhttpclient;
        private EventEntityController mockController;
        private DataContext context;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}