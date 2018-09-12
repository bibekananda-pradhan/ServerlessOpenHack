using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace ServerlessFunctionAppNew
{
    public static class GetRating
    {
        [FunctionName("GetRating")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]HttpRequest req,
            [CosmosDB(
                databaseName: "ratingDB",
                collectionName: "ratingcollection2",
                ConnectionStringSetting = "CosmosDBConnection",
                Id = "{Query.ratingId}")] ToDoItem toDoItem,
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
