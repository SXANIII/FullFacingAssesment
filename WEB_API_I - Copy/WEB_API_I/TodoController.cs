using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;

namespace WEB_API_I
{
    public class Todo
    {
        public string description { get; set; }
        public string priority { get; set; }
        public DateTime deadline { get; set; }
        public string status { get; set; }
    }


    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TodoController : ApiController
    {
        /// <summary>
        /// CREATE METHOD
        /// </summary>
        /// <param> Accepts JSON body in an API platform ie. Postman </param>
        /// <returns>Confirmation message</returns>

        // POST api/todo 
        public async Task<IEnumerable> Post([FromBody] Todo value)
        {
            try
            {
                var dbClient = new MongoClient("mongodb+srv://shaneUser:Kingdomhearts3@cluster0.itnty.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
                var database = dbClient.GetDatabase("tododb");
                var collection = database.GetCollection<BsonDocument>("todo");

                BsonDocument document = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(value.ToJson());

                await collection.InsertOneAsync(document);

                return "Todo Created";
            }
            catch 
            {
                return "An Error has occured";
            };
        }

        /// <summary>
        /// READ METHODS
        /// </summary>
        /// <param>ACCEPTS VARIOUS PARAMETERS AS Documented</param>
        /// <returns>JSON RESULT or Error Message</returns>

        // GET api/todo
        public IEnumerable Get()
        {
            try
            {
                var dbClient = new MongoClient("mongodb+srv://shaneUser:Kingdomhearts3@cluster0.itnty.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
                var data = dbClient.GetDatabase("tododb").GetCollection<BsonDocument>("todo").AsQueryable();
                return data.ToList().ConvertAll(BsonTypeMapper.MapToDotNetValue).ToList();
            }
            catch
            {
                return "An Error has occured";
            };
        }

        // GET api/todo?status=
        public IEnumerable Get(string status)
        {
            try
            {
                var dbClient = new MongoClient("mongodb+srv://shaneUser:Kingdomhearts3@cluster0.itnty.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
                var data = dbClient.GetDatabase("tododb");
                var collection = data.GetCollection<BsonDocument>("todo");
                var filter = Builders<BsonDocument>.Filter.Eq("status", status);
                return collection.Find(filter).ToList().ConvertAll(BsonTypeMapper.MapToDotNetValue).ToList();
            }
            catch
            {
                return "An Error has occured";
            };
        }

        // GET api/todo?sortby=&orderBy=
        public IEnumerable GetbySort(string sortBy, string orderBy)
        {
            try
            {
                var dbClient = new MongoClient("mongodb+srv://shaneUser:Kingdomhearts3@cluster0.itnty.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
                var data = dbClient.GetDatabase("tododb");
                var collection = data.GetCollection<BsonDocument>("todo");

                if (orderBy == "asc")
                {
                    var filter = Builders<BsonDocument>.Filter.Empty;
                    var sortorder = Builders<BsonDocument>.Sort.Ascending(sortBy);

                    var results = collection.Find(filter).Sort(sortorder);
                    return results.ToList().ConvertAll(BsonTypeMapper.MapToDotNetValue).ToList();
                }
                else if (orderBy == "desc")
                {
                    var filter = Builders<BsonDocument>.Filter.Empty;
                    var sortorder = Builders<BsonDocument>.Sort.Descending(sortBy);

                    var results = collection.Find(filter).Sort(sortorder);
                    return results.ToList().ConvertAll(BsonTypeMapper.MapToDotNetValue).ToList();
                }
                else 
                {
                    var filter = Builders<BsonDocument>.Filter.Empty;
                    return collection.Find(filter).ToList().ConvertAll(BsonTypeMapper.MapToDotNetValue).ToList();
                }
            }
            catch
            {
                return "An Error has occured";
            };
        }

        // GET api/todo?fields=
        public IEnumerable GetByFields(string fields)
        {
            try
            {
                var dbClient = new MongoClient("mongodb+srv://shaneUser:Kingdomhearts3@cluster0.itnty.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
                var database = dbClient.GetDatabase("tododb");
                var collection = database.GetCollection<BsonDocument>("todo");

                var filter = Builders<BsonDocument>.Filter.Empty;

                ProjectionDefinition<BsonDocument> projection = null;

                projection = Builders<BsonDocument>.Projection.Include(fields);
                return collection.Find(filter).Project(projection).ToList().ConvertAll(BsonTypeMapper.MapToDotNetValue).ToList();
            }

            catch 
            {
                return "An Error has occured";
            };
        }


        // GET api/todo/priority/ + desc or asc
        public IEnumerable GetByLimitAndOffset(int offset, int limit)
        {
            try
            {
                var dbClient = new MongoClient("mongodb+srv://shaneUser:Kingdomhearts3@cluster0.itnty.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
                var database = dbClient.GetDatabase("tododb");
                var collection = database.GetCollection<BsonDocument>("todo");

                var filter = Builders<BsonDocument>.Filter.Empty;
                return collection.Find(filter).Skip(offset).Limit(limit).ToList().ConvertAll(BsonTypeMapper.MapToDotNetValue).ToList();
            }
            catch 
            {
                return "An Error has occured";
            };
        }

        /// <summary>
        /// UPDATE METHOD
        /// </summary>
        /// <param>ACCEPTS ID in url and JSON in body of POSTMAN</param>
        /// <returns>Confirmation or Error message</returns>

        // PUT api/values/5 
        public async Task <IEnumerable> Put (int id, [FromBody] Todo value)
        {
            try { 
                var dbClient = new MongoClient("mongodb+srv://shaneUser:Kingdomhearts3@cluster0.itnty.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
                var database = dbClient.GetDatabase("tododb");
                var collection = database.GetCollection<BsonDocument>("todo");

                var filter = Builders<BsonDocument>.Filter.Eq("Id", id);
                var update = Builders<BsonDocument>.Update.Set("description", value.description);

                var deadlinefilter = Builders<BsonDocument>.Filter.Eq("Id", id);
                var deadlineupdate = Builders<BsonDocument>.Update.Set("deadline", value.deadline);

                var statusfilter = Builders<BsonDocument>.Filter.Eq("Id", id);
                var statusupdate = Builders<BsonDocument>.Update.Set("status", value.status);

                await collection.UpdateOneAsync(filter, update);
                await collection.UpdateOneAsync(deadlinefilter, deadlineupdate);
                await collection.UpdateOneAsync(statusfilter, statusupdate);

                return "To do Updated";
            }
            catch
            {
                return "An Error has occured";
            };
        }

        /// <summary>
        /// DELETE METHODS
        /// </summary>
        /// <param>Accepts ID to delete Record</param>
        /// <returns>returns Confirmation message or Error message</returns>

        // DELETE api/values/5 
        public async Task <IEnumerable> Delete(int id)
        {
            try { 
                var dbClient = new MongoClient("mongodb+srv://shaneUser:Kingdomhearts3@cluster0.itnty.mongodb.net/myFirstDatabase?retryWrites=true&w=majority");
                var database = dbClient.GetDatabase("tododb");
                var collection = database.GetCollection<BsonDocument>("todo");

                var deleteFilter = Builders<BsonDocument>.Filter.Eq("Id", id);

                await collection.DeleteOneAsync(deleteFilter);

                var data = dbClient.GetDatabase("tododb").GetCollection<BsonDocument>("todo").AsQueryable();

                return "To do Deleted";
            }
            catch
            {
                return "An Error has occured";
            };
        }
    }
}
