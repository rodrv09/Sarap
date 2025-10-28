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
    public class FacturasUITests
    {
        [Fact]
        public void CrearFactura_DesdeUI_DespuesDeLogin()
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

            wait.Until(drv => drv.FindElement(By.CssSelector(".sidebar a[href*='Facturas']")));

            driver.FindElement(By.CssSelector(".sidebar a[href*='Facturas']")).Click();

            var btnNuevaFactura = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a.btn-primary[asp-action='Crear']")));
            btnNuevaFactura.Click();

            wait.Until(drv => drv.FindElement(By.Id("ClienteId"))); 

            new SelectElement(driver.FindElement(By.Id("ClienteId"))).SelectByIndex(1); 
            new SelectElement(driver.FindElement(By.Id("TipoPago"))).SelectByText("Efectivo"); 
            driver.FindElement(By.Id("Total")).SendKeys("100.00"); 

            new SelectElement(driver.FindElement(By.Id("ProductoId"))).SelectByIndex(1);
            driver.FindElement(By.Id("CantidadProducto")).SendKeys("2");

            var btnGuardar = driver.FindElement(By.CssSelector("button[type='submit']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btnGuardar);
            wait.Until(ExpectedConditions.ElementToBeClickable(btnGuardar));
            btnGuardar.Click();

            wait.Until(drv => drv.FindElement(By.TagName("body")).Text.Contains("100.00")); 

            var bodyText = driver.FindElement(By.TagName("body")).Text;
            Assert.Contains("100.00", bodyText);
        }
    }
}
