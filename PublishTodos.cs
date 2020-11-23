using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace cll
{
    public static class PublishTodos
    {
        [FunctionName("PublishTodos")]
        [return: Queue("todoqueue", Connection = "StorageConnectionAppSetting")]
        public static string Run([CosmosDBTrigger(
            databaseName: "ToDoItems",
            collectionName: "Items",
            ConnectionStringSetting = "CosmosDBConnection",
            LeaseCollectionName = "leases", CreateLeaseCollectionIfNotExists= true)]IReadOnlyList<Document> input, ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                return input[0].ToString();
            }
            throw new KeyNotFoundException("No result returned for function");
        }
    }
}
