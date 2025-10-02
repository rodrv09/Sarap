using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using Xunit;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace TuProyecto.Tests
{
    public class ActivarClienteUITest
    {
        [Fact]
        public void ActivarCliente_DesdeBoton()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");

            using var driver = new ChromeDriver(options);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            driver.Navigate().GoToUrl("https://espsarapiqui-e6gnb8cadkgycccc.canadacentral-01.azurewebsites.net/Account/Login");
            driver.FindElement(By.Id("username")).SendKeys("RODRV16");
            driver.FindElement(By.Id("password")).SendKeys("12345678");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            wait.Until(drv => drv.FindElement(By.CssSelector(".sidebar a[href*='Clientes']"))).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(
                "a.btn-outline-success[href*='/Clientes/Activar/3']"
            )));

            var btnActivar = driver.FindElement(By.CssSelector(
                "a.btn-outline-success[href*='/Clientes/Activar/3']"
            ));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btnActivar);
            wait.Until(ExpectedConditions.ElementToBeClickable(btnActivar)).Click();

            wait.Until(drv => drv.PageSource.Contains("Activo"));
            Assert.Contains("Activo", driver.PageSource);
        }

        [Fact]
        public void DesactivarCliente_DesdeBoton()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");

            using var driver = new ChromeDriver(options);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            driver.Navigate().GoToUrl("https://espsarapiqui-e6gnb8cadkgycccc.canadacentral-01.azurewebsites.net/Account/Login");
            driver.FindElement(By.Id("username")).SendKeys("RODRV16");
            driver.FindElement(By.Id("password")).SendKeys("12345678");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            wait.Until(drv => drv.FindElement(By.CssSelector(".sidebar a[href*='Clientes']"))).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(
                "a.btn-outline-warning[href*='/Clientes/Desactivar/3']"
            )));

            var btnDesactivar = driver.FindElement(By.CssSelector(
                "a.btn-outline-warning[href*='/Clientes/Desactivar/3']"
            ));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btnDesactivar);
            wait.Until(ExpectedConditions.ElementToBeClickable(btnDesactivar)).Click();

            wait.Until(drv => drv.PageSource.Contains("Inactivo"));
            Assert.Contains("Inactivo", driver.PageSource);
        }
    }
}
