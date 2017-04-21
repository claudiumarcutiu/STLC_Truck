using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.IO.Ports;
using System.Threading;
using JoystickInterface;

namespace JoystickSample
{
    public partial class frmMain : Form
    {
        private JoystickInterface.Joystick jst;
        String ip, port, url, image_url;
        float firstAxis, secondAxis; //valorile citite de pe axe
        float k; //secondAxis
        int v; //firstAxis
        float speedLeftFront, speedLeftBack, speedRightFront, speedRightBack; //valoare carea care e trimisa pe serial
        //SerialPort _serial = new SerialPort("COM12", 9600);
        byte[] byteArr = new byte[4];
        char test = (char)65;
        int test2 = 65;
        bool joystickConnected = true;

        bool isMoving = false, portOpen = false, connectedToServer = false, streamIsOn = false;

        public frmMain()
        {
            InitializeComponent();
            disableDirectionalButtons();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                // grab the joystick
                jst = new JoystickInterface.Joystick(this.Handle);
                string[] sticks = jst.FindJoysticks();
                jst.AcquireJoystick(sticks[0]);
                // add the axis controls to the axis container
                for (int i = 0; i < jst.AxisCount; i++)
                {
                    Axis ax = new Axis();
                    ax.AxisId = i + 1;
                    flpAxes.Controls.Add(ax);
                }

                // add the button controls to the button container
                for (int i = 0; i < jst.Buttons.Length; i++)
                {
                    JoystickSample.Button btn = new Button();
                    btn.ButtonId = i + 1;
                    btn.ButtonStatus = jst.Buttons[i];
                    flpButtons.Controls.Add(btn);
                }

                // start updating positions
                tmrUpdateStick.Enabled = true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Niciun joystick nu este conectat!\n", "Warning!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                joystickConnected = false;
            }
        }

        private void tmrUpdateStick_Tick(object sender, EventArgs e)
        {
            // get status
            jst.UpdateStatus();
            /*Only for testing serial port
             * if (portOpen)
            {
                serialPort1.Write("1");
            }
             */

            // update the axes positions
            foreach (Control ax in flpAxes.Controls)
            {
                if (ax is Axis)
                {
                    switch (((Axis)ax).AxisId)
                    {
                        case 1:
                            ((Axis)ax).AxisPos = jst.AxisA;
                            break;
                        case 2:
                            ((Axis)ax).AxisPos = jst.AxisB;
                            break;
                        case 3:
                            ((Axis)ax).AxisPos = jst.AxisC;
                            break;
                        case 4:
                            ((Axis)ax).AxisPos = jst.AxisD;
                            break;
                        case 5:
                            ((Axis)ax).AxisPos = jst.AxisE;
                            break;
                        case 6:
                            ((Axis)ax).AxisPos = jst.AxisF;
                            break;
                    }
                }
            }

            pictureBox1.Left = jst.AxisC / 350 - 2;
            pictureBox1.Top = jst.AxisA / 440 - 5;
            if (pictureBox1.Top == -5)
            {
                pictureBox1.Top = 7;
            }

            // update each button status
            foreach (Control btn in flpButtons.Controls)
            {
                if (btn is JoystickSample.Button)
                {
                    ((JoystickSample.Button)btn).ButtonStatus =
                        jst.Buttons[((JoystickSample.Button)btn).ButtonId - 1];
                    if (((JoystickSample.Button)btn).ButtonStatus == true)
                    {
                        int buttonId = ((JoystickSample.Button)btn).ButtonId;
                        textBox1.Text = ((JoystickSample.Button)btn).ButtonId.ToString();
                        switch (buttonId)
                        {
                            case 8:   //bifare checkbox pentru butoanele de directie
                                CBdirButtons.Checked = !CBdirButtons.Checked;
                                break;
                            case 5:  //porneste streamingul de poze
                                button1.PerformClick();
                                break;
                            case 1:
                                button4.PerformClick();
                                break;
                            case 2:
                                button3.PerformClick();
                                break;
                            case 7:
                                button2.PerformClick();
                                break;
                        }

                    }
                    //textBox1.Text = ((JoystickSample.Button)btn).ButtonStatus.ToString();
                }
            }
        }

