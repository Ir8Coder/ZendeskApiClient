﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZendeskApi.Client.Http;
using ZendeskApi.Client.Resources;
using ZendeskApi.Contracts.Models;
using ZendeskApi.Contracts.Requests;
using ZendeskApi.Contracts.Responses;
using Moq;
using NUnit.Framework;

namespace ZendeskApi.Client.Tests.Resources
{
    public class TicketResourceFixture
    {
        private Mock<IRestClient> _client;

        [SetUp]
        public void SetUp()
        {
            _client = new Mock<IRestClient>();
        }

        [Test]
        public void Get_Called_CallsBuildUriWithFieldId()
        {
            // Given
            _client.Setup(b => b.BuildUri(It.IsAny<string>(), It.Is<string>(s => s.Contains("321")))).Returns(new Uri("http://search"));
            var ticketResource = new TicketResource(_client.Object);

            // When
            ticketResource.Get(321);

            // Then
            _client.Verify(c => c.BuildUri(It.Is<string>(s => s.Contains("321")), ""));
        }

        [Test]
        public void Get_Called_ReturnsTicketResponse()
        {
            // Given
            var response = new TicketResponse { Item = new Ticket { Id = 1 }};
            _client.Setup(b => b.GetAsync<TicketResponse>(It.IsAny<Uri>())).Returns(TaskHelper.CreateTaskFromResult(response));
            var ticketResource = new TicketResource(_client.Object);

            // When
            var result = ticketResource.Get(321);

            // Then
            Assert.That(result, Is.EqualTo(response));
        }

        [Test]
        public void GetAll_Called_CallsBuildUriWithFieldId()
        {
            // Given
            _client.Setup(b => b.BuildUri(It.IsAny<string>(), It.IsAny<string>())).Returns(new Uri("http://search"));
            var ticketResource = new TicketResource(_client.Object);

            // When
            ticketResource.GetAll(new List<long> { 321, 456, 789 });

            // Then
            _client.Verify(c => c.BuildUri(It.Is<string>(s => s.Contains("/show_many")), It.IsAny<string>()));
        }

        [Test]
        public void GetAll_Called_ReturnsTicketResponse()
        {
            // Given
            var response = new TicketListResponse { Results = new List<Ticket> { new Ticket { Id = 1 } } };
            _client.Setup(b => b.GetAsync<TicketListResponse>(It.IsAny<Uri>())).Returns(TaskHelper.CreateTaskFromResult(response));
            var ticketResource = new TicketResource(_client.Object);

            // When
            var result = ticketResource.GetAll(new List<long> { 321, 456, 789 });

            // Then
            Assert.That(result, Is.EqualTo(response));
        }

        [Test]
        public void Put_Called_BuildsUri()
        {
            // Given
            var request = new TicketRequest { Item = new Ticket { Subject = "blah blah", Id = 123 } };
            var ticketResource = new TicketResource(_client.Object);

            // When
            ticketResource.Put(request);

            // Then
            _client.Setup(b => b.BuildUri(It.IsAny<string>(), ""));
        }

        [Test]
        public void Put_CalledWithTicket_ReturnsTicketReponse()
        {
            // Given
            var response = new TicketResponse { Item = new Ticket { Subject = "blah blah" } };
            var request = new TicketRequest { Item = new Ticket { Subject = "blah blah", Id = 123 } };
            _client.Setup(b => b.PutAsync<TicketResponse>(It.IsAny<Uri>(), request, "application/json")).Returns(TaskHelper.CreateTaskFromResult(response));
            var ticketResource = new TicketResource(_client.Object);

            // When
            var result = ticketResource.Put(request);

            // Then
            Assert.That(result, Is.EqualTo(response));
        }

        [Test]
        public void Put_TicketHasNoId_ThrowsException()
        {
            // Given
            var response = new TicketResponse { Item = new Ticket { Subject = "blah blah" } };
            var request = new TicketRequest { Item = new Ticket { Subject = "blah blah" } };
            _client.Setup(b => b.PutAsync<TicketResponse>(It.IsAny<Uri>(), request, "application/json")).Returns(TaskHelper.CreateTaskFromResult(response));
            var ticketResource = new TicketResource(_client.Object);

            // When, Then
            Assert.Throws<AggregateException>(() => ticketResource.Put(request));
        }

        [Test]
        public void Post_Called_BuildsUri()
        {
            // Given
            var request = new TicketRequest { Item = new Ticket { Subject = "blah blah" } };
            var ticketResource = new TicketResource(_client.Object);
            
            // When
            ticketResource.Post(request);

            // Then
            _client.Setup(b => b.BuildUri(It.IsAny<string>(), ""));
        }

        [Test]
        public void Post_CalledWithTicket_ReturnsTicketReponse()
        {
            // Given
            var response = new TicketResponse { Item = new Ticket { Subject = "blah blah" } };
            var request = new TicketRequest { Item = new Ticket { Subject = "blah blah" } };
            _client.Setup(b => b.PostAsync<TicketResponse>(It.IsAny<Uri>(), request, "application/json")).Returns(TaskHelper.CreateTaskFromResult(response));
            var ticketResource = new TicketResource(_client.Object);

            // When
            var result = ticketResource.Post(request);

            // Then
            Assert.That(result, Is.EqualTo(response));
        }

        [Test]
        public void Delete_Called_CallsBuildUriWithFieldId()
        {
            // Given
            _client.Setup(b => b.BuildUri(It.IsAny<string>(), It.Is<string>(s => s.Contains("321")))).Returns(new Uri("http://search"));
            var ticketResource = new TicketResource(_client.Object);

            // When
            ticketResource.Delete(321);

            // Then
            _client.Verify(c => c.BuildUri(It.Is<string>(s => s.Contains("321")), ""));
        }

        [Test]
        public void Delete_Called_CallsDeleteOnClient()
        {
            // Given
            var response = new TicketResponse { Item = new Ticket { Id = 1 } };
            _client.Setup(b => b.GetAsync<TicketResponse>(It.IsAny<Uri>())).Returns(TaskHelper.CreateTaskFromResult(response));
            var ticketResource = new TicketResource(_client.Object);

            // When
            ticketResource.Delete(321);

            // Then
            _client.Verify(c => c.DeleteAsync(It.IsAny<Uri>()));
        }
    }
}
