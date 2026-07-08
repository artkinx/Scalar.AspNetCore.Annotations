using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artkinx.ScalarAspNetCore.Annotations.Core.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class ScalarCodeSampleAttribute : Attribute
{
    public string Language { get; }
    public string Code { get; }
    public string Title { get; }

    public ScalarCodeSampleAttribute(string language, string code, string title = "")
    {
        Language = language;
        Code = code;
        Title = title;
    }
}

