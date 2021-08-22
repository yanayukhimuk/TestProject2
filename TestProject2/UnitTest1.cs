using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;

namespace TestProject2
{
    public class MainTest
    {
        private object wait;

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test1()
        {

            ////////////1////////////////////////////////////1//////////////////////////////////////////1/////////////////////////////////1//////////////////////////////////////1///////

            //Open browser
            IWebDriver webDriver = new ChromeDriver();
            webDriver.Manage().Window.Maximize();
            //Navigate to the site 
            webDriver.Navigate().GoToUrl("https://www.google.com/");

            //Identify search 
            IWebElement SearchInput = webDriver.FindElement(By.Name("q"));

            //Search website
            SearchInput.SendKeys("����������� �������� ������");
            SearchInput.Submit();

            //Go to the target website 
            IWebElement SearchResult = null; //��������!!!
            Assert.DoesNotThrow(() => SearchResult = webDriver.FindElement(By.CssSelector("a[href=\"https://www.rw.by/\"]")));
            SearchResult.Click(); // �����, ��� Displayed, ��� ������� ����� � �� displayed, �� ���� 

            var CheckSite = webDriver.FindElement(By.Id("tickets_form")); //����� � ���, ����� ������, ��������� �� ������ ��������
            Assert.That(CheckSite.Displayed, Is.True);

            ////////////2////////////////////////////////////2//////////////////////////////////////////2/////////////////////////////////2//////////////////////////////////////2///////

            //  Change the language 
            var LanguageSwitcher = webDriver.FindElement(By.LinkText("ENG"));
            LanguageSwitcher.Click();

            // Find news
            var NewsItems = webDriver.FindElements(By.CssSelector(".index-news-list dt")); // . - ���������� � ������, ������ - ������ ������
            Assert.GreaterOrEqual(NewsItems.Count, 4);

            //footer - ��� � ���� bottom
            Assert.DoesNotThrow(() => webDriver.FindElement(By.CssSelector(".footer-extra .copyright")));

            Assert.DoesNotThrow(() => Assert.GreaterOrEqual(
                webDriver.FindElements(By.CssSelector(".menu-items td")).Count, 5 // webDriver.FindElements(By.CssSelector(".menu-items td")) - ��� ������
                )); //�������� ��� ���

            LanguageSwitcher = webDriver.FindElement(By.LinkText("���"));
            LanguageSwitcher.Click();

            ////////////3////////////////////////////////////3//////////////////////////////////////////3/////////////////////////////////3//////////////////////////////////////3///////

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

            Assert.DoesNotThrow(() => Assert.GreaterOrEqual(
                webDriver.FindElements(By.CssSelector(".search-result .name")).Count, 15
                ));

            System.Diagnostics.Debug.WriteLine(webDriver.FindElements(By.CssSelector(".search-result .name")).ToString()); //�� ����, ��� ������� ���������� � �������

            ////////////4////////////////////////////////////4//////////////////////////////////////////4/////////////////////////////////4//////////////////////////////////////4///////

            IWebElement SiteLogo = webDriver.FindElement(By.CssSelector(".top-tools .top-logo"));
            SiteLogo.Click();

            IWebElement WhereFrom = webDriver.FindElement(By.Name("from"));
            IWebElement WhereTo = webDriver.FindElement(By.Name("to"));

            WhereFrom.SendKeys("�����");
            WhereTo.SendKeys("�����");

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

            var DaysOfTravel = webDriver.FindElement(By.XPath("//*[@id='workarea']/div[2]/div[1]/div/div[3]")); 
            Assert.That(DaysOfTravel.Displayed, Is.True);

            IWebElement SiteLogo2 = webDriver.FindElement(By.CssSelector(".header-bottom .logo-png"));
            SiteLogo.Click();

            var CheckSite2 = webDriver.FindElement(By.Id("tickets_form")); 
            Assert.That(CheckSite2.Displayed, Is.True);

        }

        [Test]
        public void Test2()
        {


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

