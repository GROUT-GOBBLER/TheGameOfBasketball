

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace basketballUI.utilities
{
    internal class MauiUser
    {

        String Url { get; set; }
        public MauiUser(String Url)
        {

            Url = Url;

        }
        async private void Get(String controller, String result)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync($"API_URL/" + controller);
                    // List<Weather> w = JsonArray.JsonConvert.DeserializeObject<List<Person>>
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();


                    }
                    else
                    {
                        result = "failed";
                    }





                }
                catch (Exception ex)
                {
                    result = "failed";
                }
            }

        }

        async private void GetAt(String controller, String result, int id)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync($"API_URL/{controller}/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsStringAsync();//returns the json


                    }
                    else
                    {
                        result = "failed";
                    }





                }
                catch (Exception ex)
                {
                    result = "failed";
                }
            }

        }
        async private void Post(String controller, String result, Object model)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {

                    HttpResponseMessage response1 = await client.PostAsJsonAsync($"API_URL/{controller}", model);
                    if (response1.IsSuccessStatusCode)
                    {
                        result = $"success ";
                    }
                    else
                    {
                        result = $"no success  " + response1;
                    }











                }
                catch (Exception ex)
                {
                    result = "failed";
                }
            }
        }
        async private void Edit(String controller, String result, Object model, int id)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {

                    HttpResponseMessage response1 = await client.PutAsJsonAsync($"API_URL/{controller}/{id}", model);
                    if (response1.IsSuccessStatusCode)
                    {
                        result = $"success ";
                    }
                    else
                    {
                        result = $"no success  " + response1;
                    }











                }
                catch (Exception ex)
                {
                    result = "failed";
                }
            }
        }
        async private void Delete(String controller, String result, int id)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {

                    HttpResponseMessage response1 = await client.DeleteAsync($"{Url}/{id}");
                    if (response1.IsSuccessStatusCode)
                    {
                        result = $"success ";
                    }
                    else
                    {
                        result = $"no success  " + response1;
                    }











                }
                catch (Exception ex)
                {
                    result = "failed";
                }
            }
        }

    }
    

}

