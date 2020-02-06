using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TaskRunner
{
    class ArgDefs<T>
    {
        private List<SwitchDef> _switches;

        public ArgDefs()
        {
            _switches = new List<SwitchDef>();
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
            _switches.Add(new SwitchDef
            {
                Name = expression.GetPropertyInfo().Name,
                Switch = @switch,
                IsDefault = isDefault,
                IsRequired = isRequired
            });
        }
    }
}