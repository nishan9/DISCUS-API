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
        public async Task TestGetAllUsers()
        {
            Social fakeSocial = new Social { linkedIn = "linkedin", sussex = "rere" };
            Education fakeEducation = new Education { school = "fd", Available = "", CareerStage = "", Department = "", GraduationDate = "" };
            List<string> tags = new List<string>();
            tags.Add("gdGFD");
            List<int> events = new List<int>();
            events.Add(23);
            UserMetadata userMetadata = new UserMetadata { Social = fakeSocial, Education = fakeEducation, Research = "fdsds", Expertise = tags, Interest = tags, Events = events };
            User fakeUser = new User { Email = "nishan@iclodu.com" , User_metadata = userMetadata};
            List<User> fakeUserList = new List<User>();
            fakeUserList.Add(fakeUser);
            UserSearch fakeSearch = new UserSearch { Length = 12, Limit = 23, Start = 2, Total = 2, Users = fakeUserList }; 


            mockhttpclient
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).Returns(Task.FromResult(new HttpResponseMessage
                { StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(fakeSearch),
                    Encoding.UTF8,
                    "application/json")
                }));

            ObjectResult result = await mockController.GetAllUsers() as ObjectResult;
            UserSearch Actual = result.Value as UserSearch; 
            Assert.AreEqual(Actual.Length, fakeSearch.Length); 
        }

        [Test]
        public async Task TestGetOneUser()
        {
            Social fakeSocial = new Social { linkedIn = "linkedin", sussex = "rere" };
            Education fakeEducation = new Education { school = "fd", Available = "", CareerStage = "", Department = "", GraduationDate = "" };
            List<string> tags = new List<string>();
            tags.Add("gdGFD");
            List<int> events = new List<int>();
            events.Add(23);
            UserMetadata userMetadata = new UserMetadata { Social = fakeSocial, Education = fakeEducation, Research = "fdsds", Expertise = tags, Interest = tags, Events = events };
            User fakeUser = new User { Email = "nishan@iclodu.com", User_metadata = userMetadata, User_id = "linkedIn|3232" };
            

            mockhttpclient
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(fakeUser),
                    Encoding.UTF8,
                    "application/json")
                }));

            ObjectResult result = await mockController.GetOneUser("linkedIn|3232") as ObjectResult;
            User Actual = result.Value as User;
            Assert.AreEqual(Actual.Name, fakeUser.Name);
        }

        [Test]
        public async Task TestUpdateUser()
        {
            Social fakeSocial = new Social { linkedIn = "linkedin", sussex = "rere" };
            Education fakeEducation = new Education { school = "fd", Available = "", CareerStage = "", Department = "", GraduationDate = "" };
            List<string> tags = new List<string>();
            tags.Add("gdGFD");
            List<int> events = new List<int>();
            events.Add(23);
            UserMetadata userMetadata = new UserMetadata { Social = fakeSocial, Education = fakeEducation, Research = "fdsds", Expertise = tags, Interest = tags, Events = events };
            User fakeUser = new User { Email = "nishan@iclodu.com", User_metadata = userMetadata, User_id = "linkedIn|3232" };


            mockhttpclient
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(fakeUser),
                    Encoding.UTF8,
                    "application/json")
                }));

            ObjectResult result = await mockController.UpdateUser(fakeUser) as ObjectResult;
            User Actual = result.Value as User;
            Assert.AreEqual(Actual, fakeUser);
        }

        [Test]
        public async Task TestDeleteUser()
        {
            Social fakeSocial = new Social { linkedIn = "linkedin", sussex = "rere" };
            Education fakeEducation = new Education { school = "fd", Available = "", CareerStage = "", Department = "", GraduationDate = "" };
            List<string> tags = new List<string>();
            tags.Add("gdGFD");
            List<int> events = new List<int>();
            events.Add(23);
            UserMetadata userMetadata = new UserMetadata { Social = fakeSocial, Education = fakeEducation, Research = "fdsds", Expertise = tags, Interest = tags, Events = events };
            User fakeUser = new User { Email = "nishan@iclodu.com", User_metadata = userMetadata, User_id = "linkedIn|3232" };


            mockhttpclient
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(fakeUser),
                    Encoding.UTF8,
                    "application/json")
                }));

            ObjectResult result = await mockController.DeleteUser("linkedIn|3232") as ObjectResult;
            User Actual = result.Value as User;
            Assert.AreEqual(Actual.Name, fakeUser.Name);
        }

        [Test]
        public async Task TestGetPageUser()
        {
            Social fakeSocial = new Social { linkedIn = "linkedin", sussex = "rere" };
            Education fakeEducation = new Education { school = "fd", Available = "", CareerStage = "", Department = "", GraduationDate = "" };
            List<string> tags = new List<string>();
            tags.Add("gdGFD");
            List<int> events = new List<int>();
            events.Add(23);
            UserMetadata userMetadata = new UserMetadata { Social = fakeSocial, Education = fakeEducation, Research = "fdsds", Expertise = tags, Interest = tags, Events = events };
            User fakeUser = new User { Email = "nishan@iclodu.com", User_metadata = userMetadata };
            List<User> fakeUserList = new List<User>();
            fakeUserList.Add(fakeUser);
            UserSearch fakeSearch = new UserSearch { Length = 12, Limit = 23, Start = 2, Total = 2, Users = fakeUserList };


            mockhttpclient
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(fakeSearch),
                    Encoding.UTF8,
                    "application/json")
                }));

            ObjectResult result = await mockController.GetPageUser("Nishan",0) as ObjectResult;
            UserSearch Actual = result.Value as UserSearch;
            Assert.AreEqual(Actual.Length, fakeSearch.Length);
        }

    }
}