        private void flpAxes_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (streamIsOn)
                return;
            if (connectedToServer)
            {
                streamIsOn = true;
                image_url = url + "/shot.jpg";
                //image_url = "X-45.jpg";
                getStream();

                richTextBox1.AppendText(getTime() + " : Transmisiune pornita.\n");
            }
            else
            {
                error(1);
            }
        }

        public void error(int message)
        {
            switch (message)
            {
                case 1:
                    System.Windows.Forms.MessageBox.Show("Nu sunteti conectat la niciun server Android!", "Eroare!!!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    break;
                case 2:
                    System.Windows.Forms.MessageBox.Show("Deja sunteti conectat!\nDeconectati-va inainte si dupa conectati-va la alt server!", "Eroare!!!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    break;
                case 3:
                    System.Windows.Forms.MessageBox.Show("Nu s-a putut stabili o conexiune la adresa IP specificata!", "Eroare!!!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    break;
            }
        }

        public string getTime()
        {
            return DateTime.Now.ToString("h:mm:ss tt");
        }

        public void getStream()
        {
            imageTimer.Enabled = true;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            /*WebRequest req = WebRequest.Create("[URL here]");
            WebResponse response = req.GetResponse();
            Stream stream = response.GetResponseStream();
            pictureBox2.ImageLocation = image_url;*/
            try
            {
                //image_url = "X-45.jpg";
                var request = WebRequest.Create(image_url);

                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    int width = 600;
                    int height = 300;
                    Bitmap sourceBMP = (Bitmap)Bitmap.FromStream(stream);
                    Bitmap result = new Bitmap(width, height);
                    using (Graphics g = Graphics.FromImage(result))
                        g.DrawImage(sourceBMP, 0, 0, width, height);
                    pictureBox2.Image = result;
                }
            }
            catch (System.NotImplementedException ex) { Debug.Print(ex.Message); }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (streamIsOn)
            {
                streamIsOn = false;
                imageTimer.Enabled = false;
                pictureBox2.ImageLocation = null;
                richTextBox1.AppendText(getTime() + " : Trasmisiune oprita.\n");
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Transmisiunea este deja oprita!", "WARNING!!!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            /*WebRequest http = WebRequest.Create(url+"/enabletorch");
            using (HttpWebResponse response = (HttpWebResponse)http.GetResponse())
            {
                Console.WriteLine(response.LastModified);
            }*/
            if (connectedToServer)
            {
                System.Net.HttpWebRequest req = (HttpWebRequest)System.Net.HttpWebRequest.Create(url + "/enabletorch");
                System.Net.HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                res.Close();

                richTextBox1.AppendText(getTime() + " : Blit pornit.\n");
            }
            else
            {
                error(1);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (connectedToServer)
            {
                System.Net.HttpWebRequest req = (HttpWebRequest)System.Net.HttpWebRequest.Create(url + "/disabletorch");
                System.Net.HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                res.Close();

                richTextBox1.AppendText(getTime() + " : Blit oprit.\n");
            }
            else
            {
                error(1);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }
        private void CBdirButtons_CheckedChanged(object sender, EventArgs e)
        {
            if (CBdirButtons.Checked)
            {
                enableDirectionalButtons();
                richTextBox1.AppendText(getTime() + " : Butoane de directie active.\n");
            }
            else
            {
                disableDirectionalButtons();
                richTextBox1.AppendText(getTime() + " : Butoane de directie inactive.\n");
            }



        }

        public void disableDirectionalButtons()
        {
            Bstanga.Enabled = false;
            Bsus.Enabled = false;
            Bjos.Enabled = false;
            Bdreapta.Enabled = false;
        }
        public void enableDirectionalButtons()
        {
            Bstanga.Enabled = true;
            Bsus.Enabled = true;
            Bjos.Enabled = true;
            Bdreapta.Enabled = true;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.ScrollToCaret();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox3.Text == "" || textBox2.Text == "")
                    System.Windows.Forms.MessageBox.Show("Cel putin un camp de text este gol!", "Eroare!!!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                else
                {
                    ip = textBox2.Text;
                    port = textBox3.Text;
                    url = "http://" + ip + ":" + port;
                    System.Net.HttpWebRequest req = (HttpWebRequest)System.Net.HttpWebRequest.Create(url);
                    System.Net.HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                    res.Close();
                    connectedToServer = true;
                    richTextBox1.AppendText(getTime() + " : Conectat la server.\n");
                }
            }
            catch
            {
                error(3);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textPortNumber.Text == "")
            {
                System.Windows.Forms.MessageBox.Show("Niciun numar de port specificat", "Eroare!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            else if (!portOpen)
            {
                portOpen = true;
                button6.Text = "Inchide Port";

                try
                {
                    serialPort1.PortName = "COM" + textPortNumber.Text;
                    serialPort1.Open();
                    richTextBox1.AppendText(getTime() + " : Conectat la " + serialPort1.PortName + " .\n");
                    portTimer.Enabled = true;
                    sendTimer.Enabled = true;
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Error : " + ex.Message);
                }
            }
            else
            {
                serialPort1.Close();
                portOpen = false;

                button6.Text = "Open Port";
                portTimer.Enabled = false;
                sendTimer.Enabled = false;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            connectedToServer = false;
            textBox2.Text = "";
            textBox3.Text = "";
            url = "";
            port = "";
            ip = "";
        }
        float sendByte;
        private void portTimer_Tick(object sender, EventArgs e)
        {




            firstAxis = (float)Math.Round(jst.AxisA / 1092.25);  //1092.25 ca sa avem valori intre 0 si 60 pentru 65535/1092.25
            secondAxis = (float)Math.Round(jst.AxisC / 1092.25);//(jst.AxisC) / 32767 - 1;

            if (!isMoving && joystickConnected)
            {
                byteArr[0] = (byte)(30 + 0 * 60);
                byteArr[1] = (byte)(30 + 1 * 60);
                byteArr[2] = (byte)(30 + 2 * 60);
                byteArr[3] = (byte)(30 + 3 * 60);
                if (secondAxis != 30 || firstAxis != 30)
                {
                    isMoving = true;
                }
            }
            else if(!joystickConnected)
            {
                byteArr[0] = (byte)(30 + 0 * 60);
                byteArr[1] = (byte)(30 + 1 * 60);
                byteArr[2] = (byte)(30 + 2 * 60);
                byteArr[3] = (byte)(30 + 3 * 60);
            }
            else 
            {
                //****************Stanga/Dreapta******//
                if (secondAxis != 30)
                {
                    if (secondAxis < 30)
                    {
                        //***********Partea Stanga***********//
                        byteArr[0] = (byte)(secondAxis + 59);
                        byteArr[1] = (byte)(120 - secondAxis - 1);
                        byteArr[2] = (byte)(secondAxis + 120);
                        byteArr[3] = (byte)(secondAxis + 180);

                        if (byteArr[0] == 30)
                        {
                            byteArr[0] = 31;
                        }
                        if (byteArr[1] == 90)
                        {
                            byteArr[1] = 91;
                        }
                        if (byteArr[2] == 120)
                        {
                            byteArr[2] = 121;
                        }
                        if (byteArr[3] == 180)
                        {
                            byteArr[3] = 181;
                        }
                    }
                    else
                    {
                        //***********Partea Dreapta*********//
                        byteArr[0] = (byte)(60 - secondAxis);
                        byteArr[1] = (byte)(secondAxis + (60 - secondAxis));
                        byteArr[2] = (byte)(120 + secondAxis - 1); // (byte)((firstAxis + 2 * 60) + 1);
                        byteArr[3] = (byte)(secondAxis + 180 - 10);

                        if (byteArr[0] == 0)
                        {
                            byteArr[0] = 1;
                        }
                        if (byteArr[1] == 90)
                        {
                            byteArr[1] = 89;
                        }
                        if (byteArr[2] == 180)
                        {
                            byteArr[2] = 179;
                        }
                        if (byteArr[3] == 240)
                        {
                            byteArr[3] = 239;
                        }
                    }
                }
                //********************Fata-Spate***********//
                else if (firstAxis != 30)
                {
                    if (firstAxis < 30)
                    {
                        byteArr[0] = (byte)((firstAxis + 0 * 60) + 1);
                        byteArr[1] = (byte)((firstAxis + 1 * 60) + 1);
                        byteArr[2] = (byte)((firstAxis + 2 * 60) + 1);
                        byteArr[3] = (byte)((firstAxis + 3 * 60) + 1);
                    }
                    else
                    {
                        byteArr[0] = (byte)((firstAxis + 0 * 60) - 2);
                        byteArr[1] = (byte)((firstAxis + 1 * 60) - 2);
                        byteArr[2] = (byte)((firstAxis + 2 * 60) - 2);
                        byteArr[3] = (byte)((firstAxis + 3 * 60) - 2);
                    }
                    if (byteArr[0] < 30)
                    {
                        if (byteArr[0] == 0)
                        {
                            byteArr[0] = 1;
                        }
                        if (byteArr[1] == 60)
                        {
                            byteArr[1] = 61;
                        }
                        if (byteArr[2] == 120)
                        {
                            byteArr[2] = 121;
                        }
                        if (byteArr[3] == 180)
                        {
                            byteArr[3] = 181;
                        }
                    }
                    else
                    {
                        if (byteArr[0] == 60)
                        {
                            byteArr[0] = 59;
                        }
                        if (byteArr[1] == 120)
                        {
                            byteArr[1] = 119;
                        }
                        if (byteArr[2] == 180)
                        {
                            byteArr[2] = 179;
                        }
                        if (byteArr[3] == 240)
                        {
                            byteArr[3] = 239;
                        }
                    }
                }
                else
                {
                    isMoving = false;
                }
            }

            /*if (firstAxis > 30)
            {
                if (secondAxis < 0)
                {
                    speedRightFront = speedRightBack = firstAxis;
                    speedLeftFront = speedLeftBack = (float)Math.Floor(firstAxis * Math.Abs(secondAxis));
                }
                else if(secondAxis > 0)
                {
                    speedLeftFront = speedLeftBack = firstAxis;
                    speedRightFront = speedRightBack = (float)Math.Floor(firstAxis * Math.Abs(secondAxis));
                }
                else
                {
                    speedLeftFront = speedLeftBack = speedRightFront = speedRightBack = firstAxis;
                }
            }*/

            /* if (secondAxis == 1)
             {
                 secondAxis -= 0.0001F;
             }
   
             if (firstAxis == 0)
             {
                 firstAxis = 1;
             }
             if (secondAxis > 1.0)
             {
                 secondAxis = 1F;
             }
             speedLeftFront = speedRightFront = speedLeftBack = speedRightBack = firstAxis;
             if (secondAxis > 0.0)
             {
                 speedRightFront = speedRightBack = (float)Math.Floor(firstAxis * secondAxis); //sendByte = (float)(firstAxis * (1.0 - secondAxis));
             }
             else if (secondAxis < 0.0)
             {
                 speedLeftFront = speedLeftBack = (float)Math.Floor(firstAxis *  Math.Abs(secondAxis)); //sendByte = (float)(firstAxis * (1.0 - secondAxis));
             }
             if (firstAxis == 30)
             {
                 speedRightFront = speedRightBack = speedLeftFront = speedLeftBack = firstAxis;
             }*/

            //serialSend();

            /*else
            {
                sendByte = 30;
            }
            sendByte =(float) Math.Floor(sendByte);
            serialSend();

            if (secondAxis > 0.0)
            {

            }
            else if (secondAxis < 0.0)
            {

            }
            else
            {
                serialPort1.Write();
                serialPort1.Write();
                serialPort1.Write();
                serialPort1.Write();
            }*/

            /*speedLeftBack = speedLeftBack + 1 - 1;
            speedLeftFront += 60;
            speedRightFront += 120;
            speedRightBack += 180;

            richTextBox1.AppendText("//"+(byte)speedLeftFront + "   " + speedLeftBack    + "  " +
                                    + (byte)speedRightFront + "   " + speedRightBack  + " || " + 
                                    firstAxis + "   " + secondAxis + "\n"); */
        }


        /*public void serialSend(byte[] byteToSend, int position)
        {
            //richTextBox1.AppendText(byteToSend[position].ToString("X") + " \n");
             serialPort1.Write(byteToSend, position, 1);
             int z;
             for (int i = 0; i < 10000; i++)
                 z = (int)(3.143212 * 2.115432);

            //serialPort1.Write((byte[])speedLeftFront); 
            //serialPort1.Write((byte)speedLeftBack);
        }*/

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            //Debug.Print(indata);

            //richTextBox1.AppendText(indata);
        }
        int sendTimerCounter = 1;
        private void sendTimer_Tick(object sender, EventArgs e)
        {
            switch (sendTimerCounter)
            {
                case 1:
                    serialPort1.Write(byteArr, 0, 1);
                    sendTimerCounter++;
                    break;
                case 2:
                    serialPort1.Write(byteArr, 1, 1);
                    sendTimerCounter++;
                    break;
                case 3:
                    serialPort1.Write(byteArr, 2, 1);
                    sendTimerCounter++;
                    break;
                case 4:
                    serialPort1.Write(byteArr, 3, 1);
                    sendTimerCounter = 1;
                    break;

            }
            Debug.Write(firstAxis + "  " + secondAxis + "  " + byteArr[0] + "  " + byteArr[1] + "  " + byteArr[2] + "  " + byteArr[3]);
        }

        private void openCanvas_Click(object sender, EventArgs e)
        {
            Form frm2 = new canvas(this);
            frm2.Show();
        }

        public void form2Send(byte[] data)
        {
            sendTimerCounter = 5;
            for (int i = 0; i < 12; i++)
            {
                switch (sendTimerCounter)
                {
                    case 5:
                        serialPort1.Write(data, 0, 1);
                        sendTimerCounter++;
                        break;
                    case 6:
                        serialPort1.Write(data, 1, 1);
                        sendTimerCounter++;
                        break;
                    case 7:
                        serialPort1.Write(data, 2, 1);
                        sendTimerCounter++;
                        break;
                    case 8:
                        serialPort1.Write(data, 3, 1);
                        sendTimerCounter = 5;
                        break;

                }
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}