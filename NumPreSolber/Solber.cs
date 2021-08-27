using System;
using System.Text;
using System.Reflection;

namespace NumPreSolber
{
    public class Solber
    {
        int[,] iMainGrid;
        int[,] iPotentialsGrid;
        int[,] iPotentialBits;  /* iPotentialsGridの対応するマスに立っているビットの数 */

        public Solber()
        {
            iMainGrid = new int[Consts.HeightMax, Consts.WidthMax];
            iPotentialsGrid = new int[Consts.HeightMax, Consts.WidthMax];
            iPotentialBits = new int[Consts.HeightMax, Consts.WidthMax];
            initPotentialsGrid();
            //DispPotential(0, 0);
            //clrPotential(0, 0, 5);
            //DispPotential(0, 0);
            //DispPotential(5, 5);

            //DispPotentialGridAll();
        }

        /**********************************************************************/
        /****** 初期化関係の関数                                            ******/
        /**********************************************************************/
        /* このクラスのiMainGridに問題をセットする */
        public void setQuestion(int [,] q)
        {
            for(int j = 0; j < Consts.HeightMax; j++)
            {
                for(int i = 0; i < Consts.WidthMax; i++)
                {
                    iMainGrid[j, i] = q[j, i];
                }
            }

            DispGrid(iMainGrid);
        }

        /* iPotentialGridの初期化 */
        /* 全てのマスに1から9全てのビットを立てる */
        private void initPotentialsGrid()
        {
            for(int j = 0; j < Consts.HeightMax; j++)
            {
                for(int i = 0; i < Consts.WidthMax; i++)
                {
                    iPotentialsGrid[j, i] = 0;
                    iPotentialBits[j, i] = 0;
                    for (int k = Consts.NumMin; k < Consts.NumMax; k++)
                    {
                        if(setPotential(j,i,k+1) != true)
                        {
                            outPutSystemErrror(MethodBase.GetCurrentMethod().Name);
                        }
                    }
                }
            }
        }
        
        /* 引数で指定した位置に可能性のある数字の位置に対応するbitを立てる */
        /* input h:縦位置(0〜Consts.HeightMax-1) */
        /*       w:横位置(0〜Consts.WidthMax-1)  */
        /*       iPotNum:可能性のある数字(1〜9)  */
        /* return 引数チェックの結果             */
        private bool setPotential(int h, int w, int iPotNum)
        {
            bool blRtn = true;
            /* 引数チェック */
            if((check_HW(h,w) != true) || (check_iPotNum(iPotNum) != true))
            {
                blRtn = false;
                return blRtn;
            }

            if((iPotentialsGrid[h, w] & Consts.POTs[iPotNum - 1]) == 0)
            {
                /* ビットが立っていなければビットを立てて iPotentialBits を更新する */
                iPotentialsGrid[h, w] |= Consts.POTs[iPotNum - 1];
                iPotentialBits[h, w]++;

                /* ErrorCheck */
                if(iPotentialBits[h, w] > 45)
                {
                    outPutSystemErrror("over45 " + MethodBase.GetCurrentMethod().Name);
                }
            }
            return blRtn;
        }

        /**********************************************************************/
        /****** の関数                                            ******/
        /**********************************************************************/
        private bool check_HW(int h, int w)
        {
            /* input h:縦位置(0〜Consts.HeightMax-1) */
            /*       w:横位置(0〜Consts.WidthMax-1)  */
            /* をチェックする。OKならばtrue            */
            if (h < 0)
            {
                return false;
            }
            if(Consts.HeightMax <= h)
            {
                return false;
            }
            if(w < 0)
            {
                return false;
            }
            if(Consts.WidthMax <= w)
            {
                return false;
            }
            return true;
        }
        private bool check_iPotNum(int iPotNum)
        {
            /*       iPotNum:可能性のある数字(1〜9)  */
            /* をチェックする。OKならtrue            */
            if(iPotNum <= Consts.NumMin)
            {
                return false;
            }
            if (Consts.NumMax < iPotNum)
            {
                return false;
            }
            return true;
        }

        private bool clrPotential(int h, int w, int iPotNum)
        {
            bool blRtn = true;
            /* 引数チェック */
            if ((check_HW(h, w) != true) || (check_iPotNum(iPotNum) != true))
            {
                blRtn = false;
                return blRtn;
            }
            /* フラグが立っていればクリアする */
            if ((iPotentialsGrid[h,w] & Consts.POTs[iPotNum-1]) != 0)
            {
                iPotentialsGrid[h, w] ^= Consts.POTs[iPotNum-1];
                iPotentialBits[h, w]--;
                /* エラーチェック */
                if(iPotentialBits[h, w] < 0)
                {
                    outPutSystemErrror("[E]:iPotentialBits < 0");
                    blRtn = false;
                }
            }
            else
            {
                /* フラグが立っていないのにクリアしようとした場合は警告 */
                outPutSystemErrror("[W]:iPotentialBits is Not Enable");
            }
            return blRtn;
        }

