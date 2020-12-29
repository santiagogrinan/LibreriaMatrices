using System;
using System.Collections.Generic;
using System.Text;

namespace LibreríaMatrices
{
    class Utilities
    {
        internal static bool IsRowNull(Matrix input, int row)
        {
            if (!(row < input.Rows)) throw new Exception("Row is bigger than matrix number row");

            for (int i = 0; i < input.Cols; i++)
                if (input[row, i] != 0) return false;

            return true;
        }

        /// <summary>
        /// Return the number of the column which is the first no null value. If all is null, return the lenght of the row.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        internal static int GetFirtsIndexNoNull(Matrix input, int row)
        {
            if (!(row < input.Rows)) throw new Exception("Row is higher than matrix number row");

            for (int i = 0; i < input.Cols; i++)
                if (input[row, i] != 0) return i;

            return input.Rows;
        }
    }
}
