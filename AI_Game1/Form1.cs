using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace AI_Game1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        ANNXML xml;
        ANN ann;
        int input;
        int h = 3;
        int[] hn; double[] inputs;
        double[] target = new double[2]; 
        AI_Data data = new AI_Data();
        struct AI_Data
        {
            public Point from;
            public Point to;
            public PointF LenZeta;
            public PointF Acc;
            
            public void CalcAcc()
            {
                float len = (float)Math.Pow((from.X - to.X) * (from.X - to.X) + (from.Y - to.Y) * (from.Y - to.Y), 0.5d);
                float aux;
                LenZeta = new PointF();
                Acc = new PointF();
                if (len == 0.0f)
                {
                    LenZeta.X = 0.0f;
                    LenZeta.Y = 0.0f;
                    Acc.X = 0.0f;
                    Acc.Y = 0.0f;
                }
                else
                {
                    aux = (to.X - from.X) / len;
                    
                    LenZeta.X = (float)Math.Acos(aux);
                    LenZeta.Y = 0.0f;

                    Acc.X = ((to.X - from.X) / len);
                    Acc.Y = ((to.Y - from.Y) / len);
                }                    
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            xml = new ANNXML(); ball01 = new Balls();
            panel1.MouseClick += new MouseEventHandler(mouseClick);
        }
        Balls ball01;
        private void button6_Click(object sender, EventArgs e)
        {
            this.Closing += new CancelEventHandler(Form1_Closing);
            Balls.Pos pos = new Balls.Pos();
            Balls.Speed speed = new Balls.Speed();

            pos.XPos = (float)panel1.Width / 2.0f;
            pos.YPos = (float)panel1.Height / 2.0f;
            mp.X = panel1.Width / 2; mp.Y = panel1.Height / 2;
            speed.XSpeed = 0.0f; speed.YSpeed = 0.0f;
            ball01.Init(panel1, speed, pos, Color.Red, "01");
            ball01.GetPos_Event += new Balls.GetParam(ball_GetPos_Event);
            ball01.Start(200);
        }
        System.Drawing.Pen E = new Pen(Color.White, 1.5f);
        System.Drawing.Pen B = new Pen(Color.Blue, 1.5f);
        Point oldmp;
        private void mouseClick(object o, MouseEventArgs e)
        {
            if (oldmp != null)
                ball01.GetGraphics.DrawEllipse(E, oldmp.X, oldmp.Y, 6.0f, 6.0f);
            mp = e.Location;
            ball01.GetGraphics.DrawEllipse(B, mp.X, mp.Y, 6.0f, 6.0f);
            oldmp = mp;
        }
        Point mp;
        public void ball_GetPos_Event(object o, Balls.Pos pp, Balls.Speed ss)
        {
            Balls b = (Balls)o;
            if (b.GetName() == "01")
            {
                double xl = (double)(mp.X - pp.XPos);
                double yl = (double)(mp.Y - pp.YPos);
                double len = Math.Pow(xl * xl + yl * yl, 0.5d);
                double zeta = 0.0d;
                double[] inputs = new double[1];
                if (len != 0.0d)
                {
                    zeta = Math.Atan2(yl , xl);
                    //if (xl < 0.0d)
                    //    zeta = zeta + 3.14159265d;
                    inputs[0] = zeta;
                    ann.Process(inputs);
                    ss.XSpeed = (float)(len * ann.ON[0].y * 0.06f);
                    ss.YSpeed = (float)(len * ann.ON[1].y * 0.06f);
                    ball01.SetSpeed(ss);
                }
                else
                {
                    ss.XSpeed = (float)(0.0f);
                    ss.YSpeed = (float)(0.0f);
                    ball01.SetSpeed(ss);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            input = 1;
            h = 2;
            hn = new int[h];
            inputs = new double[input];
            hn[0] = 8;
            hn[1] = 8;
            //hn[2] = 8;
            //hn[3] = 8;
            ann = new ANN(input, hn, ANN.ActivationFunction.TanH, 2, ANN.ActivationFunction.TanH);
        }
        Random rand;
        private void button2_Click(object sender, EventArgs e)
        {
            rand = new Random();
            inputs = new double[input];
            for (int s = 0; s < 800000; s++)
            {
                double zeta = (rand.NextDouble() - 0.5d) * 6.283185307179d;
                inputs[0] = zeta;
                target[0] = (double)Math.Cos(zeta);
                target[1] = (double)Math.Sin(zeta);
                ann.learnrate = 0.003d;
                for (int k = 0; k < 6; k++)
                {
                    ann.Process(inputs);
                    ann.Update(target);
                }
            }                
        }
        private void button7_Click(object sender, EventArgs e)
        {
            rand = new Random();
            inputs = new double[input];
            for (int s = 0; s < 1000; s++)
            {
                double zeta = (rand.NextDouble() - 0.5d) * 6.283185307179d;
                inputs[0] = zeta;
                target[0] = (double)Math.Cos(zeta);
                target[1] = (double)Math.Sin(zeta);
                ann.learnrate = 0.0005d;
                for (int k = 0; k < 4; k++)
                {
                    ann.Process(inputs);
                    ann.Update(target);
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string ss = xml.BuildANN(ann);
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "ANN File|*.xml";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName);
                sw.Write(ss);
                sw.Flush();
                sw.Close();
            }
        }
        int num = 0; int hnum = 0;
        private void button4_Click(object sender, EventArgs e)
        {
            //input = 4;
            //h = 3;
            //hn = new int[h];
            //inputs = new double[input];
            //hn[0] = 8;
            //hn[1] = 8;
            //hn[2] = 8;
            //ann = new ANN(input, hn, ANN.ActivationFunction.Sigmoid, 2, ANN.ActivationFunction.Sigmoid);

            treeView1.Nodes.Clear();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "ANN File|*.xml";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TreeNode root = new TreeNode("ANN");
                ANN.ActivationFunction haf = ANN.ActivationFunction.ReLU;
                ANN.ActivationFunction oaf = ANN.ActivationFunction.ReLU;
                int oo = 0;
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(ofd.FileName);
                foreach (XmlNode node in xdoc.DocumentElement.ChildNodes)
                {
                    if (node.Name == "input")
                    {
                        input = int.Parse(node.InnerText);
                        inputs = new double[input];
                    }
                    if (node.Name == "hidden")
                    {
                        h = int.Parse(node.InnerText);
                        hn = new int[h];
                        hnum = int.Parse(node.InnerText);
                    }
                    if (node.Name == "output")
                    {
                    }
                    if (node.Name == "HActivation")
                    {
                        if (node.InnerText == "ReLU")
                        {
                            haf = ANN.ActivationFunction.ReLU;
                        }
                        else if (node.InnerText == "Sigmoid")
                        {
                            haf = ANN.ActivationFunction.Sigmoid;
                        }
                        else if (node.InnerText == "TanH")
                        {
                            haf = ANN.ActivationFunction.TanH;
                        }
                    }
                    if (node.Name == "OutActivation")
                    {
                        if (node.InnerText == "ReLU")
                        {
                            oaf = ANN.ActivationFunction.ReLU;
                        }
                        else if (node.InnerText == "Sigmoid")
                        {
                            oaf = ANN.ActivationFunction.Sigmoid;
                        }
                        else if (node.InnerText == "TanH")
                        {
                            oaf = ANN.ActivationFunction.TanH;
                        }
                    }
                    if (node.Name == "OLayer")
                    {
                        XmlNode n = node["NN"];
                        oo = int.Parse(n.InnerText);
                    }
                    for (int j = 0; j < hnum; j++)
                    {
                        if (node.Name == "HLayer" + j)
                        {
                            XmlNode n = node["NN"];
                            TreeNode aux1 = new TreeNode(node.Name + ":" + n.InnerText);
                            hn[j] = int.Parse(n.InnerText);
                        }
                    }
                }
                ann = new ANN(input, hn, haf, oo, oaf);

                //*********************************

                //xdoc.Load(@"d:\AddANN2.xml");
                foreach (XmlNode node in xdoc.DocumentElement.ChildNodes)
                {
                    if (node.Name == "input")
                    {
                        TreeNode aux = new TreeNode(node.Name + ":" + node.InnerText); root.Nodes.Add(aux);
                    }
                    if (node.Name == "hidden")
                    {
                        TreeNode aux = new TreeNode(node.Name + ":" + node.InnerText); root.Nodes.Add(aux);
                    }
                    if (node.Name == "output")
                    {
                        TreeNode aux = new TreeNode(node.Name + ":" + node.InnerText); root.Nodes.Add(aux);
                    }
                    if (node.Name == "HActivation")
                    {
                        TreeNode aux = new TreeNode(node.Name + ":" + node.InnerText); root.Nodes.Add(aux);
                    }
                    if (node.Name == "OutActivation")
                    {
                        TreeNode aux = new TreeNode(node.Name + ":" + node.InnerText); root.Nodes.Add(aux);
                    }
                    if (node.Name == "OLayer")
                    {
                        XmlNode n = node["NN"];
                        TreeNode aux1 = new TreeNode(node.Name + ":" + n.InnerText);
                        num = int.Parse(n.InnerText);
                        for (int i = 0; i < num; i++)
                        {
                            n = node["ON" + i];
                            TreeNode aux2 = new TreeNode(n.Name + ": bias = " + n.Attributes[0].InnerText);
                            ann.ON[i].bias = double.Parse(n.Attributes[0].InnerText);
                            int index = 0;
                            foreach (XmlNode sn in n)
                            {
                                TreeNode aux3 = new TreeNode(sn.Name + ":" + sn.InnerText);
                                ann.ON[i].w[index++] = double.Parse(sn.InnerText);
                                aux2.Nodes.Add(aux3);
                            }
                            aux1.Nodes.Add(aux2);
                        }
                        root.Nodes.Add(aux1);
                    }
                    for (int j = 0; j < hnum; j++)
                    {
                        if (node.Name == "HLayer" + j)
                        {
                            XmlNode n = node["NN"];
                            TreeNode aux1 = new TreeNode(node.Name + ":" + n.InnerText);
                            num = int.Parse(n.InnerText);
                            for (int i = 0; i < num; i++)
                            {
                                n = node["HN" + j + "" + i];
                                TreeNode aux2 = new TreeNode(n.Name + ": bias = " + n.Attributes[0].InnerText);
                                ann.HN[j][i].bias = double.Parse(n.Attributes[0].InnerText);
                                int index = 0;
                                foreach (XmlNode sn in n)
                                {
                                    TreeNode aux3 = new TreeNode(sn.Name + ":" + sn.InnerText); aux2.Nodes.Add(aux3);
                                    ann.HN[j][i].w[index++] = double.Parse(sn.InnerText);
                                }
                                aux1.Nodes.Add(aux2);
                            }
                            root.Nodes.Add(aux1);
                        }
                    }
                }
                //ann = new ANN(input, hn, haf, oo, oaf);
                treeView1.Nodes.Add(root);
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {

            //data.from.X = (int)numericUpDown1.Value;
            //data.from.Y = (int)numericUpDown2.Value;
            //data.to.X = (int)numericUpDown3.Value;
            //data.to.Y = (int)numericUpDown4.Value;
            //data.CalcAcc();
            double zeta = (double)numericUpDown5.Value;
            textBox1.Text = Math.Cos(zeta).ToString("#.#####") + "  :  " + Math.Sin(zeta).ToString("#.#####");
            inputs[0] = zeta;
            //inputs[1] = (double)data.LenZeta.Y;
            ann.Process(inputs);
            textBox2.Text = ann.ON[0].y.ToString("#.#####") + "  :  " + ann.ON[1].y.ToString("#.#####"); 
        }

        public void Form1_Closing(object o, EventArgs e)
        {
            ball01.Stop();
        }
    }
}
