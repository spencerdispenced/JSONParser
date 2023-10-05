
namespace JSONParserNet
{
    public class Parser
    {
        private const char OPEN_BRACE = '{';
        private const char CLOSE_BRACE = '}';
        private const char OPEN_BRAKET = '[';
        private const char CLOSE_BRACKET = ']';
        private const char COMMA = ',';
        private const char COLON = ':';

        public object? Parse(List<object?> tokens)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException(paramName: nameof(tokens), message: "NUll object passed to parser");
            }

            if (tokens.Count == 0)
            {
                throw new ArgumentException(message: "Empty object passed to parser");
            }
            return InternalParse(0, tokens).Value;
        }

        private (int Increment, object? Value) InternalParse(int position, List<object?> tokens)
        {
            int i = position;
            if (tokens[i] == null || tokens[i]!.GetType() != typeof(char))
            {
                return (1, tokens[i]);
            }

            if ((char?)tokens[i] == OPEN_BRAKET)
            {
                var (increment, arr) =  ParseArray(position+1, tokens);
                return (increment + 1, arr);
            }

            if ((char?)tokens[i] == OPEN_BRACE)
            {
                var (increment, obj) = ParseObject(position+1, tokens);
                return (increment + 1, obj);
            }
            else
            {
                throw new ArgumentException($"Unable to parse json, Invalid token {tokens[i]}");
            }
        }

        private (int Increment, object? Value) ParseArray(int position, List<object?> tokens)
        {
            List<object?> jsonArray = new();

            int i = position;
            while (i < tokens.Count)
            {
                var token = tokens[i];
                if (token == null)
                {
                    jsonArray.Add(token);
                    i++;
                    continue;
                }

                if (token.Equals(COMMA))
                {
                    i++;
                    continue;
                }
                if (token.Equals(CLOSE_BRACKET))
                {
                    int increment = i - position + 1;
                    return (increment, jsonArray);
                }

                var (inc, value) = InternalParse(i, tokens);
                jsonArray.Add(value);
                i += inc;
            }

            throw new ArgumentException("No Closing Bracket");
        }

        private (int Increment, object? Value) ParseObject(int position, List<object?> tokens)
        {
            Dictionary<string, object?> jsonObject = new();

            int i = position;
            while (i < tokens.Count)
            {
                var token = tokens[i];

                if (token == null) break;

                if (token.GetType() == typeof(string))
                {
                    string? key = token.ToString();

                    if (key == null) break;

                    i++;
                    if ((char?)tokens[i] != COLON)
                    {
                        throw new Exception("Invalid object, expected colon after key");
                    }
                    i++;

                    var (inc, value) = InternalParse(i, tokens);
                    i += inc;
                    jsonObject[key] = value;
                    continue;
                }

                if (token.Equals(COMMA))
                {
                    i++;
                    if (tokens[i] != null && tokens[i]!.GetType() != typeof(string))
                    {
                        throw new Exception("Invalid object, Trailing comma");
                    }
                    continue;
                }

                if (token.Equals(CLOSE_BRACE))
                {
                    int increment = i - position + 1;
                    return (increment, jsonObject);
                }

                i++;
            }

            throw new Exception("No Closing Brace");
        }
    }
}