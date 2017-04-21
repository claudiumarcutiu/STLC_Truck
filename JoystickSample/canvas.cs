using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
namespace JoystickSample
{
    public partial class canvas : Form
    {
        private frmMain form = null;
        List<System.Drawing.Point> coordonates = new List<System.Drawing.Point>(); //Lista pentru coordonatele clickurilor
        int clickCount = 0; //numara de cate ori se da click pe pictureBox1
        Graphics G;
        Pen p = new Pen(Color.Black, 2);
        int lengthsIndex = 0;
        int anglesIndex = 0;
        List<int> lengths = new List<int>();
        List<double> angles = new List<double>();
        byte[] byteArray = new byte[4];

        Stopwatch stopwatch = new Stopwatch();
        double calibrationTime;
        double tempEditedTime;
        bool stopwatchFlag = false;

        public canvas(Form callingForm)
        {
            InitializeComponent();
            pictureBox1.BorderStyle = BorderStyle.Fixed3D;
            form = callingForm as frmMain; //referinta la frmMain
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height); //Setam un bitmap ca sursa la pictureBox1
            G = Graphics.FromImage(pictureBox1.Image); //se seteaza G = ... pentru a putea desena linii pe pictureBox1
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MouseEventArgs em = (MouseEventArgs)e;
            coordonates.Add((System.Drawing.Point)em.Location); //coordonatele de click in pictureBox1
            clickCount++; //creste contorul
            //Debug.Write("\n"+thisPosition.X + "    " + thisPosition.Y+ "\n");
            if (clickCount > 1) //daca avem cel putin 3 coordonate in lista
            {
                G.DrawLine(p, coordonates[clickCount - 2], coordonates[clickCount - 1]); //desenz linia dintre ultimele 2 clickuri

                pictureBox1.Invalidate();
                if (clickCount > 2)
                {
                    double sideOne, sideTwo, sideThree;
                    System.Drawing.Point first, second, third;
                    first = coordonates[clickCount - 3];
                    second = coordonates[clickCount - 2];
                    third = em.Location;
                    sideOne = Math.Sqrt(getSquare(second.X - first.X) + getSquare(second.Y - first.Y));
                    sideTwo = Math.Sqrt(getSquare(third.X - second.X) + getSquare(third.Y - second.Y));
                    sideThree = Math.Sqrt(getSquare(third.X - first.X) + getSquare(third.Y - first.Y));

                    if (clickCount == 3)
                    {
                        lengths.Add((int)sideOne);
                        lengths.Add((int)sideTwo);
                    }
                    else
                    {
                        lengths.Add((int)sideTwo);
                    }
                    double r = (second.X - first.X) * (second.Y - first.Y);
                    double r1 = (second.X - first.X) * (third.Y - first.Y);
                    double r2 = (second.X - third.X) * (second.Y - first.Y);

                    double leftOrRight = r - r1 - r2;

                    if (sideTwo < 1)
                    {
                        sideTwo = 1;
                    }
                    if (sideThree < 1)
                    {
                        sideThree = 1;
                    }
                    if (sideOne < 1)
                    {
                        sideOne = 1;
                    }

                    double finalAngle = Math.Acos((sideTwo*sideTwo + sideOne*sideOne - sideThree * sideThree) / (2 * sideTwo * sideOne)) * 180/Math.PI;

                    

                    int l1, m1, l2, m2;
                    l1 = second.X - (-1 * first.X);
                    m1 = second.Y - first.Y;
                    l2 = third.X - second.X;
                    m2 = third.Y - second.Y;

                    double finalAngle2 = Math.Acos((l1 * l2 + m1 * m2) / (Math.Sqrt(l1 * l1 + m1 * m1) * Math.Sqrt(l2 * l2 + m2 * m2)));
                    if (leftOrRight < 0)
                    {
                        finalAngle = -finalAngle;
                    }
                    angles.Add(finalAngle);
                    Debug.Write(finalAngle.ToString() + "   " + leftOrRight + "\n");

                    //}
                }
            }
        }

        private double getSquare(double value)
        {
            return Math.Pow(value, 2.0);
        }

        private void bTrimite_Click(object sender, EventArgs e)
        {
            ThreadStart ts = new ThreadStart(processValues);
            Thread t = new Thread(ts);
            calibrationTime = float.Parse(tbFrecare.Text);
            t.Start();
        }

        public void updateTextbox(String text)
        {
            MethodInvoker action = delegate
            { tbProgress.Text = text; };
            tbProgress.BeginInvoke(action);
            //tbProgress.Text = text;
        }

