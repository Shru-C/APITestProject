using NUnit.Framework;
using Rest;
using RestSharp;
using RestSharp.Serialization.Json;
using System.Net;
using RestClient = Rest.RestClient;

namespace SampleAPITest
{
    public class DummyApi
    {
       

        [Test]
        
            public void StatusCodeTest()
            {
            // arrange
            RestSharp.RestClient client = new RestSharp.RestClient("http://api.zippopotam.us");
                RestRequest request = new RestRequest("nl/3825", Method.GET);

                // act
                IRestResponse response = client.Execute(request);
            

                // assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }

        [Test]
        public void ContentTypeTest()
        {
            // arrange
            RestSharp.RestClient client = new RestSharp.RestClient("http://api.zippopotam.us");
            RestRequest request = new RestRequest("nl/3825", Method.GET);

            // act
            IRestResponse response = client.Execute(request);

            // assert
            Assert.That(response.ContentType, Is.EqualTo("application/json"));
        }

        [TestCase("nl", "3825", HttpStatusCode.OK, TestName = "Check status code for NL zip code 7411")]
        [TestCase("lv", "1050", HttpStatusCode.NotFound, TestName = "Check status code for LV zip code 1050")]
      
        public void StatusCodeTest(string countryCode, string zipCode, HttpStatusCode expectedHttpStatusCode)
        {
            // arrange
            RestSharp.RestClient client = new RestSharp.RestClient("http://api.zippopotam.us");
            RestRequest request = new RestRequest($"{countryCode}/{zipCode}", Method.GET);

            // act
            IRestResponse response = client.Execute(request);

            // assert
            Assert.That(response.StatusCode, Is.EqualTo(expectedHttpStatusCode));
        }

        [Test]
        public void CountryAbbreviationSerializationTest()
        {
            // arrange
            RestSharp.RestClient client = new RestSharp.RestClient("http://api.zippopotam.us");
            RestRequest request = new RestRequest("us/90210", Method.GET);

            // act
            IRestResponse response = client.Execute(request);

            LocationResponse locationResponse =
                new JsonDeserializer().Deserialize<LocationResponse>(response);
           

            // assert
            Assert.That(locationResponse.CountryAbbreviation, Is.EqualTo("US"));
        }
        [Test]
        public void StateSerializationTest()
        {
            // arrange
            RestSharp.RestClient client = new RestSharp.RestClient("http://api.zippopotam.us");
            RestRequest request = new RestRequest("us/12345", Method.GET);

            // act
            IRestResponse response = client.Execute(request);
            LocationResponse locationResponse =
                new JsonDeserializer().
                Deserialize<LocationResponse>(response);

            // assert
            Assert.That(locationResponse.Places[0].State, Is.EqualTo("New York"));
        }

        [Test]
        public void PostRequestStatusCodeTest()
        {
            // arrange
            RestSharp.RestClient client = new RestSharp.RestClient("https://reqres.in/");
            RestRequest request = new RestRequest("api/users", Method.POST);
            var requestboby = new UserCreation { name = "morpheus", job = "leader" };
            request.AddJsonBody(requestboby);

            // act
            IRestResponse response = client.Execute(request);
           
            // assert command
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }
    }
}
