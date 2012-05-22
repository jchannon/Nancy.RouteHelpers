namespace Nancy.RouteHelpers
{
    public static class Route
    {
        /// <summary>
        /// Creates a route that expects at least one integer, for example, /products/123
        /// </summary>
        /// <param name="NamedGroup">The name of the parameter in the route</param>
        /// <param name="lengthStart">The minimum number of integers in the route</param>
        /// <param name="lengthEnd">The maximum number of integers in the route</param>
        /// <returns></returns>
        public static RouteParameters AnyIntAtLeastOnce(string NamedGroup, int lengthStart, int lengthEnd)
        {
            return new RouteParameters().AnyIntAtLeastOnce(NamedGroup, lengthStart, lengthEnd);
        }

        /// <summary>
        /// Creates a route that expects at least one integer, for example, /products/123
        /// </summary>
        /// <param name="NamedGroup">The name of the parameter in the route</param>
        /// <returns></returns>
        public static RouteParameters AnyIntAtLeastOnce(string NamedGroup)
        {
            return new RouteParameters().AnyIntAtLeastOnce(NamedGroup);
        }

        /// <summary>
        /// Creates a route that will accept an int but the URL doesn't always have to contain one, for example, /products OR /products/123. NOTE: Do not use a / in your route before using this method as one is included
        /// </summary>
        /// <param name="NamedGroup">The name of the parameter in the route</param>
        /// <returns></returns>
        public static RouteParameters AnyIntOptional(string NamedGroup)
        {
            return new RouteParameters().AnyIntOptional(NamedGroup);
        }

        /// <summary>
        /// Seperates route segments by adding a '/' to the route
        /// </summary>
        /// <returns></returns>
        public static RouteParameters And()
        {
            return new RouteParameters().And();
        }

        /// <summary>
        /// Starts route segments by adding a '/' to the route
        /// </summary>
        /// <returns></returns>
        public static RouteParameters Root()
        {
            return new RouteParameters().Root();
        }

        /// <summary>
        /// Creates a route that will accept a string but the URL doesn't always have to contain one, for example, /products OR /products/mac. NOTE: Do not use a / in your route before using this method as one is included
        /// </summary>
        /// <param name="NamedGroup">The name of the parameter in the route</param>
        /// <returns></returns>
        public static RouteParameters AnyStringOptional(string NamedGroup)
        {
            return new RouteParameters().AnyStringOptional(NamedGroup);
        }

        /// <summary>
        /// Creates a route that expects at least one string, for example, /mac
        /// </summary>
        /// <param name="NamedGroup">The name of the parameter in the route</param>
        /// <param name="lengthStart">The minimum number of characters in the route</param>
        /// <param name="lengthEnd">The minimum number of characters in the route</param>
        /// <returns></returns>
        public static RouteParameters AnyStringAtLeastOnce(string NamedGroup, int lengthStart, int lengthEnd)
        {
            return new RouteParameters().AnyStringAtLeastOnce(NamedGroup, lengthStart, lengthEnd);
        }

        /// <summary>
        /// Creates a route that expects at least one string, for example, /mac
        /// </summary>
        /// <param name="NamedGroup">The name of the parameter in the route</param>
        /// <returns></returns>
        public static RouteParameters AnyStringAtLeastOnce(string NamedGroup)
        {
            return new RouteParameters().AnyStringAtLeastOnce(NamedGroup);
        }

        /// <summary>
        /// Creates a route that expects a certain pattern
        /// </summary>
        /// <param name="NamedGroup">The name of the parameter in the route</param>
        /// <param name="Exact">The exact pattern in the route</param>
        /// <returns>bob</returns>
        public static RouteParameters Exact(string NamedGroup, string Exact)
        {
            return new RouteParameters().Exact(NamedGroup, Exact);
        }       
    }
}
