using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
    public class UnitTestUserSearch
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

            ObjectResult result = await mockController.GetPageUser("Nishan", 0) as ObjectResult;
            UserSearch Actual = result.Value as UserSearch;
            Assert.AreEqual(Actual.Length, fakeSearch.Length);
        }

        [Test]
        public async Task TestGetPage()
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

            ObjectResult result = await mockController.GetPage(0, "Nishan") as ObjectResult;
            UserSearch Actual = result.Value as UserSearch;
            Assert.AreEqual(Actual.Length, fakeSearch.Length);
        }

        [Test]
        public async Task TestGetPageAll()
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

            ObjectResult result = await mockController.GetPage(0, "ALL") as ObjectResult;
            UserSearch Actual = result.Value as UserSearch;
            Assert.AreEqual(Actual.Length, fakeSearch.Length);
        }

        [Test]
        public async Task TestActiveUsers()
        {
            //API returns an integer
            mockhttpclient
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(2),
                    Encoding.UTF8,
                    "application/json")
                }));

            ObjectResult result = await mockController.GetActiveUsers() as ObjectResult;
            Assert.AreEqual(result.Value, 2);
        }

        [Test]
        public async Task TestTotalUsers()
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

            ObjectResult result = await mockController.GetTotalUsers() as ObjectResult;
            Assert.AreEqual(result.Value, 2);
        }

        [Test]
        public async Task TestGetEmailsAll()
        {

            EmailList fakeEmailUser = new EmailList { Email = "bob@gmail.com", Name = "Bob" };

            List<EmailList> fakeEmailList = new List<EmailList>(); 
            fakeEmailList.Add(fakeEmailUser);

            mockhttpclient
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(fakeEmailList),
                    Encoding.UTF8,
                    "application/json")
                }));

            ObjectResult result = await mockController.GetEmailsAll() as ObjectResult;
            List<EmailList> actual = result.Value as List<EmailList>;
            Assert.AreEqual(actual.Count, 1);
        }

        [Test]
        public async Task TestUpdateUserProfile()
        {

            EventAttendance fakeEvent = new EventAttendance { Total = 2 }; 
            mockhttpclient
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(fakeEvent),
                    Encoding.UTF8,
                    "application/json")
                }));

            ObjectResult result = await mockController.GetAttendance(1) as ObjectResult;
            EventAttendance actual = result.Value as EventAttendance;
            Assert.AreEqual(actual.Total, 2);
        }

        [Test]
        public async Task TesUpdateUserProfile()
        {
            Social fakeSocial = new Social { linkedIn = "linkedin", sussex = "rere" };
            Education fakeEducation = new Education { school = "fd", Available = "", CareerStage = "", Department = "", GraduationDate = "" };
            List<string> tags = new List<string>();
            tags.Add("gdGFD");
            List<int> events = new List<int>();
            events.Add(23);
            UserMetadata userMetadata = new UserMetadata { Social = fakeSocial, Education = fakeEducation, Research = "fdsds", Expertise = tags, Interest = tags, Events = events };
            User fakeUser = new User { Email = "nishan@iclodu.com", User_metadata = userMetadata, User_id = "linkedIn|3232" };
            UpdateUser updateUser = new UpdateUser { Name = "Nisjan", User_metadata = userMetadata }; 

            mockhttpclient
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(fakeUser),
                    Encoding.UTF8,
                    "application/json")
                }));

            ObjectResult result = await mockController.UpdateUserProfile(updateUser, "linkedIn|3232") as ObjectResult;
            User Actual = result.Value as User;
            Assert.AreEqual(Actual.Name, fakeUser.Name);
        }

        [Test]
        public async Task TestGetInterestTags()
        {

            List<string> stringlist = new List<string> { };
            stringlist.Add("Meow");
            InterestList intlist = new InterestList { Interest = stringlist };
            TagSystemInterest tagSystemInterest = new TagSystemInterest { User_metadata = intlist };

            List<TagSystemInterest> tagSystemInterests = new List<TagSystemInterest> { };
            tagSystemInterests.Add(tagSystemInterest);


            NivoModel nivoModel = new NivoModel { ID = "Meow", Value = 1 };

            mockhttpclient
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(tagSystemInterests),
                    Encoding.UTF8,
                    "application/json")
                }));

            ObjectResult result = await mockController.GetInterestTags() as ObjectResult;
            IEnumerable<NivoModel> Actual = result.Value as IEnumerable<NivoModel>; 
            Assert.AreEqual(1, Actual.ToList().Count);
        }

        [Test]
        public async Task TestGetExpertiseTags()
        {

            List<string> stringlist = new List<string> { };
            stringlist.Add("Meow");
            ExpertiseList explist = new ExpertiseList { Expertise = stringlist };
            TagSystem tagsystem = new TagSystem { User_metadata = explist };
            List<TagSystem> tagSystemslist = new List<TagSystem> { };
            tagSystemslist.Add(tagsystem); 

            NivoModel nivoModel = new NivoModel { ID = "Meow", Value = 1 };

            mockhttpclient
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(tagSystemslist),
                    Encoding.UTF8,
                    "application/json")
                }));

            ObjectResult result = await mockController.GetExpertTags() as ObjectResult;
            IEnumerable<NivoModel> Actual = result.Value as IEnumerable<NivoModel>;
            Assert.AreEqual(1, Actual.ToList().Count);
        }

        [Test]
        public async Task TestSchoolDepList()
        {
            DepartSchool fakeDep = new DepartSchool { School = "Nish", Department = "Lod" };
            EduMetada fakeEdu = new EduMetada { Education = fakeDep };
            SchoolDepartment scholdep = new SchoolDepartment() { User_metadata = fakeEdu };
            List<SchoolDepartment> listscholdep = new List<SchoolDepartment> { };
            listscholdep.Add(scholdep);

            mockhttpclient
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(listscholdep),
                    Encoding.UTF8,
                    "application/json")
                }));

            ObjectResult result = await mockController.SchoolDepList() as ObjectResult;
            List<dynamic> actual = result.Value as List<dynamic>; 
            Assert.AreEqual(11, actual.Count);
        }

        [Test]
        public async Task TestSchoolDepListMultiple()
        {
            DepartSchool fakeDep = new DepartSchool { School = "Nish", Department = "Lod" };
            EduMetada fakeEdu = new EduMetada { Education = fakeDep };
            SchoolDepartment scholdep = new SchoolDepartment() { User_metadata = fakeEdu };
            SchoolDepartment scholdep2 = new SchoolDepartment() { User_metadata = fakeEdu };

            List<SchoolDepartment> listscholdep = new List<SchoolDepartment> { };
            listscholdep.Add(scholdep);
            listscholdep.Add(scholdep2);

            mockhttpclient
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(listscholdep),
                    Encoding.UTF8,
                    "application/json")
                }));

            ObjectResult result = await mockController.SchoolDepList() as ObjectResult;
            List<dynamic> actual = result.Value as List<dynamic>;
            Assert.AreEqual(11, actual.Count);
        }
    }
}