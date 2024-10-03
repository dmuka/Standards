using System.Data;

namespace Standards.Infrastructure.Data.Repositories.Extensions
{
    public static class DataReaderExtensions
    {
        public static bool ColumnExists(this IDataReader reader, string columnName)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

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
}
