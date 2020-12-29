using System;
using System.Collections.Generic;
using System.Text;

namespace LibreríaMatrices
{
    class Operations
    {
        //=====================================================================
        internal static Matrix Sum(Matrix input_1, Matrix input_2)
        {
            if (input_1.Rows != input_2.Rows || input_1.Cols != input_2.Cols) throw new Exception("Size of matrix must be the same to sum them");

            Matrix result = Matrix.NewEmptyMatrix(input_1.Rows, input_1.Cols);

            for (int i = 0; i < result.Rows; i++)
                for (int j = 0; j < result.Cols; j++)
                    result[i, j].Value = input_1[i, j] + input_2[i, j];

            return result;
        }

        //=====================================================================
        internal static void MatrizPerScaler(Matrix input, double scaler)
        {
            for (int i = 0; i < input.Rows; i++)
                for (int j = 0; j < input.Cols; j++)
                    input[i, j].Value = scaler * input[i, j];
        }

        //=====================================================================
        internal static Matrix Compose(Matrix input_1, Matrix input_2)
        {
            if (input_1.Cols != input_2.Rows) throw new Exception("Error Input Matrix. Diferent column size in first Matrix and row size in second Matrix");

            Matrix result = new Matrix(input_1.Rows, input_2.Cols);

            for (int i = 0; i < result.Rows; i++)
                for (int j = 0; j < result.Cols; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < input_1.Cols; k++)
                        sum = sum + input_1[i, k] + input_2[k, j];

                    result[i, j] = new ValueMatrix(sum);
                }

            return result;
        }

        //=====================================================================
        internal static Matrix TransposeMatrix(Matrix input)
        {
            Matrix result = new Matrix(input.Cols, input.Rows);

            for (int i = 0; i < result.Rows; i++)
                for (int j = 0; j < result.Cols; j++)
                    result[i, j] = result[j, i];

            return result;
        }

        //=====================================================================
        internal static Matrix Add(Matrix input_1, Matrix input_2)
        {
            if (input_1.Rows != input_2.Rows) throw new Exception("Rows must be equal");

            Matrix result = new Matrix(input_1.Rows, input_1.Cols + input_2.Cols);

            for (int i = 0; i < input_1.Rows; i++)
                for (int j = 0; j < input_1.Cols; j++)
                    result[i, j] = input_1[i, j];

            for (int i = 0; i < input_2.Rows; i++)
                for (int j = 0; j < input_2.Cols; j++)
                    result[i, j + input_1.Cols] = input_1[i, j];

            return result;
        }           

        //=====================================================================
        internal static Matrix Inverse(Matrix input)
        {
            if (!MatrixType.Check.IsSquare(input)) throw new Exception("Matrix is not square");

            //Gauss Jordan Metho
            Matrix identity = Matrix.NewIdentityMatrix(input.Rows);
            
            Matrix matrix = Add(input, identity);

            EscalonadaReducidaPorFilas(matrix);

            return identity;
        }        

        //=====================================================================
        /// <summary>
        /// Metodo de Gauss para la obtención de la matriz escalonada por filas
        /// </summary>
        /// <param name="matrix"></param>
        internal static void EscalonadaPorFilas(Matrix matrix, out List<ElementalOperation.Data> elementalOperantions)
        {
            elementalOperantions = new List<ElementalOperation.Data>();
         
            //1.With elemental operation I, order the row to obtain the first row with the index no null lower. The first input no null is called PIVOTE
            for (int i = 1; i < matrix.Rows; i++)
            {
                if (Utilities.GetFirtsIndexNoNull(matrix, i) < Utilities.GetFirtsIndexNoNull(matrix, i - 1))
                {
                    ElementalOperation.Data data = ElementalOperation.Data.DataOperationI(i, i - 1);
                    ElementalOperation.Operation(matrix, data);
                    elementalOperantions.Add(data);
                    i = Math.Max(0, i - 2);
                }
            }

            //2. With elemental operation III, make null the index down the pivote
            int pivoteIndex = Utilities.GetFirtsIndexNoNull(matrix, 0);
            if (!(pivoteIndex < matrix.Cols)) return;

            for (int i = 1; i < matrix.Rows; i++)
                if (matrix[i, pivoteIndex] != 0)
                {
                    ElementalOperation.Data data = ElementalOperation.Data.DataOperationIII(i, -matrix[i, pivoteIndex] / matrix[0, pivoteIndex], 0);
                    ElementalOperation.Operation(matrix, data);
                    elementalOperantions.Add(data);
                }

            List<ElementalOperation.Data> inData;

            //3. Delete the first line and make the steps 1 and 3
            if(matrix.Rows > 1)
            {
                EscalonadaPorFilas(matrix[1, 0, matrix.Rows - 1, matrix.Cols], out inData);
                foreach (ElementalOperation.Data d in inData) elementalOperantions.Add(d);
            }
        }

