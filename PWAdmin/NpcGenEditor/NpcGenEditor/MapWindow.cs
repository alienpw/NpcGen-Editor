using DevExpress.XtraEditors;
using NpcGenEditor.Classes;
using NpcGenEditor.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NpcGenEditor
{
    public partial class MapWindow : DevExpress.XtraEditors.XtraForm
    {
        public MapWindow()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }

        private void path_drawBackground(Image img)
        {

        }


        public void GetCoordinates(List<PointF> ls)
        {
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
                    g.DrawImage(Extensions.ResizeImage(Resources.npc, 20,20), X, Y);
                }
            }
            panel1.AutoScrollPosition = new Point((Convert.ToInt32(ls[ls.Count - 1].X + (pictureBox1.Image.Width / 2))) - (pictureBox1.DisplayRectangle.Width / 2), Convert.ToInt32(Math.Abs(ls[ls.Count - 1].Y - (pictureBox1.Image.Height / 2))) - (pictureBox1.DisplayRectangle.Height / 2));
            pictureBox1.Refresh();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        private void path_tooltip(object sender, MouseEventArgs e)
        {
            string text = null;
            if (e.X > -1 && e.X < pictureBox1.BackgroundImage.Width && e.Y > -1 && e.Y < pictureBox1.BackgroundImage.Height)
            {
                double num = (double)(e.X - pictureBox1.BackgroundImage.Width / 2) + 0.5;
                double num2 = (double)(-e.Y + pictureBox1.BackgroundImage.Height / 2) - 0.5;
                text = string.Concat(string.Concat("X: " + num.ToString("F1"), "\nZ: "), num2.ToString("F1"));
                Size clientSize = panel1.ClientSize;
                int num3 = Convert.ToInt32(Math.Round((double)num + (double)(float)(pictureBox1.Image.Width / 2) - 0.5)) - clientSize.Width / 2;
                Size clientSize2 = panel1.ClientSize;
                int num4 = Convert.ToInt32(Math.Round((double)(0f - num2) + (double)(float)(pictureBox1.Image.Height / 2) - 0.5)) - clientSize2.Height / 2;
                if (num3 > panel1.HorizontalScroll.Value && num3 < panel1.HorizontalScroll.Maximum)
                {
                    panel1.HorizontalScroll.Value = num3;
                }
                if (num4 > panel1.VerticalScroll.Minimum && num4 < panel1.VerticalScroll.Maximum)
                {
                    panel1.VerticalScroll.Value = num4;
                }
                text += $"\nscrol X: {num3}\nscrol Y: {num4}";
                
                if (text != toolTip.GetToolTip(pictureBox1))
                {
                    toolTip.SetToolTip(pictureBox1, text);
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_BackgroundImageChanged(object sender, EventArgs e)
        {
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            GetCoordinates(new List<PointF>() { MousePosition });
        }
    }
}