using DevExpress.XtraEditors;
using DevExpress.XtraEditors.ButtonPanel;
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
        PointF location;

        public MapWindow()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }

        public void GetCoordinates(List<PointF> ls, float angle)
        {
            //Graphics g = Graphics.FromImage(pictureBox1.BackgroundImage);
            int z = 1;
            if (pictureBox1.BackgroundImage.Size != new Size(8192, 11264))
                z = 2;
            foreach (var item in ls)
            {
                float X = ((item.X / z) + (pictureBox1.BackgroundImage.Width / 2));
                float Y = (Math.Abs((item.Y / z) - (pictureBox1.BackgroundImage.Height / 2)));
                if (X <= (float)pictureBox1.BackgroundImage.Width && Y <= (float)pictureBox1.BackgroundImage.Height)
                {
                    /*System.Drawing.Drawing2D.Matrix m = g.Transform;
                    m.RotateAt(angle, new PointF(item.X, item.Y), System.Drawing.Drawing2D.MatrixOrder.Append);
                    g.Transform = m;*/
                   // g.DrawImage(Extensions.ResizeImage(Resources.gota, 10, 13), location.X - 6, location.Y - 8);
                    AddPoint(new PointF(X, Y), angle);
                }
            }
            panel1.AutoScrollPosition = new Point
            {
                X = Convert.ToInt32(ls[ls.Count - 1].X + (pictureBox1.BackgroundImage.Width / 2)) - (panel1.Width / 2),
                Y = Convert.ToInt32(Math.Abs(ls[ls.Count - 1].Y - (pictureBox1.BackgroundImage.Height / 2)) - (panel1.Height / 2))
            };
            pictureBox1.Refresh();
        }

        private void path_tooltip(object sender, MouseEventArgs e)
        {
            string text = null;
            if (e.X > -1 && e.X < pictureBox1.BackgroundImage.Width && e.Y > -1 && e.Y < pictureBox1.BackgroundImage.Height)
            {
                double num = (double)(e.X - pictureBox1.BackgroundImage.Width / 2) + 0.5;
                double num2 = (double)(-e.Y + pictureBox1.BackgroundImage.Height / 2) - 0.5;
                text = string.Concat(string.Concat("X: " + num.ToString("F1"), "\nZ: "), num2.ToString("F1"));                
                if (text != toolTip.GetToolTip(pictureBox1))
                {
                    toolTip.SetToolTip(pictureBox1, text);
                }
            }
            location = new PointF(e.X, e.Y);
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            AddPoint(location, 0f);
        }

        public void AddPoint(PointF loc, float angle)
        {
            Graphics g = Graphics.FromImage(pictureBox1.BackgroundImage);
            System.Drawing.Drawing2D.Matrix m = g.Transform;
            m.RotateAt(angle, loc, System.Drawing.Drawing2D.MatrixOrder.Append);
            g.Transform = m;
            g.DrawImage(Extensions.ResizeImage(Resources.gota, 10, 13), loc.X - 6, loc.Y - 8);
            pictureBox1.Refresh();
        }
    }
}