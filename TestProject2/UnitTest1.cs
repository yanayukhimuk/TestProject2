using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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
                Helper helper = new Helper(webDriver);
                brow.Goto(chrome_url);

                helper.SiteLoaded();

                chromePage.SearchSiteBRW();
                chromePage.goToFoundLnk();

                helper.SiteLoaded();

                helper.CheckThatSiteLoaded();
            }

            [Test]
            public void Test2()
            {
                BRWPage brwPage = new BRWPage(webDriver);
                Helper helper = new Helper(webDriver);
                brow.Goto(brw_url);

                helper.CheckThatSiteLoaded();

                brwPage.ChangeLanguage("ENG");

                brwPage.HaveAtLeast4NewsItemsBeenFound(); 

                brwPage.HasCopyrightBeenFound();

                brwPage.HaveAtLeast5MenuButtonsBeenFound();

                brwPage.ChangeLanguage("RUS");
            }

            [Test]
            public void Test3()
            {
                BRWPage brwPage = new BRWPage(webDriver);
                Helper helper = new Helper(webDriver);
                brow.Goto(brw_url);

                helper.CheckThatSiteLoaded();

                brwPage.SendKeysToFakeSearch();

                brwPage.FindTrainsToSaintPetersburg();

                brwPage.ShowFoundLinks();
                
            }

            [Test]
            public void Test4()
            {
                BRWPage brwPage = new BRWPage(webDriver);
                Helper helper = new Helper(webDriver);
                brow.Goto(brw_url);

                helper.CheckThatSiteLoaded();

                brwPage.SetDepartureAndDestination();

                brwPage.PrintFoundTrainsAndDatesToConsole();

                brwPage.ChooseFirstTrain();

                brwPage.GoBackToMainPage();
            }

            [TearDown]

            public void CloseBrowser()
            {
                brow.Close();
            }
        }
    }
}

