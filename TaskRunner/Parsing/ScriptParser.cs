using System;

namespace TaskRunner.Parsing
{
    /*
     * Script:                          Task [NewLine Task]*
     * Task:                            Identifier [Space Arg] NewLineArg*
     * Arg:                             Identifier Space SingleValueOrMultipleValues
     * NewLineArg:                      NewLine DoubleSpace Arg
     * SingleValueOrMultipleValues:     SingleValue | MultipleValues
     * SingleValue:                     ValueStringValueOrKeyValuePair
     * ValueStringValueOrKeyValuePair:  ValueOrStringValue | KeyValuePair
     * ValueOrStringValue:              Value | StringValue
     * KeyValuePair:                    Identifier Colon ValueOrStringValue
     * MultipleValues:                  OpenSquare [MultipleValue*] CloseSquare
     * MultipleValue:                   [Space] ValueStringValueOrKeyValuePair
     */

    public class ScriptParser : ParserBase
    {
        public ScriptParser() : base(new Lexer()
            .TokenPattern("NewLine", "\r\n")
            .TokenPattern("Colon", ":")
            .TokenPattern("OpenSquare", "[")
            .TokenPattern("CloseSquare", "]")
            .RegexPattern("Space", "^ $", "^ $")
            .RegexPattern("DoubleSpace", "^  $", "^ $")
            .RegexPattern("Identifier", "^[A-Za-z_][A-Za-z0-9_]*$", "^[A-Za-z_][A-Za-z0-9_]*$")
            .RegexPattern("Value", "^[^\"\\s:\\[\\]]+$", "^[^\"\\s:\\[\\]]+$")
            .RegexPattern("StringValue", "^\"[^\"]*\"$", "^\"[^\"]*$"))
        {
        }

        public override void Root(Node root)
        {
            Script(root);
        }

        private void Script(Node parent)
        {
            var child = AddNode(parent, "Script");
            Task(child);

            while (AreTokens("NewLine"))
            {
                Required("NewLine");
                Task(child);
            }
        }

        private void Task(Node parent)
        {
            var child = AddNode(parent, "Task");
            Required("Identifier", child);

            if (AreTokens("Space", "Identifier"))
            {
                Required("Space");
                Arg(child);
            }

            while (AreTokens("NewLine", "DoubleSpace"))
            {
                NewLineArg(child);
            }
        }

        private void NewLineArg(Node parent)
        {
            Required("NewLine");
            Required("DoubleSpace");
            Arg(parent);
        }

        private void Arg(Node parent)
        {
            var child = AddNode(parent, "Arg");

            Required("Identifier", child);
            Required("Space");
            SingleValueOrMultipleValues(child);
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