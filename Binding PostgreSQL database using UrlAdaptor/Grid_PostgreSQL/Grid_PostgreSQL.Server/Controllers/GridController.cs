using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;
using Npgsql;
using System.Data;
using System.ComponentModel.DataAnnotations;



namespace Grid_PostgreSQL.Server.Controllers
{
    [ApiController]
    public class GridController : ControllerBase
    {

        string ConnectionString = @"<Enter a valid connection string>";

        /// <summary>
        /// Processes the DataManager request to perform searching, filtering, sorting, and paging operations.
        /// </summary>
        /// <param name="DataManagerRequest">Contains the details of the data operation requested.</param>
        /// <returns>Returns a JSON object along with the total record count.</returns>
        [HttpPost]
        [Route("api/[controller]")]
        public object Post([FromBody] DataManagerRequest DataManagerRequest)
        {
            // Retrieve data from the data source (e.g., database).
            IQueryable<Orders> DataSource = GetOrderData().AsQueryable();

            // Get the total count of records.
            int totalRecordsCount = DataSource.Count();

            // Return data based on the request
            return new { result = DataSource, count = totalRecordsCount };
        }

        /// <summary>
        /// Retrieves the order data from the database.
        /// </summary>
        /// <returns>Returns a list of orders fetched from the database.</returns>
        [HttpGet]
        [Route("api/[controller]")]
        public List<Orders> GetOrderData()
        {
            // Define the SQL query to retrieve all orders from the database, ordered by OrderID.
            string queryStr = "SELECT * FROM public.\"Orders\" ORDER BY \"OrderID\"";

            // Create a new NpgsqlConnection object using the connection string.
            NpgsqlConnection Connection = new(ConnectionString);

            // Open the database connection before executing the query.
            Connection.Open();

            //Using NpgsqlCommand and Query create connection with database.
            NpgsqlCommand Command = new(queryStr, Connection);

            // Using NpgsqlDataAdapter to execute the NpgsqlCommand and fill the results into a DataTable. 
            NpgsqlDataAdapter DataAdapter = new(Command);

            // Create a DataTable to hold the data retrieved from the database.
            DataTable DataTable = new();

            // Using NpgsqlDataAdapter, process the query string and fill the data into the dataset.
            DataAdapter.Fill(DataTable);

            // Close the connection after executing the command.
            Connection.Close();

            // Cast the data fetched from NpgsqlDataAdapter to List.<T>
            List<Orders> dataSource = (from DataRow Data in DataTable.Rows
                                       select new Orders()
                                       {
                                           OrderID = Convert.ToInt32(Data["OrderID"]),
                                           CustomerID = Data["CustomerID"].ToString(),
                                           EmployeeID = Convert.ToInt32(Data["EmployeeID"]),
                                           ShipCity = Data["ShipCity"].ToString(),
                                           Freight = Convert.ToDecimal(Data["Freight"])
                                       }).ToList();
            return dataSource;
        }


        /// <summary>
        /// Inserts a new data item into the data collection.
        /// </summary>
        /// <param name="newRecord">It contains the new record detail which is need to be inserted.</param>
        /// <returns>Returns void.</returns>
        [HttpPost]
        [Route("api/[controller]/Insert")]
        public void Insert([FromBody] CRUDModel<Orders> value)
        {
            // Create query to insert the specific into the database by accessing its properties.
            string queryStr = $"Insert into \"Orders\" (\"CustomerID\", \"Freight\", \"ShipCity\", \"EmployeeID\") values('{value.value.CustomerID}',{value.value.Freight},'{value.value.ShipCity}','{value.value.EmployeeID}')";

            // Create a new NpgsqlConnection object using the connection string.
            NpgsqlConnection Connection = new NpgsqlConnection(ConnectionString);

            // Open the database connection before executing the query.
            Connection.Open();

            // Execute the Npgsql command.
            NpgsqlCommand Command = new NpgsqlCommand(queryStr, Connection);

            // Execute this code to reflect the changes into the database.
            Command.ExecuteNonQuery();

            // Close the database connection after executing the command.
            Connection.Close();

            // Add custom logic here if needed and remove the above method.
        }


        /// <summary>
        /// Update a existing data item from the data collection.
        /// </summary>
        /// <param name="Order">It contains the updated record detail which is need to be updated.</param>
        /// <returns>Returns void.</returns>
        [HttpPost]
        [Route("api/[controller]/Update")]
        public void Update([FromBody] CRUDModel<Orders> value)
        {
            // Create query to update the changes into the database by accessing its properties.
            string queryStr = $"Update \"Orders\" set \"CustomerID\"='{value.value.CustomerID}', \"Freight\"={value.value.Freight},\"EmployeeID\"={value.value.EmployeeID},\"ShipCity\"='{value.value.ShipCity}' where \"OrderID\"={value.value.OrderID}";

            // Create a new NpgsqlConnection object using the connection string.
            NpgsqlConnection Connection = new NpgsqlConnection(ConnectionString);

            // Open the database connection before executing the query.
            Connection.Open();

            // Execute the Npgsql command.
            NpgsqlCommand Command = new NpgsqlCommand(queryStr, Connection);

            // Execute this code to reflect the changes into the database.
            Command.ExecuteNonQuery();

            // Close the database connection after executing the command.
            Connection.Close();

            // Add custom logic here if needed and remove the above method.
        }


