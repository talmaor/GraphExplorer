using System.Threading.Tasks;
using System.Web.Http;
using GraphExplorer.Models;
using GraphExplorer.Utilities;

namespace GraphExplorer.Controllers.Api
{
    public class SettingsController : ApiController
    {
        private const string id = "__settings";

        private readonly FileSystemRepository<GraphSettings> _settingsRepo = new FileSystemRepository<GraphSettings>("graphsettings.json");

        public async Task<GraphSettings> Get(string collectionId)
        {
            return await _settingsRepo.GetItemAsync(collectionId);
        }

        public async Task Post([FromBody]GraphSettings value, [FromUri]string collectionId)
        {
            await _settingsRepo.CreateOrUpdateItemAsync(value, collectionId);
        }

        public async Task Delete(string collectionId)
        {
            await _settingsRepo.DeleteItemAsync(id, collectionId);
        }
    }
}