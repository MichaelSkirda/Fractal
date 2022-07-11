using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fractal
{
    public partial class Form1 : Form
    {

        private double _posX = 0;
        private double _posY = 0;
        private double _zoom = 1;

        private int xZoomLength = 3;
        private int yZoomLength = 2;

        private int xZoomOffset = 2;
        private int yZoomOffset = 1;

        private int[] pointCountInColumns = new int[] { 0 };


        private WaveOut waveOut;

        public Form1()
        {
            InitializeComponent();
            Bitmap bitmap = GetFractal();
            pictureBox1.Image = bitmap;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F13)
            {
                this.Text = "Как ты нашел клавишу F13?";
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = GetFractal();
            pictureBox1.Image = bitmap;
        }

        private Bitmap GetFractal()
        {
            int width = pictureBox1.Width;
            int height = pictureBox1.Height;

            _posX = (double)numericUpDown_X.Value;
            _posY = (double)numericUpDown_Y.Value;
            _zoom = (double)numericUpDown_Zoom.Value;

            Bitmap fractal = GenerateFractal(width, height, posX: (double)numericUpDown_X.Value, posY: (double)numericUpDown_Y.Value, zoom: (double)numericUpDown_Zoom.Value);
            return fractal;
        }

        // Zoom показывает сколько клеток мы сможем увидеть камерой
        // В высоту мы видим zoom * 2
        // В ширину мы видим zoom * 3
        private Bitmap GenerateFractal(int width, int height, double posX, double posY, double zoom)
        {
            Bitmap bitmap = new Bitmap(width, height);

            double xPixelSize = (zoom * xZoomLength) / width;
            double yPixelSize = (zoom * yZoomLength) / height;

            pointCountInColumns = new int[width];

            // Проходимся по всей координатной плоскости с левого верхнего угла
            for (int x = 1; x <= width; x++)
            {
                for (int y = 1; y < height + 1; y++)
                {
                    // Находим координаты точки
                    Point point = new Point(posX - (zoom * xZoomOffset) + (xPixelSize * x), posY - (zoom * yZoomOffset) + (yPixelSize * y));
                    Color pixel;


                    if (IsInMandelbrotSet(point))
                    {
                        pointCountInColumns[x - 1]++;
                        pixel = Color.FromArgb(0, 0, 0);
                    }
                    else
                    {
                        pixel = Color.FromArgb(255, 255, 255);
                    }

                    bitmap.SetPixel(x - 1, y - 1, pixel);
                }
            }

            foreach(int pointCountInColumn in pointCountInColumns)
            {
                Console.WriteLine($"Count of points in column is: {pointCountInColumn}");
            }

            return bitmap;
        }


        private bool IsInMandelbrotSet(Point point)
        {
            Point z = new Point(0, 0);

            for (int i = 0; i < numericUpDownIterations.Value; i++)
            {
                // z = z^2 + c

                z = new Point
                    (
                    x: (z.x * z.x - z.y * z.y) - z.x,
                    y: (double)numericUpDown1.Value * z.x * z.y 
                    );

                z.x += point.y;// * Math.Sin(point.x);
                z.y += point.x;// * Math.Cos(point.x);

                if (Math.Abs(z.x - z.y) > (double)numericUpDown2.Value)
                {
                    return false;
                }
            }

            return true;


        }

        private bool IsInMandelbrotSet2(Point point)
        {
            Point z = new Point(0, 0);

            for(int i = 0; i < numericUpDownIterations.Value; i++)
            {
                // z = z^2 + c
         
                z = new Point
                    (
                    x: z.x * z.x - z.y * z.y,
                    y: 2 * z.x * z.y    
                    );

                z.x += point.x;// * Math.Sin(point.x);
                z.y += point.y;// * Math.Sin(point.x);

                if(Math.Abs(z.x - z.y) > 2)
                {
                    return false;
                }
            }

            return true;


        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            

            Point clickPoint = new Point(me.Location.X, me.Location.Y);

            int clickX = me.Location.X;
            int clickY = me.Location.Y;

            int bitmapWidth = pictureBox1.Width;
            int bitmapHeight = pictureBox1.Height;

            double xPixelSize = (_zoom * xZoomLength) / bitmapWidth;
            double yPixelSize = (_zoom * yZoomLength) / bitmapHeight;

            // Point in mandelbrot set
            Point point = new Point(_posX - (_zoom * xZoomOffset) + (xPixelSize * clickX), _posY - (_zoom * yZoomOffset) + (yPixelSize * clickY));

            numericUpDown_X.Value = (decimal)point.x;
            numericUpDown_Y.Value = (decimal)point.y;
            if (me.Button == MouseButtons.Right)
            {
                numericUpDown_Zoom.Value = numericUpDown_Zoom.Value * 2;
            }
            else
            {
                numericUpDown_Zoom.Value = numericUpDown_Zoom.Value / 2;
            }

            Bitmap bitmap = GetFractal();
            pictureBox1.Image = bitmap;
        }

        private Point LocalPointPositionToAbsolute(Point point)
        {
            int bitmapWidth = pictureBox1.Width;
            int bitmapHeight = pictureBox1.Height;

            double xPixelSize = (_zoom * xZoomLength) / bitmapWidth;
            double yPixelSize = (_zoom * yZoomLength) / bitmapHeight;

            int absoluteX = (int)(point.x / xPixelSize);
            int absoluteY = (int)(point.y / yPixelSize);

            if (point.x < 0)
            {
                absoluteX = (int)(((_zoom * xZoomOffset) + point.x) / xPixelSize);
            }
            else
            {
                absoluteX = (int)((point.x + (_zoom * xZoomOffset)) / xPixelSize);
            }

            if (point.y < 0)
            {
                absoluteY = (int)(((_zoom * yZoomOffset) + point.y) / yPixelSize);
            }
            else
            {
                absoluteY = (int)((point.y + (_zoom * yZoomOffset)) / yPixelSize);
            }

            return new Point(absoluteX, absoluteY);
        }

        private Point AbsolutePointPositionToLocal(Point point)
        {
            int bitmapWidth = pictureBox1.Width;
            int bitmapHeight = pictureBox1.Height;

            double xPixelSize = (_zoom * xZoomLength) / bitmapWidth;
            double yPixelSize = (_zoom * yZoomLength) / bitmapHeight;

            double localX = -(_zoom * xZoomOffset) + (point.x * xPixelSize);
            double localY = -(_zoom * yZoomOffset) + (point.y * yPixelSize);

            return new Point(localX, localY);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StartStopSineWave();
        }

        private void StartStopSineWave()
        {
            float[] soundWave = new float[pointCountInColumns.Length];

            int maxValue = pointCountInColumns.Max();

            if(maxValue == 0)
            {
                maxValue = 1;
            }

            for(int i = 0; i < pointCountInColumns.Length; i++)
            {
                soundWave[i] = pointCountInColumns[i] / maxValue;
            }

            if (waveOut == null)
            {
                var sineWaveProvider = new SineWaveProvider32(soundWave);
                sineWaveProvider.SetWaveFormat(Convert.ToInt32(numericUpDown_Frequency.Value), 1); // 16kHz mono
                //sineWaveProvider.Frequency = (float)numericUpDown_Frequency.Value;
                //sineWaveProvider.Amplitude = (float)numericUpDown_Amplitude.Value;
                waveOut = new WaveOut();
                waveOut.Init(sineWaveProvider);
                waveOut.Play();
            }
            else
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }
        }

        private void savePresetButton_Click(object sender, EventArgs e)
        {
            
        }

        private void numericUpDown_X_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
