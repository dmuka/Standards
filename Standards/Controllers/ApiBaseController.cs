using Microsoft.AspNetCore.Mvc;

namespace Standards.Controllers
{
    public class ApiBaseController : ControllerBase
    {
        protected ApiBaseController()
        {
        }

        //protected bool IsUserAuthenticated => PrincipalHelper.GetPrincipal(HttpContext.Current) != null;

        //protected int GetUserId()
        //{
        //    var userId = PrincipalHelper.GetRdUserId(HttpContext.Current);

        //    if (userId.HasValue)
        //    {
        //        return userId.Value;
        //    }

        //    throw new UserShouldLoggedBeforeIdentificationException("WebApiBaseController.GetCurrentAuthUserId()");
        //}

        //protected CultureInfo GetCurrentCulture()
        //{
        //    var headerLocale = UserCultureRequestHelper.GetCultureFromHeader(HttpContext.Current);

        //    if (headerLocale != null)
        //    {
        //        Thread.CurrentThread.CurrentUICulture = headerLocale;

        //        return headerLocale;
        //    }

        //    var locale = UserCultureRequestHelper.GetCultureFromCookie(HttpContext.Current);

        //    if (locale == null)
        //    {
        //        locale = Cultures.EnglishCulture;
        //    }

        //    Thread.CurrentThread.CurrentUICulture = locale;

        //    return locale;
        //}

        //protected IRepository Repository => _repository.Value;

        //protected Domain GetCurrentDomain()
        //{
        //    var requestUri = Request.RequestUri.AbsoluteUri;

        //    var pathAndQuery = Request.RequestUri.PathAndQuery;

        //    var url = requestUri.Replace(pathAndQuery, "");

        //    var domain = _repository.Value.Query<Domain>()
        //        .FirstOrDefault(d => d.Url == url);

        //    if (domain is null)
        //    {
        //        domain = new Domain { Url = url };

        //        _repository.Value.Save(domain);
        //    }

        //    return domain;
        //}

        //protected string GetCurrentDomainUrl()
        //{
        //    var requestUri = Request.RequestUri.AbsoluteUri;

        //    var pathAndQuery = Request.RequestUri.PathAndQuery;

        //    var url = requestUri.Replace(pathAndQuery, "");

        //    var validUrl = url.Replace("http://", "https://");

        //    return validUrl;
        //}
    }
}