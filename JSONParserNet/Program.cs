using JSONParserNet;

string file = args[0];
Lexer lexer = new ();
Parser parser = new ();
Validator validator = new (lexer, parser);

var result = validator.ValidateJson(file);

System.Console.WriteLine(result);
