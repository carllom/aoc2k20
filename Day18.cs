using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace aoc2k20
{
    class Day18
    {
        private static readonly string[] Data = File.ReadAllLines("data/18-maths.txt");
        public static void Task1()
        {
            var result = 0L;
            foreach (var expr in Data)
            {
                var tokens = Tokenize(expr);
                var tree = ParseExpression(null, new Queue<Token>(tokens));
                result += tree.Eval();
            }
            Console.WriteLine($"Task #1 result: {result}");
        }

        public static IEnumerable<Token> Tokenize(string expression)
        {
            var toks = new List<Token>();
            var i = 0;
            while (i < expression.Length)
            {
                switch (expression[i])
                {
                    case ' ': // Whitespace 
                        i++;
                        break;
                    case '(': // Open parenthesis
                        toks.Add(new Token(TokenType.OParen, expression.Substring(i++, 1)));
                        break;
                    case ')': // Close parenthesis
                        toks.Add(new Token(TokenType.CParen, expression.Substring(i++, 1)));
                        break;
                    case '*': // Multiplication operation
                        toks.Add(new Token(TokenType.Mult, expression.Substring(i++, 1)));
                        break;
                    case '+': // Addition operation
                        toks.Add(new Token(TokenType.Add, expression.Substring(i++, 1)));
                        break;
                    default: // Numeric
                        var number = new StringBuilder();
                        while (i < expression.Length && char.IsDigit(expression[i])) number.Append(expression[i++]);
                        toks.Add(new Token(TokenType.Number, number.ToString()));
                        break;
                }
            }
            return toks;
        }

        /// <summary>
        /// An expression is unary (subexpression or literal)
        /// or binary (expression operation expression)
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public static Expr ParseExpression(Expr op1, Queue<Token> tokens)
        {
            // Start case - parse 1st operand
            op1 ??= ParseOperand(tokens);

            // If no tokens left we have a unary expression and are done
            if (tokens.Count == 0) return op1;

            // If we still have token we expect a binary operation
            var oper = ParseOperation(tokens);
            Expr op2 = ParseOperand(tokens);

            // Evaluate left side first
            return ParseExpression(new Operation { left = op1, op = oper, right = op2 }, tokens);
        }

        /// <summary>
        /// Consume a (valid) arithmetic operation token
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public static Token ParseOperation(Queue<Token> tokens)
        {
            var oper = tokens.Dequeue();
            // Next token is expected to be an arithmetic operation
            if (oper.type != TokenType.Add && oper.type != TokenType.Mult) throw new NotSupportedException($"Syntax error: Expected arithmetic operation, got {oper}");
            return oper;
        }

        /// <summary>
        /// Consume an operand expression (subexpression or literal)
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public static Expr ParseOperand(Queue<Token> tokens)
        {
            var opTok = tokens.Dequeue();

            Expr opExpr = null;
            // Subexpression
            if (opTok.type == TokenType.OParen)
            {
                var pCount = 1;
                var subExpr = new Queue<Token>();
                while (tokens.Count > 0)
                {
                    var sTok = tokens.Dequeue();
                    if (sTok.type == TokenType.OParen) pCount++;
                    else if (sTok.type == TokenType.CParen) pCount--;
                    if (pCount == 0) break;
                    subExpr.Enqueue(sTok);
                }
                if (pCount > 0) throw new ArgumentException("Syntax error: Unmatched parenthesis");
                opExpr = ParseExpression(null, subExpr);
            }
            else if (opTok.type == TokenType.Number)
            {
                opExpr = new Literal() { lit = opTok };
            }
            return opExpr;
        }

        public static void Task2()
        {
            var result = 0L;
            foreach (var expr in Data)
            {
                var tokens = Tokenize(expr);
                var tree = ParseExpression2(null, new Queue<Token>(tokens));
                result += tree.Eval();
            }
            Console.WriteLine($"Task #1 result: {result}");
        }

        public static Expr ParseExpression2(Expr op1, Queue<Token> tokens)
        {
            // Start case - parse 1st operand
            op1 ??= ParseOperand2(tokens);

            // If no tokens left we have a unary expression and are done
            if (tokens.Count == 0) return op1;

            // If we still have token we expect a binary operation
            var oper = ParseOperation(tokens);
            Expr op2 = ParseOperand2(tokens);

            if (tokens.Count == 0) // This is the last operation
                return new Operation { left = op1, op = oper, right = op2 };

            if (oper.type == TokenType.Add || tokens.Peek().type == TokenType.Mult) // Left has precedence
                return ParseExpression2(new Operation { left = op1, op = oper, right = op2 }, tokens);
            else // Right has precedence
                return new Operation { left = op1, op = oper, right = ParseExpression2(op2, tokens) };
        }

        public static Expr ParseOperand2(Queue<Token> tokens)
        {
            var opTok = tokens.Dequeue();

            Expr opExpr = null;
            // Subexpression
            if (opTok.type == TokenType.OParen)
            {
                var pCount = 1;
                var subExpr = new Queue<Token>();
                while (tokens.Count > 0)
                {
                    var sTok = tokens.Dequeue();
                    if (sTok.type == TokenType.OParen) pCount++;
                    else if (sTok.type == TokenType.CParen) pCount--;
                    if (pCount == 0) break;
                    subExpr.Enqueue(sTok);
                }
                if (pCount > 0) throw new ArgumentException("Syntax error: Unmatched parenthesis");
                opExpr = ParseExpression2(null, subExpr);
            }
            else if (opTok.type == TokenType.Number)
            {
                opExpr = new Literal() { lit = opTok };
            }
            return opExpr;
        }
    }

    public enum TokenType
    {
        Number,
        OParen,
        CParen,
        Mult,
        Add
    }

    public struct Token
    {
        public readonly TokenType type;
        public readonly string value;
        public Token(TokenType type, string value) { this.type = type; this.value = value; }
        public override string ToString()
        {
            return $"{value} ({type})";
        }
    }

    public abstract class Expr {
        public abstract long Eval();
    }

    public class Operation : Expr
    {
        public Expr left;
        public Token op;
        public Expr right;
        public override long Eval()
        {
            switch(op.type)
            {
                case TokenType.Add:
                    return left.Eval() + right.Eval();
                case TokenType.Mult:
                    return left.Eval() * right.Eval();
                default:
                    throw new NotSupportedException($"Unsupported operation token {op}");
            }
        }
        public override string ToString()
        {
            return $"[{left}] {op.value} [{right}]";
        }
    }

    public class Literal : Expr
    {
        public Token lit;
        public override long Eval() { return long.Parse(lit.value); }
        public override string ToString()
        {
            return lit.value;
        }
    }
}
