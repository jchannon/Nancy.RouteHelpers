
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

        /// <summary>
        /// Implicitly convert class to string
        /// </summary>
        /// <example>string MyString = new RouteParameters.Root().And().AnyIntAtLeastOnce("id");</example>
        /// <param name="f"></param>
        /// <returns></returns>
        public static implicit operator string(RouteParameters f) { return f.RouteBuilder.ToString(); }

        /// <summary>
        /// Creates a route that expects at least one integer and with a defined length
        /// </summary>
        /// <example>/1 or /1234</example>
        /// <param name="NamedGroup">The name of your parameter identifier</param>
        /// <param name="lengthStart"></param>
        /// <param name="lengthEnd"></param>
        /// <returns></returns>
        public RouteParameters AnyIntAtLeastOnce(string NamedGroup, int lengthStart, int lengthEnd)
        {
            RouteBuilder.Append(@"(?<" + NamedGroup + @">[\d]{" + lengthStart + "," + lengthEnd + "})");
            return this;
        }

        /// <summary>
        /// Creates a route that expects at least one integer
        /// </summary>
        /// <example>
        /// /products/1 or /products/123
        /// </example>
        /// <returns></returns>
        public RouteParameters AnyIntAtLeastOnce(string NamedGroup)
        {
            RouteBuilder.Append(@"(?<" + NamedGroup + @">[\d]+)");
            return this;
        }

        /// <summary>
        /// Creates a route that will accept an int but the URL doesn't always have to contain one
        /// </summary>
        /// <example>
        /// /products/1 or /products
        /// </example>
        /// <returns></returns>
        public RouteParameters AnyIntOptional(string NamedGroup)
        {
            //(?:/(?<id>[\d]*))?

            //RouteBuilder.Append(@"(?<" + NamedGroup + @">[\d]*)");
            RouteBuilder.Append(@"(?:/(?<" + NamedGroup + @">[\d]*))?");
            return this;
        }

        /// <summary>
        /// Seperates pattern segments
        /// </summary>
        /// <returns></returns>
        public RouteParameters And()
        {
            Root();
            return this;
        }

        /// <summary>
        /// Creates a route segment
        /// </summary>
        /// <returns></returns>
        public RouteParameters Root()
        {
            RouteBuilder.Append("/");
            return this;
        }

        //string, string length, certain string, certain number
    }




}
