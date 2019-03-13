using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Contracts.Model.MongoDbModel
{
    [JsonObject]
    [BsonIgnoreExtraElements]
    [KnownType(typeof(FlowGraphModel))]
    [DataContract]
    public class FlowGraphItem
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        [DataMember]
        public string _t { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public List<FlowGraphModel> FlowGraphs { get; set; }

    }


    [JsonObject]
    [BsonIgnoreExtraElements]
    [DataContract]
    public class FlowGraphModel
    {
        [DataMember]
        public DateTime TimeStamp { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public string XML { get; set; }

        [DataMember]
        public string JSON { get; set; }

        [DataMember]
        public string IdentityName { get; set; }


    }

}
