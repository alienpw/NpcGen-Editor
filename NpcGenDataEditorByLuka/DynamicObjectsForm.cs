using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace NpcGenDataEditorByLuka
{
    public partial class DynamicObjectsForm : Form
    {
        public DynamicObjectsForm(List<DefaultInformation> Default,Form1 fm)
        {
            this.DynamicsList = Default;
            InitializeComponent();
            SortDynamics();
            this.Main_form = fm;
        }
        List<DefaultInformation> DynamicsList;
        Form1 Main_form;
        int Window;
        public int SetWindow
        {
            set
            {
                Window = value;
            }
        }
        public void SortDynamics()
        {
            DynamicGrid.Rows.Clear();
            for(int i = 0;i<DynamicsList.Count;i++)
            {
                DynamicGrid.Rows.Add(i + 1, DynamicsList[i].Id, DynamicsList[i].Name);
            }
        }
        private void DynamicGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            if(DynamicGrid.CurrentRow!=null)
            {
                if(DynamicGrid.CurrentRow.Index!=-1)
                {
                    string DynScreenPath = string.Format("{0}\\DynamicObjects\\d{1}.jpg", Application.StartupPath, DynamicGrid.Rows[DynamicGrid.CurrentRow.Index].Cells[1].Value);
                    if (File.Exists(DynScreenPath))
                    {
                        DynamicPictureBox.Image = Image_resize(Bitmap.FromFile(DynScreenPath),456,257);
                    }
                }
            }
        }
        public void LanguageChange(int Language)
        {
            if(Language==1)
            {
                Accept.Text = "Принять";
                Cancel.Text = "Отменить";
            }
            else if (Language == 2)
            {
                Accept.Text = "Accept";
                Cancel.Text = "Cancel";
            }
        }
        public void SetColor(int Color)
        {
            if(Color==2)
            {
                this.BackColor = System.Drawing.Color.FromArgb(58, 58, 58);
                DynamicPictureBox.BackColor = System.Drawing.Color.FromArgb(58, 58, 58);
            }
            else
            {
                this.BackColor = System.Drawing.Color.WhiteSmoke;
                DynamicPictureBox.BackColor = System.Drawing.Color.WhiteSmoke;
            }
        }

        public Image Image_resize(Image b, int Width, int Height)
        {
            Image result = new Bitmap(Width, Height);
            using (Graphics g = Graphics.FromImage((Image)result))
            {
                g.DrawImage(b, 0, 0, Width, Height);
                g.Dispose();
            }
            return result;
        }
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        private void DynamicGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Main_form.SetId(Convert.ToInt32(DynamicGrid.CurrentRow.Cells[1].Value), 3,Window);
            this.Hide();
        }
        private void Accept_Click(object sender, EventArgs e)
        {
            if(DynamicGrid.CurrentRow!=null)
            {
                if(DynamicGrid.CurrentRow.Index!=-1)
                {
                    Main_form.SetId(Convert.ToInt32(DynamicGrid.CurrentRow.Cells[1].Value), 3,Window);
                    this.Hide();
                }
            }
        }
    }
}
