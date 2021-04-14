using CoreWebAPI.Data;
using CoreWebAPI.Models;
using CoreWebAPI.Repository;
using CoreWebAPITests.DummyData;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Attribute = CoreWebAPI.Models.Attribute;

namespace CoreWebAPITests
{

    public class BatchControllerTests
    {
        private Mock<IBatchRepository> _test;
        private BatchViewModel batchViewModel;
        private BatchRepository _sut;
        private BatchContext _context;
        Guid guid = Guid.Parse("1609738E-3266-4BCB-FE21-08D8F8E42CFD");

        //public BatchControllerTests(BatchContext batchContext)
        //{
        //    _context = batchContext;
        //}

        [SetUp]
        public void Setup()
        {
            var contextOptions = new DbContextOptionsBuilder<BatchContext>()
                                    .UseSqlServer(@"Server=DESKTOP-HO1S13A;Database=CoreWebAPIDemo;Trusted_Connection=True;MultipleActiveResultSets=true")
                                    .Options;

            _context = new BatchContext(contextOptions);

            DummyDataDBInitializer db = new DummyDataDBInitializer();
            db.Seed(_context);

            _test = new Mock<IBatchRepository>();
            _sut = new BatchRepository(_context);
            Acl acl = new Acl() { AclId = 2, ReadGroups = new string[] { "RG2" }, ReadUsers = new string[] { "RU2" } };
            List<Attribute> attributeList = new List<Attribute>
            {
                new Attribute()
                {
                    AttributeId = 2,
                    BatchId = guid,
                    Key = "Key2",
                    Value = "Value2"
                }
            };

            batchViewModel = new BatchViewModel() { Id = guid, Acl = acl, Attributes = attributeList, AclId = 2, BusinessUnit = "UKHO", ExpiryDate = DateTime.Today.AddDays(30) };
        }

        //[Test]
        //public void Test1()
        //{
        //    _test.Setup(m => m.AddBatchDetails(batchViewModel));
        //    Assert.Pass();
        //}

        [Test]
        public void GetBatchDetails()
        {
            var expectedResult = batchViewModel;
            _test.Setup(m => m.GetBatchDetails(guid)).Returns(expectedResult);

            var actualResult = _sut.GetBatchDetails(guid);

            Assert.AreEqual(expectedResult.Id, actualResult.Id);
        }

        [Test]
        public void GetBatchDetails_Null_WhenBatchIdNotExists()
        {
            guid = Guid.NewGuid();
            var expectedResult = batchViewModel;
            _test.Setup(m => m.GetBatchDetails(guid)).Returns(valueFunction:() => null);

            var actualResult = _sut.GetBatchDetails(guid);

            Assert.Null(actualResult);
        }

        [Test]
        public void AddBatchDetails()
        {
            guid = Guid.NewGuid();
            Acl acl = new Acl() { ReadGroups = new string[] { "RG3" }, ReadUsers = new string[] { "RU3" } };
            List<Attribute> attributeList = new List<Attribute>
            {
                new Attribute()
                {
                    BatchId = guid,
                    Key = "Key3",
                    Value = "Value3"
                }
            };

            batchViewModel = new BatchViewModel() { Id = guid, Acl = acl, Attributes = attributeList, AclId = 3, BusinessUnit = "UKHO", ExpiryDate = DateTime.Today.AddDays(30) };

            var expectedResult = batchViewModel;
            _test.Setup(m => m.AddBatchDetails(batchViewModel)).Returns(expectedResult);

            var actualResult = _sut.AddBatchDetails(batchViewModel);

            Assert.AreEqual(expectedResult, actualResult);
        }

        //[Test]
        //public void PostMethodSetsLocationHeader()
        //{
        //    guid = Guid.NewGuid();
        //    Acl acl = new Acl() { ReadGroups = new string[] { "RG3" }, ReadUsers = new string[] { "RU3" } };
        //    List<Attribute> attributeList = new List<Attribute>
        //    {
        //        new Attribute()
        //        {
        //            BatchId = guid,
        //            Key = "Key3",
        //            Value = "Value3"
        //        }
        //    };

        //    batchViewModel = new BatchViewModel() { Id = guid, Acl = acl, Attributes = attributeList, AclId = 3, BusinessUnit = "UKHO", ExpiryDate = DateTime.Today.AddDays(30) };

        //    // Arrange
        //    var mockRepository = new Mock<IBatchRepository>();
        //    var controller = new BatchController(mockRepository.Object);

        //    // Act
        //    IHttpActionResult actionResult = controller.PostBatchViewModel(batchViewModel);
        //    var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<BatchViewModel>;

        //    // Assert
        //    Assert.IsNotNull(createdResult);
        //    Assert.AreEqual("DefaultApi", createdResult.RouteName);
        //    Assert.AreEqual(10, createdResult.RouteValues["id"]);
        //}
    }
}