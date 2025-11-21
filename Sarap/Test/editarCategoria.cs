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
    public class editarCategoria
    {
        [Fact]
        public void EditarCategoria_DesdeModal_DespuesDeLogin()
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

            wait.Until(drv => drv.FindElement(By.CssSelector(".sidebar a[href*='Categorias']")));
            driver.FindElement(By.CssSelector(".sidebar a[href*='Categorias']")).Click();
            wait.Until(drv => drv.FindElement(By.CssSelector("table tbody")));

            var fila = driver.FindElement(By.XPath("//td[text()='Condimentos']/.."));
            var btnEditar = fila.FindElement(By.CssSelector("button.btnEditarCategoria"));
            btnEditar.Click();

            var nombreInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Nombre")));
            var descripcionInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Descripcion")));

            nombreInput.Clear();
            nombreInput.SendKeys("Condimentos Premium");
            descripcionInput.Clear();
            descripcionInput.SendKeys("Categoría mejorada para condimentos selectos");

            var modal = driver.FindElement(By.CssSelector(".modal.show"));
            var btnGuardar = wait.Until(ExpectedConditions.ElementToBeClickable(modal.FindElement(By.CssSelector("button.btn-primary"))));
            btnGuardar.Click();

            wait.Until(drv => drv.FindElement(By.TagName("body")).Text.Contains("Condimentos Premium"));

            var bodyText = driver.FindElement(By.TagName("body")).Text;
            Assert.Contains("Condimentos Premium", bodyText);
            Assert.Contains("Categoría mejorada para condimentos selectos", bodyText);

            fila = driver.FindElement(By.XPath("//td[text()='Condimentos Premium']/.."));
            btnEditar = fila.FindElement(By.CssSelector("button.btnEditarCategoria"));
            btnEditar.Click();

            modal = wait.Until(drv => drv.FindElement(By.CssSelector(".modal.show")));
            btnGuardar = wait.Until(ExpectedConditions.ElementToBeClickable(modal.FindElement(By.CssSelector("button.btn-primary"))));
            btnGuardar.Click();

            wait.Until(drv => !modal.Displayed);
        }
    }
}
