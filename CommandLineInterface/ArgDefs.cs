using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CommandLineInterface
{
    public class ArgDefs<T>
    {
        public List<SwitchDef> Switches { get; set; }

        public ArgDefs()
        {
            Switches = new List<SwitchDef>();
        }

        public ArgDefs<T> DefaultRequired<P>(Expression<Func<T, P>> expression, string @switch)
        {
            AddSwitch(expression, @switch, true, true);
            return this;
        }

        public ArgDefs<T> Required<P>(Expression<Func<T, P>> expression, string @switch)
        {
            AddSwitch(expression, @switch, false, true);
            return this;
        }

        public ArgDefs<T> Optional<P>(Expression<Func<T, P>> expression, string @switch)
        {
            AddSwitch(expression, @switch, false, false);
            return this;
        }

        private void AddSwitch<P>(Expression<Func<T, P>> expression, string @switch, bool isDefault, bool isRequired)
        {
            Switches.Add(new SwitchDef
            {
                Name = expression.GetPropertyInfo().Name,
                Switch = @switch,
                IsDefault = isDefault,
                IsRequired = isRequired
            });
        }
    }
}