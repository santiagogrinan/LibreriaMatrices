using System;
using System.Collections.Generic;
using System.Text;

namespace LibreríaMatrices
{
    class MatrixType
    {
        public class Check
        {
            //=====================================================================
            internal static bool IsRow(Matrix input)
            {
                return input.Rows == 1 && input.Cols > 0;
            }

            //=====================================================================
            internal static bool IsCol(Matrix input)
            {
                return input.Cols == 1 && input.Rows > 0;
            }

            //=====================================================================
            internal static bool IsNull(Matrix input)
            {
                for (int i = 0; i < input.Rows; i++)
                    for (int j = 0; j < input.Cols; j++)
                        if (input[i, j] != 0)
                            return false;

                return true;
            }

            //=====================================================================
            internal static bool IsSquare(Matrix input)
            {
                return input.Rows == input.Cols;
            }

            //=====================================================================
            internal static bool IsTriangularUp(Matrix input)
            {
                if (!IsSquare(input)) return false;

                for (int i = 1; i < input.Rows; i++)
                    for (int j = 0; j < i; j++)
                        if ((double)input[i, j] != 0) return false;

                return true;
            }

            //=====================================================================
            internal static bool IsTriangularDown(Matrix input)
            {
                if (!IsSquare(input)) return false;

                for (int i = 0; i < input.Rows - 1; i++)
                    for (int j = i + 1; j < input.Cols; j++)
                            if ((double)input[i, j] != 0) return false;

                return true;
            }

            //=====================================================================
            internal static bool IsDiagonal(Matrix input)
            {
                if (!IsTriangularUp(input)) return false;

                if (!IsTriangularDown(input)) return false;

                return true;
            }

            //=====================================================================
            internal static bool IsSymmetric(Matrix input)
            {
                if (!IsSquare(input)) return false;

                Matrix tranpose = Operations.TransposeMatrix(input);

                return tranpose.m_matrix == input.m_matrix;
            }

            //=====================================================================
            internal static bool IsAntiSymmetric(Matrix input)
            {
                if (!IsSquare(input)) return false;

                Matrix tranpose = Operations.TransposeMatrix(input);
                
                tranpose = Operations.MatrizPerScaler(tranpose, -1);

                return tranpose.m_matrix == input.m_matrix;
            }

            //=====================================================================
            internal static bool IsMatrizEscalonadaPorFilas(Matrix input)
            {
                //If any row null, check if is down
                for (int i = 0; i < (input.Rows - 1); i++)
                    if (Utilities.IsRowNull(input, i) && !Utilities.IsRowNull(input, i + 1))
                        return false;

                //The first index no null of the row is in the right of the first index no null of the last row
                for (int i = 1; i < input.Rows; i++)
                    if (!(Utilities.GetFirtsIndexNoNull(input, i) > Utilities.GetFirtsIndexNoNull(input, i - 1))) return false;

                return true;
            }

            //=====================================================================
            internal static bool IsMatrixEscalonadaReducidaPorFilas(Matrix input)
            {
                if (!IsMatrizEscalonadaPorFilas(input)) return false;

                //Fist index no null in any row is 1 and is the unique no null index in his column
                for (int i = 0; i < input.Rows; i++)
                {
                    int index = Utilities.GetFirtsIndexNoNull(input, i);

                    if (input[i, index] != 1) return false;

                    for (int j = 0; j < input.Rows; j++)
                        if (j != i)
                            if (input[j, index] != 0)
                                return false;
                }

                return true;
            }

            //=====================================================================
            internal static bool IsNotSingularity(Matrix matrix)
            {
                if (!IsSquare(matrix)) return false;
                return matrix.Range == matrix.Rows;
            }

            //=====================================================================
            internal static bool IsIdentity(Matrix input)
            {
                if (!IsDiagonal(input)) return false;

                for (int i = 0; i < input.Rows; i++)
                    if (input[i, i] != 1) return false;

                return true;
            }
        }
    }
}
