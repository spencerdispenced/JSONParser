using System;

namespace JSONParserNet
{
    public class Validator
    {
        private readonly Lexer _lexer;
        private readonly Parser _parser;

        public Validator(Lexer lexer, Parser parser)
        {
            _lexer = lexer;
            _parser = parser;
        }

        public bool ValidateJson(string file)
        {
            try
            {
                string text = File.ReadAllText(file);
                var tokens = _lexer.LexTokens(text);
                Dictionary<string, object>? result = (Dictionary<string, object>?)_parser.Parse(tokens);

                if (result != null)
                {
                    foreach (var item in result)
                    {
                        Console.WriteLine($"{item.Key} : {item.Value}");
                    }
                }

                return true;
            }
            catch (InvalidCastException)
            {
                try
                {
                    string text = File.ReadAllText(file);
                    var tokens = _lexer.LexTokens(text);
                    List<object>? result = (List<object>?)_parser.Parse(tokens);
                    if (result != null)
                    {
                        foreach (var item in result)
                        {
                            Console.WriteLine($"{item}");
                        }
                    }
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}");
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
                return false;
            }
        }
    }
}