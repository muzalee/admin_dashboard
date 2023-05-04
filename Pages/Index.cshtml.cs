﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using spl.Model;
using System.Diagnostics;
using System.Data.SqlClient;
using spl.Middleware;

namespace spl.Pages
{
    public class IndexModel : PageModel
    {
        public String errorMsg = "";

        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;

        public IndexModel(ILogger<IndexModel> logger, IConfiguration config)
        {
            _logger = logger;
            _configuration = config;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost(User user)
        {
            //add asp-page-handler="login" to form if want to use named method
            Debug.WriteLine($"Index OnPost: Logging user {user.Username}");
            String password = CryptoMiddleWare.ComputeSHA256Hash(user.Password);
            try
            {
                String connectionString = _configuration.GetConnectionString("DefaultConnection");
                using SqlConnection connection = new(connectionString);
                connection.Open();

                String sql = $"SELECT TOP 1 * FROM users WHERE username = '{user.Username}' AND password = '{password}'";

                using SqlCommand command = new(sql, connection);
                using SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    int id = reader.GetInt32(reader.GetOrdinal("id"));
                    string username = reader.GetString(reader.GetOrdinal("username"));
                    string userType = reader.GetString(reader.GetOrdinal("user_type"));
                    int? stationId = null;
                    int stationOrdinal = reader.GetOrdinal("id_stesen");
                    if (!reader.IsDBNull(stationOrdinal))
                    {
                        stationId = reader.GetInt32(stationOrdinal);
                    }

                    Response.Cookies.Append("UserId", id.ToString());
                    Response.Cookies.Append("Username", username);
                    Response.Cookies.Append("UserStation", stationId?.ToString() ?? "");
                    Response.Cookies.Append("UserType", userType);
                    Response.Cookies.Append("IsAuthenticated", "1");

                    connection.Close();
                    return RedirectToPage("/home/dashboard");

                }
                else
                {
                    connection.Close();
                    errorMsg = "Sila pastikan username dan password adalah betul.";
                    return Page();
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return Page();
            }
        }
    }
}