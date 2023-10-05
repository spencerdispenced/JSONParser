
namespace Tests
{
    public class ValidatorTests : IClassFixture<JsonLexerFixture>, IClassFixture<JsonParserFixture>
    {
        private readonly ITestOutputHelper _output;
        private readonly JsonLexerFixture _lexer;
        private readonly JsonParserFixture _parser;
        public ValidatorTests(ITestOutputHelper output, JsonLexerFixture lexer, JsonParserFixture parser)
        {
            _output = output;
            _lexer = lexer;
            _parser = parser;
        }

        [Fact]
        public void Validator_ValidEmptyJSONFile_ReturnsTrue()
        {
            string file = "../../../Fixtures/testFiles/step1/valid.json";
            Validator validator = new (_lexer.JsonLexer, _parser.JsonParser);

            var result = validator.ValidateJson(file);

            result.Should().Be(true);
        }

        [Fact]
        public void Validator_EmptyFile_ReturnsFalse()
        {
            string file = "../../../Fixtures/testFiles/step1/invalid.json";
            Validator validator = new (_lexer.JsonLexer, _parser.JsonParser);

            var result = validator.ValidateJson(file);

            result.Should().Be(false);
        }

        [Theory]
        [InlineData("../../../Fixtures/testFiles/step2/valid.json")]
        [InlineData("../../../Fixtures/testFiles/step2/valid2.json")]
        public void Validator_SimpleJSONObject_ReturnsTrue(string file)
        {
            Validator validator = new (_lexer.JsonLexer, _parser.JsonParser);

            var result = validator.ValidateJson(file);

            result.Should().Be(true);
        }

        [Theory]
        //[InlineData("../../../Fixtures/testFiles/step2/invalid.json")]
        [InlineData("../../../Fixtures/testFiles/step2/invalid2.json")]
        public void Validator_SimpleInvalidJSONObject_ReturnsFalse(string file)
        {
            Validator validator = new (_lexer.JsonLexer, _parser.JsonParser);

            var result = validator.ValidateJson(file);

            result.Should().Be(false);
        }

        [Theory]
        [InlineData("../../../Fixtures/testFiles/step3/valid.json")]
        public void Validator_MultipleFlatValidValues_ReturnsTrue(string file)
        {
            Validator validator = new (_lexer.JsonLexer, _parser.JsonParser);

            var result = validator.ValidateJson(file);

            result.Should().Be(true);
        }

        [Theory]
        [InlineData("../../../Fixtures/testFiles/step3/invalid.json")]
        public void Validator_MultipleFlatInValidValues_ReturnsFalse(string file)
        {
            Validator validator = new (_lexer.JsonLexer, _parser.JsonParser);

            var result = validator.ValidateJson(file);

            result.Should().Be(false);
        }

        [Theory]
        [InlineData("../../../Fixtures/testFiles/step4/valid.json")]
        [InlineData("../../../Fixtures/testFiles/step4/valid2.json")]
        public void Validator_NestedValidValues_ReturnsTrue(string file)
        {
            Validator validator = new (_lexer.JsonLexer, _parser.JsonParser);

            var result = validator.ValidateJson(file);

            result.Should().Be(true);
        }

        [Theory]
        [InlineData("../../../Fixtures/testFiles/step4/invalid.json")]
        public void Validator_NestedInValidValues_ReturnsFalse(string file)
        {
            Validator validator = new (_lexer.JsonLexer, _parser.JsonParser);

            var result = validator.ValidateJson(file);

            result.Should().Be(false);
        }

        [Theory]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail1.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail2.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail3.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail4.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail5.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail6.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail7.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail8.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail9.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail10.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail11.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail12.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail13.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail14.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail15.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail16.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail17.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail18.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail19.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail20.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail21.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail22.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail23.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail24.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail25.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail26.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail27.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail28.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail29.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail30.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail31.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail32.json")]
        [InlineData("../../../Fixtures/testFiles/testSuite/fail33.json")]
        public void Validator_TestSuiteFail_ReturnsFalse(string file)
        {
            Validator validator = new (_lexer.JsonLexer, _parser.JsonParser);

            var result = validator.ValidateJson(file);

            result.Should().Be(false);
        }

        [Theory]
        [InlineData("../../../Fixtures/testFiles/testSuite/pass1.json")] // Fail
        [InlineData("../../../Fixtures/testFiles/testSuite/pass2.json")] // Fail
        [InlineData("../../../Fixtures/testFiles/testSuite/pass3.json")]
        public void Validator_TestSuitePass_ReturnsTrue(string file)
        {
            Validator validator = new (_lexer.JsonLexer, _parser.JsonParser);

            var result = validator.ValidateJson(file);

            result.Should().Be(true);
        }
    }
}