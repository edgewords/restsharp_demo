using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serialization.Json;

namespace com.edgewords.core.restsharp.demo
{
    [Binding]
    class Users_Steps
    {
        RestClient client = new RestClient();
        IRestResponse response;

        [When(@"I request user number (.*)")]
        public void WhenIRequestUserNumber(int p0)
        {
            //set the API client
            client.BaseUrl = new Uri("http://localhost:2002/api/");
            client.Authenticator = new HttpBasicAuthenticator("edge", "edgewords");

            // create a request
            RestRequest request = new RestRequest("users/1", Method.GET, DataFormat.Json);

            //execute it and get a response
            response = client.Execute(request);
        }

        [Then(@"I get a (.*) response code")]
        public void ThenIGetAResponseCode(int iResp)
        {
            int status = (int)response.StatusCode;
            Console.WriteLine("Response Code: " + status);
            Assert.True(status == iResp);  
        }

        [Then(@"The item is an iPad")]
        public void ThenTheItemIsAnIPad()
        {
            Assert.That(response.Content.Contains("Bob Jones"));
        }


        [Given(@"that I am not authorized")]
        public void GivenThatIAmNotAuthorized()
        {
            //remove basic auth from client
            client.RemoveDefaultParameter("Authorization");
        }

        [When(@"I get all users")]
        public void WhenIGetAllUsers()
        {
            client.BaseUrl = new Uri("http://localhost:2002/api/");
            RestRequest request = new RestRequest("users", Method.GET, DataFormat.Json);

            //execute it and get a response
            response = client.Execute(request);
        }

    }
}
