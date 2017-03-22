using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.DataAccess
{
    public class DAL
    {
        private static string _connectionStringCityOfWindsor = System.Configuration.ConfigurationManager.AppSettings["connectionStringCityOfWindsor"];

        /// <summary>
        /// Holds the connection string to the database.
        /// </summary>
        public static string ConnectionStringCityOfWindsor
        {
            get
            {
                return _connectionStringCityOfWindsor;
            }
            set
            {
                value = _connectionStringCityOfWindsor;
            }
        }

        /// <summary>
        /// Fill the data table using the stored procedure or an inline query.
        /// </summary>
        /// <param name="query">The inline query or the name of the stored procedure that needs to be executed</param>
        /// <param name="paramList">Sql parameter list to the stored procedure (If any)</param>
        /// <param name="cmdType">Command type (Inline or SP)</param>
        /// <param name="connectionString">DB Connection string</param>
        /// <returns>The datatable that is retrieved from the database</returns>
        public static DataTable FillDataTable(String query, SqlParameter[] paramList, CommandType cmdType, string connectionString)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            try
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandType = cmdType;
                command.Parameters.AddRange(paramList);
                return FillDataTable(command);
            }
            catch (SqlException sqEx)
            {
                throw sqEx;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
        /// <summary>
        /// Fills the datatable using the command details that are provided.
        /// </summary>
        /// <param name="command">Sql command details that needs to be executed</param>
        /// <returns>The datatable with data (In any)</returns>
        private static DataTable FillDataTable(SqlCommand command)
        {
            try
            {
                DataTable dataTable = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                command.CommandTimeout = 15;
                adapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception sqEx)
            {
                throw sqEx;
            }
        }
    }
}
