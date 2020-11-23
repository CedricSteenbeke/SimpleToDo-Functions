using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using cll.Models;

namespace cll
{
    public static class CreateTodo
    {
        [FunctionName("CreateTodo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todos")] HttpRequest req,
            [CosmosDB(databaseName: "ToDoItems", collectionName: "Items", ConnectionStringSetting = "CosmosDBConnection")] IAsyncCollector<ToDo> todoItemOut,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            string _todoTitle = null;
            
            _todoTitle = _todoTitle ?? data?.title;

            if(_todoTitle != null){
                var guid = Guid.NewGuid().ToString();
                var _todo = new ToDo {id=guid, title=data?.title, description=data?.description, user = data?.user, due=data?.due, isComplete=false};
                await todoItemOut.AddAsync(_todo);
                return new OkObjectResult(guid);
            }
            return new BadRequestObjectResult("Title or ID missing.");           
        }
    }
}
