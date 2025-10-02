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
    public class CategoriasUITests
    {
        [Fact]
        public void CrearCategoria_DesdeModal_DespuesDeLogin()
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

            wait.Until(drv => drv.FindElement(By.CssSelector(".sidebar a[href*='Categorias']")));

            driver.FindElement(By.CssSelector(".sidebar a[href*='Categorias']")).Click();

            var btnNueva = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button[data-bs-target='#agregarCategoriaModal']")));

            btnNueva.Click();

            var nombreInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("NombreCrear")));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", nombreInput);

            nombreInput.SendKeys("Condimentos");
            driver.FindElement(By.Id("DescripcionCrear")).SendKeys("Categoría para todos los condimentos");

            var btnGuardar = driver.FindElement(By.CssSelector("#agregarCategoriaModal button.btn-primary"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btnGuardar);
            wait.Until(ExpectedConditions.ElementToBeClickable(btnGuardar));
            btnGuardar.Click();

            wait.Until(drv => drv.FindElement(By.TagName("body")).Text.Contains("Condimentos"));

            var bodyText = driver.FindElement(By.TagName("body")).Text;
            Assert.Contains("Condimentos", bodyText);
            Assert.Contains("Categoría para todos los condimentos", bodyText);
        }
    }
}
