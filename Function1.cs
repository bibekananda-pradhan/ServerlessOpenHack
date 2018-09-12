using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace ServerlessFunctionAppNew
{
    public class ToDoItem
    {
        public string Id { get; set; }
        public string userId { get; set; }
        public string productId { get; set; }
        public string locationName { get; set; }
        public int rating { get; set; }
        public string userNotes { get; set; }
        public DateTime timestamp { get; set; }
    }
    public static class Function1
    {
        [FunctionName("CreateRating")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req,
             [CosmosDB(
                databaseName: "ratingDB",
                collectionName: "ratingcollection2",
                ConnectionStringSetting = "CosmosDBConnection")]out dynamic document,
        TraceWriter log)
        {

            var parameters = req.Content.ReadAsAsync<UserRating>();
            using (var client = new HttpClient())
            {

                // anotherFunctionUri is another Azure Function's 
                // public URL, which should provide the secret code stored in app settings 
                // with key 'AnotherFunction_secret'
                Uri UserByUserID = new Uri($"https://serverlessohuser.trafficmanager.net/api/GetUser?userId={parameters.Result.userId}");

                var resUser = client.GetStringAsync(UserByUserID);
                var objUser = JsonConvert.DeserializeObject<User>(resUser.Result);


                Uri ProductByProductID = new Uri($"https://serverlessohproduct.trafficmanager.net/api/GetProduct?productId={parameters.Result.productId}");

                var resProduct = client.GetStringAsync(ProductByProductID);
                var objProduct = JsonConvert.DeserializeObject<Product>(resProduct.Result);

                // process the response
                document = null;
                if (objUser != null && objProduct != null)
                {
                    parameters.Result.id = Guid.NewGuid();
                    parameters.Result.timestamp = DateTime.Now;
                    document = parameters.Result;
                }


            }
            return new OkObjectResult(document);

        }
    }
    public class UserRating
    {
        public Guid id { get; set; }
        public string userId { get; set; }
        public string productId { get; set; }
        public string locationName { get; set; }
        public int rating { get; set; }
        public string userNotes { get; set; }
        public DateTime timestamp { get; set; }
    }
    public class Product
    {
        public string productId { get; set; }
        public string productName { get; set; }
        public string productDescription { get; set; }
    }
    public class User
    {
        public string userId { get; set; }
        public string userName { get; set; }
        public string fullName { get; set; }
    }
}
