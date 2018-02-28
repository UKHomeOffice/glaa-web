using System;

namespace GLAA.Domain
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class CascadeDeleteAttribute : Attribute
    {
    }
}
