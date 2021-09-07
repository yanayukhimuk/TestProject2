using OpenQA.Selenium;
using System;

namespace TestProject2.Pages
{
    public class BRWPage
    {
        public BRWPage(IWebDriver WebDriver)
        {
            this.WebDriver = WebDriver;
        }
        private IWebDriver WebDriver { get; }

        //webDriver.Navigate().GoToUrl("https://www.rw.by"); 
        public IWebElement CheckSite => WebDriver.FindElement(By.ClassName("footer-extra"));

        public bool HasTheSiteLoaded() => CheckSite.Displayed;

        IWebElement LanguageSwitcherENG => WebDriver.FindElement(By.LinkText("ENG"));
        public void ChangeLanguageToEng() => LanguageSwitcherENG.Click();

        public IWebElement NewsItems => WebDriver.FindElement(By.ClassName("index-news-list"));

        private bool GreaterOrEqual(object count, int v)
        {
            throw new NotImplementedException();
        }
        public bool Have5NewsItemsBeenFound()
        {
            return GreaterOrEqual(NewsItems, 4);
        }
        public IWebElement CopyRight => WebDriver.FindElement(By.CssSelector(".footer-extra .copyright"));

        public bool HasCopyrightBeenFound() => CopyRight.Displayed;

        IWebElement LanguageSwitcherRUS => WebDriver.FindElement(By.LinkText("РУС"));
        public void ChangeLanguageToRus() => LanguageSwitcherRUS.Click();


    }
}
