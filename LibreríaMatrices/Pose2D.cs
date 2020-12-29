using System;
using System.Collections.Generic;
using System.Text;

namespace LibreríaMatrices
{
    class Pose2D
    {
        enum SistemasCoordenadas
        {
            Cartesianas,
            Polares,
        }
        static void GetCoordenadas(Pose2D pose, SistemasCoordenadas sistema, out double index0, out double index1, out double index2)
        {
            switch(sistema)
            {
                case SistemasCoordenadas.Cartesianas: index0 = pose.X; index1 = pose.Y; index2 = pose.AngZ; return;
                case SistemasCoordenadas.Polares: CartesianasToPolar(pose.X, pose.Y, pose.AngZ, out index0, out index1, out index2); return;
                default: throw new NotImplementedException();
            }
        }

        static void PolarToCartesianas(double radio, double angulo, double nose, out double x, out double y, out double angZ)
        {
            x = Math.Cos(angulo) * radio;
            y = Math.Sin(angulo) * radio;
            angZ = nose;
        }

        static void CartesianasToPolar(double x, double y, double angZ, out double radio, out double angulo, out double nose)
        {
            radio = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            angulo = Math.Atan(y / x);
            nose = angZ;
        }

        static Matrix RotateMatrix2D(double ang)
        {
            double[,] values = new double[2,2];
            values[0,0] = Math.Cos(ang);
            values[0,1] = -Math.Sin(ang);
            values[1,0] = Math.Sin(ang);
            values[1,1] = Math.Cos(ang);

            return Matrix.NewMatrix(values);
        }

        double X;
        double Y;
        double AngZ;
    }

    class Point2D
    {
        double index0;
        double index1;
        double index2;
    }
}
