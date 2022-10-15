using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Game1
{
    public class ANN
    {
        //ANN(InputsX,Hidden Neurals,Output Neurals)
        public double[] InputsX;
        public List<Neural[]> HN;
        public Neural[] ON;
        private double learnRate;
        public ActivationFunction haf;
        public ActivationFunction oaf;
        public double learnrate
        {
            get { return learnRate; }
            set { learnRate = value; }
        }
        public enum ActivationFunction
        {
            Sigmoid,
            ReLU,
            TanH,
            SoftMax,
        }
        public ANN(int x, int[] h, ActivationFunction haf, int o, ActivationFunction oaf)
        {
            learnRate = 0.6d;
            InputsX = new double[x];
            HN = new List<Neural[]>();
            Neural[] hn;
            ON = new Neural[o];

            this.haf = haf;
            this.oaf = oaf;

            for (int i = 0; i < h.Length; i++)
            {
                hn = new Neural[h[i]];
                HN.Add(hn);
            }
            for (int i = 1; i < HN.Count; i++)
            {
                for (int j = 0; j < HN[i].Length; j++)
                {
                    HN[i][j] = new Neural(HN[i - 1].Length);
                }
            }

            for (int i = 0; i < HN[0].Length; i++)
            {
                HN[0][i] = new Neural(x);
            }
            for (int i = 0; i < o; i++)
            {
                ON[i] = new Neural(HN[HN.Count - 1].Length);
            }
        }
        public void Update(double[] Target)
        {
            for (int i = 0; i < ON.Length; i++)
            {
                if (oaf == ActivationFunction.ReLU)
                {
                    //ReLU
                    ON[i].Error = DerivatReLU(ON[i].y) * (Target[i] - ON[i].y);
                }
                else if (oaf == ActivationFunction.Sigmoid)
                {
                    // Segmoid
                    ON[i].Error = ON[i].y * (1.0f - ON[i].y) * (Target[i] - ON[i].y);
                }
                 else if (oaf == ActivationFunction.TanH)
                {
                    // TanH
                    ON[i].Error = DerivatTanh(ON[i].y) * (Target[i] - ON[i].y);;
                }
            }
            for (int i = 0; i < HN[HN.Count - 1].Length; i++)
            {
                HN[HN.Count - 1][i].Error = 0.0f;
                for (int j = 0; j < ON.Length; j++)
                {
                    HN[HN.Count - 1][i].Error += ON[j].w[i] * ON[j].Error;
                }
                if (haf == ActivationFunction.ReLU)
                {
                    //ReLU
                    HN[HN.Count - 1][i].Error = HN[HN.Count - 1][i].Error * DerivatReLU(HN[HN.Count - 1][i].y);
                }
                else if (haf == ActivationFunction.Sigmoid)
                {
                    // Segmoid
                    HN[HN.Count - 1][i].Error = HN[HN.Count - 1][i].Error * HN[HN.Count - 1][i].y * (1.0f - HN[HN.Count - 1][i].y);
                }
                else if (haf == ActivationFunction.TanH)
                {
                    // TanH
                     HN[HN.Count - 1][i].Error = HN[HN.Count - 1][i].Error * DerivatTanh(HN[HN.Count - 1][i].y);
                }
            }
            for (int k = HN.Count - 2; k >= 0; k--)
            {
                for (int i = 0; i < HN[k].Length; i++)
                {
                    HN[k][i].Error = 0.0f;
                    for (int j = 0; j < HN[k + 1].Length; j++)
                    {
                        HN[k][i].Error += HN[k + 1][j].w[i] * HN[k + 1][j].Error;
                    }
                    if (haf == ActivationFunction.ReLU)
                    {
                        //ReLU
                        HN[k][i].Error = HN[k][i].Error * DerivatReLU(HN[k][i].y);
                    }
                    else if (haf == ActivationFunction.Sigmoid)
                    {
                        // Segmoid
                        HN[k][i].Error = HN[k][i].Error * HN[k][i].y * (1.0f - HN[k][i].y);
                    }
                     else if (haf == ActivationFunction.TanH)
                    {
                        // TanH
                         HN[k][i].Error = HN[k][i].Error * DerivatTanh(HN[k][i].y);
                    }
                }
            }
            for (int j = 0; j < ON.Length; j++)
            {
                for (int i = 0; i < ON[j].w.Length; i++)
                {
                    ON[j].w[i] = ON[j].w[i] + learnRate * HN[HN.Count - 1][i].y * ON[j].Error;
                }
                ON[j].bias += learnRate * ON[j].Error;
            }
            for (int k = 1; k < HN.Count; k++)
            {
                for (int i = 0; i < HN[k].Length; i++)
                {
                    for (int j = 0; j < HN[k][i].w.Length; j++)
                    {
                        HN[k][i].w[j] = HN[k][i].w[j] + learnRate * HN[k - 1][j].y * HN[k][i].Error;
                    }
                    HN[k][i].bias += learnRate * HN[k][i].Error;
                }
            }
            for (int i = 0; i < HN[0].Length; i++)
            {
                for (int j = 0; j < HN[0][i].w.Length; j++)
                {
                    HN[0][i].w[j] = HN[0][i].w[j] + learnRate * InputsX[j] * HN[0][i].Error;
                }
                HN[0][i].bias += learnRate * HN[0][i].Error;
            }

            //********************** update other hidden 
        }
        public void Process(double[] inputsx)
        {
            InputsX = inputsx;
            double aux;
            //*******************************************
            for (int i = 0; i < HN[0].Length; i++)
            {
                aux = 0.0d;
                for (int j = 0; j < HN[0][i].w.Length; j++)
                {
                    aux += InputsX[j] * HN[0][i].w[j];
                }
                aux += HN[0][i].bias;
                HN[0][i].net = aux;
                if (haf == ActivationFunction.ReLU)
                {
                    //ReLU
                    HN[0][i].y = ReLU(aux);
                }
                else if (haf == ActivationFunction.Sigmoid)
                {
                    // Segmoid
                    HN[0][i].y = sigmoid(aux);
                }
                else if (haf == ActivationFunction.TanH)
                {
                    // TanH
                    HN[0][i].y = Tanh(aux);
                }
            }
            //*******************************************
            for (int k = 1; k < HN.Count; k++)
            {
                for (int i = 0; i < HN[k].Length; i++)
                {
                    aux = 0.0d;
                    for (int j = 0; j < HN[k][i].w.Length; j++)
                    {
                        aux += HN[k - 1][j].y * HN[k][i].w[j];
                    }
                    aux += HN[k][i].bias;
                    HN[k][i].net = aux;
                    if (haf == ActivationFunction.ReLU)
                    {
                        //ReLU
                        HN[k][i].y = ReLU(aux);
                    }
                    else if (haf == ActivationFunction.Sigmoid)
                    {
                        // Segmoid
                        HN[k][i].y = sigmoid(aux);
                    }
                    else if (haf == ActivationFunction.TanH)
                    {
                        // TanH
                        HN[k][i].y = Tanh(aux);
                    }
                    
                }
            }

            //*******************************************
            double[] yy = new double[ON.Length];
            for (int j = 0; j < ON.Length; j++)
            {
                aux = 0.0f;
                for (int i = 0; i < ON[j].w.Length; i++)
                {
                    aux += HN[HN.Count - 1][i].y * ON[j].w[i];
                }
                aux += ON[j].bias;
                ON[j].net = aux;
                if (oaf == ActivationFunction.ReLU)
                {
                    //ReLU
                    ON[j].y = ReLU(aux);
                }
                else if (oaf == ActivationFunction.ReLU)
                {
                    // Segmoid
                    ON[j].y = sigmoid(aux);
                }
                else if (haf == ActivationFunction.TanH)
                {
                    // TanH
                    ON[j].y = Tanh(aux);
                }
                yy[j] = ON[j].y;
            }
            //*******************************************
            //return yy;
        }
        public double sigmoid(double xx)
        {
            return (1.0d / (1.0d + (double)Math.Exp(-xx)));
        }
        public double Derivatsigmoid(double xx)
        {
            return (sigmoid(xx) * (1.0d - sigmoid(xx)));
        }
        double aux = 0.0d;
        public double ReLU(double xx)
        {
            //if (xx > 0.0) return xx;
            //else return (aux * xx);
            return xx;
        }
        public double DerivatReLU(double xx)
        {
            //if (xx > 0.0) return 1.0d;
            //else return aux;
            return 1.0d;
        }
        public double Tanh(double x)
        {
            return ((Math.Exp(x) - Math.Exp(-x)) / (Math.Exp(x) + Math.Exp(-x)));
        }
        public double DerivatTanh(double x)
        {
            return (1.0d - (Tanh(x) * Tanh(x)));
        }
        public double[] SoftMax(double[] arr)
        {
            double sum = 0.0d;
            double[] aux = new double[arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                aux[i] = Math.Pow(Math.E, arr[i]);
                sum += aux[i];
            }
            for (int i = 0; i < arr.Length; i++)
            {
                aux[i] = aux[i] / sum;
            }
            return aux;
        }
        public double DerivatSoftMax(int index, double[] arr)
        {
            double sum = 0.0d;
            double[] aux = new double[arr.Length];
            double po = 0.0d;
            for (int i = 0; i < arr.Length; i++)
            {
                sum += Math.Pow(Math.E, arr[i]);
            }
            sum = sum * sum;
            for (int i = 0; (i < arr.Length) && (i != index); i++)
            {
                po += Math.Pow(Math.E, arr[i]);
            }
            po = (arr[index] * po) / sum;
            return po;
        }
        public double[] LogLoss(double[] y, double[] o)
        {
            double[] ll = new double[y.Length];
            for (int i = 0; i < y.Length; i++)
            {
                ll[i] = -1.0d * y[i] * Math.Log10(o[i]) - (1.0d - y[i]) * Math.Log10(1.0d - o[i]);
            }
            return ll;
        }
        public double[] DerivatLogLoss(double[] y, double[] o)
        {
            double[] ll = new double[y.Length];
            for (int i = 0; i < y.Length; i++)
            {
                ll[i] = -1.0d * y[i] * (1.0d / o[i]) - (1.0d - y[i]) * (1.0d / (1.0d - o[i]));
            }
            return ll;
        }
        public class Neural
        {
            public double[] w;
            public double bias;
            public double Error;
            public double net;
            public double y;
            static Random rand = new Random();

            public Neural(int inputs)
            {
                w = new double[inputs];
                for (int i = 0; i < inputs; i++)
                {
                    w[i] = ((double)rand.Next(-3000, 3000) / 5000.0f);
                }
                bias = ((double)rand.Next(-2000, 2000) / 5000.0f);
            }
        }
    }
}
