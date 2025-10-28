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
    public class ProveedoresUITests
    {
        [Fact]
        public void CrearProveedor_DesdeModal_DespuesDeLogin()
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

            var linkProveedores = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".sidebar a[href*='Proveedores']")));
            linkProveedores.Click();

            var btnNuevoProveedor = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button[data-bs-target='#agregarProveedorModal']")));
            btnNuevoProveedor.Click();

            var nombreEmpresaInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("NombreEmpresaCrear")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", nombreEmpresaInput);

            nombreEmpresaInput.SendKeys("Proveedor Test S.A.");
            driver.FindElement(By.Id("ContactoNombreCrear")).SendKeys("María López");
            driver.FindElement(By.Id("TelefonoCrear")).SendKeys("88881111");
            driver.FindElement(By.Id("EmailCrear")).SendKeys("maria.lopez@test.com");
            driver.FindElement(By.Id("DireccionCrear")).SendKeys("Alajuela, Costa Rica");

            var fechaInput = driver.FindElement(By.Id("FechaRegistroCrear"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].value='2025-10-02';", fechaInput);

            var btnGuardar = driver.FindElement(By.CssSelector("#agregarProveedorModal button.btn-success"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btnGuardar);
            wait.Until(ExpectedConditions.ElementToBeClickable(btnGuardar));
            btnGuardar.Click();

            wait.Until(drv => drv.FindElement(By.TagName("body")).Text.Contains("Proveedor Test S.A."));

            var bodyText = driver.FindElement(By.TagName("body")).Text;
            Assert.Contains("Proveedor Test S.A.", bodyText);
            Assert.Contains("María López", bodyText);
        }
    }
}
