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
    public class TestsT
    {
        private IOptions<Auth0Settings> auth0settings;
        private IOptions<SendGridSettings> sendgridSettings;
        private Mock<HttpMessageHandler> mockhttpclient;
        private UserSearchController mockController;

        [OneTimeSetUp]
        public void Setup()
        {
            mockhttpclient = new Mock<HttpMessageHandler>();
            auth0settings = Options.Create<Auth0Settings>(new Auth0Settings { Auth0ManagmentKey = "API KEY" });
            sendgridSettings = Options.Create<SendGridSettings>(new SendGridSettings { SENDGRID_API_KEY = "API KEY" });
            mockController = new UserSearchController(new HttpClient(mockhttpclient.Object), auth0settings, sendgridSettings); 
        }

        [Test]
        public async Task Test1()
        {
            UserMetadata fakeMetadata = new UserMetadata { Research = "lorem", Events = [2,3], Expertise = new List<int>(new int[] { 1, 2, 3 }) , Interest = ["dsds"] Social = { linkedIn = "linkedin" , sussex = "sussex" }, Education = { Available = "true", CareerStage = "UG", Department = "Inf", GraduationDate = "2222", school = "School of EngInf" } }; 

            User FakeUser = new User { User_id = "linkedIn|3232", }

            UserSearch fakeSearch = new UserSearch { Start = 1, Length = 2, Limit = 3, Total = 3, Users = new List<User>({ new User { email = "nishan9@icloud.com" } }) }; 

            mockhttpclient
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).Returns(Task.FromResult(new HttpResponseMessage
                { StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(fakeSearch),
                    Encoding.UTF8,
                    "application/json")
                }));

            var result = mockController.GetAllUsers();
            Assert.Pass(); 
        }

    }
}