namespace Standards.Infrastructure.Exceptions.Enum
{
    public enum StatusCodeByError
    {
        InternalServerError = 500,

        BadGateway = 502,

        ServiceUnavailable = 503,

        BadRequest = 400,

        Unauthorized = 401,

        Forbidden = 403,

        NotFound = 404,

        Conflict = 409
    }
}
