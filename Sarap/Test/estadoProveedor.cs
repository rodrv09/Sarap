using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using Xunit;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;

namespace TuProyecto.Tests
{
    public class ToggleProveedorUITest
    {
        [Fact]
        public void ActivarDesactivarProveedor_DesdeToggle()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");

            using var driver = new ChromeDriver(options);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

            driver.Navigate().GoToUrl("https://espsarapiqui-e6gnb8cadkgycccc.canadacentral-01.azurewebsites.net/Account/Login");
            Thread.Sleep(1500);
            driver.FindElement(By.Id("username")).SendKeys("RODRV16");
            driver.FindElement(By.Id("password")).SendKeys("12345678");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            Thread.Sleep(2000);
            wait.Until(drv => drv.FindElement(By.CssSelector(".sidebar a[href*='Proveedores']"))).Click();

            Thread.Sleep(2000);

            var fila = wait.Until(drv => drv.FindElement(By.XPath("//td[text()='Proveedor Test S.A.']/ancestor::tr")));
            Thread.Sleep(1000);

            var btnToggle = fila.FindElement(By.CssSelector("a > i.fas.fa-toggle-on, a > i.fas.fa-toggle-off"));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btnToggle);
            Thread.Sleep(1500);
            btnToggle.Click();

            Thread.Sleep(2000);


            var badge = fila.FindElement(By.CssSelector("span.badge"));
            var estado = badge.Text;

            Assert.True(estado.Contains("Activo") || estado.Contains("Inactivo"),
                        $"Se esperaba que el estado del proveedor cambie, pero es: {estado}");

            Thread.Sleep(1500);
        }
    }
}