        /// <summary>
        /// Remove a specific data item from the data collection.
        /// </summary>
        /// <param name="value">It contains the specific record detail which is need to be removed.</param>
        /// <return>Returns void.</return>
        [HttpPost]
        [Route("api/[controller]/Remove")]
        public void Remove([FromBody] CRUDModel<Orders> value)
        {
            //Create query to remove the specific from database by passing the primary key column value.
            string queryStr = $"DELETE FROM \"Orders\" WHERE \"OrderID\" = {value.key}";

            // Create a new NpgsqlConnection object using the connection string.
            NpgsqlConnection Connection = new NpgsqlConnection(ConnectionString);

            // Open the database connection before executing the query.
            Connection.Open();

            // Execute the Npgsql command.
            NpgsqlCommand Command = new NpgsqlCommand(queryStr, Connection);

            //Execute this code to reflect the changes into the database.
            Command.ExecuteNonQuery();

            // Close the database connection after executing the command.
            Connection.Close();

            //Add custom logic here if needed and remove above method.
        }


        /// <summary>
        /// Batch update (Insert, Update, and Delete) a collection of data items from the data collection.
        /// </summary>
        /// <param name="CRUDModel<T>">The set of information along with details about the CRUD actions to be executed from the database.</param>
        /// <returns>Returns void.</returns>
        [HttpPost]
        [Route("api/[controller]/BatchUpdate")]
        public IActionResult BatchUpdate([FromBody] CRUDModel<Orders> value)
        {
            if (value.changed != null && value.changed.Count > 0)
            {
                foreach (Orders Record in (IEnumerable<Orders>)value.changed)
                {
                    // Create query to update the changes into the database by accessing its properties.
                    string queryStr = $"Update \"Orders\" set \"CustomerID\"='{value.value.CustomerID}', \"Freight\"={value.value.Freight},\"EmployeeID\"={value.value.EmployeeID},\"ShipCity\"='{value.value.ShipCity}' where \"OrderID\"={value.value.OrderID}";

                    // Create a new NpgsqlConnection object using the connection string.
                    NpgsqlConnection Connection = new NpgsqlConnection(ConnectionString);

                    // Open the database connection before executing the query.
                    Connection.Open();

                    // Execute the Npgsql command.
                    NpgsqlCommand Command = new NpgsqlCommand(queryStr, Connection);

                    // Execute this code to reflect the changes into the database.
                    Command.ExecuteNonQuery();

                    // Close the database connection after executing the command.
                    Connection.Close();

                    // Add custom logic here if needed and remove the above method.
                }
            }
            if (value.added != null && value.added.Count > 0)
            {
                foreach (Orders Record in (IEnumerable<Orders>)value.added)
                {
                    // Create query to insert the specific into the database by accessing its properties.
                    string queryStr = $"Insert into \"Orders\" (\"CustomerID\", \"Freight\", \"ShipCity\", \"EmployeeID\") values('{value.value.CustomerID}',{value.value.Freight},'{value.value.ShipCity}','{value.value.EmployeeID}')";

                    // Create a new NpgsqlConnection object using the connection string.
                    NpgsqlConnection Connection = new NpgsqlConnection(ConnectionString);

                    // Open the database connection before executing the query.
                    Connection.Open();

                    // Execute the Npgsql command.
                    NpgsqlCommand Command = new NpgsqlCommand(queryStr, Connection);

                    // Execute this code to reflect the changes into the database.
                    Command.ExecuteNonQuery();

                    // Close the database connection after executing the command.
                    Connection.Close();

                    // Add custom logic here if needed and remove the above method.
                }
            }
            if (value.deleted != null && value.deleted.Count > 0)
            {
                foreach (Orders Record in (IEnumerable<Orders>)value.deleted)
                {
                    //Create query to remove the specific from database by passing the primary key column value.
                    string queryStr = $"DELETE FROM \"Orders\" WHERE \"OrderID\" = {value.key}";

                    // Create a new NpgsqlConnection object using the connection string.
                    NpgsqlConnection Connection = new NpgsqlConnection(ConnectionString);

                    // Open the database connection before executing the query.
                    Connection.Open();

                    // Execute the Npgsql command.
                    NpgsqlCommand Command = new NpgsqlCommand(queryStr, Connection);

                    //Execute this code to reflect the changes into the database.
                    Command.ExecuteNonQuery();

                    // Close the database connection after executing the command.
                    Connection.Close();

                    //Add custom logic here if needed and remove above method.
                }
            }
            return new JsonResult(value);
        }

        public class CRUDModel<T> where T : class
        {
            public string? action { get; set; }
            public string? keyColumn { get; set; }
            public object? key { get; set; }
            public T? value { get; set; }
            public List<T>? added { get; set; }
            public List<T>? changed { get; set; }
            public List<T>? deleted { get; set; }
            public IDictionary<string, object>? @params { get; set; }
        }

        public class Orders
        {
            [Key]
            public int? OrderID { get; set; }
            public string? CustomerID { get; set; }
            public int? EmployeeID { get; set; }
            public decimal Freight { get; set; }
            public string? ShipCity { get; set; }
        }
    }


}
