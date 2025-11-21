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
    public class UsuariosUITests
    {
        [Fact]
        public void CrearUsuario_DesdeModal_DespuesDeLogin()
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

            var linkUsuarios = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".sidebar a[href*='Usuarios']")));
            linkUsuarios.Click();

            var btnNuevoUsuario = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button[data-bs-target='#agregarUsuarioModal']")));
            btnNuevoUsuario.Click();

            var nombreInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("NombreCrear")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", nombreInput);

            nombreInput.SendKeys("Juan");
            driver.FindElement(By.Id("ApellidoCrear")).SendKeys("Pérez");
            driver.FindElement(By.Id("EmailCrear")).SendKeys("juan.perez@test.com");
            driver.FindElement(By.Id("TelefonoCrear")).SendKeys("88882222");
            driver.FindElement(By.Id("NombreUsuarioCrear")).SendKeys("juanperez");
            driver.FindElement(By.Id("ContraseñaCrear")).SendKeys("12345678");

            var rolSelect = new SelectElement(driver.FindElement(By.Id("RolCrear")));
            rolSelect.SelectByText("Usuario");

            var fechaInput = driver.FindElement(By.Id("FechaCreacionCrear"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].value=arguments[1];", fechaInput, DateTime.Now.ToString("yyyy-MM-dd"));

            var btnGuardar = driver.FindElement(By.CssSelector("#agregarUsuarioModal button.btn-primary"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btnGuardar);
            wait.Until(ExpectedConditions.ElementToBeClickable(btnGuardar));
            btnGuardar.Click();

            wait.Until(drv => drv.FindElement(By.TagName("body")).Text.Contains("Juan"));

            var bodyText = driver.FindElement(By.TagName("body")).Text;
            Assert.Contains("Juan", bodyText);
            Assert.Contains("Pérez", bodyText);
            Assert.Contains("juan.perez@test.com", bodyText);
        }
    }
}
