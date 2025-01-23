using System.Text;

namespace Infrastructure.Logging;

public class LogEntry
{
    private const string ByUser = " by ";
    private const string Request = "Request: ";
    private const string Query = "Query params: ";
    private const string Body = "Body: ";
    private const string Response = "Response: ";
    private const string Type = ", type: ";
    private const string LoggingError = "Error while logging api request: ";
    private const string Separator = "============";


    public string Status { get; set; } = string.Empty;
    public string HttpMethod { get; set; } = string.Empty;
    public string RequestUrl { get; set; } = string.Empty;
    public Dictionary<string, string> QueryParams { get; set; } = new();
    public string JsonRequestBody { get; set; } = string.Empty;
    public string JsonResponseBody { get; set; } = string.Empty;
    public DateTime TimeStamp { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string ResponseType { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;

    public override string ToString()
    {
        var result = new StringBuilder();

        result.Append(TimeStamp);
        result.Append(ByUser);
        result.Append(UserName ?? "Unknown");
        result.Append(' ');
        result.Append(Request);
        result.Append(HttpMethod);
        result.Append(' ');
        result.AppendLine(RequestUrl);

        if (QueryParams.Count > 0)
        {
            result.Append(Query);

            foreach (var key in QueryParams.Keys)
            {
                result.Append($"{key}={QueryParams[key]}; ");
            }

            result.AppendLine();
        }

        if (JsonRequestBody != string.Empty)
        {
            result.Append(Body);
            result.AppendLine(JsonRequestBody);
        }

        result.Append(Response);
        result.Append(Status);

        if (ResponseType != string.Empty)
        {
            result.Append(Type);
            result.AppendLine(ResponseType);
        }
        else
        {
            result.AppendLine();
        }

        if (JsonResponseBody != string.Empty)
        {
            result.AppendLine(Body);
            result.AppendLine(JsonResponseBody);
        }

        if (ErrorMessage != string.Empty)
        {
            result.Append(LoggingError);
            result.AppendLine(ErrorMessage);
        }

        result.Append(Separator);

        return result.ToString();
    }
}