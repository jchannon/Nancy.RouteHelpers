using Nancy.Testing;
using NUnit.Framework;
using Nancy.Testing.Fakes;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

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

            Console.WriteLine("******Module Def: " + module.Routes.First().Description.Path);
            Console.WriteLine("******Request Path: " + Path);


            var result = browser.Get(Path, with =>
            {
                with.HttpRequest();
            });

            Console.WriteLine("******Parameter Values:");
            IEnumerable<string> keys = ((DynamicDictionary)result.Context.Parameters).GetDynamicMemberNames();
            foreach (var item in keys)
            {
                Console.WriteLine("******" + item + " : " + result.Context.Parameters[item]);
            }

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

        [TestCase("", "/a")]
        [TestCase("", "/ab")]
        [TestCase("", "/abc")]
        public void Browser_GetReqest_AcceptsAnyString(string Root, string Path)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, "/" + Route.AnyStringAtLeastOnce("id"), "AnyString");

            //Assert
            Assert.AreEqual("AnyString", result.Body.AsString());
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

        /**********
         * AnyString 
        ***********/

        [Test]
        public void Browser_GetRequest_AcceptsAnyStringAndAnyInt()
        {
            //Arrange & Act
            var result = SetupRouteResponse("", "/product/123", Route.Root().AnyStringAtLeastOnce("name").And().AnyIntAtLeastOnce("id"), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());

        }

        [Test]
        public void Browser_GetRequest_AcceptsAnyStringAndAnyIntWithRange()
        {
            //Arrange & Act
            var result = SetupRouteResponse("", "/product/123", Route.Root().AnyStringAtLeastOnce("name").And().AnyIntAtLeastOnce("id", 1, 3), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());

        }

        [TestCase("", "/product/123")]
        [TestCase("", "/product")]
        public void Browser_GetRequest_AcceptsAnyStringAndOptionalInt(string Root, string Path)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, Route.Root().AnyStringAtLeastOnce("name").And().AnyIntOptional("id"), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());
        }

        [Test]
        public void Browser_GetRequest_AcceptsAnyStringAndAnyStringWithRange()
        {
            //Arrange & Act
            var result = SetupRouteResponse("", "/product/macbookpro", Route.Root().AnyStringAtLeastOnce("name").And().AnyStringAtLeastOnce("product", 1, 10), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());
        }

        [TestCase("", "/product/macbookpro")]
        [TestCase("", "/product")]
        public void Browser_GetRequest_AcceptsAnyStringAndOptionalString(string Root, string Path)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, Route.Root().AnyStringAtLeastOnce("name").And().AnyStringOptional("product"), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());
        }

        [Test]
        public void Browser_GetRequest_AcceptsAnyStringAndExactPattern()
        {
            //Arrange & Act
            var result = SetupRouteResponse("", "/product/macbookpro", Route.Root().AnyStringAtLeastOnce("name").And().Exact("product", "macbookpro"), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());

        }

        /********
         * AnyStringWithRange
        ********/

        [Test]
        public void Browser_GetRequest_AcceptsAnyStringWithRangeAndAnyIntWithRange()
        {
            //Arrange & Act
            var result = SetupRouteResponse("", "/product/123", Route.Root().AnyStringAtLeastOnce("name", 1, 7).And().AnyIntAtLeastOnce("id", 1, 3), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());

        }

        [Test]
        public void Browser_GetRequest_AcceptsAnyStringWithRangeAndAnyInt()
        {
            //Arrange & Act
            var result = SetupRouteResponse("", "/product/123", Route.Root().AnyStringAtLeastOnce("name", 1, 7).And().AnyIntAtLeastOnce("id"), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());
        }

        [TestCase("", "/product/123")]
        [TestCase("", "/product")]
        public void Browser_GetRequest_AcceptsAnyStringWithRangeAndOptionalInt(string Root, string Path)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, Route.Root().AnyStringAtLeastOnce("name", 1, 7).And().AnyIntOptional("id"), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());
        }

        [Test]
        public void Browser_GetRequest_AcceptsAnyStringWithRangeAndAnyString()
        {
            //Arrange & Act
            var result = SetupRouteResponse("", "/product/macbookpro", Route.Root().AnyStringAtLeastOnce("name", 1, 7).And().AnyStringAtLeastOnce("id"), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());
        }

        [TestCase("", "/product/macbookpro")]
        [TestCase("", "/product")]
        public void Browser_GetRequest_AcceptsAnyStringWithRangeAndOptionalString(string Root, string Path)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, Route.Root().AnyStringAtLeastOnce("name", 1, 7).And().AnyStringOptional("id"), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());

        }

        [Test]
        public void Browser_GetRequest_AcceptsAnyStringWithRangeAndExactPattern()
        {
            //Arrange & Act
            var result = SetupRouteResponse("", "/product/macbookpro", Route.Root().AnyStringAtLeastOnce("name", 1, 7).And().Exact("id", "macbookpro"), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());
        }

        /********
         * OptionalString
        ********/

        [TestCase("", "/product/123")]
        [TestCase("", "/123")]
        public void Browser_GetRequest_AcceptsOptionalStringAndAnyIntWithRange(string Root, string Path)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, Route.AnyStringOptional("name").And().AnyIntAtLeastOnce("id", 1, 3), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());
        }

        [TestCase("", "/product/123")]
        [TestCase("", "/123")]
        public void Browser_GetRequest_AcceptsOptionalStringAndAnyInt(string Root, string Path)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, Route.AnyStringOptional("name").And().AnyIntAtLeastOnce("id"), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());

        }

        public class RequestParameterTestClass
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData(new object[] { "", "/product", Route.AnyStringOptional("name").AnyIntOptional("id"), "id", "" });
                    yield return new TestCaseData(new object[] { "", "/product", Route.AnyStringOptional("name").AnyIntOptional("id"), "name", "product" });
                    yield return new TestCaseData(new object[] { "", "/123", Route.AnyStringOptional("name").AnyIntOptional("id"), "id", "" });
                    yield return new TestCaseData(new object[] { "", "/123", Route.AnyStringOptional("name").AnyIntOptional("id"), "name", "123" });

                    yield return new TestCaseData(new object[] { "", "/123/456", Route.Root().AnyIntAtLeastOnce("id").And().AnyIntOptional("anotherid"), "anotherid", "456" });
                    yield return new TestCaseData(new object[] { "", "/123", Route.Root().AnyIntAtLeastOnce("id").And().AnyIntOptional("anotherid"), "anotherid", "" });
                    yield return new TestCaseData(new object[] { "", "/123/macbookpro", Route.Root().AnyIntAtLeastOnce("id").And().AnyStringOptional("anotherid"), "anotherid", "macbookpro" });
                    yield return new TestCaseData(new object[] { "", "/123", Route.Root().AnyIntAtLeastOnce("id").And().AnyStringOptional("anotherid"), "anotherid", "" });

                    yield return new TestCaseData(new object[] { "", "/123/456", Route.Root().AnyIntAtLeastOnce("id", 1, 3).AnyIntOptional("anotherid"), "anotherid", "456" });
                    yield return new TestCaseData(new object[] { "", "/123", Route.Root().AnyIntAtLeastOnce("id", 1, 3).AnyIntOptional("anotherid"), "anotherid", "" });
                    yield return new TestCaseData(new object[] { "", "/123/macbookpro", Route.Root().AnyIntAtLeastOnce("id", 1, 3).AnyStringOptional("anotherid"), "anotherid", "macbookpro" });
                    yield return new TestCaseData(new object[] { "", "/123", Route.Root().AnyIntAtLeastOnce("id", 1, 3).AnyStringOptional("anotherid"), "anotherid", "" });

                    yield return new TestCaseData(new object[] { "", "/123", Route.AnyIntOptional("id").And().AnyIntAtLeastOnce("musthaveid", 1, 3), "musthaveid", "123" });
                    yield return new TestCaseData(new object[] { "", "/456", Route.AnyIntOptional("id").And().AnyIntAtLeastOnce("musthaveid", 1, 3), "musthaveid", "456" });
                    yield return new TestCaseData(new object[] { "", "/123/456", Route.AnyIntOptional("id").And().AnyIntAtLeastOnce("musthaveid", 1, 3), "id", "123" });
                    yield return new TestCaseData(new object[] { "", "/123/macbookpro", Route.AnyIntOptional("id").And().AnyStringAtLeastOnce("musthaveid", 1, 10), "id", "123" });
                    yield return new TestCaseData(new object[] { "", "/macbookpro", Route.AnyIntOptional("id").And().AnyStringAtLeastOnce("musthaveid", 1, 10), "musthaveid", "macbookpro" });
                    yield return new TestCaseData(new object[] { "", "/123/macbookpro", Route.AnyIntOptional("id").And().AnyStringAtLeastOnce("musthaveid"), "id", "123" });
                    yield return new TestCaseData(new object[] { "", "/macbookpro", Route.AnyIntOptional("id").And().AnyStringAtLeastOnce("musthaveid"), "musthaveid", "macbookpro" });
                    yield return new TestCaseData(new object[] { "", "/123/macbookpro", Route.AnyIntOptional("id").AnyStringOptional("musthaveid"), "id", "123" });
                    yield return new TestCaseData(new object[] { "", "/macbookpro", Route.AnyIntOptional("id").AnyStringOptional("musthaveid"), "musthaveid", "macbookpro" });

                    yield return new TestCaseData(new object[] { "", "/macbookpro/123", Route.Root().Exact("id", "macbookpro").And().AnyIntOptional("musthaveid"), "musthaveid", "123" });
                    yield return new TestCaseData(new object[] { "", "/macbookpro", Route.Root().Exact("id", "macbookpro").And().AnyIntOptional("musthaveid"), "musthaveid", "" });
                    yield return new TestCaseData(new object[] { "", "/macbookpro/antiglare", Route.Root().Exact("id", "macbookpro").And().AnyStringOptional("musthaveid"), "musthaveid", "antiglare" });
                    yield return new TestCaseData(new object[] { "", "/macbookpro", Route.Root().Exact("id", "macbookpro").And().AnyStringOptional("musthaveid"), "musthaveid", "" });
                }
            }
        }

        [Test, TestCaseSource(typeof(RequestParameterTestClass), "TestCases")]
        public void Browser_GetRequest_RequestParametersPopulated(string Root, string Path, RouteParameters Route, string Param, string Expected)
        {
            var result = SetupRouteResponse(Root, Path, Route, "MatchIt");

            Assert.AreEqual(Expected, (string)result.Context.Parameters[Param]);
        }

        [TestCase("", "/product/123")]
        [TestCase("", "/product")]
        [TestCase("", "/123")]
        [TestCase("", "/")]
        public void Browser_GetRequest_AcceptsOptionalStringAndOptionalInt(string Root, string Path)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, Route.AnyStringOptional("name").AnyIntOptional("id"), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());

        }

        [TestCase("", "/product/macbookpro")]
        [TestCase("", "/macbookpro")]
        public void Browser_GetRequest_AcceptsOptionalStringAndAnyStringWithRange(string Root, string Path)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, Route.AnyStringOptional("name").And().AnyStringAtLeastOnce("id", 1, 10), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());

        }

        [TestCase("", "/product/macbookpro")]
        [TestCase("", "/macbookpro")]
        public void Browser_GetRequest_AcceptsOptionalStringAndAnyString(string Root, string Path)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, Route.AnyStringOptional("name").And().AnyStringAtLeastOnce("id"), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());
        }

        [TestCase("", "/product/macbookpro")]
        [TestCase("", "/macbookpro")]
        public void Browser_GetRequest_AcceptsOptionalStringAndExactPattern(string Root, string Path)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, Route.AnyStringOptional("name").And().Exact("id", "macbookpro"), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());

        }

        /********
         * AnyInt
        *********/

        [Test]
        public void Browser_GetRequest_AcceptsAnyIntAndAnyIntWithRange()
        {
            //Arrange & Act
            var result = SetupRouteResponse("", "/123/456", Route.Root().AnyIntAtLeastOnce("id").And().AnyIntAtLeastOnce("anotherid", 1, 7), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());
        }

        [TestCase("", "/123/456")]
        [TestCase("", "/123")]
        public void Browser_GetRequest_AcceptsAnyIntAndOptionalInt(string Root, string Path)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, Route.Root().AnyIntAtLeastOnce("id").AnyIntOptional("anotherid"), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());

        }

        [Test]
        public void Browser_GetRequest_AcceptsAnyIntAndAnyStringWithRange()
        {
            //Arrange & Act
            var result = SetupRouteResponse("", "/123/macbookpro", Route.Root().AnyIntAtLeastOnce("id").And().AnyStringAtLeastOnce("anotherid", 1, 10), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());
        }

        [Test]
        public void Browser_GetRequest_AcceptsAnyIntAndAnyString()
        {
            //Arrange & Act
            var result = SetupRouteResponse("", "/123/macbookpro", Route.Root().AnyIntAtLeastOnce("id").And().AnyStringAtLeastOnce("anotherid"), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());

        }

        [TestCase("", "/123/macbookpro")]
        [TestCase("", "/123")]
        public void Browser_GetRequest_AcceptsAnyIntAndOptionalString(string Root, string Path)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, Route.Root().AnyIntAtLeastOnce("id").AnyStringOptional("anotherid"), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());
        }

        [Test]
        public void Browser_GetRequest_AcceptsAnyIntAndExactPattern()
        {
            //Arrange & Act
            var result = SetupRouteResponse("", "/123/macbookpro", Route.Root().AnyIntAtLeastOnce("id").And().Exact("anotherid", "macbookpro"), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());
        }

        /********
         * AnyIntWithRange
        ********/

        [Test]
        public void Browser_GetRequest_AcceptsAnyIntWithRangeAndAnyInt()
        {
            //Arrange & Act
            var result = SetupRouteResponse("", "/123/456", Route.Root().AnyIntAtLeastOnce("id", 1, 3).And().AnyIntAtLeastOnce("anotherid"), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());
        }

        [TestCase("", "/123/456")]
        [TestCase("", "/123")]
        public void Browser_GetRequest_AcceptsAnyIntWithRangeAndOptionalInt(string Root, string Path)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, Route.Root().AnyIntAtLeastOnce("id", 1, 3).AnyIntOptional("anotherid"), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());
        }

        [Test]
        public void Browser_GetRequest_AcceptsAnyIntWithRangeAndAnyStringWithRange()
        {
            //Arrange & Act
            var result = SetupRouteResponse("", "/123/macbookpro", Route.Root().AnyIntAtLeastOnce("id", 1, 3).And().AnyStringAtLeastOnce("anotherid", 1, 10), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());
        }

        [Test]
        public void Browser_GetRequest_AcceptsAnyIntWithRangeAndAnyString()
        {
            //Arrange & Act
            var result = SetupRouteResponse("", "/123/macbookpro", Route.Root().AnyIntAtLeastOnce("id", 1, 3).And().AnyStringAtLeastOnce("anotherid"), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());
        }

        [TestCase("", "/123/macbookpro")]
        [TestCase("", "/123")]
        public void Browser_GetRequest_AcceptsAnyIntWithRangeAndOptionalString(string Root, string Path)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, Route.Root().AnyIntAtLeastOnce("id", 1, 3).AnyStringOptional("anotherid"), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());
        }

        [Test]
        public void Browser_GetRequest_AcceptsAnyIntWithRangeAndExactPattern()
        {
            //Arrange & Act
            var result = SetupRouteResponse("", "/123/macbookpro", Route.Root().AnyIntAtLeastOnce("id", 1, 3).And().Exact("anotherid", "macbookpro"), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());

        }

        /********
         * OptionalInt
         ********/

        [TestCase("", "/123/456")]
        [TestCase("", "/123")]
        public void Browser_GetRequest_AcceptsAnyIntOptionalAndAnyIntWithRange(string Root, string Path)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, Route.AnyIntOptional("id").And().AnyIntAtLeastOnce("musthaveid", 1, 3), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());

        }

        [TestCase("", "/123/456")]
        [TestCase("", "/123")]
        public void Browser_GetRequest_AcceptsAnyIntOptionalAndAnyInt(string Root, string Path)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, Route.AnyIntOptional("id").And().AnyIntAtLeastOnce("musthaveid", 1, 3), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());
        }

        [TestCase("", "/123/macbookpro")]
        [TestCase("", "/macbookpro")]
        public void Browser_GetRequest_AcceptsAnyIntOptionalAndAnyStringWithRange(string Root, string Path)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, Route.AnyIntOptional("id").And().AnyStringAtLeastOnce("musthaveid", 1, 10), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());
        }

        [TestCase("", "/123/macbookpro")]
        [TestCase("", "/macbookpro")]
        public void Browser_GetRequest_AcceptsAnyIntOptionalAndAnyString(string Root, string Path)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, Route.AnyIntOptional("id").And().AnyStringAtLeastOnce("musthaveid"), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());
        }

        [TestCase("", "/123/macbookpro")]
        [TestCase("", "/macbookpro")]
        public void Browser_GetRequest_AcceptsAnyIntOptionalAndOptionalString(string Root, string Path)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, Route.AnyIntOptional("id").AnyStringOptional("musthaveid"), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());

        }

        [TestCase("", "/123/macbookpro")]
        [TestCase("", "/macbookpro")]
        public void Browser_GetRequest_AcceptsAnyIntOptionalAndExactPattern(string Root, string Path)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, Route.AnyIntOptional("id").And().Exact("musthaveid", "macbookpro"), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());
        }

        /********
         * ExactPattern
        ********/

        [Test]
        public void Browser_GetRequest_AcceptsExactPatternAndAnyInt()
        {
            //Arrange & Act
            var result = SetupRouteResponse("", "/macbookpro/123", Route.Root().Exact("id", "macbookpro").And().AnyIntAtLeastOnce("musthaveid"), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());
        }

        [Test]
        public void Browser_GetRequest_AcceptsExactPatternAndAnyIntWithRange()
        {
            //Arrange & Act
            var result = SetupRouteResponse("", "/macbookpro/123", Route.Root().Exact("id", "macbookpro").And().AnyIntAtLeastOnce("musthaveid", 1, 3), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());

        }


        [TestCase("", "/macbookpro/123")]
        [TestCase("", "/macbookpro")]
        public void Browser_GetRequest_AcceptsExactPatternAndOptionalInt(string Root, string Path)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, Route.Root().Exact("id", "macbookpro").And().AnyIntOptional("musthaveid"), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());

        }

        [Test]
        public void Browser_GetRequest_AcceptsExactPatternAndAnyStringWithRange()
        {
            //Arrange & Act
            var result = SetupRouteResponse("", "/macbookpro/antiglare", Route.Root().Exact("id", "macbookpro").And().AnyStringAtLeastOnce("musthaveid", 1, 10), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());

        }

        [Test]
        public void Browser_GetRequest_AcceptsExactPatternAndAnyString()
        {
            //Arrange & Act
            var result = SetupRouteResponse("", "/macbookpro/antiglare", Route.Root().Exact("id", "macbookpro").And().AnyStringAtLeastOnce("musthaveid"), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());
        }

        [TestCase("", "/macbookpro/antiglare")]
        [TestCase("", "/macbookpro")]
        public void Browser_GetRequest_AcceptsExactPatternAndOptionalString(string Root, string Path)
        {
            //Arrange & Act
            var result = SetupRouteResponse(Root, Path, Route.Root().Exact("id", "macbookpro").And().AnyStringOptional("musthaveid"), "MatchIt");

            //Assert
            Assert.AreEqual("MatchIt", result.Body.AsString());
        }

    }
}
