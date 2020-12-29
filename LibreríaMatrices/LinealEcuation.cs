using System;
using System.Collections.Generic;
using System.Text;

namespace LibreríaMatrices
{
    class LinealEcuations
    {
        static double[] Resolve(Ecuation[] ecuations)
        {
            for (int i = 1; i < ecuations.Length; i++)
                if (ecuations[i - 1].Data.Length != ecuations[i].Data.Length)
                    throw new Exception("Diferent Lenght in the input Ecuations");

            Matrix matrizAmpliada = new Matrix(ecuations.Length, ecuations[0].Data.Length);

            Operations.EscalonadaPorFilas(matrizAmpliada, out List<ElementalOperation.Data> elementalOperation);

            Matrix matrizIncognitas = matrizAmpliada[0, 0, matrizAmpliada.Rows, matrizAmpliada.Cols - 1];

            if (matrizAmpliada.Range > matrizIncognitas.Range) throw new Exception("El sistema es incompatrible");

            if (matrizIncognitas.Range < matrizIncognitas.Cols) throw new Exception("El sistema tiene infinitas soluciones");

            double[] result = new double[matrizIncognitas.Cols];

            for(int i = result.Length - 1; i > -1; i--)
            {
                double sum = 0;
                for (int j = result.Length - 1; j > i; i--)
                    sum = sum + result[j] * matrizIncognitas[i, j];

                result[i] = (matrizAmpliada[i, matrizAmpliada.Cols - 1] - sum) / matrizIncognitas[i, i];
            }

            return result;

        }

        public class Ecuation
        {
            public Ecuation(double[] input)
            {
                Data = input;
            }

            internal double[] Data { get; private set; }

        }

    }

    


}
