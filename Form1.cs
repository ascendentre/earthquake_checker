using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Handler handler = new Handler();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            handler.SeqHandling();
            sw.Stop();
            chart1.Series[0].Points.AddXY(0, sw.ElapsedMilliseconds);
            for(int i = 1; i <= 8; i++)
            {
                handler = new Handler();
                sw.Restart();
                handler.ParHandling(i);
                sw.Stop();
                chart1.Series[0].Points.AddXY(i, sw.ElapsedMilliseconds);
            }
        }
    }

    public class Handler
    {
        public float[,] a = null;
        public float[,] b = null;
        public int n = 1000;
        public Random r = new Random();
        public Handler()
        {
            a = new float[n, n];
            b = new float[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    a[i, j] = r.Next(-5, 5);
                    b[i, j] = r.Next(-5, 5);
                }
            }
        }

        public void SeqHandling()
        {
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    a[i, j] = f(a[i, j] + b[i, j]);
        }

        public void ParHandling(int m)
        {
            int elemPerThread = n / m;
            var opt = new ParallelOptions();
            opt.MaxDegreeOfParallelism = m;
            Parallel.For(
                0, m, opt, l =>
                {
                    int beg = elemPerThread * l;
                    int end = elemPerThread * (l + 1) - 1 + (l == m - 1 ? a.Length % m : 0);
                    for (int i = beg; i <= end; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            a[i, j] = f(a[i, j] + b[i, j]);
                        }
                    }
                });
        }

        public float f(float value)
        {
            return value * -1;
        }
    }
}
