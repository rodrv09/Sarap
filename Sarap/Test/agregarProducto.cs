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
    public class ProductosUITests
    {
        [Fact]
        public void CrearProducto_Desactivado_DesdeModal_DespuesDeLogin()
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

            wait.Until(drv => drv.FindElement(By.CssSelector(".sidebar a[href*='Productos']")));

            driver.FindElement(By.CssSelector(".sidebar a[href*='Productos']")).Click();

            var btnAgregar = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button[data-bs-target='#agregarProductoModal']")));
            btnAgregar.Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("NombreCrear")));

            driver.FindElement(By.Id("NombreCrear")).SendKeys("Salsa Picante");
            driver.FindElement(By.Id("DescripcionCrear")).SendKeys("Salsa muy picante");
            new SelectElement(driver.FindElement(By.Id("CategoriaCrear"))).SelectByText("Condimentos");
            new SelectElement(driver.FindElement(By.Id("UnidadMedidaCrear"))).SelectByText("Mililitros");
            driver.FindElement(By.Id("CantidadCrear")).SendKeys("250");
            driver.FindElement(By.Id("PrecioCrear")).SendKeys("3.50");
            new SelectElement(driver.FindElement(By.Id("ProveedorIDCrear"))).SelectByIndex(1); // primer proveedor

            var activoCheckbox = driver.FindElement(By.Id("ActivoCrear"));
            if (activoCheckbox.Selected)
            {
                activoCheckbox.Click();
            }

            driver.FindElement(By.Id("StockMinimoCrear")).SendKeys("10");

            var btnGuardar = driver.FindElement(By.CssSelector("#agregarProductoModal button[type='submit']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btnGuardar);
            wait.Until(ExpectedConditions.ElementToBeClickable(btnGuardar));
            btnGuardar.Click();

            wait.Until(drv => drv.FindElement(By.TagName("body")).Text.Contains("Salsa Picante"));

            var bodyText = driver.FindElement(By.TagName("body")).Text;
            Assert.Contains("Salsa Picante", bodyText);
            Assert.Contains("Salsa muy picante", bodyText);
        }
    }
}
