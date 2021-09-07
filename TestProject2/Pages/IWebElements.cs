using System;

namespace TestProject2.Pages
{
    public interface IWebElements
    {
        object Select(Func<object, object> p);
    }
}