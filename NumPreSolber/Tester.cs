using System;
namespace NumPreSolber
{
    public class Tester
    {
        public int[,,] iQuestions;
        public Tester()
        {
            iQuestions = new int[,,]

            {
                {   /* No0 */
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
                {   /* No1 */
                    {0,1,2,3,4,5,6,7,8 },
                    {9,8,7,6,5,4,3,2,1 },
                    {0,1,2,3,4,5,6,7,8 },
                    {9,8,7,6,5,4,3,2,1 },
                    {0,1,2,3,4,5,6,7,8 },
                    {9,8,7,6,5,4,3,2,1 },
                    {0,1,2,3,4,5,6,7,8 },
                    {9,8,7,6,5,4,3,2,1 },
                    {0,1,2,3,4,5,6,7,8 }
                }
            };
        }

        public void getQuestion(int testNumber, ref int[,] iGrid)
        {
            for(int j = 0; j <  Consts.WidthMax; j++)
            {
                for(int i = 0; i < Consts.HeightMax; i++)
                {
                    iGrid[j, i] = iQuestions[testNumber, j, i];
                }
            }
        }
    }
}
