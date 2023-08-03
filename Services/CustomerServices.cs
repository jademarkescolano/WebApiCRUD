using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System.Data;
using WebApiSample.Class;
using WebApiSample.Models;

namespace WebApiSample.Services
{
    public class CustomerServices
    {
        private readonly AppDb _constring;
        public IConfiguration Configuration;
        private readonly AppSettings _appSetting;



        public CustomerServices(AppDb constring, IConfiguration configuration, IOptions<AppSettings> appSettings)
        {
            _constring = constring;
            Configuration = configuration;
            _appSetting = appSettings.Value;
        }

        //View Clients
        public async Task<List<customer>> Customer()
        {
            List<customer> _customer = new();
            using (var con = new MySqlConnection(_constring.GetConnection()))
            {
                await con.OpenAsync().ConfigureAwait(false);
                var com = new MySqlCommand("SELECT * FROM customer", con)
                {
                    CommandType = CommandType.Text
                };
                var rdr = await com.ExecuteReaderAsync().ConfigureAwait(false);
                while (await rdr.ReadAsync().ConfigureAwait(false))
                {
                    _customer.Add(new customer
                    {
                        customerid = Convert.ToInt32(rdr["customerid"]),
                        name = rdr["name"].ToString(),
                        address = rdr["address"].ToString(),
                    });
                }
            }
            return _customer;
        }


        
        public async Task<int> AddCustomer(customer _customer)
        {
            using (var con = new MySqlConnection(_constring.GetConnection()))
            {
                await con.OpenAsync().ConfigureAwait(false);
                var com = new MySqlCommand("INSERT INTO customer (customerid,name,address) VALUES (@customerid,@name,@address)", con)
                {
                    CommandType = CommandType.Text,
                };
                com.Parameters.Clear();
                com.Parameters.AddWithValue("@customerid", _customer.customerid);
                com.Parameters.AddWithValue("@name", _customer.name);
                com.Parameters.AddWithValue("@Address", _customer.address);
                return await com.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        public async Task<int> UpdateCustomer(customer _customer)
        {
            using (var con = new MySqlConnection(_constring.GetConnection()))
            {
                await con.OpenAsync().ConfigureAwait(false);
                var com = new MySqlCommand("UPDATE customer SET name=@name, address=@address WHERE customerid=@customerid", con)
                {
                    CommandType = CommandType.Text,
                };
                com.Parameters.Clear();
                com.Parameters.AddWithValue("@customerid", _customer.customerid);
                com.Parameters.AddWithValue("@name", _customer.name);
                com.Parameters.AddWithValue("@address", _customer.address);
                return await com.ExecuteNonQueryAsync().ConfigureAwait(false);
            }

        }
        public async Task<int> DeleteCustomer(customer _customer)
        {
            using (var con = new MySqlConnection(_constring.GetConnection()))
            {
                await con.OpenAsync().ConfigureAwait(false);
                var com = new MySqlCommand("DELETE FROM customer WHERE customerid=@customerid", con)
                {
                    CommandType = CommandType.Text,
                };
                com.Parameters.Clear();
                com.Parameters.AddWithValue("@customerid", _customer.customerid);
                return await com.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }
    }
}
