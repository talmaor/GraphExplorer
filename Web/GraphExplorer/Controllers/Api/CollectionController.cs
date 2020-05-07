using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using GraphExplorer.Configuration;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace GraphExplorer.Controllers.Api
{
    public class CollectionController : ApiController
    {
        [HttpGet]
        public dynamic GetCollections()
        {
            var client = DocDbSettings.Client;
            var tenantId = Utilities.Utilities.GetCurrentUserTenant(User);
            if (tenantId == null)
            {
                return null;
            }

            Database database = client.CreateDatabaseQuery($"SELECT * FROM d WHERE d.id = \"{tenantId}\"").
                AsEnumerable()?.
                FirstOrDefault();

            if (database == null)
            {
                return null;
            }

            var collections = client.
                CreateDocumentCollectionQuery(database.SelfLink).
                Select(_ => _.Id).
                ToList();   

            return collections;
        }

        [HttpPost]
        public async Task CreateCollection([FromUri] string name)
        {
            await CreateCollectionIfNotExistsAsync(name);
        }

        [HttpDelete]
        public async Task DeleteCollection(string name)
        {
            await DeleteCollectionAsync(name);
        }

        private async Task CreateCollectionIfNotExistsAsync(string collectionId)
        {
            try
            {
                await DocDbSettings.Client.ReadDocumentCollectionAsync(
                    UriFactory.CreateDocumentCollectionUri(DocDbSettings.DatabaseId, collectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                    await DocDbSettings.Client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(DocDbSettings.DatabaseId),
                        new DocumentCollection {Id = collectionId},
                        new RequestOptions {OfferThroughput = 400});
                else
                    throw;
            }
        }

        private async Task DeleteCollectionAsync(string collectionId)
        {
            await DocDbSettings.Client.DeleteDocumentCollectionAsync(
                UriFactory.CreateDocumentCollectionUri(DocDbSettings.DatabaseId, collectionId));
        }
    }
}