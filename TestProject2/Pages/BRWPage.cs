using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Linq;
namespace TestProject2.Pages
{
    public class BRWPage
    {

        public BRWPage(IWebDriver WebDriver)
        {
            this.WebDriver = WebDriver;
        }
        private IWebDriver WebDriver { get; }

        const string url = "https://www.rw.by/";
        public void Goto() => WebDriver.Navigate().GoToUrl(url);
        public void HasTheSiteLoaded() => 
            Assert.That(WebDriver.FindElement(By.ClassName("footer-extra")).Displayed);

        public void ChangeLanguage(string RequiredLanguage)
        {
            if (RequiredLanguage == "ENG")
            {
                WebDriver.FindElement(By.LinkText("ENG")).Click();
            }

            else if (RequiredLanguage == "RUS")
            {
                WebDriver.FindElement(By.LinkText("RUS")).Click();
            }
        }
        public void HaveAtLeast4NewsItemsBeenFound() => 
            Assert.GreaterOrEqual(WebDriver.FindElements(By.CssSelector(".index-news-list dt")).Count, 4);

        public void HaveAtLeast5MenuButtonsBeenFound() =>
            Assert.GreaterOrEqual(WebDriver.FindElements(By.CssSelector(".menu-items td")).Count, 5);
       public void HasCopyrightBeenFound() => 
            Assert.That(WebDriver.FindElement(By.CssSelector(".footer-extra .copyright")).Displayed);

        IWebElement SearchInput => WebDriver.FindElement(By.Name("q"));

        readonly string GeneratedSymbols = GenerateSymb();
        public void SendKeysToFakeSearch()
        {
            SearchInput.SendKeys(GeneratedSymbols);
            SearchInput.Submit();
            Assert.AreEqual("https://www.rw.by/search/?s=Y&q=" + GeneratedSymbols, WebDriver.Url);
            Assert.DoesNotThrow(() => WebDriver.FindElement(By.CssSelector(".search-result .notetext")));
        }

        public void ShowFoundLinks() => 
            WebDriver.FindElements(By.CssSelector(".search-result .name")).Select(el => el.GetAttribute("href")).ToList().ForEach(Console.WriteLine);
        public void SetDepartureAndDestination()
        {
            WebDriver.FindElement(By.Name("from")).SendKeys("Брест");
            WebDriver.FindElement(By.Name("to")).SendKeys("Минск");
            WebDriver.FindElement(By.Id("yDate")).SendKeys(DateTime.Now.AddDays(5).ToShortDateString());
            WebDriver.FindElement(By.ClassName("ui-state-active")).Click();
            WebDriver.FindElement(By.CssSelector("#fTickets input[type=\"submit\"")).Click();
        }

        public void PrintFoundTrainsAndDatesToConsole()
        {
            WebDriver.FindElements(By.ClassName("train-route")).Zip(WebDriver.FindElements(By.ClassName("train-from-time")))
                    .ToList()
                    .ForEach(pair => Console.WriteLine(pair.First.Text + " " + pair.Second.Text));
        }

        public void ChooseFirstTrain()
        {
            WebDriver.FindElements(By.ClassName("train-route"))[0].Click();
            Assert.That(WebDriver.FindElement(By.ClassName("sch-title__title")).Displayed);

            Assert.That(WebDriver.FindElement(By.CssSelector(".sch-title__descr")).Displayed);
        }

        public void GoBackToMainPage()
        {
            WebDriver.FindElement(By.CssSelector(".header-bottom .logo-png")).Click();

            Assert.That(WebDriver.FindElement(By.ClassName("g-footer")).Displayed);
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

        public void FindTrainsToSaintPetersburg()
        {
            SearchInput.Clear();
            SearchInput.SendKeys("Санкт-Петербург");
            SearchInput.Submit();
            Assert.GreaterOrEqual(WebDriver.FindElements(By.CssSelector(".search-result .name")).Count, 15);
        }
    }
}
