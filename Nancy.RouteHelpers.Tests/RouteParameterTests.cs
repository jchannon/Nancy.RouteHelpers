
using Nancy.Bootstrapper;
using Nancy.Testing;
using NUnit.Framework;

namespace Nancy.RouteHelpers.Tests
{
	public class RouteParameterTests
	{
        private DefaultNancyBootstrapper SetupBootStrapper()
        {
            return new DefaultNancyBootstrapper();
        }

		[Test]
		public void Url_consisting_of_more_than_four_ints()
		{
            INancyBootstrapper bootstrapper = SetupBootStrapper();
			var browser = new Browser(bootstrapper);

			Assert.AreEqual("AnyInt", browser.Get("/13245").Body.AsString());
		}

        [Test]
		public void Url_consiting_of_between_1_and_4_ints()
		{
            INancyBootstrapper bootstrapper = SetupBootStrapper();
			var browser = new Browser(bootstrapper);

            Assert.AreEqual("IntOfLength1To4", browser.Get("/123").Body.AsString());
		}

        [Test]
        public void Url_WithWithoutInt_Accepted()
        {
            INancyBootstrapper bootstrapper = SetupBootStrapper();
            var browser = new Browser(bootstrapper);

            Assert.AreEqual("OptionalInt", browser.Get("/").Body.AsString());
        }

        [Test]
        public void BaseUrl_WithWithoutInt_Accepted()
        {
            INancyBootstrapper bootstrapper = SetupBootStrapper();
            var browser = new Browser(bootstrapper);

            Assert.AreEqual("OptionalInt", browser.Get("/dinners/23").Body.AsString());
        }
	}
}
