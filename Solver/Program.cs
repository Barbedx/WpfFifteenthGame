// See https://aka.ms/new-console-template for more information
using Solver;
Console.ReadKey();

Console.WriteLine("Hello, World!");
var blocks = new int[][] 
{ 
    new int[] {3, 5 }, 
    new int[] { 1, 4 }, 
    new int[] { 0, 2 } 
};
var board = new Board(blocks);
//var solver = new Solver.Solver(board);

//foreach (var item in solver.solution())
//{
//Console.WriteLine(item);

//}
Console.ReadKey();

