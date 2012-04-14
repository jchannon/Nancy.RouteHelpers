
namespace Nancy.RouteHelpers
{
	public static class RouteParameters
	{
		public static string AnyInt()
		{
			return @"/(?<id>[\d]*)";
		}

		public static string AnyInt(int lengthStart, int lengthEnd)
		{
			return @"/(?<id>[\d]{" + lengthStart + "," + lengthEnd + "})";
		}

        /// <summary>
        /// Creates a route that will expect one or more ints
        /// </summary>
        /// <example>
        /// /products/1 or /products/123
        /// </example>
        /// <returns></returns>
        public static string AnyIntAtLeastOnce()
        {
            return @"/(?<id>[\d]+)";
        }

        /// <summary>
        /// Creates a route that will accept an int but doesn't have to
        /// </summary>
        /// <example>
        /// /products/1 or /products
        /// </example>
        /// <returns></returns>
        public static string PossibleInt()
        {
            return @"/(?<id>[\d]*)";
        }

        public static string And()
        {
            return Root();
        }

        public static string Root()
        {
            return "/";
        }

		//string, string length, certain string, certain number
	}
}
