using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class RateLimitAttribute : ActionFilterAttribute
{
    // This is a simple in-memory store for demo purposes.
    private static Dictionary<string, DateTime> _requestTimes = new Dictionary<string, DateTime>();

    private int _maxRequestsPerSecond;

    public RateLimitAttribute(int maxRequestsPerSecond)
    {
        _maxRequestsPerSecond = maxRequestsPerSecond;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var clientIp = context.HttpContext.Connection.RemoteIpAddress.ToString();
        
        if (_requestTimes.ContainsKey(clientIp))
        {
            var lastRequest = _requestTimes[clientIp];
            var timeSinceLastRequest = DateTime.UtcNow - lastRequest;

            if (timeSinceLastRequest.TotalSeconds < 1.0 / _maxRequestsPerSecond)
            {
                // Block request
                context.Result = new ContentResult
                {
                    Content = "Too many requests, please slow down.",
                    StatusCode = 429
                };
                return;
            }
        }

        _requestTimes[clientIp] = DateTime.UtcNow;
    }
}


// //x And note that this implementation doesn't handle multiple concurrent requests from the same IP very well: if you get a burst of requests all at once, it will let through as many as arrive before the first one is processed. A more robust implementation would need to use a lock or a semaphore to ensure that only one request is processed at a time.

// Lastly, it does not take into account that a client might use multiple IP addresses to circumvent the rate limit. Therefore, for a production-ready rate limiter, consider using a more sophisticated mechanism for identifying clients, such as API keys, and consider using a library specifically designed for rate limiting.