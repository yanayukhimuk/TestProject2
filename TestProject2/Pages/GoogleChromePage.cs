﻿using System;
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
        public IWebElement searchInput => WebDriver.FindElement(By.Name("q"));
        
        public void SearchSiteBRW()
        {
            searchInput.SendKeys("белорусская железная дорога");
            searchInput.SendKeys(Keys.Enter);
        }

        public IWebElement SearchLinkToBRWSite => WebDriver.FindElement(By.CssSelector("a[href=\"https://www.rw.by/\"]"));
        public void goToFoundLnk() => SearchLinkToBRWSite.Click();

    }
}
