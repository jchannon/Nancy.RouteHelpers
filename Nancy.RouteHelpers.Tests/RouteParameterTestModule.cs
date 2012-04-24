namespace Nancy.RouteHelpers.Tests
{
    public class RouteParameterTestModule : NancyModule
    {
        public RouteParameterTestModule()
        {
            //Get[Route.Root().AnyIntAtLeastOnce("id", 1, 4)] = parameters =>
            //                                        {
            //                                            return "IntOfLength1To4";
            //                                        };


            //Get[Route.Root().AnyIntAtLeastOnce("id")] = parameters =>
            //                                    {
            //                                        return "AnyInt";
            //                                    };

            Get[Route.AnyIntOptional("id")] = parameters =>
                {
                    return "OptionalInt";
                };
        }
    }
}