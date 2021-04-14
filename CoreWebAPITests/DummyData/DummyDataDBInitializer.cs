using CoreWebAPI.Data;
using CoreWebAPI.Models;
using System;
using Attribute = CoreWebAPI.Models.Attribute;

namespace CoreWebAPITests.DummyData
{
    public class DummyDataDBInitializer
    {
        public void Seed(BatchContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.BatchDetails.AddRange(
                new BatchViewModel() { Id = Guid.Parse("45A68966-9BF0-41C2-1285-08D8F146DB68"), BusinessUnit = "UKHO",
                    ExpiryDate = DateTime.Parse("2021-03-27 17:36:03.6470000"), AclId = 1 },
                new BatchViewModel() { Id = Guid.Parse("1609738E-3266-4BCB-FE21-08D8F8E42CFD"), BusinessUnit = "UKHO",
                    ExpiryDate = DateTime.Today.AddDays(30), AclId = 2
                }
            );

            context.Acl.AddRange(
                new Acl() { /*AclId = 1,*/ ReadGroups = new string[]{ "RG1"}, ReadUsers = new string[] { "RU1" }},
                new Acl() { /*AclId = 2,*/ ReadGroups = new string[]{ "RG2" }, ReadUsers = new string[] { "RU2" }}
            );

            context.Attribute.AddRange(
                new Attribute() { /*AttributeId = 1,*/ Key = "Key1", Value = "Value1", BatchId = Guid.Parse("45A68966-9BF0-41C2-1285-08D8F146DB68")},
                new Attribute() { /*AttributeId = 2,*/ Key = "Key2", Value = "Value2", BatchId = Guid.Parse("1609738E-3266-4BCB-FE21-08D8F8E42CFD") }
            );

            context.SaveChanges();
        }
    }
}
