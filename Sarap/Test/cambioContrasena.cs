using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using Xunit;
using System;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace TuProyecto.Tests
{
    public class CambiarCredencialesUITests
    {
        [Fact]
        public void Login_Y_CambiarCredenciales_DeberiaActualizarCorreoYContraseña()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");

            using var driver = new ChromeDriver(options);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            driver.Navigate().GoToUrl("https://espsarapiqui-e6gnb8cadkgycccc.canadacentral-01.azurewebsites.net/Account/Login");

         

            var btnLogin = driver.FindElement(By.CssSelector("button.btn-primary"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btnLogin);
            wait.Until(ExpectedConditions.ElementToBeClickable(btnLogin));
            btnLogin.Click();

            var linkCambiar = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Cambiar contraseña")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", linkCambiar);
            linkCambiar.Click();

            var nombreInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("NombreUsuario")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", nombreInput);
            nombreInput.Clear();
            nombreInput.SendKeys("RODRV16");

            var emailInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("NuevoEmail")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", emailInput);
            emailInput.Clear();
            emailInput.SendKeys("RODRV16@test.com");

            var contraseñaActualInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Contrase_aActual")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", contraseñaActualInput);
            contraseñaActualInput.SendKeys("12345678");

            var nuevaContraseñaInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("NuevaContrase_a")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", nuevaContraseñaInput);
            nuevaContraseñaInput.SendKeys("123456789");

            var confirmarContraseñaInput = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("ConfirmarContrase_a")));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", confirmarContraseñaInput);
            confirmarContraseñaInput.SendKeys("123456789");

            var btnGuardar = driver.FindElement(By.CssSelector("button.btn-primary"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btnGuardar);
            wait.Until(ExpectedConditions.ElementToBeClickable(btnGuardar));
            btnGuardar.Click();

            var bodyText = wait.Until(drv => drv.FindElement(By.TagName("body"))).Text;
            Assert.Contains("Credenciales actualizadas", bodyText); 
        }
    }
}
