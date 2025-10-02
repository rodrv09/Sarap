using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using Xunit;
using System;

namespace TuProyecto.Tests
{
    public class RegistroUITests
    {
        [Fact]
        public void UsuarioDeberiaRegistrarseCorrectamente()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");

            using var driver = new ChromeDriver(options);

            driver.Navigate().GoToUrl("https://espsarapiqui-e6gnb8cadkgycccc.canadacentral-01.azurewebsites.net/Account/Register");

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            wait.Until(drv => drv.FindElement(By.Id("Nombre"))).SendKeys("Valeria");
            driver.FindElement(By.Id("Apellido")).SendKeys("Rodríguez");
            driver.FindElement(By.Id("NombreUsuario")).SendKeys("VALROD16");
            driver.FindElement(By.Id("Email")).SendKeys("valeria.rodriguez@test.com");
            driver.FindElement(By.Id("Telefono")).SendKeys("88881234");
            driver.FindElement(By.Id("Password")).SendKeys("Test1234!");
            driver.FindElement(By.Id("ConfirmPassword")).SendKeys("Test1234!");

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            wait.Until(drv => drv.Url.Contains("/Login") || drv.PageSource.Contains("Registro exitoso"));

            Assert.Contains("Login", driver.Title); 
        }
    }
}
