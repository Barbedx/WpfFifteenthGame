using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solver
{
    public class Board
    {
        private int[][] blocks; //   Наше поле. пустое место будем обозначать нулем.
        private int zeroX;    // это нам пригодится в будущем - координаты нуля
        private int zeroY;
        private int _h; //  мера

        public Board(int[][] blocks)
        {
            int[][] blocks2 = deepCopy(blocks);   //   копируем, так как нам нужно быть уверенными в неизменяемости
            this.blocks = blocks2;

            _h = 0;
            for (int i = 0; i < blocks.Length; i++)
            {  //  в этом цикле определяем координаты нуля и вычисляем h(x)
                for (int j = 0; j < blocks[i].Length; j++)
                {
                    if (blocks[i][j] != (i * dimensionY() + j + 1) && blocks[i][j] != 0)
                    {  // если 0 не на своем месте - не считается
                        _h += 1;
                    }
                    if (blocks[i][j] == 0)
                    {
                        zeroX = (int)i;
                        zeroY = (int)j;
                    }
                }
            }
        }


        public int dimensionX()
        {
            return blocks.Length;
        }
        public int dimensionY()
        {
            return blocks[0].Length;
        }

        public int h()
        {
            return _h;
        }

        public bool isGoal()
        {  //   если все на своем месте, значит это искомая позиция
            return _h == 0;
        }


        public override bool Equals(object? obj) 
        {
            if (this == obj) return true;
            if (obj == null || this.GetType() != obj.GetType()) return false;

            Board board = (Board)obj;

            if (board.dimensionX() != dimensionX() 
                && board.dimensionY() != dimensionY()) return false;
            for (int i = 0; i < blocks.Length; i++)
            {
                for (int j = 0; j < blocks[i].Length; j++)
                {
                    if (blocks[i][j] != board.blocks[i][j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public int GetNullFieldIndex()
        {
            if (blocks[0][0] == 0) return 0;
            if (blocks[0][1] == 0) return 1;
            if (blocks[1][0] == 0) return 2;
            if (blocks[1][1] == 0) return 3;
            if (blocks[2][0] == 0) return 4;
            if (blocks[2][1] == 0) return 5;
            throw new ArgumentException("Map doesn't contains ZERO field");
        }

        public IEnumerable<Board> Neighbors()
        {  // все соседние позиции
           // меняем ноль с соседней клеткой, то есть всего 4 варианта
           // если соседнего нет (0 может быть с краю), chng(...) вернет null
            var boardList = new HashSet<Board>();
            boardList.Add(chng(getNewBlock(), zeroX, zeroY, zeroX, zeroY + 1));
            boardList.Add(chng(getNewBlock(), zeroX, zeroY, zeroX, zeroY - 1));
            boardList.Add(chng(getNewBlock(), zeroX, zeroY, zeroX - 1, zeroY));
            boardList.Add(chng(getNewBlock(), zeroX, zeroY, zeroX + 1, zeroY));

            return boardList;
        }

        private int[][] getNewBlock()
        { //  опять же, для неизменяемости
            return deepCopy(blocks);
        }

        private Board chng(int[][] blocks2, int x1, int y1, int x2, int y2)
        {  //  в этом методе меняем два соседних поля

            if (x2 > -1 && x2 < dimensionX() && y2 > -1 && y2 < dimensionY())
            {
                int t = blocks2[x2][y2];
                blocks2[x2][y2] = blocks2[x1][y1];
                blocks2[x1][y1] = t;
                return new Board(blocks2);
            }
            else
                return null;

        }


        public override  string ToString()
        {
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < blocks.Length; i++)
            {
                for (int j = 0; j < blocks[i].Length; j++)
                {
                    s.Append(blocks[i][j]);
                }
                s.Append(Environment.NewLine);
            }
            return s.ToString();
        }

        private static int[][] deepCopy(int[][] original)
        {
            if (original == null)
            {
                return null;
            }

              int[][] result = new int[original.Length][];
            for (int i = 0; i < original.Length; i++)
            {
                result[i] = new int[original[i].Length];
                for (int j = 0; j < original[i].Length; j++)
                {
                    result[i][j] = original[i][j];
                }
            }
            return result;
        } 
    }
}
