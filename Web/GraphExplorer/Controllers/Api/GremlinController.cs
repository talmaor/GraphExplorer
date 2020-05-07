using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using GraphExplorer.Configuration;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Graphs;
using Newtonsoft.Json.Linq;

namespace GraphExplorer.Controllers.Api
{
    public class GremlinController : ApiController
    {
        [Route("tenant")]
        [HttpGet]
        [Authorize]
        public async Task<dynamic> CurrentTenant()
        {
            return Utilities.Utilities.GetCurrentUserTenant(User);
        }

        [Route("user")]
        [HttpGet]
        [Authorize]
        public async Task<dynamic> CurrentUser()
        {
            return Utilities.Utilities.GetCurrentUserName(User);
        }

        [HttpGet]
        [Authorize]
        public async Task<dynamic> Get(string query, string collectionId)
        {
            Database database = DocDbSettings.Client.
                CreateDatabaseQuery(sqlExpression: $"SELECT * FROM d WHERE d.id = \"{Utilities.Utilities.GetCurrentUserTenant(User)}\"").
                AsEnumerable()?.
                FirstOrDefault();

            if (database == null)
            {
                throw new Exception("Still missing data for this tenant");
            }

            var collections = DocDbSettings.Client.CreateDocumentCollectionQuery(database.SelfLink).ToList();
            var coll = collections.FirstOrDefault(_ => _.Id == collectionId);

            var results = new List<dynamic>();
            var queries = query.Split(';');

            //split query on ; to allow for multiple queries
            foreach (var q in queries)
                if (!string.IsNullOrEmpty(q))
                {
                    var singleQuery = q.Trim();

                    foreach (var task in await ExecuteQuery(coll, singleQuery))
                    {
                        if (task["objects"] != null)
                        {
                            foreach (var innerTask in task["objects"])
                            {
                                results.Add(new { queryText = singleQuery, queryResult = innerTask});
                            }

                            continue;
                        }

                        results.Add(new {queryText = singleQuery, queryResult = task});
                    }
                }

            return results;
        }

        private async Task<List<JToken>> ExecuteQuery(DocumentCollection coll, string query)
        {
            var results = new List<JToken>();
            var gremlinQuery = DocDbSettings.Client.CreateGremlinQuery(coll, query);

            while (gremlinQuery.HasMoreResults)
            {
                try
                {
                    foreach (var result in await gremlinQuery.ExecuteNextAsync())
                    {
                        results.Add(result);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }   

            return results;
        }
    }
}