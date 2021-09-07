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

                chromePage.SiteLoaded();

                chromePage.SearchSiteBRW();
                chromePage.goToFoundLnk();

                chromePage.SiteLoaded();

                brwPage.HasTheSiteLoaded();
            }

            [Test]
            public void Test2()
            {
                BRWPage brwPage = new BRWPage(webDriver);
                brow.Goto(brw_url);

                brwPage.SiteLoaded();

                brwPage.ChangeLanguageToEng();

                brwPage.Have4NewsItemsBeenFound(); 

                brwPage.HasCopyrightBeenFound();

                brwPage.Have5MenuButtonsFound();

                brwPage.ChangeLanguageToRus();
            }

            [Test]
            public void Test3()
            {
                BRWPage brwPage = new BRWPage(webDriver);
                brow.Goto(brw_url);

                brwPage.SiteLoaded();

                brwPage.sendKeysToSearch();

                brwPage.sendKeysToSearchTwo();

                brwPage.showFoundLinks();
                
            }

            [Test]
            public void Test4()
            {
                BRWPage brwPage = new BRWPage(webDriver);
                brow.Goto(brw_url);

                brwPage.SiteLoaded();

                brwPage.setDestination();

                brwPage.showTrains();

                brwPage.chooseFirstTrain();

                brwPage.goBackToMainPage();
            }

        }
    }
}

