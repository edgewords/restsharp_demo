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
        static RestClientOptions options = new RestClientOptions("http://localhost:2002/api/"); //BaseUrl
        RestClient client = new RestClient(options) { Authenticator = new HttpBasicAuthenticator("edge", "edgewords") }; //Set default authenticator
        RestResponse response; //v107+

        [When(@"I request user number (.*)")]
        public void WhenIRequestUserNumber(int p0)
        {
            // create a request
            RestRequest request = new RestRequest("users/1", Method.Get);

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
            client.Authenticator = null;
        }

        [When(@"I get all users")]
        public void WhenIGetAllUsers()
        {
            RestRequest request = new RestRequest("users", Method.Get);

            //execute it and get a response (synchronously)
            response = client.Execute(request);
        }

    }
}
