using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    public class ITEM
    {    // Чтобы узнать длину пути, нам нужно помнить предидущие позиции (и не только поэтому)
        public ITEM? prevBoard;  // ссылка на предыдущий
        public Board? board;   // сама позиция

        public ITEM(ITEM? prevBoard, Board? board)
        {
            this.prevBoard = prevBoard;
            this.board = board;
        }

        public Board? getBoard()
        {
            return board;
        }
        public /*static*/ int measure()//(ITEM item)
        {
            ITEM? item2 = this;// item;
            int c = 0;   // g(x)
            int measure = item2.getBoard().h();  // h(x)
            while (true)
            {
                c++;
                item2 = item2.prevBoard;
                if (item2 == null)
                {
                    // g(x) + h(x)
                    return measure + c;
                }
            }
        }

    }
    public class ItemComparer : IComparer<ITEM>
    {

        public int Compare(ITEM? x, ITEM? y)
        {
            return x.measure().CompareTo(y.measure());
        }
    }

    public class Solver
    {
        private Board initial;    //
        private List<Board> result = new List<Board>();   // этот лист - цепочка ходов, приводящих к решению задачи


        public Solver(Board initial, CancellationToken token) //add Iprogeres??? add IcancelationToken?
        {
            this.initial = initial;

            if (!isSolvable()) return;  //  сначала можно проверить, а решаема ли задача?

            //  очередь. Для нахождения приоритетного сравниваем меры

            var priorityQueue = new PriorityQueue<ITEM, ITEM>(10, new ItemComparer());
            //    @Override
            //    public int compare(ITEM o1, ITEM o2)
            //    {
            //        return new Integer(measure(o1)).compareTo(new Integer(measure(o2)));
            //    }
            //});


            // шаг 1
            var fitrst = new ITEM(null, initial);
            priorityQueue.Enqueue(fitrst, fitrst);

            while (true)
            {
                ITEM board = priorityQueue.Dequeue();// .poll(); //  шаг 2

                if (token.IsCancellationRequested)
                    return;
                //   если дошли до решения, сохраняем весь путь ходов в лист
                if (board.board.isGoal())
                {
                    itemToList(new ITEM(board, board.board));
                    return;
                }

                //   шаг 3
                var iterator = board.board.Neighbors().GetEnumerator();// .iterator(); // соседи
                while (iterator.MoveNext())
                {
                    if (token.IsCancellationRequested)
                        return;
                    
                    //if(priorityQueue.Count > 10000)
                    Board board1 = (Board)iterator.Current;
                    //оптимизация. Очевидно, что один из соседей - это позиция
                    // которая была ходом раньше. Чтобы не возвращаться в состояния,
                    // которые уже были делаем проверку. Экономим время и память.
                    if (board1 != null && !containsInPath(board, board1))
                    {
                        var item = new ITEM(board, board1);
                        priorityQueue.Enqueue(item, item);
                    }
                }

            }
        }

        //  вычисляем f(x)
        //private static int measure(ITEM item)
        //{
        //    ITEM item2 = item;
        //    int c = 0;   // g(x)
        //    int measure = item.getBoard().h();  // h(x)
        //    while (true)
        //    {
        //        c++;
        //        item2 = item2.prevBoard;
        //        if (item2 == null)
        //        {
        //            // g(x) + h(x)
        //            return measure + c;
        //        }
        //    }
        //}

        //  сохранение
        private void itemToList(ITEM item)
        {
            ITEM item2 = item;
            while (true)
            {
                item2 = item2.prevBoard;
                if (item2 == null)
                {
                    result.Reverse();
                    //Collections.reverse(result);
                    return;
                }
                result.Add(item2.board);
            }
        }

        // была ли уже такая позиция в пути
        private bool containsInPath(ITEM item, Board board)
        {
            ITEM item2 = item;
            while (true)
            {
                if (item2.board.Equals(board)) return true;
                item2 = item2.prevBoard;
                if (item2 == null) return false;
            }
        }


        public bool isSolvable()
        {
            return true;
        }

        public int moves()
        {
            if (!isSolvable()) return -1;
            return result.Count - 1;
        }


        // все ради этого метода - чтобы вернуть result
        public IEnumerable<Board> solution()
        {
            return result;
        }
       // https://www.pvsm.ru/java/16174
    }
}
