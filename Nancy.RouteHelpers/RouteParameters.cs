
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

		//string, string length, certain string, certain number
	}
}
