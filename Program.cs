using System;
using System.IO;
using System.Linq;

namespace Klasteryzacja
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = readData();
            data = convert(data);

            double[] unknown = new double[] { 0.64, 0.26, 0 };

            var numberOfIris = classify(unknown, data, 3, 4);
            switch (numberOfIris)
            {
                case 1:
                    Console.WriteLine("Iris-setosa");
                    break;
                case 2:
                    Console.WriteLine("Iris-versicolor");
                    break;
                case 3:
                    Console.WriteLine("Iris-virginica");
                    break;
                default:
                    Console.WriteLine("ERROR");
                    break;
            }
            Console.ReadKey();

        }
        static int classify(double[] unknown, double[][] trainData, int numClasses, int k)
        {
            int n = trainData.Length;
            IndexAndDistance[] info = new IndexAndDistance[n];
            for (int i = 0; i < n; ++i)
            {
                IndexAndDistance curr = new IndexAndDistance();
                double dist = distance(unknown, trainData[i]);
                curr.idx = i;
                curr.dist = dist;
                info[i] = curr;
            }
            Array.Sort(info);
           return Vote(info,trainData,numClasses,k);
        }
        static double[][] readData()
        {
            string[] lines = File.ReadAllLines(@"D:\Semestr 4\Systemy sztucznej inteligencji\IRIS.txt");


            double[][] data = new double[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                string[] tmp = lines[i].Split(',');

                data[i] = new double[tmp.Length];

                for (int j = 0; j < tmp.Length - 1; j++)
                {
                    Console.WriteLine(tmp[j]);
                    data[i][j] = Convert.ToDouble(tmp[j].Replace('.', ','));

                }
                switch (tmp[tmp.Length - 1])
                {
                    case "Iris-setosa":
                        data[i][tmp.Length - 1] = 1;
                        break;
                    case "Iris-versicolor":
                        data[i][tmp.Length - 1] = 2;
                        break;
                    case "Iris-virginica":
                        data[i][tmp.Length - 1] = 3;
                        break;
                    default:
                        data[i][tmp.Length - 1] = 0;
                        break;
                }
                //NORMALIZACJA

                double minimum = data[i].Take(tmp.Length - 1).Min();                //Take wycina tablice
                double maximum = data[i].Take(tmp.Length - 1).Max();
                for (int j = 0; j < tmp.Length - 1; j++)
                {
                    data[i][j] = normalize(data[i][j], minimum, maximum);

                }
            }
            return data;
        }
        static double normalize(double value, double minimum, double maximum)
        {
            double newmin = 0;
            double newmax = 1;

            value = (double)((value - minimum) / (maximum - minimum) * (newmax - newmin) + newmin);
            Console.WriteLine(value);
            return value;

        }

        static double[][] convert(double[][] tmp)
        {
            double[][] data = new double[tmp.Length][];
            for (int i = 0; i < tmp.Length; i++)
            {
                data[i] = new double[3];

                data[i][0] = tmp[i][1];
                data[i][1] = tmp[i][2];
                data[i][2] = tmp[i][4];


            }
            return data;
        }

            static double distance(double[] unknown, double[] data)
            {
                double sum = 0.0;
                for (int i = 0; i < unknown.Length; ++i)
                    sum += (unknown[i] - data[i]) * (unknown[i] - data[i]);
                return Math.Sqrt(sum);
            }
            static int Vote(IndexAndDistance[] info, double[][] trainData, int numClasses, int k)
            {
                int[] votes = new int[numClasses];  // One cell per class
                for (int i = 0; i < k; ++i)
                {                                       // Just first k
                    int idx = info[i].idx;            // Which train item
                    int c = (int)trainData[idx][2];   // Class in last cell
                    ++votes[c];
                }
                int mostVotes = 0;
                int classWithMostVotes = 0;
                for (int j = 0; j < numClasses; ++j)
                {
                    if (votes[j] > mostVotes)
                    {
                        mostVotes = votes[j];
                        classWithMostVotes = j;
                    }
                }
                return classWithMostVotes;
            }
        }
    }