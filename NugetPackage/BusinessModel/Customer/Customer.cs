using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BusinessModel.Customer
{
    public class Customer
    {
        // MongoDB ObjectId is used as the primary identifier.
        [BsonId]
        public ObjectId Id { get; set; }

        // This will map to "name" in MongoDB
        [BsonElement("name")]
        public string Name { get; set; }

        // These are additional fields for customer information.
        [BsonElement("FirstName")]
        public string FirstName { get; set; }

        [BsonElement("Surname")]
        public string Surname { get; set; }

        // This maps to "EmailAddress" in MongoDB.
        [BsonElement("EmailAddress")]
        public string EmailAddress { get; set; }

        // If the following fields are needed, you can uncomment them
        [BsonElement("RequestSourceId")]
        public int RequestSourceId { get; set; }

        // Optional fields for extended info
        [BsonElement("BranchCode")]
        public string BranchCode { get; set; }

        [BsonElement("Gender")]
        public string Gender { get; set; }

        // Optional date of birth - using nullable DateTime
        [BsonElement("DOB")]
        public DateTime? DOB { get; set; }

        // Optional boolean fields for permissions
        [BsonElement("PermissionToMarket")]
        public bool PermissionToMarket { get; set; }

        [BsonElement("Is3rdPartyMarketingAllowed")]
        public bool Is3rdPartyMarketingAllowed { get; set; }

        // Optional flag for rewards program participation
        [BsonElement("RewardsOptIn")]
        public bool RewardsOptIn { get; set; }

        // Optional flag for school days participation
        [BsonElement("IsSchoolDaysAllowed")]
        public bool IsSchoolDaysAllowed { get; set; }

        // Nullable fields can be used for optional properties
        [BsonElement("IdPassportNumber")]
        public string IdPassportNumber { get; set; }

    }
}
