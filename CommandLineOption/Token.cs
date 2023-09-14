using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineOption
{
    internal abstract class Token
    {
        private readonly TokenType m_tag;
        public TokenType Tag => m_tag;
        private readonly string m_text;
        public string Text => m_text;

        protected Token(TokenType tag,string text)
        {
            m_tag = tag;
            m_text = text;
        }

        public static Token Name(string text)
        {
            return new Name(text);
        }

        public static Token Value(string text)
        {
            return new Value(text);
        }

    }

    internal class Name : Token,IEquatable<Name>
    {
        public Name(string text) : base(TokenType.Name, text)
        {
        }

        public bool Equals(Name other)
        {
            if (other == null)
            {
                return false;
            }

            if (Tag.Equals(other.Tag) && Text.Equals(other.Text))
            {
                return true;
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            Name name = obj as Name;
            if (name != null)
            {
                return Equals(name);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return new { Tag, Text }.GetHashCode();
        }
    }

    internal class Value : Token,IEquatable<Value>
    {
        public Value(string text) : base(TokenType.Value, text)
        {
        }

        public bool Equals(Value other)
        {
            if(other == null)
            {
                return false;
            }

            if(Tag.Equals(other.Tag))
            {
                return Text.Equals(other.Text);
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            Value value = obj as Value;
            if(value != null)
            {
                return Equals(value);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return new { Tag,Text}.GetHashCode();
        }
    }


    internal enum TokenType
    {
        Name,
        Value,
    }
}
