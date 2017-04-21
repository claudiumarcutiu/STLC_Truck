namespace JoystickSample
{
    partial class canvas
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.bTrimite = new System.Windows.Forms.Button();
            this.tbProgress = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.buttonStratCalibration = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonStopCalibration = new System.Windows.Forms.Button();
            this.tbFrecare = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bResetCanvas = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox1.Location = new System.Drawing.Point(5, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(613, 418);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // bTrimite
            // 
            this.bTrimite.Location = new System.Drawing.Point(650, 25);
            this.bTrimite.Name = "bTrimite";
            this.bTrimite.Size = new System.Drawing.Size(131, 57);
            this.bTrimite.TabIndex = 1;
            this.bTrimite.Text = "Trimite date";
            this.bTrimite.UseVisualStyleBackColor = true;
            this.bTrimite.Click += new System.EventHandler(this.bTrimite_Click);
            // 
            // tbProgress
            // 
            this.tbProgress.Location = new System.Drawing.Point(23, 19);
            this.tbProgress.Name = "tbProgress";
            this.tbProgress.Size = new System.Drawing.Size(100, 20);
            this.tbProgress.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.groupBox1.Controls.Add(this.tbProgress);
            this.groupBox1.Location = new System.Drawing.Point(640, 191);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(154, 49);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Progres";
            // 
            // buttonStratCalibration
            // 
            this.buttonStratCalibration.Location = new System.Drawing.Point(6, 16);
            this.buttonStratCalibration.Name = "buttonStratCalibration";
            this.buttonStratCalibration.Size = new System.Drawing.Size(75, 23);
            this.buttonStratCalibration.TabIndex = 5;
            this.buttonStratCalibration.Text = "Start";
            this.buttonStratCalibration.UseVisualStyleBackColor = true;
            this.buttonStratCalibration.Click += new System.EventHandler(this.buttonStratCalibration_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.groupBox2.Controls.Add(this.buttonStopCalibration);
            this.groupBox2.Controls.Add(this.buttonStratCalibration);
            this.groupBox2.Location = new System.Drawing.Point(624, 287);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(170, 52);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Calibrare";
            // 
            // buttonStopCalibration
            // 
            this.buttonStopCalibration.Location = new System.Drawing.Point(87, 16);
            this.buttonStopCalibration.Name = "buttonStopCalibration";
            this.buttonStopCalibration.Size = new System.Drawing.Size(75, 23);
            this.buttonStopCalibration.TabIndex = 6;
            this.buttonStopCalibration.Text = "Stop";
            this.buttonStopCalibration.UseVisualStyleBackColor = true;
            this.buttonStopCalibration.Click += new System.EventHandler(this.buttonStopCalibration_Click);
            // 
            // tbFrecare
            // 
            this.tbFrecare.Location = new System.Drawing.Point(681, 392);
            this.tbFrecare.Name = "tbFrecare";
            this.tbFrecare.Size = new System.Drawing.Size(100, 20);
            this.tbFrecare.TabIndex = 7;
            this.tbFrecare.TextChanged += new System.EventHandler(this.tbFrecare_TextChanged);
            this.tbFrecare.MouseLeave += new System.EventHandler(this.tbFrecare_MouseLeave);
            this.tbFrecare.MouseHover += new System.EventHandler(this.tbFrecare_MouseHover);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(695, 376);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Coef. Frecare";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // bResetCanvas
            // 
            this.bResetCanvas.Location = new System.Drawing.Point(650, 96);
            this.bResetCanvas.Name = "bResetCanvas";
            this.bResetCanvas.Size = new System.Drawing.Size(131, 63);
            this.bResetCanvas.TabIndex = 9;
            this.bResetCanvas.Text = "Resetare";
            this.bResetCanvas.UseVisualStyleBackColor = true;
            this.bResetCanvas.Click += new System.EventHandler(this.bResetCanvas_Click);
            // 
            // canvas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 424);
            this.Controls.Add(this.bResetCanvas);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbFrecare);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.bTrimite);
            this.Controls.Add(this.pictureBox1);
            this.Name = "canvas";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.canvas_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button bTrimite;
        private System.Windows.Forms.TextBox tbProgress;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Button buttonStratCalibration;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonStopCalibration;
        private System.Windows.Forms.TextBox tbFrecare;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bResetCanvas;
    }
}