using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject2.Pages
{

    //Actions taken on google site 
    public class GoogleChromePage
    {
        public GoogleChromePage(IWebDriver webDriver)
        {
            Driver = webDriver;
        }
        public IWebDriver Driver { get; }
        public IWebElement SearchInput => Driver.FindElement(By.Name("q"));
        
        public void SearchSiteBRW()
        {
            SearchInput.SendKeys("белорусская железная дорога");
            SearchInput.SendKeys(Keys.Enter);
        }
        
    }
}
