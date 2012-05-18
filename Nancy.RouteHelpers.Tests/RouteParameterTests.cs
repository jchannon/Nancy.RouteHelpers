
using Nancy.Bootstrapper;
using Nancy.Testing;
using NUnit.Framework;
using Nancy.Testing.Fakes;
using System;
using System.Diagnostics;
using System.Linq;

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

        private BrowserResponse SetupRouteResponse(string Root, string Path, string NancyRoute, string ExpectedResponse)
        {
            var module = SetupModule(Root + NancyRoute, x => ExpectedResponse);

            var boostrapper = SetupBoot(module);

            var browser = new Browser(boostrapper);

            //Act
            var result = browser.Get(Path, with =>
            {
                with.HttpRequest();
            });

            return result;
        }

        [TestCase("", "/1", 1, 3)]
        [TestCase("", "/12", 1, 3)]
        [TestCase("", "/123", 1, 3)]
        [TestCase("", "/1", 1, 1)]
        [TestCase("", "/12", 1, 2)]
        public void Browser_GetRequest_AcceptsAnyIntWithLengthRange(string Root, string Path, int LengthStart, int LengthEnd)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, "/" + Route.AnyIntAtLeastOnce("id", LengthStart, LengthEnd), "AnyIntLength");

            //Assert
            Assert.AreEqual("AnyIntLength", result.Body.AsString());

        }

        [TestCase("", "/1")]
        [TestCase("", "/123456789")]
        [TestCase("/dinners", "/dinners/1")]
        [TestCase("/dinners", "/dinners/123")]
        [TestCase("/dinners/edit", "/dinners/edit/123")]
        public void Browser_GetRequest_AcceptsAnyInt(string Root, string Path)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, "/" + Route.AnyIntAtLeastOnce("id"), "AnyInt");

            //Assert
            Assert.AreEqual("AnyInt", result.Body.AsString());

        }


        [TestCase("", "/")]
        [TestCase("", "/123")]
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

        [TestCase("", "/")]
        [TestCase("", "/abc")]
        [TestCase("/dinners", "/dinners")]
        [TestCase("/dinners", "/dinners/abc")]
        [TestCase("/dinners", "/dinners/!£$%&()_+{}")]
        public void Browser_GetReqest_AcceptsOptionalString(string Root, string Path)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, Route.AnyStringOptional("id"), "OptionalString");

            //Assert
            Assert.AreEqual("OptionalString", result.Body.AsString());

        }

        [TestCase("", "/a", 1, 1)]
        [TestCase("", "/a", 1, 2)]
        [TestCase("", "/ab", 1, 2)]
        [TestCase("", "/a", 1, 3)]
        [TestCase("", "/ab", 1, 3)]
        [TestCase("", "/abc", 1, 3)]
        public void Browser_GetReqest_AcceptsAnyStringWithLengthRange(string Root, string Path, int LengthStart, int LengthEnd)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, "/" + Route.AnyStringAtLeastOnce("id", LengthStart, LengthEnd), "AnyStringWithRange");

            //Assert
            Assert.AreEqual("AnyStringWithRange", result.Body.AsString());

        }

        [TestCase("", "/abc", "abc")]
        [TestCase("", "/abcdefghijklmnopqrstuvwxyz", "abcdefghijklmnopqrstuvwxyz")]
        [TestCase("", "/1", "1")]
        [TestCase("", "/12", "12")]
        [TestCase("", "/123", "123")]
        public void Browser_GetRequest_AcceptsExactPattern(string Root, string Path, string Exact)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, "/" + Route.Exact("id", Exact), "Exact");


            //Assert
            Assert.AreEqual("Exact", result.Body.AsString());
        }


    }
}
