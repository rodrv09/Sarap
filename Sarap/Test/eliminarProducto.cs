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
    public class eliminarProducto
    {
        [Fact]
        public void EliminarProducto_DesdeFilaEspecifica_DespuesDeLogin()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");

            using var driver = new ChromeDriver(options);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

            driver.Navigate().GoToUrl("https://espsarapiqui-e6gnb8cadkgycccc.canadacentral-01.azurewebsites.net/Account/Login");
            driver.FindElement(By.Id("username")).SendKeys("RODRV16");
            driver.FindElement(By.Id("password")).SendKeys("12345678");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            wait.Until(drv => drv.FindElement(By.CssSelector(".sidebar a[href*='Productos']")));
            driver.FindElement(By.CssSelector(".sidebar a[href*='Productos']")).Click();

            wait.Until(drv => drv.FindElement(By.CssSelector("table tbody")));

            var filaProducto = driver.FindElement(By.XPath("//a[@href='/Productos/Eliminar/17']/ancestor::tr"));

            var btnEliminar = filaProducto.FindElement(By.CssSelector("a.btn-outline-danger"));
            btnEliminar.Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("form button.btn-danger")));

            var btnConfirmarEliminar = driver.FindElement(By.CssSelector("form button.btn-danger"));
            btnConfirmarEliminar.Click();

            wait.Until(drv => !drv.FindElement(By.TagName("body")).Text.Contains("Producto a eliminar")); 
            var bodyText = driver.FindElement(By.TagName("body")).Text;
            Assert.DoesNotContain("Producto a eliminar", bodyText); 
        }
    }
}
