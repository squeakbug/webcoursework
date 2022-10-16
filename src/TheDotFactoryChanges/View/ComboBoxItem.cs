using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View
{
    public class ComboBoxItem
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public ComboBoxItem(string name, string value)
        {
            Name = name;
            Value = Value;
        }

        public override string ToString() => Name;
    }
}
