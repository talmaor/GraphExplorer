using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using GraphExplorer.Configuration;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Graphs;
using Newtonsoft.Json.Linq;

namespace GraphExplorer.Controllers
{
    public class GremlinController : ApiController
    {
        private static DocDbConfig _dbConfig = AppSettings.Instance.GetSection<DocDbConfig>("DocumentDBConfig");

        [HttpGet]
        public async Task<dynamic> Get(string query, string collectionId)
        {
            Database database = DocDbSettings.Client
                .CreateDatabaseQuery("SELECT * FROM d WHERE d.id = \"" + DocDbSettings.DatabaseId + "\"").AsEnumerable()
                .FirstOrDefault();
            var collections = DocDbSettings.Client.CreateDocumentCollectionQuery(database.SelfLink).ToList();
            var coll = collections.FirstOrDefault(x => x.Id == collectionId);

            var tasks = new List<Task>();
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
                foreach (var result in await gremlinQuery.ExecuteNextAsync())
                {
                    results.Add(result);
                }
            }   

            return results;
        }
    }
}