
namespace Tests
{
    public class ParserTests : IClassFixture<JsonLexerFixture>, IClassFixture<JsonParserFixture>
    {
        private readonly ITestOutputHelper _testoutputHelper;
        private readonly JsonLexerFixture _lexer;
        private readonly JsonParserFixture _parser;

        public ParserTests(ITestOutputHelper testOutputHelper, JsonLexerFixture lexer, JsonParserFixture parser)
        {
            _testoutputHelper = testOutputHelper;
            _lexer = lexer;
            _parser = parser;
        }

        [Fact]
        public void Parser_ParseNUll_ThrowsArgumentNullException()
        {
            Action action = () => _parser.JsonParser.Parse(null!);

            action.Should().Throw<ArgumentNullException>()
            .WithMessage("NUll object passed to parser (Parameter 'tokens')");
        }

        [Fact]
        public void Parser_Empty_ThrowsArgumentException()
        {
            List<object?> tokens = new();

            Action action = () => _parser.JsonParser.Parse(tokens);

            action.Should().Throw<ArgumentException>()
            .WithMessage("Empty object passed to parser");
        }

        [Theory]
        [InlineData("null", null)]
        [InlineData("101", 101)]
        [InlineData("true", true)]
        [InlineData("\"Dog\"", "Dog")]
        public void Parser_Primatives_ReturnsPrimitiveTypes(string json, object? expected)
        {
            var tokens = _lexer.JsonLexer.LexTokens(json);

            var result = _parser.JsonParser.Parse(tokens);

            result.Should().Be(expected);
        }

        [Fact]
        public void Parser_NoClosingBracket_ThrowsArgumentError()
        {
            var tokens = _lexer.JsonLexer.LexTokens("[");

            Action action = () => _parser.JsonParser.Parse(tokens);

            action.Should().Throw<ArgumentException>()
            .WithMessage("No Closing Bracket");
        }

        [Theory]
        [InlineData("]")]
        [InlineData("}")]
        public void Parser_InvalidToken_ThrowsArgumentError(string token)
        {
            var tokens = _lexer.JsonLexer.LexTokens(token);

            Action action = () => _parser.JsonParser.Parse(tokens);

            action.Should().Throw<ArgumentException>()
            .WithMessage($"Unable to parse json, Invalid token {token}");
        }

        [Fact]
        public void Parser_EmptyArray_ReturnsEmptyArray()
        {
            var tokens = _lexer.JsonLexer.LexTokens("[]");

            var result = _parser.JsonParser.Parse(tokens);
            var values = Assert.IsType<List<object?>>(result);

            values.Should().BeEmpty();
        }

        [Fact]
        public void Parser_SingleItemArray_ReturnsEmptyArray()
        {
            var tokens = _lexer.JsonLexer.LexTokens("[\"Dog\"]");

            List<object?> expected =  new() {"Dog"};

            var result = _parser.JsonParser.Parse(tokens);
            var values = Assert.IsType<List<object?>>(result);

            values.Should().Equal(expected);
        }

        [Fact]
        public void Parser_FlatNumberArray_ReturnsNumberArray()
        {
            var tokens = _lexer.JsonLexer.LexTokens("[ 1 , 69 , 420 ]");
            List<object?> expected =  new() {1, 69, 420};

            var result = _parser.JsonParser.Parse(tokens);
            var values = Assert.IsType<List<object?>>(result);

            values.Should().Equal(expected);
        }
        [Fact]
        public void Parser_FlatBoolArray_ReturnsBoolArray()
        {
            var tokens = _lexer.JsonLexer.LexTokens("[true, false]");
            List<object?> expected =  new() {true, false};

            var result = _parser.JsonParser.Parse(tokens);
            var values = Assert.IsType<List<object?>>(result);

            values.Should().Equal(expected);
        }

        [Fact]
        public void Parser_FlatStringArray_ReturnStringsArray()
        {
            var tokens = _lexer.JsonLexer.LexTokens("[ \"one\" , \"two\" , \"\\\"three\\\"\" ]");
            List<object?> expected =  new() {"one", "two", "\"three\""};

            var result = _parser.JsonParser.Parse(tokens);
            var values = Assert.IsType<List<object?>>(result);

            values.Should().Equal(expected);
        }

        [Fact]
        public void Parser_FlatNullArray_ReturnNullArray()
        {
            var tokens = _lexer.JsonLexer.LexTokens("[ null, null]");
            List<object?> expected =  new() {null, null};

            var result = _parser.JsonParser.Parse(tokens);
            var values = Assert.IsType<List<object?>>(result);

            values.Should().Equal(expected);
        }

