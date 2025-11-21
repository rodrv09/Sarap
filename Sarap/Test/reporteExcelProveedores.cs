using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using Xunit;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace TuProyecto.Tests
{
    public class DescargarReporteProveedoresUITest
    {
        [Fact]
        public void DescargarReporteProveedores_DesdeNavbar()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");

            string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            options.AddUserProfilePreference("download.default_directory", downloadPath);
            options.AddUserProfilePreference("download.prompt_for_download", false);
            options.AddUserProfilePreference("safebrowsing.enabled", true);

            using var driver = new ChromeDriver(options);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            driver.Navigate().GoToUrl("https://espsarapiqui-e6gnb8cadkgycccc.canadacentral-01.azurewebsites.net/Account/Login");
            driver.FindElement(By.Id("username")).SendKeys("RODRV16");
            driver.FindElement(By.Id("password")).SendKeys("12345678");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            wait.Until(drv => drv.FindElement(By.CssSelector(".sidebar a[href*='Reportes']"))).Click();

            var btnDescargar = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(
                "a.btn-outline-success[href*='/Reportes/ExportarExcelProveedores']"
            )));
            btnDescargar.Click();

            Thread.Sleep(5000); 

            var archivoDescargado = Directory.GetFiles(downloadPath, "*.xlsx")
                                             .Select(f => new FileInfo(f))
                                             .OrderByDescending(f => f.CreationTime)
                                             .FirstOrDefault();

            Assert.NotNull(archivoDescargado); 
            Console.WriteLine("Archivo descargado: " + archivoDescargado.FullName);
        }
    }
}
