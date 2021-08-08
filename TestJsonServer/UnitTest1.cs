using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace TestJsonServer
{
    [TestClass]
    public class UnitTest1
    {
        //Initializing the restclient as null
        RestClient client = null;
        [TestInitialize]
        //This method is calling evrytime to initialzie the restclient object
        public void SetUp()
        {
            client = new RestClient("http://localhost:3000");
        }
        /// <summary>
        /// UC1-->Getting all the person details from json
        /// </summary>
        /// <returns></returns>
        public IRestResponse ReadAddressBookData()
        {
            //Get request 
            RestRequest request = new RestRequest("/AddressBook", Method.GET);
            //Passing the request and execute 
            IRestResponse response = client.Execute(request);
            //Return the response
            return response;
        }
        /// <summary>
        /// Testmethod to pass the test case
        /// </summary>
        [TestMethod]
        public void OnCallingGetAPI_ReturnPersonDetails()
        {
            IRestResponse response = ReadAddressBookData();
            //Convert the json object to list(deserialize)
            var res = JsonConvert.DeserializeObject<List<Person>>(response.Content);
            Assert.AreEqual(2, res.Count);
            //Check the status code 
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            //printing the data in console
            foreach (var i in res)
            {
                Console.WriteLine("FirstName:{0}\t LastName:{1}\t PhoneNumber:{2}\t, Address:{3}\t City:{4}\t,State:{5}\t ZipCode:{6}\t EmailId:{7}\t", i.FirstName, i.LastName, i.PhoneNumber, i.Addresses, i.City, i.State, i.ZipCode, i.EmailId);
            }
        }
        //add data to json server
        public void AddingInJsonServer(JsonObject jsonObject)
        {
            RestRequest request = new RestRequest("/AddressBook", Method.POST);
            request.AddParameter("application/json", jsonObject, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

        }
        /// <summary>
        /// UC2--->Adding a contact details in json server
        /// </summary>
        [TestMethod]
        public void OnCallingPostAPI_ReturnEmployee()
        {
            List<JsonObject> list = new List<JsonObject>();
            JsonObject json = new JsonObject();
            //Adding the details
            json.Add("FirstName", "Subhiksha");
            json.Add("LastName", "Ramesh");
            json.Add("PhoneNumber", "8975487224");
            json.Add("Addresses", "Avenue Road");
            json.Add("City", "Bangalore");
            json.Add("State", "Karnataka");
            json.Add("ZipCode", "64578");
            json.Add("EmailId", "subhiksha@gmail.com");
            list.Add(json);
            JsonObject json1 = new JsonObject();
            json1.Add("FirstName", "Arun");
            json1.Add("LastName", "Ramalingam");
            json1.Add("PhoneNumber", "9875847652");
            json1.Add("Addresses", "Colaba Causeway");
            json1.Add("City", "Mumbai");
            json1.Add("State", "Maharashtra");
            json1.Add("ZipCode", "65478");
           json1.Add("EmailId", "arun@gmail.com");
            list.Add(json1);
            //convert the jsonobject to employee object
            foreach (var i in list)
            {
                AddingInJsonServer(i);
            }

            IRestResponse response = ReadAddressBookData();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void OnCallingPutAPI_UpdateEmployeeDetails()
        {
            //Passing the method type as put(update existing employee details)
            RestRequest request = new RestRequest("/AddressBook/2", Method.PUT);
            //Creating a object
            JsonObject json = new JsonObject();
            //Adding the details
            json.Add("FirstName", "Priya");
            json.Add("LastName", "Venkat");
            json.Add("PhoneNumber", "9875847574");
            json.Add("Addresses", "Banu nagar");
            json.Add("City", "Chennai");
            json.Add("State", "TamilNadu");
            json.Add("ZipCode", "600145");
            json.Add("EmailId", "priyadarshini@gmail.com");
            //passing the type as json 
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            //convert the jsonobject to employee object
            var res = JsonConvert.DeserializeObject<Person>(response.Content);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
