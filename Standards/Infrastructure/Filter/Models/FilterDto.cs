namespace Standards.Infrastructure.Filter.Models;

public record FilterDto
{
    public string SearchQuery { get; set; }
    public int Page { get; set; }
    public int ItemsPerPage { get; set; }
    public FilterBy FilterBy { get; set; }
    public SortBy SortBy { get; set; }
}