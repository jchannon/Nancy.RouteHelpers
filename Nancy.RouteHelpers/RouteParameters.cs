using System.Text;

namespace Nancy.RouteHelpers
{
    public class RouteParameters
    {
        private StringBuilder builder;
        public StringBuilder RouteBuilder
        {
            get
            {
                if (builder == null)
                    builder = new StringBuilder();

                return builder;
            }
        }
        
        public static implicit operator string(RouteParameters f) { return f.RouteBuilder.ToString(); }

        /// <summary>
        /// Creates a route that expects at least one integer, for example, /products/123
        /// </summary>
        /// <param name="NamedGroup">The name of the parameter in the route</param>
        /// <param name="lengthStart">The minimum number of integers in the route</param>
        /// <param name="lengthEnd">The maximum number of integers in the route</param>
        /// <returns></returns>
        public RouteParameters AnyIntAtLeastOnce(string NamedGroup, int lengthStart, int lengthEnd)
        {
            RouteBuilder.Append(@"(?<" + NamedGroup + @">[\d]{" + lengthStart + "," + lengthEnd + "})");
            return this;
        }

        /// <summary>
        /// Creates a route that expects at least one integer, for example, /products/123
        /// </summary>
        /// <param name="NamedGroup">The name of the parameter in the route</param>
        /// <returns></returns>
        public RouteParameters AnyIntAtLeastOnce(string NamedGroup)
        {
            RouteBuilder.Append(@"(?<" + NamedGroup + @">[\d]+)");
            return this;
        }

        /// <summary>
        /// Creates a route that will accept an int but the URL doesn't always have to contain one, for example, /products OR /products/123. NOTE: Do not use a / in your route before using this method as one is included
        /// </summary>
        /// <param name="NamedGroup">The name of the parameter in the route</param>
        /// <returns></returns>
        public RouteParameters AnyIntOptional(string NamedGroup)
        {
            RouteBuilder.Append(@"/?(?<" + NamedGroup + @">[\d]*)");
            return this;
        }

        /// <summary>
        /// Seperates route segments by adding a '/' to the route
        /// </summary>
        /// <returns></returns>
        public RouteParameters And()
        {
            Root();
            return this;
        }

        /// <summary>
        /// Starts route segments by adding a '/' to the route
        /// </summary>
        /// <returns></returns>
        public RouteParameters Root()
        {
            RouteBuilder.Append("/");
            return this;
        }

        /// <summary>
        /// Creates a route that will accept a string but the URL doesn't always have to contain one, for example, /products OR /products/mac. NOTE: Do not use a / in your route before using this method as one is included
        /// </summary>
        /// <param name="NamedGroup">The name of the parameter in the route</param>
        /// <returns></returns>
        public RouteParameters AnyStringOptional(string NamedGroup)
        {
            RouteBuilder.Append(@"/?(?<" + NamedGroup + @">[\S]*)");
            
            return this;
        }

        /// <summary>
        /// Creates a route that expects at least one string, for example, /mac
        /// </summary>
        /// <param name="NamedGroup">The name of the parameter in the route</param>
        /// <param name="lengthStart">The minimum number of characters in the route</param>
        /// <param name="lengthEnd">The minimum number of characters in the route</param>
        /// <returns></returns>
        public RouteParameters AnyStringAtLeastOnce(string NamedGroup, int lengthStart, int lengthEnd)
        {
            RouteBuilder.Append(@"(?<" + NamedGroup + @">[\S]{" + lengthStart + "," + lengthEnd + "})");

            return this;
        }

        /// <summary>
        /// Creates a route that expects at least one string, for example, /mac
        /// </summary>
        /// <param name="NamedGroup">The name of the parameter in the route</param>
        /// <returns></returns>
        public RouteParameters AnyStringAtLeastOnce(string NamedGroup)
        {
            RouteBuilder.Append(@"(?<" + NamedGroup + @">[\S]+)");

            return this;
        }

        /// <summary>
        /// Creates a route that expects a certain pattern
        /// </summary>
        /// <param name="NamedGroup">The name of the parameter in the route</param>
        /// <param name="Exact">The exact pattern in the route</param>
        /// <returns>bob</returns>
        public RouteParameters Exact(string NamedGroup, string Exact)
        {
            RouteBuilder.Append(@"(?<" + NamedGroup + ">" + Exact + ")");

            return this;
        }
    }
}
