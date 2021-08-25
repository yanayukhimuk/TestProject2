using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;

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

        class WebSiteTest
        {
            Browser_Settings brow = new Browser_Settings();
            IWebDriver webDriver;
            string chrome_url = "https://www.google.com/";
            string brw_url = "https://www.rw.by/";

            [SetUp] // добавила предустановку - нашла такой вариант 
            public void StartBrowser()
            {
                brow.Init_Browser();
            }

            [Test]
            public void Test1() // рабочий 
            {
                brow.Goto(chrome_url);
                System.Threading.Thread.Sleep(2000);

                webDriver = brow.getDriver;

                IWebElement SearchInput = webDriver.FindElement(By.Name("q"));

                SearchInput.SendKeys("белорусская железная дорога");
                SearchInput.Submit();

                IWebElement SearchResult = webDriver.FindElement(By.CssSelector("a[href=\"https://www.rw.by/\"]"));
                SearchResult.Click();

                var CheckSite = webDriver.FindElement(By.Id("tickets_form")); //возможна проверка страницы через JS
                Assert.That(CheckSite.Displayed, Is.True);
            }

            [Test]
            public void Test2() //рабочий 
            {
                brow.Goto(brw_url);
                System.Threading.Thread.Sleep(2000);

                webDriver = brow.getDriver;

                var LanguageSwitcher = webDriver.FindElement(By.LinkText("ENG"));
                LanguageSwitcher.Click();

                var NewsItems = webDriver.FindElements(By.CssSelector(".index-news-list dt"));
                Assert.GreaterOrEqual(NewsItems.Count, 4);

                Assert.DoesNotThrow(() => webDriver.FindElement(By.CssSelector(".footer-extra .copyright")));

                Assert.GreaterOrEqual(webDriver.FindElements(By.CssSelector(".menu-items td")).Count, 5);

                LanguageSwitcher = webDriver.FindElement(By.LinkText("РУС"));
                LanguageSwitcher.Click();
            }

            [Test]
            public void Test3() //рабочий, но есть вопросы 
            {
                brow.Goto(brw_url);
                System.Threading.Thread.Sleep(2000);

                webDriver = brow.getDriver;

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

                Console.WriteLine(webDriver.FindElements(By.CssSelector(".search-result .name"))); //дважды ищу и ссылки не вывел 
            }

            [Test]
            public void Test4()
            {
                brow.Goto(brw_url);
                System.Threading.Thread.Sleep(2000);

                webDriver = brow.getDriver;

                IWebElement WhereFrom = webDriver.FindElement(By.Name("from"));
                IWebElement WhereTo = webDriver.FindElement(By.Name("to"));

                WhereFrom.SendKeys("Брест");
                WhereTo.SendKeys("Минск");
                                               
                DateTime CurrentDate = DateTime.Now; // методом определила текущую дату 
                Console.WriteLine(CurrentDate);

                DateTime RequiredDate = CurrentDate.AddDays(5); // нашла нужную (+ 5 дней), независимо от того, какая текущая 
                Console.WriteLine(RequiredDate);

                IWebElement Calendar = webDriver.FindElement(By.CssSelector(".calendar"));
                Calendar.Click(); // открываем календарь 

                // тут пошла проблема - хотела получить все даты из календаря и сравнить с нужной (над ли вытягивать атрибут?) пыталась JavaScriptExecutor применить уже (дату
                // получилось вбить, но для поиска этого не достаточно - всё равно на появляющемся календаре нужно ткнуть в нужную дату - как на неё выйти? как попасть на highlight 

                var SearchDay = RequiredDate.Day;

                List<IWebElement> tableContent = new List<IWebElement>(webDriver.FindElement(By.Id("ui-datepicker-div"))
                   .FindElements(By.CssSelector("td"))); // список элементов 

                foreach (IWebElement ele in tableContent)
                {
                    string date = ele.Text;

                    if (date.Equals(SearchDay))
                    {
                        ele.Click();
                        break;
                    }
                }

                var ToFind = webDriver.FindElement(By.XPath("//*[@id='fTickets']/div[2]/div[1]/span/input"));
                ToFind.Click(); // если просто кликнуть, будет текущая дата по умолчанию 

                //IWebElement FirstTrain = webDriver.FindElement(By.CssSelector(".sch-table__cell cell-1 .sch-table__route .train-route"));
                //FirstTrain.Click(); - как обратиться к элементам лучше? не получаеся ни через XPath, ни через selector 

                //IWebElement TrainDisplayed = webDriver.FindElement(By.CssSelector(".row .col-lg-9 col-md-8 col-xs-12 .sch-title__title h2"));
                //Assert.That(TrainDisplayed.Displayed, Is.True);

                // var DaysOfTravel = webDriver.FindElement(By.CssSelector(".sch-title__descr")); // XPath вручную (как привязываться к элементам) 
                //Assert.That(DaysOfTravel.Displayed, Is.True);

                //IWebElement SiteLogo = webDriver.FindElement(By.CssSelector(".header-bottom .logo-png"));
                //SiteLogo.Click();

                // var CheckSite2 = webDriver.FindElement(By.Id("tickets_form"));
                // Assert.That(CheckSite2.Displayed, Is.True);


            }

            static string GenerateSymb() //генерация для инпута 
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

