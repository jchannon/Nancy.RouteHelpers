
namespace Nancy.RouteHelpers.Tests
{
    public class RouteParameterWithBasePathModule : NancyModule
    {
        public RouteParameterWithBasePathModule() : base("/dinners")
        {
            Get[Route.AnyIntOptional("id")] = parameters =>
            {
                return "/dinners/OptionalInt";
            };
        }
    }
}
