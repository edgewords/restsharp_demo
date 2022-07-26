using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
//using RestSharp.Serialization.Json;

namespace com.edgewords.core.restsharp.demo.features.step_bindings
{
    [Binding]
    class Products_Steps
    {
        static RestClientOptions options = new RestClientOptions("http://localhost:2002/api/"); //Base URL
        RestClient client = new RestClient(options); //Set options for RestSharp v107+ API
        RestResponse response; //to hold response
        RestResponse<Product> prod_response;
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
            // create a request
            RestRequest request = new RestRequest("products/3", Method.Delete);
            //execute it and get a response synchronously
            response = client.Execute(request);
        }


        [When(@"I GET a product that does not exist")]
        public void WhenIGETAProductThatDoesNotExist()
        {
            // create a request
            RestRequest request = new RestRequest("products/9999", Method.Get); //Method.GET, DataFormat.Json
            //execute it and get a response synchronously
            response = client.Execute(request);
        }

        [When(@"I GET product record number (.*)")]
        public void WhenIGETProductRecordNumber(int prod_num)
        {
            RestRequest request = new RestRequest("products/1", Method.Get);
            response = client.Execute(request);
        }

        [Then(@"The product is an ""(.*)""")]
        public void ThenTheProductIsAn(string prod_name)
        {
            RestRequest request = new RestRequest("products/1", Method.Get);
            var prod_response = client.Execute<Product>(request); //overide default return object to <Product> so de-serialize into our object
            Product prod = prod_response.Data;
            Assert.That(prod.name.Contains(prod_name));
        }

        //updating products
        [Given(@"That I have just created a new product with name ""(.*)"" and price of (.*)")]
        public void GivenThatIHaveJustCreatedANewProductWithNameAndPriceOf(string p_name, int p_price)
        {
            RestRequest request = new RestRequest("products/", Method.Post); 
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
            RestRequest request = new RestRequest("products/" + prod_id, Method.Put);
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
            RestRequest request = new RestRequest("products/" + prod_id, Method.Get);
            prod_response = client.Execute<Product>(request);
            Product prod = prod_response.Data;
            Console.WriteLine(prod.name + "is returned ");
            Assert.That(prod.name.Contains(p_name));
        }

        //create multiple products (Sceanrio outline)
        [When(@"I POST a new product with ""(.*)"", (.*)")]
        public void WhenIPOSTANewProductWithPendrive(string p_name, int p_price)
        {
            RestRequest request = new RestRequest("products/", Method.Post);
            request.AddJsonBody(new { name = p_name, price = p_price });
            prod_response = client.Execute<Product>(request); //overide default return object to <Product> so de-serialize into our object
                                                              //Capture the id of the new product:
            Product newprod = prod_response.Data;
            prod_id = newprod.id;
            Console.WriteLine("New Prod ID: " + prod_id);
        }
    }
}

