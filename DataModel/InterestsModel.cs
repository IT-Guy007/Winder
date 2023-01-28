using System.Data.SqlClient;

namespace DataModel;

public class InterestsModel {
    public List<string> GetInterestsFromDataBase(SqlConnection connection) {
        List<string> interests = new List<string>();

        string sql = "SELECT * FROM Winder.Winder.[Interests];";
        SqlCommand command = new SqlCommand(sql, connection);
        try {
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) {
                var item = reader["name"] as string;
                interests.Add(item);
            }
        } catch (SqlException e) {
            Console.WriteLine("Error retrieving interests from database");
            Console.WriteLine(e.ToString());
            Console.WriteLine(e.StackTrace);

        }

        return interests;
    }
}