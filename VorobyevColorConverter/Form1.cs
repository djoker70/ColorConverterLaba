using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static VorobyevColorConverter.Program;


namespace VorobyevColorConverter
{
    public partial class Form1 : Form
    {
        ColorConvert cc = new ColorConvert();
        public Form1()
        {
            InitializeComponent();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string imagePath;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "image files|*.jpg;*.png";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                imagePath = dialog.FileName;
                cc.rgbImage = new Bitmap(imagePath);
                this.pictureBox1.Image = cc.rgbImage;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //this.Cursor = Cursor.WaitCursor;
            cc.ConvertRGBimagetoHSV();
            int width = cc.rgbImage.Width;
            int height = cc.rgbImage.Height;

            if(this.checkBox1.Checked)
            {
                width = width / 2;
            }
      

            for(int i = 0; i < width; i++)
                for(int j = 0; j < height; j++)
                {
                    //тон
                    int tmpH = cc.hsvImage[i, j].h + trackBar1.Value;
                    if (tmpH < 0)
                        cc.hsvImage[i, j].h = (ushort)(tmpH + 360);
                    else if (tmpH >= 360)
                        cc.hsvImage[i, j].h = (ushort)(tmpH - 360);
                    else
                        cc.hsvImage[i, j].h = (ushort)tmpH;
                    //насыщенность
                    int tmpS = cc.hsvImage[i, j].s + trackBar2.Value;
                    if (tmpS < 0)
                        cc.hsvImage[i, j].s = 0;
                    else if (tmpS >= 100)
                        cc.hsvImage[i, j].s = 100;
                    else
                        cc.hsvImage[i, j].s = (byte)(tmpS);
                    //яркость
                    int tmpV = cc.hsvImage[i, j].v + trackBar3.Value;
                    if (tmpV < 0)
                        cc.hsvImage[i, j].v = 0;
                    else if (tmpS >= 100)
                        cc.hsvImage[i, j].v = 100;
                    else
                        cc.hsvImage[i, j].v = (byte)(tmpV);
                }

            trackBar1.Value = 0;
            trackBar2.Value = 0;
            trackBar3.Value = 0;
            cc.ConvertHSVimagetoRGB();
            pictureBox1.Image = cc.rgbImage;
            //this.Cursor = Cursor.Default;
        }

        /*private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Title = "Сохранить картинку как ...";
            saveFileDialog1.OverwritePrompt = true;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.Filter = "Image Files(*.jpg)|*.jpg";
            saveFileDialog1.ShowHelp = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    Bitmap bmp = new Bitmap(this.pictureBox1.Image);
                    //string path = saveFileDialog1.FileName;
                    //костыль,спросить как надо.
                    string path = "C:\\Users\\djoke\\Desktop\\resultConvert.jpg";
                    bmp.Save($"{path}");
                }
            }
        }*/
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Jpeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            saveFileDialog1.Title = "Сохранить изображение";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                FileStream fs = (FileStream)saveFileDialog1.OpenFile();

                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        this.pictureBox1.Image.Save(fs, ImageFormat.Jpeg);
                        break;
                    case 2:
                        this.pictureBox1.Image.Save(fs, ImageFormat.Bmp);
                        break;
                    case 3:
                        this.pictureBox1.Image.Save(fs, ImageFormat.Gif);
                        break;
                }
                fs.Close();
            }
        }
    }
}
