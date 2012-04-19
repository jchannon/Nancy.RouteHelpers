using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nancy.RouteHelpers.Tests
{
    public class RouteParameterWithBasePathModule : NancyModule
    {
        public RouteParameterWithBasePathModule() : base("/dinners")
        {
            Get[Route.AnyIntOptional("id")] = parameters =>
            {
                return "OptionalInt";
            };
        }
    }
}
