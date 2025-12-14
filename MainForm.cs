using System;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace ImagePixelDemo
{
    public partial class MainForm : Form
    {
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Button btnStep;
        private Button btnStartStop;
        private Button btnLoad1;
        private Button btnLoad2;
        private Timer animationTimer;
        private bool isAnimating = false;
        private string[] imageFiles;
        private int currentIndex = 0;

        public MainForm() 
        {
            this.Text = "Image Pixel Demo";
            this.ClientSize = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            int imageWidth = 350;
            int imageHeight = 350;
            int spacing = 40;
            int centerX = (this.ClientSize.Width - (imageWidth * 2 + spacing)) / 2;
            int centerY = (this.ClientSize.Height - imageHeight) / 2 - 60;

            pictureBox1 = new PictureBox
            {
                Width = imageWidth,
                Height = imageHeight,
                BorderStyle = BorderStyle.FixedSingle,
                Left = centerX,
                Top = centerY,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            pictureBox2 = new PictureBox
            {
                Width = imageWidth,
                Height = imageHeight,
                BorderStyle = BorderStyle.FixedSingle,
                Left = centerX + imageWidth + spacing,
                Top = centerY,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            btnStep = new Button
            {
                Text = "Step",
                Width = 120,
                Height = 40,
                Left = centerX,
                Top = pictureBox1.Bottom + 20
            };

            btnStartStop = new Button
            {
                Text = "Start",
                Width = 120,
                Height = 40,
                Left = centerX + imageWidth + spacing,
                Top = pictureBox2.Bottom + 20
            };

            btnLoad1 = new Button
            {
                Text = "Load Image 1",
                Width = 120,
                Height = 40,
                Left = centerX,
                Top = btnStep.Bottom + 10
            };

            btnLoad2 = new Button
            {
                Text = "Load Images 2",
                Width = 120,
                Height = 40,
                Left = centerX + imageWidth + spacing,
                Top = btnStartStop.Bottom + 10
            };
            animationTimer = new Timer { Interval = 3 };
            animationTimer.Tick += AnimationTimer_Tick;

            Controls.Add(pictureBox1);
            Controls.Add(pictureBox2);
            Controls.Add(btnStep);
            Controls.Add(btnStartStop);
            Controls.Add(btnLoad1);
            Controls.Add(btnLoad2);

            btnStep.Click += BtnStep_Click;
            btnStartStop.Click += BtnStartStop_Click;
            btnLoad1.Click += BtnLoad1_Click;
            btnLoad2.Click += BtnLoad2_Click;
        }

        private void BtnStep_Click(object? sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
                pictureBox1.Image = ApplyFilter((Bitmap)pictureBox1.Image);
        }

        private void BtnStartStop_Click(object? sender, EventArgs e)
        {
            if (!isAnimating)
            {
                if (imageFiles != null && imageFiles.Length > 0)
                {
                    animationTimer.Start();
                    btnStartStop.Text = "Stop";
                    isAnimating = true;
                }
                else
                {
                    MessageBox.Show("حمّل صور أولاً.");
                }
            }
            else
            {
                animationTimer.Stop();
                btnStartStop.Text = "Start";
                isAnimating = false;
            }
        }

        private void BtnLoad1_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = new Bitmap(ofd.FileName);
                }
            }
        }

        private void BtnLoad2_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                ofd.Multiselect = true;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    imageFiles = ofd.FileNames;
                    currentIndex = 0;
                    pictureBox2.Image = new Bitmap(imageFiles[currentIndex]);
                }
            }
        }

        private void AnimationTimer_Tick(object? sender, EventArgs e)
        {
            if (imageFiles != null && imageFiles.Length > 0)
            {
                currentIndex++;
                if (currentIndex >= imageFiles.Length)
                    currentIndex = 0;

                pictureBox2.Image = new Bitmap(imageFiles[currentIndex]);
            }
        }

        private Bitmap ApplyFilter(Bitmap bmp)
        {
            Bitmap newBmp = new Bitmap(bmp.Width, bmp.Height);
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color c = bmp.GetPixel(x, y);
                    Color inverted = Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B);
                    newBmp.SetPixel(x, y, inverted);
                }
            }
            return newBmp;
        }
    }
}