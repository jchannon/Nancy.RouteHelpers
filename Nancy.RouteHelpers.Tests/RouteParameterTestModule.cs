namespace Nancy.RouteHelpers.Tests
{
	public class RouteParameterTestModule : NancyModule
	{
		public RouteParameterTestModule() 
		{
            Get[new RouteParameters().Root().AnyIntAtLeastOnce("id", 1, 4)] = parameters =>
                                                    {
                                                        return "IntOfLength1To4";
                                                    };

            Get[new RouteParameters().Root().AnyIntAtLeastOnce("id")] = parameters =>
                                                {
                                                    return "AnyInt";
                                                };

            Get[new RouteParameters().Root().AnyIntOptional("id")] = parameters =>
                {
                    return "OptionalInt";
                };
		}
	}
}