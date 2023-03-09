using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace NpcGenDataEditorByLuka
{
    public partial class TesteMapa : Form
    {
        public Form1 mainForm;
        PointF point;
        PointF pointAdd;

        public TesteMapa(Form1 form, Bitmap img)
        {
            mainForm = form;
            InitializeComponent();
            point = new PointF(0, 0);
            pictureBox_path.Image = img;
            pictureBox_path.Width = img.Width;
            pictureBox_path.Height = img.Height;
        }

        public void GetCoordinates(List<PointF> ls)
        {
            Graphics g = Graphics.FromImage(pictureBox_path.Image);
            int z = 1;
            if (pictureBox_path.Image.Size != new Size(8192, 11264))
                z = 2;
            foreach (var item in ls)
            {
                float X = ((item.X / z) + (pictureBox_path.Image.Width / 2)) - 10;
                float Y = (Math.Abs((item.Y / z) - (pictureBox_path.Image.Height / 2))) - 8;
                if (X <= (float)pictureBox_path.Image.Width && Y <= (float)pictureBox_path.Image.Height)
                {
                    g.DrawImage(NpcGenDataEditorByLuka.Properties.Resources.arrow, X, Y);
                }
            }
            pictureBox_path.AutoScrollOffset = new Point((Convert.ToInt32(ls[ls.Count - 1].X + (pictureBox_path.Image.Width / 2))) - (pictureBox_path.DisplayRectangle.Width / 2), Convert.ToInt32(Math.Abs(ls[ls.Count - 1].Y - (pictureBox_path.Image.Height / 2))) - (pictureBox_path.DisplayRectangle.Height / 2));
            //pictureBox_path.ScrollTo((Convert.ToInt32(ls[ls.Count - 1].X + (pictureBox_path.Image.Width / 2))) - (pictureBox_path.DisplayRectangle.Width / 2), Convert.ToInt32(Math.Abs(ls[ls.Count - 1].Y - (pictureBox_path.Image.Height / 2))) - (pictureBox_path.DisplayRectangle.Height / 2));
            pictureBox_path.Refresh();
        }
        public void SetPicture(Bitmap bmz)
        {
            pictureBox_path.Image = bmz;
        }


        private void path_tooltip(object sender, MouseEventArgs e)
        {
            string text = null;
            if (e.X > -1 && e.X < pictureBox_path.Image.Width && e.Y > -1 && e.Y < pictureBox_path.Image.Height)
            {
                float pos_x = e.X;
                float pos_y = e.Y;

                if (pictureBox_path.Image.Height > 10000)
                {
                    float posx = pos_x - 4096;
                    float posy = -pos_y + 5632;
                    double cx = pos_x / 10 - 9.6;
                    double cy = 1113.2 - (pos_y / 10);
                    text = string.Format("X: {0} | Y: {1}\nCX: {2} | CY: {3}\nMapa Principal", posx, posy, cx, cy);
                    pointAdd = new PointF(posx, posy);
                }
                else
                {
                    float numx = ((float)(e.X - pictureBox_path.Image.Width / 2) + 0.5f) * 2;
                    float numy = ((float)(-e.Y + pictureBox_path.Image.Height / 2) - 0.5f) * 2;
                    double cx = numx / 10 - 9.6;
                    double cy = 1113.2 - (numy / 10);
                    text = string.Format("X: {0} | Y: {1}\nCX: {2} | CY: {3}\nInstância", numx, numy, cx, cy);
                    pointAdd = new PointF(numx, numy);
                }

                if (text != toolTip.GetToolTip(pictureBox_path))
                {
                    toolTip.SetToolTip(pictureBox_path, text);
                }

                point = new PointF(pos_x, pos_y);
            }

        }

        public void DrawPoint()
        {
            Graphics g = Graphics.FromImage(pictureBox_path.Image);
            g.DrawImage(Properties.Resources.npc, point.X - 5, point.Y - 5, 10, 10);
            pictureBox_path.Refresh();
        }

        private void pictureBox_path_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void TesteMapa_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        void OpenAddFromMap(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var s = sender as BarManager;
            new AddFromMap(this, e.Item.Id, pointAdd).ShowDialog();
        }

        private void barButtonItem2_DefaultDropDownLinkChanged(object sender, DefaultDropDownLinkChangedEventArgs e)
        {

        }
    }
}
