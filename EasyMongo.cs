namespace EasyMongo
{
    class EasyMongo
    {
        private static Boolean hadError = false;
        public static void Main(String[] args)
        {
            runPrompt();
        }

        public static void runPrompt()
        {
            Console.WriteLine("Type q to quit");

            while (true)
            {
                Console.Write(">> ");
                String? line = Console.ReadLine();

                if (line == "q" || line == "quit" || line == "exit" || line == null) break;

                run(line);
                hadError = false;
            }
        }

        public static void run(String source)
        {
            Scanner scanner = new Scanner(source);
            List<Token> tokens = scanner.scanTokens();

            for (int i = 0; i < tokens.Count; i++)
            {
                Console.Write(tokens[i].type + " ");
            }
            Console.WriteLine();

            if(hadError) return;
        }

        public static void error(int line, string message)
        {
            report(line, "", message);
        }

        public static void error(Token token, String message)
        {
            if (token.type == TokenType.EOF)
                report(token.line, " at end", message);
            else
                report(token.line, " at '" + token.lexeme + "'", message);
        }

        private static void report(int line, String where, String message)
        {
            Console.WriteLine("[line " + line + "] Error" + where + ": " + message);
            hadError = true;
        }
    }
}