        void processValues()
        {
            float percentProgress = 0;
            double progressValue = 0;
            updateTextbox(progressValue + "");
            //tbProgress.Text = percentProgress + " %";
            //int limit = angles.Count + lengths.Count;
            List<double> allItems = new List<double>();
            for (int i = 0; i < angles.Count; i++)
            {
                allItems.Add(lengths[i]);
                allItems.Add(angles[i]);
                if (i == angles.Count - 1)
                {
                    allItems.Add(lengths[i + 1]);
                }
            }
            Boolean isAngle = false;
            for (int i = 0; i < allItems.Count; i++)
            {
                double itemValue = 100 / allItems.Count;
                double x = allItems[i];
                if (i % 2 == 0)
                {
                    //Inseamna ca e latura
                    //
                    byteArray[0] = 2;
                    byteArray[1] = 2 + 60;
                    byteArray[2] = 2 + 120;
                    byteArray[3] = 2 + 180;
                }
                else
                {
                    //Inseamna ca e unghi
                    if (x < 0)
                    {
                        x = -x;
                        byteArray[0] = (byte)2;
                        byteArray[1] = (byte)2 + 60;
                        byteArray[2] = (byte)58 + 120;
                        byteArray[3] = (byte)58 + 180;
                    }
                    else
                    {
                        byteArray[0] = (byte)58;
                        byteArray[1] = (byte)58 + 60;
                        byteArray[2] = (byte)2 + 120;
                        byteArray[3] = (byte)2 + 180;
                    }
                    isAngle = true;


                }
                if (isAngle)
                {
                    //must be an angle
                    //int time = (int)(8 * x);
                    if (x < 0)
                        x = -x;
                    x = 180 - x;
                    int time = (int)(calibrationTime * x / 360);
                    form.form2Send(byteArray);
                    try
                    {
                        Thread.Sleep(time);
                    }
                    catch (Exception e)
                    {
                        Debug.Write(e.Message);
                    }
                }
                else
                {
                    //must be a line
                    form.form2Send(byteArray);
                    int time = (int)(x * 8);
                    Thread.Sleep(time);
                    if (i == allItems.Count - 1)
                    {
                        updateTextbox("100");
                        byteArray[0] = (byte)30;
                        byteArray[1] = (byte)30;
                        byteArray[2] = (byte)30;
                        byteArray[3] = (byte)30;
                        form.form2Send(byteArray);

                    }
                }
                progressValue += itemValue;
                if (i != allItems.Count - 1)
                    updateTextbox("" + progressValue);
                isAngle = false;
            }
        }

        private void buttonStratCalibration_Click(object sender, EventArgs e)
        {
            stopwatch.Start();
            byteArray[0] = (byte)58;
            byteArray[1] = (byte)58 + 60;
            byteArray[2] = (byte)2 + 120;
            byteArray[3] = (byte)2 + 180;
            form.form2Send(byteArray);
            calibrationTime = 0;
            stopwatchFlag = true;
        }

        private void buttonStopCalibration_Click(object sender, EventArgs e)
        {
            if (stopwatchFlag)
            {
                stopwatch.Stop();
                byteArray[0] = (byte)30;
                byteArray[1] = (byte)30;
                byteArray[2] = (byte)30;
                byteArray[3] = (byte)30;
                form.form2Send(byteArray);
                calibrationTime = stopwatch.Elapsed.TotalMilliseconds / 5;
                tbFrecare.Text = "" + calibrationTime;
                tempEditedTime = calibrationTime;
                stopwatchFlag = false;
            }
        }

        private void tbFrecare_MouseHover(object sender, EventArgs e)
        {
            //tbFrecare.Text = "" + calibrationTime;
        }

        private void tbFrecare_TextChanged(object sender, EventArgs e)
        {
            // if (tbFrecare.Text != "" && tbFrecare.Text != ""+calibrationTime)
            // tempEditedTime = float.Parse(tbFrecare.Text);
        }

        private void canvas_MouseLeave(object sender, EventArgs e)
        {

        }

        private void canvas_MouseHover(object sender, EventArgs e)
        {

        }

        private void tbFrecare_MouseLeave(object sender, EventArgs e)
        {
            // tbFrecare.Text = "" + tempEditedTime;
        }

        private void bResetCanvas_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height); //Setam un bitmap ca sursa la pictureBox1
            G = Graphics.FromImage(pictureBox1.Image); //se seteaza G = ... pentru a putea desena linii pe pictureBox1
            lengths.Clear();
            angles.Clear();
            clickCount = 0;
            coordonates.Clear();
        }

        private void canvas_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

    }
}
