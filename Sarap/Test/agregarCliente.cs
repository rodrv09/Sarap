using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using Xunit;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

public class ClienteUITests
{
    [Fact]
    public void CrearCliente_DesdeModal_DespuesDeLogin_UsandoBarraLateral()
    {
        new DriverManager().SetUpDriver(new ChromeConfig());
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");

        using var driver = new ChromeDriver(options);
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

        try
        {
            driver.Navigate().GoToUrl("https://espsarapiqui-e6gnb8cadkgycccc.canadacentral-01.azurewebsites.net/Account/Login");

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("username"))).SendKeys("RODRV16");
            driver.FindElement(By.Id("password")).SendKeys("12345678");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".sidebar a[href*='Clientes']")));

            var linkClientes = driver.FindElement(By.CssSelector(".sidebar a[href*='Clientes']"));
            linkClientes.Click();

            var btnNuevoCliente = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button[data-bs-target='#agregarClienteModal']")));
            btnNuevoCliente.Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("NombreCrear")));

            driver.FindElement(By.Id("NombreCrear")).SendKeys("Pedro");
            driver.FindElement(By.Id("ApellidoCrear")).SendKeys("Ramírez");
            driver.FindElement(By.Id("EmailCrear")).SendKeys("pedro.ramirez@test.com");
            driver.FindElement(By.Id("TelefonoCrear")).SendKeys("88887777");
            driver.FindElement(By.Id("DireccionCrear")).SendKeys("Cartago");

            var fechaInput = driver.FindElement(By.Id("FechaRegistroCrear"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].value='2025-10-02';", fechaInput);

            driver.FindElement(By.CssSelector("form button[type='submit']")).Click();

            wait.Until(drv => drv.FindElement(By.TagName("body")).Text.Contains("Pedro"));

            var bodyText = driver.FindElement(By.TagName("body")).Text;
            Assert.Contains("Pedro", bodyText);
            Assert.Contains("Ramírez", bodyText);
        }
        finally
        {
            driver.Quit();
        }
    }
}
