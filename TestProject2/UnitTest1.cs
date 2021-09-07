using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using TestProject2.Pages;

namespace TestProject2
{
    public class MainTest
    {
        public class Browser_Settings
        {
            IWebDriver driver;
            public void Init_Browser()
            {
                driver = new ChromeDriver();
                driver.Manage().Window.Maximize();
            }
            public void Goto(string url)
            {
                driver.Url = url;
            }
            public void Close()
            {
                driver.Quit();
            }
            public IWebDriver getDriver
            {
                get { return driver; }
            }
        }
        // AAA - именование
        class WebSiteTest
        {
            Browser_Settings brow = new Browser_Settings();
            IWebDriver webDriver => brow.getDriver;

            public WebSiteTest()
            {
                var urls = System.IO.File.ReadAllLines("file.txt");
                chrome_url = urls[0];
                brw_url = urls[1];
            }
            string chrome_url;
            string brw_url;

            [SetUp] // добавила предустановку - нашла такой вариант 
            public void StartBrowser()
            {
                brow.Init_Browser();
            }

            [Test]
            public void Test1()
            {
                GoogleChromePage chromePage = new GoogleChromePage(webDriver);
                BRWPage brwPage = new BRWPage(webDriver);
                brow.Goto(chrome_url);

                SiteLoaded();
                //var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(20));
                //wait.Until(driver1 => ((IJavaScriptExecutor)webDriver)
                //.ExecuteScript("return document.readyState")
                //.Equals("complete")); //JS чтобы дождаться загрузки страницы

                chromePage.SearchSiteBRW();
                chromePage.goToFoundLnk();

                SiteLoaded();

                brwPage.HasTheSiteLoaded();
            }

            [Test]
            public void Test2()
            {
                BRWPage brwPage = new BRWPage(webDriver);
                brow.Goto(brw_url);

                SiteLoaded();

                brwPage.ChangeLanguageToEng();
                //var LanguageSwitcher = webDriver.FindElement(By.LinkText("ENG"));//обработка exception - Assert doesnt throw 
                //LanguageSwitcher.Click();

                //brwPage.Have5NewsItemsBeenFound();
                var NewsItems = webDriver.FindElements(By.CssSelector(".index-news-list dt"));
                Assert.GreaterOrEqual(NewsItems.Count, 4);

                brwPage.HasCopyrightBeenFound();

                //Assert.DoesNotThrow(() => webDriver.FindElement(By.CssSelector(".footer-extra .copyright")));

                Assert.GreaterOrEqual(webDriver.FindElements(By.CssSelector(".menu-items td")).Count, 5);

                brwPage.ChangeLanguageToRus();
            }

            [Test]
            public void Test3()
            {
                brow.Goto(brw_url);

                var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(20)); 
                
                wait.Until(driver1 =>
                ((IJavaScriptExecutor)webDriver)
                .ExecuteScript("return document.readyState")
                .Equals("complete"));

                IWebElement SearchInputTwo = webDriver.FindElement(By.Name("q"));
                var symbs = GenerateSymb();
                SearchInputTwo.SendKeys(symbs);
                SearchInputTwo.Submit();

                Assert.AreEqual("https://www.rw.by/search/?s=Y&q=" + symbs, webDriver.Url);

                Assert.DoesNotThrow(() => webDriver.FindElement(By.CssSelector(".search-result .notetext")));

                IWebElement SearchInputThree = webDriver.FindElement(By.Name("q"));
                SearchInputThree.Clear();
                SearchInputThree.SendKeys("Санкт-Петербург");
                SearchInputThree.Submit();

                Assert.GreaterOrEqual(webDriver.FindElements(By.CssSelector(".search-result .name")).Count, 15);
                webDriver.FindElements(By.CssSelector(".search-result .name")).Select(el => el.GetAttribute("href")).ToList().ForEach(Console.WriteLine);

            }

            [Test]
            public void Test4()
            {
                brow.Goto(brw_url);
                var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(20)); 
                
                wait.Until(driver1 =>
                ((IJavaScriptExecutor)webDriver)
                .ExecuteScript("return document.readyState")
                .Equals("complete"));

                IWebElement WhereFrom = webDriver.FindElement(By.Name("from"));
                IWebElement WhereTo = webDriver.FindElement(By.Name("to"));

                WhereFrom.SendKeys("Брест");
                WhereTo.SendKeys("Минск");

                webDriver.FindElement(By.Id("yDate"))
                    .SendKeys(DateTime.Now.AddDays(5).ToShortDateString());
                webDriver.FindElement(By.ClassName("ui-state-active")).Click();
                webDriver.FindElement(By.CssSelector("#fTickets input[type=\"submit\"")).Click();

                webDriver.FindElements(By.ClassName("train-route")).Zip(webDriver.FindElements(By.ClassName("train-from-time")))
                    .ToList()
                    .ForEach(pair => Console.WriteLine(pair.First.Text + " " + pair.Second.Text));

                webDriver.FindElements(By.ClassName("train-route"))[0].Click();
                Assert.That(webDriver.FindElement(By.ClassName("sch-title__title")).Displayed, Is.True);

                Assert.That(webDriver.FindElement(By.CssSelector(".sch-title__descr")).Displayed, Is.True);

                webDriver.FindElement(By.CssSelector(".header-bottom .logo-png")).Click();

                Assert.That(webDriver.FindElement(By.ClassName("g-footer")).Displayed, Is.True);

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

            public void SiteLoaded()
            {
                var wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(20));
                wait.Until(driver1 => ((IJavaScriptExecutor)webDriver)
                .ExecuteScript("return document.readyState")
                .Equals("complete"));
            }
        }
    }
}