        /**********************************************************************/
        /****** 以下、表示、デバッグ用等の関数                                ******/
        /**********************************************************************/
        /* 現状のGrid(9×9)の状態を標準出力に出力する */
        public void DispGrid(int[,] dispGrid)
        {
            // 標準出力のエンコーディングにUTF-8を用いる
            Console.OutputEncoding = Encoding.UTF8;
            String Waku_Upper = "┏━┯━┯━┳━┯━┯━┳━┯━┯━┓";
            String Waku_middle1 = "┠─┼─┼─╂─┼─┼─╂─┼─┼─┨";
            String Waku_middle2 = "┣━┿━┿━╋━┿━┿━╋━┿━┿━┫";
            String Waku_buttom = "┗━┷━┷━┻━┷━┷━┻━┷━┷━┛";


            Console.WriteLine(Waku_Upper);
            for (int j = 0; j < Consts.HeightMax; j++)
            {
                Console.Write("┃");
                for (int i = 0; i < Consts.WidthMax; i++)
                {
                    if (dispGrid[j, i] == 0)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write("{0}", dispGrid[j, i]);
                    }

                    if (i % 3 == 2)
                    {
                        Console.Write("┃");
                    }
                    else
                    {
                        Console.Write("│");
                    }
                }
                Console.WriteLine("");
                if (j % 3 == 2)
                {
                    if (j < Consts.HeightMax - 1)
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
        /* 現状のiPotentialsGridの状態を標準出力に出力する */
        public void DispPotentialGridAll()
        {
            // 標準出力のエンコーディングにUTF-8を用いる
            Console.OutputEncoding = Encoding.UTF8;
            String Waku_Upper =   "┏━┯━┯━┳━┯━┯━┳━┯━┯━┳━┯━┯━┳━┯━┯━┳━┯━┯━┳━┯━┯━┳━┯━┯━┳━┯━┯━┓";
            String Waku_middle1 = "┠─┼─┼─╂─┼─┼─╂─┼─┼─╂─┼─┼─╂─┼─┼─╂─┼─┼─╂─┼─┼─╂─┼─┼─╂─┼─┼─┨";
            String Waku_middle2 = "┣━┿━┿━╋━┿━┿━╋━┿━┿━╋━┿━┿━╋━┿━┿━╋━┿━┿━╋━┿━┿━╋━┿━┿━╋━┿━┿━┫";
            String Waku_buttom =  "┗━┷━┷━┻━┷━┷━┻━┷━┷━┻━┷━┷━┻━┷━┷━┻━┷━┷━┻━┷━┷━┻━┷━┷━┻━┷━┷━┛";


            
            int jk = 0;
            int il = 0;

            Console.WriteLine(Waku_Upper);
            for (int j = 0; j < Consts.HeightMax; j++)
            {
                jk = j % 3;
                for (int k = 0; k < 3; k++)
                {
                    Console.Write("┃");
                    for (int i = 0; i < Consts.WidthMax; i++)
                    {
                        il = i % 3;
                        for (int l = 0; l < 3; l++)
                        {
                            /* bitが立っていれば数字、立ってなければスペース */
                            if ((iPotentialsGrid[j, i] & Consts.POTs[k*3+l]) == 0)
                            {
                                Console.Write(" ");
                            }
                            else
                            {
                                Console.Write("{0}", (k*3+l) + 1);
                            }
                            /* 罫線 */
                            if ((l % 3) == 2)
                            {
                                Console.Write("┃");
                            }
                            else
                            {
                                Console.Write("│");
                            }
                        }
                    }
                    Console.WriteLine("");
                    if ((k % 3) == 2)
                    {
                        if (j*3+jk < Consts.HeightMax*3 - 1)
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
        /* 指定したマスのiPotentialsGridの状態を標準出力に出力する */
        /* input h:縦位置(0〜Consts.HeightMax-1) */
        /*       w:横位置(0〜Consts.WidthMax-1)  */
        /*       iPotNum:可能性のある数字(1〜9)  */
        public void DispPotential(int h, int w)
        {
            /* 引数チェック */
            if (check_HW(h, w) != true)
            {
                outPutSystemErrror(MethodBase.GetCurrentMethod().Name);
            }

            Console.WriteLine("Potential of [{0},{1}] = {2}({3}bits Enbale)", h, w, iPotentialsGrid[h,w], iPotentialBits[h,w]);
            for (int k = 0; k < 3; k++)
            {
                for(int l = 0; l < 3; l++)
                {
                    /* bitが立っていれば数字、立ってなければスペース */
                    if ((iPotentialsGrid[h, w] & Consts.POTs[k * 3 + l]) == 0)
                    {
                        Console.Write("  ");
                    }
                    else
                    {
                        Console.Write(" {0}", (k * 3 + l) + 1);
                    }
                }
                Console.WriteLine("");
            }
        }

        /* デバッグ用エラー出力 */
        private void outPutSystemErrror(String str)
        {
            Console.Error.WriteLine("Error!!! -----> : " + str);
        }
    }
}
