using System;
using System.Text;
using System.Reflection;

namespace NumPreSolber
{
    public class Solber
    {
        struct stPoints
        {
            public int h_idx;
            public int w_idx;

            public stPoints(int h, int w)
            {
                h_idx = h;
                w_idx = w;
            }
        }
        class NineData
        {
            private int targetHindex;   /* 対象セルの縦座標 */
            private int targetWindex;   /* 対象セルの横座標 */
            private int type;           /* 縦か横かBlockか */
            public int[] MG;
            public int[] PG;
            public int[] PB;

            public NineData()
            {
                targetHindex = 0;
                targetWindex = 0;
                type = 0;
                MG = new int[Consts.NINE];
                PG = new int[Consts.NINE];
                PB = new int[Consts.NINE];
            }
            public void setTarget(int hIdx, int wIdx, int setType, in int[,] iMG, in int[,] iPG, in int[,] iPB)
            {
                if ((hIdx < 0) || (Consts.HeightMax <= hIdx) || (wIdx < 0) || (Consts.WidthMax <= wIdx) || (setType < 0) || (3 <= setType))
                {
                    Console.Error.WriteLine("Error!!! -----> : " + "NineData.setTarget() input error");
                }
                targetHindex = hIdx;
                targetWindex = wIdx;
                type = setType;

                /* タイプ別に9つのデータをメンバ変数に格納する */
                switch (type)
                {
                    case Consts.Type_YOKO:
                        for (int i = 0; i < Consts.NINE; i++)
                        {
                            MG[i] = iMG[targetHindex, i];
                            PG[i] = iPG[targetHindex, i];
                            PB[i] = iPB[targetHindex, i];
                        }
                        break;
                    case Consts.Type_TATE:
                        for (int j = 0; j < Consts.NINE; j++)
                        {
                            MG[j] = iMG[j, targetWindex];
                            PG[j] = iPG[j, targetWindex];
                            PB[j] = iPB[j, targetWindex];
                        }
                        break;
                    case Consts.Type_BLOCK:
                        int block_UL_hIdx = (targetHindex / 3) * 3; /* ターゲットセルを含むブロックの左上セルの縦座標 */
                        int block_UL_wIdx = (targetWindex / 3) * 3; /* ターゲットセルを含むブロックの左上セルの横座標 */

                        for (int j = 0; j < 3; j++)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                /* 1 2 3 
                                 * 6 5 4  ---> MG[9] = [1,2,3,6,5,4,7,8,9]
                                 * 7 8 9
                                 */
                                MG[j * 3 + i] = iMG[block_UL_hIdx + j, block_UL_wIdx + i];
                                PG[j * 3 + i] = iPG[block_UL_hIdx + j, block_UL_wIdx + i];
                                PB[j * 3 + i] = iPB[block_UL_hIdx + j, block_UL_wIdx + i];
                            }
                        }
                        break;
                    default:
                        Console.Error.WriteLine("Error!!! -----> : " + "NineData.setTarget() setType error");
                        break;
                }
            }
            public void rverseDatas(ref int[,] iMG, ref int[,] iPG, ref int[,] iPB)
            {
                switch (type)
                {
                    case Consts.Type_YOKO:
                        for (int i = 0; i < Consts.NINE; i++)
                        {
                            iMG[targetHindex, i] = MG[i];
                            iPG[targetHindex, i] = PG[i];
                            iPB[targetHindex, i] = PB[i];
                        }
                        break;
                    case Consts.Type_TATE:
                        for (int j = 0; j < Consts.NINE; j++)
                        {
                            iMG[j, targetWindex] = MG[j];
                            iPG[j, targetWindex] = PG[j];
                            iPB[j, targetWindex] = PB[j];
                        }
                        break;
                    case Consts.Type_BLOCK:
                        int block_UL_hIdx = (targetHindex / 3) * 3; /* ターゲットセルを含むブロックの左上セルの縦座標 */
                        int block_UL_wIdx = (targetWindex / 3) * 3; /* ターゲットセルを含むブロックの左上セルの横座標 */

                        for (int j = 0; j < 3; j++)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                /* 1 2 3 
                                 * 6 5 4  <--- MG[9] = [1,2,3,6,5,4,7,8,9]
                                 * 7 8 9
                                 */
                                iMG[block_UL_hIdx + j, block_UL_wIdx + i] = MG[j * 3 + i];
                                iPG[block_UL_hIdx + j, block_UL_wIdx + i] = PG[j * 3 + i];
                                iPB[block_UL_hIdx + j, block_UL_wIdx + i] = PB[j * 3 + i];
                            }
                        }
                        break;
                }

            }

            public void updatePGbyMG()
            {
                for (int n = 0; n < Consts.NINE; n++)
                {
                    if (MG[n] != 0)
                    {
                        for (int m = 0; m < Consts.NINE; m++)
                        {
                            if (m == n)
                            {
                                PG[m] = 0;
                                PB[m] = 0;
                            }
                            else
                            {
                                clrPG(m, MG[n]);
                            }
                        }
                    }
                }
            }

            private void clrPG(int index, int iPotNum)
            {
                /* フラグが立っていればクリアする */
                if ((PG[index] & Consts.POTs[iPotNum - 1]) != 0)
                {
                    PG[index] ^= Consts.POTs[iPotNum - 1];
                    PB[index]--;
                    /* エラーチェック */
                    if (PB[index] < 0)
                    {
                        Console.Error.WriteLine("[E]:iPotentialBits < 0");
                    }
                }
            }
        }

        int[,] iMainGrid;
        int[,] iPotentialsGrid;
        int[,] iPotentialBits;  /* iPotentialsGridの対応するマスに立っているビットの数 */
        stPoints[] stBlockULPoints;

        /* デバッグ用に使用する変数 */
        /* -------------------- */
        int test_iQLv;
        int test_iQNo;
        bool test_blIsCheckAnswer = Consts.DISABLE_CHECK_ANS;
        Tester test_ANS;
        /* -------------------- */

        public Solber()
        {
            iMainGrid = new int[Consts.HeightMax, Consts.WidthMax];
            iPotentialsGrid = new int[Consts.HeightMax, Consts.WidthMax];
            iPotentialBits = new int[Consts.HeightMax, Consts.WidthMax];
            stBlockULPoints = new stPoints[Consts.BlockNumMax];

            /* 初期化 */
            initPotentialsGrid();
            initBlockPoints();

        }

        /**********************************************************************/
        /****** 初期化関係の関数                                            ******/
        /**********************************************************************/
        /* このクラスのiMainGridに問題をセットする
         */
        public void setQuestion(int[,] q)
        {
            for (int j = 0; j < Consts.HeightMax; j++)
            {
                for (int i = 0; i < Consts.WidthMax; i++)
                {
                    iMainGrid[j, i] = q[j, i];
                }
            }

            DispGrid(iMainGrid);
        }
        /* iPotentialGridの初期化
         * 全てのマスに1から9全てのビットを立てる
         */
        private void initPotentialsGrid()
        {
            for (int j = 0; j < Consts.HeightMax; j++)
            {
                for (int i = 0; i < Consts.WidthMax; i++)
                {
                    iPotentialsGrid[j, i] = 0;
                    iPotentialBits[j, i] = 0;
                    for (int k = Consts.NumMin; k < Consts.NumMax; k++)
                    {
                        if (setPotential(j, i, k + 1) != true)
                        {
                            outPutSystemErrror(MethodBase.GetCurrentMethod().Name);
                        }
                    }
                }
            }
        }
        /* 引数で指定した位置に可能性のある数字の位置に対応するbitを立てる 
         * input h:縦位置(0〜Consts.HeightMax-1)
         *       w:横位置(0〜Consts.WidthMax-1)
         *       iPotNum:可能性のある数字(1〜9)
         * return 引数チェックの結果
         */
        private bool setPotential(int h, int w, int iPotNum)
        {
            bool blRtn = true;
            /* 引数チェック */
            if ((check_HW(h, w) != true) || (check_iPotNum(iPotNum) != true))
            {
                blRtn = false;
                return blRtn;
            }

            if ((iPotentialsGrid[h, w] & Consts.POTs[iPotNum - 1]) == 0)
            {
                /* ビットが立っていなければビットを立てて iPotentialBits を更新する */
                iPotentialsGrid[h, w] |= Consts.POTs[iPotNum - 1];
                iPotentialBits[h, w]++;

                /* ErrorCheck */
                if (iPotentialBits[h, w] > 45)
                {
                    outPutSystemErrror("over45 " + MethodBase.GetCurrentMethod().Name);
                }
            }
            return blRtn;
        }
        /* 各Block(3*3マス)の左上のマスの全体の座標をセットする
         */
        private void initBlockPoints()
        {
            /* ■□□▲□□●□□
             * □□□□□□□□□
             * □□□□□□□□□
             * ◆□□▼□□◉□□
             * □□□□□□□□□
             * □□□□□□□□□
             * !□□#□□$□□
             * □□□□□□□□□
             * □□□□□□□□□
             * 
             * stBlockULPoints[0] = ■の座標(0,0)
             * stBlockULPoints[1] = ▲の座標(0,3)
             * stBlockULPoints[2] = ●の座標(0,6)
             * stBlockULPoints[3] = ◆の座標(3,0)
             * ・・・
             * stBlockULPoints[8] = $の座標(6,6)
             */
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    stBlockULPoints[j * 3 + i].h_idx = j * 3;
                    stBlockULPoints[j * 3 + i].w_idx = i * 3;
                }
            }
        }

        /* デバッグ用に解答チェック用の情報をセットする
         */
        public void test_setAnswerInfo(int iLv, int iNo)
        {
            test_iQLv = iLv;
            test_iQNo = iNo;
            test_blIsCheckAnswer = Consts.ENABLE_CHECK_ANS;
            test_ANS = new Tester();

        }

        /**********************************************************************/
        /****** 共通で使用する関数                                          ******/
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
            if (Consts.HeightMax <= h)
            {
                return false;
            }
            if (w < 0)
            {
                return false;
            }
            if (Consts.WidthMax <= w)
            {
                return false;
            }
            return true;
        }
        private bool check_iPotNum(int iPotNum)
        {
            /*       iPotNum:可能性のある数字(1〜9)  */
            /* をチェックする。OKならtrue            */
            if (iPotNum <= Consts.NumMin)
            {
                return false;
            }
            if (Consts.NumMax < iPotNum)
            {
                return false;
            }
            return true;
        }

        /* 引数で指定した位置に、引数で指定した入る可能性のある数字の位置に対応するbitをクリアする
         * input h:縦位置(0〜Consts.HeightMax-1)
         *       w:横位置(0〜Consts.WidthMax-1)
         *       iPotNum:可能性のある数字(1〜9)
         * return 引数チェックの結果
         */
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
            if ((iPotentialsGrid[h, w] & Consts.POTs[iPotNum - 1]) != 0)
            {
                iPotentialsGrid[h, w] ^= Consts.POTs[iPotNum - 1];
                iPotentialBits[h, w]--;
                /* エラーチェック */
                if (iPotentialBits[h, w] < 0)
                {
                    outPutSystemErrror("[E]:iPotentialBits < 0");
                    blRtn = false;
                }
            }
            else
            {
                /* フラグが立っていないのにクリアしようとした場合は警告 */
                //outPutSystemErrror("[W]:iPotentialBits is Not Enable");
            }
            return blRtn;
        }

        /* 引数で指定したPotentialsGridの値から実際の値を返す
         * POTs配列内の値と引数が一致しない場合は、候補が複数あり確定できない、またはすでに確定済みの状態でエラーとして0を返す
         * これは引数の値が入っていたPotentialsGridと同じ位置のPotentialBitsの値が1ではない場合、
         * 引数の値が入っていたPotentialsGridと同じ位置のMainGridの値が非0(値確定済み)でPotentialBitsの値が0の場合に発生する
         */
        private int convertPG2Number(int PGval)
        {
            int rtnNumber = 0;
            for (int i = 0; i < Consts.NINE; i++)
            {
                if (Consts.POTs[i] == PGval)
                {
                    rtnNumber = i + 1;
                }
            }
            return rtnNumber;
        }

        /* 終了チェック関数(true:終了、false:未完了)
         */
        private bool isFinish()
        {
            bool rtn = true;
            for (int j = 0; j < Consts.HeightMax; j++)
            {
                for (int i = 0; i < Consts.WidthMax; i++)
                {
                    if (iPotentialBits[j, i] != 0)
                    {
                        rtn = false;
                        return rtn;
                    }
                }
            }
            return rtn;
        }

        private void clrPotentialsGrid(int h_idx, int w_idx, int iPotNum)
        {
            /* フラグが立っていればクリアする */
            if ((iPotentialsGrid[h_idx, w_idx] & Consts.POTs[iPotNum - 1]) != 0)
            {
                iPotentialsGrid[h_idx, w_idx] ^= Consts.POTs[iPotNum - 1];
                iPotentialBits[h_idx, w_idx]--;
                /* エラーチェック */
                if (iPotentialBits[h_idx, w_idx] < 0)
                {
                    outPutSystemErrror(MethodBase.GetCurrentMethod().Name);
                }
            }
        }

        /*
         * 
         */
        public void SolberMain()
        {
            bool Rtn;
            /* 問題の初期値から初期ポテンシャルグリッドを作成する */
            updatePotentialsGridByMainGrid();
            /* 初期ポテンシャルグリッドから明示的に解決できる値を埋める処理を繰り返す */
            Rtn = updateMainGridByPotentialsGrid();

            if (Rtn == true)
            {
                /* 解決済みなら結果を表示 */
                DispGrid(iMainGrid);
            }
            else
            {
                DispPotentialGridAll();
                DispGrid(iMainGrid);

                removePossibilityWithConbination();
            }











        }

        private void updatePotentialsGridByMainGrid()
        {
#if true
            for (int n = 0; n < Consts.NINE; n++)
            {
                updatePotentialsGridByMainGrid(n, 0, Consts.Type_YOKO);
                updatePotentialsGridByMainGrid(0, n, Consts.Type_TATE);
                updatePotentialsGridByMainGrid(n, Consts.Type_BLOCK);
            }
#else
            for (int n = 0; n < Consts.NINE; n++)
            {
                updatePotentialsGridByMainGrid(n, 0, Consts.Type_YOKO);
            }
            for (int n = 0; n < Consts.NINE; n++)
            {
                updatePotentialsGridByMainGrid(0, n, Consts.Type_TATE);
            }
            for (int n = 0; n < Consts.NINE; n++)
            {
                //updatePotentialsGridByMainGrid(n, Consts.Type_BLOCK);
            }
#endif

        }
        private void updatePotentialsGridByMainGrid(int BlockNumber, int iType)
        {
            updatePotentialsGridByMainGrid(stBlockULPoints[BlockNumber].h_idx, stBlockULPoints[BlockNumber].w_idx, iType);
        }
        private void updatePotentialsGridByMainGrid(int hIdx, int wIdx, int iType)
        {
            NineData ND = new NineData();
            ND.setTarget(hIdx, wIdx, iType, in iMainGrid, in iPotentialsGrid, in iPotentialBits);
            ND.updatePGbyMG();
            ND.rverseDatas(ref iMainGrid, ref iPotentialsGrid, ref iPotentialBits);
        }

        private void updatePotentialsGridByMainGrid(int[,] changedMG)
        {
            for (int j = 0; j < Consts.WidthMax; j++)
            {
                for (int i = 0; i < Consts.HeightMax; i++)
                {
                    if (changedMG[j, i] != 0)
                    {
                        /* 対象セルを含む横、縦、ブロックのポテンシャルグリッドの内容を更新する */
                        updatePotentialsGridByMainGrid(new stPoints(j, i));
                        /* 変更済みなので元に戻す */
                        changedMG[j, i] = 0;
                    }
                }
            }
        }
        private void updatePotentialsGridByMainGrid(stPoints target)
        {
            updatePotentialsGridByMainGrid(target.h_idx, target.w_idx, Consts.Type_YOKO);
            updatePotentialsGridByMainGrid(target.h_idx, target.w_idx, Consts.Type_TATE);
            updatePotentialsGridByMainGrid(target.h_idx, target.w_idx, Consts.Type_BLOCK);
        }

        private bool updateMainGridByPotentialsGrid()
        {
            int iLimit = Consts.HeightMax * Consts.WidthMax * Consts.NINE;
            int iLoopCounter = 0;

            int[,] changedMainGrid = new int[Consts.HeightMax, Consts.WidthMax];


            bool isChangeMG = updateMainGridByPotentialsGridSub(ref changedMainGrid);

            while (isChangeMG == true)
            {
                updatePotentialsGridByMainGrid(changedMainGrid);
                isChangeMG = updateMainGridByPotentialsGridSub(ref changedMainGrid);
                //DispPotentialGridAll();
                iLoopCounter++;
                if (iLimit < iLoopCounter)
                {
                    outPutSystemErrror("loop limit" + MethodBase.GetCurrentMethod().Name);
                    break;
                }
            }

            /* 終了チェック */
            return isFinish();


        }
        private bool updateMainGridByPotentialsGridSub(ref int[,] changedMG)
        {
            bool isChangedMainGridValue = false;
            for (int j = 0; j < Consts.HeightMax; j++)
            {
                for (int i = 0; i < Consts.WidthMax; i++)
                {
                    if (iPotentialBits[j, i] == 1)
                    {
                        int setValue = convertPG2Number(iPotentialsGrid[j, i]);
                        if (setValue != 0)
                        {
                            if (test_checkAnswer(j, i, setValue))
                            {
                                iMainGrid[j, i] = setValue;
                                clrPotentialsGrid(j, i, setValue);

                                isChangedMainGridValue = true;
                                changedMG[j, i] = setValue;
                            }
                            else
                            {
                                /* 何か処理を間違えている */
                                int breakpoint = 0;
                                while (true)
                                {
                                    breakpoint = 1;
                                }
                            }
                        }
                        else
                        {
                            /* ここに来てはいけない */
                            changedMG[j, i] = -1;
                            outPutSystemErrror(MethodBase.GetCurrentMethod().Name);
                        }

                    }
                    else
                    {
                        /* 変更がないマスは0で初期化 */
                        changedMG[j, i] = 0;
                    }

                }
            }

            return isChangedMainGridValue;
        }

        private void removePossibilityWithConbination()
        {

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
            String Waku_Upper = "┏━┯━┯━┳━┯━┯━┳━┯━┯━┳━┯━┯━┳━┯━┯━┳━┯━┯━┳━┯━┯━┳━┯━┯━┳━┯━┯━┓";
            String Waku_middle1 = "┠─┼─┼─╂─┼─┼─╂─┼─┼─╂─┼─┼─╂─┼─┼─╂─┼─┼─╂─┼─┼─╂─┼─┼─╂─┼─┼─┨";
            String Waku_middle2 = "┣━┿━┿━╋━┿━┿━╋━┿━┿━╋━┿━┿━╋━┿━┿━╋━┿━┿━╋━┿━┿━╋━┿━┿━╋━┿━┿━┫";
            String Waku_buttom = "┗━┷━┷━┻━┷━┷━┻━┷━┷━┻━┷━┷━┻━┷━┷━┻━┷━┷━┻━┷━┷━┻━┷━┷━┻━┷━┷━┛";



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
                            if ((iPotentialsGrid[j, i] & Consts.POTs[k * 3 + l]) == 0)
                            {
                                Console.Write(" ");
                            }
                            else
                            {
                                Console.Write("{0}", (k * 3 + l) + 1);
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
                        if (j * 3 + jk < Consts.HeightMax * 3 - 1)
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

            Console.WriteLine("Potential of [{0},{1}] = {2}({3}bits Enbale)", h, w, iPotentialsGrid[h, w], iPotentialBits[h, w]);
            for (int k = 0; k < 3; k++)
            {
                for (int l = 0; l < 3; l++)
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

        public bool test_checkAnswer(int hIdx, int wIdx, int checkVal)
        {
            if (test_blIsCheckAnswer == Consts.ENABLE_CHECK_ANS)
            {
                if (test_ANS.getAnswer(test_iQLv, test_iQNo, hIdx, wIdx) == checkVal)
                {
                    return true;
                }
                else
                {
                    outPutSystemErrror(MethodBase.GetCurrentMethod().Name);
                    outPutSystemErrror("");
                    outPutSystemErrror("<<<<<<<<<< =============== >>>>>>>>>>");
                    outPutSystemErrror("<<<<<<<<<< Important Error >>>>>>>>>>");
                    outPutSystemErrror("<<<<<<<<<< =============== >>>>>>>>>>");
                    outPutSystemErrror("");

                    return false;
                }
            }
            else
            {
                /* 解答チェックするフラグが立っていなければ無条件で正解とする */
                return true;
            }
        }


        /* デバッグ用エラー出力 */
        /* outPutSystemErrror(MethodBase.GetCurrentMethod().Name); */
        private void outPutSystemErrror(String str)
        {
            Console.Error.WriteLine("Error!!! -----> : " + str);
        }
    }

    public class Solber_test
    {
        int[,] iQuestGrid;
        Solber sol;
        int QuestionLv;
        int QuestionNo;
        int QuestionNumMax;
        Tester tester;
        public Solber_test()
        {
            tester = new Tester();
            executTestAll();
        }

        public void executTestAll()
        {
            Console.WriteLine("<<< Test Start >>>");
            executeTest_EasyLv();
            executeTest_NormalLv();
            Console.WriteLine("<<< All Test Passed !! >>>");

        }

        public void executeTest_EasyLv()
        {
            Console.WriteLine("<<< EasyLv Test Start >>>");
            iQuestGrid = new int[Consts.HeightMax, Consts.WidthMax];

            QuestionLv = Consts.QUESTION_Lv_Easy;                             /* ←ここで問題のLevelを指定 */
            QuestionNo = 0;                                                     /* ←ここで使用する問題のNoを指定 */
            QuestionNumMax = tester.getQuestionNumMax(Consts.QUESTION_Lv_Easy);

            for(QuestionNo = 0; QuestionNo < QuestionNumMax; QuestionNo++)
            {
                Console.WriteLine("<<< Question No:{0} >>>", QuestionNo);
                tester.getQuestion(QuestionLv, QuestionNo, ref iQuestGrid);
                sol = new Solber();
                sol.setQuestion(iQuestGrid);
                sol.test_setAnswerInfo(QuestionLv, QuestionNo);                 
                sol.SolberMain();
            }
        }
        public void executeTest_NormalLv()
        {

        }


    }

}
