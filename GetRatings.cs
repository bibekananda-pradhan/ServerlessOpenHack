
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ServerlessFunctionAppNew
{
    public static class GetRatings
    {
        [FunctionName("GetRatings")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetRatings/{id}")]HttpRequest req,
            [CosmosDB(
                databaseName: "ratingDB",
                collectionName: "ratingcollection2",
                ConnectionStringSetting = "CosmosDBConnection",
                SqlQuery ="SELECT * FROM c where c.userId = {id}")] IEnumerable<ToDoItem> toDoItem,
            TraceWriter log)

        {
            log.Info("C# HTTP trigger function processed a request.");

            if (toDoItem == null)
            {
                log.Info($"ToDo item not found");
            }
            else
            {
                log.Info("Found ToDo item");
            }
            return new OkObjectResult(toDoItem);
        }
    }
}
