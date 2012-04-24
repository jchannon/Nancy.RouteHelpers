using Nancy.Bootstrapper;
using Nancy.Testing;
using NUnit.Framework;

namespace Nancy.RouteHelpers.Tests
{
    public class RouteParameterWithBasePathModuleTests
    {
        private static DefaultNancyBootstrapper SetupBootStrapper()
        {
            return new DefaultNancyBootstrapper();
        }

        [Test]
        public void Empty_string_should_match_optional_int()
        {
            INancyBootstrapper bootstrapper = SetupBootStrapper();
            var browser = new Browser(bootstrapper);

            Assert.AreEqual("OptionalInt", browser.Get("/dinners").Body.AsString());
        }

        [Test]
        public void Slash_should_match_optional_int()
        {
            INancyBootstrapper bootstrapper = SetupBootStrapper();
            var browser = new Browser(bootstrapper);

            Assert.AreEqual("OptionalInt", browser.Get("/dinners/").Body.AsString());
        }

        [Test]
        public void Int_should_match_optional_int()
        {
            INancyBootstrapper bootstrapper = SetupBootStrapper();
            var browser = new Browser(bootstrapper);

            Assert.AreEqual("OptionalInt", browser.Get("/dinners/23").Body.AsString());
        } 
    }
}