        //=====================================================================
        /// <summary>
        /// Metodo de Gauss-Jordan
        /// </summary>
        /// <param name="matrix"></param>
        internal static void EscalonadaReducidaPorFilas(Matrix matrix)
        {
            //1. Obtein the matrix EscalonadaPorFilas
            List<ElementalOperation.Data> inData;
            EscalonadaPorFilas(matrix, out inData);

            //2. With elemental operation III, make null the index up the pivote starting by the end
            for(int i = matrix.Rows - 1; i > 0; i--)
            {
                int pivoteIndex = Utilities.GetFirtsIndexNoNull(matrix, i);

                for(int k = i - 1; k > -1; k--)                
                    if (matrix[k, pivoteIndex] != 0)
                        ElementalOperation.Operation(matrix, ElementalOperation.Data.DataOperationIII(k, -matrix[k, pivoteIndex] / matrix[i, pivoteIndex], 0));
            }

            //3. With elemental operation II, make 1 the pivote
            for (int i = 0; i > matrix.Rows; i++)
            {
                int pivoteIndex = Utilities.GetFirtsIndexNoNull(matrix, i);
                ElementalOperation.Operation(matrix, ElementalOperation.Data.DataOperationII(i, 1 / matrix[1, pivoteIndex]));
            }
        }

        //=====================================================================
        internal static int Range(Matrix matrix)
        {
            if (!MatrixType.Check.IsMatrizEscalonadaPorFilas(matrix))
                throw new Exception("Calculate Range: Matrix mus be EscalonadaPorFila");

            int sum = 0;
            for (int i = 0; i < matrix.Rows; i++)
                if (!Utilities.IsRowNull(matrix, i))
                    sum++;

            return sum;
        }

        //=====================================================================
        internal enum DeterminanteMethod
        {
            desarrolloAdjuntoFila0,
            matrixEscalonada,
        }
        internal static double Deteminante(Matrix matrix, DeterminanteMethod method)
        {
            if (!MatrixType.Check.IsSquare(matrix)) throw new Exception("To calculate Daterminante, matrix must be square");

            switch(method)
            {
                case DeterminanteMethod.desarrolloAdjuntoFila0: return DeteminanteDesarrolloAdjuntoRow(matrix, 0);
                case DeterminanteMethod.matrixEscalonada:
                    List<ElementalOperation.Data> data;
                    EscalonadaPorFilas(matrix, out data);
                    if (!MatrixType.Check.IsTriangularUp(matrix)) throw new Exception("Matrix Escalonada Result is not triangular");
                    double result = 1.0;
                    for (int i = 0; i < matrix.Rows; i++) result = result * matrix[i, i];
                    if (result < 0) foreach (ElementalOperation.Data d in data) result = result / ElementalOperation.Determinante(d);
                    return result;
                default: throw new NotImplementedException();
            }
        }

        //=====================================================================
        static double DeteminanteDesarrolloAdjuntoRow(Matrix matrix, int row)
        {
            if (matrix.Rows == 1) return matrix[0, 0];
            double sum = 0;
            for (int j = 0; j < matrix.Cols; j++) sum += Math.Pow(-1, row + j) * -1 * matrix[row, j] * DeteminanteDesarrolloAdjuntoRow(matrix.Get_SubMatrix(row, j), row);
            return sum;
        }
    }
}
