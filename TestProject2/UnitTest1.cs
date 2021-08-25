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

            [SetUp] // �������� ������������� - ����� ����� ������� 
            public void StartBrowser()
            {
                brow.Init_Browser();
            }

            [Test]
            public void Test1() // ������� 
            {
                brow.Goto(chrome_url);
                System.Threading.Thread.Sleep(2000);

                webDriver = brow.getDriver;

                IWebElement SearchInput = webDriver.FindElement(By.Name("q"));

                SearchInput.SendKeys("����������� �������� ������");
                SearchInput.Submit();

                IWebElement SearchResult = webDriver.FindElement(By.CssSelector("a[href=\"https://www.rw.by/\"]"));
                SearchResult.Click();

                var CheckSite = webDriver.FindElement(By.Id("tickets_form")); //�������� �������� �������� ����� JS
                Assert.That(CheckSite.Displayed, Is.True);
            }

            [Test]
            public void Test2() //������� 
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

                LanguageSwitcher = webDriver.FindElement(By.LinkText("���"));
                LanguageSwitcher.Click();
            }

            [Test]
            public void Test3() //�������, �� ���� ������� 
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
                SearchInputThree.SendKeys("�����-���������");
                SearchInputThree.Submit();

                Assert.GreaterOrEqual(webDriver.FindElements(By.CssSelector(".search-result .name")).Count, 15);

                Console.WriteLine(webDriver.FindElements(By.CssSelector(".search-result .name"))); //������ ��� � ������ �� ����� 
            }

            [Test]
            public void Test4()
            {
                brow.Goto(brw_url);
                System.Threading.Thread.Sleep(2000);

                webDriver = brow.getDriver;

                IWebElement WhereFrom = webDriver.FindElement(By.Name("from"));
                IWebElement WhereTo = webDriver.FindElement(By.Name("to"));

                WhereFrom.SendKeys("�����");
                WhereTo.SendKeys("�����");
                                               
                DateTime CurrentDate = DateTime.Now; // ������� ���������� ������� ���� 
                Console.WriteLine(CurrentDate);

                DateTime RequiredDate = CurrentDate.AddDays(5); // ����� ������ (+ 5 ����), ���������� �� ����, ����� ������� 
                Console.WriteLine(RequiredDate);

                IWebElement Calendar = webDriver.FindElement(By.CssSelector(".calendar"));
                Calendar.Click(); // ��������� ��������� 

                // ��� ����� �������� - ������ �������� ��� ���� �� ��������� � �������� � ������ (��� �� ���������� �������?) �������� JavaScriptExecutor ��������� ��� (����
                // ���������� �����, �� ��� ������ ����� �� ���������� - �� ����� �� ������������ ��������� ����� ������ � ������ ���� - ��� �� �� �����? ��� ������� �� highlight 

                var SearchDay = RequiredDate.Day;

                List<IWebElement> tableContent = new List<IWebElement>(webDriver.FindElement(By.Id("ui-datepicker-div"))
                   .FindElements(By.CssSelector("td"))); // ������ ��������� 

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
                ToFind.Click(); // ���� ������ ��������, ����� ������� ���� �� ��������� 

                //IWebElement FirstTrain = webDriver.FindElement(By.CssSelector(".sch-table__cell cell-1 .sch-table__route .train-route"));
                //FirstTrain.Click(); - ��� ���������� � ��������� �����? �� ��������� �� ����� XPath, �� ����� selector 

                //IWebElement TrainDisplayed = webDriver.FindElement(By.CssSelector(".row .col-lg-9 col-md-8 col-xs-12 .sch-title__title h2"));
                //Assert.That(TrainDisplayed.Displayed, Is.True);

                // var DaysOfTravel = webDriver.FindElement(By.CssSelector(".sch-title__descr")); // XPath ������� (��� ������������� � ���������) 
                //Assert.That(DaysOfTravel.Displayed, Is.True);

                //IWebElement SiteLogo = webDriver.FindElement(By.CssSelector(".header-bottom .logo-png"));
                //SiteLogo.Click();

                // var CheckSite2 = webDriver.FindElement(By.Id("tickets_form"));
                // Assert.That(CheckSite2.Displayed, Is.True);


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
        }
    }
}

