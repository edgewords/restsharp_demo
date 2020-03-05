using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serialization.Json;

namespace com.edgewords.core.restsharp.demo.features.step_bindings
{
    [Binding]
    class Products_Steps
    {
        RestClient client = new RestClient();
        IRestResponse response;
        IRestResponse<Product> prod_response;
        public int prod_id;

        [Then(@"I get a (.*) response")]
        public void ThenIGetAResponseCode(int iResp)
        {
            int status = (int)response.StatusCode;
            Console.WriteLine("Response Code: " + status);
            Assert.True(status == iResp);
        }

        [When(@"I Delete a product")]
        public void WhenIDeleteAProduct()
        {
            //set the API client
            client.BaseUrl = new Uri("http://localhost:2002/api/");
            // create a request
            RestRequest request = new RestRequest("products/3", Method.DELETE, DataFormat.Json);
            //execute it and get a response
            response = client.Execute(request);
        }


        [When(@"I GET a product that does not exist")]
        public void WhenIGETAProductThatDoesNotExist()
        {
            client.BaseUrl = new Uri("http://localhost:2002/api/");
            // create a request
            RestRequest request = new RestRequest("products/9999", Method.GET, DataFormat.Json);
            //execute it and get a response
            response = client.Execute(request);
        }

        [When(@"I GET product record number (.*)")]
        public void WhenIGETProductRecordNumber(int prod_num)
        {
            client.BaseUrl = new Uri("http://localhost:2002/api/");
            RestRequest request = new RestRequest("products/1", Method.GET, DataFormat.Json);
            response = client.Execute(request);
        }

        [Then(@"The product is an ""(.*)""")]
        public void ThenTheProductIsAn(string prod_name)
        {
            client.BaseUrl = new Uri("http://localhost:2002/api/");
            RestRequest request = new RestRequest("products/1", Method.GET, DataFormat.Json);
            var prod_response = client.Execute<Product>(request);
            Product prod = prod_response.Data;
            Assert.That(prod.name.Contains(prod_name));
        }

        //updating products
        [Given(@"That I have just created a new product with name ""(.*)"" and price of (.*)")]
        public void GivenThatIHaveJustCreatedANewProductWithNameAndPriceOf(string p_name, int p_price)
        {
            client.BaseUrl = new Uri("http://localhost:2002/api/");
            RestRequest request = new RestRequest("products/", Method.POST, DataFormat.Json);
            request.AddJsonBody(new { name = p_name, price = p_price });
            prod_response = client.Execute<Product>(request); //overide default return object to <Product> so de-serialize into our object
            //Capture the id of the new product:
            Product newprod = prod_response.Data;
            prod_id = newprod.id;
            Console.WriteLine("New Prod ID: " + prod_id);
        }

        [When(@"I Update that product with a name of ""(.*)"" and a price of (.*)")]
        public void WhenIUpdateThatProductWithANameOfAndAPriceOf(string p_name, int p_price)
        {
            Console.WriteLine("Prod ID is " + prod_id);
            client.BaseUrl = new Uri("http://localhost:2002/api/");
            RestRequest request = new RestRequest("products/" + prod_id, Method.PUT, DataFormat.Json);
            request.AddJsonBody(new { name = p_name, price = p_price });
            prod_response = client.Execute<Product>(request); //overide default return object to <Product> so de-serialize into our object
        }


        [Then(@"I get a (.*) response back")]
        public void ThenIGetAResponseBack(int iResp)
        {
            int status = (int)prod_response.StatusCode;
            Console.WriteLine("Response Code: " + status);
            Assert.True(status == iResp);
        }

        [Then(@"that Product is now a ""(.*)""")]
        public void ThenThatProductIsNowA(string p_name)
        {
            client.BaseUrl = new Uri("http://localhost:2002/api/");
            RestRequest request = new RestRequest("products/" + prod_id, Method.GET, DataFormat.Json);
            prod_response = client.Execute<Product>(request);
            Product prod = prod_response.Data;
            Assert.That(prod.name.Contains(p_name));
        }

        //create multiple products (Sceanrio outline)
        [When(@"I POST a new product with ""(.*)"", (.*)")]
        public void WhenIPOSTANewProductWithPendrive(string p_name, int p_price)
        {
            client.BaseUrl = new Uri("http://localhost:2002/api/");
            RestRequest request = new RestRequest("products/", Method.POST, DataFormat.Json);
            request.AddJsonBody(new { name = p_name, price = p_price });
            prod_response = client.Execute<Product>(request); //overide default return object to <Product> so de-serialize into our object
                                                                //Capture the id of the new product:
            Product newprod = prod_response.Data;
            prod_id = newprod.id;
            Console.WriteLine("New Prod ID: " + prod_id);
        }
    }
}

