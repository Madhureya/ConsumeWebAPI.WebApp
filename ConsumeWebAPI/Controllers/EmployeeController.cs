using ConsumeWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ConsumeWebAPI.Controllers
{
    public class EmployeeController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7144/api");
        HttpClient client;
        public EmployeeController()
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }

        public IActionResult Index()
        {
            List<Employee> employees = new List<Employee>();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/employee").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                employees = JsonConvert.DeserializeObject<List<Employee>>(data);
            }
            return View(employees);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateOrUpdate(Employee employee)
        {
            string data = JsonConvert.SerializeObject(employee);
            StringContent stringContent = new StringContent(data, Encoding.UTF8, "application/json");

            if (employee.Id == 0)
            {
                HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/employee", stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return View("Create", employee);
            }
            else
            {
                HttpResponseMessage response = client.PutAsync(client.BaseAddress + $"/employee?empId={employee.Id}", stringContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return View("Create", employee);
            }
        }


        public ActionResult Delete(int empId)
        {
            HttpResponseMessage response = client.DeleteAsync(client.BaseAddress + $"/employee?empId={empId}").Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return BadRequest("Something went Wrong");
        }

        public ActionResult Edit(Employee employee)
        {
            return View("Create", employee);
        }

        public ActionResult Search(string searchString)
        {
            List<Employee> employees = new List<Employee>();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/employee/" + searchString).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                employees = JsonConvert.DeserializeObject<List<Employee>>(data);
            }
            return View("Index", employees);
        }
    }
}
