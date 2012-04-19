using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nancy.RouteHelpers
{
    public static class Route
    {


        public static RouteParameters AnyIntAtLeastOnce(string NamedGroup)
        {
            return new RouteParameters().AnyIntAtLeastOnce(NamedGroup);
        }

        public static RouteParameters AnyIntOptional(string NamedGroup)
        {
            return new RouteParameters().AnyIntOptional(NamedGroup);
        }

        public static RouteParameters Root()
        {
            return new RouteParameters().Root();
        }
    }
}
