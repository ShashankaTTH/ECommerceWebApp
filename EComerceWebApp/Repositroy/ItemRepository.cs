using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using EComerceWebApp.Interface;
using EComerceWebApp.Model;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata;
using System.Data.Common;
using static EComerceWebApp.Components.Pages.GetItems;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Data;

namespace EComerceWebApp.Components.Pages
{
    public class ItemRepository : IItemRepository
    {
        #region||prooperty||
        public string? ConnectionString = null;
        public IConfiguration Configuration { get; set; }


        // Constructor with dependency injection for SqlConnection
        public ItemRepository()
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json")
                                .AddEnvironmentVariables();
            Configuration = builder.Build();

            ConnectionString = Configuration.GetSection("ConnectionString:SQLServer").Value;
        }
        #endregion

        // GetItems method as per IItemRepository interface
        #region||get items||
        public List<Item> GetItems()
            {
            var items = new List<Item>();
            try
            {
                string query = "SELECT ITEM_ID, NAME, DESCRIPTION, PRICE, IMAGE FROM ITEM";

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand cm = new SqlCommand(query);
                    try
                    {
                        cm.Connection = connection;
                        connection.Open();
                        if (connection.State != System.Data.ConnectionState.Open)
                        {
                            connection.Open();
                        }
                        SqlDataReader dataReader = cm.ExecuteReader();
                        //reader.Read();
                        while (dataReader.Read())
                        {
                            //byte[] imageData = dataReader["IMAGE"] as byte[];
                        items.Add(new Item()
                            {
                                ITEM_ID = (int)dataReader["ITEM_ID"],
                                NAME = dataReader["NAME"].ToString(),
                                DESCRIPTION = dataReader["DESCRIPTION"].ToString(),
                                PRICE = dataReader["PRICE"].ToString()
                            /*// Image = imageData*/
                        });
                        }
                        connection.Close();

                    }
                    catch (Exception ex)
                    {
                        connection.Close();
                    }
                }
                return items;
            }
            catch (Exception ex)
            {
                // Log or handle exception (e.g., connection errors, SQL errors)
                Console.WriteLine($"An error occurred: {ex.Message}");
                // You can also throw the exception or handle it as needed
            }
            

            return items;
        }
        #endregion

        #region||getting the image ||
        public string getItemImageById(int Id)
        {
            string? imageUrl = "/Items/";  // Base URL for serving images

            string query = $@"SELECT ITEM_ID, IMAGE, IMAGE_NAME
                      FROM ITEM WHERE ITEM_ID = {Id}";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        string? imageName = Convert.ToString(dr["IMAGE_NAME"]);
                        if (!string.IsNullOrEmpty(imageName))
                        {
                            // Construct the full URL for the image
                            imageUrl += imageName;
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    connection.Close();
                }
            }

            return imageUrl;
        }
        #endregion

        #region||AddToCart||
        public bool AddToCart(CartItem cartItem)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                //Pass tabel valued parameter to Store Procedure                  

                SqlCommand cmd = new SqlCommand("ADDTOCART", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ITEM_ID", cartItem.ITEM_ID);
                cmd.Parameters.AddWithValue("@ITEM_NAME", cartItem.NAME);
                cmd.Parameters.AddWithValue("@IMAGE", cartItem.IMAGE_URL);
                cmd.Parameters.AddWithValue("@QUANTITY", cartItem.QUANTITY);

                con.Open();
                var count = cmd.ExecuteNonQuery();

                con.Close();
                if (count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region||AddItems||
        public void AddItem(Item item)
        {
            //_items.Add(item);
        }
        #endregion
        /* #region || UploadData to local storage method ||
         private async Task<string> UploadData(IFormFile fileUpload)
         {
             try
             {
                 // Specify the directory where you want to save the file

                 // string localDirectory = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
                 string localDirectory = @"C:\Items"; // Change this to your desired directory

                 // Ensure the directory exists
                 if (!Directory.Exists(localDirectory))
                 {
                     Directory.CreateDirectory(localDirectory);
                 }

                 // Combine the directory path with the file name
                 var filePath = Path.Combine(localDirectory, fileUpload.FileName);

                 // Save the file to the specified directory
                 using (var stream = new FileStream(filePath, FileMode.Create))
                 {
                     await fileUpload.CopyToAsync(stream);
                 }

                 // Return the full path of the saved file
                 return filePath;
             }
             catch (Exception e)
             {
                 Console.WriteLine(e.ToString());
                 return "";
             }
         }
         #endregion*/
    }
}
