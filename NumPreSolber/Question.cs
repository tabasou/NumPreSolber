using System;
using System.Text;

namespace NumPreSolber
{
    public class Question
    {
        int [,] iQuestGrid;
        Solber sol;
        int QuestionLv;
        int QuestionNo;
        public Question()
        {
            iQuestGrid = new int[Consts.HeightMax, Consts.WidthMax];

            QuestionLv = Consts.QUESTION_Lv_Difficult;                               /* ←ここで問題のLevelを指定 */
            QuestionNo = 2;                                                     /* ←ここで使用する問題のNoを指定 */

            this.Test_QuestionSelect(QuestionLv, QuestionNo);
            DispGrid();
            sol = new Solber();
            sol.setQuestion(iQuestGrid);
            //sol.test_setAnswerInfo(QuestionLv, QuestionNo);                   /* 問題の解答をチェックしながら解く場合は実行する */
            sol.SolberMain();


            /* 全問題テスト */
            Solber_test SolTest = new Solber_test();
        }

        /* Test用の問題をiQuestGridにセットする */
        public void Test_QuestionSelect(int iLv, int iNo)
        {
            Tester tester = new Tester();

            tester.getQuestion(iLv, iNo, ref iQuestGrid);
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
        public const int POT_1 = 0b0000_0000_0000_0001;
        public const int POT_2 = 0b0000_0000_0000_0010;
        public const int POT_3 = 0b0000_0000_0000_0100;
        public const int POT_4 = 0b0000_0000_0000_1000;
        public const int POT_5 = 0b0000_0000_0001_0000;
        public const int POT_6 = 0b0000_0000_0010_0000;
        public const int POT_7 = 0b0000_0000_0100_0000;
        public const int POT_8 = 0b0000_0000_1000_0000;
        public const int POT_9 = 0b0000_0001_0000_0000;

        public static readonly int[] POTs = { POT_1, POT_2, POT_3, POT_4, POT_5, POT_6, POT_7, POT_8, POT_9};

        public const int Type_YOKO = 0;
        public const int Type_TATE = 1;
        public const int Type_BLOCK = 2;

        public const int NINE = 9;

        public const int QUESTION_Lv_Easy = 0;
        public const int QUESTION_Lv_Normal = 1;
        public const int QUESTION_Lv_Difficult = 2;
        public const int QUESTION_Lv_Expert = 3;

        public const bool ENABLE_CHECK_ANS = true;
        public const bool DISABLE_CHECK_ANS = false;


    }
}
