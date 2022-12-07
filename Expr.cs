namespace EasyMongo
{
    public abstract class Expr
    {
        internal interface Visitor<R>
        {
            // T VisitLiteralExpression(Expr.LiteralExpression literalExpression);
            R VisitBinaryExpr<T>(Binary expression);
            R VisitGroupingExpr<T>(Grouping expression);
            // T VisitVariableExpression(Expr.VariableExpression expression);
            // T VisitLogicalExpression(Expr.LogicalExpression expression);
        }

        public class Grouping : Expr
        {
            public Grouping(Expr expression)
            {
                this.expression = expression;
            }

            internal override R Accept<R>(Visitor<R> visitor)
            {
                return visitor.VisitGroupingExpr<R>(this);
            }

            public readonly Expr expression;
        }

        public class Binary : Expr
        {
            public Binary(Expr left, Token op, Expr right)
            {
                this.left = left;
                this.op = op;
                this.right = right;
            }

            internal override R Accept<R>(Visitor<R> visitor)
            {
                return visitor.VisitBinaryExpr<R>(this);
            }


            public readonly Expr left;
            public readonly Token op;
            public readonly Expr right;
        }

        internal abstract R Accept<R>(Visitor<R> visitor);
    }
}