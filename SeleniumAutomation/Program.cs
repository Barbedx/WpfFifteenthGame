// See https://aka.ms/new-console-template for more information
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium;
using Solver;

Console.WriteLine("Hello, World!");



ChromeOptions options = new();
options.AddAdditionalOption("useAutomationExtension", false);
options.AddExcludedArgument("enable-automation");
options.AddArgument("--disable-blink-features=AutomationControlled");
//options.AddArgument("user-agent=Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_2) AppleWebKit/537.36(KHTML, like Gecko) Chrome/75.0.3770.100 Safari/537.36");
using ChromeDriver? driver = new(options);

driver.Navigate().GoToUrl("https://celebrate.pringles.com/pl_PL/Home/");
var waitingDriver = new WebDriverWait(driver, TimeSpan.FromSeconds(500))
    .Until(ExpectedConditions.ElementIsVisible(By.ClassName("jqp-piece")));

Console.WriteLine("Game finded!!");


// get the data
var elements = driver.FindElements(By.ClassName("jqp-piece")).Where(x => x.GetAttribute("expectedposition") != null)
    .Select(item => new PuzzzleItem { Current = int.Parse(item.GetAttribute("current")), Expected = int.Parse(item.GetAttribute("expectedposition")) });

foreach (var item in elements)
{
    Console.WriteLine($"curent:{item.Current} expected: {item.Expected}");
}

var blocks = new int[][]
{
    new int[] { GetByPos(elements, 1), GetByPos(elements, 2) },
    new int[] { GetByPos(elements, 3),GetByPos(elements, 4) },
    new int[] { GetByPos(elements, 5), GetByPos(elements, 6) }
};

var board = new Board(blocks);
var solver = new Solver.Solver(board, new CancellationTokenSource().Token);

var solutions = solver.solution().Skip(1);

foreach (var item in solutions)//solver.solution())
{

    var index = item.GetNullFieldIndex();
    var puzzleItem = driver.FindElement(SelectorByAttributeValue("current", index));
    driver.FindElement(SelectorByAttributeValue("current", index)).Click();
    //new WebDriverWait(driver, TimeSpan.FromSeconds(7));
    Thread.Sleep(TimeSpan.FromSeconds(0.2));
    Console.WriteLine($"performed click  to {index} for solution:");  
    Console.WriteLine(item);
}
Console.WriteLine("solved!");
Console.ReadKey();


By SelectorByAttributeValue(string v1, object v2)
{
    return By.XPath($"//a[@class = 'jqp-piece' and  @{v1} = '{v2}']");
}

Console.ReadKey();

int GetByPos(IEnumerable<PuzzzleItem> elements, int id)
{

    return elements.FirstOrDefault(x => x.Current == id-1)?.Expected ?? 0;
}


class PuzzzleItem
{
    public int Current { get; set; }
    public int Expected { get; set; }
}