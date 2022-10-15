using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AI_Game1
{
    public class Balls
    {
        public delegate void GetParam(object o, Pos p, Speed s);
        public event GetParam GetPos_Event;
        public struct Speed
        {
            public float XSpeed;
            public float YSpeed;
            public void invert()
            {
                this.XSpeed = -1.0f * this.YSpeed;
                this.YSpeed = -1.0f * this.XSpeed;
            }
        }
        public struct Pos
        {
            public float XPos;
            public float YPos;
        }
        string name;
        Speed InSpeed;
        Pos InPos; int ms;
        Pos ThisPos;
        Speed ThisSpeed;
        System.Windows.Forms.Panel P;
        System.Threading.Thread Render;
        System.Drawing.Graphics g;
        System.Drawing.Pen D, E, B;
        Pos[] lastPos = new Pos[4]; int index = 0;
        public void Init(System.Windows.Forms.Panel p, Speed speed, Pos pos, System.Drawing.Color C, string n)
        {
            P = p; InSpeed = speed; InPos = pos;
            g = P.CreateGraphics();
            ThisPos = InPos;
            ThisSpeed = InSpeed;
            name = n;
            D = new Pen(Color.Red, 0.8f);
            E = new Pen(Color.White, 0.8f);
            B = new Pen(Color.Blue, 1.5f);
            g.Clear(System.Drawing.Color.White);
            Render = new System.Threading.Thread(
                new System.Threading.ThreadStart(BallRender));
        }
        public System.Drawing.Graphics GetGraphics
        {
            get { return g; }
        }
        public void Start(int ms)
        {
            this.ms = ms;
            Render.Start();
        }
        public string GetName()
        {
            return this.name;
        }
        public void Stop()
        {
            try
            {
                Render.Abort();
            }
            catch (Exception ex)
            {
            }
        }
        public void SetPos(Pos p)
        {
            ThisPos = p;
        }
        public void SetSpeed(Speed s)
        {
            ThisSpeed = s;
        }
        private void BallRender()
        {
            while (true)
            {
                if (index == lastPos.Length - 1)
                    index = 0;
                lastPos[index] = this.ThisPos;
                Reflaction();
                Draw();
                Render.Join(this.ms); index++;
            }
        }
        private void Draw()
        {
            for (int i = 0; i < index; i++)
                g.DrawEllipse(E, lastPos[i].XPos, lastPos[i].YPos, 6.0f, 6.0f);
            for (int i = index; i < lastPos.Length; i++)
                g.DrawEllipse(E, lastPos[i].XPos, lastPos[i].YPos, 6.0f, 6.0f);
            ThisPos.XPos += ThisSpeed.XSpeed;
            ThisPos.YPos += ThisSpeed.YSpeed;
            GetPos_Event((object)this, ThisPos, ThisSpeed);
            g.DrawEllipse(D, ThisPos.XPos, ThisPos.YPos, 6.0f, 6.0f);
        }
        private void Draw(PointF mp)
        {
            g.DrawEllipse(D, mp.X, mp.Y, 6.0f, 6.0f);
        }
        private void Reflaction()
        {
            if (ThisPos.XPos <= 0.0f)
            {
                ThisPos.XPos = 1.0f;
                ThisSpeed.XSpeed = -1.0f * ThisSpeed.XSpeed;
            }
            if (ThisPos.XPos >= (float)P.Width - 10.0f)
            {
                ThisPos.XPos = (float)P.Width - 11.0f;
                ThisSpeed.XSpeed = -1.0f * ThisSpeed.XSpeed;
            }
            if (ThisPos.YPos <= 0.0f)
            {
                ThisPos.YPos = 1.0f;
                ThisSpeed.YSpeed = -1.0f * ThisSpeed.YSpeed;
            }
            if (ThisPos.YPos >= (float)P.Height - 10.0f)
            {
                ThisPos.YPos = (float)P.Height - 11.0f;
                ThisSpeed.YSpeed = -1.0f * ThisSpeed.YSpeed;
            }

        }
    }
}
