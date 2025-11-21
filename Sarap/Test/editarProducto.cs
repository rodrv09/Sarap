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
    public class editarProducto
    {
        [Fact]
        public void EditarProducto_DesdeModal_DespuesDeLogin()
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

            var primeraFila = driver.FindElement(By.CssSelector("table tbody tr"));

            var btnEditar = primeraFila.FindElement(By.CssSelector("button.btnEditarProducto"));
            btnEditar.Click();

            var modal = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#editarProductoModal.modal.show")));

            var nombreInput = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("Nombre")));
            var descripcionInput = driver.FindElement(By.Id("Descripcion"));
            var categoriaInput = driver.FindElement(By.Id("Categoria"));
            var unidadInput = driver.FindElement(By.Id("UnidadMedida"));
            var cantidadInput = driver.FindElement(By.Id("Cantidad"));
            var precioInput = driver.FindElement(By.Id("Precio"));
            var proveedorInput = driver.FindElement(By.Id("ProveedorID"));
            var activoCheckbox = driver.FindElement(By.Id("Activo"));
            var stockInput = driver.FindElement(By.Id("StockMinimo"));

            nombreInput.Clear();
            nombreInput.SendKeys("Pr");
            descripcionInput.Clear();
            descripcionInput.SendKeys("Descripción actualizada");
            categoriaInput.Clear();
            categoriaInput.SendKeys("Condimentos");
            unidadInput.Clear();
            unidadInput.SendKeys("Unidades");
            cantidadInput.Clear();
            cantidadInput.SendKeys("50");
            precioInput.Clear();
            precioInput.SendKeys("12.50");
            proveedorInput.Clear();
            proveedorInput.SendKeys("1"); 
            if (!activoCheckbox.Selected) activoCheckbox.Click();
            stockInput.Clear();
            stockInput.SendKeys("10");

            var btnGuardar = wait.Until(ExpectedConditions.ElementToBeClickable(modal.FindElement(By.CssSelector("button.btn-primary"))));
            btnGuardar.Click();

            wait.Until(drv => drv.FindElement(By.TagName("body")).Text.Contains("Producto Editado"));

            var bodyText = driver.FindElement(By.TagName("body")).Text;
            Assert.Contains("Producto Editado", bodyText);
            Assert.Contains("Descripción actualizada", bodyText);
        }
    }
}
