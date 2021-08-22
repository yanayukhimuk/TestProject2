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
    public class BRWPage
    {
        public BRWPage(IWebDriver WebDriver)
        {
            this.WebDriver = WebDriver;
        }
        private IWebDriver WebDriver { get; }

        //webDriver.Navigate().GoToUrl("https://www.rw.by"); 
        public IWebElement CheckSite => WebDriver.FindElement(By.Id("tickets_form"));
        
        public bool HasFound() =>CheckSite.Displayed;

        IWebElement LanguageSwitcher => WebDriver.FindElement(By.LinkText("ENG"));
        public void ChangeLanguage() => LanguageSwitcher.Click();

        public IWebElement NewsItems => WebDriver.FindElement(By.ClassName("index-news-list"));
    }
}
