using System.Data;
using FinanceManagement.Application.Helpers;
using Microsoft.Data.SqlClient;

public static class PaginationHelper
{
    public static async Task<PagedResult<T>> CreateAsync<T>(
        string procedureName,
        string connectionString,
        int pageNumber,
        int pageSize,
        string? searchName,
        Func<SqlDataReader, T> map)
    {
        var result = new PagedResult<T>();
        var data = new List<T>();
        int totalRecords = 0;

        using (SqlConnection connection = new SqlConnection(connectionString))
        using (SqlCommand command = new SqlCommand(procedureName, connection))
        {
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("@PageNumber", SqlDbType.Int).Value = pageNumber;
            command.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
            command.Parameters.Add("@SearchName", SqlDbType.NVarChar, 100).Value =
                string.IsNullOrWhiteSpace(searchName) ? DBNull.Value : searchName;

            await connection.OpenAsync();

            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {

                if (await reader.ReadAsync())
                {
                    totalRecords = reader.GetInt32(0);
                }
                await reader.NextResultAsync();
                while (await reader.ReadAsync())
                {
                    data.Add(map(reader));
                }
            }
        }

        result.PageNumber = pageNumber;
        result.PageSize = pageSize;
        result.TotalRecords = totalRecords;
        result.TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
        result.Data = data;

        return result;
    }
}