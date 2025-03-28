﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

public class AuthenticatedRouteConstraint : IRouteConstraint
{
    public bool Match(HttpContext httpContext, IRouter route, string routeKey,
        RouteValueDictionary values, RouteDirection routeDirection)
    {
        // Return true if the user is authenticated, false otherwise
        return httpContext.User?.Identity?.IsAuthenticated ?? false;
    }
}