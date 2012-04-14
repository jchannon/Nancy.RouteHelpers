namespace Nancy.RouteHelpers.Tests
{
	public class RouteParameterTestModule : NancyModule
	{
		public RouteParameterTestModule()
		{
			Get[RouteParameters.AnyInt(1, 4)] = parameters =>
			                                    	{
			                                    		return "IntOfLength1To4";
			                                    	};

			Get[RouteParameters.AnyInt()] = parameters =>
			                                	{

			                                		return "AnyInt";
			                                	};
		}
	}
}