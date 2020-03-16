using System;
using System.Collections.Generic;
using System.Linq;
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

        public ArgDefs<T> DefaultRequired<P>(Expression<Func<T, P>> expression, string @switch = null)
        {
            AddSwitch(expression, @switch, true, true);
            return this;
        }

        public ArgDefs<T> DefaultOptional<P>(Expression<Func<T, P>> expression, string @switch = null)
        {
            AddSwitch(expression, @switch, true, false);
            return this;
        }

        public ArgDefs<T> Required<P>(Expression<Func<T, P>> expression, string @switch = null)
        {
            AddSwitch(expression, @switch, false, true);
            return this;
        }

        public ArgDefs<T> Optional<P>(Expression<Func<T, P>> expression, string @switch = null)
        {
            AddSwitch(expression, @switch, false, false);
            return this;
        }

        private void AddSwitch<P>(Expression<Func<T, P>> expression, string @switch, bool isDefault, bool isRequired)
        {
            var name = expression.GetPropertyInfo().Name;

            Switches.Add(new SwitchDef
            {
                Name = name,
                Switch = @switch ?? new string(name.ToCharArray().Where(Char.IsUpper).ToArray()).ToLower(),
                IsDefault = isDefault,
                IsRequired = isRequired
            });
        }
    }
}