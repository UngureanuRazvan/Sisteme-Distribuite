using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ParallelTest
{
    public class Program
    {
        const int value = 100000000;
        
        static void SimpleParallel()
        {
            ParallelOptions ops = new ParallelOptions();
            ops.MaxDegreeOfParallelism = Environment.ProcessorCount;
            var psClass = new ParallelSeqClass();
            
            Console.Error.WriteLine("Simple loop Test \n");

            Console.Error.WriteLine("Executing sequential loop...");
            Stopwatch watch = Stopwatch.StartNew();
            psClass.SequentialMethod(value);
            watch.Stop();
            Console.WriteLine("Sequential Method:" + watch.ElapsedMilliseconds + " Miliseconds");

            Console.Error.WriteLine("Executing parallel loop...");
            watch = Stopwatch.StartNew();
            psClass.ParallelMethod(ops, value);
            //psClass.ParallelMethod(value);
            watch.Stop();
            Console.WriteLine("Parallel Method: " + watch.ElapsedMilliseconds + " Miliseconds");

            
        }
        //ToDo Excel graph based on a number of tests
         static void MatrixParallel()
        {   //mxn || nxp == mxp
            int colCount = 1000;
            int rowCount = 1000;
            //int colCount2 = 270;
            var matrixTest = new MatricesMultiplication();

            double[,] m1 = matrixTest.InitializeMatrix(rowCount, colCount);
            double[,] m2 = matrixTest.InitializeMatrix(rowCount, colCount);
            //double[,] result = new double[rowCount, colCount2];
            double[,] result = new double[rowCount, colCount];
            Console.Error.WriteLine("\nMatrix Multiplication Test ");
            //Sequential version.
            Console.Error.WriteLine("Executing sequential loop...");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            matrixTest.MultiplyMatricesSequential(m1, m2, result);
            stopwatch.Stop();
            Console.Error.WriteLine("Sequential loop time: {0}", stopwatch.ElapsedMilliseconds + " milliseconds");

            stopwatch.Reset();
            result = new double[rowCount, colCount];

            //Parallel loop.
            Console.Error.WriteLine("Executing parallel loop...");
            stopwatch.Start();
            matrixTest.MultiplyMatricesParallel(m1, m2, result);
            stopwatch.Stop();
            Console.Error.WriteLine("Parallel loop time: {0}", stopwatch.ElapsedMilliseconds + " milliseconds");

            Console.Error.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            //SimpleParallel();

            MatrixParallel();
        }

    }
}
