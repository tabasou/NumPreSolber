using System;
using System.Text;

namespace NumPreSolber
{
    public class Question
    {
        int [,] iQuestGrid;
        public Question()
        {
            iQuestGrid = new int[Consts.HeightMax, Consts.WidthMax];

            this.Test_QuestionSelect(0);
            DispGrid();

        }

        /* Test用の問題をiQuestGridにセットする */
        public void Test_QuestionSelect(int iNo)
        {
            Tester tester = new Tester();

            tester.getQuestion(0, ref iQuestGrid);
        }
        /* 現状のGridの状態を標準出力に出力する */
        public void DispGrid()
        {
            // 標準出力のエンコーディングにUTF-8を用いる
            Console.OutputEncoding = Encoding.UTF8;
            String Waku_Upper   = "┏━┯━┯━┳━┯━┯━┳━┯━┯━┓";
            String Waku_middle1 = "┠─┼─┼─╂─┼─┼─╂─┼─┼─┨";
            String Waku_middle2 = "┣━┿━┿━╋━┿━┿━╋━┿━┿━┫";
            String Waku_buttom  = "┗━┷━┷━┻━┷━┷━┻━┷━┷━┛";


            Console.WriteLine(Waku_Upper);
            for(int j = 0; j < Consts.HeightMax; j++)
            {
                Console.Write("┃");
                for(int i = 0; i < Consts.WidthMax; i++)
                {
                    if(iQuestGrid[j, i] == 0)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write("{0}", iQuestGrid[j, i]);
                    }
                    
                    if(i % 3 == 2)
                    {
                        Console.Write("┃");
                    }
                    else
                    {
                        Console.Write("│");
                    }
                }
                Console.WriteLine("");
                if(j % 3 == 2)
                {
                    if(j < Consts.HeightMax - 1)
                    {
                        Console.WriteLine(Waku_middle2);
                    }
                    else
                    {
                        Console.WriteLine(Waku_buttom);
                    }
                }
                else
                {
                    Console.WriteLine(Waku_middle1);
                }
            }
        }

    }
    /* 定数定義 */
    public class Consts
    {
        public const int WidthMax = 9;      /* 横方向のマスの最大値 */
        public const int HeightMax = 9;     /* 縦方向のマスの最大値 */
        public const int NumMin = 0;        /* マスに入る数字の最小値（0は未決定） */
        public const int NumMax = 9;        /* マスに入る数字の最大値 */
        public const int BlockNumMax = 9;   /* Block(3×3マス)の数（左上から横方向に0,1,2下に行って3,4,5） */

    }
}
