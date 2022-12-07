namespace EasyMongo {
    public class Scanner {
        private readonly String source;
        private readonly List<Token> tokens = new List<Token>();
        private int current = 0;
        private int start = 0;
        private int line = 1;

        static readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>
        {
            {"and", TokenType.AND},
            {"not", TokenType.NOT},
            {"in", TokenType.IN},
            {"when", TokenType.WHEN},
            {"all", TokenType.ALL},
            {"print", TokenType.PRINT},
            {"using", TokenType.USING},
            {"file", TokenType.FILE}
        };

        public Scanner(String source)
        {
            this.source = source;
        }

        public List<Token> scanTokens()
        {
            while (!isAtEnd())
            {
                // We are at the beginning of the next lexeme
                start = current;
                scanToken();
            }

            tokens.Add(new Token(TokenType.EOF, "", null, line));
            return tokens;
        }

        private void scanToken()
        {
            char c = advance();
            switch (c)
            {
                case ',':
                    addToken(TokenType.COMMA);
                    break;
                case '.':
                    addToken(TokenType.DOT);
                    break;
                case '-':
                    addToken(TokenType.MINUS);
                    break;
                case '+':
                    addToken(TokenType.PLUS);
                    break;
                case '*':
                    addToken(TokenType.STAR);
                    break;
                case '=':
                    addToken(TokenType.EQUAL);
                    break;
                case '<':
                    addToken(match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                    break;
                case '>':
                    addToken(match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                    break;
                // Ignore meaningless characters
                case ' ':
                case '\r':
                case '\t':
                    // Ignore whitespace
                    break;

                case '\n':
                    line++;
                    break;

                // String literals
                case '"':
                    scanString();
                    break;

                // Unexpected character
                default:
                    if (isDigit(c))
                    {
                        number();
                    }
                    else if (isAlpha(c))
                    {
                        identifier();
                    }
                    else
                    {
                        EasyMongo.error(line, "Unexpected character.");
                    }
                    break;
            }
        }

        // Tell if all characters have been consumed
        private Boolean isAtEnd() { return current >= source.Length; }

        // Consume next character in source file and return it
        private char advance() { return source[current++]; }

        // Creates token for current lexeme
        private void addToken(TokenType type) { addToken(type, null); }

        private void addToken(TokenType type, Object? literal)
        {
            String text = source.Substring(start, current - start);
            tokens.Add(new Token(type, text, literal, line));
        }

        // Determine if next char forms a lexeme
        // Ex: "!=" or "!(isAtEnd())"
        private Boolean match(char expected)
        {
            if (isAtEnd() || source[current] != expected) return false;

            current++;
            return true;
        }

        // Find end of comment
        // Like advance() but doesnt consume the character, called a lookahead
        private char peek()
        {
            if (isAtEnd()) return '\0';

            return source[current];
        }

        private char peekNext()
        {
            if (current + 1 > source.Length) return '\0';
            return source[current + 1];
        }

        private void scanString()
        {
            while (peek() != '"' && !isAtEnd())
            {
                if (peek() == '\n') line++;
                advance();
            }

            if (isAtEnd())
            {
                EasyMongo.error(line, "Unterminated string.");
                return;
            }

            // Trim the surrounding quotes
            String value = source.Substring(start + 1, current - start - 1);
            addToken(TokenType.STRING, value);

            advance();
        }

        private Boolean isDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private void number()
        {
            while (isDigit(peek())) advance();

            // Look for a fractional part
            if (peek() == '.' && isDigit(peekNext()))
            {
                // Consume the "."
                advance();

                while (isDigit(peek())) advance();
            }

            addToken(TokenType.NUMBER, Double.Parse(source.Substring(start, current - start)));
        }

        private void identifier()
        {
            while (isAlphaNumeric(peek())) advance();

            String text = source.Substring(start, current - start);
            if (keywords.ContainsKey(text))
            {
                addToken(keywords[text]);
            }
            else
            {
                addToken(TokenType.IDENTIFIER);
            }
        }

        private Boolean isAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                    (c >= 'A' && c <= 'Z') ||
                    c == '_';
        }

        private Boolean isAlphaNumeric(char c)
        {
            return isAlpha(c) || isDigit(c);
        }

        public int Line
        {
            get { return line; }
        }
    }
}