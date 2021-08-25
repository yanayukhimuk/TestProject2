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

        class WebSiteTest
        {
            Browser_Settings brow = new Browser_Settings();
            IWebDriver webDriver => brow.getDriver;
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
            public void Test3() //рабочий, но есть вопросы 
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

                //var elements = webDriver.FindElements(By.CssSelector(".search-result .name")); - другой вариант 
                //foreach(var el in elements)
                //    Console.WriteLine(el.GetAttribute("href"));
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

                ////DateTime сurrentDate = DateTime.Now; // методом определила текущую дату 
                // Console.WriteLine(сurrentDate);

                // DateTime requiredDate = GetRequiredDateElem().AddDays(5); // нашла нужную (+ 5 дней), независимо от того, какая текущая 
                // //Console.WriteLine(requiredDate);
                webDriver.FindElement(By.CssSelector(".calendar")).Click();

                GetRequiredDateElem(DateTime.Now.AddDays(5)).Click();

                webDriver.FindElement(By.CssSelector("#fTickets input[type=\"submit\"")).Click();

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

            IWebElement GetRequiredDateElem(DateTime requiredDate) => webDriver.FindElement(By.Id("ui-datepicker-div"))
                    .FindElements(By.CssSelector("td"))
                    .Where(el =>
                    {
                        try
                        {
                            return el.FindElement(By.ClassName("ui-state-default")).Text == requiredDate.Day.ToString();
                        }
                        catch (NoSuchElementException)
                        {
                            return false;
                        }
                    }).FirstOrDefault(el =>
            {
                var contextEl = webDriver.FindElement(By.ClassName("ui-datepicker-today"));
                var onclickStr = contextEl.GetAttribute("onclick");
                var commaInd1 = onclickStr.IndexOf(',');
                var commaInd2 = onclickStr.IndexOf(',', commaInd1 + 1);
                var commaInd3 = onclickStr.IndexOf(',', commaInd2 + 1);
                var month = Convert.ToInt32(onclickStr[(commaInd1 + 1)..commaInd2]);
                var year = Convert.ToInt32(onclickStr[(commaInd2 + 1)..commaInd3]);
                return requiredDate.Month == month && requiredDate.Year == year;
            });

        }
    }
}

