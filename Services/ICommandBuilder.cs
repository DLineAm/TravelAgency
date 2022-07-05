using System.Data.SqlClient;

namespace TravelAgency.Services
{
    public interface ICommandBuilder : IBuilder
    {
        ICommandBuilder AddParameter(string parameter, object value);
        ICommandBuilder SetConnection(SqlConnection connection);
        ICommandBuilder SetCommandString(string command);
        ICommandBuilder AddDateParameter(string parameter, object value);
        //SqlCommand Build();
    }
}