
using Nancy.Bootstrapper;
using Nancy.Testing;
using NUnit.Framework;
using Nancy.Testing.Fakes;
using System;


namespace Nancy.RouteHelpers.Tests
{
    public class RouteParameterTests
    {
        private FakeNancyModule SetupModule(string Path, Func<dynamic, Response> Response)
        {
            return new FakeNancyModule(with =>
            {
                with.Get(Path, Response);
            });
        }

        private ConfigurableBootstrapper SetupBoot(NancyModule module)
        {
            return new ConfigurableBootstrapper(with =>
            {
                with.DisableAutoRegistration();
                with.Module(module, module.GetType().FullName);
            });
        }

        private BrowserResponse SetupRouteResponse(string Root, string Path, RouteParameters NancyRoute, string ExpectedResponse)
        {
            var module = SetupModule(Root + "/" + NancyRoute, x => ExpectedResponse);

            var boostrapper = SetupBoot(module);

            var browser = new Browser(boostrapper);

            //Act
            var result = browser.Get(Path, with =>
            {
                with.HttpRequest();
            });

            return result;
        }

        [TestCase("/", "/1", 1, 3)]
        [TestCase("/", "/12", 1, 3)]
        [TestCase("/", "/123", 1, 3)]
        [TestCase("/", "/1", 1, 1)]
        [TestCase("/", "/12", 1, 2)]
        public void Browser_GetRequest_AcceptsAnyIntWithLengthRange(string Root, string Path, int LengthStart, int LengthEnd)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, Route.AnyIntAtLeastOnce("id", LengthStart, LengthEnd), "AnyIntLength");

            //Assert
            Assert.AreEqual("AnyIntLength", result.Body.AsString());

        }

        [TestCase("/", "/1")]
        [TestCase("/", "/123456789")]
        [TestCase("/dinners", "/dinners/1")]
        [TestCase("/dinners", "/dinners/123")]
        [TestCase("/dinners/edit", "/dinners/edit/123")]
        public void Browser_GetRequest_AcceptsAnyInt(string Root, string Path)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, Route.AnyIntAtLeastOnce("id"), "AnyInt");

            //Assert
            Assert.AreEqual("AnyInt", result.Body.AsString());

        }


        [TestCase("/", "/")]
        [TestCase("/", "/123")]
        [TestCase("/dinners", "/dinners")]
        [TestCase("/dinners", "/dinners/123")]
        [TestCase("/dinners/create", "/dinners/create")]
        [TestCase("/dinners/edit", "/dinners/edit/123")]
        public void Browser_GetRequest_AcceptsOptionalInt(string Root, string Path)
        {

            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, Route.AnyIntOptional("id"), "OptionalInt");

            //Assert
            Assert.AreEqual("OptionalInt", result.Body.AsString());
        }
    }
}
