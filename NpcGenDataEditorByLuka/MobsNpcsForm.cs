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
    public partial class MobsNpcsForm : Form
    {
        public MobsNpcsForm(Form1 fm, List<NpcMonster> ls, List<NpcMonster> rs, int Monsters, int Npcs)
        {
            this.resources = rs;
            InitializeComponent();
            this.Main_form = fm;
            this.MobsAndNpcs = ls;
            this.Monsters = Monsters;
            this.Npcs = Npcs;
            RefreshGrids();
        }
        Form1 Main_form;
        List<NpcMonster> MobsAndNpcs;
        int Monsters;
        int Npcs;
        List<NpcMonster> resources;
        int MainAction;
        int Window;
        List<int> MobsSearchIndexes = new List<int>();
        int MobsSearchPosition;
        List<int> NpcsSearchIndexes = new List<int>();
        int NpcsSearchPosition;
        List<int> ResourcesSearchIndexes = new List<int>();
        int ResourcesSearchPosition;
        public void RefreshGrids()
        {
            NpcsGrid.ScrollBars = ScrollBars.None;
            MobsGrid.ScrollBars = ScrollBars.None;
            ResourcesGrid.ScrollBars = ScrollBars.None;
            for (int i = 0; i < Monsters; i++)
            {
                MobsGrid.Rows.Add(i + 1, MobsAndNpcs[i].Id, MobsAndNpcs[i].Name);
            }
            for (int i = Monsters; i < Npcs + Monsters; i++)
            {
                NpcsGrid.Rows.Add(i + 1 - Monsters, MobsAndNpcs[i].Id, MobsAndNpcs[i].Name);
            }
            for (int i = 0; i < resources.Count; i++)
            {
                ResourcesGrid.Rows.Add(i + 1, resources[i].Id, resources[i].Name);
            }
            MobsGrid.ScrollBars = ScrollBars.Vertical;
            NpcsGrid.ScrollBars = ScrollBars.Vertical;
            ResourcesGrid.ScrollBars = ScrollBars.Vertical;
        }
        private void AcceptButton_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0 && Window == 1)
                Main_form.SetId(Convert.ToInt32(MobsGrid.CurrentRow.Cells[1].Value), MainAction,Window);
            else if (tabControl1.SelectedIndex == 0 && Window == 2)
                Main_form.SetId(Convert.ToInt32(MobsGrid.CurrentRow.Cells[1].Value), MainAction,Window);

            else if (tabControl1.SelectedIndex == 1 && Window == 1)
                Main_form.SetId(Convert.ToInt32(NpcsGrid.CurrentRow.Cells[1].Value), MainAction,Window);
            else if (tabControl1.SelectedIndex == 1 && Window == 2)
                Main_form.SetId(Convert.ToInt32(NpcsGrid.CurrentRow.Cells[1].Value), MainAction,Window);

            else if (tabControl1.SelectedIndex == 2 && Window == 1)
                Main_form.SetId(Convert.ToInt32(ResourcesGrid.CurrentRow.Cells[1].Value), MainAction,Window);
            else if (tabControl1.SelectedIndex == 2 && Window == 2)
                Main_form.SetId(Convert.ToInt32(ResourcesGrid.CurrentRow.Cells[1].Value), MainAction,Window);
            this.Hide();
        }
        private void ResourcesGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Main_form.SetId(Convert.ToInt32(ResourcesGrid.CurrentRow.Cells[1].Value), MainAction,Window);
            this.Hide();
        }
        private void NpcsGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Main_form.SetId(Convert.ToInt32(NpcsGrid.CurrentRow.Cells[1].Value), MainAction,Window);
            this.Hide();
        }
        private void MobsGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Main_form.SetId(Convert.ToInt32(MobsGrid.CurrentRow.Cells[1].Value), MainAction,Window);
            this.Hide();
        }
        public void FindRow(int Index, string act)
        {
            if (act == "Mob")
            {
                tabControl1.SelectedIndex = 0;
                MobsGrid.CurrentCell = MobsGrid.Rows[Index].Cells[1];
            }
            else if (act == "Npc")
            {
                tabControl1.SelectedIndex = 1;
                NpcsGrid.CurrentCell = NpcsGrid.Rows[Index].Cells[1];
            }
            else if (act == "Resource")
            {
                tabControl1.SelectedIndex = 2;
                ResourcesGrid.CurrentCell = ResourcesGrid.Rows[Index].Cells[1];
            }
        }
        public int SetAction
        {
            set
            {
                MainAction = value;
            }
        }
        public int SetWindow
        {
            set
            {
                Window = value;
            }
        }
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        public void RefreshLanguage(int Language)
        {
            if (Language == 1)
            {
                tabPage1.Text = "Мобы";
                tabPage2.Text = "Нипа";
                tabPage3.Text = "Ресурсы";
                AcceptButton.Text = "Выбрать";
                CancelButton.Text = "Отменить";
                label1.Text = "Поиск:";
                label2.Text = "Поиск:";
                label3.Text = "Поиск:";
            }
            else if (Language == 2)
            {
                tabPage1.Text = "Monsters";
                tabPage2.Text = "Npcs";
                tabPage3.Text = "Resources";
                AcceptButton.Text = "Select";
                CancelButton.Text = "Cancel";
                label1.Text = "Search:";
                label2.Text = "Search:";
                label3.Text = "Search:";
            }
        }
        private void MobsSearchTextbox_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(MobsSearchTextbox.Text))
            {
                MobsSearchPosition = 0;
                MobsSearchIndexes = MobsGrid.Rows.Cast<DataGridViewRow>().Where(i => i.Cells[1].Value.ToString().Contains(MobsSearchTextbox.Text) || i.Cells[2].Value.ToString().ToLower().Contains(MobsSearchTextbox.Text.ToLower())).Select(v=>v.Index).ToList();
                if (MobsSearchIndexes.Count != 0)
                {
                    MobsGrid.CurrentCell = MobsGrid.Rows[MobsSearchIndexes[0]].Cells[1];
                    MobsGrid.FirstDisplayedScrollingRowIndex = MobsGrid.Rows[MobsSearchIndexes[0]].Index;
                    MobsSearchPosition++;
                }
            }
        }
        private void NpcsSearchTextbox_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NpcsSearchTextbox.Text))
            {
                NpcsSearchPosition = 0;
                NpcsSearchIndexes = NpcsGrid.Rows.Cast<DataGridViewRow>().Where(i => i.Cells[1].Value.ToString().Contains(NpcsSearchTextbox.Text) || i.Cells[2].Value.ToString().ToLower().Contains(NpcsSearchTextbox.Text.ToLower())).Select(v=>v.Index).ToList();
                if (NpcsSearchIndexes.Count != 0)
                {
                    NpcsGrid.CurrentCell = NpcsGrid.Rows[NpcsSearchIndexes[0]].Cells[1];
                    NpcsGrid.FirstDisplayedScrollingRowIndex = NpcsGrid.Rows[NpcsSearchIndexes[0]].Index;
                    NpcsSearchPosition++;
                }
            }
        }
        private void ResourcesSearchTextbox_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ResourcesSearchTextbox.Text))
            {
                ResourcesSearchPosition = 0;
                ResourcesSearchIndexes = ResourcesGrid.Rows.Cast<DataGridViewRow>().Where(i => i.Cells[1].Value.ToString().Contains(ResourcesSearchTextbox.Text) || i.Cells[2].Value.ToString().ToLower().Contains(ResourcesSearchTextbox.Text.ToLower())).Select(v=>v.Index).ToList();
                if (ResourcesSearchIndexes.Count != 0)
                {
                    ResourcesGrid.CurrentCell = ResourcesGrid.Rows[ResourcesSearchIndexes[0]].Cells[1];
                    ResourcesGrid.FirstDisplayedScrollingRowIndex = ResourcesGrid.Rows[ResourcesSearchIndexes[0]].Index;
                    ResourcesSearchPosition++;
                }
            }
        }
        private void ContinueMobsSearch_Click(object sender, EventArgs e)
        {
            if (MobsSearchIndexes != null)
            {
                if (MobsSearchPosition != MobsSearchIndexes.Count)
                {
                    MobsGrid.CurrentCell = MobsGrid.Rows[MobsSearchIndexes[MobsSearchPosition]].Cells[1];
                    MobsGrid.FirstDisplayedScrollingRowIndex = MobsGrid.Rows[MobsSearchIndexes[MobsSearchPosition]].Index;
                    MobsSearchPosition++;
                }
            }
        }
        private void ContinueNpcsSearch_Click(object sender, EventArgs e)
        {
            if (NpcsSearchIndexes != null)
            {
                if (NpcsSearchPosition != NpcsSearchIndexes.Count)
                {
                    NpcsGrid.CurrentCell = NpcsGrid.Rows[NpcsSearchIndexes[NpcsSearchPosition]].Cells[1];
                    NpcsGrid.FirstDisplayedScrollingRowIndex = NpcsGrid.Rows[NpcsSearchIndexes[NpcsSearchPosition]].Index;
                    NpcsSearchPosition++;
                }
            }
        }
        private void ContinueResourcesSearch_Click(object sender, EventArgs e)
        {
            if (ResourcesSearchIndexes != null)
            {
                if (ResourcesSearchPosition != ResourcesSearchIndexes.Count)
                {
                    ResourcesGrid.CurrentCell = ResourcesGrid.Rows[ResourcesSearchIndexes[ResourcesSearchPosition]].Cells[1];
                    ResourcesGrid.FirstDisplayedScrollingRowIndex = ResourcesGrid.Rows[ResourcesSearchIndexes[ResourcesSearchPosition]].Index;
                    ResourcesSearchPosition++;
                }
            }

        }
        private void MobsSearchTextbox_DoubleClick(object sender, EventArgs e)
        {
            MobsSearchTextbox.SelectAll();
        }
        private void NpcsSearchTextbox_DoubleClick(object sender, EventArgs e)
        {
            NpcsSearchTextbox.SelectAll();
        }
        private void ResourcesSearchTextbox_DoubleClick(object sender, EventArgs e)
        {
            ResourcesSearchTextbox.SelectAll();
        }
    }
}
