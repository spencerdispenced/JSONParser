  
namespace Tests.Fixtures
{ 
    public class JsonParserFixture
    {
        public Parser JsonParser { get; }

        public JsonParserFixture()
        {
            JsonParser = new Parser();
        }
    }
}