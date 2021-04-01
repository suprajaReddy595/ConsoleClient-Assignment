using ConsoleApp1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClient1
{
    class Program
    {
        static void Main(string[] args)
        {
            GetAllStudents().Wait();
            Console.WriteLine("Enter Id");
            int id = int.Parse(Console.ReadLine());
            GetStudentById(id).Wait();

            Student student = new Student();
            Console.WriteLine("Enter ID");
            student.StudentId = int.Parse(Console.ReadLine());
            Console.WriteLine("ENter Name");
            student.Name = Console.ReadLine();
            Console.WriteLine("Enter Batch");
            student.Batch = Console.ReadLine();
            Console.WriteLine("ENter Marks");
            student.Marks = int.Parse(Console.ReadLine());
            Console.WriteLine("ENter Date of Birth");
            student.DateOfBirth = Convert.ToDateTime(Console.ReadLine());

            Insert(student).Wait();
            GetAllStudents().Wait();

            Put().Wait();
            GetAllStudents().Wait();

            Delete().Wait();
            GetAllStudents().Wait();

        }


        static async Task GetAllStudents()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44388/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("api/Students");
                if (response.IsSuccessStatusCode)
                {

                    var jsonString = response.Content.ReadAsStringAsync();
                    jsonString.Wait();

                    var studentList = JsonConvert.DeserializeObject<List<Student>>(jsonString.Result);

                    foreach (var temp in studentList)
                    {
                        Console.WriteLine("Id:{0}\tName:{1}", temp.StudentId, temp.Name);





                    }

                }
                else
                {
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine("Internal server Error");
                }

            }
        }

        static async Task GetStudentById(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44388/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/Students/" + id);
                if (response.IsSuccessStatusCode)
                {
                    Student student = await response.Content.ReadAsAsync<Student>();
                    Console.WriteLine("Id:{0}\tName:{1}", student.StudentId, student.Name);
                    //  Console.WriteLine("No of Employee in Department: {0}", department.Employees.Count);
                }
                else
                {
                    Console.WriteLine(response.StatusCode);

                }


            }
        }
        static async Task Insert(Student student)
        {
            using (var client = new HttpClient())
            {
                //Send HTTP requests from here. 
                client.BaseAddress = new Uri("https://localhost:44388/");


                HttpResponseMessage response = await client.PostAsJsonAsync("api/Students", student);

                if (response.IsSuccessStatusCode)
                {
                    // Get the URI of the created resource.  
                    Console.WriteLine(response.StatusCode);
                }
            }
        }
        static async Task Put()
        {

            using (var client = new HttpClient())
            {
                //Send HTTP requests from here. 
                client.BaseAddress = new Uri("https://localhost:44388/");

                //PUT Method  
                var department = new Student() { StudentId = 9, Name = "Updated Department" };
                int id = 1;
                HttpResponseMessage response = await client.PutAsJsonAsync("api/Students/" + id, department);
                if (response.IsSuccessStatusCode)

                {
                    Console.WriteLine(response.StatusCode);
                }
                else
                {
                    Console.WriteLine(response.StatusCode);
                }
            }
        }

        static async Task Delete()
        {
            using (var client = new HttpClient())
            {
                //Send HTTP requests from here. 
                client.BaseAddress = new Uri("https://localhost:44388/");


                int id = 1;
                HttpResponseMessage response = await client.DeleteAsync("api/Students/" + id);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode);
                }
                else
                    Console.WriteLine(response.StatusCode);
            }
        }
    }
}