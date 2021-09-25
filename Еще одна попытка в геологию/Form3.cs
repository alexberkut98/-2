using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
namespace Еще_одна_попытка_в_геологию
{
    public partial class Form3 : Form
    {
        Image img;
        int angle;
        Size StartSize;
        int newWidth = 0;
        int newHeight = 0;
        int size = 1;
        public Form3(Image A)
        {   
            InitializeComponent();
            img = A;
            StartSize = img.Size;
        }
        public static Image resizeImage(Image img, Size NewSize)
        {
            Image NewImage = new Bitmap(NewSize.Width, NewSize.Height);
            using (Graphics G = Graphics.FromImage((Bitmap)NewImage))
            {
                G.DrawImage(img, new Rectangle(Point.Empty, NewSize));
            }
            return NewImage;
        }
        private void Form3_Paint(object sender, PaintEventArgs e)
        {
                newWidth = 0;
                newHeight = 0;
                Bitmap bmp1 = new Bitmap(img.Width, img.Height);
                angle = (int)(angle * 3.6);
                if (angle <= 90)
                {
                    newWidth = (int)(bmp1.Width * Math.Cos(2 * Math.PI * angle / 360) + bmp1.Height * Math.Sin(2 * Math.PI * angle / 360));
                    newHeight = (int)(bmp1.Height * Math.Cos(2 * Math.PI * angle / 360) + bmp1.Width * Math.Sin(2 * Math.PI * angle / 360));
                }
                if (angle > 90 && angle <= 180)
                {
                    newWidth = (int)(bmp1.Width * -Math.Cos(2 * Math.PI * angle / 360) + bmp1.Height * Math.Sin(2 * Math.PI * angle / 360));
                    newHeight = (int)(bmp1.Height * -Math.Cos(2 * Math.PI * angle / 360) + bmp1.Width * Math.Sin(2 * Math.PI * angle / 360));
                }
                if (angle > 180 && angle <= 270)
                {
                    newWidth = (int)(bmp1.Width * -Math.Cos(2 * Math.PI * angle / 360) + bmp1.Height * -Math.Sin(2 * Math.PI * angle / 360));
                    newHeight = (int)(bmp1.Height * -Math.Cos(2 * Math.PI * angle / 360) + bmp1.Width * -Math.Sin(2 * Math.PI * angle / 360));
                }
                if (angle > 270 && angle <= 360)
                {
                    newWidth = (int)(bmp1.Width * Math.Cos(2 * Math.PI * angle / 360) + bmp1.Height * -Math.Sin(2 * Math.PI * angle / 360));
                    newHeight = (int)(bmp1.Height * Math.Cos(2 * Math.PI * angle / 360) + bmp1.Width * -Math.Sin(2 * Math.PI * angle / 360));
                }
                Bitmap bmp = new Bitmap(newWidth, newHeight);
                Graphics g = Graphics.FromImage(bmp);
                g.TranslateTransform(newWidth / 2, newHeight / 2);
                g.RotateTransform(angle);
                g.TranslateTransform(-img.Width / 2, -img.Height / 2);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, 0, 0);
                e.Graphics.TranslateTransform(Width / 2, Height / 2);
                e.Graphics.DrawImage(bmp, -bmp.Width / 2, -bmp.Height / 2);
        }

        private void Form3_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            angle = Ang.Value;
            Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (size * 2 < 16)
            {
                size *= 2;
                StartSize.Width *= 2;
                StartSize.Height *= 2;
                img = resizeImage(img, StartSize);
                Invalidate();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (size * 3 < 16)
            {
                size *= 3;
                StartSize.Width *= 3;
                StartSize.Height *= 3;
                img = resizeImage(img, StartSize);
                Invalidate();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (size * 5 < 16)
            {
                size *= 5;
                StartSize.Width *= 5;
                StartSize.Height *= 5;
                img = resizeImage(img, StartSize);
                Invalidate();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (StartSize.Width / 2 != 0 && StartSize.Height / 2 != 0)
            {
                size /= 2;
                StartSize.Width /= 2;
                StartSize.Height /= 2;
                img = resizeImage(img, StartSize);
                Invalidate();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (StartSize.Width / 3 != 0 && StartSize.Height / 3 != 0)
            {
                size /= 3;
                StartSize.Width /= 3;
                StartSize.Height /= 3;
                img = resizeImage(img, StartSize);
                Invalidate();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (StartSize.Width /5 != 0 && StartSize.Height / 5 != 0)
            {
                size /= 5;
                StartSize.Width /= 5;
                StartSize.Height /= 5;
                img = resizeImage(img, StartSize);
                Invalidate();
            }
        }
    }
}
