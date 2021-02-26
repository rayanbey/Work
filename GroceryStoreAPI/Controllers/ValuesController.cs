using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GroceryStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Customer>> Get()
        {
            using (var reader = new StreamReader("database.json"))
            {
                var jsonFromFile = reader.ReadToEnd();
                 JObject o = JObject.Parse(jsonFromFile);
                  JArray a = (JArray)o["customers"];

                IEnumerable<Customer> customers = a.ToObject<IEnumerable<Customer>>();
                return customers.ToList();
            }
              
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<Customer>> Get(int id)
        {
            var list = Get().Value.ToList();

            var query = from cus in list
                        where id == cus.ID
                            select cus;
                return query.ToList(); 
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] Customer value)
        {
            var list = Get().Value.ToList();

            using (StreamWriter file = new StreamWriter("database.json"))
            {
                list.Add(value);
                var collectionWrapper = new
                {
                    customers = list
                };
                var output = JsonConvert.SerializeObject(collectionWrapper);
                file.WriteLine(output);
            }

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            IEnumerable<Customer> query = Get().Value;
            foreach (Customer c in query)
            {
                if(c.ID==id)
                c.Name = value;
            }
            using (StreamWriter file = new StreamWriter("database.json"))
            {
                var collectionWrapper = new
                {
                    customers = query
                };
                var output = JsonConvert.SerializeObject(collectionWrapper);
                file.WriteLine(output);
            }
        }
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var list = Get().Value.ToList();

            using (StreamWriter file = new StreamWriter("database.json"))
            {
                list.RemoveAll(x => x.ID== id);

                var collectionWrapper = new
                {
                    customers = list
                };
                var output = JsonConvert.SerializeObject(collectionWrapper);

                file.WriteLine(output);
            }
        }
    }
}
