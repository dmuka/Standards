namespace Standards.Data.Repositories.Models
{
    public class QueryDetails<T> : BaseQueryDetails<T> where T : class
    {
        /// <summary>
        /// Gets or sets the value of number of item you want to skip in the query.
        /// </summary>
        public int? Skip { get; set; }

        /// <summary>
        /// Gets or sets the value of number of item you want to take in the query.
        /// </summary>
        public int? Take { get; set; }
    }
}
