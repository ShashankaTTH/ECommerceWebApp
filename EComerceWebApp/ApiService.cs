using Newtonsoft.Json;
using System.Data;
using System.Net.Http.Headers;
using System.Text;

namespace EComerceWebApp
{
    public class ApiService
    {
        #region || ApiCallToGetDataDynamically ||
        public DataTable GetDataTableDynamically(string path)
        {
            string baseApiUrl = $@"https://localhost:7139/api/EcomControllercs/{path}";
            var client = new HttpClient();
            var response = client.GetAsync(baseApiUrl).Result;
            var result = response.Content.ReadAsStringAsync().Result;

            dynamic jsonResponse = JsonConvert.DeserializeObject<dynamic>(result);

            if (jsonResponse.data == null)
            {
                throw new Exception("The response does not contain loan applications data.");
            }

            DataTable dt = ConvertDynamicJsonToDataTable(jsonResponse.data);

            return dt;
        }


        private DataTable ConvertDynamicJsonToDataTable(dynamic jsonArray)
        {
            DataTable dt = new DataTable();
            bool columnsAdded = false;
            foreach (var item in jsonArray)
            {
                if (!columnsAdded)
                {
                    foreach (var property in item)
                    {
                        string columnName = property.Name;
                        dt.Columns.Add(columnName);
                    }
                    columnsAdded = true;
                }
                DataRow row = dt.NewRow();
                foreach (var property in item)
                {
                    string columnName = property.Name;
                    var value = property.Value?.ToString() ?? DBNull.Value.ToString();
                    row[columnName] = value;
                }

                dt.Rows.Add(row);
            }
            return dt;
        }

        #endregion

        #region||Send data to post Api||
        public bool SendDataToPostApi(object body, string path)
        {
            string baseApiUrl = $@"https://localhost:7139/api/EcomControllercs/{path}";

            using (var client = new HttpClient())
            {
               //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
                var response = client.PostAsync(baseApiUrl, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        #region||Send data to delete Api||
        public bool SendReqToDeleteApi(int id, string path)
        {
            string baseApiUrl = $@"https://localhost:7139/api/EcomControllercs/{path}?id={id}";

            using (var client = new HttpClient())
            {
                var response = client.DeleteAsync(baseApiUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion
    }
}
