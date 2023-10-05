using System;
using System.Collections.ObjectModel;

namespace JSONParserNet
{
    public class Lexer
    {
        private const int NULL_LEN = 4;
        private const int TRUE_LEN = 4;
        private const int FALSE_LEN = 5;
        private const char QUOTE = '"';
        private const char OPEN_BRACE = '{';
        private const char CLOSE_BRACE = '}';
        private const char OPEN_BRAKET = '[';
        private const char CLOSE_BRACKET = ']';
        private const char COMMA = ',';
        private const char COLON = ':';
        private static readonly IList<char> SYNTAX = new ReadOnlyCollection<char>(new List<char>
        {
            OPEN_BRACE, CLOSE_BRACE, OPEN_BRAKET, CLOSE_BRACKET, COMMA, COLON
        });
        private static readonly IList<char> WHITESPACE = new ReadOnlyCollection<char>(new List<char>
        {
            ' ', '\r', '\t', '\n', '\b'
        });

        public List<object?> LexTokens(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw new ArgumentException("No input received");

            List<object?> tokens = new();
            for (int i = 0; i < json.Length; i++)
            {
                char c = json[i];

                if (WHITESPACE.Contains(c))
                    continue;

                if (c == 'n')
                {
                    var (token, increment) = LexNull(i, json);
                    tokens.Add(token);
                    i += increment;
                    continue;
                }

                if (c is 't' or 'f')
                {
                    var (token, increment) = LexBool(i, json);
                    tokens.Add(token);
                    i += increment;
                    continue;
                }

                if (c is '"')
                {
                    var (token, increment) = LexString(i, json);
                    tokens.Add(token);
                    i += increment;
                    continue;
                }

                if (c >= 48 && c <= 57 )
                {
                    var (token, increment) = LexNumber(i, json);
                    tokens.Add(token);
                    i += increment;
                    continue;
                }

                if (SYNTAX.Contains(c))
                {
                    tokens.Add(c);
                }
                else
                {
                    throw new ArgumentException($"Invalid JSON Token: {c}");
                }
            }

            return tokens;
        }

        private static (object? Token, int increment) LexNull(int position, string json)
        {
            string token = "";
            int i = position;
             while (i < json.Length && json[i] != COMMA && json[i] != CLOSE_BRACKET && json[i] != CLOSE_BRACKET)
            {
                token += json[i];
                i++;
            }

            if (token.Length == NULL_LEN && token.Equals("null"))
                return (null, NULL_LEN-1);

            throw new ArgumentException("Invalid null Token");
        }

        private static (object? Token, int increment) LexBool(int position, string json)
        {
            string token = "";
            int i = position;
            while (i < json.Length && json[i] != COMMA && json[i] != CLOSE_BRACKET && json[i] != CLOSE_BRACE)
            {
                token += json[i];
                i++;
            }

            token = token.Trim();
            if (token.Length == TRUE_LEN && token.Equals("true"))
                return (true, TRUE_LEN-1);

            if (token.Length == FALSE_LEN && token.Equals("false"))
                return (false, FALSE_LEN-1);

            throw new ArgumentException("Invalid bool Token");
        }

        private static (object? Token, int increment) LexString(int position, string json)
        {
            string token = "";
            for (int i = position + 1; i < json.Length; i++)
            {
                char c = json[i];

                if (c == '\\') // Add quote to token, then skip to avoid next check
                {
                    i++;
                    token += json[i];
                    continue;
                }

                if (c == QUOTE)
                {
                    return (token, i - position);
                }
                token += c;
            }

            throw new ArgumentException("Missing closing quote");
        }

        private static (object? Token, int increment) LexNumber(int position, string json)
        {
            string token = "";
            int i = position;
            while (i < json.Length && json[i] != COMMA && json[i] != CLOSE_BRACKET && json[i] != CLOSE_BRACE)
            {
                token += json[i];
                i++;
            }

            if (token.Length > 1 && token[0].Equals('0'))
            {
                throw new ArgumentException("Invalid number token, leading zeros");
            }

            try
            {
                int result = int.Parse(token);
                return (result, token.Length-1);
            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid Number Token");
            }
        }
    }
}