        [Fact]
        public void Parser_NestedNumberArray_ReturnsNestedNumberArray()
        {
            var tokens = _lexer.JsonLexer.LexTokens("[ 1 , [69 , 420], 35]");
            List<object?> expectedOuter =  new() {1, 35};
            List<object?> expectedInner = new() {69, 420};
            expectedOuter.Insert(1, expectedInner);

            var result = _parser.JsonParser.Parse(tokens);
            var outerValues = Assert.IsType<List<object?>>(result);
            var innerValues = Assert.IsType<List<object?>>(outerValues[1]);

            outerValues[0].Should().Be(1);
            outerValues[2].Should().Be(35);
            innerValues.Should().Equal(expectedInner);
        }

        [Fact]
        public void Parser_NestedMixedArray_ReturnsNestedMixedArray()
        {
            var tokens = _lexer.JsonLexer.LexTokens("[ 1 , [\"Dog\", true , false ], 35]");
            List<object?> expectedOuter =  new() {1, 35};
            List<object?> expectedInner =  new() {"Dog", true, false};
            expectedOuter.Insert(1, expectedInner);

            var result = _parser.JsonParser.Parse(tokens);
            var outerValues = Assert.IsType<List<object?>>(result);
            var innerValues = Assert.IsType<List<object?>>(outerValues[1]);

            outerValues[0].Should().Be(1);
            outerValues[2].Should().Be(35);
            innerValues.Should().Equal(expectedInner);
        }

        [Fact]
        public void Parser_EmptyObject_ReturnsEmptyObject()
        {
            var tokens = _lexer.JsonLexer.LexTokens("{}");

            var result = _parser.JsonParser.Parse(tokens);
            var value = Assert.IsType<Dictionary<string, object?>>(result);

            value.Should().BeEmpty();
        }

        [Fact]
        public void Parser_FlatObject_ReturnsDictionary()
        {
            string json = """{"Dog": "Meatball"}""";
            var tokens = _lexer.JsonLexer.LexTokens(json);

            var result = _parser.JsonParser.Parse(tokens);
            var value = Assert.IsType<Dictionary<string, object?>>(result);

            value["Dog"].Should().Be("Meatball");
        }

        [Fact]
        public void Parser_FlatObject_ReturnsJsonObject()
        {
            string json = """{"a": null,"b": true,"c": 360,"d": "Dog"}""";
            Dictionary<string, object?> expected =  new()
            {
                {"a", null}, {"b", true}, {"c", 360}, {"d", "Dog"}
            };
            var tokens = _lexer.JsonLexer.LexTokens(json);

            var result = _parser.JsonParser.Parse(tokens);
            var value = Assert.IsType<Dictionary<string, object?>>(result);

            value.Should().Equal(expected);
        }

        [Fact]
        public void Parser_ObjectWithEmptyNestedCollections_ReturnsJsonObject()
        {
            string json = """{ "Dog": "Meatball","Age": 9,"Commands Known": {},"Favorite Treats": [] }""";
            var tokens = _lexer.JsonLexer.LexTokens(json);

            var result = _parser.JsonParser.Parse(tokens);
            var value = Assert.IsType<Dictionary<string, object?>>(result);

            value["Dog"].Should().Be("Meatball");
            value["Age"].Should().Be(9);
            var value2 = Assert.IsType<Dictionary<string, object?>>(value["Commands Known"]);
            value2.Should().BeEmpty().And.BeOfType<Dictionary<string,object?>>();
            var value3 = Assert.IsType<List<object?>>(value["Favorite Treats"]);
            value3.Should().BeEmpty().And.BeOfType<List<object?>>();
        }

        [Fact]
        public void Parser_ObjectWithPopulatedNestedCollections_ReturnsJsonObject()
        {
            string json = """{ "Dog": "Meatball","Age": 9,"Commands Known": {"sit": true, "stay": false},"Favorite Treats": ["hotdogs", "bananas"] }""";
            var tokens = _lexer.JsonLexer.LexTokens(json);

            var result = _parser.JsonParser.Parse(tokens);
            var value = Assert.IsType<Dictionary<string, object?>>(result);

            value["Dog"].Should().Be("Meatball");
            value["Age"].Should().Be(9);
            var value2 = Assert.IsType<Dictionary<string, object?>>(value["Commands Known"]);
            value2["sit"].Should().Be(true);
            value2["stay"].Should().Be(false);
            var value3 = Assert.IsType<List<object?>>(value["Favorite Treats"]);
            value3[0].Should().Be("hotdogs");
            value3[1].Should().Be("bananas");
        }
    }
}