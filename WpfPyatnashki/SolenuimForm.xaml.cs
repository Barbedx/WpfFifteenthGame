using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

using SeleniumExtras.WaitHelpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfPyatnashki
{
    /// <summary>
    /// Interaction logic for SolenuimForm.xaml
    /// </summary>
    public partial class SolenuimForm : Window
    {
        public SolenuimForm()
        {
            InitializeComponent();
          

            ChromeOptions options = new();
            options.AddAdditionalOption("useAutomationExtension", false);
            options.AddExcludedArgument("enable-automation");
            options.AddArgument("--disable-blink-features=AutomationControlled");
            //options.AddArgument("user-agent=Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_2) AppleWebKit/537.36(KHTML, like Gecko) Chrome/75.0.3770.100 Safari/537.36");
            ChromeDriver? driver = new ChromeDriver(options);
           
            driver.Navigate().GoToUrl("https://celebrate.pringles.com/");


            var waitingDriver = new WebDriverWait(driver, TimeSpan.FromSeconds(100))
                .Until(ExpectedConditions.ElementIsVisible(By.Id("yourIDHere")));

        }
    }
}
