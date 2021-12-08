using Microsoft.Data.Sqlite;

namespace geoDistance.Database
{
    public class AddressDatabase
    {
        public AddressDatabase()
        {

            CreateTable();

        }

        public void CreateTable()
        {
            using (var connection = new SqliteConnection("Data Source=address.db"))
            {
                connection.Open();

                var tableCommand = connection.CreateCommand();
                // tableCommand.CommandText = "CREATE TABLE address(nam"


            }
        }
    }
}