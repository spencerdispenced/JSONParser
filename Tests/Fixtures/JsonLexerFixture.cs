
namespace Tests.Fixtures
{
    public class JsonLexerFixture
    {
        public Lexer JsonLexer { get; }

        public JsonLexerFixture()
        {
            JsonLexer = new Lexer();
        }
    }
}