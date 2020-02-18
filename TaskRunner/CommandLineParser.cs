using System;
using TaskRunner.Parsing;

namespace TaskRunner
{
    /*
     * CommandLine :                    Identifier Arguments
     * Arguments :                      Argument*
     * Argument :                       [Space Dash Identifier] [Space SingleValueOrMultipleValues]
     * SingleValueOrMultipleValues:     SingleValue | MultipleValues
     * SingleValue:                     ValueStringValueOrKeyValuePair
     * ValueStringValueOrKeyValuePair:  ValueOrStringValue | KeyValuePair
     * ValueOrStringValue:              Value | StringValue
     * KeyValuePair:                    Identifier Colon ValueOrStringValue
     * MultipleValues:                  OpenSquare [MultipleValue*] CloseSquare
     * MultipleValue:                   [Space] ValueStringValueOrKeyValuePair
     *
     * Identifier: 
     * Value: 
     * StringValue
     *
     * Colon: :
     * Dash: -
     * OpenSquare: [
     * CloseSquare: ]
     * Space
     */
    public class CommandLineParser : ParserBase
    {
        public CommandLineParser() : base(new Lexer()
            .RegexPattern("Identifier", "^[A-Za-z_][A-Za-z0-9_]*$", "^[A-Za-z_][A-Za-z0-9_]*$")
            .RegexPattern("Value", "^[^\"\\s:\\[\\]-]+$", "^[^\"\\s:\\[\\]-]+$")
            .RegexPattern("StringValue", "^\"[^\"]*\"$", "^\"[^\"]*$")
            .RegexPattern("Space", "^ $", "^ $")
            .TokenPattern("Colon", ":")
            .TokenPattern("Dash", "-")
            .TokenPattern("OpenSquare", "[")
            .TokenPattern("CloseSquare", "]")
            .RegexPattern("Space", "^ $", "^ $")
        )
        {
        }

        public override void Root(Node root)
        {
            CommandLine(root);
        }

        private void CommandLine(Node parent)
        {
            var child = AddNode(parent, "CommandLine");
            Required("Identifier", child);

            Arguments(child);
        }

        private void Arguments(Node parent)
        {
            while (AreTokens("Space"))
            {
                Argument(parent);
            }
        }

        private void Argument(Node parent)
        {
            var child = AddNode(parent, "Argument");
            
            if (AreTokens("Space", "Dash"))
            {
                Required("Space");
                Required("Dash");
                Required("Identifier", child);
            }

            if (AreTokens("Space", "Value") || AreTokens("Space", "StringValue") || AreTokens("Space", "OpenSquare"))
            {
                Required("Space");
                SingleValueOrMultipleValues(child);
            }
        }

        private void SingleValueOrMultipleValues(Node parent)
        {
            if (AreTokens("OpenSquare"))
            {
                MultipleValues(parent);
            }
            else
            {
                SingleValue(parent);
            }
        }

        private void SingleValue(Node parent)
        {
            ValueStringValueOrKeyValuePair(parent);
        }

        private void ValueStringValueOrKeyValuePair(Node parent)
        {
            if (AreTokens("Identifier", "Colon"))
            {
                KeyValuePair(parent);
            }
            else if (AreTokens("Value") || AreTokens("StringValue"))
            {
                ValueOrStringValue(parent);
            }
            else
            {
                throw new Exception("Identifier Value or StringValue token expected.");
            }
        }

        private void KeyValuePair(Node parent)
        {
            var child = AddNode(parent, "KeyValuePair");

            Required("Identifier", child);
            Required("Colon");
            ValueOrStringValue(child);
        }

        private void ValueOrStringValue(Node parent)
        {
            if (AreTokens("Value"))
            {
                Required("Value", parent);
            }
            else if (AreTokens("StringValue"))
            {
                Required("StringValue", parent);
            }
            else
            {
                throw new Exception("Value or StringValue token expected.");
            }
        }

        private void MultipleValues(Node parent)
        {
            var child = AddNode(parent, "MultipleValues");

            Required("OpenSquare");
            while (AreTokens("Identifier") || AreTokens("Value") || AreTokens("StringValue") || AreTokens("Space"))
            {
                MultipleValue(child);
            }
            Required("CloseSquare");
        }

        private void MultipleValue(Node parent)
        {
            if (AreTokens("Space"))
            {
                Required("Space");
            }

            ValueStringValueOrKeyValuePair(parent);
        }
    }
}