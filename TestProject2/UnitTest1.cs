using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;

namespace TestProject2
{
    public class MainTest
    {
        public class BrowserSettings
        {
            IWebDriver driver;
            public void InitBrowser()
            {
                driver = new ChromeDriver();
                driver.Manage().Window.Maximize();
            }
            public string Title
            {
                get { return driver.Title; }
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
            BrowserSettings brow = new BrowserSettings();
            IWebDriver webDriver = new ChromeDriver();

            [SetUp]
            public void StartBrowser()
            {
                brow.InitBrowser();
            }

            [Test]
            public void Test1()
            {
                webDriver.Navigate().GoToUrl("https://www.google.com/");

                IWebElement SearchInput = webDriver.FindElement(By.Name("q"));

                SearchInput.SendKeys("����������� �������� ������");
                SearchInput.Submit();
 
                IWebElement SearchResult = webDriver.FindElement(By.CssSelector("a[href=\"https://www.rw.by/\"]"));
                SearchResult.Click(); 

                var CheckSite = webDriver.FindElement(By.Id("tickets_form")); //�������� �������� �������� ����� JS
                Assert.That(CheckSite.Displayed, Is.True);
            }

            public void Test2()
            {
                var LanguageSwitcher = webDriver.FindElement(By.LinkText("ENG"));
                LanguageSwitcher.Click();

                var NewsItems = webDriver.FindElements(By.CssSelector(".index-news-list dt")); 
                Assert.GreaterOrEqual(NewsItems.Count, 4);

                Assert.DoesNotThrow(() => webDriver.FindElement(By.CssSelector(".footer-extra .copyright")));

                Assert.GreaterOrEqual(webDriver.FindElements(By.CssSelector(".menu-items td")).Count, 5);

                LanguageSwitcher = webDriver.FindElement(By.LinkText("���"));
                LanguageSwitcher.Click();
            }

            public void Test3()
            {
                IWebElement SearchInputTwo = webDriver.FindElement(By.Name("q"));
                var symbs = GenerateSymb();
                SearchInputTwo.SendKeys(symbs);
                SearchInputTwo.Submit();

                Assert.AreEqual("https://www.rw.by/search/?s=Y&q=" + symbs, webDriver.Url);

                Assert.DoesNotThrow(() => webDriver.FindElement(By.CssSelector(".search-result .notetext")));

                IWebElement SearchInputThree = webDriver.FindElement(By.Name("q"));
                SearchInputThree.Clear();
                SearchInputThree.SendKeys("�����-���������");
                SearchInputThree.Submit();

                Assert.GreaterOrEqual(webDriver.FindElements(By.CssSelector(".search-result .name")).Count, 15);

                Console.WriteLine(webDriver.FindElements(By.CssSelector(".search-result .name")).ToString()); //Console.Writeline
            }

            public void Test4()
            { 
                IWebElement SiteLogo = webDriver.FindElement(By.CssSelector(".top-tools .top-logo"));
                SiteLogo.Click();

                IWebElement WhereFrom = webDriver.FindElement(By.Name("from"));
                IWebElement WhereTo = webDriver.FindElement(By.Name("to"));

                WhereFrom.SendKeys("�����");
                WhereTo.SendKeys("�����");

                //�������� ����� �� ������� ���� + 5 ���� == � ����� � datepicker 

                //IWebElement Calendar = webDriver.FindElement(By.CssSelector(".calendar"));
                //Calendar.Click();

                ///////// ���� ��������� �������� � ������ � Date picker 

                //List<IWebElement> tableContent = new List<IWebElement>(webDriver.FindElement(By.Id("ui-datepicker-div"))
                //    .FindElements(By.CssSelector("td"))); // ������ ��������� 

                //foreach (IWebElement ele in tableContent) 
                //{
                //    string date = ele.Text; 

                //    if (date.Equals(27)) // ���� ������ ������� ������ ���� 
                //    {
                //        ele.Click(); // ������, ��� ���������� �� 5, �� ����, ��� ���������� ��� 
                //    }
                //}

                IWebElement NewDate = webDriver.FindElement(By.XPath("//*[@id='fTickets']/div[2]/div[3]/a[2]")); //��� ��� �� ���� ���� �������� ������ ����, ������ ���������� ���� ����� ������� 
                NewDate.Click();

                var ToFind = webDriver.FindElement(By.XPath("//*[@id='fTickets']/div[2]/div[1]/span/input"));
                ToFind.Click();

                IWebElement FirstTrain = webDriver.FindElement((By.XPath("//*[@id='sch - route']/div[3]/div[2]/div[1]/div[3]/div/div[1]/div/div[1]")));
                FirstTrain.Click();

                var TrainDisplayed = webDriver.FindElement(By.XPath("//*[@id='workarea']/div[2]/div[1]/div/div[2]"));
                Assert.That(TrainDisplayed.Displayed, Is.True);

                var DaysOfTravel = webDriver.FindElement(By.XPath("//*[@id='workarea']/div[2]/div[1]/div/div[3]")); // XPath ������� (��� ������������� � ���������) 
                Assert.That(DaysOfTravel.Displayed, Is.True);

                IWebElement SiteLogo2 = webDriver.FindElement(By.CssSelector(".header-bottom .logo-png"));
                SiteLogo.Click();

                var CheckSite2 = webDriver.FindElement(By.Id("tickets_form"));
                Assert.That(CheckSite2.Displayed, Is.True);

            }

            static string GenerateSymb() //��������� ��� ������ 
            {
                string s = string.Empty;
                Random rand = new Random();
                for (int i = 0; i < 20; i++)
                {
                    s += (char)rand.Next('a', 'z' + 1);
                }
                return s;
            }

            public void ConsoleOutput() //������� ������� ������ � ������� 
            {
                string[] strings = new string[15];
                for (int i = 0; i < 16; i++)
                {
                    Console.WriteLine($"Result[{0}] = {1}", i, strings[i]);
                    Console.ReadKey();
                }
            }
        }
    }
}

