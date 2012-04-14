
namespace Nancy.RouteHelpers
{
	public static class RouteParameters
	{
		public static string AnyInt()
		{
			return @"/(?<id>[\d]+)";
		}

		public static string AnyInt(int val1, int val2)
		{
			return @"/(?<id>[\d]{" + val1 + "," + val2 + "})";
		}

		//string, string length, certain string, certain number
	}
}
