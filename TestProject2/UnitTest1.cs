using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;


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
                brow.Goto(chrome_url);
                System.Threading.Thread.Sleep(2000); //убрать - 2 типа ожидания в Selenium - test 
                //Page object - только логика

                IWebElement SearchInput = webDriver.FindElement(By.Name("q"));

                SearchInput.SendKeys("белорусская железная дорога");
                SearchInput.Submit();

                IWebElement SearchResult = webDriver.FindElement(By.CssSelector("a[href=\"https://www.rw.by/\"]"));
                SearchResult.Click();

                System.Threading.Thread.Sleep(2000);

                var CheckSite = webDriver.FindElement(By.ClassName("footer-extra")); //возможна проверка страницы через JS
                Assert.That(CheckSite.Displayed, Is.True);
            }

            [Test]
            public void Test2() 
            {
                brow.Goto(brw_url);
                System.Threading.Thread.Sleep(2000);

                var LanguageSwitcher = webDriver.FindElement(By.LinkText("ENG"));//обработка exception - Assert doesnt throw 
                LanguageSwitcher.Click();

                var NewsItems = webDriver.FindElements(By.CssSelector(".index-news-list dt"));
                Assert.GreaterOrEqual(NewsItems.Count, 4);

                Assert.DoesNotThrow(() => webDriver.FindElement(By.CssSelector(".footer-extra .copyright")));

                Assert.GreaterOrEqual(webDriver.FindElements(By.CssSelector(".menu-items td")).Count, 5);

                LanguageSwitcher = webDriver.FindElement(By.LinkText("РУС"));
                LanguageSwitcher.Click();
            }

            [Test]
            public void Test3()  
            {
                brow.Goto(brw_url);
                System.Threading.Thread.Sleep(2000);

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
                System.Threading.Thread.Sleep(2000);

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
        }
    }
}

