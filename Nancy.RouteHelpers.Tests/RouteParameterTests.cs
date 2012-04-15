
using Nancy.Bootstrapper;
using Nancy.Testing;
using Xunit;

namespace Nancy.RouteHelpers.Tests
{
	public class RouteParameterTests
	{
        private DefaultNancyBootstrapper SetupBootStrapper()
        {
            return new DefaultNancyBootstrapper();
        }

		[Fact]
		public void Url_consisting_of_more_than_four_ints()
		{
            INancyBootstrapper bootstrapper = SetupBootStrapper();
			var browser = new Browser(bootstrapper);

			Assert.Equal("AnyInt", browser.Get("/13245").Body.AsString());
		}

		[Fact]
		public void Url_consiting_of_between_1_and_4_ints()
		{
            INancyBootstrapper bootstrapper = SetupBootStrapper();
			var browser = new Browser(bootstrapper);

			Assert.Equal("IntOfLength1To4", browser.Get("/123").Body.AsString());
		}

        [Fact]
        public void Url_WithWithoutInt_Accepted()
        {
            INancyBootstrapper bootstrapper = SetupBootStrapper();
            var browser = new Browser(bootstrapper);

            Assert.Equal("OptionalInt", browser.Get("/").Body.AsString());
        }
	}
}
