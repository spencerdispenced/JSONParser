
namespace Tests
{
    public class LexerTests : IClassFixture<JsonLexerFixture>
    {
        private readonly JsonLexerFixture _lexer;

        public LexerTests(JsonLexerFixture lexer)
        {
            _lexer = lexer;
        }

        [Fact]
        public void LexTokens_EmptyArguments_ThrowsException()
        {
            Action action = () => _lexer.JsonLexer.LexTokens("");
            action.Should().Throw<ArgumentException>()
            .WithMessage("No input received");
        }

       [Fact]
        public void LexTokens_NullValue_ReturnsNUll()
        {
            var result = _lexer.JsonLexer.LexTokens("null");
            result[0].Should().BeNull();
        }

        [Theory]
        [InlineData("nall")]
        [InlineData("n")]
        [InlineData("nully")]
        public void LexTokens_InvalidNullValue_ThrowsException(string json)
        {
            Action action = () => _lexer.JsonLexer.LexTokens(json);
            action.Should().Throw<ArgumentException>()
            .WithMessage("Invalid null Token");
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        public void LexTokens_ValidBoolValue_ReturnsBool(string json, bool expected)
        {
            var result = _lexer.JsonLexer.LexTokens(json);
            result[0].Should().Be(expected);
        }

        [Theory]
        [InlineData("tree")]
        [InlineData("t")]
        [InlineData("tr")]
        [InlineData("treef")]
        [InlineData("float")]
        [InlineData("f")]
        [InlineData("flo")]
        [InlineData("floating")]
        public void LexTokens_InValidBoolValue_ThrowsException(string json)
        {
            Action action = () => _lexer.JsonLexer.LexTokens(json);
            action.Should().Throw<ArgumentException>().WithMessage("Invalid bool Token");
        }

        [Theory]
        [InlineData("\"\"", "")]
        [InlineData(" \"one\"", "one")]
        [InlineData(" \" two \"", " two ")]
        [InlineData("\"\\\"three\\\"\"", "\"three\"")]
        public void LexTokens_ValidStrings_ReturnsString(string json, string expected)
        {
            var result = _lexer.JsonLexer.LexTokens(json);
            result[0].Should().Be(expected);
        }

        [Fact]
        public void LexTokens_InValidString_ThrowsException()
        {
            Action action = () => _lexer.JsonLexer.LexTokens("\"");
            action.Should().Throw<ArgumentException>()
            .WithMessage("Missing closing quote");
        }

        [Theory]
        [InlineData(" 0", 0)]
        [InlineData("1", 1)]
        [InlineData("10", 10)]
        [InlineData("69", 69)]
        [InlineData("420", 420)]
        public void LexTokens_ValidNumber_ReturnsNumber(string json, int expected)
        {
            var result = _lexer.JsonLexer.LexTokens(json);
            result[0].Should().Be(expected);
        }

        [Fact]
        public void LexTokens_InValidNumber_ThrowsException()
        {
            Action action = () => _lexer.JsonLexer.LexTokens("4x5");
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void LexTokens_FlatArray_ReturnsArrayOfTokens()
        {
            string json = """[ 1, 69, 420 ]""";
            List<object?> expected =  new() { '[', 1, ',', 69, ',',  420, ']'};
            var result = _lexer.JsonLexer.LexTokens(json);
            result.Should().Equal(expected);
        }

        [Fact]
        public void LexTokens_FlatStringArray_ReturnsArrayOfTokens()
        {
            string json = """[ "one" , "two" , "\"three\"" ]""";
            List<object?> expected =  new() { '[', "one", ',',  "two", ',', "\"three\"" , ']'};
            var result = _lexer.JsonLexer.LexTokens(json);
            result.Should().Equal(expected);
        }

        [Fact]
        public void LexTokens_FlatObject_ReturnsArrayOfTokens()
        {
            string json = """{"a": null,"b": true,"c": 360,"d": "Dog"}""";
            List<object?> expected =  new()
            {
                '{', "a", ':', null, ',', "b", ':', true, ',', "c", ':', 360, ',', "d", ':', "Dog", '}'
            };
            var result = _lexer.JsonLexer.LexTokens(json);
            result.Should().Equal(expected);
        }

        [Fact]
        public void LexTokens_NestedObjectWithArrays_ReturnsArrayOfTokens()
        {
            string json = """{"a": null,"b": {"b": [{ "b": true,"c": 360,"dog1": "Zeke"}],"c": 420,"dog2": "Meatball"}}""";
            List<object?> expected =  new()
            {
                '{' , "a", ':', null, ',', "b",
                 ':', '{', "b", ':', '[', '{', "b",
                  ':', true, ',', "c", ':', 360, ',',
                   "dog1",':',  "Zeke", '}', ']', ',',
                    "c", ':', 420, ',', "dog2", ':', "Meatball", '}', '}'
            };
            var result = _lexer.JsonLexer.LexTokens(json);
            result.Should().Equal(expected);
        }

        [Fact]
        public void LexTokens_InValidNestedObjectWithArrays_ThrowsArgumentError()
        {
            string json = """{"a": n,"b": {"b": [{ "b": true,"c": 360,"dog1": "Zeke"}],"c": 420,"dog2": "Meatball"}}""";
            Action action = () => _lexer.JsonLexer.LexTokens(json);
            action.Should().Throw<ArgumentException>()
            .WithMessage("Invalid null Token");
        }

        [Fact]
        public void Lexer_InvalidNestedObject_ThrowsArgumentError()
        {
            Lexer lex = new();
            string json = """{"key": "value","key-n": 101,"key-o": {"inner key": "inner value"},"key-l": ['list value']}""";
            string c = "'";
            Action action = () => lex.LexTokens(json);
            action.Should().Throw<ArgumentException>()
            .WithMessage($"Invalid JSON Token: {c}");
        }
    }
}