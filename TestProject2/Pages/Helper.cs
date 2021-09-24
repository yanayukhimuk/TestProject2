using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;

namespace TestProject2.Pages
{
    class Helper
    {
        public Helper (IWebDriver WebDriver)
        {
            this.WebDriver = WebDriver;
        }
        private IWebDriver WebDriver { get; }

        public void SiteLoaded() //в базовую страницу 
        {
            var wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(20));
            wait.Until(driver1 => ((IJavaScriptExecutor)WebDriver)
            .ExecuteScript("return document.readyState")
            .Equals("complete"));
        }

        public void CheckThatSiteLoaded()
        {
            var wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(20));
            wait.Until(driver1 => ((IJavaScriptExecutor)WebDriver)
            .ExecuteScript("return document.readyState")
            .Equals("complete"));

            var wait2 = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(10));
            wait2.Until(driver1 => WebDriver.FindElement(By.ClassName("people-theat")).Displayed);
            Assert.That(WebDriver.FindElement(By.ClassName("people-theat")).Displayed, Is.True, "The elem");
        }

        static string GenerateSymb() //в helper
        {
            string s = string.Empty;
            Random rand = new Random();
            for (int i = 0; i < 20; i++)
            {
                s += (char)rand.Next('a', 'z' + 1);
            }
            return s;
        }
    }
}
