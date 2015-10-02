﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Embedded;
using Lending.Core;
using Lending.Core.NewUser;
using Lending.Execution.Auth;
using Lending.Execution.EventStore;
using NUnit.Framework;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.Text;

namespace Tests.NewUser
{
    public class NewUserRequestHandlerTests : DatabaseAndEventStoreFixtureBase
    {
        [Test]
        public void ExistingUserShouldNotBeCreatedAndNoEventEmitted()
        {
            var existingUser = DefaultTestData.ServiceStackUser1;
            SaveEntities(existingUser);//This is needed because ServiceStack will persist the AuthDto behind the scenes on sign-up

            CommitTransactionAndOpenNew();

            var request = new AuthSessionDouble();
            var expectedResponse = new BaseResponse();

            var expectedUser = DefaultTestData.ServiceStackUser1;

            var sut = new NewUserRequestHandler(() => Session, new UnexpectedRepository());
            BaseResponse actualResponse = sut.HandleRequest(request);

            actualResponse.ShouldEqual(expectedResponse);

            CommitTransactionAndOpenNew();

            int numberOfUsersInDb = Session
                .QueryOver<ServiceStackUser>()
                .RowCount()
                ;

            Assert.That(numberOfUsersInDb, Is.EqualTo(1));
        }

        [Test]
        public void NewUserShouldBeCreatedAndEventEmitted()
        {
            var authDto = DefaultTestData.UserAuthPersistenceDto1;
            SaveEntities(authDto);//This is needed because ServiceStack will persist the AuthDto behind the scenes on sign-up

            CommitTransactionAndOpenNew();

            var stream = "User-" + authDto.Id;
            var request = new AuthSessionDouble();
            var expectedResponse = new BaseResponse();
            var expectedUser = DefaultTestData.ServiceStackUser1;
            var expectedEvent = new UserAdded(Guid.NewGuid(), authDto.DisplayName, authDto.PrimaryEmail);
            //expectedEvent.Id = "1";

            var sut = new NewUserRequestHandler(() => Session, Emitter);
            BaseResponse actualResponse = sut.HandleRequest(request);

            actualResponse.ShouldEqual(expectedResponse);

            CommitTransactionAndOpenNew();
            WriteEmittedEvents();

            ServiceStackUser userInDb = Session
                .QueryOver<ServiceStackUser>()
                .SingleOrDefault()
                ;

            userInDb.ShouldEqual(expectedUser);


            StreamEventsSlice slice = Connection.ReadStreamEventsBackwardAsync(stream, 0, 10, false).Result;
            Assert.That(slice.Events.Count(), Is.EqualTo(1));
            var value = Encoding.UTF8.GetString(slice.Events[0].Event.Data);
            UserAdded actual = value.FromJson<UserAdded>();
            actual.ShouldEqual(expectedEvent);
        }

        public class AuthSessionDouble : IAuthSession
        {
            public bool HasRole(string role)
            {
                throw new NotImplementedException();
            }

            public bool HasPermission(string permission)
            {
                throw new NotImplementedException();
            }

            public bool IsAuthorized(string provider)
            {
                throw new NotImplementedException();
            }

            public void OnRegistered(IServiceBase registrationService)
            {
                throw new NotImplementedException();
            }

            public void OnAuthenticated(IServiceBase authService, IAuthSession session, IOAuthTokens tokens, Dictionary<string, string> authInfo)
            {
                throw new NotImplementedException();
            }

            public void OnLogout(IServiceBase authService)
            {
                throw new NotImplementedException();
            }

            public void OnCreated(IHttpRequest httpReq)
            {
                throw new NotImplementedException();
            }

            public string ReferrerUrl { get; set; }
            public string Id { get; set; }

            public string UserAuthId
            {
                get { return "1"; }
                set { } 
            }

            public string UserAuthName { get; set; }
            public string UserName { get; set; }
            public string DisplayName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public List<IOAuthTokens> ProviderOAuthAccess { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime LastModified { get; set; }
            public List<string> Roles { get; set; }
            public List<string> Permissions { get; set; }
            public bool IsAuthenticated { get; set; }
            public string Sequence { get; set; }
        }

        public class UnexpectedRepository : IRepository
        {
            public TAggregate GetById<TAggregate>(Guid id) where TAggregate : Aggregate
            {
                throw new NotImplementedException();
            }

            public TAggregate GetById<TAggregate>(Guid id, int version) where TAggregate : Aggregate
            {
                throw new NotImplementedException();
            }

            public void Save(Aggregate aggregate, Guid commitId, Action<IDictionary<string, object>> updateHeaders)
            {
                throw new NotImplementedException();
            }

            public void EmitEvent(string stream, Event @event)
            {
                Assert.Fail("UserAdded should not be emitted for existing user");
            }
        }
    }
}
