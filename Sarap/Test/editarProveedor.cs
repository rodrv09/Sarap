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
    public class EditarProveedorUITest
    {
        [Fact]
        public void EditarProveedor_DesdeModal_DespuesDeLogin()
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

            wait.Until(drv => drv.FindElement(By.CssSelector(".sidebar a[href*='Proveedores']"))).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("button.btnEditarProveedor")));

            var btnEditar = driver.FindElement(By.CssSelector(
                "button.btnEditarProveedor[data-proveedor*='\"ProveedorId\":6']"
            ));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btnEditar);
            wait.Until(ExpectedConditions.ElementToBeClickable(btnEditar)).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("NombreEmpresa")));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ContactoNombre")));

            var inputNombre = driver.FindElement(By.Id("NombreEmpresa"));
            inputNombre.Clear();
            inputNombre.SendKeys("Proveedor Actualizado");

            var inputContacto = driver.FindElement(By.Id("ContactoNombre"));
            inputContacto.Clear();
            inputContacto.SendKeys("Contacto Actualizado");

            var btnGuardar = driver.FindElement(By.CssSelector("#editarProveedorModal button.btn-primary"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btnGuardar);
            wait.Until(ExpectedConditions.ElementToBeClickable(btnGuardar)).Click();

            wait.Until(drv => drv.PageSource.Contains("Proveedor Actualizado"));
            Assert.Contains("Proveedor Actualizado", driver.PageSource);
        }
    }
}
