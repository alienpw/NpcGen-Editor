using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NpcGenDataEditorByLuka
{
    public partial class ShowLocationWindow : Form
    {
        public ShowLocationWindow(Form1 fm, Bitmap fff)
        {
            this.Main_form = fm;
            InitializeComponent();
            MapImage = fff;
            this.pictureBox1.Image = fff;
        }
        Form1 Main_form;
        Bitmap MapImage;
        public void GetCoordinates(List<PointF> ls)
        {
            pictureBox1.Zoom = 100;
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            int z = 1;
            if (pictureBox1.Image.Size != new Size(8192, 11264))
                z = 2;
            foreach (var item in ls)
            {
                float X = ((item.X / z) + (pictureBox1.Image.Width / 2)) - 10;
                float Y = (Math.Abs((item.Y / z) - (pictureBox1.Image.Height / 2))) - 8;
                if (X <= (float)pictureBox1.Image.Width && Y <= (float)pictureBox1.Image.Height)
                {
                    g.DrawImage(NpcGenDataEditorByLuka.Properties.Resources.arrow, X, Y);
                }
            }
            pictureBox1.ScrollTo((Convert.ToInt32(ls[ls.Count - 1].X + (pictureBox1.Image.Width / 2))) - (pictureBox1.DisplayRectangle.Width / 2), Convert.ToInt32(Math.Abs(ls[ls.Count - 1].Y - (pictureBox1.Image.Height / 2))) - (pictureBox1.DisplayRectangle.Height / 2));
            pictureBox1.Refresh();
        }
        public void SetPicture(Bitmap bmz)
        {
            pictureBox1.Image = bmz;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }

}
