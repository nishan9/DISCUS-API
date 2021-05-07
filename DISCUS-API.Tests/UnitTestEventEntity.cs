using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DISCUS_API.Controllers;
using DISCUS_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DISCUS_API.Tests
{
    public class UnitTestEventEntity
    {
        private DbContextOptions<DataContext> dbContextOptions = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase(databaseName: "TestDB").Options;
        private IOptions<Auth0Settings> auth0settings;
        private Mock<HttpMessageHandler> mockhttpclient;
        private EventEntityController mockController;
        private DataContext context;

        [OneTimeSetUp]
        public void Setup()
        {
            mockhttpclient = new Mock<HttpMessageHandler>();
            auth0settings = Options.Create<Auth0Settings>(new Auth0Settings { Auth0ManagmentKey = "API KEY" });
            context = new DataContext(dbContextOptions);
            context.AddRange(new List<EventEntity>
            { new EventEntity
                { Title = "Hack Sussex",
                  Id = 2,
                  IsApproved = true,
                  IsDISCUS = false,
                  DateTime = DateTime.Now,
                  FinishedDateTime = DateTime.Today.AddDays(1),
                  Description = "Lorem Ipsim",
                  Tags = "Chemistry",
                  Type = "fd",
                  URL = "www.hacksussex.com"
                }
            });
            context.AddRange(new List<EventEntity>
            { new EventEntity
                { Title = "Hack Sussex",
                  Id = 5,
                  IsApproved = false,
                  IsDISCUS = false,
                  DateTime = DateTime.Now,
                  FinishedDateTime = DateTime.Now,
                  Description = "Lorem Ipsim",
                  Tags = "Chemistry",
                  Type = "fd",
                  URL = "www.hacksussex.com"
                }
            });
            context.SaveChanges();
            mockController = new EventEntityController(new HttpClient(mockhttpclient.Object), auth0settings, context);
        }


        [Test, Order(9)]
        public async Task TestUpcomingEvents()
        {
            ObjectResult result = await mockController.GetUpcomingEvents() as ObjectResult;
            List<EventEntity> ActualResults = result.Value as List<EventEntity>;
            Assert.AreEqual(1, ActualResults.Count());
        }

        [Test, Order(2)]
        public async Task TestPastEvents()
        {
            ObjectResult result = await mockController.GetPastEvents() as ObjectResult;
            List<EventEntity> ActualResults = result.Value as List<EventEntity>;
            Assert.AreEqual(0, ActualResults.Count());
        }

        [Test, Order(3)]
        public async Task TestCountEvents()
        {
            ObjectResult result = await mockController.CountEvents() as ObjectResult;
            Assert.AreEqual(2, result.Value);
        }

        [Test, Order(4)]
        public async Task TestGetSpecificEvent()
        {
            ObjectResult result = await mockController.GetSpecificEvent(5) as ObjectResult;
            EventEntity ActualResults = result.Value as EventEntity;
            Assert.AreEqual(5, ActualResults.Id);
        }

        [Test]
        public async Task TestGetNonexistentEvent()
        {
            ObjectResult result = await mockController.GetSpecificEvent(1) as ObjectResult;
            Assert.AreEqual("No record found", result.Value);
        }

        [Test, Order(5)]
        public async Task TestUpdateEventEntity()
        {
            ObjectResult result = await mockController.UpdateEventEntity(new EventEntity
            {
                Title = "Hack Sussex 5",
                IsApproved = true,
                IsDISCUS = false,
                DateTime = DateTime.Now,
                FinishedDateTime = DateTime.Now,
                Description = "Lorem Ipsim",
                Tags = "Chemistry",
                Type = "fd",
                URL = "www.hacksussex.com"
            }, 5) as ObjectResult;
            EventEntity ActualResults = result.Value as EventEntity;
            Assert.AreEqual("Hack Sussex 5", ActualResults.Title);
        }

        [Test, Order(6)]
        public async Task TestUpdateInvalidEventEntity()
        {
            ObjectResult result = await mockController.UpdateEventEntity(new EventEntity
            {
                Title = "Hack Sussex 5",
                IsApproved = true,
                IsDISCUS = false,
                DateTime = DateTime.Now,
                FinishedDateTime = DateTime.Now,
                Description = "Lorem Ipsim",
                Tags = "Chemistry",
                Type = "fd",
                URL = "www.hacksussex.com"
            }, 200) as ObjectResult;
            EventEntity ActualResults = result.Value as EventEntity;
            Assert.AreEqual(null, ActualResults);
        }


        [Test, Order(7)]
        public async Task TestCreateEvent()
        {
            ObjectResult result = await mockController.CreateEventEntity(new EventEntity
            {
                Title = "Hack Sussex",
                Id = 18,
                IsApproved = false,
                IsDISCUS = false,
                DateTime = DateTime.Now,
                FinishedDateTime = DateTime.Now,
                Description = "Lorem Ipsim",
                Tags = "Chemistry",
                Type = "fd",
                URL = "www.hacksussex.com"
            }) as ObjectResult;
            EventEntity ActualResults = result.Value as EventEntity;
            Assert.AreEqual(18, ActualResults.Id);
        }

        [Test, Order(8)]
        public async Task TestGetEventToApprove()
        {
            ObjectResult result = await mockController.GetEventToApprove() as ObjectResult;
            List<EventEntity> ActualResults = result.Value as List<EventEntity>;
            Assert.AreEqual(2, ActualResults.Count);
        }

        [Test, Order(10)]
        public async Task TestDeleteEvent()
        {
            ObjectResult result = await mockController.CreateEventEntity(new EventEntity
            {
                Title = "Hack Sussex",
                Id = 900,
                IsApproved = false,
                IsDISCUS = false,
                DateTime = DateTime.Now,
                FinishedDateTime = DateTime.Now,
                Description = "Lorem Ipsim",
                Tags = "Chemistry",
                Type = "fd",
                URL = "www.hacksussex.com"
            }) as ObjectResult;

            EventEntity ActualResults = result.Value as EventEntity;
            Assert.AreEqual(900, ActualResults.Id);

            ObjectResult result2 = await mockController.DeleteEventEntity(900) as ObjectResult;
            EventEntity actualres = result2.Value as EventEntity; 
            Assert.AreEqual(900, actualres.Id);
        }



    }
}