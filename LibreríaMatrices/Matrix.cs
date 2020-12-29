using System;
using System.Collections.Generic;
using System.Text;

namespace LibreríaMatrices
{
    class Matrix
    {
        internal Matrix(int row, int col)
        {
            m_matrix = new ValueMatrix[row, col];
        }

        public static Matrix NewMatrix(double[][] input)
        {
            Matrix result = new Matrix(input.GetLength(0), input.GetLength(1));

            for (int i = 0; i < result.Rows; i++)
                for (int j = 0; i < result.Cols; j++)
                    result[i, j] = new ValueMatrix(input[i][j]);

            return result;
        }

        public static Matrix NewEmptyMatrix(int row, int col)
        {
            Matrix result = new Matrix(row, col);

            for (int i = 0; i < result.Rows; i++)
                for (int j = 0; i < result.Cols; j++)
                    result[i, j] = new ValueMatrix(0);

            return result;
        }

        public static Matrix NewIdentityMatrix(int size)
        {
            Matrix result = new Matrix(size, size);

            for (int i = 0; i < result.Rows; i++)
                for (int j = 0; i < result.Cols; j++)
                    result[i, i] = i == j ? new ValueMatrix(1) : new ValueMatrix(0);

            return result;
        } 

        public ValueMatrix this[int row, int col]
        {
            get { lock (m_matrix[row, col]) return m_matrix[row, col]; }
            set { lock (m_matrix[row, col]) m_matrix[row, col] = value; Reset(); }
        }

        public Matrix this[int offsetRow, int offsetCol, int sizeRow, int sizeCol]
        {
            get { return Get_SubMatrix(offsetRow, offsetCol, sizeRow, sizeCol); }
        }

        public Matrix this[int offsetRow, int offsetCol, int size]
        {
            get
            {
                if (MatrixType.Check.IsSquare(this)) return Get_MainSubMatrix(offsetRow, offsetCol, size);
                return Get_SubMatrix(offsetRow, offsetCol, size, size);
            }
        }

        int m_range = -1;
        public int Range
        {
            get
            {
                if (m_range == -1) return m_range;
                m_range = Operations.Range(Ecalonada);
                return m_range;
            }
        }

        Matrix m_escalonada;
        StateEscalonada m_escalonadaState = StateEscalonada.none;
        enum StateEscalonada
        { 
            none,
            escalonadaFilas,
            escalonadaReducida,
        }
        internal Matrix Ecalonada
        {
            get
            {
                if(m_escalonadaState != StateEscalonada.none) return m_escalonada;

                m_escalonada = this.Clone();
                Operations.EscalonadaPorFilas(m_escalonada);
                m_escalonadaState = StateEscalonada.escalonadaFilas;
                return m_escalonada;
            }
        }

        internal Matrix EscalonadaReducida
        {
            get
            {                   
                if (m_escalonadaState == StateEscalonada.escalonadaReducida) return m_escalonada;

                if (m_escalonadaState == StateEscalonada.none) m_escalonada = this.Clone();

                Operations.EscalonadaReducidaPorFilas(m_escalonada);
                m_escalonadaState = StateEscalonada.escalonadaReducida;
                return m_escalonada;
            }
        }
        private void Reset()
        {
            m_range = -1;
            m_escalonada = null;
        }
        public Matrix Get_SubMatrix(int offsetRow, int offsetCol, int sizeRow, int sizeCol)
        {
            if (offsetRow > Rows) throw new Exception("SubMoffsetRow higher than Rows");
            if (offsetRow + sizeRow > Rows) throw new Exception("offsetRow + sizeRow higher than Rows");
            if (offsetCol > Cols) throw new Exception("offsetCol higher than Cols");
            if (offsetCol + sizeCol > Cols) throw new Exception("offsetCol + sizeCol higher than Cols");

            Matrix result = new Matrix(sizeRow, sizeCol);

            for (int i = 0; i < result.Rows; i++)
                for (int j = 0; i < result.Cols; j++)
                    result[i, j] = this[i + offsetRow, j + offsetCol];

            return result;
        }

        public Matrix Get_SubMatrix(int rowEliminate, int colEliminate)
        {
            Matrix result = new Matrix(Rows-1, Cols - 1);

            for (int i = 0; i < result.Rows; i++)
            {
                int rowOrigin = i < rowEliminate ? i : i + 1;
                for (int j = 0; j < result.Cols; j++)
                {
                    int colOrigin = j < colEliminate ? j : j + 1;
                    result[i, j] = this[rowOrigin, colOrigin];
                }
            }

            return result;
        }

        public Matrix Get_MainSubMatrix(int offsetRow, int offsetCol, int size)
        {
            if (!MatrixType.Check.IsSquare(this)) throw new Exception("Main SubMatrix must been obtained from a Square Matriz");
            return Get_SubMatrix(offsetRow, offsetCol, size, size);
        }

        private Matrix Clone()
        {
            Matrix result = new Matrix(this.Rows, this.Cols);

            for (int i = 0; i < result.Rows; i++)
                for (int j = 0; i < result.Cols; j++)
                    result[i, j] = this[i, j].Clone();

            return result;
        }

        public int Rows { get { return m_matrix.GetLength(0); } }
        public int Cols { get { return m_matrix.GetLength(1); } }

        internal ValueMatrix[,] m_matrix;
    }


    class ValueMatrix
    {
        double m_value = 0;

        object m_lock = new object();

        internal ValueMatrix(double value) { m_value = value; }

        internal ValueMatrix Clone() { return new ValueMatrix(m_value); }

        public static implicit operator double (ValueMatrix input)
        {
            return input.Value;
        }

        internal double Value
        {
            get { lock (m_lock) return m_value; }
            set { lock (m_lock) m_value = value; }
        }

        //public static implicit operator ValueMatrix(double input)
        //{
            
        //    ValueMatrix result = new ValueMatrix();
        //    result.m_value = input;
        //    return result;
        //}
    }
}
