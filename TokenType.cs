namespace EasyMongo {
    public enum TokenType
    {
        // Single character tokens
        COMMA, DOT, PLUS, MINUS, STAR,

        // One or two character tokens
        EQUAL, NOT_EQUAL, GREATER, GREATER_EQUAL,
        LESS, LESS_EQUAL,

        // Literals
        STRING, NUMBER, IDENTIFIER,

        // Keywords
        AND, NOT, IN, WHEN, ALL, PRINT, NOT_PRINT, USING, FILE,

        // EOF
        EOF
    }
}