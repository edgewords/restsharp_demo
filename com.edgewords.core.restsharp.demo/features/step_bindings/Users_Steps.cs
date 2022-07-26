using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;

namespace com.edgewords.core.restsharp.demo
{
    [Binding]
    class Users_Steps
    {
        static RestClientOptions options = new RestClientOptions("http://localhost:2002/api/"); //v107
        RestClient client = new RestClient(options) { Authenticator = new HttpBasicAuthenticator("edge", "edgewords") }; //Added option for v107
        RestResponse response; //v107+

        [When(@"I request user number (.*)")]
        public void WhenIRequestUserNumber(int p0)
        {
            //set the API client

            //client.Authenticator = new HttpBasicAuthenticator("edge", "edgewords");

            // create a request
            RestRequest request = new RestRequest("users/1", Method.Get); //Method.GET, DataFormat.Json

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
            //client.RemoveDefaultParameter("Authorization"); //v106
            client.Authenticator = null;
        }

        [When(@"I get all users")]
        public void WhenIGetAllUsers()
        {
            //client.BaseUrl = new Uri("http://localhost:2002/api/");
            RestRequest request = new RestRequest("users", Method.Get); //Method.GET, DataFormat.Json

            //execute it and get a response
            response = client.Execute(request);
        }

    }
}
