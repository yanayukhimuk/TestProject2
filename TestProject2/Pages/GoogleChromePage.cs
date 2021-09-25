using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestProject2.Pages
{

    //Actions taken on google site 
    public class GoogleChromePage
    {
        public GoogleChromePage(IWebDriver WebDriver)
        {
            this.WebDriver = WebDriver;
        }
        private IWebDriver WebDriver { get; }
        public IWebElement SearchInput => 
            WebDriver.FindElement(By.Name("q")); // SearchInput можно в Helper?
        
        public void SearchSiteBRW()
        {
            SearchInput.SendKeys("белорусская железная дорога");
            SearchInput.SendKeys(Keys.Enter);
        }
        public void goToFoundLnk() => 
            WebDriver.FindElement(By.CssSelector("a[href=\"https://www.rw.by/\"]")).Click();

    }
}
