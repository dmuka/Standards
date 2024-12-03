using System.Data;

namespace Infrastructure.Data.Repositories.Extensions;

public static class DataReaderExtensions
{
    public static bool ColumnExists(this IDataReader reader, string columnName)
    {
        ArgumentNullException.ThrowIfNull(reader);

        for (var i = 0; i < reader.FieldCount; i++)
        {
            if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }
}