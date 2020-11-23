using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace cll
{
    public static class GetTodos
    {
        [FunctionName("GetTodos")]
         public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todos")] HttpRequest req,
            [CosmosDB(databaseName: "ToDoItems", collectionName: "Items", ConnectionStringSetting = "CosmosDBConnection")] IEnumerable<JObject> allTodos,
            ILogger log)
        {
            string userId = null;

            if (req.GetQueryParameterDictionary()?.TryGetValue(@"userId", out userId) == true
                && !string.IsNullOrWhiteSpace(userId))
            {
                var userTodos = allTodos.Where(r => r.Value<string>(@"user") == userId);
                return !userTodos.Any() ? new NotFoundObjectResult("No todo's found for this user.") : (IActionResult)new OkObjectResult(userTodos);
            }

            return new BadRequestObjectResult("userId param is required.");
        }
    }
}
