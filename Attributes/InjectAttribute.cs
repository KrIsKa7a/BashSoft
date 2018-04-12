using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class InjectAttribute : Attribute
    {
        public InjectAttribute()
        {

        }
    }
}
