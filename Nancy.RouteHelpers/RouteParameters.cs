using System.Text;

namespace Nancy.RouteHelpers
{
    public class RouteParameters
    {
        private StringBuilder builder;
        public StringBuilder RouteBuilder
        {
            get
            {
                if (builder == null)
                    builder = new StringBuilder();

                return builder;
            }
        }

        
        public static implicit operator string(RouteParameters f) { return f.RouteBuilder.ToString(); }

        
        public RouteParameters AnyIntAtLeastOnce(string NamedGroup, int lengthStart, int lengthEnd)
        {
            RouteBuilder.Append(@"(?<" + NamedGroup + @">[\d]{" + lengthStart + "," + lengthEnd + "})");
            return this;
        }


        public RouteParameters AnyIntAtLeastOnce(string NamedGroup)
        {
            RouteBuilder.Append(@"(?<" + NamedGroup + @">[\d]+)");
            return this;
        }

        
        public RouteParameters AnyIntOptional(string NamedGroup)
        {
            RouteBuilder.Append(@"/?(?<" + NamedGroup + @">[\d]*)");
            return this;
        }
        
        public RouteParameters And()
        {
            Root();
            return this;
        }
        
        public RouteParameters Root()
        {
            RouteBuilder.Append("/");
            return this;
        }

        public RouteParameters AnyStringOptional(string NamedGroup)
        {
            RouteBuilder.Append(@"/?(?<" + NamedGroup + @">[\S]*)");
            
            return this;
        }

        public RouteParameters AnyStringAtLeastOnce(string NamedGroup)
        {
            RouteBuilder.Append(@"(?<" + NamedGroup + @">[\S]+)");

            return this;
        }

        public RouteParameters AnyStringAtLeastOnce(string NamedGroup, int lengthStart, int lengthEnd)
        {
            RouteBuilder.Append(@"(?<" + NamedGroup + @">[\S]{" + lengthStart + "," + lengthEnd + "})");

            return this;
        }

        public RouteParameters Exact(string NamedGroup, string Exact)
        {
            RouteBuilder.Append(@"(?<" + NamedGroup + ">" + Exact + ")");

            return this;
        }
    }
}
