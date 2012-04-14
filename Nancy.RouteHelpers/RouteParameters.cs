
namespace Nancy.RouteHelpers
{
	public static class RouteParameters
	{
		public static string AnyInt()
		{
			return @"/(?<id>[\d]+)";
		}

		public static string AnyInt(int lengthStart, int lengthEnd)
		{
			return @"/(?<id>[\d]{" + lengthStart + "," + lengthEnd + "})";
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
