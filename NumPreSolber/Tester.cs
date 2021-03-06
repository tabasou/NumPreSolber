using System;
namespace NumPreSolber
{
    public class Tester
    {
        public int[,,] iQuestions_EasyLv;
        private int[,,] iAnswer_EasyLv;
        public Tester()
        {
            /* EasyLevel */
            iQuestions_EasyLv = new int[,,]
            {
                /* Question */
                {   /* No 0 */
                    {0,0,1,9,8,4,7,6,0 },
                    {6,0,0,0,5,7,0,0,0 },
                    {8,0,7,0,1,0,0,0,0 },
                    {9,6,0,3,0,8,1,0,5 },
                    {1,8,5,0,2,0,0,7,3 },
                    {3,0,0,0,0,0,2,0,8 },
                    {2,1,0,0,0,0,0,3,6 },
                    {0,0,0,1,0,0,0,0,4 },
                    {0,9,6,0,0,2,5,1,0 },
                },
                {   /* No 1 */
                    {0,0,0,8,0,5,0,1,3 },
                    {0,0,0,2,0,3,6,0,0 },
                    {6,0,0,0,9,0,2,0,4 },
                    {0,0,0,0,0,0,0,0,5 },
                    {0,4,0,1,0,0,7,0,6 },
                    {2,5,6,3,0,4,8,9,0 },
                    {5,9,0,0,0,7,1,0,2 },
                    {1,0,2,0,8,0,4,7,0 },
                    {0,0,4,9,1,0,0,3,8 },
                },
                {   /* No 2 */
                    {0,6,0,0,8,0,4,2,0 },
                    {0,1,5,0,6,0,3,7,8 },
                    {0,0,0,4,0,0,0,6,0 },
                    {1,0,0,6,0,4,8,3,0 },
                    {3,0,6,0,1,0,7,0,5 },
                    {0,8,0,3,5,0,0,0,0 },
                    {8,3,0,9,4,0,0,0,0 },
                    {0,7,2,1,3,0,9,0,0 },
                    {0,0,9,0,2,0,6,1,0 }
                }
            };
            iAnswer_EasyLv = new int[,,]
            {
                {
                    {5,3,1,9,8,4,7,6,2 },
                    {6,4,9,2,5,7,3,8,1 },
                    {8,2,7,6,1,3,4,5,9 },
                    {9,6,2,3,7,8,1,4,5 },
                    {1,8,5,4,2,9,6,7,3 },
                    {3,7,4,5,6,1,2,9,8 },
                    {2,1,8,7,4,5,9,3,6 },
                    {7,5,3,1,9,6,8,2,4 },
                    {4,9,6,8,3,2,5,1,7 },
                },
                {
                    {4,2,7,8,6,5,9,1,3 },
                    {9,1,5,2,4,3,6,8,7 },
                    {6,8,3,7,9,1,2,5,4 },
                    {8,7,1,6,2,9,3,4,5 },
                    {3,4,9,1,5,8,7,2,6 },
                    {2,5,6,3,7,4,8,9,1 },
                    {5,9,8,4,3,7,1,6,2 },
                    {1,3,2,5,8,6,4,7,9 },
                    {7,6,4,9,1,2,5,3,8 },
                }
            };

            /* NormalLevel */

        }

        public void getQuestion(int QuestionLv, int testNumber, ref int[,] iGrid)
        {
            switch (QuestionLv)
            {
                case Consts.QUESTION_Lv_Easy:
                    for (int j = 0; j < Consts.WidthMax; j++)
                    {
                        for (int i = 0; i < Consts.HeightMax; i++)
                        {
                            iGrid[j, i] = iQuestions_EasyLv[testNumber, j, i];
                        }
                    }
                    break;
                default:
                    break;

            }
        }
        public int getAnswer(int Lv, int No, int hIdx, int wIdx)
        {
            int rtn = 0;
            switch (Lv)
            {
                case Consts.QUESTION_Lv_Easy:
                    rtn = iAnswer_EasyLv[No, hIdx, wIdx];
                    break;
                default:
                    break;
            }
            return rtn;
        }
        
    }
}
