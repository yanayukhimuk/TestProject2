using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq; //for Zip, Select = collections  

namespace TestProject2.Pages
{
    public class BRWPage
    {
        public BRWPage(IWebDriver WebDriver)
        {
            this.WebDriver = WebDriver;
        }
        private IWebDriver WebDriver { get; }

        public IWebElement CheckSite => WebDriver.FindElement(By.ClassName("footer-extra"));

        public bool HasTheSiteLoaded() => CheckSite.Displayed;

        public IWebElement LanguageSwitcherENG => WebDriver.FindElement(By.LinkText("ENG"));
        public void ChangeLanguageToEng() => LanguageSwitcherENG.Click();

        public void Have4NewsItemsBeenFound()
        {
            var NewsItems = WebDriver.FindElements(By.CssSelector(".index-news-list dt"));
            Assert.GreaterOrEqual(NewsItems.Count, 4);
        }

        public void Have5MenuButtonsFound()
        {
            Assert.GreaterOrEqual(WebDriver.FindElements(By.CssSelector(".menu-items td")).Count, 5);
        }
        public IWebElement CopyRight => WebDriver.FindElement(By.CssSelector(".footer-extra .copyright"));

        public bool HasCopyrightBeenFound() => CopyRight.Displayed;

        IWebElement LanguageSwitcherRUS => WebDriver.FindElement(By.LinkText("РУС"));
        public void ChangeLanguageToRus() => LanguageSwitcherRUS.Click();

        IWebElement searchInput => WebDriver.FindElement(By.Name("q"));

        string Symbs = GenerateSymb();
        public void sendKeysToSearch()
        {
            searchInput.SendKeys(Symbs);
            searchInput.Submit();
            Assert.AreEqual("https://www.rw.by/search/?s=Y&q=" + Symbs, WebDriver.Url);
            Assert.DoesNotThrow(() => WebDriver.FindElement(By.CssSelector(".search-result .notetext")));
        }

        public void showFoundLinks()
        {
            WebDriver.FindElements(By.CssSelector(".search-result .name")).Select(el => el.GetAttribute("href")).ToList().ForEach(Console.WriteLine);
        }



        public IWebElement WhereFrom => WebDriver.FindElement(By.Name("from"));
        public IWebElement WhereTo => WebDriver.FindElement(By.Name("to"));

        public IWebElement yDate => WebDriver.FindElement(By.Id("yDate"));

        public void setDestination()
        {
            WhereFrom.SendKeys("Брест");
            WhereTo.SendKeys("Минск");
            yDate.SendKeys(DateTime.Now.AddDays(5).ToShortDateString());
            WebDriver.FindElement(By.ClassName("ui-state-active")).Click();
            WebDriver.FindElement(By.CssSelector("#fTickets input[type=\"submit\"")).Click();
        }
       
        public void showTrains()
        {
            WebDriver.FindElements(By.ClassName("train-route")).Zip(WebDriver.FindElements(By.ClassName("train-from-time")))
                    .ToList()
                    .ForEach(pair => Console.WriteLine(pair.First.Text + " " + pair.Second.Text));
        }

        public void chooseFirstTrain()
        {
            WebDriver.FindElements(By.ClassName("train-route"))[0].Click();
            Assert.That(WebDriver.FindElement(By.ClassName("sch-title__title")).Displayed, Is.True);

            Assert.That(WebDriver.FindElement(By.CssSelector(".sch-title__descr")).Displayed, Is.True);
        }

        public void goBackToMainPage()
        {
            WebDriver.FindElement(By.CssSelector(".header-bottom .logo-png")).Click();

            Assert.That(WebDriver.FindElement(By.ClassName("g-footer")).Displayed, Is.True);
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

        public void sendKeysToSearchTwo()
        {
            searchInput.Clear();
            searchInput.SendKeys("Санкт-Петербург");
            searchInput.Submit();
            Assert.GreaterOrEqual(WebDriver.FindElements(By.CssSelector(".search-result .name")).Count, 15);
        }

        public void SiteLoaded()
        {
            var wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(20));
            wait.Until(driver1 => ((IJavaScriptExecutor)WebDriver)
            .ExecuteScript("return document.readyState")
            .Equals("complete"));
        }
    }
}
