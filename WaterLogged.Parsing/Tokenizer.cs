﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WaterLogged.Parsing.Tokens;

namespace WaterLogged.Parsing
{
    public class Tokenizer
    {
        private int _index;

        //$ means interpret as method
        //# means interpret as number (or bool)
        //% means interpret as string
        //Nothing means interpret as string

        public Tokens.Token[] EvaluateTokens(string expression)
        {
            List<Token> tokens = new List<Token>();

            for (_index = 0; _index < expression.Length; _index++)
            {
                char curChar = expression[_index];
                if (curChar == '{')
                {
                    tokens.Add(new OpenBraceToken().Init(_index, "{"));
                }
                else if (curChar == '}')
                {
                    tokens.Add(new CloseBraceToken().Init(_index, "}"));
                }
                else if (curChar == '#')
                {
                    tokens.Add(new LogicalToken().Init(_index, "#"));
                }
                else if (curChar == '$')
                {
                    tokens.Add(new InterpolateToken().Init(_index, "$"));
                }
                else if (curChar == '%')
                {
                    tokens.Add(new LiteralToken().Init(_index, "%"));
                }
                else if (curChar == ',')
                {
                    tokens.Add(new CommaToken().Init(_index, ","));
                }
                else if (curChar == ':')
                {
                    tokens.Add(new ColonToken().Init(_index, ":"));
                }
                else
                {
                    tokens.Add(new TextDataToken().Init(_index, ReadUntil(expression, '\n', '{', '}', '#', '$', '%', ',', ':')));
                }
            }
            return tokens.ToArray();
        }

        private string ReadUntil(string expression, params char[] targets)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = _index; i < expression.Length; i++)
            {
                char curChar = expression[i];
                if (targets.Contains(curChar))
                {
                    _index = i - 1;
                    break;
                }
                builder.Append(curChar);
                _index++;
            }

            return builder.ToString();
        }
    }
}
