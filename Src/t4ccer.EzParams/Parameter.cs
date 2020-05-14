using System;

namespace t4ccer.EzParams
{
    internal class Parameter
    {
        public string name, alias, defValue, description;

        public Parameter(string name, string alias, string defValue, string description)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.alias = alias;
            this.defValue = defValue;
            this.description = description ?? throw new ArgumentNullException(nameof(description));
        }
    }
}
