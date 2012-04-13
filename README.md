# Nancy.RouteHelpers
## Introduction 

**Welcome to Nancy.RouteHelpers!**

This library allows you to write pretty [NancyFX][2] routes without nasty looking regex strings in your routes.

    public class HomeModule : BaseModule
    {
        public DinnerModule()
        {
            Get[RouteParameters.AnyInt()] = parameters =>
            {
                return View["Index"];
            };
        }
    }

[2]: http://nancyfx.org/