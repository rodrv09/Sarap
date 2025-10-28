using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using Xunit;
using System;

namespace TuProyecto.Tests
{
    public class LoginUITests
    {
        [Fact]
        public void UsuarioDeberiaLoguearseCorrectamente()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized"); 

            using var driver = new ChromeDriver(options);

            driver.Navigate().GoToUrl("https://espsarapiqui-e6gnb8cadkgycccc.canadacentral-01.azurewebsites.net/");

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            var inputUsuario = wait.Until(drv => drv.FindElement(By.Id("username")));
            inputUsuario.SendKeys("RODRV16");

            var inputPassword = driver.FindElement(By.Id("password"));
            inputPassword.SendKeys("12345678");

            var btnLogin = driver.FindElement(By.CssSelector("button[type='submit']"));
            btnLogin.Click();

            wait.Until(drv => drv.Url.Contains("/Clientes") || drv.Url.Contains("/Dashboard"));

            
            Assert.Contains("Clientes", driver.Title); 
        }
    }
}
