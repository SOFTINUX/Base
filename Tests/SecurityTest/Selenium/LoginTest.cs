using System;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using Xunit;

namespace SecurityTest
{
    public class LoginTest : IDisposable
    {
        // You need the Chrome driver.
        // Find it here: http://chromedriver.storage.googleapis.com/index.html
        // place the chromedriver into current directory or in a directory on the PATH environment variable

        private readonly ChromeDriver driver;

        public LoginTest()
        {
            ChromeOptions options = new ChromeOptions();
            //options.AddExtensions(new File("/path/to/extension.crx"));
            driver = new ChromeDriver(options);
        }

        public void Dispose()
        {
           driver.Dispose();
        }

        [Fact]
        public void GotoLoginPage()
        {
            driver.Navigate().GoToUrl("http://localhost:5000/account/signin?next=%2F");
            Console.WriteLine(driver.Title);

            IWebElement loginBox = driver.FindElement(By.Id("username"));
            loginBox.SendKeys("admin");
            IWebElement passwordBox = driver.FindElement(By.Id("password"));
            passwordBox.SendKeys("123password");
            IWebElement singinLink = driver.FindElement(By.Id("signin"));
            //singinLink.Submit();
            singinLink.Click();

            Console.WriteLine(driver.Title);
            driver.Quit();
        }

    }
}