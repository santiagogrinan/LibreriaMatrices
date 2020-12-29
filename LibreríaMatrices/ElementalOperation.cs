using System;
using System.Collections.Generic;
using System.Text;

namespace LibreríaMatrices
{
    class ElementalOperation
    {
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        #region Public Functions
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::: 
        //=====================================================================
        public static void Operation(Matrix matrix, Data data)
        {
            if (data is DataOperationI)
            {
                DataOperationI dataI = (DataOperationI)data;
                RowsElementalOperationI(matrix, dataI.Row_1, dataI.Row_2);
                return;
            }

            if (data is DataOperationII)
            {
                DataOperationII dataII = (DataOperationII)data;
                RowsElementalOperationII(matrix, dataII.Row, dataII.Scalar);
                return;
            }

            if (data is DataOperationI)
            {
                DataOperationIII dataIII = (DataOperationIII)data;
                RowsElementalOperationIII(matrix, dataIII.RowToTransfor, dataIII.Scalar, dataIII.RowOrigin);
                return;
            }
        }

        //=====================================================================
        public static double Determinante(Data data)
        {
            if (data is DataOperationI) return -1;
            if (data is DataOperationII) return ((DataOperationII)data).Scalar;
            if (data is DataOperationIII) return 1;

            throw new Exception("DataType is not defined");
        }

        //=====================================================================
        public static Matrix ElementalMatrix(Data data, int size)
        {
            Matrix result = Matrix.NewIdentityMatrix(size);
            Operation(result, data);
            return result;
        }

        #endregion
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        #region Public Class
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::: 
        //=====================================================================
        public abstract class Data
        {
            public static Data DataOperationI(int row_1, int row_2)
            {
                return new DataOperationI(row_1, row_2);
            }

            public static Data DataOperationII(int row, double scalar)
            {
                return new DataOperationII(row, scalar);
            }

            public static Data DataOperationIII(int rowToTransfor, double scalar, int rowOrigin)
            {
                return new DataOperationIII(rowToTransfor, scalar, rowOrigin);
            }
        }

        #endregion
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        #region Private Function
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::: 
        //=====================================================================
        static void RowsElementalOperationI(Matrix matrix, int row_1, int row_2)
        {
            if (!(row_1 < matrix.Rows) || !(row_2 < matrix.Cols)) throw new Exception("Row is bigger than matrix number row");

            for (int j = 0; j < matrix.Cols; j++)
            {
                double auxValue = matrix[row_1, j];
                matrix[row_1, j] = matrix[row_2, j];
                matrix[row_2, j].Value = auxValue;
            }
        }

        //=====================================================================
        static void RowsElementalOperationII(Matrix matrix, int row, double scalar)
        {
            if (!(row < matrix.Rows)) throw new Exception("Row is bigger than matrix number row");

            for (int j = 0; j < matrix.Cols; j++)
                matrix[row, j].Value = matrix[row, j] * scalar;
        }

        //=====================================================================
        static void RowsElementalOperationIII(Matrix matrix, int rowToTransfor, double scalar, int rowOrigin)
        {
            if (!(rowToTransfor < matrix.Rows) || !(rowOrigin < matrix.Cols)) throw new Exception("Row is bigger than matrix number row");

            for (int j = 0; j < matrix.Cols; j++)
                matrix[rowToTransfor, j].Value = matrix[rowToTransfor, j] + matrix[rowOrigin, j] * scalar;
        }

        #endregion
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        #region Data Class Implementation
        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::: 
        class DataOperationI : Data
        {
            internal DataOperationI(int row_1, int row_2)
            {
                Row_1 = row_1;
                Row_2 = row_2;
            }
            internal int Row_1 { get; private set; }
            internal int Row_2 { get; private set; }
        }

        class DataOperationII : Data
        {
            internal DataOperationII(int row, double scalar)
            {
                Row = row;
                Scalar = scalar;
            }

            internal int Row { get; private set; }
            internal double Scalar { get; private set; }
        }

        class DataOperationIII : Data
        {
            internal DataOperationIII(int rowToTransfor, double scalar, int rowOrigin)
            {
                RowToTransfor = rowToTransfor;
                Scalar = scalar;
                RowOrigin = rowOrigin;
            }

            internal int RowToTransfor { get; private set; }
            internal double Scalar { get; private set; }
            internal int RowOrigin { get; private set; }
        }
        #endregion
    }
}
