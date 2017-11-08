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
using System.Xml;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using LBLIBRARY;
namespace NpcGenDataEditorByLuka
{
    public partial class Form1 : Form
    {
        [DllImport("Kernel32.dll")]
        static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, int lpNumberOfBytesRead);
        public Form1()
        {
            InitializeComponent();
            try
            {
                KBDHook.LocalHook = false;
                KBDHook.KeyDown += new KBDHook.HookKeyPress(KBDHook_KeyDown);
                KBDHook.InstallHook();
            }
            catch { }
        }
        #region MainMeanings
        List<int> SearchMonsters;
        List<int> SearchResources;
        List<int> SearchDynamics;
        List<int> SearchTriggers;
        NpcGen Read;
        Elementsdata Element;
        Pck_engine PckFile;
        ShowLocationWindow MapForm;
        MobsNpcsForm ChooseFromElementsForm;
        DynamicObjectsForm DynamicForm;
        List<ClassDefaultMonsters> MonstersContact;
        List<ClassDefaultResources> ResourcesContact;
        List<ClassDynamicObject> DynamicsContact;
        List<DefaultInformation> DynamicsListRu;
        List<DefaultInformation> DynamicsListEn;
        List<MapLoadedInformation> LoadedMapConfigs;
        List<GameMapInfo> Maps;
        List<int> NpcRowCollection;
        List<int> UnderNpcRowCollection;
        List<int> ResourcesRowCollection;
        List<int> UnderResourcesRowCollection;
        List<int> DynamicsRowCollection;
        List<int> TriggersRowCollection;
        /// <summary>
        /// Список ошибочных существ(Индес в списке ошибок,Индес в списке существ)
        /// </summary>
        List<IntDictionary> ErrorExistenceCollection = new List<IntDictionary>();
        List<IntDictionary> ErrorResourcesCollection = new List<IntDictionary>();
        List<IntDictionary> ErrorDynamicsCollection = new List<IntDictionary>();
        Keys MonstersDefaultKey;
        Keys MonstersExtraKey;
        Keys ResourcesDefaultKey;
        Keys ResourcesExtraKey;
        Keys DynamicsDefaultKey;
        Keys DynamicsExtraKey;
        int NpcRowIndex;
        int NpcGroupIndex;
        int ResourcesRowIndex;
        int ResourcesGroupIndex;
        int DynamicRowIndex;
        int TriggersRowIndex;
        int Action;
        int Language = 1;
        int InterfaceColor = 1;
        bool AllowCellChanging = true;
        int ErrorsLanguage;
        #endregion
        #region Extra
        void KBDHook_KeyDown(LLKHEventArgs e)
        {
            ClassPosition cs = null;
            if ((e.Keys == MonstersExtraKey && ModifierKeys == MonstersDefaultKey) || (e.Keys == ResourcesExtraKey && ModifierKeys == ResourcesDefaultKey) || (e.Keys == DynamicsExtraKey && ModifierKeys == DynamicsDefaultKey))
            {
                cs = GetCoordinates();
            }
            if (cs != null)
            {
                if (e.Keys == MonstersExtraKey && ModifierKeys == MonstersDefaultKey)
                {
                    Read.NpcMobsAmount++;
                    ClassDefaultMonsters dm = new ClassDefaultMonsters()
                    {
                        X_position = cs.PosX,
                        Y_position = cs.PosY,
                        Z_position = cs.PosZ,
                        X_direction = cs.DirX,
                        Y_direction = cs.DirY,
                        Z_direction = cs.DirZ,
                        Amount_in_group = 1,
                        Location = AddintExistenceType.SelectedIndex,
                        Type = AddMonsterType.SelectedIndex,
                        Trigger_id = Convert.ToInt32(AddMonsterTrigger.Value)
                    };
                    ClassExtraMonsters ex = new ClassExtraMonsters()
                    {
                        Id = Convert.ToInt32(AddMonsterId.Value),
                        Amount = Convert.ToInt32(AddMonsterAmount.Value),
                        Respawn = Convert.ToInt32(AddMonsterRespawnTime.Value)
                    };
                    dm.MobDops = new List<ClassExtraMonsters>
                    {
                        ex
                    };
                    Read.NpcMobList.Add(dm);
                    string k = "?";
                    int j = Element.ExistenceLists.FindIndex(z => z.Id == dm.MobDops[0].Id);
                    if (j != -1)
                    {
                        k = Element.ExistenceLists[j].Name;
                    }
                    NpcMobsGrid.Rows.Add(Read.NpcMobsAmount, dm.MobDops[0].Id, k);
                    NpcMobsGrid.CurrentCell = NpcMobsGrid.Rows[NpcMobsGrid.Rows.Count - 1].Cells[1];
                }
                if (e.Keys == ResourcesExtraKey && ModifierKeys == ResourcesDefaultKey)
                {
                    Read.ResourcesAmount++;
                    ClassDefaultResources dm = new ClassDefaultResources()
                    {
                        X_position = cs.PosX,
                        Y_position = cs.PosY,
                        Z_position = cs.PosZ,
                        Amount_in_group = 1,
                        Trigger_id = Convert.ToInt32(AddResourcesTrigger.Value)
                    };
                    ClassExtraResources ex = new ClassExtraResources()
                    {
                        Id = Convert.ToInt32(AddResourceID.Value),
                        Amount = Convert.ToInt32(AddResourceAmount.Value),
                        Respawntime = Convert.ToInt32(AddResourceRespawnTime.Value)
                    };
                    dm.ResExtra = new List<ClassExtraResources>
                    {
                        ex
                    };
                    Read.ResourcesList.Add(dm);
                    string k = "?";
                    int j = Element.ResourcesList.FindIndex(z => z.Id == dm.ResExtra[0].Id);
                    if (j != -1)
                    {
                        k = Element.ResourcesList[j].Name;
                    }
                    ResourcesGrid.Rows.Add(Read.NpcMobsAmount, dm.ResExtra[0].Id, k);
                    ResourcesGrid.CurrentCell = ResourcesGrid.Rows[ResourcesGrid.Rows.Count - 1].Cells[1];
                }
                if (e.Keys == DynamicsExtraKey && ModifierKeys == DynamicsDefaultKey)
                {
                    ClassDynamicObject dm = new ClassDynamicObject()
                    {
                        Id = Convert.ToInt32(AddDynamicsTrigger.Value),
                        TriggerId = Convert.ToInt32(AddDynamicsID.Value),
                        X_position = cs.PosX,
                        Y_position = cs.PosY,
                        Z_position = cs.PosZ
                    };
                    Read.DynobjectAmount++;
                    Read.DynamicsList.Add(dm);
                    string k = "?";
                    if (Language == 1)
                    {
                        int j = DynamicsListRu.FindIndex(z => z.Id == dm.Id);
                        if (j != -1)
                            k = DynamicsListRu[j].Name;
                    }
                    else if (Language == 2)
                    {
                        int j = DynamicsListEn.FindIndex(z => z.Id == dm.Id);
                        if (j != -1)
                            k = DynamicsListEn[j].Name;
                    }
                    DynamicGrid.Rows.Add(Read.DynobjectAmount, dm.Id, k, dm.TriggerId);
                    DynamicGrid.CurrentCell = DynamicGrid.Rows[DynamicGrid.Rows.Count - 1].Cells[1];
                }
            }
        }
        public ClassPosition GetCoordinates()
        {
            Process[] pname = Process.GetProcessesByName("elementclient");
            if (pname.Length == 0)
            {
                if (Language == 1)
                    MessageBox.Show("Клиент игры не запущен!!...", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (Language == 2)
                    MessageBox.Show("Game isn't running!!...", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
            else
            {
                int[] Adresses = new int[3];
                int DX = 0;
                int DY = 0;
                int DZ = 0;
                int PX = 0;
                int PY = 0;
                int PZ = 0;
                Process[] Procceses = Process.GetProcessesByName("elementclient");
                IntPtr Process_name = (IntPtr)0;
                if (Procceses.Length > 0)
                {
                    Process_name = Procceses[0].Handle;
                }
                byte[] size = new byte[4];
                int BaseOffset = 0;
                switch (Version_combobox.SelectedIndex)
                {
                    case 0:
                        {
                            Adresses[0] = 0x00C0CDEC;
                            Adresses[1] = 0x00000034;
                            Adresses[2] = 0x0000131C;
                            DX = 0x0000015C;
                            DY = 0x00000160;
                            DZ = 0x00000164;
                            PX = 0x0000017C;
                            PY = 0x00000180;
                            PZ = 0x00000184;
                            break;
                        }
                    case 1:
                        {
                            Adresses[0] = 0x00B280C4;
                            Adresses[1] = 0x00000034;
                            Adresses[2] = 0x000010E8;
                            DX = 0x0000015C;
                            DY = 0x00000160;
                            DZ = 0x00000164;
                            PX = 0x0000017C;
                            PY = 0x00000180;
                            PZ = 0x00000184;
                            break;
                        }
                    case 2:
                        {
                            Adresses[0] = 0x00B9029C;
                            Adresses[1] = 0x00000034;
                            Adresses[2] = 0x000011D8;
                            DX = 0x0000015C;
                            DY = 0x00000160;
                            DZ = 0x00000164;
                            PX = 0x0000017C;
                            PY = 0x00000180;
                            PZ = 0x00000184;
                            break;
                        }
                    case 3:
                        {
                            Adresses[0] = 0x00C0CDEC;
                            Adresses[1] = 0x00000034;
                            Adresses[2] = 0x0000131C;
                            DX = 0x00000160;
                            DY = 0x00000164;
                            DZ = 0x00000168;
                            PX = 0x00000180;
                            PY = 0x00000184;
                            PZ = 0x00000188;
                            break;
                        }
                    case 4:
                        {
                            Adresses[0] = 0x00C392CC;
                            Adresses[1] = 0x00000034;
                            Adresses[2] = 0x00001340;
                            DX = 0x0000015C;
                            DY = 0x00000160;
                            DZ = 0x00000164;
                            PX = 0x00000180;
                            PY = 0x00000184;
                            PZ = 0x00000188;
                            break;
                        }
                    case 5:
                        {

                            Adresses[0] = 0x00C76DCC;
                            Adresses[1] = 0x0000002C;
                            Adresses[2] = 0x00001420;
                            DX = 0x0000015C;
                            DY = 0x00000160;
                            DZ = 0x00000164;
                            PX = 0x00000180;
                            PY = 0x00000184;
                            PZ = 0x00000188;
                            break;
                        }
                    case 6:
                        {
                            Adresses[0] = 0x00C7D20C;
                            Adresses[1] = 0x0000002C;
                            Adresses[2] = 0x00001420;
                            DX = 00000160;
                            DY = 00000164;
                            DZ = 00000168;
                            PX = 0x00000180;
                            PY = 0x00000184;
                            PZ = 0x00000188;
                            break;
                        }
                    case 7:
                        {
                            Adresses[0] = 0x00DA433C;
                            Adresses[1] = 0x0000001C;
                            Adresses[2] = 0x00000028;
                            DX = 0x0000006C;
                            DY = 0x00000070;
                            DZ = 0x00000074;
                            PX = 0x0000003C;
                            PY = 0x00000040;
                            PZ = 0x00000044;
                            break;
                        }
                }
                for (int h = 0; h < Adresses.Length; h++)
                {
                    ReadProcessMemory((IntPtr)Process_name, ((IntPtr)(BaseOffset + Adresses[h])), size, 4, 0);
                    BaseOffset = BitConverter.ToInt32(size, 0);
                }
                ReadProcessMemory((IntPtr)Process_name, ((IntPtr)(BaseOffset + PX)), size, 4, 0);
                float PosX = BitConverter.ToSingle(size, 0);
                ReadProcessMemory((IntPtr)Process_name, ((IntPtr)(BaseOffset + PY)), size, 4, 0);
                float PosY = BitConverter.ToSingle(size, 0);
                ReadProcessMemory((IntPtr)Process_name, ((IntPtr)(BaseOffset + PZ)), size, 4, 0);
                float PosZ = BitConverter.ToSingle(size, 0);
                ReadProcessMemory((IntPtr)Process_name, ((IntPtr)(BaseOffset + DX)), size, 4, 0);
                float DirX = BitConverter.ToSingle(size, 0);
                ReadProcessMemory((IntPtr)Process_name, ((IntPtr)(BaseOffset + DY)), size, 4, 0);
                float DirY = BitConverter.ToSingle(size, 0);
                ReadProcessMemory((IntPtr)Process_name, ((IntPtr)(BaseOffset + DZ)), size, 4, 0);
                float DirZ = BitConverter.ToSingle(size, 0);
                return new ClassPosition(PosX, PosY, PosZ, DirX, DirY, DirZ);
            }
        }
        List<PointF> GetPoint(int Action)
        {
            List<PointF> ls = new List<PointF>();
            if (Action == 1)
            {
                foreach (var item in NpcRowCollection)
                {
                    ls.Add(new PointF(Read.NpcMobList[item].X_position, Read.NpcMobList[item].Z_position));
                }
            }
            else if (Action == 2)
            {
                foreach (var item in ResourcesRowCollection)
                {
                    ls.Add(new PointF(Read.ResourcesList[item].X_position, Read.ResourcesList[item].Z_position));
                }
            }
            else if (Action == 3)
            {
                foreach (var item in DynamicsRowCollection)
                {
                    ls.Add(new PointF(Read.DynamicsList[item].X_position, Read.DynamicsList[item].Z_position));
                }
            }
            return ls;
        }
        void LinkMaps(List<FileTable> l, string MapName)
        {
            int val = 256;
            Bitmap bm = null;
            int MapIndex = LoadedMapConfigs.FindIndex(z => z.Name == MapName);
            if (MapIndex != -1)
            {
                if (l.Count == 88)
                    val = 1024;
                bm = new Bitmap(LoadedMapConfigs[MapIndex].Width, LoadedMapConfigs[MapIndex].Height);
                int x = 0;
                int y = 0;
                Graphics gr = Graphics.FromImage(bm);
                MapProgress.BeginInvoke(new MethodInvoker(delegate
                {
                    MapProgress.Maximum = l.Count;
                    MapProgress.Value = 0;
                }));
                l=l.OrderBy(t => t.filePath).ToList();
                for (int i = 0; i < l.Count; i++)
                {
                    gr.DrawImage( LBLIBRARY.LBLIBRARY.LoadDDSImage(PckFile.readFile(l[i]).ToArray()), new Point(x, y));
                    x += val;
                    if (x == bm.Width)
                    {
                        y += val;
                        x = 0;
                    }
                    MapProgress.BeginInvoke(new MethodInvoker(delegate
                    {
                        MapProgress.Value++;
                    }));
                }
                this.BeginInvoke(new MethodInvoker(delegate
                {
                    MapProgress.Value = 0;
                    if (MapForm == null)
                    {
                        MapForm = new ShowLocationWindow(this, bm);
                        MapForm.Show(this);
                    }
                    else if (MapForm.Visible == false)
                    {
                        MapForm = new ShowLocationWindow(this, bm);
                        MapForm.Show(this);
                    }
                    else
                    {
                        MapForm.SetPicture(bm);
                    }
                }));
            }
            else
            {
                if (Language == 1)
                    MessageBox.Show("Настройки карты не найдены в Maps.conf", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Map options haven't been found in Maps.conf", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void SetId(int Id, int act,int Window)
        {
            if (act == 1 && Window ==1)
            {
                Id_numeric.Value = Id;
                UnderNpcAndMobs_Leave(Id_numeric, null);
            }
            else if (act == 1 && Window == 2)
            {
                ExistenceSearchId.Text = Id.ToString();
                ExistenceSearchId_TextChanged(null, null);
            }
            else if (act == 2&&Window==1)
            {
                RId_numeric.Value = Id;
                UnderResourcesLeave(RId_numeric, null);
            }
            else if (act==2&&Window ==2)
            {
                ResourceSearchId.Text = Id.ToString();
                ResourceSearchId_TextChanged(null, null);
            }
            else if (act == 3&& Window ==1)
            {
                DId_numeric.Text = Id.ToString();
                DynamicsLeave(DId_numeric, null);
            }
            else if (act==3 && Window==2)
            {
                DynamicSearchId.Text = Id.ToString();
                DynamicSearchId_TextChanged(null, null);
            }

        }
        private void DefaultMobButton_combobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Control co = sender as Control;
            KeysConverter ks = new KeysConverter();
            switch (co.Name)
            {
                case "DefaultMobButton_combobox":
                    {
                        MonstersDefaultKey = (Keys)ks.ConvertFromString(DefaultMobButton_combobox.SelectedItem.ToString());
                        break;
                    }
                case "ExtraMobButton_combobox":
                    {
                        MonstersExtraKey = (Keys)ks.ConvertFromString(ExtraMobButton_combobox.SelectedItem.ToString());
                        break;
                    }
                case "DefaultResourceButton_combobox":
                    {
                        ResourcesDefaultKey = (Keys)ks.ConvertFromString(DefaultResourceButton_combobox.SelectedItem.ToString());
                        break;
                    }
                case "ExtraResourceButton_combobox":
                    {
                        ResourcesExtraKey = (Keys)ks.ConvertFromString(ExtraResourceButton_combobox.SelectedItem.ToString());
                        break;
                    }
                case "DefaultDynamicsButton_combobox":
                    {
                        DynamicsDefaultKey = (Keys)ks.ConvertFromString(DefaultDynamicsButton_combobox.SelectedItem.ToString());
                        break;
                    }
                case "ExtraDynamicsButton_combobox":
                    {
                        DynamicsExtraKey = (Keys)ks.ConvertFromString(ExtraDynamicsButton_combobox.SelectedItem.ToString());
                        break;
                    }
            }
        }
        #endregion
        #region Default
        private void SearchElementButton(object sender, EventArgs e)
        {
            if (Element_dialog.ShowDialog() == DialogResult.OK)
            {
                Elements_textbox.Text = Element_dialog.FileName;
            }
        }
        private void SearchNpcgenButton(object sender, EventArgs e)
        {
            if (Npcgen_dialog.ShowDialog() == DialogResult.OK)
                Npcgen_textbox.Text = Npcgen_dialog.FileName;
        }
        private void SearchSurfacesButton(object sender, EventArgs e)
        {
            if (Surfaces_search.ShowDialog() == DialogResult.OK)
                Surfaces_path.Text = Surfaces_search.FileName;
        }
        private void OpenElementAndNpcgen(object sender, EventArgs e)
        {
            if (File.Exists(Npcgen_textbox.Text) && File.Exists(Elements_textbox.Text))
            {
                Read = new NpcGen();
                BinaryReader br = new BinaryReader(File.Open(Npcgen_textbox.Text, FileMode.Open));
                Read.ReadNpcgen(br);
                br.Close();
                Element = new Elementsdata(new BinaryReader(File.Open(Elements_textbox.Text, FileMode.Open)));
                if (Element.Version == -10)
                {
                    if (Language == 1)
                        MessageBox.Show("Версия elements.data не поддерживается!!...", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if (Language == 2)
                        MessageBox.Show("Unsupported elements.data version!!...", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                this.Text = Npcgen_textbox.Text + "  -  Version " + Read.File_version.ToString() + "  -  " + "Npcgen Editor By Luka v1.5";
                NpcMobsGrid.ScrollBars = ScrollBars.None;
                ResourcesGrid.ScrollBars = ScrollBars.None;
                DynamicGrid.ScrollBars = ScrollBars.None;
                TriggersGrid.ScrollBars = ScrollBars.None;
                new Thread(delegate ()
                {
                    ChooseFromElementsForm = new MobsNpcsForm(this, Element.ExistenceLists, Element.ResourcesList, Element.MonsterdAmount, Element.NpcsAmount);
                    ChooseFromElementsForm.RefreshLanguage(Language);
                }).Start();
                NpcMobsGrid.Rows.Clear();
                ResourcesGrid.Rows.Clear();
                DynamicGrid.Rows.Clear();
                TriggersGrid.Rows.Clear();
                ErrorsGrid.Rows.Clear();
                MainProgressBar.Maximum = Read.NpcMobsAmount + Read.ResourcesAmount + Read.DynobjectAmount + Read.TriggersAmount;
                SortNpcGen();
                SortDynamicObjects();
                SortTriggers();
                NpcMobsGrid.ScrollBars = ScrollBars.Vertical;
                ResourcesGrid.ScrollBars = ScrollBars.Vertical;
                DynamicGrid.ScrollBars = ScrollBars.Vertical;
                TriggersGrid.ScrollBars = ScrollBars.Vertical;
                if (Language == 1)
                {
                    ExistenceTab.Text = string.Format("Мобы и Нипы" + " 1/{0}", Read.NpcMobsAmount);
                    ResourcesTab.Text = string.Format("Ресурсы" + " 1/{0}", Read.ResourcesAmount);
                    DynObjectsTab.Text = string.Format("Динамические Объекты" + " 1/{0}", Read.DynobjectAmount);
                    TriggersTab.Text = string.Format("Тригеры" + " 1/{0}", Read.TriggersAmount);
                }
                else
                {
                    ExistenceTab.Text = string.Format("Mobs and Npcs" + " 1/{0}", Read.NpcMobsAmount);
                    ResourcesTab.Text = string.Format("Resources" + " 1/{0}", Read.ResourcesAmount);
                    DynObjectsTab.Text = string.Format("Dynamic Objects" + " 1/{0}", Read.DynobjectAmount);
                    TriggersTab.Text = string.Format("Triggers" + " 1/{0}", Read.TriggersAmount);
                }
                if (Read.File_version <= 6)
                {
                    if (Language == 1)
                        MessageBox.Show("Обратите внимание,в этой версии триггеры не были доступны,но их можно редактировать для конвертирования в другую версию!!...", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if (Language == 2)
                        MessageBox.Show("Make attention,triggers didn't exist in this file version,but you can edit them for converting to another version!!...", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                string CheckOnErrors = "Проверить файл на ошибки?";
                if (Language == 2)
                {
                    CheckOnErrors = "Do you want to check file on errors?";
                }
                DialogResult dg = MessageBox.Show(CheckOnErrors, "Npcgen Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dg == DialogResult.Yes)
                {
                    SearchErrorsButton_Click(null, null);
                    MainTabControl.SelectedIndex = 5;
                }

            }
            else
            {
                if (Language == 1)
                    MessageBox.Show("Файл не существует!!...", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (Language == 2)
                    MessageBox.Show("File doesn't exist!!...", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void SortTriggers()
        {
            #region Triggers
            for (int i = 0; i < Read.TriggersAmount; i++)
            {
                TriggersGrid.Rows.Add(i + 1, Read.TriggersList[i].Id, Read.TriggersList[i].GmID, Read.TriggersList[i].TriggerName);
            }
            if (TriggersGrid.Rows.Count != 0)
                TriggersGrid.CurrentCell = TriggersGrid.Rows[0].Cells[1];
            TriggersGrid_CurrentCellChanged(null, null);
            #endregion
        }
        public void SortNpcGen()
        {
            #region NpcsAndMobs
            for (int i = 0; i < Read.NpcMobsAmount; i++)
            {
                int[] Id_joined = new int[Read.NpcMobList[i].Amount_in_group];
                string[] Names_joined = new string[Read.NpcMobList[i].Amount_in_group];
                for (int j = 0; j < Read.NpcMobList[i].Amount_in_group; j++)
                {
                    Id_joined[j] = Read.NpcMobList[i].MobDops[j].Id;
                    int ind = Element.ExistenceLists.FindIndex(e => e.Id == Id_joined[j]);
                    if (ind != -1)
                    {
                        Names_joined[j] = Element.ExistenceLists[ind].Name;
                    }
                    else
                    {
                        Names_joined[j] = "?";
                    }
                }
                NpcMobsGrid.Rows.Add(i + 1, string.Join(",", Id_joined), string.Join(",", Names_joined));
                if (Read.NpcMobList[i].Type == 1)
                {
                    NpcMobsGrid.Rows[i].Cells[1].Style.ForeColor = Color.FromArgb(251, 251, 107);
                    NpcMobsGrid.Rows[i].Cells[2].Style.ForeColor = Color.FromArgb(251, 251, 107);
                }
                MainProgressBar.Value++;
            }
            ExistenceGrid_CellChanged(null, null);
            #endregion
            #region Resources
            for (int i = 0; i < Read.ResourcesAmount; i++)
            {
                int[] Id_joined = new int[Read.ResourcesList[i].Amount_in_group];
                string[] Names_joined = new string[Read.ResourcesList[i].Amount_in_group];
                for (int j = 0; j < Read.ResourcesList[i].Amount_in_group; j++)
                {
                    Id_joined[j] = Read.ResourcesList[i].ResExtra[j].Id;
                    int ind = Element.ResourcesList.FindIndex(e => e.Id == Id_joined[j]);
                    if (ind != -1)
                    {
                        Names_joined[j] = Element.ResourcesList[ind].Name;
                    }
                    else
                    {
                        Names_joined[j] = "?";
                    }
                }
                ResourcesGrid.Rows.Add(i + 1, string.Join(",", Id_joined), string.Join(",", Names_joined));
                MainProgressBar.Value++;
            }
            MainProgressBar.Value = 0;
            if (ResourcesGrid.Rows.Count != 0)
                ResourcesGrid.CurrentCell = ResourcesGrid.Rows[0].Cells[1];
            if (ResourcesGroupGrid.Rows.Count != 0)
                ResourcesGroupGrid.CurrentCell = ResourcesGroupGrid.Rows[0].Cells[1];
            #endregion
        }
        public void SortDynamicObjects()
        {
            for (int i = 0; i < Read.DynobjectAmount; i++)
            {
                DynamicGrid.Rows.Add(i + 1, Read.DynamicsList[i].Id, GetDynamicName(Read.DynamicsList[i].Id), Read.DynamicsList[i].TriggerId);
            }
            if (DynamicGrid.Rows.Count != 0)
            {
                DynamicGrid.CurrentCell = DynamicGrid.Rows[0].Cells[1];
                DynamicGrid_CurrentCellChanged(null, null);
            }
        }
        public string GetDynamicName(int Id)
        {
            string k = "?";
            if(Language==1)
            {
                int Ind = DynamicsListRu.FindIndex(z => z.Id == Id);
                if(Ind !=-1)
                {
                    k = DynamicsListRu[Ind].Name;
                }
            }
            else if(Language==2)
            {
                int Ind = DynamicsListEn.FindIndex(z => z.Id == Id);
                if (Ind != -1)
                {
                    k = DynamicsListEn[Ind].Name;
                }
            }
            return k;
        }
        private void Open_surfaces_Click(object sender, EventArgs e)
        {
            if (File.Exists(Surfaces_path.Text))
            {
                try
                {
                    PckFile = new Pck_engine(Surfaces_path.Text);
                    PckFile.readTable();
                    Maps = new List<GameMapInfo>();
                    List<FileTable> AllFiles = PckFile.File_table.Where(z => z.filePath.Contains("minimaps") && !z.filePath.Contains("surfaces\\minimaps\\world")).ToList();
                    List<string> AlreadyExistion = new List<string>();
                    foreach (FileTable fs in AllFiles)
                    {
                        GameMapInfo map = new GameMapInfo();
                        List<string> st = fs.filePath.Split('\\').ToList();
                        map.MapName = st.ElementAt(st.Count - 2);
                        st.RemoveAt(st.Count - 1);
                        map.MapPath = string.Join("\\", st);
                        if (AlreadyExistion.FindIndex(v => v == string.Join("\\", st)) == -1)
                        {
                            map.MapFragments = AllFiles.Where(z => z.filePath.Contains(map.MapPath)).ToList().OrderBy(z => z.filePath).ToList();
                            Maps.Add(map);
                            AlreadyExistion.Add(map.MapPath);
                        }
                    }
                    if (Maps.Count != 0)
                    {
                        GameMapInfo map1 = new GameMapInfo()
                        {
                            MapName = "World",
                            MapPath = "surfaces\\maps\\"
                        };
                        map1.MapFragments = PckFile.File_table.Where(z => z.filePath.Contains(map1.MapPath)).ToList();
                        Maps.Add(map1);
                        Maps_combobox.Items.Clear();
                        for (int i = 0; i < Maps.Count; i++)
                        {
                            Maps_combobox.Items.Add(Maps[i].MapName);
                        }
                        Maps_combobox.SelectedIndex = Maps_combobox.Items.Count - 1;
                    }
                }
                catch
                {
                    if (Language == 1)
                    {
                        MessageBox.Show("Загружен неверный файл!!...", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (Language == 2)
                    {
                        MessageBox.Show("Loaded wrong file!!...", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    PckFile = null;
                    Maps_combobox.Items.Clear();
                    Maps = null;
                    MapForm = null;
                }
            }
            else
            {
                if (Language == 1)
                {
                    MessageBox.Show("Указанный surfaces.pck не существует!!...", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (Language == 2)
                {
                    MessageBox.Show("Selected surfaces.pck doesn't exist!!...", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void SaveFile_Click(object sender, EventArgs e)
        {
            if (File.Exists(Npcgen_textbox.Text))
            {
                BinaryWriter bw = new BinaryWriter(File.Create(Npcgen_textbox.Text));
                Read.WriteNpcgen(bw, Read.File_version);
                bw.Close();
                if (Language == 1)
                    MessageBox.Show("Файл успешно сохранен!!...", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (Language == 2)
                    MessageBox.Show("File has been successfully saved!!...", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (Language == 1)
                    MessageBox.Show("Файл не существует!!...", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (Language == 2)
                    MessageBox.Show("File doesn't exist!!...", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void ConvertAndSaveButton_Click(object sender, EventArgs e)
        {
            if (Read != null)
            {
                if (Npcgen_save_dialog.ShowDialog() == DialogResult.OK)
                {
                    BinaryWriter bw = new BinaryWriter(File.Create(Npcgen_save_dialog.FileName));
                    Read.WriteNpcgen(bw, Convert.ToInt32(ConvertComboboxVersion.SelectedItem));
                    bw.Close();
                    if (Language == 1)
                        MessageBox.Show("Файл успешно сохранен!!", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if (Language == 2)
                        MessageBox.Show("File has been successfully saved!!", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void ShowOnMapsButton_Click(object sender, EventArgs e)
        {
            if (Maps != null)
            {
                int SelectedIndex = Maps_combobox.SelectedIndex;
                System.Threading.Thread th = new System.Threading.Thread(() => LinkMaps(Maps[SelectedIndex].MapFragments, Maps[SelectedIndex].MapName));
                th.Start();
            }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            XmlTextWriter vt = new XmlTextWriter(string.Format(Application.StartupPath + "\\Npcgen Editor Settings.xml"), Encoding.UTF8)
            {
                Formatting = Formatting.Indented,
                IndentChar = '\t',
                Indentation = 1,
                QuoteChar = '\''

            };
            #region Default
            vt.WriteStartDocument();
            vt.WriteStartElement("root");
            vt.WriteStartAttribute("ProductName");
            vt.WriteString("NpcGen Editor By Luka");
            vt.WriteEndAttribute();
            vt.WriteStartElement("Settings");
            vt.WriteElementString("Version", "1.5");
            vt.WriteElementString("Language", Language.ToString());
            vt.WriteElementString("Interface_Color", InterfaceColor.ToString());
            #endregion
            #region Element
            if (!string.IsNullOrWhiteSpace(Elements_textbox.Text))
                vt.WriteElementString("ElementPath", Elements_textbox.Text);
            else vt.WriteElementString("ElementPath", "Not Loaded");
            #endregion
            #region Npcgen
            if (!string.IsNullOrWhiteSpace(Npcgen_textbox.Text))
                vt.WriteElementString("NpcgenPath", Npcgen_textbox.Text);
            else vt.WriteElementString("NpcgenPath", "Not Loaded");
            #endregion
            #region Surfaces
            if (!string.IsNullOrWhiteSpace(Surfaces_path.Text))
                vt.WriteElementString("SurfacesPath", Surfaces_path.Text);
            else vt.WriteElementString("SurfacesPath", "Not Loaded");
            #endregion
            #region GlobalHook
            if (DefaultMobButton_combobox.SelectedIndex == -1)
                DefaultMobButton_combobox.SelectedIndex = 0;
            if (ExtraMobButton_combobox.SelectedIndex == -1)
                ExtraMobButton_combobox.SelectedIndex = 0;
            if (DefaultResourceButton_combobox.SelectedIndex == -1)
                DefaultResourceButton_combobox.SelectedIndex = 0;
            if (ExtraResourceButton_combobox.SelectedIndex == -1)
                ExtraResourceButton_combobox.SelectedIndex = 0;
            if (DefaultDynamicsButton_combobox.SelectedIndex == -1)
                DefaultDynamicsButton_combobox.SelectedIndex = 0;
            if (ExtraDynamicsButton_combobox.SelectedIndex == -1)
                ExtraDynamicsButton_combobox.SelectedIndex = 0;
            vt.WriteElementString("ClientVersion", Version_combobox.SelectedIndex.ToString());
            vt.WriteElementString("MobsOrNpcsHotKey", string.Format("{0}+{1}", DefaultMobButton_combobox.SelectedIndex, ExtraMobButton_combobox.SelectedIndex));
            vt.WriteElementString("ResourcesHotKey", string.Format("{0}+{1}", DefaultResourceButton_combobox.SelectedIndex, ExtraResourceButton_combobox.SelectedIndex));
            vt.WriteElementString("DynamicsHotKey", string.Format("{0}+{1}", DefaultDynamicsButton_combobox.SelectedIndex, ExtraDynamicsButton_combobox.SelectedIndex));
            vt.WriteEndElement();
            #endregion
            #region WriteMonsterProperties
            vt.WriteStartElement("Existence_properties");
            vt.WriteElementString("Existence_ID", AddMonsterId.Value.ToString());
            vt.WriteElementString("Existence_Amount", AddMonsterAmount.Value.ToString());
            vt.WriteElementString("Existence_RespawnTime", AddMonsterRespawnTime.Value.ToString());
            vt.WriteElementString("Existence_Trigger", AddMonsterTrigger.Value.ToString());
            vt.WriteElementString("Existence_Location", AddintExistenceType.SelectedIndex.ToString());
            vt.WriteElementString("Existence_Type", AddMonsterType.SelectedIndex.ToString());
            vt.WriteEndElement();
            #endregion
            #region WriteResourcesProperties
            vt.WriteStartElement("Resources_properties");
            vt.WriteElementString("Resources_ID", AddResourceID.Value.ToString());
            vt.WriteElementString("Resources_Amount", AddResourceAmount.Value.ToString());
            vt.WriteElementString("Resources_RespawnTime", AddResourceRespawnTime.Value.ToString());
            vt.WriteElementString("Resources_Trigger", AddResourcesTrigger.Value.ToString());
            vt.WriteEndElement();
            #endregion
            #region WriteDynamicsProperties
            vt.WriteStartElement("Dynamics_properties");
            vt.WriteElementString("Dynamics_Trigger", AddDynamicsID.Value.ToString());
            vt.WriteElementString("Dynamics_ID", AddDynamicsTrigger.Value.ToString());
            vt.WriteEndElement();
            vt.WriteEndDocument();
            vt.Close();
            #endregion
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            #region ReadDynamics
            DynamicsListRu = new List<DefaultInformation>();
            DynamicsListEn = new List<DefaultInformation>();
            try
            {
                if (File.Exists(Application.StartupPath + "\\DynObjectInfo.RU"))
                {
                    StreamReader sr = new StreamReader(Application.StartupPath + "\\DynObjectInfo.RU");
                    while (true)
                    {
                        var k = sr.ReadLine().Split(new string[] { "->" }, StringSplitOptions.None);
                        DefaultInformation di = new DefaultInformation()
                        {
                            Id = Convert.ToInt32(k[0]),
                            Name = k[1]
                        };
                        DynamicsListRu.Add(di);
                        if (sr.EndOfStream == true)
                            break;
                    }
                }
                if (File.Exists(Application.StartupPath + "\\DynObjectInfo.EN"))
                {
                    StreamReader sr = new StreamReader(Application.StartupPath + "\\DynObjectInfo.EN");
                    while (true)
                    {
                        var k = sr.ReadLine().Split(new string[] { "->" }, StringSplitOptions.None);
                        DefaultInformation di = new DefaultInformation()
                        {
                            Id = Convert.ToInt32(k[0]),
                            Name = k[1]
                        };
                        DynamicsListEn.Add(di);
                        if (sr.EndOfStream == true)
                            break;
                    }
                }

            }
            catch
            {

            }
            #endregion
            ConvertComboboxVersion.SelectedIndex = 0;
            #region ReadXml
            try
            {
                if (File.Exists(Application.StartupPath + "\\Npcgen Editor Settings.xml"))
                {
                    using (System.Xml.XmlTextReader rg = new XmlTextReader(string.Format(Application.StartupPath + "\\Npcgen Editor Settings.xml")))
                    {
                        #region FilePathes
                        rg.ReadToFollowing("Language");
                        int Lang = Convert.ToInt32(rg.ReadElementContentAsString());
                        if (Lang == 2)
                        {
                            English.Checked = true;
                            ChangeLanguage(English, null);
                        }
                        rg.ReadToFollowing("Interface_Color");
                        int inter = Convert.ToInt32(rg.ReadElementContentAsString());
                        if(inter == 2)
                        {
                            Dark.Checked = true;
                            InterfaceColorChanged(Dark,null);
                        }
                        rg.ReadToFollowing("ElementPath");
                        Elements_textbox.Text = rg.ReadElementContentAsString();
                        rg.ReadToFollowing("NpcgenPath");
                        Npcgen_textbox.Text = rg.ReadElementContentAsString();
                        rg.ReadToFollowing("SurfacesPath");
                        Surfaces_path.Text = rg.ReadElementContentAsString();
                        #endregion
                        #region GlobalHook
                        rg.ReadToFollowing("ClientVersion");
                        Version_combobox.SelectedIndex = Convert.ToInt32(rg.ReadElementContentAsString());
                        rg.ReadToFollowing("MobsOrNpcsHotKey");
                        string[] MobsHotKey = rg.ReadElementContentAsString().Split('+');
                        DefaultMobButton_combobox.SelectedIndex = Convert.ToInt32(MobsHotKey[0]);
                        ExtraMobButton_combobox.SelectedIndex = Convert.ToInt32(MobsHotKey[1]);
                        rg.ReadToFollowing("ResourcesHotKey");
                        string[] ResourcesHotKey = rg.ReadElementContentAsString().Split('+');
                        DefaultResourceButton_combobox.SelectedIndex = Convert.ToInt32(ResourcesHotKey[0]);
                        ExtraResourceButton_combobox.SelectedIndex = Convert.ToInt32(ResourcesHotKey[1]);
                        rg.ReadToFollowing("DynamicsHotKey");
                        string[] DynamicsHotKey = rg.ReadElementContentAsString().Split('+');
                        DefaultDynamicsButton_combobox.SelectedIndex = Convert.ToInt32(DynamicsHotKey[0]);
                        ExtraDynamicsButton_combobox.SelectedIndex = Convert.ToInt32(DynamicsHotKey[1]);
                        DefaultMobButton_combobox_SelectedIndexChanged(DefaultMobButton_combobox, null);
                        DefaultMobButton_combobox_SelectedIndexChanged(ExtraMobButton_combobox, null);
                        DefaultMobButton_combobox_SelectedIndexChanged(DefaultResourceButton_combobox, null);
                        DefaultMobButton_combobox_SelectedIndexChanged(ExtraResourceButton_combobox, null);
                        DefaultMobButton_combobox_SelectedIndexChanged(DefaultDynamicsButton_combobox, null);
                        DefaultMobButton_combobox_SelectedIndexChanged(ExtraDynamicsButton_combobox, null);
                        #endregion
                        #region MonsterOptions
                        rg.ReadToFollowing("Existence_ID");
                        AddMonsterId.Value = Convert.ToDecimal(rg.ReadElementContentAsString());
                        rg.ReadToFollowing("Existence_Amount");
                        AddMonsterAmount.Value = Convert.ToDecimal(rg.ReadElementContentAsString());
                        rg.ReadToFollowing("Existence_RespawnTime");
                        AddMonsterRespawnTime.Value = Convert.ToDecimal(rg.ReadElementContentAsString());
                        rg.ReadToFollowing("Existence_Trigger");
                        AddMonsterTrigger.Value = Convert.ToDecimal(rg.ReadElementContentAsString());
                        rg.ReadToFollowing("Existence_Location");
                        int MonsterLocationIndex = Convert.ToInt32(rg.ReadElementContentAsString());
                        if (MonsterLocationIndex < 0)
                            MonsterLocationIndex = 0;
                        AddintExistenceType.SelectedIndex = MonsterLocationIndex;
                        rg.ReadToFollowing("Existence_Type");
                        int ExistenceType = Convert.ToInt32(rg.ReadElementContentAsString());
                        if (ExistenceType < 0)
                            ExistenceType = 0;
                        AddMonsterType.SelectedIndex = ExistenceType;
                        #endregion
                        #region ResourcesOptions
                        rg.ReadToFollowing("Resources_ID");
                        AddResourceID.Value = Convert.ToDecimal(rg.ReadElementContentAsString());
                        rg.ReadToFollowing("Resources_Amount");
                        AddResourceAmount.Value = Convert.ToDecimal(rg.ReadElementContentAsString());
                        rg.ReadToFollowing("Resources_RespawnTime");
                        AddResourceRespawnTime.Value = Convert.ToDecimal(rg.ReadElementContentAsString());
                        rg.ReadToFollowing("Resources_Trigger");
                        AddResourcesTrigger.Value = Convert.ToDecimal(rg.ReadElementContentAsString());
                        #endregion
                        #region DynamicsOptions
                        rg.ReadToFollowing("Dynamics_Trigger");
                        AddDynamicsID.Value = Convert.ToDecimal(rg.ReadElementContentAsString());
                        rg.ReadToFollowing("Dynamics_ID");
                        AddDynamicsTrigger.Value = Convert.ToDecimal(rg.ReadElementContentAsString());
                        #endregion
                    }
                }
                else
                {
                    AddintExistenceType.SelectedIndex = 0;
                    AddMonsterType.SelectedIndex = 0;
                    Version_combobox.SelectedIndex = 0;
                    DefaultMobButton_combobox.SelectedIndex = 0;
                    ExtraMobButton_combobox.SelectedIndex = 1;
                    DefaultResourceButton_combobox.SelectedIndex = 0;
                    ExtraResourceButton_combobox.SelectedIndex = 2;
                    DefaultDynamicsButton_combobox.SelectedIndex = 0;
                    ExtraDynamicsButton_combobox.SelectedIndex = 3;
                    DefaultMobButton_combobox_SelectedIndexChanged(DefaultMobButton_combobox, null);
                    DefaultMobButton_combobox_SelectedIndexChanged(ExtraMobButton_combobox, null);
                    DefaultMobButton_combobox_SelectedIndexChanged(DefaultResourceButton_combobox, null);
                    DefaultMobButton_combobox_SelectedIndexChanged(ExtraResourceButton_combobox, null);
                    DefaultMobButton_combobox_SelectedIndexChanged(DefaultDynamicsButton_combobox, null);
                    DefaultMobButton_combobox_SelectedIndexChanged(ExtraDynamicsButton_combobox, null);
                }
            }
            catch
            {
                if (Language == 1)
                    MessageBox.Show("Ошибка при загрузке настроек", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Error when reading options", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            #endregion
            #region ReadMapsConfigs
            LoadedMapConfigs = new List<MapLoadedInformation>();
            if (File.Exists(Application.StartupPath + "\\Maps.conf"))
            {
                StreamReader sr = new StreamReader(Application.StartupPath + "\\Maps.conf");
                while (true)
                {
                    var k = sr.ReadLine().Split(new string[] { "->" }, StringSplitOptions.None);
                    MapLoadedInformation di = new MapLoadedInformation()
                    {
                        Name = k[0]
                    };
                    string[] Sizes = k[1].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    int.TryParse(Sizes[0], out di.Width);
                    int.TryParse(Sizes[1], out di.Height);
                    LoadedMapConfigs.Add(di);
                    if (sr.EndOfStream == true)
                        break;
                }
            }
            #endregion
        }
        private void InformationButton_Click(object sender, EventArgs e)
        {
            Process.Start("https://kn1fe-zone.ru/index.php?members/luka.1491/");
            MessageBox.Show("Perfect world: Npcgen.data Editor \rVersion: v1.5\rSkype:Luka007789\r                                         05.04.2017\r                                              © Luka", "Npcgen Editor By Luka", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void UpObjects(object sender, EventArgs e)
        {
            if (Read != null)
            {
                DataGridView Grid = null;
                int GridIndex = MainTabControl.SelectedIndex;
                if (MainTabControl.SelectedIndex == 0)
                    Grid = NpcMobsGrid;
                else if (MainTabControl.SelectedIndex == 1)
                    Grid = ResourcesGrid;
                else if (MainTabControl.SelectedIndex == 2)
                    Grid = DynamicGrid;
                else if (MainTabControl.SelectedIndex == 3)
                    Grid = TriggersGrid;
                var dgs = Grid.SelectedRows.Cast<DataGridViewRow>().OrderBy(i => i.Index).ToList();
                var colle = Grid.SelectedRows.Cast<DataGridViewRow>().OrderBy(i => i.Index).ToList();
                if (dgs.Count != 0)
                {
                    if (dgs[0].Index != 0)
                    {
                        AllowCellChanging = false;
                        foreach (DataGridViewRow Row in dgs)
                        {
                            int Current = Row.Index;
                            int Previous = Row.Index - 1;
                            Grid.Rows.Remove(Row);
                            Grid.Rows.Insert(Previous, Row);
                            if (GridIndex == 0)
                            {
                                ClassDefaultMonsters np = Read.NpcMobList[Current];
                                Read.NpcMobList.RemoveAt(Current);
                                Read.NpcMobList.Insert(Previous, np);
                            }
                            else if (GridIndex == 1)
                            {
                                ClassDefaultResources np = Read.ResourcesList[Current];
                                Read.ResourcesList.RemoveAt(Current);
                                Read.ResourcesList.Insert(Previous, np);
                            }
                            else if (GridIndex == 2)
                            {
                                ClassDynamicObject np = Read.DynamicsList[Current];
                                Read.DynamicsList.RemoveAt(Current);
                                Read.DynamicsList.Insert(Previous, np);
                            }
                            else if (GridIndex == 3)
                            {
                                ClassTrigger np = Read.TriggersList[Current];
                                Read.TriggersList.RemoveAt(Current);
                                Read.TriggersList.Insert(Previous, np);
                            }
                        }
                        AllowCellChanging = true;
                        Grid.CurrentCell = Grid.Rows[dgs[dgs.Count() - 1].Index].Cells[1];
                        foreach (DataGridViewRow row in colle)
                        {

                            Grid.Rows[row.Index].Selected = true;
                        }
                    }
                }
            }
        }
        private void DownObjects(object sender, EventArgs e)
        {
            if (Read != null)
            {
                DataGridView Grid = null;
                int GridIndex = MainTabControl.SelectedIndex;
                if (MainTabControl.SelectedIndex == 0)
                    Grid = NpcMobsGrid;
                else if (MainTabControl.SelectedIndex == 1)
                    Grid = ResourcesGrid;
                else if (MainTabControl.SelectedIndex == 2)
                    Grid = DynamicGrid;
                else if (MainTabControl.SelectedIndex == 3)
                    Grid = TriggersGrid;
                var dgs = Grid.SelectedRows.Cast<DataGridViewRow>().OrderByDescending(i => i.Index).ToList();
                var colle = Grid.SelectedRows.Cast<DataGridViewRow>().OrderByDescending(i => i.Index).ToList();
                if (dgs.Count != 0)
                {
                    if (dgs[0].Index != Grid.Rows.Count - 1)
                    {
                        AllowCellChanging = false;
                        foreach (DataGridViewRow Row in dgs)
                        {
                            int Current = Row.Index;
                            int Next = Row.Index + 1;
                            Grid.Rows.Remove(Row);
                            Grid.Rows.Insert(Next, Row);
                            if (GridIndex == 0)
                            {
                                ClassDefaultMonsters np = Read.NpcMobList[Current];
                                Read.NpcMobList.RemoveAt(Current);
                                Read.NpcMobList.Insert(Next, np);
                            }
                            else if (GridIndex == 1)
                            {
                                ClassDefaultResources np = Read.ResourcesList[Current];
                                Read.ResourcesList.RemoveAt(Current);
                                Read.ResourcesList.Insert(Next, np);
                            }
                            else if (GridIndex == 2)
                            {
                                ClassDynamicObject np = Read.DynamicsList[Current];
                                Read.DynamicsList.RemoveAt(Current);
                                Read.DynamicsList.Insert(Next, np);
                            }
                            else if (GridIndex == 3)
                            {
                                ClassTrigger np = Read.TriggersList[Current];
                                Read.TriggersList.RemoveAt(Current);
                                Read.TriggersList.Insert(Next, np);
                            }

                        }
                        AllowCellChanging = true;
                        Grid.CurrentCell = Grid.Rows[dgs[dgs.Count() - 1].Index].Cells[1];
                        foreach (DataGridViewRow row in colle)
                        {

                            Grid.Rows[row.Index].Selected = true;
                        }
                    }
                }
            }

        }
        private void GridsKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W && ModifierKeys == Keys.Shift)
            {
                UpObjects(null, null);
            }
            if (e.KeyCode == Keys.S && ModifierKeys == Keys.Shift)
            {
                DownObjects(null, null);
            }
        }
        private void MoveToTrigger_Click(object sender, EventArgs e)
        {
            if (Read != null)
            {
                int TriggerId = 0;
                if (MainTabControl.SelectedIndex == 0)
                    int.TryParse(Trigger.Text, out TriggerId);
                else if (MainTabControl.SelectedIndex == 1)
                    int.TryParse(RTriggerID.Text, out TriggerId);
                else if (MainTabControl.SelectedIndex == 2)
                    int.TryParse(DTrigger_id.Text, out TriggerId);
                int TriggerIndex = Read.TriggersList.FindIndex(z => z.Id == TriggerId);
                if (TriggerIndex != -1)
                {
                    TriggersGrid.CurrentCell = TriggersGrid.Rows[TriggerIndex].Cells[1];
                    MainTabControl.SelectedIndex = 3;
                }
                else
                {
                    if (Language == 1)
                    {
                        MessageBox.Show("Операция не удалась!!...", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (Language == 2)
                    {
                        MessageBox.Show("Invalid action!!...", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
        private void LineUpX_Click(object sender, EventArgs e)
        {
            if (Read != null)
            {
                if (MainTabControl.SelectedIndex == 0)
                {
                    foreach (var item in NpcRowCollection)
                    {
                        Read.NpcMobList[item].X_position = Read.NpcMobList[NpcRowIndex].X_position;
                    }
                }
                else if (MainTabControl.SelectedIndex == 1)
                {
                    foreach (var item in ResourcesRowCollection)
                    {
                        Read.ResourcesList[item].X_position = Read.ResourcesList[NpcRowIndex].X_position;
                    }
                }
                else if (MainTabControl.SelectedIndex == 2)
                {
                    foreach (var item in DynamicsRowCollection)
                    {
                        Read.DynamicsList[item].X_position = Read.DynamicsList[NpcRowIndex].X_position;
                    }
                }
            }
        }
        private void LineUpZ_Click(object sender, EventArgs e)
        {
            if (Read != null)
            {
                if (MainTabControl.SelectedIndex == 0)
                {
                    foreach (var item in NpcRowCollection)
                    {
                        Read.NpcMobList[item].Z_position = Read.NpcMobList[NpcRowIndex].Z_position;
                    }
                }
                else if (MainTabControl.SelectedIndex == 1)
                {
                    foreach (var item in ResourcesRowCollection)
                    {
                        Read.ResourcesList[item].Z_position = Read.ResourcesList[NpcRowIndex].Z_position;
                    }
                }
                else if (MainTabControl.SelectedIndex == 2)
                {
                    foreach (var item in DynamicsRowCollection)
                    {
                        Read.DynamicsList[item].Z_position = Read.DynamicsList[NpcRowIndex].Z_position;
                    }
                }
            }
        }
        private void ChangeLanguage(object sender, EventArgs e)
        {
            int LocationIndex = ExistenceLocating.SelectedIndex;
            int TypeIndex = ExistenceType.SelectedIndex;
            int AddLocationIndex = AddintExistenceType.SelectedIndex;
            int AddMonsterTypeIndex = AddMonsterType.SelectedIndex;
            int StartWeekDay = TStartWeekDay.SelectedIndex;
            int StopWeekDay = TStopWeekDay.SelectedIndex;
            int AgressionIndex = Agression.SelectedIndex;
            int PathIndex = Path_type.SelectedIndex;
            Control co = sender as Control;
            #region Russian
            if (co.Name == "Russian")
            {
                Language = 1;
                OpenFiles.Text = "Открыть";
                Open_surfaces.Text = "Открыть";
                ButtonShowMap.Text = "Показать карту";
                if (Read == null)
                {
                    ExistenceTab.Text = "Мобы и Нипы";
                    ResourcesTab.Text = "Ресурсы";
                    DynObjectsTab.Text = "Динамические Объекты";
                    TriggersTab.Text = "Тригеры";
                }
                else
                {
                    ExistenceTab.Text = string.Format("Мобы и Нипы" + " {0}/{1}", NpcRowCollection.Count, Read.NpcMobsAmount);
                    ResourcesTab.Text = string.Format("Ресурсы" + " {0}/{1}", ResourcesRowCollection.Count, Read.ResourcesAmount);
                    DynObjectsTab.Text = string.Format("Динамические Объекты" + " {0}/{1}", DynamicsRowCollection.Count, Read.DynobjectAmount);
                    TriggersTab.Text = string.Format("Тригеры" + " {0}/{1}", TriggersRowCollection.Count, Read.TriggersAmount);
                }
                OptionsTab.Text = "Настройки и сохранение";
                SearchTab.Text = "Поиск";
                ErrorsTab.Text = "Ошибки";
                ExistenceLocating.Items.Clear();
                ExistenceLocating.Items.AddRange(new string[] { "Наземный", "Свободный" });
                ExistenceLocating.SelectedIndex = LocationIndex;
                AddintExistenceType.Items.Clear();
                AddintExistenceType.Items.AddRange(new string[] { "Наземный", "Свободный" });
                AddintExistenceType.SelectedIndex = AddLocationIndex;
                ExistenceType.Items.Clear();
                ExistenceType.Items.AddRange(new string[] { "Моб", "Нпс" });
                ExistenceType.SelectedIndex = TypeIndex;
                AddMonsterType.Items.Clear();
                AddMonsterType.Items.AddRange(new string[] { "Моб", "Нпс" });
                AddMonsterType.SelectedIndex = AddMonsterTypeIndex;
                MainGroupBox.Text = "Основное";
                groupBox1.Text = "Мобы|Нипы";
                groupBox3.Text = "Основное";
                groupBox7.Text = "Основное";
                groupBox9.Text = "Изображение";
                groupBox8.Text = "Настройки для добавления новых существ";
                label3.Text = "Расположе:";
                label3.Location = new Point(2, 14);
                label9.Text = "Поворот X:"; label9.Location = new Point(5, 106);
                label10.Text = "Поворот Y:"; label10.Location = new Point(5, 131);
                label11.Text = "Поворот Z:"; label11.Location = new Point(5, 154);
                label14.Text = "Разброс X:"; label14.Location = new Point(6, 178);
                label13.Text = "Разброс Y:"; label13.Location = new Point(6, 201);
                label12.Text = "Разброс Z:"; label12.Location = new Point(6, 224);
                label15.Text = "Тип:"; label15.Location = new Point(215, 12);
                label5.Text = "В группе:"; label5.Location = new Point(186, 39);
                label16.Text = "Тип группы:"; label16.Location = new Point(172, 62);
                label18.Text = "Триггер:"; label18.Location = new Point(191, 109);
                label19.Text = "Врем.Жизни:"; label19.Location = new Point(165, 132);
                label20.Text = "Макс.колво:"; label20.Location = new Point(169, 155);
                ExistenceAutoRevive.Text = "Мгновенный респавн"; ExistenceAutoRevive.Location = new Point(201, 200);
                ExistenceInitGen.Text = "Активировать генератор"; ExistenceInitGen.Location = new Point(184, 178);
                ExistenceCloneButton.Text = "Клонировать";
                ExistenceRemoveButton.Text = "Удалить";
                ExistenceGroupCloneButton.Text = "Клонировать";
                ExistenceGroupRemoveButton.Text = "Удалить";
                label22.Text = "Количество:"; label22.Location = new Point(32, 37);
                label23.Text = "Время респавна:"; label23.Location = new Point(5, 60);
                label24.Text = "Кол-во смертей:"; label24.Location = new Point(8, 83);
                label25.Text = "Агрессия:"; label25.Location = new Point(49, 106);
                label26.Text = "Тип пути:"; label26.Location = new Point(51, 130);
                label27.Text = "Скорость:"; label27.Location = new Point(45, 154);
                label28.Text = "Путь:"; label28.Location = new Point(72, 177);
                label29.Text = "Оффсет воды:"; label29.Location = new Point(16, 201);
                label30.Text = "Оффсет повор:"; label30.Location = new Point(196, 14);
                label31.Text = "Просит помощь:"; label31.Location = new Point(193, 60);
                label32.Text = "Нужна помощь:"; label32.Location = new Point(199, 83);
                label33.Text = "Показ трупа(Сек):"; label33.Location = new Point(184, 107);
                label35.Text = "Группа:"; label35.Location = new Point(245, 37);
                ExistenceInsertCordsFromGame.Text = "Вставить координаты с игры";
                label67.Text = "ID триггера:"; label67.Location = new Point(89, 18);
                label68.Text = "Расположение:"; label68.Location = new Point(62, 39);
                label66.Text = "Тип существа:"; label66.Location = new Point(70, 62);
                label65.Text = "ID создаваемого существа:"; label65.Location = new Point(8, 89);
                label64.Text = "Количество существ:"; label64.Location = new Point(42, 112);
                label63.Text = "Время респавна:"; label63.Location = new Point(62, 135);
                ResourcesCloneButton.Text = "Клонировать";
                ResourcesRemoveButton.Text = "Удалить";
                label43.Text = "Разброс X:"; label43.Location = new Point(8, 82);
                label42.Text = "Разброс Z:"; label42.Location = new Point(9, 105);
                label45.Text = "Наклон 1:"; label45.Location = new Point(14, 128);
                label44.Text = "Наклон 2:"; label44.Location = new Point(14, 151);
                label41.Text = "Поворот:"; label41.Location = new Point(16, 174);
                label51.Text = "В группе:"; label51.Location = new Point(189, 15);
                label37.Text = "Триггер:"; label37.Location = new Point(192, 60);
                groupBox11.Text = "Настройки для добавления новых ресурсов";
                label75.Text = "ID триггера:"; label75.Location = new Point(90, 18);
                label57.Text = "ID создаваемого ресурса:"; label57.Location = new Point(18, 42);
                label56.Text = "Количество ресурсов:"; label56.Location = new Point(40, 65);
                label54.Text = "Время респавна:"; label54.Location = new Point(65, 88);
                ResourcesInsertCordsFromGame.Text = "Вставить координаты с игры";
                ResourcesGroupCloneButton.Text = "Клонировать";
                ResourcesGroupRemoveButton.Text = "Удалить";
                groupBox5.Text = "Ресурсы";
                ResourcesInitGen.Text = "Активировать генератор"; ResourcesInitGen.Location = new Point(187,107);
                ResourcesAutoRevive.Text = "Мгновенный респавн"; ResourcesAutoRevive.Location = new Point(204, 129);
                label59.Text = "Количество:"; label59.Location = new Point(224, 41);
                label58.Text = "Время респа:"; label58.Location = new Point(218, 67);
                label55.Text = "Тип:"; label55.Location = new Point(272, 93);
                label53.Text = "Над землей:"; label53.Location = new Point(224, 119);
                DynObjectsCloneButton.Text = "Клонировать";
                DynObjectsRemoveButton.Text = "Удалить";
                label72.Text = "Наклон 1:"; label72.Location = new Point(4, 107);
                label71.Text = "Наклон 2:"; label71.Location = new Point(184, 38);
                label70.Text = "Поворот:"; label70.Location = new Point(187, 61);
                label73.Text = "Триггер:"; label73.Location = new Point(192, 83);
                label74.Text = "Увеличение:"; label74.Location = new Point(167, 107);
                groupBox10.Text = "Настройки для добавления Дин.Объектов";
                label61.Text = "ID Дин.Объекта:"; label61.Location = new Point(69, 18);
                label40.Text = "ID триггера:"; label40.Location = new Point(89, 43);
                DynObjectsInsertCordsFromGame.Text = "Вставить координаты с игры";
                TriggersCloneButton.Text = "Клонировать";
                TriggersRemoveButton.Text = "Удалить";
                groupBox12.Text = "Основное";
                groupBox16.Text = "Запуск";
                groupBox17.Text = "Выключение";
                groupBox13.Text = "Используется в существах";
                groupBox14.Text = "Используется в ресурсах";
                groupBox15.Text = "Используется в дин.объектах";
                GotoNpcMobsContacts.Text = "Перейти к выбранному";
                GotoResourcesContacts.Text = "Перейти к выбранному";
                GotoDynamicsContacts.Text = "Перейти к выбранному";
                label89.Text = "ID Триггера:"; label89.Location = new Point(71, 16);
                label79.Text = "ID в панели ГМ:"; label79.Location = new Point(50, 39);
                label99.Text = "Название:"; label99.Location = new Point(81, 61);
                label83.Text = "Задержка включения:"; label83.Location = new Point(14, 84);
                label84.Text = "Задержка выключения:"; label84.Location = new Point(5, 106);
                TAutoStart.Text = "Запускать автоматически";
                label85.Text = "Продолжительность:"; label85.Location = new Point(7, 147);
                TStartBySchedule.Text = "Запускать по графику"; TStartBySchedule.Location = new Point(178, 170);
                TStopBySchedule.Text = "Выключать по графику"; TStopBySchedule.Location = new Point(174, 190);
                label86.Text = "Год:"; label86.Location = new Point(55, 16);
                label87.Text = "Месяц:"; label87.Location = new Point(38, 40);
                label88.Text = "День недели:"; label88.Location = new Point(1, 65);
                label90.Text = "День:"; label90.Location = new Point(168, 15);
                label91.Text = "Час:"; label91.Location = new Point(177, 41);
                label92.Text = "Минута:"; label92.Location = new Point(154, 65);
                label97.Text = "Год:"; label97.Location = new Point(55, 16);
                label96.Text = "Месяц:"; label96.Location = new Point(38, 40);
                label98.Text = "День недели:"; label98.Location = new Point(1, 65);
                label94.Text = "Час:"; label94.Location = new Point(177, 41);
                label95.Text = "День:"; label95.Location = new Point(168, 15);
                label93.Text = "Минута:"; label93.Location = new Point(154, 65);
                label52.Text = "Версия клиента для захвата координат из игры:";
                groupBox6.Text = "Горячие клавиши";
                label50.Text = "Существо:"; label50.Location = new Point(20, 19);
                label77.Text = "Ресурс:"; label77.Location = new Point(37, 46);
                label101.Text = "Дин.Объект:"; label101.Location = new Point(7, 73);
                SaveFile.Text = "Сохранить Npcgen.data";
                ConvertComboboxVersion.Location = new Point(335, 435);
                ConvertAndSaveButton.Text = "Конвертировать в     версию и сохранить";
                TStartWeekDay.Items.Clear();
                TStartWeekDay.Items.AddRange(new string[] {"Все",
"Воскресенье",
"Понедельник",
"Вторник",
"Среда",
"Четверг",
"Пятница",
"Суббота"});
                TStartWeekDay.SelectedIndex = StartWeekDay;
                TStopWeekDay.Items.Clear();
                TStopWeekDay.Items.AddRange(new string[] {"Все",
"Воскресенье",
"Понедельник",
"Вторник",
"Среда",
"Четверг",
"Пятница",
"Суббота"});
                TStopWeekDay.SelectedIndex = StopWeekDay;
                DeleteEmptyTrigger.Text = "Удалить пустые триггеры";
                MoveToTrigger.Text = "Перейти к триггеру";
                toolStripMenuItem1.Text = "Переместить";
                UpExistence.Text = "Выше   Shift+W";
                DownExistence.Text = "Ниже    Shift+S";
                toolStripMenuItem4.Text = "Переместить";
                UpTrigger.Text = "Выше   Shift+W";
                DownTrigger.Text = "Ниже    Shift+S";
                Agression.Items[0] = "Нет";
                Agression.Items[1] = "Агрессивный";
                Agression.Items[2] = "Защита";
                Path_type.Items[0] = "Нет";
                Path_type.Items[1] = "Обычный";
                Path_type.Items[2] = "Зацикленный";
                LineUpX.Text = "Выстроить в ряд по X";
                LineUpZ.Text = "Выстроить в ряд по Z";
                groupBox18.Text = "Поиск в существах";
                groupBox19.Text = "Поиск в ресурсах";
                groupBox20.Text = "Поиск в Дин.Объектах";
                groupBox21.Text = "Поиск в Триггерах";
                MoveToSelected.Text = "Перейти к выбранному";
                ExistenceSearchName_Radio.Text = "Название"; ExistenceSearchName_Radio.Location = new Point(1, 43);
                ExistenceSearchTrigger_Radio.Text = "Триггер"; ExistenceSearchTrigger_Radio.Location = new Point(10, 64);
                ExistenceSearchPath_Radio.Text = "Путь"; ExistenceSearchPath_Radio.Location = new Point(27, 86);
                ExistenceSearchButton.Text = "Найти";
                ResourceSearchName_Radio.Text = "Название"; ResourceSearchName_Radio.Location = new Point(1, 43);
                ResourceSearchTrigger_Radio.Text = "Триггер"; ResourceSearchTrigger_Radio.Location = new Point(10, 64);
                ResourceSearchButton.Text = "Найти";
                DynamicSearchName_Radio.Text = "Название"; DynamicSearchName_Radio.Location = new Point(1, 43);
                DynamicSearchTrigger_Radio.Text = "Триггер"; DynamicSearchTrigger_Radio.Location = new Point(10, 64);
                DynamicSearchButton.Text = "Найти";
                TriggerSearchName_Radio.Text = "Название"; TriggerSearchName_Radio.Location = new Point(1, 64);
                TriggerSearchButton.Text = "Найти";
                SearchErrorsButton.Text = "Найти ошибки";
                RemoveAllErrors.Text = "Удалить все объекты";
                ExportExistence.Text = "Экспорт";
                ImportExistence.Text = "Импорт";
                LineUpExistenceDropDown.Text = "Выстроить";
                ToolStripLineUpX.Text = "По Х";
                ToolStripLineUpZ.Text = "По Z";
                MoveExistenceDropDown.Text = "Переместить";
                MoveUpToolStripMenuItem.Text = "Выше   Shift+W";
                MoveDownToolStripMenuItem.Text = "Ниже    Shift+S";
                ExportResources.Text = "Экспорт";
                ImportResources.Text = "Импорт";
                LineUpResource.Text = "Выстроить";
                ResourcesOnX.Text = "По X";
                ResourcesOnZ.Text = "По Z";
                MoveResources.Text = "Переместить";
                ResourceUp.Text = "Выше   Shift+W";
                ResourceDown.Text = "Ниже Shift+S";
                toolStripButton3.Text = "Экспорт";
                toolStripButton4.Text = "Импорт";
                toolStripDropDownButton3.Text = "Выстроить";
                toolStripMenuItem7.Text = "По X";
                toolStripMenuItem8.Text = "По Z";
                toolStripDropDownButton4.Text = "Переместить";
                toolStripMenuItem9.Text = "Выше   Shift+W";
                toolStripMenuItem10.Text = "Ниже Shift+S";
                toolStripButton5.Text = "Экспорт";
                toolStripButton6.Text = "Импорт";
                toolStripDropDownButton6.Text = "Переместить";
                toolStripMenuItem13.Text = "Выше   Shift+W";
                toolStripMenuItem14.Text = "Ниже Shift+S";
                toolStripButton7.Text = "Очистить";
                экспортToolStripMenuItem.Text = "Экспорт";
                импортToolStripMenuItem.Text = "Импорт";
                toolStripMenuItem11.Text = "Импорт";
                toolStripMenuItem12.Text = "Экспорт";
                DynamicForm = new DynamicObjectsForm(DynamicsListRu, this);
                DynamicForm.LanguageChange(1);
            }
            #endregion
            #region English
            else if (co.Name == "English")
            {
                Language = 2;
                OpenFiles.Text = "Open";
                Open_surfaces.Text = "Open";
                ButtonShowMap.Text = "Show map";
                if (Read == null)
                {
                    ExistenceTab.Text = "Mobs and Npcs";
                    ResourcesTab.Text = "Resources";
                    DynObjectsTab.Text = "Dynamic Objects";
                    TriggersTab.Text = "Triggers";
                }
                else
                {
                    ExistenceTab.Text = string.Format("Mobs and Npcs" + " {0}/{1}", NpcRowCollection.Count, Read.NpcMobsAmount);
                    ResourcesTab.Text = string.Format("Resources" + " {0}/{1}", ResourcesRowCollection.Count, Read.ResourcesAmount);
                    DynObjectsTab.Text = string.Format("Dynamic Objects" + " {0}/{1}", DynamicsRowCollection.Count, Read.DynobjectAmount);
                    TriggersTab.Text = string.Format("Triggers" + " {0}/{1}", TriggersRowCollection.Count, Read.TriggersAmount);
                }
                OptionsTab.Text = "Options and saving";
                SearchTab.Text = "Search";
                ErrorsTab.Text = "Errors";
                ExistenceLocating.Items.Clear();
                ExistenceLocating.Items.AddRange(new string[] { "Ground", "Free" });
                ExistenceLocating.SelectedIndex = LocationIndex;
                AddintExistenceType.Items.Clear();
                AddintExistenceType.Items.AddRange(new string[] { "Ground", "Free" });
                AddintExistenceType.SelectedIndex = AddLocationIndex;
                ExistenceType.Items.Clear();
                ExistenceType.Items.AddRange(new string[] { "Mob", "Npc" });
                ExistenceType.SelectedIndex = TypeIndex;
                AddMonsterType.Items.Clear();
                AddMonsterType.Items.AddRange(new string[] { "Mob", "Npc" });
                AddMonsterType.SelectedIndex = AddMonsterTypeIndex;
                MainGroupBox.Text = "Default";
                groupBox8.Text = "Options for adding new existence";
                groupBox1.Text = "Mobs|Npcs";
                label3.Text = "Location:";
                label3.Location = new Point(21, 14);
                label9.Text = "Rotation X:"; label9.Location = new Point(11, 106);
                label10.Text = "Rotation Y:"; label10.Location = new Point(11, 131);
                label11.Text = "Rotation Z:"; label11.Location = new Point(11, 154);
                label14.Text = "Scatter X:"; label14.Location = new Point(18, 178);
                label13.Text = "Scatter Y:"; label13.Location = new Point(18, 201);
                label12.Text = "Scatter Z:"; label12.Location = new Point(18, 224);
                label15.Text = "Type:"; label15.Location = new Point(210, 12);
                label5.Text = "In group:"; label5.Location = new Point(192, 39);
                label16.Text = "Group type:"; label16.Location = new Point(178, 62);
                label18.Text = "Trigger:"; label18.Location = new Point(197, 109);
                label19.Text = "Life time:"; label19.Location = new Point(189, 132);
                label20.Text = "Max amount:"; label20.Location = new Point(168, 155);
                ExistenceAutoRevive.Text = "Instant respawn"; ExistenceAutoRevive.Location = new Point(235, 201);
                ExistenceInitGen.Text = "Activate generator"; ExistenceInitGen.Location = new Point(223, 178);
                ResourcesInitGen.Text = "Activate generator"; ResourcesInitGen.Location = new Point(226, 107);
                ResourcesAutoRevive.Text = "Instant respawn"; ResourcesAutoRevive.Location = new Point(238, 129);
                ExistenceCloneButton.Text = "Clone";
                ExistenceRemoveButton.Text = "Delete";
                ExistenceGroupCloneButton.Text = "Clone";
                ExistenceGroupRemoveButton.Text = "Delete";
                label22.Text = "Amount:"; label22.Location = new Point(59, 37);
                label23.Text = "Respawn time:"; label23.Location = new Point(22, 60);
                label24.Text = "Death amount:"; label24.Location = new Point(23, 83);
                label25.Text = "Agression:"; label25.Location = new Point(47, 106);
                label26.Text = "Path type:"; label26.Location = new Point(51, 130);
                label27.Text = "Speed:"; label27.Location = new Point(65, 154);
                label28.Text = "Type:"; label28.Location = new Point(75, 177);
                label29.Text = "Water offset:"; label29.Location = new Point(37, 201);
                label30.Text = "Turn offset:"; label30.Location = new Point(228, 14);
                label31.Text = "Ask help:"; label31.Location = new Point(239, 60);
                label32.Text = "Need help:"; label32.Location = new Point(227, 83);
                label33.Text = "Corpse(sec):"; label33.Location = new Point(219, 107);
                label35.Text = "Group:"; label35.Location = new Point(251, 37);
                ExistenceInsertCordsFromGame.Text = "Insert Coordinates from game";
                label67.Text = "Trigger ID:"; label67.Location = new Point(101, 18);
                label68.Text = "Location:"; label68.Location = new Point(100, 39);
                label66.Text = "Existence type:"; label66.Location = new Point(68, 62);
                label65.Text = "Existence ID:"; label65.Location = new Point(86, 89);
                label64.Text = "Existence amount:"; label64.Location = new Point(62, 112);
                label63.Text = "Respawn time:"; label63.Location = new Point(79, 135);
                groupBox3.Text = "Default";
                ResourcesCloneButton.Text = "Clone";
                ResourcesRemoveButton.Text = "Delete";
                label43.Text = "Spread X:"; label43.Location = new Point(15, 82);
                label42.Text = "Spread Z:"; label42.Location = new Point(16, 105);
                label45.Text = "Incline 1:"; label45.Location = new Point(20, 128);
                label44.Text = "Incline 2:"; label44.Location = new Point(20, 151);
                label41.Text = "Rotation:"; label41.Location = new Point(20, 174);
                label51.Text = "In group:"; label51.Location = new Point(194, 15);
                label37.Text = "Trigger:"; label37.Location = new Point(200, 60);
                groupBox11.Text = "Options for adding new resources";
                label75.Text = "Trigger ID:"; label75.Location = new Point(101, 18);
                label57.Text = "Resource ID:"; label57.Location = new Point(87, 42);
                label56.Text = "Resources amount:"; label56.Location = new Point(58, 65);
                label54.Text = "Respawn time:"; label54.Location = new Point(80, 88);
                label53.Text = "Above ground:"; label53.Location = new Point(218, 119);
                ResourcesInsertCordsFromGame.Text = "Insert Coordinates from game";
                ResourcesGroupCloneButton.Text = "Clone";
                ResourcesGroupRemoveButton.Text = "Delete";
                groupBox5.Text = "Resources";
                label59.Text = "Amount:"; label59.Location = new Point(251, 41);
                label58.Text = "Respa. time:"; label58.Location = new Point(227, 67);
                label55.Text = "Type:"; label55.Location = new Point(267, 93);
                DynObjectsCloneButton.Text = "Clone";
                DynObjectsRemoveButton.Text = "Delete";
                groupBox7.Text = "Default";
                groupBox9.Text = "Image";
                label72.Text = "Incline 1:"; label72.Location = new Point(11, 107);
                label71.Text = "Incline 2:"; label71.Location = new Point(192, 38);
                label70.Text = "Rotation:"; label70.Location = new Point(190, 61);
                label73.Text = "Trigger:"; label73.Location = new Point(198, 83);
                label74.Text = "Scale:"; label74.Location = new Point(206, 107);
                groupBox10.Text = "Options for adding new dyn.Objects";
                label61.Text = "Dyn.Object ID:"; label61.Location = new Point(81, 18);
                label40.Text = "Trigger ID:"; label40.Location = new Point(100, 43);
                DynObjectsInsertCordsFromGame.Text = "Insert Coordinates from game";
                TriggersCloneButton.Text = "Clone";
                TriggersRemoveButton.Text = "Delete";
                groupBox12.Text = "Default";
                groupBox16.Text = "Start schedule";
                groupBox17.Text = "Stop schedule";
                groupBox13.Text = "Using in existence";
                groupBox14.Text = "Using in resources";
                groupBox15.Text = "Using in Dyn.Objects";
                GotoNpcMobsContacts.Text = "Move to selected";
                GotoResourcesContacts.Text = "Move to selected";
                GotoDynamicsContacts.Text = "Move to selected";
                label89.Text = "Trigger ID:"; label89.Location = new Point(85, 16);
                label79.Text = "ID in GM console:"; label79.Location = new Point(44, 39);
                label99.Text = "Name:"; label99.Location = new Point(103, 61);
                label83.Text = "Start delay:"; label83.Location = new Point(80, 84);
                label84.Text = "Stop delay:"; label84.Location = new Point(81, 106);
                TAutoStart.Text = "Start automatically";
                label85.Text = "During:"; label85.Location = new Point(95, 147);
                TStartBySchedule.Text = "Start on schedule"; TStartBySchedule.Location = new Point(209, 170);
                TStopBySchedule.Text = "Stop on schedule"; TStopBySchedule.Location = new Point(209, 190);
                label86.Text = "Year:"; label86.Location = new Point(52, 16);
                label87.Text = "Month:"; label87.Location = new Point(41, 40);
                label88.Text = "Week day:"; label88.Location = new Point(20, 65);
                label90.Text = "Day:"; label90.Location = new Point(177, 15);
                label91.Text = "Hour:"; label91.Location = new Point(171, 41);
                label92.Text = "Minute:"; label92.Location = new Point(160, 65);
                label97.Text = "Year:"; label97.Location = new Point(50, 16);
                label96.Text = "Month:"; label96.Location = new Point(40, 40);
                label98.Text = "Week day:"; label98.Location = new Point(22, 65);
                label95.Text = "Day:"; label95.Location = new Point(177, 15);
                label94.Text = "Hour:"; label94.Location = new Point(171, 41);
                label93.Text = "Minute:"; label93.Location = new Point(160, 65);
                label52.Text = "Client version for catching coordinates from game:";
                groupBox6.Text = "Hot keys";
                label50.Text = "Existence:"; label50.Location = new Point(24, 19);
                label77.Text = "Resource:"; label77.Location = new Point(23, 46);
                label101.Text = "Dyn.object:"; label101.Location = new Point(18, 73);
                SaveFile.Text = "Save Npcgen.data";
                ConvertComboboxVersion.Location = new Point(274, 435);
                ConvertAndSaveButton.Text = "Convert to               version and save";
                TStartWeekDay.Items.Clear();
                TStartWeekDay.Items.AddRange(new string[] {"All",
"Sunday",
"Monday",
"Tuesday",
"Wednesday",
"Thursday",
"Friday",
"Saturday"});
                TStartWeekDay.SelectedIndex = StartWeekDay;
                TStopWeekDay.Items.Clear();
                TStopWeekDay.Items.AddRange(new string[] {"All",
"Sunday",
"Monday",
"Tuesday",
"Wednesday",
"Thursday",
"Friday",
"Saturday"});
                TStopWeekDay.SelectedIndex = StopWeekDay;
                DeleteEmptyTrigger.Text = "Delete empty triggers";
                MoveToTrigger.Text = "Move to trigger";
                toolStripMenuItem1.Text = "Move";
                UpExistence.Text = "Up           Shift+W";
                DownExistence.Text = "Down      Shift+S";
                toolStripMenuItem4.Text = "Move";
                UpTrigger.Text = "Up           Shift+W";
                DownTrigger.Text = "Down      Shift+S";
                Agression.Items[0] = "No";
                Agression.Items[1] = "Aggressive";
                Agression.Items[2] = "Defend";
                Path_type.Items[0] = "No";
                Path_type.Items[1] = "Default";
                Path_type.Items[2] = "Cycle";
                LineUpX.Text = "Line up on X";
                LineUpZ.Text = "Line up on Z";
                groupBox18.Text = "Search in existence";
                groupBox19.Text = "Search in resources";
                groupBox20.Text = "Search in Dyn.Objects";
                groupBox21.Text = "Search in triggers";
                MoveToSelected.Text = "Move to selected";
                ExistenceSearchName_Radio.Text = "Name"; ExistenceSearchName_Radio.Location = new Point(22, 43);
                ExistenceSearchTrigger_Radio.Text = "Trigger"; ExistenceSearchTrigger_Radio.Location = new Point(17, 64);
                ExistenceSearchPath_Radio.Text = "Path"; ExistenceSearchPath_Radio.Location = new Point(28, 86);
                ExistenceSearchButton.Text = "Search";
                ResourceSearchName_Radio.Text = "Name"; ResourceSearchName_Radio.Location = new Point(22, 43);
                ResourceSearchTrigger_Radio.Text = "Trigger"; ResourceSearchTrigger_Radio.Location = new Point(17, 64);
                ResourceSearchButton.Text = "Search";
                DynamicSearchName_Radio.Text = "Name"; DynamicSearchName_Radio.Location = new Point(22, 43);
                DynamicSearchTrigger_Radio.Text = "Trigger"; DynamicSearchTrigger_Radio.Location = new Point(17, 64);
                DynamicSearchButton.Text = "Search";
                TriggerSearchName_Radio.Text = "Name"; TriggerSearchName_Radio.Location = new Point(22, 64);
                TriggerSearchButton.Text = "Search";
                SearchErrorsButton.Text = "Search errors";
                RemoveAllErrors.Text = "Remove all objects";
                ExportExistence.Text = "Export";
                ImportExistence.Text = "Import";
                LineUpExistenceDropDown.Text = "Line up";
                ToolStripLineUpX.Text = "On Х";
                ToolStripLineUpZ.Text = "On Z";
                MoveExistenceDropDown.Text = "Move objects";
                MoveUpToolStripMenuItem.Text = "Up           Shift+W";
                MoveDownToolStripMenuItem.Text = "Down      Shift+S";
                ExportResources.Text = "Export";
                ImportResources.Text = "Import";
                LineUpResource.Text = "Line up";
                ResourcesOnX.Text = "On X";
                ResourcesOnZ.Text = "On Z";
                MoveResources.Text = "Move objects";
                ResourceUp.Text = "Up           Shift+W";
                ResourceDown.Text = "Down      Shift+S";
                toolStripButton3.Text = "Export";
                toolStripButton4.Text = "Import";
                toolStripDropDownButton3.Text = "Line up";
                toolStripMenuItem7.Text = "On X";
                toolStripMenuItem8.Text = "On Z";
                toolStripDropDownButton4.Text = "Move objects";
                toolStripMenuItem9.Text = "Up   Shift+W";
                toolStripMenuItem10.Text = "Down Shift+S";
                toolStripButton5.Text = "Export";
                toolStripButton6.Text = "Import";
                toolStripDropDownButton6.Text = "Move objects";
                toolStripMenuItem13.Text = "Up   Shift+W";
                toolStripMenuItem14.Text = "Down Shift+S";
                toolStripButton7.Text = "Clear";
                экспортToolStripMenuItem.Text = "Export";
                импортToolStripMenuItem.Text = "Import";
                toolStripMenuItem11.Text = "Import";
                toolStripMenuItem12.Text = "Export";
                DynamicForm = new DynamicObjectsForm(DynamicsListEn, this);
                DynamicForm.LanguageChange(2);
            }
            #endregion
            if (Read != null)
            {
                DynamicGrid.Rows.Clear();
                SortDynamicObjects();
            }
            if (ChooseFromElementsForm != null)
            {
                ChooseFromElementsForm.RefreshLanguage(Language);
            }

        }
        #endregion
        #region NpcsAndMobs
        private void NpcMobsGrid_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > 0 & e.RowIndex >= 0)
            {
                string j = Convert.ToString(NpcMobsGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                toolTip1.SetToolTip(NpcMobsGrid, j);
            }
        }
        private void ExistenceGrid_CellChanged(object sender, EventArgs e)
        {
            if (AllowCellChanging == true)
            {
                NpcsGroupGrid.Rows.Clear();
                NpcRowCollection = NpcMobsGrid.SelectedRows.Cast<DataGridViewRow>().Select(q => q.Index).OrderByDescending(r => r).ToList();
                if (NpcMobsGrid.CurrentRow != null)
                {
                    NpcRowIndex = NpcMobsGrid.CurrentRow.Index;
                    if (NpcRowIndex != -1)
                    {
                        ExistenceLocating.SelectedIndex = Read.NpcMobList[NpcRowIndex].Location;
                        Group_amount_textbox.Text = Read.NpcMobList[NpcRowIndex].Amount_in_group.ToString();
                        X_position.Text = Read.NpcMobList[NpcRowIndex].X_position.ToString();
                        Y_position.Text = Read.NpcMobList[NpcRowIndex].Y_position.ToString();
                        Z_position.Text = Read.NpcMobList[NpcRowIndex].Z_position.ToString();
                        X_rotate.Text = Read.NpcMobList[NpcRowIndex].X_direction.ToString();
                        Y_rotate.Text = Read.NpcMobList[NpcRowIndex].Y_direction.ToString();
                        Z_rotate.Text = Read.NpcMobList[NpcRowIndex].Z_direction.ToString();
                        X_scatter.Text = Read.NpcMobList[NpcRowIndex].X_random.ToString();
                        Y_scatter.Text = Read.NpcMobList[NpcRowIndex].Y_random.ToString();
                        Z_scatter.Text = Read.NpcMobList[NpcRowIndex].Z_random.ToString();
                        ExistenceType.SelectedIndex = Read.NpcMobList[NpcRowIndex].Type;
                        Group_type.Text = Read.NpcMobList[NpcRowIndex].iGroupType.ToString();
                        ExistenceInitGen.Checked = Convert.ToBoolean(Read.NpcMobList[NpcRowIndex].BInitGen);
                        ExistenceAutoRevive.Checked = Convert.ToBoolean(Read.NpcMobList[NpcRowIndex].bAutoRevive);
                        BValicOnce.Checked = Convert.ToBoolean(Read.NpcMobList[NpcRowIndex].BValicOnce);
                        dwGenId.Text = Read.NpcMobList[NpcRowIndex].dwGenId.ToString();
                        Trigger.Text = Read.NpcMobList[NpcRowIndex].Trigger_id.ToString();
                        Life_time.Text = Read.NpcMobList[NpcRowIndex].Life_time.ToString();
                        IMaxNuml.Text = Read.NpcMobList[NpcRowIndex].MaxRespawnTime.ToString();
                        for (int i = 0; i < Read.NpcMobList[NpcRowIndex].Amount_in_group; i++)
                        {
                            string Name = "?";
                            int n = Element.ExistenceLists.FindIndex(v => v.Id == Read.NpcMobList[NpcRowIndex].MobDops[i].Id);
                            if (n != -1)
                                Name = Element.ExistenceLists[n].Name;
                            NpcsGroupGrid.Rows.Add(i + 1, Read.NpcMobList[NpcRowIndex].MobDops[i].Id, Name);
                            if (Read.NpcMobList[i].Type == 1)
                            {
                                NpcsGroupGrid.Rows[i].Cells[1].Style.ForeColor = Color.FromArgb(251, 251, 107);
                                NpcsGroupGrid.Rows[i].Cells[2].Style.ForeColor = Color.FromArgb(251, 251, 107);
                            }
                        }
                    }
                    if (MapForm != null && MainProgressBar.Value == 0 && NpcRowCollection.Count != 0 && MapForm.Visible == true)
                    {
                        MapForm.GetCoordinates(GetPoint(1));
                    }
                    UnderExistenceGrid_CellChanged(null, null);
                }
                if (Language == 1)
                {
                    ExistenceTab.Text = string.Format("Мобы и Нипы" + " {0}/{1}", NpcRowCollection.Count, Read.NpcMobsAmount);
                }
                else
                {
                    ExistenceTab.Text = string.Format("Mobs and Npcs" + " {0}/{1}", NpcRowCollection.Count, Read.NpcMobsAmount);
                }
            }
        }
        private void UnderExistenceGrid_CellChanged(object sender, EventArgs e)
        {
            if (NpcsGroupGrid.CurrentRow != null)
            {
                if (NpcsGroupGrid.CurrentRow.Index != -1)
                {
                    UnderNpcRowCollection = NpcsGroupGrid.SelectedRows.Cast<DataGridViewRow>().Select(b => b.Index).ToList();
                    NpcGroupIndex = NpcsGroupGrid.CurrentRow.Index;
                    Id_numeric.Value = Read.NpcMobList[NpcRowIndex].MobDops[NpcGroupIndex].Id;
                    Amount_numeric.Value = Read.NpcMobList[NpcRowIndex].MobDops[NpcGroupIndex].Amount;
                    Respawn_numeric.Value = Read.NpcMobList[NpcRowIndex].MobDops[NpcGroupIndex].Respawn;
                    DeathAmount_numeric.Value = Read.NpcMobList[NpcRowIndex].MobDops[NpcGroupIndex].Dead_amount;
                    Agression.SelectedIndex = Read.NpcMobList[NpcRowIndex].MobDops[NpcGroupIndex].Agression;
                    Path_type.SelectedIndex = Read.NpcMobList[NpcRowIndex].MobDops[NpcGroupIndex].Path_type;
                    Path_speed.Value = Read.NpcMobList[NpcRowIndex].MobDops[NpcGroupIndex].Speed;
                    Path_numeric.Value = Read.NpcMobList[NpcRowIndex].MobDops[NpcGroupIndex].Path;
                    Water_numeric.Value = Convert.ToDecimal(Read.NpcMobList[NpcRowIndex].MobDops[NpcGroupIndex].fOffsetWater);
                    Turn_numeric.Value = Convert.ToDecimal(Read.NpcMobList[NpcRowIndex].MobDops[NpcGroupIndex].fOffsetTrn);
                    Group_numeric.Value = Read.NpcMobList[NpcRowIndex].MobDops[NpcGroupIndex].Group;
                    AskHelp_numeric.Value = Read.NpcMobList[NpcRowIndex].MobDops[NpcGroupIndex].Group_help_sender;
                    NeedHelp_numeric.Value = Read.NpcMobList[NpcRowIndex].MobDops[NpcGroupIndex].Group_help_Needer;
                    bNeedHelp.Checked = Convert.ToBoolean(Read.NpcMobList[NpcRowIndex].MobDops[NpcGroupIndex].bNeedHelp);
                    bFaction.Checked = Convert.ToBoolean(Read.NpcMobList[NpcRowIndex].MobDops[NpcGroupIndex].bFaction);
                    bFac_Helper.Checked = Convert.ToBoolean(Read.NpcMobList[NpcRowIndex].MobDops[NpcGroupIndex].bFac_Helper);
                    bFac_Accept.Checked = Convert.ToBoolean(Read.NpcMobList[NpcRowIndex].MobDops[NpcGroupIndex].bFac_Accept);
                    Deadtime_numeric.Value = Read.NpcMobList[NpcRowIndex].MobDops[NpcGroupIndex].Dead_time;
                    RefreshLower_numeric.Value = Read.NpcMobList[NpcRowIndex].MobDops[NpcGroupIndex].RefreshLower;
                }
            }
        }
        private void NpcAndMobsDefaultLeave(object sender, EventArgs e)
        {
            if (NpcRowCollection != null && Read != null)
            {
                Control co = sender as Control;
                float FloatValue;
                int IntValue;
                switch (co.Name)
                {
                    case "ExistenceLocating":
                        {
                            foreach (var item in NpcRowCollection)
                            {
                                Read.NpcMobList[item].Location = ExistenceLocating.SelectedIndex;
                            }
                            break;
                        }
                    case "X_position":
                        {
                            float.TryParse(X_position.Text, out FloatValue);
                            foreach (var item in NpcRowCollection)
                            {
                                Read.NpcMobList[item].X_position = FloatValue;
                            }
                            break;
                        }
                    case "Y_position":
                        {
                            float.TryParse(Y_position.Text, out FloatValue);
                            foreach (var item in NpcRowCollection)
                            {
                                Read.NpcMobList[item].Y_position = FloatValue;
                            }
                            break;
                        }
                    case "Z_position":
                        {
                            float.TryParse(Z_position.Text, out FloatValue);
                            foreach (var item in NpcRowCollection)
                            {
                                Read.NpcMobList[item].Z_position = FloatValue;
                            }
                            break;
                        }
                    case "X_rotate":
                        {
                            float.TryParse(X_rotate.Text, out FloatValue);
                            foreach (var item in NpcRowCollection)
                            {
                                Read.NpcMobList[item].X_direction = FloatValue;
                            }
                            break;
                        }
                    case "Y_rotate":
                        {
                            float.TryParse(Y_rotate.Text, out FloatValue);
                            foreach (var item in NpcRowCollection)
                            {
                                Read.NpcMobList[item].Y_direction = FloatValue;
                            }
                            break;
                        }
                    case "Z_rotate":
                        {
                            float.TryParse(Z_rotate.Text, out FloatValue);
                            foreach (var item in NpcRowCollection)
                            {
                                Read.NpcMobList[item].Z_direction = FloatValue;
                            }
                            break;
                        }
                    case "X_scatter":
                        {
                            float.TryParse(X_scatter.Text, out FloatValue);
                            foreach (var item in NpcRowCollection)
                            {
                                Read.NpcMobList[item].X_random = FloatValue;
                            }
                            break;
                        }
                    case "Y_scatter":
                        {
                            float.TryParse(Y_scatter.Text, out FloatValue);
                            foreach (var item in NpcRowCollection)
                            {
                                Read.NpcMobList[item].Y_random = FloatValue;
                            }
                            break;
                        }
                    case "Z_scatter":
                        {
                            float.TryParse(Z_scatter.Text, out FloatValue);
                            foreach (var item in NpcRowCollection)
                            {
                                Read.NpcMobList[item].Z_random = FloatValue;
                            }
                            break;
                        }
                    case "ExistenceType":
                        {
                            foreach (var item in NpcRowCollection)
                            {
                                Read.NpcMobList[item].Type = ExistenceType.SelectedIndex;
                                if (ExistenceType.SelectedIndex == 1)
                                {
                                    NpcMobsGrid.Rows[item].Cells[1].Style.ForeColor = Color.FromArgb(251, 251, 107);
                                    NpcMobsGrid.Rows[item].Cells[2].Style.ForeColor = Color.FromArgb(251, 251, 107);
                                }
                                else
                                {
                                    NpcMobsGrid.Rows[item].Cells[1].Style.ForeColor = Color.FromArgb(77, 255, 143);
                                    NpcMobsGrid.Rows[item].Cells[2].Style.ForeColor = Color.White;
                                }
                            }
                            break;
                        }
                    case "Group_type":
                        {
                            int.TryParse(Group_type.Text, out IntValue);
                            foreach (var item in NpcRowCollection)
                            {
                                Read.NpcMobList[item].iGroupType = IntValue;
                            }
                            break;
                        }
                    case "ExistenceInitGen":
                        {
                            foreach (var item in NpcRowCollection)
                            {
                                Read.NpcMobList[item].BInitGen = Convert.ToByte(ExistenceInitGen.Checked);
                            }
                            break;
                        }
                    case "ExistenceAutoRevive":
                        {
                            foreach (var item in NpcRowCollection)
                            {
                                Read.NpcMobList[item].bAutoRevive = Convert.ToByte(ExistenceAutoRevive.Checked);
                            }
                            break;
                        }
                    case "BValicOnce":
                        {
                            foreach (var item in NpcRowCollection)
                            {
                                Read.NpcMobList[item].BValicOnce = Convert.ToByte(BValicOnce.Checked);
                            }
                            break;
                        }
                    case "dwGenId":
                        {
                            int.TryParse(dwGenId.Text, out IntValue);
                            foreach (var item in NpcRowCollection)
                            {
                                Read.NpcMobList[item].dwGenId = IntValue;
                            }
                            break;
                        }
                    case "Trigger":
                        {
                            int.TryParse(Trigger.Text, out IntValue);
                            foreach (var item in NpcRowCollection)
                            {
                                Read.NpcMobList[item].Trigger_id = IntValue;
                            }
                            break;
                        }
                    case "Life_time":
                        {
                            int.TryParse(Life_time.Text, out IntValue);
                            foreach (var item in NpcRowCollection)
                            {
                                Read.NpcMobList[item].Life_time = IntValue;
                            }
                            break;
                        }
                    case "IMaxNuml":
                        {
                            int.TryParse(IMaxNuml.Text, out IntValue);
                            foreach (var item in NpcRowCollection)
                            {
                                Read.NpcMobList[item].MaxRespawnTime = IntValue;
                            }
                            break;
                        }
                }
            }
        }
        private void NpcAndMobsDefault_EnterPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                NpcAndMobsDefaultLeave(sender, null);
        }
        private void UnderNpcAndMobs_Leave(object sender, EventArgs e)
        {
            if (NpcRowCollection != null && Read != null)
            {
                Control co = sender as Control;
                switch (co.Name)
                {
                    case "Id_numeric":
                        {
                            foreach (var Index in NpcRowCollection)
                            {
                                foreach (var UnderIndex in UnderNpcRowCollection)
                                {
                                    if (Read.NpcMobList[Index].Amount_in_group >= UnderIndex + 1)
                                    {
                                        Read.NpcMobList[Index].MobDops[UnderIndex].Id = Convert.ToInt32(Id_numeric.Value);
                                        NpcsGroupGrid.Rows[UnderIndex].Cells[1].Value = Convert.ToInt32(Id_numeric.Value);
                                        int ind = Element.ExistenceLists.FindIndex(c => c.Id == Convert.ToInt32(Id_numeric.Value));
                                        if (ind != -1)
                                            NpcsGroupGrid.Rows[UnderIndex].Cells[2].Value = Element.ExistenceLists[ind].Name;
                                        else
                                            NpcsGroupGrid.Rows[UnderIndex].Cells[2].Value = "?";
                                    }
                                }
                                RefreshRowNpcAndMobs(Index);
                            }
                            break;
                        }
                    case "Amount_numeric":
                        {
                            foreach (var item in NpcRowCollection)
                            {
                                foreach (var k in UnderNpcRowCollection)
                                {
                                    if (Read.NpcMobList[item].Amount_in_group >= k + 1)
                                    {
                                        Read.NpcMobList[item].MobDops[k].Amount = Convert.ToInt32(Amount_numeric.Value);
                                    }

                                }
                            }
                            break;
                        }
                    case "Respawn_numeric":
                        {
                            foreach (var item in NpcRowCollection)
                            {
                                foreach (var k in UnderNpcRowCollection)
                                {
                                    if (Read.NpcMobList[item].Amount_in_group >= k + 1)
                                    {
                                        Read.NpcMobList[item].MobDops[k].Respawn = Convert.ToInt32(Respawn_numeric.Value);
                                    }

                                }
                            }
                            break;
                        }
                    case "DeathAmount_numeric":
                        {
                            foreach (var item in NpcRowCollection)
                            {
                                foreach (var k in UnderNpcRowCollection)
                                {
                                    if (Read.NpcMobList[item].Amount_in_group >= k + 1)
                                    {
                                        Read.NpcMobList[item].MobDops[k].Dead_amount = Convert.ToInt32(DeathAmount_numeric.Value);
                                    }

                                }
                            }
                            break;
                        }
                    case "Agression":
                        {
                            foreach (var item in NpcRowCollection)
                            {
                                foreach (var k in UnderNpcRowCollection)
                                {
                                    if (Read.NpcMobList[item].Amount_in_group >= k + 1)
                                    {
                                        Read.NpcMobList[item].MobDops[k].Agression = Agression.SelectedIndex;
                                    }

                                }
                            }
                            break;
                        }
                    case "Path_type":
                        {
                            foreach (var item in NpcRowCollection)
                            {
                                foreach (var k in UnderNpcRowCollection)
                                {
                                    if (Read.NpcMobList[item].Amount_in_group >= k + 1)
                                    {
                                        Read.NpcMobList[item].MobDops[k].Path_type = Path_type.SelectedIndex;
                                    }

                                }
                            }
                            break;
                        }
                    case "Path_speed":
                        {
                            foreach (var item in NpcRowCollection)
                            {
                                foreach (var k in UnderNpcRowCollection)
                                {
                                    if (Read.NpcMobList[item].Amount_in_group >= k + 1)
                                    {
                                        Read.NpcMobList[item].MobDops[k].Speed = Convert.ToInt32(Path_speed.Value);
                                    }

                                }
                            }
                            break;

                        }
                    case "Path_numeric":
                        {
                            foreach (var item in NpcRowCollection)
                            {
                                foreach (var k in UnderNpcRowCollection)
                                {
                                    if (Read.NpcMobList[item].Amount_in_group >= k + 1)
                                    {
                                        Read.NpcMobList[item].MobDops[k].Path = Convert.ToInt32(Path_numeric.Value);
                                    }

                                }
                            }
                            break;

                        }
                    case "Water_numeric":
                        {
                            foreach (var item in NpcRowCollection)
                            {
                                foreach (var k in UnderNpcRowCollection)
                                {
                                    if (Read.NpcMobList[item].Amount_in_group >= k + 1)
                                    {
                                        Read.NpcMobList[item].MobDops[k].fOffsetWater = Convert.ToSingle(Water_numeric.Value);
                                    }

                                }
                            }
                            break;
                        }
                    case "Turn_numeric":
                        {
                            foreach (var item in NpcRowCollection)
                            {
                                foreach (var k in UnderNpcRowCollection)
                                {
                                    if (Read.NpcMobList[item].Amount_in_group >= k + 1)
                                    {
                                        Read.NpcMobList[item].MobDops[k].fOffsetTrn = Convert.ToSingle(Turn_numeric.Value);
                                    }

                                }
                            }
                            break;
                        }
                    case "Group_numeric":
                        {
                            foreach (var item in NpcRowCollection)
                            {
                                foreach (var k in UnderNpcRowCollection)
                                {
                                    if (Read.NpcMobList[item].Amount_in_group >= k + 1)
                                    {
                                        Read.NpcMobList[item].MobDops[k].Group = Convert.ToInt32(Group_numeric.Value);
                                    }

                                }
                            }
                            break;
                        }
                    case "AskHelp_numeric":
                        {
                            foreach (var item in NpcRowCollection)
                            {
                                foreach (var k in UnderNpcRowCollection)
                                {
                                    if (Read.NpcMobList[item].Amount_in_group >= k + 1)
                                    {
                                        Read.NpcMobList[item].MobDops[k].Group_help_sender = Convert.ToInt32(AskHelp_numeric.Value);
                                    }

                                }
                            }
                            break;
                        }
                    case "NeedHelp_numeric":
                        {
                            foreach (var item in NpcRowCollection)
                            {
                                foreach (var k in UnderNpcRowCollection)
                                {
                                    if (Read.NpcMobList[item].Amount_in_group >= k + 1)
                                    {
                                        Read.NpcMobList[item].MobDops[k].Group_help_Needer = Convert.ToInt32(NeedHelp_numeric.Value);
                                    }
                                }
                            }
                            break;
                        }
                    case "bNeedHelp":
                        {
                            foreach (var item in NpcRowCollection)
                            {
                                foreach (var k in UnderNpcRowCollection)
                                {
                                    if (Read.NpcMobList[item].Amount_in_group >= k + 1)
                                    {
                                        Read.NpcMobList[item].MobDops[k].bNeedHelp = Convert.ToByte(bNeedHelp.Checked);
                                    }
                                }
                            }
                            break;
                        }
                    case "bFac_Accept":
                        {
                            foreach (var item in NpcRowCollection)
                            {
                                foreach (var k in UnderNpcRowCollection)
                                {
                                    if (Read.NpcMobList[item].Amount_in_group >= k + 1)
                                    {
                                        Read.NpcMobList[item].MobDops[k].bFac_Accept = Convert.ToByte(bFac_Accept.Checked);
                                    }
                                }
                            }
                            break;
                        }
                    case "bFac_Helper":
                        {
                            foreach (var item in NpcRowCollection)
                            {
                                foreach (var k in UnderNpcRowCollection)
                                {
                                    if (Read.NpcMobList[item].Amount_in_group >= k + 1)
                                    {
                                        Read.NpcMobList[item].MobDops[k].bFac_Helper = Convert.ToByte(bFac_Helper.Checked);
                                    }
                                }
                            }
                            break;
                        }
                    case "bFaction":
                        {
                            foreach (var item in NpcRowCollection)
                            {
                                foreach (var k in UnderNpcRowCollection)
                                {
                                    if (Read.NpcMobList[item].Amount_in_group >= k + 1)
                                    {
                                        Read.NpcMobList[item].MobDops[k].bFaction = Convert.ToByte(bFaction.Checked);
                                    }
                                }
                            }
                            break;
                        }
                    case "Deadtime_numeric":
                        {
                            foreach (var item in NpcRowCollection)
                            {
                                foreach (var k in UnderNpcRowCollection)
                                {
                                    if (Read.NpcMobList[item].Amount_in_group >= k + 1)
                                    {
                                        Read.NpcMobList[item].MobDops[k].Dead_time = Convert.ToInt32(Deadtime_numeric.Value);
                                    }
                                }
                            }
                            break;
                        }
                    case "RefreshLower_numeric":
                        {
                            foreach (var item in NpcRowCollection)
                            {
                                foreach (var k in UnderNpcRowCollection)
                                {
                                    if (Read.NpcMobList[item].Amount_in_group >= k + 1)
                                    {
                                        Read.NpcMobList[item].MobDops[k].RefreshLower = Convert.ToInt32(RefreshLower_numeric.Value);
                                    }
                                }
                            }
                            break;
                        }
                }
            }
        }
        private void UnderNpcAndMobs_EnterPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                UnderNpcAndMobs_Leave(sender, null);
        }
        private void CloneNpcAndMobFull(object sender, EventArgs e)
        {
            if (NpcMobsGrid.Rows.Count > 0)
            {
                NpcMobsGrid.ScrollBars = ScrollBars.None;
                NpcRowCollection = NpcRowCollection.OrderBy(z => z).ToList();
                foreach (int k in NpcRowCollection)
                {
                    Read.NpcMobsAmount++;
                    ClassDefaultMonsters mn = new ClassDefaultMonsters()
                    {
                        Amount_in_group = Read.NpcMobList[k].Amount_in_group,
                        bAutoRevive = Read.NpcMobList[k].bAutoRevive,
                        BInitGen = Read.NpcMobList[k].BInitGen,
                        BValicOnce = Read.NpcMobList[k].BValicOnce,
                        dwGenId = Read.NpcMobList[k].dwGenId,
                        iGroupType = Read.NpcMobList[k].iGroupType,
                        MaxRespawnTime = Read.NpcMobList[k].MaxRespawnTime,
                        Type = Read.NpcMobList[k].Type,
                        Life_time = Read.NpcMobList[k].Life_time,
                        Trigger_id = Read.NpcMobList[k].Trigger_id,
                        Location = Read.NpcMobList[k].Location,
                        X_direction = Read.NpcMobList[k].X_direction,
                        X_position = Read.NpcMobList[k].X_position,
                        X_random = Read.NpcMobList[k].X_random,
                        Y_direction = Read.NpcMobList[k].Y_direction,
                        Y_position = Read.NpcMobList[k].Y_position,
                        Y_random = Read.NpcMobList[k].Y_random,
                        Z_direction = Read.NpcMobList[k].Z_direction,
                        Z_position = Read.NpcMobList[k].Z_position,
                        Z_random = Read.NpcMobList[k].Z_random,
                        MobDops = new List<ClassExtraMonsters>()
                    };
                    for (int i = 0; i < Read.NpcMobList[k].Amount_in_group; i++)
                    {
                        ClassExtraMonsters ex = new ClassExtraMonsters()
                        {
                            Agression = Read.NpcMobList[k].MobDops[i].Agression,
                            Amount = Read.NpcMobList[k].MobDops[i].Amount,
                            bFac_Accept = Read.NpcMobList[k].MobDops[i].bFac_Accept,
                            bFac_Helper = Read.NpcMobList[k].MobDops[i].bFac_Helper,
                            bFaction = Read.NpcMobList[k].MobDops[i].bFaction,
                            bNeedHelp = Read.NpcMobList[k].MobDops[i].bNeedHelp,
                            Dead_amount = Read.NpcMobList[k].MobDops[i].Dead_amount,
                            Dead_time = Read.NpcMobList[k].MobDops[i].Dead_time,
                            fOffsetTrn = Read.NpcMobList[k].MobDops[i].fOffsetTrn,
                            fOffsetWater = Read.NpcMobList[k].MobDops[i].fOffsetWater,
                            Group = Read.NpcMobList[k].MobDops[i].Group,
                            Group_help_Needer = Read.NpcMobList[k].MobDops[i].Group_help_Needer,
                            Group_help_sender = Read.NpcMobList[k].MobDops[i].Group_help_sender,
                            Id = Read.NpcMobList[k].MobDops[i].Id,
                            Path = Read.NpcMobList[k].MobDops[i].Path,
                            Path_type = Read.NpcMobList[k].MobDops[i].Path_type,
                            RefreshLower = Read.NpcMobList[k].MobDops[i].RefreshLower,
                            Respawn = Read.NpcMobList[k].MobDops[i].Respawn,
                            Speed = Read.NpcMobList[k].MobDops[i].Speed
                        };
                        mn.MobDops.Add(ex);
                    }
                    Read.NpcMobList.Add(mn);
                    NpcMobsGrid.Rows.Add(NpcMobsGrid.Rows.Count, NpcMobsGrid.Rows[k].Cells[1].Value, NpcMobsGrid.Rows[k].Cells[2].Value);
                }
                NpcMobsGrid.ScrollBars = ScrollBars.Vertical;
                var RowsClone = NpcRowCollection;
                NpcMobsGrid.CurrentCell = NpcMobsGrid.Rows[NpcMobsGrid.Rows.Count - 1].Cells[1];
                for (int i = 1; i <= RowsClone.Count; i++)
                {
                    NpcMobsGrid.Rows[NpcMobsGrid.Rows.Count - i].Selected = true;
                }
                ExistenceGrid_CellChanged(null, null);
            }
            else
            {
                if (Read != null)
                {
                    Read.NpcMobsAmount++;
                    ClassDefaultMonsters mb = new ClassDefaultMonsters()
                    {
                        Location = 0,
                        Type = 0,
                        Amount_in_group = 1,
                        MobDops = new List<ClassExtraMonsters>()
                    };
                    ClassExtraMonsters me = new ClassExtraMonsters()
                    {
                        Id = 16,
                        Amount = 1,
                        Respawn = 30
                    };
                    mb.MobDops.Add(me);
                    Read.NpcMobList.Add(mb);
                    if (Language == 1)
                        NpcMobsGrid.Rows.Add(1, 16, "Зеленый мотыль");
                    else
                        NpcMobsGrid.Rows.Add(1, 16, "Green WaterBeetle");
                    ExistenceGrid_CellChanged(null, null);
                }
            }
            if (Language == 1)
            {
                ExistenceTab.Text = string.Format("Мобы и Нипы" + " {0}/{1}", NpcRowCollection.Count, Read.NpcMobsAmount);
            }
            else
            {
                ExistenceTab.Text = string.Format("Mobs and Npcs" + " {0}/{1}", NpcRowCollection.Count, Read.NpcMobsAmount);
            }
        }
        private void RemoveNpcAndMobFull(object sender, EventArgs e)
        {
            if (Read != null && NpcRowCollection.Count != 0)
            {
                string Dialog = "Вы уверены,что хотите удалить выбранные объекты?";
                if (Language == 2)
                    Dialog = "Are you sure that you want to delete selected objects?";
                DialogResult dg = MessageBox.Show(Dialog, "Npcgen Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dg == DialogResult.Yes)
                {
                    int IndexBeforeRemoving = NpcRowCollection.Min();
                    NpcMobsGrid.ScrollBars = ScrollBars.None;
                    ErrorsGrid.ScrollBars = ScrollBars.None;
                    AllowCellChanging = false;
                    NpcMobsGrid.ClearSelection();
                    MainProgressBar.Maximum = NpcRowCollection.Count;
                    Read.NpcMobsAmount -= NpcRowCollection.Count;
                    foreach (int i in NpcRowCollection)
                    {
                        var Matched = ErrorExistenceCollection.Where(f => f.GridIndex == i).OrderByDescending(f => f.ErrorInex).ToList();
                        foreach (var item in Matched)
                        {
                            ErrorsGrid.Rows.RemoveAt(item.ErrorInex);
                            ErrorExistenceCollection.RemoveAt(item.ErrorInex);
                        }
                        ErrorExistenceCollection.Where(b => b.GridIndex > i).ToList().ForEach(s => s.ErrorInex -= Matched.Count);
                        ErrorExistenceCollection.Where(a => a.GridIndex > i).ToList().ForEach(s => s.GridIndex--);
                        ErrorResourcesCollection.ForEach(s => s.ErrorInex -= Matched.Count);
                        ErrorDynamicsCollection.ForEach(s => s.ErrorInex -= Matched.Count);
                        Read.NpcMobList.RemoveAt(i);
                        NpcMobsGrid.Rows.RemoveAt(i);
                        MainProgressBar.Value++;
                    }
                    if (ErrorsGrid.Rows.Count != 0)
                    {
                        for (int i = 0; i < ErrorExistenceCollection.Count; i++)
                        {
                            ErrorsGrid.Rows[i].Cells[0].Value = i + 1;
                            ErrorsGrid.Rows[i].Cells[1].Value = ErrorExistenceCollection[i].GridIndex + 1;
                        }
                        int Amount = (ErrorResourcesCollection.Count != 0) ? ErrorResourcesCollection.Min(f => f.ErrorInex) : 0;
                        for (int i = Amount; i <= Amount; i++)
                        {
                            ErrorsGrid.Rows[i].Cells[0].Value = i + 1;
                        }
                        int Amount1 = (ErrorDynamicsCollection.Count != 0) ? ErrorDynamicsCollection.Min(f => f.ErrorInex) : 0;
                        for (int i = Amount1; i <= Amount1; i++)
                        {
                            ErrorsGrid.Rows[i].Cells[0].Value = i + 1;
                        }
                    }
                    AllowCellChanging = true;
                    MainProgressBar.Value = 0;
                    NpcMobsGrid.ScrollBars = ScrollBars.Vertical;
                    ErrorsGrid.ScrollBars = ScrollBars.Vertical;
                    if (NpcMobsGrid.Rows.Count > IndexBeforeRemoving)
                    {
                        NpcMobsGrid.CurrentCell = NpcMobsGrid.Rows[IndexBeforeRemoving].Cells[1];
                        NpcMobsGrid.FirstDisplayedScrollingRowIndex = IndexBeforeRemoving;
                    }
                    else if (NpcMobsGrid.Rows.Count != 0)
                    {
                        NpcMobsGrid.CurrentCell = NpcMobsGrid.Rows[NpcMobsGrid.Rows.Count - 1].Cells[1];
                        NpcMobsGrid.FirstDisplayedScrollingRowIndex = NpcMobsGrid.Rows.Count - 1;
                    }
                    ExistenceGrid_CellChanged(null, null);
                    for (int i = 0; i < NpcMobsGrid.Rows.Count; i++)
                    {
                        NpcMobsGrid.Rows[i].Cells[0].Value = i + 1;
                    }
                    if (Language == 1)
                    {
                        ExistenceTab.Text = string.Format("Мобы и Нипы" + " 1/{0}", Read.NpcMobsAmount);
                    }
                    else
                    {
                        ExistenceTab.Text = string.Format("Mobs and Npcs" + " 1/{0}", Read.NpcMobsAmount);
                    }
                }
            }
        }
        private void Id_numeric_DoubleClick(object sender, EventArgs e)
        {
            if (ChooseFromElementsForm != null)
            {
                ChooseFromElementsForm.SetAction = 1;
                ChooseFromElementsForm.SetWindow = 1;
                int Ind = Element.ExistenceLists.FindIndex(z => z.Id == Convert.ToInt32(Id_numeric.Value));
                if (Ind != -1)
                {
                    if (Ind >= Element.MonsterdAmount)
                    {
                        ChooseFromElementsForm.FindRow(Ind - Element.MonsterdAmount, "Npc");
                    }
                    else
                    {
                        ChooseFromElementsForm.FindRow(Ind, "Mob");
                    }
                }
                else
                {
                    if (Language == 1)
                        MessageBox.Show("ID не найдено в elements.data!!...", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if (Language == 2)
                        MessageBox.Show("ID not found in elements.data!!...", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                ChooseFromElementsForm.ShowDialog(this);
            }
        }
        private void CloneNpcinGroupButton_Click(object sender, EventArgs e)
        {
            if (NpcMobsGrid.Rows.Count > 0 && NpcsGroupGrid.Rows.Count > 0)
            {
                UnderNpcRowCollection = UnderNpcRowCollection.OrderBy(z => z).ToList();
                foreach (int Item in UnderNpcRowCollection)
                {
                    Read.NpcMobList[NpcRowIndex].Amount_in_group++;
                    ClassExtraMonsters ex = new ClassExtraMonsters()
                    {
                        Agression = Read.NpcMobList[NpcRowIndex].MobDops[Item].Agression,
                        Amount = Read.NpcMobList[NpcRowIndex].MobDops[Item].Agression,
                        bFac_Accept = Read.NpcMobList[NpcRowIndex].MobDops[Item].bFac_Accept,
                        bFac_Helper = Read.NpcMobList[NpcRowIndex].MobDops[Item].bFac_Helper,
                        bFaction = Read.NpcMobList[NpcRowIndex].MobDops[Item].bFaction,
                        bNeedHelp = Read.NpcMobList[NpcRowIndex].MobDops[Item].bNeedHelp,
                        Dead_amount = Read.NpcMobList[NpcRowIndex].MobDops[Item].Dead_amount,
                        Dead_time = Read.NpcMobList[NpcRowIndex].MobDops[Item].Dead_time,
                        fOffsetTrn = Read.NpcMobList[NpcRowIndex].MobDops[Item].fOffsetTrn,
                        fOffsetWater = Read.NpcMobList[NpcRowIndex].MobDops[Item].fOffsetWater,
                        Group = Read.NpcMobList[NpcRowIndex].MobDops[Item].Group,
                        Group_help_Needer = Read.NpcMobList[NpcRowIndex].MobDops[Item].Group_help_Needer,
                        Group_help_sender = Read.NpcMobList[NpcRowIndex].MobDops[Item].Group_help_sender,
                        Id = Read.NpcMobList[NpcRowIndex].MobDops[Item].Id,
                        Path = Read.NpcMobList[NpcRowIndex].MobDops[Item].Path,
                        Path_type = Read.NpcMobList[NpcRowIndex].MobDops[Item].Path_type,
                        RefreshLower = Read.NpcMobList[NpcRowIndex].MobDops[Item].RefreshLower,
                        Respawn = Read.NpcMobList[NpcRowIndex].MobDops[Item].Respawn,
                        Speed = Read.NpcMobList[NpcRowIndex].MobDops[Item].Speed
                    };
                    Read.NpcMobList[NpcRowIndex].MobDops.Add(ex);
                    NpcsGroupGrid.Rows.Add(Read.NpcMobList[NpcRowIndex].Amount_in_group, NpcsGroupGrid.Rows[Item].Cells[1].Value, NpcsGroupGrid.Rows[Item].Cells[2].Value);
                }
                RefreshRowNpcAndMobs(NpcRowIndex);
                NpcsGroupGrid.ClearSelection();
                for (int z = 1; z <= UnderNpcRowCollection.Count; z++)
                {
                    NpcsGroupGrid.Rows[NpcsGroupGrid.Rows.Count - z].Selected = true;
                }
                NpcsGroupGrid.CurrentCell = NpcsGroupGrid.Rows[NpcsGroupGrid.Rows.Count - 1].Cells[1];
                NpcsGroupGrid.FirstDisplayedScrollingRowIndex = NpcsGroupGrid.Rows.Count - 1;
            }
            else
            {
                ClassExtraMonsters ex = new ClassExtraMonsters();
                Read.NpcMobList[NpcRowIndex].Amount_in_group++;
                ex.Id = 16;
                ex.Amount = 1;
                ex.Respawn = 60;
                Read.NpcMobList[NpcRowIndex].MobDops.Add(ex);
                if (Language == 1)
                    NpcsGroupGrid.Rows.Add(1, 16, "Зеленый мотыль");
                else if (Language == 2)
                    NpcsGroupGrid.Rows.Add(1, 16, "Green WaterBeetle");
                RefreshRowNpcAndMobs(NpcRowIndex);
                UnderExistenceGrid_CellChanged(null, null);
            }
        }
        private void DeleteNpcinGroupButton_Click(object sender, EventArgs e)
        {
            if (UnderNpcRowCollection != null && NpcMobsGrid.Rows.Count > 0 && NpcsGroupGrid.Rows.Count > 0)
            {
                string Dialog = "Вы уверены,что хотите удалить выбранные объекты?";
                if (Language == 2)
                    Dialog = "Are you sure that you want to delete selected objects?";
                DialogResult dg = MessageBox.Show(Dialog, "Npcgen Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dg == DialogResult.Yes)
                {
                    UnderNpcRowCollection = UnderNpcRowCollection.OrderByDescending(z => z).ToList();
                    int IndexBeforeRemoving = UnderNpcRowCollection.Min();
                    NpcsGroupGrid.ClearSelection();
                    foreach (var item in UnderNpcRowCollection)
                    {
                        Read.NpcMobList[NpcRowIndex].Amount_in_group--;
                        Read.NpcMobList[NpcRowIndex].MobDops.RemoveAt(item);
                        NpcsGroupGrid.Rows.RemoveAt(item);
                    }
                    RefreshRowNpcAndMobs(NpcRowIndex);
                    if (NpcsGroupGrid.Rows.Count > IndexBeforeRemoving)
                    {
                        NpcsGroupGrid.CurrentCell = NpcsGroupGrid.Rows[IndexBeforeRemoving].Cells[1];
                        NpcsGroupGrid.FirstDisplayedScrollingRowIndex = IndexBeforeRemoving;
                    }
                    else if (NpcsGroupGrid.Rows.Count != 0)
                    {
                        NpcsGroupGrid.CurrentCell = NpcsGroupGrid.Rows[NpcsGroupGrid.Rows.Count - 1].Cells[1];
                        NpcsGroupGrid.FirstDisplayedScrollingRowIndex = NpcsGroupGrid.Rows.Count - 1;
                    }
                    UnderExistenceGrid_CellChanged(null, null);
                }
            }
        }
        public void RefreshRowNpcAndMobs(int index)
        {
            int[] Id_joined = new int[Read.NpcMobList[index].Amount_in_group];
            string[] Names_joined = new string[Read.NpcMobList[index].Amount_in_group];
            for (int j = 0; j < Read.NpcMobList[index].Amount_in_group; j++)
            {
                Id_joined[j] = Read.NpcMobList[index].MobDops[j].Id;
                int ind = Element.ExistenceLists.FindIndex(v => v.Id == Id_joined[j]);
                if (ind != -1)
                {
                    Names_joined[j] = Element.ExistenceLists[ind].Name;
                }
                else
                {
                    Names_joined[j] = "?";
                }
            }
            NpcMobsGrid.Rows[index].Cells[1].Value = string.Join(",", Id_joined);
            NpcMobsGrid.Rows[index].Cells[2].Value = string.Join(",", Names_joined);
        }
        private void InsertCordsFromGame_Click(object sender, EventArgs e)
        {
            ClassPosition cs = GetCoordinates();
            if (cs != null)
            {
                X_position.Text = cs.PosX.ToString();
                Y_position.Text = cs.PosY.ToString();
                Z_position.Text = cs.PosZ.ToString();
                X_rotate.Text = cs.DirX.ToString();
                Y_rotate.Text = cs.DirY.ToString();
                Z_rotate.Text = cs.DirZ.ToString();
                NpcAndMobsDefaultLeave(X_position, null);
                NpcAndMobsDefaultLeave(Y_position, null);
                NpcAndMobsDefaultLeave(Z_position, null);
                NpcAndMobsDefaultLeave(X_rotate, null);
                NpcAndMobsDefaultLeave(Y_rotate, null);
                NpcAndMobsDefaultLeave(Z_rotate, null);
            }
        }
        #endregion
        #region Resources
        private void ResourcesGrid_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > 0 & e.RowIndex >= 0)
            {
                string j = Convert.ToString(ResourcesGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                toolTip1.SetToolTip(ResourcesGrid, j);
            }
        }
        private void ResourcesGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            if (AllowCellChanging == true)
            {
                ResourcesRowCollection = ResourcesGrid.SelectedRows.Cast<DataGridViewRow>().Select(f => f.Index).OrderByDescending(t => t).ToList();
                if (ResourcesGrid.CurrentRow != null)
                {
                    ResourcesRowIndex = ResourcesGrid.CurrentRow.Index;
                    if (ResourcesRowIndex != -1)
                    {
                        RX_position.Text = Read.ResourcesList[ResourcesRowIndex].X_position.ToString();
                        RY_position.Text = Read.ResourcesList[ResourcesRowIndex].Y_position.ToString();
                        RZ_position.Text = Read.ResourcesList[ResourcesRowIndex].Z_position.ToString();
                        RX_Random.Text = Read.ResourcesList[ResourcesRowIndex].X_Random.ToString();
                        RZ_Random.Text = Read.ResourcesList[ResourcesRowIndex].Z_Random.ToString();
                        RInCline1.Text = Read.ResourcesList[ResourcesRowIndex].InCline1.ToString();
                        RInCline2.Text = Read.ResourcesList[ResourcesRowIndex].InCline2.ToString();
                        RRotation.Text = Read.ResourcesList[ResourcesRowIndex].Rotation.ToString();
                        RGroup_amount_textbox.Text = Read.ResourcesList[ResourcesRowIndex].Amount_in_group.ToString();
                        RdwGenID.Text = Read.ResourcesList[ResourcesRowIndex].dwGenID.ToString();
                        RTriggerID.Text = Read.ResourcesList[ResourcesRowIndex].Trigger_id.ToString();
                        RIMaxNuml.Text = Read.ResourcesList[ResourcesRowIndex].IMaxNum.ToString();
                        ResourcesInitGen.Checked = Convert.ToBoolean(Read.ResourcesList[ResourcesRowIndex].bInitGen);
                        ResourcesAutoRevive.Checked = Convert.ToBoolean(Read.ResourcesList[ResourcesRowIndex].bAutoRevive);
                        RBValidOnce.Checked = Convert.ToBoolean(Read.ResourcesList[ResourcesRowIndex].bValidOnce);
                        ResourcesGroupGrid.Rows.Clear();
                        for (int i = 0; i < Read.ResourcesList[ResourcesRowIndex].Amount_in_group; i++)
                        {
                            string Name = "?";
                            int Ind = Element.ResourcesList.FindIndex(v => v.Id == Read.ResourcesList[ResourcesRowIndex].ResExtra[i].Id);
                            if (Ind != -1)
                                Name = Element.ResourcesList[Ind].Name;
                            ResourcesGroupGrid.Rows.Add(i + 1, Read.ResourcesList[ResourcesRowIndex].ResExtra[i].Id, Name);
                        }
                        if (MapForm != null && MainProgressBar.Value == 0 && ResourcesRowCollection.Count != 0 && MapForm.Visible == true)
                        {
                            MapForm.GetCoordinates(GetPoint(2));
                        }
                        ResourcesGroupGrid_CurrentCellChanged(null, null);
                    }
                }
            }
            if (Language == 1)
            {
                ResourcesTab.Text = string.Format("Ресурсы" + " {0}/{1}", ResourcesRowCollection.Count, Read.ResourcesAmount);
            }
            else
            {
                ResourcesTab.Text = string.Format("Resources" + " {0}/{1}", ResourcesRowCollection.Count, Read.ResourcesAmount);
            }
        }
        private void ResourcesGroupGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            if (ResourcesGroupGrid.CurrentRow != null)
            {
                if (ResourcesGroupGrid.CurrentRow.Index != -1)
                {
                    UnderResourcesRowCollection = new List<int>(ResourcesGroupGrid.SelectedRows.Count);
                    foreach (DataGridViewRow item in ResourcesGroupGrid.SelectedRows)
                    {
                        UnderResourcesRowCollection.Add(item.Index);
                    }
                    ResourcesGroupIndex = ResourcesGroupGrid.CurrentRow.Index;
                    RId_numeric.Value = Read.ResourcesList[ResourcesRowIndex].ResExtra[ResourcesGroupIndex].Id;
                    RAmount_numeric.Value = Read.ResourcesList[ResourcesRowIndex].ResExtra[ResourcesGroupIndex].Amount;
                    RRespawn_numeric.Value = Read.ResourcesList[ResourcesRowIndex].ResExtra[ResourcesGroupIndex].Respawntime;
                    RType_numeric.Value = Read.ResourcesList[ResourcesRowIndex].ResExtra[ResourcesGroupIndex].ResourceType;
                    RfHeiOff_numeric.Value = Convert.ToDecimal(Read.ResourcesList[ResourcesRowIndex].ResExtra[ResourcesGroupIndex].fHeiOff);
                }
            }
        }
        private void ResourcesDefault_EnterPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ResourcesDefaultLeave(sender, null);
        }
        private void ResourcesDefaultLeave(object sender, EventArgs e)
        {
            Control co = sender as Control;
            float FloatValue;
            int IntValue;
            byte ByteValue;
            switch (co.Name)
            {
                case "RX_position":
                    {
                        float.TryParse(RX_position.Text, out FloatValue);
                        foreach (var item in ResourcesRowCollection)
                        {
                            Read.ResourcesList[item].X_position = FloatValue;
                        }
                        break;
                    }
                case "RY_position":
                    {
                        float.TryParse(RY_position.Text, out FloatValue);
                        foreach (var item in ResourcesRowCollection)
                        {
                            Read.ResourcesList[item].Y_position = FloatValue;
                        }
                        break;
                    }
                case "RZ_position":
                    {
                        float.TryParse(RZ_position.Text, out FloatValue);
                        foreach (var item in ResourcesRowCollection)
                        {
                            Read.ResourcesList[item].Z_position = FloatValue;
                        }
                        break;
                    }
                case "RX_Random":
                    {
                        float.TryParse(RX_Random.Text, out FloatValue);
                        foreach (var item in ResourcesRowCollection)
                        {
                            Read.ResourcesList[item].X_Random = FloatValue;
                        }
                        break;
                    }
                case "RZ_Random":
                    {
                        float.TryParse(RZ_Random.Text, out FloatValue);
                        foreach (var item in ResourcesRowCollection)
                        {
                            Read.ResourcesList[item].Z_Random = FloatValue;
                        }
                        break;
                    }
                case "RInCline1":
                    {
                        Byte.TryParse(RInCline1.Text, out ByteValue);
                        foreach (var item in ResourcesRowCollection)
                        {
                            Read.ResourcesList[item].InCline1 = ByteValue;
                        }
                        break;
                    }
                case "RInCline2":
                    {
                        Byte.TryParse(RInCline2.Text, out ByteValue);
                        foreach (var item in ResourcesRowCollection)
                        {
                            Read.ResourcesList[item].InCline2 = ByteValue;
                        }
                        break;
                    }
                case "RRotation":
                    {
                        Byte.TryParse(RRotation.Text, out ByteValue);
                        foreach (var item in ResourcesRowCollection)
                        {
                            Read.ResourcesList[item].Rotation = ByteValue;
                        }
                        break;
                    }
                case "RdwGenID":
                    {
                        int.TryParse(RdwGenID.Text, out IntValue);
                        foreach (var item in ResourcesRowCollection)
                        {
                            Read.ResourcesList[item].dwGenID = IntValue;
                        }
                        break;
                    }
                case "RTriggerID":
                    {
                        int.TryParse(RTriggerID.Text, out IntValue);
                        foreach (var item in ResourcesRowCollection)
                        {
                            Read.ResourcesList[item].Trigger_id = IntValue;
                        }
                        break;
                    }
                case "RIMaxNuml":
                    {
                        int.TryParse(RIMaxNuml.Text, out IntValue);
                        foreach (var item in ResourcesRowCollection)
                        {
                            Read.ResourcesList[item].IMaxNum = IntValue;
                        }
                        break;
                    }
                case "ResourcesInitGen":
                    {
                        foreach (var item in ResourcesRowCollection)
                        {
                            Read.ResourcesList[item].bInitGen = Convert.ToByte(ResourcesInitGen.Checked);
                        }
                        break;
                    }
                case "ResourcesAutoRevive":
                    {
                        foreach (var item in ResourcesRowCollection)
                        {
                            Read.ResourcesList[item].bAutoRevive = Convert.ToByte(ResourcesAutoRevive.Checked);
                        }
                        break;
                    }
                case "RBValidOnce":
                    {
                        foreach (var item in ResourcesRowCollection)
                        {
                            Read.ResourcesList[item].bValidOnce = Convert.ToByte(RBValidOnce.Checked);
                        }
                        break;
                    }
            }
        }
        private void UnderResources_EnterPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                UnderResourcesLeave(sender, null);
        }
        private void UnderResourcesLeave(object sender, EventArgs e)
        {
            Control co = sender as Control;
            if (UnderResourcesRowCollection != null)
            {
                switch (co.Name)
                {
                    case "RId_numeric":
                        {
                            foreach (var Index in ResourcesRowCollection)
                            {
                                foreach (var UnderIndex in UnderResourcesRowCollection)
                                {
                                    if (Read.ResourcesList[Index].Amount_in_group >= UnderIndex + 1)
                                    {
                                        Read.ResourcesList[Index].ResExtra[UnderIndex].Id = Convert.ToInt32(RId_numeric.Value);
                                        ResourcesGroupGrid.Rows[UnderIndex].Cells[1].Value = Convert.ToInt32(RId_numeric.Value);
                                        int ind = Element.ResourcesList.FindIndex(c => c.Id == Convert.ToInt32(RId_numeric.Value));
                                        if (ind != -1)
                                            ResourcesGroupGrid.Rows[UnderIndex].Cells[2].Value = Element.ResourcesList[ind].Name;
                                        else
                                            ResourcesGroupGrid.Rows[UnderIndex].Cells[2].Value = "?";
                                    }
                                }
                                RefreshResourcesRow(Index);
                            }
                            break;
                        }
                    case "RAmount_numeric":
                        {
                            foreach (var item in ResourcesRowCollection)
                            {
                                foreach (var k in UnderResourcesRowCollection)
                                {
                                    if (Read.ResourcesList[item].Amount_in_group >= k + 1)
                                    {
                                        Read.ResourcesList[item].ResExtra[k].Amount = Convert.ToInt32(RAmount_numeric.Value);
                                    }
                                }
                            }
                            break;
                        }
                    case "RRespawn_numeric":
                        {
                            foreach (var item in ResourcesRowCollection)
                            {
                                foreach (var k in UnderResourcesRowCollection)
                                {
                                    if (Read.ResourcesList[item].Amount_in_group >= k + 1)
                                    {
                                        Read.ResourcesList[item].ResExtra[k].Respawntime = Convert.ToInt32(RRespawn_numeric.Value);
                                    }
                                }
                            }
                            break;
                        }
                    case "RType_numeric":
                        {
                            foreach (var item in ResourcesRowCollection)
                            {
                                foreach (var k in UnderResourcesRowCollection)
                                {
                                    if (Read.ResourcesList[item].Amount_in_group >= k + 1)
                                    {
                                        Read.ResourcesList[item].ResExtra[k].ResourceType = Convert.ToInt32(RType_numeric.Value);
                                    }
                                }
                            }
                            break;
                        }
                    case "RfHeiOff_numeric":
                        {
                            foreach (var item in ResourcesRowCollection)
                            {
                                foreach (var k in UnderResourcesRowCollection)
                                {
                                    if (Read.ResourcesList[item].Amount_in_group >= k + 1)
                                    {
                                        Read.ResourcesList[item].ResExtra[k].fHeiOff = Convert.ToSingle(RfHeiOff_numeric.Value);
                                    }
                                }
                            }
                            break;
                        }
                }
            }
        }
        public void RefreshResourcesRow(int index)
        {
            int[] Id_joined = new int[Read.ResourcesList[index].Amount_in_group];
            string[] Names_joined = new string[Read.ResourcesList[index].Amount_in_group];
            for (int j = 0; j < Read.ResourcesList[index].Amount_in_group; j++)
            {
                Id_joined[j] = Read.ResourcesList[index].ResExtra[j].Id;
                int ind = Element.ResourcesList.FindIndex(v => v.Id == Id_joined[j]);
                if (ind != -1)
                {
                    Names_joined[j] = Element.ResourcesList[ind].Name;
                }
                else
                {
                    Names_joined[j] = "?";
                }
            }
            ResourcesGrid.Rows[index].Cells[1].Value = string.Join(",", Id_joined);
            ResourcesGrid.Rows[index].Cells[2].Value = string.Join(",", Names_joined);
        }
        private void CloneResurcesFull_Click(object sender, EventArgs e)
        {
            if (ResourcesGrid.Rows.Count > 0)
            {
                ResourcesGrid.ScrollBars = ScrollBars.None;
                ResourcesRowCollection = ResourcesRowCollection.OrderBy(z => z).ToList();
                foreach (int k in ResourcesRowCollection)
                {
                    Read.ResourcesAmount++;
                    ClassDefaultResources mn = new ClassDefaultResources()
                    {
                        Amount_in_group = Read.ResourcesList[k].Amount_in_group,
                        X_position = Read.ResourcesList[k].X_position,
                        Y_position = Read.ResourcesList[k].Y_position,
                        Z_position = Read.ResourcesList[k].Z_position,
                        bAutoRevive = Read.ResourcesList[k].bAutoRevive,
                        bInitGen = Read.ResourcesList[k].bInitGen,
                        bValidOnce = Read.ResourcesList[k].bValidOnce,
                        dwGenID = Read.ResourcesList[k].dwGenID,
                        IMaxNum = Read.ResourcesList[k].IMaxNum,
                        Trigger_id = Read.ResourcesList[k].Trigger_id,
                        InCline1 = Read.ResourcesList[k].InCline1,
                        X_Random = Read.ResourcesList[k].X_Random,
                        InCline2 = Read.ResourcesList[k].InCline2,
                        Z_Random = Read.ResourcesList[k].Z_Random,
                        Rotation = Read.ResourcesList[k].Rotation,
                        ResExtra = new List<ClassExtraResources>()
                    };
                    for (int i = 0; i < Read.ResourcesList[k].Amount_in_group; i++)
                    {
                        ClassExtraResources ex = new ClassExtraResources()
                        {
                            Id = Read.ResourcesList[k].ResExtra[i].Id,
                            ResourceType = Read.ResourcesList[k].ResExtra[i].ResourceType,
                            Respawntime = Read.ResourcesList[k].ResExtra[i].Respawntime,
                            Amount = Read.ResourcesList[k].ResExtra[i].Amount,
                            fHeiOff = Read.ResourcesList[k].ResExtra[i].fHeiOff
                        };
                        mn.ResExtra.Add(ex);
                    }
                    Read.ResourcesList.Add(mn);
                    ResourcesGrid.Rows.Add(Read.ResourcesAmount, ResourcesGrid.Rows[k].Cells[1].Value, ResourcesGrid.Rows[k].Cells[2].Value);
                }
                ResourcesGrid.ScrollBars = ScrollBars.Vertical;
                var RowsClone = ResourcesRowCollection;
                ResourcesGrid.CurrentCell = ResourcesGrid.Rows[ResourcesGrid.Rows.Count - 1].Cells[1];
                for (int i = 1; i <= RowsClone.Count; i++)
                {
                    ResourcesGrid.Rows[ResourcesGrid.Rows.Count - i].Selected = true;
                }
                ResourcesGrid_CurrentCellChanged(null, null);
            }
            else
            {
                Read.ResourcesAmount++;
                ClassDefaultResources mb = new ClassDefaultResources()
                {
                    Amount_in_group = 1,
                    ResExtra = new List<ClassExtraResources>()
                };
                ClassExtraResources me = new ClassExtraResources()
                {
                    Id = 3074,
                    Amount = 1,
                    Respawntime = 60,
                    ResourceType = 80
                };
                mb.ResExtra.Add(me);
                Read.ResourcesList.Add(mb);
                if (Language == 1)
                    ResourcesGrid.Rows.Add(1, 3074, "Высохший древесный корень");
                else
                    ResourcesGrid.Rows.Add(1, 3074, "Withered root");
                ResourcesGrid_CurrentCellChanged(null, null);
            }
            if (Language == 1)
            {
                ResourcesTab.Text = string.Format("Ресурсы" + " 1/{0}", Read.ResourcesAmount);
            }
            else
            {
                ResourcesTab.Text = string.Format("Resources" + " 1/{0}", Read.ResourcesAmount);
            }
        }
        private void RemoveResourceFull_Click(object sender, EventArgs e)
        {
            if (Read != null && ResourcesRowCollection.Count != 0)
            {
                string Dialog = "Вы уверены,что хотите удалить выбранные объекты?";
                if (Language == 2)
                    Dialog = "Are you sure that you want to delete selected objects?";
                DialogResult dg = MessageBox.Show(Dialog, "Npcgen Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dg == DialogResult.Yes)
                {
                    int IndexBeforeRemoving = ResourcesRowCollection.Min();
                    ResourcesGrid.ScrollBars = ScrollBars.None;
                    ErrorsGrid.ScrollBars = ScrollBars.None;
                    AllowCellChanging = false;
                    ResourcesGrid.ClearSelection();
                    MainProgressBar.Maximum = ResourcesRowCollection.Count;
                    Read.ResourcesAmount -= ResourcesRowCollection.Count;
                    foreach (int i in ResourcesRowCollection)
                    {
                        var Matched = ErrorResourcesCollection.Where(f => f.GridIndex == i).OrderByDescending(f => f.ErrorInex).ToList();
                        foreach (var item in Matched)
                        {
                            ErrorsGrid.Rows.RemoveAt(item.ErrorInex);
                            ErrorResourcesCollection.RemoveAt(ErrorResourcesCollection.FindIndex(t => t.ErrorInex == item.ErrorInex));
                        }
                        ErrorResourcesCollection.Where(b => b.GridIndex > i).ToList().ForEach(s => s.ErrorInex -= Matched.Count);
                        ErrorResourcesCollection.Where(a => a.GridIndex > i).ToList().ForEach(s => s.GridIndex--);
                        ErrorDynamicsCollection.ForEach(s => s.ErrorInex -= Matched.Count);
                        Read.ResourcesList.RemoveAt(i);
                        ResourcesGrid.Rows.RemoveAt(i);
                        MainProgressBar.Value++;
                    }
                    if (ErrorsGrid.Rows.Count != 0)
                    {
                        int Amount = (ErrorResourcesCollection.Count != 0) ? ErrorResourcesCollection.Min(f => f.ErrorInex) : 0;
                        int MaxAmount = (ErrorResourcesCollection.Count != 0) ? ErrorResourcesCollection.Max(f => f.ErrorInex) : 0;
                        for (int i = Amount; i <= MaxAmount; i++)
                        {
                            ErrorsGrid.Rows[i].Cells[0].Value = i + 1;
                            ErrorsGrid.Rows[i].Cells[1].Value = ErrorResourcesCollection.Find(f => f.ErrorInex == i).GridIndex + 1;
                        }
                        int Amount1 = (ErrorDynamicsCollection.Count != 0) ? ErrorDynamicsCollection.Min(f => f.ErrorInex) : 0;
                        for (int i = Amount1; i <= Amount1; i++)
                        {
                            ErrorsGrid.Rows[i].Cells[0].Value = i + 1;
                        }
                    }
                    AllowCellChanging = true;
                    MainProgressBar.Value = 0;
                    ResourcesGrid.ScrollBars = ScrollBars.Vertical;
                    ErrorsGrid.ScrollBars = ScrollBars.Vertical;
                    if (ResourcesGrid.Rows.Count > IndexBeforeRemoving)
                    {
                        ResourcesGrid.CurrentCell = ResourcesGrid.Rows[IndexBeforeRemoving].Cells[1];
                        ResourcesGrid.FirstDisplayedScrollingRowIndex = IndexBeforeRemoving;
                    }
                    else if (ResourcesGrid.Rows.Count != 0)
                    {
                        ResourcesGrid.CurrentCell = ResourcesGrid.Rows[ResourcesGrid.Rows.Count - 1].Cells[1];
                        ResourcesGrid.FirstDisplayedScrollingRowIndex = ResourcesGrid.Rows.Count - 1;
                    }
                    ResourcesGrid_CurrentCellChanged(null, null);
                    for (int i = 0; i < ResourcesGrid.Rows.Count; i++)
                    {
                        ResourcesGrid.Rows[i].Cells[0].Value = i + 1;
                    }
                    if (Language == 1)
                    {
                        ResourcesTab.Text = string.Format("Ресурсы" + " 1/{0}", Read.ResourcesAmount);
                    }
                    else
                    {
                        ResourcesTab.Text = string.Format("Resources" + " 1/{0}", Read.ResourcesAmount);
                    }
                }
            }
        }
        private void CloneResourcesInGroup_Click(object sender, EventArgs e)
        {
            if (ResourcesGrid.Rows.Count > 0)
            {
                if (ResourcesGroupGrid.Rows.Count > 0)
                {
                    UnderResourcesRowCollection = UnderResourcesRowCollection.OrderBy(z => z).ToList();
                    foreach (var item in UnderResourcesRowCollection)
                    {
                        Read.ResourcesList[ResourcesRowIndex].Amount_in_group++;
                        ClassExtraResources ex = new ClassExtraResources()
                        {
                            Id = Read.ResourcesList[ResourcesRowIndex].ResExtra[item].Id,
                            Amount = Read.ResourcesList[ResourcesRowIndex].ResExtra[item].Amount,
                            ResourceType = Read.ResourcesList[ResourcesRowIndex].ResExtra[item].ResourceType,
                            Respawntime = Read.ResourcesList[ResourcesRowIndex].ResExtra[item].Respawntime,
                            fHeiOff = Read.ResourcesList[ResourcesRowIndex].ResExtra[item].fHeiOff
                        };
                        Read.ResourcesList[ResourcesRowIndex].ResExtra.Add(ex);
                        ResourcesGroupGrid.Rows.Add(Read.ResourcesList[ResourcesRowIndex].Amount_in_group, ResourcesGroupGrid.Rows[item].Cells[1].Value, ResourcesGroupGrid.Rows[item].Cells[2].Value);
                    }
                    RefreshResourcesRow(ResourcesRowIndex);
                    ResourcesGroupGrid.ClearSelection();
                    for (int z = 1; z <= UnderResourcesRowCollection.Count; z++)
                    {
                        ResourcesGroupGrid.Rows[ResourcesGroupGrid.Rows.Count - z].Selected = true;
                    }
                    ResourcesGroupGrid.CurrentCell = ResourcesGroupGrid.Rows[ResourcesGroupGrid.Rows.Count - 1].Cells[1];
                    ResourcesGroupGrid.FirstDisplayedScrollingRowIndex = ResourcesGroupGrid.Rows.Count - 1;
                }
                else
                {
                    ClassExtraResources ex = new ClassExtraResources();
                    Read.ResourcesList[ResourcesRowIndex].Amount_in_group++;
                    ex.Id = 3074;
                    ex.Amount = 1;
                    ex.Respawntime = 60;
                    ex.ResourceType = 80;
                    Read.ResourcesList[ResourcesRowIndex].ResExtra.Add(ex);
                    if (Language == 1)
                        ResourcesGroupGrid.Rows.Add(1, 3074, "Высохший древесный корень");
                    else
                        ResourcesGroupGrid.Rows.Add(1, 3074, "Withered root");
                    RefreshResourcesRow(ResourcesRowIndex);
                    ResourcesGroupGrid_CurrentCellChanged(null, null);
                }
            }
        }
        private void RemoveResourcesInGroup_Click(object sender, EventArgs e)
        {
            UnderResourcesRowCollection = UnderResourcesRowCollection.OrderByDescending(z => z).ToList();
            if (UnderResourcesRowCollection.Count != 0 && ResourcesGrid.Rows.Count > 0 && ResourcesGroupGrid.Rows.Count > 0)
            {
                string Dialog = "Вы уверены,что хотите удалить выбранные ресурсы?";
                if (Language == 2)
                    Dialog = "Are you sure that you want to delete selected resources?";
                DialogResult dg = MessageBox.Show(Dialog, "Npcgen Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dg == DialogResult.Yes)
                {
                    int IndexBeforeRemoving = UnderResourcesRowCollection.Min();
                    ResourcesGroupGrid.ClearSelection();
                    foreach (var item in UnderResourcesRowCollection)
                    {
                        Read.ResourcesList[ResourcesRowIndex].Amount_in_group--;
                        Read.ResourcesList[ResourcesRowIndex].ResExtra.RemoveAt(item);
                        ResourcesGroupGrid.Rows.RemoveAt(item);
                    }
                    RefreshResourcesRow(ResourcesRowIndex);
                    if (ResourcesGroupGrid.Rows.Count > IndexBeforeRemoving)
                    {
                        ResourcesGroupGrid.CurrentCell = ResourcesGroupGrid.Rows[IndexBeforeRemoving].Cells[1];
                        ResourcesGroupGrid.FirstDisplayedScrollingRowIndex = IndexBeforeRemoving;
                    }
                    else if (ResourcesGroupGrid.Rows.Count != 0)
                    {
                        ResourcesGroupGrid.CurrentCell = ResourcesGroupGrid.Rows[ResourcesGroupGrid.Rows.Count - 1].Cells[1];
                        ResourcesGroupGrid.FirstDisplayedScrollingRowIndex = ResourcesGroupGrid.Rows.Count - 1;
                    }
                    ResourcesGroupGrid_CurrentCellChanged(null, null);
                }
            }
        }
        private void RId_numeric_DoubleClick(object sender, EventArgs e)
        {
            if (ChooseFromElementsForm != null)
            {
                ChooseFromElementsForm.SetAction = 2;
                ChooseFromElementsForm.SetWindow = 1;
                int ind = Element.ResourcesList.FindIndex(z => z.Id == Convert.ToInt32(RId_numeric.Value));
                if (ind != -1)
                {
                    ChooseFromElementsForm.FindRow(ind, "Resource");
                }
                ChooseFromElementsForm.ShowDialog(this);
            }
        }
        private void RInsterCordsFromGame_Click(object sender, EventArgs e)
        {
            ClassPosition cs = GetCoordinates();
            if (cs != null)
            {
                RX_position.Text = cs.PosX.ToString();
                RY_position.Text = cs.PosY.ToString();
                RZ_position.Text = cs.PosZ.ToString();
                ResourcesDefaultLeave(RX_position, null);
                ResourcesDefaultLeave(RY_position, null);
                ResourcesDefaultLeave(RZ_position, null);
            }
        }
        #endregion
        #region Dynamics
        private void DynamicGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            if (AllowCellChanging == true)
            {
                DynamicsRowCollection = DynamicGrid.SelectedRows.Cast<DataGridViewRow>().Select(f => f.Index).OrderByDescending(v => v).ToList();
                if (DynamicGrid.CurrentRow != null)
                {
                    DynamicRowIndex = DynamicGrid.CurrentRow.Index;
                    if (DynamicRowIndex != -1)
                    {
                        DId_numeric.Text = Read.DynamicsList[DynamicRowIndex].Id.ToString();
                        Label_DynamicName.Text = DynamicGrid.Rows[DynamicRowIndex].Cells[2].Value.ToString();
                        DX_position.Text = Read.DynamicsList[DynamicRowIndex].X_position.ToString();
                        DY_position.Text = Read.DynamicsList[DynamicRowIndex].Y_position.ToString();
                        DZ_position.Text = Read.DynamicsList[DynamicRowIndex].Z_position.ToString();
                        DIncline1.Text = Read.DynamicsList[DynamicRowIndex].InCline1.ToString();
                        DIncline2.Text = Read.DynamicsList[DynamicRowIndex].InCline2.ToString();
                        DRotation.Text = Read.DynamicsList[DynamicRowIndex].Rotation.ToString();
                        DTrigger_id.Text = Read.DynamicsList[DynamicRowIndex].TriggerId.ToString();
                        DScale.Text = Read.DynamicsList[DynamicRowIndex].Scale.ToString();
                        string DynScreenPath = string.Format("{0}\\DynamicObjects\\d{1}.jpg", Application.StartupPath, DynamicGrid.Rows[DynamicRowIndex].Cells[1].Value);
                        if (File.Exists(DynScreenPath))
                        {
                            DynamicPictureBox.Image = Bitmap.FromFile(DynScreenPath);
                        }
                        if (MapForm != null && MainProgressBar.Value == 0 && DynamicsRowCollection.Count != 0 && MapForm.Visible == true)
                        {
                            MapForm.GetCoordinates(GetPoint(3));
                        }
                    }
                }
            }
            if (Language == 1)
            {
                DynObjectsTab.Text = string.Format("Динамические Объекты" + " {0}/{1}", DynamicsRowCollection.Count, Read.DynobjectAmount);
            }
            else
            {
                DynObjectsTab.Text = string.Format("Dynamic Objects" + " {0}/{1}", DynamicsRowCollection.Count, Read.DynobjectAmount);
            }
        }
        private void IdFind(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(DId_numeric.Text))
            {
                    Label_DynamicName.Text = GetDynamicName(Convert.ToInt32(DId_numeric.Text));
                string path = string.Format("{0}\\DynamicObjects\\d{1}.jpg", Application.StartupPath, Convert.ToInt32(DId_numeric.Text));
                if (File.Exists(path))
                {
                    DynamicPictureBox.Image = Bitmap.FromFile(path);
                }
                if(Label_DynamicName.Text == "?")
                {
                    DynamicPictureBox.Image = null;
                }

            }
        }
        private void DynamicsLeave(object sender, EventArgs e)
        {
            Control co = sender as Control;
            if (DynamicsRowCollection != null)
            {
                float FloatValue;
                int IntValue;
                byte ByteValue;
                switch (co.Name)
                {
                    case "DId_numeric":
                        {
                            int.TryParse(DId_numeric.Text, out IntValue);
                            foreach (var item in DynamicsRowCollection)
                            {
                                Read.DynamicsList[item].Id = IntValue;
                                DynamicGrid.Rows[item].Cells[1].Value = IntValue;
                                DynamicGrid.Rows[item].Cells[2].Value = Label_DynamicName.Text;
                            }
                            break;
                        }
                    case "DX_position":
                        {
                            float.TryParse(DX_position.Text, out FloatValue);
                            foreach (var item in DynamicsRowCollection)
                            {
                                Read.DynamicsList[item].X_position = FloatValue;
                            }
                            break;
                        }
                    case "DY_position":
                        {
                            float.TryParse(DY_position.Text, out FloatValue);
                            foreach (var item in DynamicsRowCollection)
                            {
                                Read.DynamicsList[item].Y_position = FloatValue;
                            }
                            break;
                        }
                    case "DZ_position":
                        {
                            float.TryParse(DZ_position.Text, out FloatValue);
                            foreach (var item in DynamicsRowCollection)
                            {
                                Read.DynamicsList[item].Z_position = FloatValue;
                            }
                            break;
                        }
                    case "DIncline1":
                        {
                            byte.TryParse(DIncline1.Text, out ByteValue);
                            foreach (var item in DynamicsRowCollection)
                            {
                                Read.DynamicsList[item].InCline1 = ByteValue;
                            }
                            break;
                        }
                    case "DIncline2":
                        {
                            byte.TryParse(DIncline2.Text, out ByteValue);
                            foreach (var item in DynamicsRowCollection)
                            {
                                Read.DynamicsList[item].InCline2 = ByteValue;
                            }
                            break;
                        }
                    case "DRotation":
                        {
                            byte.TryParse(DRotation.Text, out ByteValue);
                            foreach (var item in DynamicsRowCollection)
                            {
                                Read.DynamicsList[item].Rotation = ByteValue;
                            }
                            break;
                        }
                    case "DTrigger_id":
                        {
                            int.TryParse(DTrigger_id.Text, out IntValue);
                            foreach (var item in DynamicsRowCollection)
                            {
                                DynamicGrid.Rows[item].Cells[3].Value = IntValue;
                                Read.DynamicsList[item].TriggerId = IntValue;
                            }
                            break;
                        }
                    case "DScale":
                        {
                            byte.TryParse(DScale.Text, out ByteValue);
                            foreach (var item in DynamicsRowCollection)
                            {
                                Read.DynamicsList[item].Scale = ByteValue;
                            }
                            break;
                        }
                }
            }
        }
        private void DynamicsKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                DynamicsLeave(sender, null);
        }
        private void DClone_button_Click(object sender, EventArgs e)
        {
            if (DynamicGrid.Rows.Count > 0)
            {
                DynamicGrid.ScrollBars = ScrollBars.None;
                DynamicsRowCollection = DynamicsRowCollection.OrderBy(z => z).ToList();
                foreach (int k in DynamicsRowCollection)
                {
                    Read.DynobjectAmount++;
                    ClassDynamicObject mn = new ClassDynamicObject()
                    {
                        Id = Read.DynamicsList[k].Id,
                        InCline1 = Read.DynamicsList[k].InCline1,
                        InCline2 = Read.DynamicsList[k].InCline2,
                        Rotation = Read.DynamicsList[k].Rotation,
                        Scale = Read.DynamicsList[k].Scale,
                        TriggerId = Read.DynamicsList[k].TriggerId,
                        X_position = Read.DynamicsList[k].X_position,
                        Y_position = Read.DynamicsList[k].Y_position,
                        Z_position = Read.DynamicsList[k].Z_position
                    };
                    Read.DynamicsList.Add(mn);
                    DynamicGrid.Rows.Add(Read.DynobjectAmount, DynamicGrid.Rows[k].Cells[1].Value, DynamicGrid.Rows[k].Cells[2].Value, DynamicGrid.Rows[k].Cells[3].Value);
                }
                DynamicGrid.ScrollBars = ScrollBars.Vertical;
                var RowsClone = DynamicsRowCollection;
                DynamicGrid.CurrentCell = DynamicGrid.Rows[DynamicGrid.Rows.Count - 1].Cells[1];
                for (int i = 1; i <= RowsClone.Count; i++)
                {
                    DynamicGrid.Rows[DynamicGrid.Rows.Count - i].Selected = true;
                }
                DynamicGrid_CurrentCellChanged(null, null);
            }
            else
            {
                Read.DynobjectAmount++;
                ClassDynamicObject mb = new ClassDynamicObject()
                {
                    Id = 16
                };
                Read.DynamicsList.Add(mb);
                if (Language == 1)
                    DynamicGrid.Rows.Add(1, 9, "Ворота", 0);
                else
                    DynamicGrid.Rows.Add(1, 9, "Gates", 0);
                DynamicGrid_CurrentCellChanged(null, null);
            }
            if (Language == 1)
            {
                DynObjectsTab.Text = string.Format("Динамические Объекты" + " 1/{0}", Read.DynobjectAmount);
            }
            else
            {
                DynObjectsTab.Text = string.Format("Dynamic Objects" + " 1/{0}", Read.DynobjectAmount);
            }
        }
        private void DDelete_button_Click(object sender, EventArgs e)
        {
            if (Read != null && DynamicsRowCollection.Count != 0)
            {
                string Dialog = "Вы уверены,что хотите удалить выбранные объекты?";
                if (Language == 2)
                    Dialog = "Are you sure that you want to delete selected objects?";
                DialogResult dg = MessageBox.Show(Dialog, "Npcgen Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dg == DialogResult.Yes)
                {
                    int IndexBeforeRemoving = DynamicsRowCollection.Min();
                    DynamicGrid.ScrollBars = ScrollBars.None;
                    ErrorsGrid.ScrollBars = ScrollBars.None;
                    AllowCellChanging = false;
                    DynamicGrid.ClearSelection();
                    MainProgressBar.Maximum = DynamicsRowCollection.Count;
                    Read.DynobjectAmount -= DynamicsRowCollection.Count;
                    foreach (int i in DynamicsRowCollection)
                    {
                        var Matched = ErrorDynamicsCollection.Where(f => f.GridIndex == i).OrderByDescending(f => f.ErrorInex).ToList();
                        foreach (var item in Matched)
                        {
                            ErrorsGrid.Rows.RemoveAt(item.ErrorInex);
                            ErrorDynamicsCollection.RemoveAt(ErrorDynamicsCollection.FindIndex(t => t.ErrorInex == item.ErrorInex));
                        }
                        ErrorDynamicsCollection.Where(b => b.GridIndex > i).ToList().ForEach(s => s.ErrorInex -= Matched.Count);
                        ErrorDynamicsCollection.Where(a => a.GridIndex > i).ToList().ForEach(s => s.GridIndex--);
                        Read.DynamicsList.RemoveAt(i);
                        DynamicGrid.Rows.RemoveAt(i);
                        MainProgressBar.Value++;
                    }
                    if (ErrorsGrid.Rows.Count != 0)
                    {
                        int Amount = (ErrorDynamicsCollection.Count != 0) ? ErrorDynamicsCollection.Min(f => f.ErrorInex) : 0;
                        int MaxAmount = (ErrorDynamicsCollection.Count != 0) ? ErrorDynamicsCollection.Max(f => f.ErrorInex) : 0;
                        for (int i = Amount; i <= MaxAmount; i++)
                        {
                            ErrorsGrid.Rows[i].Cells[0].Value = i + 1;
                            ErrorsGrid.Rows[i].Cells[1].Value = ErrorDynamicsCollection.Find(f => f.ErrorInex == i).GridIndex + 1;
                        }
                    }
                    AllowCellChanging = true;
                    MainProgressBar.Value = 0;
                    DynamicGrid.ScrollBars = ScrollBars.Vertical;
                    ErrorsGrid.ScrollBars = ScrollBars.Vertical;
                    if (DynamicGrid.Rows.Count > IndexBeforeRemoving)
                    {
                        DynamicGrid.CurrentCell = DynamicGrid.Rows[IndexBeforeRemoving].Cells[1];
                        DynamicGrid.FirstDisplayedScrollingRowIndex = IndexBeforeRemoving;
                    }
                    else if (DynamicGrid.Rows.Count != 0)
                    {
                        DynamicGrid.CurrentCell = DynamicGrid.Rows[DynamicGrid.Rows.Count - 1].Cells[1];
                        DynamicGrid.FirstDisplayedScrollingRowIndex = DynamicGrid.Rows.Count - 1;
                    }
                    DynamicGrid_CurrentCellChanged(null, null);
                    for (int i = 0; i < DynamicGrid.Rows.Count; i++)
                    {
                        DynamicGrid.Rows[i].Cells[0].Value = i + 1;
                    }
                    if (Language == 1)
                    {
                        DynObjectsTab.Text = string.Format("Динамические объекты" + " 1/{0}", Read.DynobjectAmount);
                    }
                    else
                    {
                        DynObjectsTab.Text = string.Format("Dynamic objects" + " 1/{0}", Read.DynobjectAmount);
                    }
                }
            }
        }
        private void DInsterCordsFromGame_Click(object sender, EventArgs e)
        {
            ClassPosition cs = GetCoordinates();
            if (cs != null)
            {
                DX_position.Text = cs.PosX.ToString();
                DY_position.Text = cs.PosY.ToString();
                DZ_position.Text = cs.PosZ.ToString();
                DynamicsLeave(DX_position, null);
                DynamicsLeave(DY_position, null);
                DynamicsLeave(DZ_position, null);
            }
        }
        #endregion
        #region Triggers
        private void TriggerUsingInMobsAndNpcsGrid_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > 0 & e.RowIndex >= 0)
            {
                string j = Convert.ToString(MUTrigger.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                toolTip1.SetToolTip(MUTrigger, j);
            }
        }
        private void TriggersGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            if (AllowCellChanging == true)
            {
                TriggersRowCollection = TriggersGrid.SelectedRows.Cast<DataGridViewRow>().Select(f => f.Index).OrderByDescending(v => v).ToList();
                if (TriggersGrid.CurrentRow != null)
                {
                    TriggersRowIndex = TriggersGrid.CurrentRow.Index;
                    if (TriggersRowIndex != -1)
                    {
                        TId_textbox.Text = Read.TriggersList[TriggersRowIndex].Id.ToString();
                        TGmId_textbox.Text = Read.TriggersList[TriggersRowIndex].GmID.ToString();
                        TName_textbox.Text = Read.TriggersList[TriggersRowIndex].TriggerName.ToString();
                        TWaitStart_textbox.Text = Read.TriggersList[TriggersRowIndex].WaitWhileStart.ToString();
                        TWaitStop_textbox.Text = Read.TriggersList[TriggersRowIndex].WaitWhileStop.ToString();
                        TAutoStart.Checked = Convert.ToBoolean(Read.TriggersList[TriggersRowIndex].AutoStart);
                        TDuration.Text = Read.TriggersList[TriggersRowIndex].Duration.ToString();
                        TStartBySchedule.Checked = Convert.ToBoolean(Read.TriggersList[TriggersRowIndex].DontStartOnSchedule);
                        TStopBySchedule.Checked = Convert.ToBoolean(Read.TriggersList[TriggersRowIndex].DontStopOnSchedule);
                        TStartYear.Text = Read.TriggersList[TriggersRowIndex].StartYear.ToString();
                        TStartMonth.Text = Read.TriggersList[TriggersRowIndex].StartMonth.ToString();
                        TStartWeekDay.SelectedIndex = Read.TriggersList[TriggersRowIndex].StartWeekDay + 1;
                        TStartDay.Text = Read.TriggersList[TriggersRowIndex].StartDay.ToString();
                        TStartHour.Text = Read.TriggersList[TriggersRowIndex].StartHour.ToString();
                        TStartMinute.Text = Read.TriggersList[TriggersRowIndex].StartMinute.ToString();
                        TStopYear.Text = Read.TriggersList[TriggersRowIndex].StopYear.ToString();
                        TStopMonth.Text = Read.TriggersList[TriggersRowIndex].StopMonth.ToString();
                        TStopWeekDay.SelectedIndex = Read.TriggersList[TriggersRowIndex].StopWeekDay + 1;
                        TStopDay.Text = Read.TriggersList[TriggersRowIndex].StopDay.ToString();
                        TStopHour.Text = Read.TriggersList[TriggersRowIndex].StopHour.ToString();
                        TStopMinute.Text = Read.TriggersList[TriggersRowIndex].StopMinute.ToString();
                        MUTrigger.ScrollBars = ScrollBars.None;
                        RUTrigger.ScrollBars = ScrollBars.None;
                        DUTrigger.ScrollBars = ScrollBars.None;
                        MonstersContact = Read.NpcMobList.Where(z => z.Trigger_id == Read.TriggersList[TriggersRowIndex].Id).ToList();
                        MUTrigger.Rows.Clear();
                        for (int i = 0; i < MonstersContact.Count; i++)
                        {
                            string ID = Convert.ToString(NpcMobsGrid.Rows[Read.NpcMobList.IndexOf(MonstersContact[i])].Cells[1].Value);
                            string names = Convert.ToString(NpcMobsGrid.Rows[Read.NpcMobList.IndexOf(MonstersContact[i])].Cells[2].Value);
                            MUTrigger.Rows.Add(i + 1, ID, names);
                        }
                        ResourcesContact = Read.ResourcesList.Where(z => z.Trigger_id == Read.TriggersList[TriggersRowIndex].Id).ToList();
                        RUTrigger.Rows.Clear();
                        for (int i = 0; i < ResourcesContact.Count; i++)
                        {
                            string ID = Convert.ToString(ResourcesGrid.Rows[Read.ResourcesList.IndexOf(ResourcesContact[i])].Cells[1].Value);
                            string names = Convert.ToString(ResourcesGrid.Rows[Read.ResourcesList.IndexOf(ResourcesContact[i])].Cells[2].Value);
                            RUTrigger.Rows.Add(i + 1, ID, names);
                        }
                        DynamicsContact = Read.DynamicsList.Where(z => z.TriggerId == Read.TriggersList[TriggersRowIndex].Id).ToList();
                        DUTrigger.Rows.Clear();
                        for (int i = 0; i < DynamicsContact.Count; i++)
                        {
                            string ID = Convert.ToString(DynamicGrid.Rows[Read.DynamicsList.IndexOf(DynamicsContact[i])].Cells[1].Value);
                            string names = Convert.ToString(DynamicGrid.Rows[Read.DynamicsList.IndexOf(DynamicsContact[i])].Cells[2].Value);
                            DUTrigger.Rows.Add(i + 1, ID, names);
                        }
                        MUTrigger.ScrollBars = ScrollBars.Vertical;
                        RUTrigger.ScrollBars = ScrollBars.Vertical;
                        DUTrigger.ScrollBars = ScrollBars.Vertical;
                    }
                }
            }
            if (Language == 1)
            {
                TriggersTab.Text = string.Format("Триггеры" + " {0}/{1}", TriggersRowCollection.Count, Read.TriggersAmount);
            }
            else
            {
                TriggersTab.Text = string.Format("Triggers" + " {0}/{1}", TriggersRowCollection.Count, Read.TriggersAmount);
            }
        }
        private void TId_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                TId_textbox_Leave(sender, new EventArgs());
        }
        private void TId_textbox_Leave(object sender, EventArgs e)
        {
            Control co = sender as Control;
            int IntValue;
            switch (co.Name)
            {
                case "TId_textbox":
                    {
                        int.TryParse(TId_textbox.Text, out IntValue);
                        foreach (var item in TriggersRowCollection)
                        {
                            Read.TriggersList[item].Id = IntValue;
                            TriggersGrid.Rows[item].Cells[1].Value = IntValue;
                        }
                        break;
                    }
                case "TGmId_textbox":
                    {
                        int.TryParse(TGmId_textbox.Text, out IntValue);
                        foreach (var item in TriggersRowCollection)
                        {
                            Read.TriggersList[item].GmID = IntValue;
                            TriggersGrid.Rows[item].Cells[2].Value = IntValue;
                        }
                        break;
                    }
                case "TName_textbox":
                    {
                        foreach (var item in TriggersRowCollection)
                        {
                            Read.TriggersList[item].TriggerName = TName_textbox.Text;
                            TriggersGrid.Rows[item].Cells[3].Value = TName_textbox.Text;
                        }
                        break;
                    }
                case "TWaitStart_textbox":
                    {
                        int.TryParse(TWaitStart_textbox.Text, out IntValue);
                        foreach (var item in TriggersRowCollection)
                        {
                            Read.TriggersList[item].WaitWhileStart = IntValue;
                        }
                        break;
                    }
                case "TWaitStop_textbox":
                    {
                        int.TryParse(TWaitStop_textbox.Text, out IntValue);
                        foreach (var item in TriggersRowCollection)
                        {
                            Read.TriggersList[item].WaitWhileStop = IntValue;
                        }
                        break;
                    }
                case "TDuration":
                    {
                        int.TryParse(TDuration.Text, out IntValue);
                        foreach (var item in TriggersRowCollection)
                        {
                            Read.TriggersList[item].Duration = IntValue;
                        }
                        break;
                    }
                case "TAutoStart":
                    {
                        foreach (var item in TriggersRowCollection)
                        {
                            Read.TriggersList[item].AutoStart = Convert.ToByte(TAutoStart.Checked);
                        }
                        break;
                    }
                case "TStartBySchedule":
                    {
                        foreach (var item in TriggersRowCollection)
                        {
                            Read.TriggersList[item].DontStartOnSchedule = Convert.ToByte(TStartBySchedule.Checked);
                        }
                        break;
                    }
                case "TStopBySchedule":
                    {
                        foreach (var item in TriggersRowCollection)
                        {
                            Read.TriggersList[item].DontStopOnSchedule = Convert.ToByte(TStopBySchedule.Checked);
                        }
                        break;
                    }
                case "TStartYear":
                    {
                        int.TryParse(TStartYear.Text, out IntValue);
                        foreach (var item in TriggersRowCollection)
                        {
                            Read.TriggersList[item].StartYear = IntValue;
                        }
                        break;
                    }
                case "TStartMonth":
                    {
                        int.TryParse(TStartMonth.Text, out IntValue);
                        foreach (var item in TriggersRowCollection)
                        {
                            Read.TriggersList[item].StartMonth = IntValue;
                        }
                        break;
                    }
                case "TStartWeekDay":
                    {
                        foreach (var item in TriggersRowCollection)
                        {
                            Read.TriggersList[item].StartWeekDay = TStartWeekDay.SelectedIndex - 1;
                        }
                        break;
                    }
                case "TStartDay":
                    {
                        int.TryParse(TStartDay.Text, out IntValue);
                        foreach (var item in TriggersRowCollection)
                        {
                            Read.TriggersList[item].StartDay = IntValue;
                        }
                        break;
                    }
                case "TStartHour":
                    {
                        int.TryParse(TStartHour.Text, out IntValue);
                        foreach (var item in TriggersRowCollection)
                        {
                            Read.TriggersList[item].StartHour = IntValue;
                        }
                        break;
                    }
                case "TStartMinute":
                    {
                        int.TryParse(TStartMinute.Text, out IntValue);
                        foreach (var item in TriggersRowCollection)
                        {
                            Read.TriggersList[item].StartMinute = IntValue;
                        }
                        break;
                    }
                case "TStopYear":
                    {
                        int.TryParse(TStopYear.Text, out IntValue);
                        foreach (var item in TriggersRowCollection)
                        {
                            Read.TriggersList[item].StopYear = IntValue;
                        }
                        break;
                    }
                case "TStopMonth":
                    {
                        int.TryParse(TStopMonth.Text, out IntValue);
                        foreach (var item in TriggersRowCollection)
                        {
                            Read.TriggersList[item].StopMonth = IntValue;
                        }
                        break;
                    }
                case "TStopWeekDay":
                    {
                        foreach (var item in TriggersRowCollection)
                        {
                            Read.TriggersList[item].StopWeekDay = TStopWeekDay.SelectedIndex - 1;
                        }
                        break;
                    }
                case "TStopDay":
                    {
                        int.TryParse(TStopDay.Text, out IntValue);
                        foreach (var item in TriggersRowCollection)
                        {
                            Read.TriggersList[item].StopDay = IntValue;
                        }
                        break;
                    }
                case "TStopHour":
                    {
                        int.TryParse(TStopHour.Text, out IntValue);
                        foreach (var item in TriggersRowCollection)
                        {
                            Read.TriggersList[item].StopHour = IntValue;
                        }
                        break;
                    }
                case "TStopMinute":
                    {
                        int.TryParse(TStopMinute.Text, out IntValue);
                        foreach (var item in TriggersRowCollection)
                        {
                            Read.TriggersList[item].StopMinute = IntValue;
                        }
                        break;
                    }
            }

        }
        private void CloneTrigger_Click(object sender, EventArgs e)
        {
            if (TriggersGrid.Rows.Count > 0)
            {
                TriggersGrid.ScrollBars = ScrollBars.None;
                TriggersRowCollection = TriggersRowCollection.OrderBy(z => z).ToList();
                foreach (int k in TriggersRowCollection)
                {
                    Read.TriggersAmount++;
                    ClassTrigger mn = new ClassTrigger()
                    {
                        Id = Read.TriggersList.Max(z => z.Id) + 1,
                        GmID = Read.TriggersList.Max(z => z.GmID) + 1,
                        TriggerName = Read.TriggersList[k].TriggerName,
                        AutoStart = Read.TriggersList[k].AutoStart,
                        DontStartOnSchedule = Read.TriggersList[k].DontStartOnSchedule,
                        DontStopOnSchedule = Read.TriggersList[k].DontStopOnSchedule,
                        Duration = Read.TriggersList[k].Duration,
                        StartDay = Read.TriggersList[k].StartDay,
                        StartHour = Read.TriggersList[k].StartHour,
                        StartMinute = Read.TriggersList[k].StartMinute,
                        StartMonth = Read.TriggersList[k].StartMonth,
                        StartWeekDay = Read.TriggersList[k].StartWeekDay,
                        StartYear = Read.TriggersList[k].StartYear,
                        StopDay = Read.TriggersList[k].StopDay,
                        StopHour = Read.TriggersList[k].StopHour,
                        StopMinute = Read.TriggersList[k].StopMinute,
                        StopMonth = Read.TriggersList[k].StopMonth,
                        StopWeekDay = Read.TriggersList[k].StopWeekDay,
                        StopYear = Read.TriggersList[k].StopYear,
                        WaitWhileStart = Read.TriggersList[k].WaitWhileStart,
                        WaitWhileStop = Read.TriggersList[k].WaitWhileStop
                    };
                    Read.TriggersList.Add(mn);
                    TriggersGrid.Rows.Add(Read.TriggersAmount, mn.Id, mn.GmID, TriggersGrid.Rows[k].Cells[3].Value);
                }
                TriggersGrid.ScrollBars = ScrollBars.Vertical;
                var RowsClone = TriggersRowCollection;
                TriggersGrid.CurrentCell = TriggersGrid.Rows[TriggersGrid.Rows.Count - 1].Cells[1];
                for (int i = 1; i <= RowsClone.Count; i++)
                {
                    TriggersGrid.Rows[TriggersGrid.Rows.Count - i].Selected = true;
                }
                TriggersGrid_CurrentCellChanged(null, null);
            }
            else
            {
                Read.TriggersAmount++;
                ClassTrigger mb = new ClassTrigger()
                {
                    Id = 1,
                    GmID = 2,
                    TriggerName = "TriggerOne",
                    Duration = 60,
                    StartYear = -1,
                    StartMonth = -1,
                    StartWeekDay = -1,
                    StartDay = -1,
                    StartHour = -1,
                    StartMinute = -1,
                    StopYear = -1,
                    StopMonth = -1,
                    StopWeekDay = -1,
                    StopDay = -1,
                    StopHour = -1,
                    StopMinute = -1
                };
                Read.TriggersList.Add(mb);
                TriggersGrid.Rows.Add(1, 1, 2, "TriggerOne");
                TriggersGrid_CurrentCellChanged(null, null);
            }
            if (Language == 1)
            {
                TriggersTab.Text = string.Format("Триггеры" + " 1/{0}", Read.TriggersAmount);
            }
            else
            {
                TriggersTab.Text = string.Format("Triggers" + " 1/{0}", Read.TriggersAmount);
            }
        }
        private void DeleteTrigger_Click(object sender, EventArgs e)
        {
            if (TriggersRowCollection.Count != 0)
            {
                string Dialog = "Вы уверены,что хотите удалить выбранные триггера?";
                if (Language == 2)
                    Dialog = "Are you sure that you want to delete selected triggers?";
                DialogResult dg = MessageBox.Show(Dialog, "Npcgen Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dg == DialogResult.Yes)
                {
                    TriggersRowCollection = TriggersRowCollection.OrderByDescending(z => z).ToList();
                    TriggersGrid.ScrollBars = ScrollBars.None;
                    int IndexBeforeRemoving = TriggersRowCollection.Min();
                    TriggersGrid.ClearSelection();
                    Read.TriggersAmount -= TriggersRowCollection.Count;
                    foreach (var Index in TriggersRowCollection)
                    {
                        Read.TriggersList.RemoveAt(Index);
                        TriggersGrid.Rows.RemoveAt(Index);
                    }
                    TriggersGrid.ScrollBars = ScrollBars.Vertical;
                    if (TriggersGrid.Rows.Count > IndexBeforeRemoving)
                    {
                        TriggersGrid.CurrentCell = TriggersGrid.Rows[IndexBeforeRemoving].Cells[1];
                        TriggersGrid.FirstDisplayedScrollingRowIndex = IndexBeforeRemoving;
                    }
                    else if (TriggersGrid.Rows.Count != 0)
                    {
                        TriggersGrid.CurrentCell = TriggersGrid.Rows[TriggersGrid.Rows.Count - 1].Cells[1];
                        TriggersGrid.FirstDisplayedScrollingRowIndex = TriggersGrid.Rows.Count - 1;
                    }
                    if (TriggersGrid.Rows.Count == 0)
                        TriggersGrid.Rows.Clear();
                    TriggersGrid_CurrentCellChanged(null, null);
                    if (Language == 1)
                    {
                        TriggersTab.Text = string.Format("Триггеры" + " 1/{0}", Read.TriggersAmount);
                    }
                    else
                    {
                        TriggersTab.Text = string.Format("Triggers" + " 1/{0}", Read.TriggersAmount);
                    }
                }
            }
        }
        private void GotoNpcMobsContacts_Click(object sender, EventArgs e)
        {
            try
            {
                if (MUTrigger.SelectedRows.Count != 0)
                {
                    NpcMobsGrid.ClearSelection();
                    NpcMobsGrid.CurrentCell = NpcMobsGrid.Rows[Read.NpcMobList.IndexOf(MonstersContact[MUTrigger.SelectedRows[MUTrigger.SelectedRows.Count - 1].Index])].Cells[1];
                    for (int i = 0; i < MUTrigger.SelectedRows.Count; i++)
                    {
                        NpcMobsGrid.Rows[Read.NpcMobList.IndexOf(MonstersContact[MUTrigger.SelectedRows[i].Index])].Selected = true;
                    }
                    ExistenceGrid_CellChanged(null, null);
                    MainTabControl.SelectedIndex = 0;
                }
            }
            catch
            {
                if (Language == 1)
                    MessageBox.Show("Действие невозможно", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Wrong action", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void GotoResourcesContacts_Click(object sender, EventArgs e)
        {
            try
            {
                if (RUTrigger.SelectedRows.Count != 0)
                {
                    ResourcesGrid.ClearSelection();
                    ResourcesGrid.CurrentCell = ResourcesGrid.Rows[Read.ResourcesList.IndexOf(ResourcesContact[RUTrigger.SelectedRows[RUTrigger.SelectedRows.Count - 1].Index])].Cells[1];
                    for (int i = 0; i < RUTrigger.SelectedRows.Count; i++)
                    {
                        ResourcesGrid.Rows[Read.ResourcesList.IndexOf(ResourcesContact[RUTrigger.SelectedRows[i].Index])].Selected = true;
                    }
                    MainTabControl.SelectedIndex = 1;
                }
            }
            catch
            {
                if (Language == 1)
                    MessageBox.Show("Действие невозможно", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Wrong action", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void GotoDynamicsContacts_Click(object sender, EventArgs e)
        {
            try
            {
                if (DUTrigger.SelectedRows.Count != 0)
                {
                    DynamicGrid.ClearSelection();
                    DynamicGrid.CurrentCell = DynamicGrid.Rows[Read.DynamicsList.IndexOf(DynamicsContact[DUTrigger.SelectedRows[DUTrigger.SelectedRows.Count - 1].Index])].Cells[1];
                    for (int i = 0; i < DUTrigger.SelectedRows.Count; i++)
                    {
                        DynamicGrid.Rows[Read.DynamicsList.IndexOf(DynamicsContact[DUTrigger.SelectedRows[i].Index])].Selected = true;
                    }
                    MainTabControl.SelectedIndex = 2;
                }
            }
            catch
            {
                if (Language == 1)
                    MessageBox.Show("Действие невозможно", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Wrong action", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void DeleteEmptyTrigger_Click(object sender, EventArgs e)
        {
            if (Read != null)
            {
                List<int> Removable = new List<int>();
                for (int i = 0; i < Read.TriggersAmount; i++)
                {
                    var Exs = Read.NpcMobList.FindIndex(z => z.Trigger_id == Read.TriggersList[i].Id);
                    var Res = Read.ResourcesList.FindIndex(z => z.Trigger_id == Read.TriggersList[i].Id);
                    var Dyns = Read.DynamicsList.FindIndex(z => z.TriggerId == Read.TriggersList[i].Id);
                    if (Exs == -1 && Res == -1 && Dyns == -1)
                    {
                        Removable.Add(Read.TriggersList.IndexOf(Read.TriggersList[i]));
                    }
                }
                TriggersGrid.ScrollBars = ScrollBars.None;
                Removable = Removable.OrderByDescending(z => z).ToList();
                string Dialog = "Вы уверены,что хотите удалить неиспользуемые триггеры?";
                if (Language == 2)
                    Dialog = "Are you sure that you want to delete empty triggers?";
                DialogResult dg = MessageBox.Show(Dialog, "Npcgen Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dg == DialogResult.Yes)
                {
                    foreach (int item in Removable)
                    {
                        Read.TriggersAmount--;
                        Read.TriggersList.RemoveAt(item);
                        TriggersGrid.Rows.RemoveAt(item);
                    }
                    TriggersGrid.ScrollBars = ScrollBars.Vertical;
                    if (Language == 1)
                    {
                        TriggersTab.Text = string.Format("Триггеры" + " 1/{0}", Read.TriggersAmount);
                    }
                    else
                    {
                        TriggersTab.Text = string.Format("Triggers" + " 1/{0}", Read.TriggersAmount);
                    }
                    if (Language == 1)
                        MessageBox.Show(string.Format("Удалено {0} триггеров", Removable.Count), "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show(string.Format("Deleted {0} triggers", Removable.Count), "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void MUTrigger_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            GotoNpcMobsContacts_Click(null, null);
        }
        private void RUTrigger_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            GotoResourcesContacts_Click(null, null);
        }
        private void DUTrigger_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            GotoDynamicsContacts_Click(null, null);
        }
        #endregion
        #region Search
        private void ExistenceSearchButton_Click(object sender, EventArgs e)
        {
            if (Read != null)
            {
                Action = 1;
                string Section = "Существа";
                if (Language == 2)
                    Section = "Existence";
                SearchMonsters = new List<int>();
                SearchGrid.ScrollBars = ScrollBars.None;
                SearchGrid.Rows.Clear();
                if (ExistenceSearchId_Radio.Checked == true)
                {
                    int.TryParse(ExistenceSearchId.Text, out int IdIndex);
                    SearchMonsters = Read.NpcMobList.Select((item, index) => new { Item = item, Index = index }).Where(o => o.Item.MobDops.Any(x => x.Id == IdIndex)).Select(o => o.Index).ToList();
                    for (int i = 0; i < SearchMonsters.Count(); i++)
                    {
                        SearchGrid.Rows.Add(i + 1, NpcMobsGrid.Rows[SearchMonsters[i]].Cells[1].Value, NpcMobsGrid.Rows[SearchMonsters[i]].Cells[2].Value, Section);
                    }
                }
                else if (ExistenceSearchName_Radio.Checked == true)
                {
                    int ind = 1;
                    for (int i = 0; i < NpcMobsGrid.Rows.Count; i++)
                    {
                        if (NpcMobsGrid.Rows[i].Cells[2].Value.ToString().ToLower().Contains(ExistenceSearchName.Text.ToLower()))
                        {
                            SearchMonsters.Add(i);
                            SearchGrid.Rows.Add(ind, NpcMobsGrid.Rows[i].Cells[1].Value, NpcMobsGrid.Rows[i].Cells[2].Value, Section);
                            ind++;
                        }
                    }
                }
                else if (ExistenceSearchTrigger_Radio.Checked == true)
                {
                    int.TryParse(ExistenceSearchId.Text, out int IdIndex);
                    SearchMonsters = Read.NpcMobList.Select((item, index) => new { Item = item, Index = index }).Where(z => z.Item.Trigger_id == IdIndex).Select(o => o.Index).ToList();
                    for (int i = 0; i < SearchMonsters.Count; i++)
                    {
                        SearchGrid.Rows.Add(i + 1, NpcMobsGrid.Rows[SearchMonsters[i]].Cells[1].Value, NpcMobsGrid.Rows[SearchMonsters[i]].Cells[2].Value, Section);
                    }
                }
                else if (ExistenceSearchPath_Radio.Checked == true)
                {
                    int.TryParse(ExistenceSearchPath.Text, out int IdIndex);
                    SearchMonsters = Read.NpcMobList.Select((item, index) => new { Item = item, Index = index }).Where(o => o.Item.MobDops.Any(x => x.Path == IdIndex)).Select(o => o.Index).ToList();
                    for (int i = 0; i < SearchMonsters.Count; i++)
                    {
                        SearchGrid.Rows.Add(i + 1, NpcMobsGrid.Rows[SearchMonsters[i]].Cells[1].Value, NpcMobsGrid.Rows[SearchMonsters[i]].Cells[2].Value, Section);
                    }
                }
                SearchGrid.ScrollBars = ScrollBars.Vertical;
            }
        }
        private void ResourceSearchButton_Click(object sender, EventArgs e)
        {
            if (Read != null)
            {
                Action = 2;
                string Section = "Ресерсы";
                if (Language == 2)
                    Section = "Resources";
                SearchResources = new List<int>();
                SearchGrid.ScrollBars = ScrollBars.None;
                SearchGrid.Rows.Clear();
                if (ResourceSearchId_Radio.Checked == true)
                {
                    int.TryParse(ResourceSearchId.Text, out int IdIndex);
                    SearchResources = Read.ResourcesList.Select((item, index) => new { Item = item, Index = index }).Where(o => o.Item.ResExtra.Any(x => x.Id == IdIndex)).Select(o => o.Index).ToList();
                    for (int i = 0; i < SearchResources.Count(); i++)
                    {
                        SearchGrid.Rows.Add(i + 1, ResourcesGrid.Rows[SearchResources[i]].Cells[1].Value, ResourcesGrid.Rows[SearchResources[i]].Cells[2].Value, Section);
                    }
                }
                else if (ResourceSearchName_Radio.Checked == true)
                {
                    int ind = 1;
                    for (int i = 0; i < ResourcesGrid.Rows.Count; i++)
                    {
                        if (ResourcesGrid.Rows[i].Cells[2].Value.ToString().ToLower().Contains(ResourceSearchName.Text.ToLower()))
                        {
                            SearchResources.Add(i);
                            SearchGrid.Rows.Add(ind, ResourcesGrid.Rows[i].Cells[1].Value, ResourcesGrid.Rows[i].Cells[2].Value, Section);
                            ind++;
                        }
                    }
                }
                else if (ResourceSearchTrigger_Radio.Checked == true)
                {
                    int.TryParse(ResourceSearchTrigger.Text, out int IdIndex);
                    SearchResources = Read.ResourcesList.Select((item, index) => new { Item = item, Index = index }).Where(z => z.Item.Trigger_id == IdIndex).Select(o => o.Index).ToList();
                    for (int i = 0; i < SearchResources.Count; i++)
                    {
                        SearchGrid.Rows.Add(i + 1, ResourcesGrid.Rows[SearchResources[i]].Cells[1].Value, ResourcesGrid.Rows[SearchResources[i]].Cells[2].Value, Section);
                    }
                }
                SearchGrid.ScrollBars = ScrollBars.Vertical;
            }
        }
        private void DynamicSearchButton_Click(object sender, EventArgs e)
        {
            if (Read != null)
            {
                Action = 3;
                string Section = "Дин.Объекты";
                if (Language == 2)
                    Section = "Dyn.Objects";
                SearchDynamics = new List<int>();
                DynamicGrid.ScrollBars = ScrollBars.None;
                SearchGrid.Rows.Clear();
                if (DynamicSearchId_Radio.Checked == true)
                {
                    int.TryParse(DynamicSearchId.Text, out int IdIndex);
                    SearchDynamics = Read.DynamicsList.Select((item, index) => new { Item = item, Index = index }).Where(o => o.Item.Id == IdIndex).Select(o => o.Index).ToList();
                    for (int i = 0; i < SearchDynamics.Count(); i++)
                    {
                        SearchGrid.Rows.Add(i + 1, DynamicGrid.Rows[SearchDynamics[i]].Cells[1].Value, DynamicGrid.Rows[SearchDynamics[i]].Cells[2].Value, Section);
                    }
                }
                else if (DynamicSearchName_Radio.Checked == true)
                {
                    int ind = 1;
                    for (int i = 0; i < DynamicGrid.Rows.Count; i++)
                    {
                        if (DynamicGrid.Rows[i].Cells[2].Value.ToString().ToLower().Contains(DynamicSearchName.Text.ToLower()))
                        {
                            SearchDynamics.Add(i);
                            SearchGrid.Rows.Add(ind, DynamicGrid.Rows[i].Cells[1].Value, DynamicGrid.Rows[i].Cells[2].Value, Section);
                            ind++;
                        }
                    }
                }
                else if (DynamicSearchTrigger_Radio.Checked == true)
                {
                    int.TryParse(DynamicSearchTrigger.Text, out int IdIndex);
                    SearchDynamics = Read.DynamicsList.Select((item, index) => new { Item = item, Index = index }).Where(z => z.Item.TriggerId == IdIndex).Select(o => o.Index).ToList();
                    for (int i = 0; i < SearchDynamics.Count; i++)
                    {
                        SearchGrid.Rows.Add(i + 1, DynamicGrid.Rows[SearchDynamics[i]].Cells[1].Value, DynamicGrid.Rows[SearchDynamics[i]].Cells[2].Value, Section);
                    }
                }
                DynamicGrid.ScrollBars = ScrollBars.Vertical;
            }
        }
        private void TriggerSearchButton_Click(object sender, EventArgs e)
        {
            if (Read != null)
            {
                Action = 4;
                string Section = "Триггеры";
                if (Language == 2)
                    Section = "Triggers";
                SearchTriggers = new List<int>();
                SearchGrid.ScrollBars = ScrollBars.None;
                SearchGrid.Rows.Clear();
                if (TriggerSearchId_Radio.Checked == true)
                {
                    int.TryParse(TriggerSearchID.Text, out int IdIndex);
                    SearchTriggers = Read.TriggersList.Select((item, index) => new { Item = item, Index = index }).Where(o => o.Item.Id == IdIndex).Select(o => o.Index).ToList();
                    for (int i = 0; i < SearchTriggers.Count(); i++)
                    {
                        SearchGrid.Rows.Add(i + 1, TriggersGrid.Rows[SearchTriggers[i]].Cells[1].Value, TriggersGrid.Rows[SearchTriggers[i]].Cells[3].Value, Section);
                    }
                }
                else if (TriggerSearchGmId_Radio.Checked == true)
                {
                    int.TryParse(TriggerSearchGmID.Text, out int IdIndex);
                    SearchTriggers = Read.TriggersList.Select((item, index) => new { Item = item, Index = index }).Where(z => z.Item.GmID == IdIndex).Select(o => o.Index).ToList();
                    for (int i = 0; i < SearchTriggers.Count; i++)
                    {
                        SearchGrid.Rows.Add(i + 1, TriggersGrid.Rows[SearchTriggers[i]].Cells[1].Value, TriggersGrid.Rows[SearchTriggers[i]].Cells[3].Value, Section);
                    }
                }
                else if (TriggerSearchName_Radio.Checked == true)
                {
                    int ind = 1;
                    for (int i = 0; i < TriggersGrid.Rows.Count; i++)
                    {
                        if (TriggersGrid.Rows[i].Cells[3].Value.ToString().ToLower().Contains(TriggerSearchName.Text.ToLower()))
                        {
                            SearchTriggers.Add(i);
                            SearchGrid.Rows.Add(ind, TriggersGrid.Rows[i].Cells[1].Value, TriggersGrid.Rows[i].Cells[3].Value, Section);
                            ind++;
                        }
                    }
                }
                SearchGrid.ScrollBars = ScrollBars.Vertical;
            }
        }
        private void MoveToSelected_Click(object sender, EventArgs e)
        {
            try
            {
                if (Read != null)
                {
                    if (Action == 1 && SearchGrid.Rows.Count != 0)
                    {
                        NpcMobsGrid.CurrentCell = NpcMobsGrid.Rows[SearchMonsters[SearchGrid.SelectedRows[SearchGrid.SelectedRows.Count - 1].Index]].Cells[1];
                        foreach (DataGridViewRow p in SearchGrid.SelectedRows)
                        {
                            NpcMobsGrid.Rows[SearchMonsters[p.Index]].Selected = true;
                        }
                        MainTabControl.SelectedIndex = 0;
                        ExistenceGrid_CellChanged(null, null);
                    }
                    else if (Action == 2 && SearchGrid.Rows.Count != 0)
                    {
                        ResourcesGrid.CurrentCell = ResourcesGrid.Rows[SearchResources[SearchGrid.SelectedRows[SearchGrid.SelectedRows.Count - 1].Index]].Cells[1];
                        foreach (DataGridViewRow p in SearchGrid.SelectedRows)
                        {
                            ResourcesGrid.Rows[SearchResources[p.Index]].Selected = true;
                        }
                        MainTabControl.SelectedIndex = 1;
                        ResourcesGrid_CurrentCellChanged(null, null);
                    }
                    else if (Action == 3 && SearchGrid.Rows.Count != 0)
                    {
                        DynamicGrid.CurrentCell = DynamicGrid.Rows[SearchDynamics[SearchGrid.SelectedRows[SearchGrid.SelectedRows.Count - 1].Index]].Cells[1];
                        foreach (DataGridViewRow p in SearchGrid.SelectedRows)
                        {
                            DynamicGrid.Rows[SearchDynamics[p.Index]].Selected = true;
                        }
                        MainTabControl.SelectedIndex = 2;
                        DynamicGrid_CurrentCellChanged(null, null);
                    }
                    else if (Action == 4 && SearchGrid.Rows.Count != 0)
                    {
                        TriggersGrid.CurrentCell = TriggersGrid.Rows[SearchTriggers[SearchGrid.SelectedRows[SearchGrid.SelectedRows.Count - 1].Index]].Cells[1];
                        foreach (DataGridViewRow p in SearchGrid.SelectedRows)
                        {
                            TriggersGrid.Rows[SearchTriggers[p.Index]].Selected = true;
                        }
                        MainTabControl.SelectedIndex = 3;
                        TriggersGrid_CurrentCellChanged(null, null);
                    }
                }
            }
            catch
            {
                if (Language == 1)
                    MessageBox.Show("Invalid operation!!...", "NpcGen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Неверное действие!!...", "NpcGen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void TriggerSearchID_TextChanged(object sender, EventArgs e)
        {
            if (Read != null)
            {
                int.TryParse(TriggerSearchID.Text, out int Parsed);
                int index = Read.TriggersList.FindIndex(z => z.Id == Parsed);
                if (index != -1)
                {
                    TriggerSearchName.Text = TriggersGrid.Rows[index].Cells[3].Value.ToString();
                }
            }
        }
        private void TriggerSearchGmID_TextChanged(object sender, EventArgs e)
        {
            if (Read != null)
            {
                int.TryParse(TriggerSearchGmID.Text, out int Parsed);
                int index = Read.TriggersList.FindIndex(z => z.GmID == Parsed);
                if (index != -1)
                {
                    TriggerSearchName.Text = TriggersGrid.Rows[index].Cells[3].Value.ToString();
                }
            }
        }
        private void ExistenceSearchId_TextChanged(object sender, EventArgs e)
        {
            if (Read != null)
            {
                int.TryParse(ExistenceSearchId.Text, out int Parsed);
                int i = Element.ExistenceLists.FindIndex(z => z.Id == Parsed);
                if (i != -1)
                {
                    ExistenceSearchName.Text = Element.ExistenceLists[i].Name;
                }
                else
                {
                    ExistenceSearchName.Text = "";
                }
            }
        }
        private void ResourceSearchId_TextChanged(object sender, EventArgs e)
        {
            if (Read != null)
            {
                int.TryParse(ResourceSearchId.Text, out int Parsed);
                int i = Element.ResourcesList.FindIndex(z => z.Id == Parsed);
                if (i != -1)
                {
                    ResourceSearchName.Text = Element.ResourcesList[i].Name;
                }
                else
                {
                    ResourceSearchName.Text = "";
                }
            }
        }
        private void DynamicSearchId_TextChanged(object sender, EventArgs e)
        {
            if (Read != null)
            {
                int.TryParse(DynamicSearchId.Text, out int Parsed);
                if (Language == 1)
                {
                    int i = DynamicsListRu.FindIndex(z => z.Id == Parsed);
                    if (i != -1)
                    {
                        DynamicSearchName.Text = DynamicsListRu[i].Name;
                    }
                    else
                    {
                        DynamicSearchName.Text = "";
                    }

                }
                else if (Language == 2)
                {
                    int i = DynamicsListEn.FindIndex(z => z.Id == Parsed);
                    if (i != -1)
                    {
                        DynamicSearchName.Text = DynamicsListEn[i].Name;
                    }
                    else
                    {
                        DynamicSearchName.Text = "";
                    }
                }
            }
        }
        #endregion
        #region Errors
        private void MainTabControl_Click(object sender, EventArgs e)
        {
            Control co = sender as Control;
            switch (co.Name)
            {
                case "ExistenceSearchId":
                    {
                        ExistenceSearchId_Radio.Checked = true;
                        break;
                    }
                case "ExistenceSearchName":
                    {
                        ExistenceSearchName_Radio.Checked = true;
                        break;
                    }
                case "ExistenceSearchTrigger":
                    {
                        ExistenceSearchTrigger_Radio.Checked = true;
                        break;
                    }
                case "ExistenceSearchPath":
                    {
                        ExistenceSearchPath_Radio.Checked = true;
                        break;
                    }
                case "ResourceSearchId":
                    {
                        ResourceSearchId_Radio.Checked = true;
                        break;
                    }
                case "ResourceSearchName":
                    {
                        ResourceSearchName_Radio.Checked = true;
                        break;
                    }
                case "ResourceSearchTrigger":
                    {
                        ResourceSearchTrigger_Radio.Checked = true;
                        break;
                    }
                case "DynamicSearchId":
                    {
                        DynamicSearchId_Radio.Checked = true;
                        break;
                    }
                case "DynamicSearchName":
                    {
                        DynamicSearchName_Radio.Checked = true;
                        break;
                    }
                case "DynamicSearchTrigger":
                    {
                        DynamicSearchTrigger_Radio.Checked = true;
                        break;
                    }
                case "TriggerSearchID":
                    {
                        TriggerSearchId_Radio.Checked = true;
                        break;
                    }
                case "TriggerSearchGmID":
                    {
                        TriggerSearchGmId_Radio.Checked = true;
                        break;
                    }
                case "TriggerSearchName":
                    {
                        TriggerSearchName_Radio.Checked = true;
                        break;
                    }


            }
        }
        private void DId_numeric_DoubleClick(object sender, EventArgs e)
        {
            DynamicForm.SetWindow = 1;
            DynamicForm.ShowDialog(this);
        }
        private void SearchGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            MoveToSelected_Click(null, null);
        }
        private void SearchErrorsButton_Click(object sender, EventArgs e)
        {
            if (Read != null)
            {
                ErrorExistenceCollection = new List<IntDictionary>();
                ErrorResourcesCollection = new List<IntDictionary>();
                ErrorDynamicsCollection = new List<IntDictionary>();
                ErrorsLanguage = Language;
                string Monsters = "Мобы и Нпс";
                string Resources = "Ресурсы";
                string Dynamics = "Динамические объекты";
                string AmountInGroup = "Количество объектов в группе не может быть равно 0";
                string Trigger = "Триггер не найден";
                string ObjectID = "Неизвестный ID";
                string ObjectAmount = "Количество объекта не может быть равно 0";
                if (Language == 2)
                {
                    Monsters = "Mobs And Npcs";
                    Resources = "Resources";
                    Dynamics = "Dynamic Objects";
                    AmountInGroup = "Objects amount in group can't be equal to 0";
                    Trigger = "Trigger not found";
                    ObjectID = "Unknown ID";
                    ObjectAmount = "Object amount can't be equal to 0";
                }
                MainProgressBar.Maximum = Read.NpcMobsAmount + Read.ResourcesAmount + Read.DynobjectAmount;
                ErrorsGrid.ScrollBars = ScrollBars.None;
                ErrorsGrid.Rows.Clear();
                int RowsCount = 1;
                #region Existence
                for (int i = 0; i < Read.NpcMobsAmount; i++)
                {
                    if (Read.NpcMobList[i].Amount_in_group == 0)
                    {
                        ErrorsGrid.Rows.Add(RowsCount, i + 1, Monsters, AmountInGroup);
                        ErrorExistenceCollection.Add(new IntDictionary(RowsCount - 1, i));
                        RowsCount++;
                    }
                    if (Read.NpcMobList[i].Trigger_id != 0)
                    {
                        if (Read.TriggersList.FindIndex(f => f.Id == Read.NpcMobList[i].Trigger_id) == -1)
                        {
                            ErrorsGrid.Rows.Add(RowsCount, i + 1, Monsters, Trigger);
                            ErrorExistenceCollection.Add(new IntDictionary(RowsCount - 1, i));
                            RowsCount++;
                        }
                    }
                    for (int z = 0; z < Read.NpcMobList[i].Amount_in_group; z++)
                    {
                        if (Element.ExistenceLists.FindIndex(v => v.Id == Read.NpcMobList[i].MobDops[z].Id) == -1)
                        {
                            ErrorsGrid.Rows.Add(RowsCount, i + 1, Monsters, ObjectID);
                            ErrorExistenceCollection.Add(new IntDictionary(RowsCount - 1, i));
                            RowsCount++;
                            break;
                        }
                    }
                    for (int b = 0; b < Read.NpcMobList[i].Amount_in_group; b++)
                    {
                        if (Read.NpcMobList[i].MobDops[b].Amount == 0)
                        {
                            ErrorsGrid.Rows.Add(RowsCount, i + 1, Monsters, ObjectAmount);
                            ErrorExistenceCollection.Add(new IntDictionary(RowsCount - 1, i));
                            RowsCount++;
                            break;
                        }
                    }
                    MainProgressBar.Value++;
                }
                #endregion
                #region Resources
                for (int i = 0; i < Read.ResourcesAmount; i++)
                {
                    if (Read.ResourcesList[i].Amount_in_group == 0)
                    {
                        ErrorsGrid.Rows.Add(RowsCount, i + 1, Resources, AmountInGroup);
                        ErrorResourcesCollection.Add(new IntDictionary(RowsCount - 1, i));
                        RowsCount++;
                    }
                    if (Read.ResourcesList[i].Trigger_id != 0)
                    {
                        if (Read.TriggersList.FindIndex(f => f.Id == Read.ResourcesList[i].Trigger_id) == -1)
                        {
                            ErrorsGrid.Rows.Add(RowsCount, i + 1, Resources, Trigger);
                            ErrorResourcesCollection.Add(new IntDictionary(RowsCount - 1, i));
                            RowsCount++;
                        }
                    }
                    for (int z = 0; z < Read.ResourcesList[i].Amount_in_group; z++)
                    {
                        if (Element.ResourcesList.FindIndex(v => v.Id == Read.ResourcesList[i].ResExtra[z].Id) == -1)
                        {
                            ErrorsGrid.Rows.Add(RowsCount, i + 1, Resources, ObjectID);
                            ErrorResourcesCollection.Add(new IntDictionary(RowsCount - 1, i));
                            RowsCount++;
                            break;
                        }
                    }
                    for (int b = 0; b < Read.ResourcesList[i].Amount_in_group; b++)
                    {
                        if (Read.ResourcesList[i].ResExtra[b].Amount == 0)
                        {
                            ErrorsGrid.Rows.Add(RowsCount, i + 1, Resources, ObjectAmount);
                            ErrorResourcesCollection.Add(new IntDictionary(RowsCount - 1, i));
                            RowsCount++;
                            break;
                        }
                    }
                    MainProgressBar.Value++;
                }
                #endregion
                #region Dynamics
                for (int i = 0; i < Read.DynobjectAmount; i++)
                {
                    if (Read.DynamicsList[i].TriggerId != 0)
                    {
                        if (Read.TriggersList.FindIndex(z => z.Id == Read.DynamicsList[i].TriggerId) == -1)
                        {
                            ErrorsGrid.Rows.Add(RowsCount, i + 1, Dynamics, Trigger);
                            ErrorDynamicsCollection.Add(new IntDictionary(RowsCount - 1, i));
                            RowsCount++;
                        }
                    }
                    MainProgressBar.Value++;
                }
                #endregion
                MainProgressBar.Value = 0;
                ErrorsGrid.ScrollBars = ScrollBars.Vertical;
            }
        }
        private void ErrorsGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string SecondCellText = ErrorsGrid.Rows[e.RowIndex].Cells[2].Value.ToString();
                if (SecondCellText == "Мобы и Нпс" || SecondCellText == "Mobs And Npcs")
                {
                    NpcMobsGrid.CurrentCell = NpcMobsGrid.Rows[Convert.ToInt32(ErrorsGrid.Rows[e.RowIndex].Cells[1].Value) - 1].Cells[1];
                    MainTabControl.SelectedIndex = 0;
                }
                else if (SecondCellText == "Ресурсы" || SecondCellText == "Resources")
                {
                    ResourcesGrid.CurrentCell = ResourcesGrid.Rows[Convert.ToInt32(ErrorsGrid.Rows[e.RowIndex].Cells[1].Value) - 1].Cells[1];
                    MainTabControl.SelectedIndex = 1;
                }
                else if (SecondCellText == "Динамические объекты" || SecondCellText == "Dynamic Objects")
                {
                    DynamicGrid.CurrentCell = DynamicGrid.Rows[Convert.ToInt32(ErrorsGrid.Rows[e.RowIndex].Cells[1].Value) - 1].Cells[1];
                    MainTabControl.SelectedIndex = 2;
                }
            }
            catch
            {
                if (Language == 1)
                    MessageBox.Show("Объект не существует!!...", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Object doesn't exist!!...", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void RemoveAllErrors_Click(object sender, EventArgs e)
        {
            string Dialog = "Вы уверены,что хотите удалить все объекты с ошибками?";
            if (Language == 2)
                Dialog = "Are you sure that you want to delete all objects with errors?";
            DialogResult dg = MessageBox.Show(Dialog, "Npcgen Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dg == DialogResult.Yes)
            {
                NpcMobsGrid.ScrollBars = ScrollBars.None;
                ResourcesGrid.ScrollBars = ScrollBars.None;
                DynamicGrid.ScrollBars = ScrollBars.None;
                ErrorsGrid.Rows.Clear();
                List<int> Existence = new List<int>();
                List<int> Resources = new List<int>();
                List<int> Dynamics = new List<int>();
                ErrorExistenceCollection = ErrorExistenceCollection.OrderByDescending(r => r.GridIndex).ToList();
                MainProgressBar.Maximum = ErrorExistenceCollection.Count + ErrorResourcesCollection.Count + ErrorDynamicsCollection.Count;
                foreach (var k in ErrorExistenceCollection)
                {
                    if (Existence.FindIndex(f => f == k.GridIndex) == -1)
                    {
                        NpcMobsGrid.Rows.RemoveAt(k.GridIndex);
                        Read.NpcMobList.RemoveAt(k.GridIndex);
                        Existence.Add(k.GridIndex);
                    }
                    MainProgressBar.Value++;
                }
                Read.NpcMobsAmount = Read.NpcMobList.Count;
                ErrorExistenceCollection.Clear();
                ExistenceGrid_CellChanged(null, null);

                ErrorResourcesCollection = ErrorResourcesCollection.OrderByDescending(r => r.GridIndex).ToList();
                foreach (var k in ErrorResourcesCollection)
                {
                    if (Resources.FindIndex(f => f == k.GridIndex) == -1)
                    {
                        ResourcesGrid.Rows.RemoveAt(k.GridIndex);
                        Read.ResourcesList.RemoveAt(k.GridIndex);
                        Resources.Add(k.GridIndex);
                    }
                    MainProgressBar.Value++;
                }
                Read.ResourcesAmount = Read.ResourcesList.Count;
                ErrorResourcesCollection.Clear();
                ResourcesGrid_CurrentCellChanged(null, null);
                ErrorDynamicsCollection = ErrorDynamicsCollection.OrderByDescending(r => r.GridIndex).ToList();
                foreach (var k in ErrorDynamicsCollection)
                {
                    if (Dynamics.FindIndex(f => f == k.GridIndex) == -1)
                    {
                        DynamicGrid.Rows.RemoveAt(k.GridIndex);
                        Read.DynamicsList.RemoveAt(k.GridIndex);
                        Dynamics.Add(k.GridIndex);
                    }
                    MainProgressBar.Value++;
                }
                NpcMobsGrid.ScrollBars = ScrollBars.Vertical;
                ResourcesGrid.ScrollBars = ScrollBars.Vertical;
                DynamicGrid.ScrollBars = ScrollBars.Vertical;
                if (NpcMobsGrid.CurrentCell != null)
                    NpcMobsGrid.FirstDisplayedScrollingRowIndex = NpcMobsGrid.CurrentCell.RowIndex;
                if (ResourcesGrid.CurrentCell != null)
                    ResourcesGrid.FirstDisplayedScrollingRowIndex = ResourcesGrid.CurrentCell.RowIndex;
                if (DynamicGrid.CurrentCell != null)
                    DynamicGrid.FirstDisplayedScrollingRowIndex = DynamicGrid.CurrentCell.RowIndex;
                Read.DynobjectAmount = Read.DynamicsList.Count;
                ErrorDynamicsCollection.Clear();
                DynamicGrid_CurrentCellChanged(null, null);
                MainProgressBar.Value = 0;
            }
        }
        #endregion
        private void GetAllControls(Control container, ref List<Control> ControlList)
        {
            foreach (Control c in container.Controls)
            {
                GetAllControls(c, ref ControlList);
                if (c is TabPage || c is GroupBox || c is RadioButton)
                {
                    ControlList.Add(c);
                }
            }
        }
        private void GetAllLabels(Control container, ref List<Control> ControlList)
        {
            foreach (Control c in container.Controls)
            {
                GetAllLabels(c, ref ControlList);
                if (c is Label || c is RadioButton || c is GroupBox)
                {
                    ControlList.Add(c);
                }
            }
        }
        private void GetAllTextBoxs(Control container, ref List<Control> ControlList)
        {
            foreach (Control c in container.Controls)
            {
                GetAllTextBoxs(c, ref ControlList);
                if (c is TextBox)
                {
                    ControlList.Add(c);
                }
            }
        }
        private void InterfaceColorChanged(object sender, EventArgs e)
        {
            if ((sender as Control).Name == "Dark")
            {
                InterfaceColor = 2;
                this.BackColor = Color.FromArgb(58, 58, 58);
                List<Control> BackGrounds = new List<Control>();
                GetAllControls(this, ref BackGrounds);
                foreach (var item in BackGrounds)
                {
                    item.BackColor = Color.FromArgb(58, 58, 58);
                }
                List<Control> ForeColors = new List<Control>();
                GetAllLabels(this, ref ForeColors);
                foreach (var item in ForeColors)
                {
                    item.ForeColor = Color.FromArgb(220, 220, 220);
                }
                DynamicPictureBox.BackColor = Color.FromArgb(58, 58, 58);
                List<Control> TextBoxs = new List<Control>();
                GetAllTextBoxs(this, ref TextBoxs);
                foreach (var item in TextBoxs)
                {
                    item.BackColor = Color.FromArgb(75, 75, 75);
                    item.ForeColor = Color.FromArgb(243, 243, 243);
                }
                ExistenceToolStrip.BackColor = Color.FromArgb(58, 58, 58);
                toolStrip1.BackColor = Color.FromArgb(58, 58, 58);
                toolStrip2.BackColor = Color.FromArgb(58, 58, 58);
                toolStrip3.BackColor = Color.FromArgb(58, 58, 58);
                ExportExistence.ForeColor = Color.FromArgb(240, 240, 240);
                ImportExistence.ForeColor = Color.FromArgb(240, 240, 240);
                ExportResources.ForeColor = Color.FromArgb(240, 240, 240);
                ImportResources.ForeColor = Color.FromArgb(240, 240, 240);
                LineUpResource.ForeColor = Color.FromArgb(240, 240, 240);
                MoveResources.ForeColor = Color.FromArgb(240, 240, 240);
                toolStripButton3.ForeColor = Color.FromArgb(240, 240, 240);
                toolStripButton4.ForeColor = Color.FromArgb(240, 240, 240);
                toolStripDropDownButton3.ForeColor = Color.FromArgb(240, 240, 240);
                toolStripDropDownButton4.ForeColor = Color.FromArgb(240, 240, 240);
                toolStripButton5.ForeColor = Color.FromArgb(240, 240, 240);
                toolStripButton6.ForeColor = Color.FromArgb(240, 240, 240);
                toolStripButton7.ForeColor = Color.FromArgb(240, 240, 240);
                toolStripDropDownButton6.ForeColor = Color.FromArgb(240, 240, 240);
                LineUpExistenceDropDown.ForeColor = Color.FromArgb(240, 240, 240);
                MoveExistenceDropDown.ForeColor = Color.FromArgb(240, 240, 240);
                Agression.BackColor = Color.FromArgb(90, 90, 90);
                Path_type.BackColor = Color.FromArgb(90, 90, 90);
                Agression.ForeColor = Color.FromArgb(245, 245, 245);
                Path_type.ForeColor = Color.FromArgb(245, 245, 245);
            }
            else
            {
                InterfaceColor = 1;
                this.BackColor = Color.FromArgb(239, 244, 250);
                List<Control> BackGrounds = new List<Control>();
                GetAllControls(this, ref BackGrounds);
                foreach (var item in BackGrounds)
                {
                    item.BackColor = Color.FromArgb(239, 244, 250);
                }
                List<Control> ForeColors = new List<Control>();
                GetAllLabels(this, ref ForeColors);
                foreach (var item in ForeColors)
                {
                    item.ForeColor = SystemColors.ControlText;
                }
                DynamicPictureBox.BackColor = Color.FromArgb(239, 244, 250);
                List<Control> TextBoxs = new List<Control>();
                GetAllTextBoxs(this, ref TextBoxs);
                foreach (var item in TextBoxs)
                {
                    item.BackColor = SystemColors.Window;
                    item.ForeColor = SystemColors.ControlText;
                }
                ExistenceToolStrip.BackColor = Color.FromArgb(239, 244, 250);
                toolStrip1.BackColor = Color.FromArgb(239, 244, 250);
                toolStrip2.BackColor = Color.FromArgb(239, 244, 250);
                toolStrip3.BackColor = Color.FromArgb(239, 244, 250);
                ExportExistence.ForeColor = SystemColors.ControlText;
                ImportExistence.ForeColor = SystemColors.ControlText;
                ExportResources.ForeColor = SystemColors.ControlText;
                ImportResources.ForeColor = SystemColors.ControlText;
                LineUpResource.ForeColor = SystemColors.ControlText;
                MoveResources.ForeColor = SystemColors.ControlText;
                toolStripButton3.ForeColor = SystemColors.ControlText;
                toolStripButton4.ForeColor = SystemColors.ControlText;
                toolStripDropDownButton3.ForeColor = SystemColors.ControlText;
                toolStripDropDownButton4.ForeColor = SystemColors.ControlText;
                toolStripButton5.ForeColor = SystemColors.ControlText;
                toolStripButton6.ForeColor = SystemColors.ControlText;
                toolStripButton7.ForeColor = SystemColors.ControlText;
                toolStripDropDownButton6.ForeColor = SystemColors.ControlText;
                LineUpExistenceDropDown.ForeColor = SystemColors.ControlText;
                MoveExistenceDropDown.ForeColor = SystemColors.ControlText;
                Agression.BackColor = Color.FromArgb(255, 192, 128);
                Path_type.BackColor = Color.FromArgb(255, 192, 128);
                Agression.ForeColor = SystemColors.ControlText;
                Path_type.ForeColor = SystemColors.ControlText;
            }
            Id_numeric.BackColor = Color.FromArgb(128, 255, 128);
            RId_numeric.BackColor = Color.FromArgb(128, 255, 128);
            DId_numeric.BackColor = Color.FromArgb(128, 255, 128);
            Id_numeric.ForeColor = Color.Black;
            RId_numeric.ForeColor = Color.Black;
            DId_numeric.ForeColor = Color.Black;
            groupBox23.BackColor = Color.FromArgb(128, 255, 128);
            groupBox23.ForeColor = Color.Black;
            Clear.ForeColor = Color.Black;
            Dark.ForeColor = Color.Black;
            Clear.BackColor = Color.Transparent;
            Dark.BackColor = Color.Transparent;
            Label_DynamicName.ForeColor = Color.Red;
            if (DynamicForm != null)
            {
                DynamicForm.SetColor(InterfaceColor);
            }
        }
        public void ShowWrongFile()
        {
            if (Language == 1)
            {
                MessageBox.Show("Неверный файл!!...", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Wrong file!!...", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void ExportExistence_Click(object sender, EventArgs e)
        {
            if (Read != null)
            {
                SaveFileDialog ofd = new SaveFileDialog();
                if (MainTabControl.SelectedIndex == 0)
                {
                    if (NpcRowCollection.Count != 0)
                    {
                        ofd.FileName = string.Format("Npcgen Existences[{0}]", NpcRowCollection.Count);
                        ofd.Filter = "Npcgen Existence | *.nblee";
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            Read.ExportExistence(ofd.FileName, NpcRowCollection);
                        }
                    }
                }
                else if (MainTabControl.SelectedIndex == 1)
                {
                    if (ResourcesRowCollection.Count != 0)
                    {
                        ofd.FileName = string.Format("Npcgen Resources[{0}]", ResourcesRowCollection.Count);
                        ofd.Filter = "Npcgen Resource | *.nbler";
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            Read.ExportResource(ofd.FileName, ResourcesRowCollection);
                        }
                    }
                }
                else if (MainTabControl.SelectedIndex == 2)
                {
                    if (DynamicsRowCollection.Count != 0)
                    {
                        ofd.FileName = string.Format("Npcgen Dynamic Objects[{0}]", DynamicsRowCollection.Count);
                        ofd.Filter = "Npcgen Dynamic Objects | *.nbled";
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            Read.ExportDynamics(ofd.FileName, DynamicsRowCollection);
                        }
                    }
                }
                else if (MainTabControl.SelectedIndex == 3)
                {
                    if (TriggersRowCollection.Count != 0)
                    {
                        ofd.FileName = string.Format("Npcgen Triggers[{0}]", TriggersRowCollection.Count);
                        ofd.Filter = "Npcgen Triggers | *.nblet";
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            Read.ExportTriggers(ofd.FileName, TriggersRowCollection);
                        }
                    }
                }
            }
            else
            {
                if (Language == 1)
                {
                    MessageBox.Show("Файл не загружен!!...", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("File isn't loaded!!...", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void ImportExistence_Click(object sender, EventArgs e)
        {
            if (Read != null)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (MainTabControl.SelectedIndex == 0)
                {
                    ofd.FileName = "Npcgen Exported Existence";
                    ofd.Filter = "Npcgen Existence | *.nblee";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        BinaryReader br = new BinaryReader(File.Open(ofd.FileName, FileMode.Open));
                        if (Encoding.Default.GetString(br.ReadBytes(33)).Split(new string[] { "||" }, StringSplitOptions.None).ElementAt(1) == "Existence")
                        {
                            int Amount = br.ReadInt32();
                            for (int i = 0; i < Amount; i++)
                            {
                                ClassDefaultMonsters mo = Read.ReadExistence(br, 15);
                                int[] Id_joined = new int[mo.Amount_in_group];
                                string[] Names_joined = new string[mo.Amount_in_group];
                                for (int j = 0; j < mo.Amount_in_group; j++)
                                {
                                    Id_joined[j] = mo.MobDops[j].Id;
                                    int ind = Element.ExistenceLists.FindIndex(v => v.Id == Id_joined[j]);
                                    if (ind != -1)
                                    {
                                        Names_joined[j] = Element.ExistenceLists[ind].Name;
                                    }
                                    else
                                    {
                                        Names_joined[j] = "?";
                                    }
                                }
                                NpcMobsGrid.Rows.Add(NpcMobsGrid.Rows.Count + 1, string.Join(",", Id_joined), string.Join(",", Names_joined));
                                Read.NpcMobList.Add(mo);
                                Read.NpcMobsAmount++;
                            }
                            NpcMobsGrid.CurrentCell = NpcMobsGrid.Rows[NpcMobsGrid.Rows.Count - 1].Cells[1];
                            for (int z = 0; z < Amount; z++)
                            {
                                NpcMobsGrid.Rows[NpcMobsGrid.Rows.Count - 1 - z].Selected = true;
                            }
                            ExistenceGrid_CellChanged(null, null);
                        }
                        else
                        {
                            ShowWrongFile();
                            return;
                        }
                        br.Close();
                    }
                }
                else if (MainTabControl.SelectedIndex == 1)
                {
                    ofd.FileName = "Npcgen Exported Resources";
                    ofd.Filter = "Npcgen Resources | *.nbler";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        BinaryReader br = new BinaryReader(File.Open(ofd.FileName, FileMode.Open));
                        if (Encoding.Default.GetString(br.ReadBytes(33)).Split(new string[] { "||" }, StringSplitOptions.None).ElementAt(1) == "Resources")
                        {
                            int Amount = br.ReadInt32();
                            for (int i = 0; i < Amount; i++)
                            {
                                ClassDefaultResources mo = Read.ReadResource(br, 15);
                                int[] Id_joined = new int[mo.Amount_in_group];
                                string[] Names_joined = new string[mo.Amount_in_group];
                                for (int j = 0; j < mo.Amount_in_group; j++)
                                {
                                    Id_joined[j] = mo.ResExtra[j].Id;
                                    int ind = Element.ResourcesList.FindIndex(v => v.Id == Id_joined[j]);
                                    if (ind != -1)
                                    {
                                        Names_joined[j] = Element.ResourcesList[ind].Name;
                                    }
                                    else
                                    {
                                        Names_joined[j] = "?";
                                    }
                                }
                                ResourcesGrid.Rows.Add(ResourcesGrid.Rows.Count + 1, string.Join(",", Id_joined), string.Join(",", Names_joined));
                                Read.ResourcesList.Add(mo);
                                Read.ResourcesAmount++;
                            }
                            ResourcesGrid.CurrentCell = ResourcesGrid.Rows[ResourcesGrid.Rows.Count - 1].Cells[1];
                            for (int z = 0; z < Amount; z++)
                            {
                                ResourcesGrid.Rows[ResourcesGrid.Rows.Count - 1 - z].Selected = true;
                            }
                            ResourcesGrid_CurrentCellChanged(null, null);
                        }
                        else
                        {
                            ShowWrongFile();
                            return;
                        }
                    }
                }
                else if (MainTabControl.SelectedIndex == 2)
                {
                    ofd.FileName = "Npcgen Exported Dynamic Objects";
                    ofd.Filter = "Npcgen Dynamic Objects | *.nbled";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        BinaryReader br = new BinaryReader(File.Open(ofd.FileName, FileMode.Open));
                        if (Encoding.Default.GetString(br.ReadBytes(33)).Split(new string[] { "||" }, StringSplitOptions.None).ElementAt(1) == "DynObject")
                        {
                            int Amount = br.ReadInt32();
                            for (int i = 0; i < Amount; i++)
                            {
                                ClassDynamicObject dn = Read.ReadDynObjects(br, 15);
                                DynamicGrid.Rows.Add(DynamicGrid.Rows.Count + 1, dn.Id,GetDynamicName(dn.Id),dn.TriggerId);
                                Read.DynamicsList.Add(dn);
                                Read.DynobjectAmount++;
                            }
                            DynamicGrid.CurrentCell = DynamicGrid.Rows[DynamicGrid.Rows.Count - 1].Cells[1];
                            for (int z = 0; z < Amount; z++)
                            {
                                DynamicGrid.Rows[DynamicGrid.Rows.Count - 1 - z].Selected = true;
                            }
                            DynamicGrid_CurrentCellChanged(null, null);
                        }
                    }
                }
                else if (MainTabControl.SelectedIndex ==3)
                {
                    ofd.FileName = "Npcgen Exported Triggers";
                    ofd.Filter = "Npcgen Triggers | *.nblet";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        BinaryReader br = new BinaryReader(File.Open(ofd.FileName, FileMode.Open));
                        if (Encoding.Default.GetString(br.ReadBytes(33)).Split(new string[] { "||" }, StringSplitOptions.None).ElementAt(1) == "Triggerss")
                        {
                            int Amount = br.ReadInt32();
                            for (int i = 0; i < Amount; i++)
                            {
                                ClassTrigger dn = Read.ReadTrigger(br, 15);
                                TriggersGrid.Rows.Add(TriggersGrid.Rows.Count + 1, dn.Id, dn.GmID, dn.TriggerName);
                                Read.TriggersList.Add(dn);
                                Read.TriggersAmount++;
                            }
                            TriggersGrid.CurrentCell = TriggersGrid.Rows[TriggersGrid.Rows.Count - 1].Cells[1];
                            for (int z = 0; z < Amount; z++)
                            {
                                TriggersGrid.Rows[TriggersGrid.Rows.Count - 1 - z].Selected = true;
                            }
                            TriggersGrid_CurrentCellChanged(null, null);
                        }
                    }
                }
            }
            else
            {
                if (Language == 1)
                {
                    MessageBox.Show("Файл не загружен!!...", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("File isn't loaded!!...", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }
        private void ExistenceSearchId_DoubleClick(object sender, EventArgs e)
        {
            if (ChooseFromElementsForm != null)
            {
                int.TryParse(ExistenceSearchId.Text, out int b);
                int Ind = Element.ExistenceLists.FindIndex(z => z.Id == b);
                ChooseFromElementsForm.SetAction = 1;
                ChooseFromElementsForm.SetWindow = 2;
                if (Ind != -1)
                {
                    if (Ind >= Element.MonsterdAmount)
                    {
                        ChooseFromElementsForm.FindRow(Ind - Element.MonsterdAmount, "Npc");
                    }
                    else
                    {
                        ChooseFromElementsForm.FindRow(Ind, "Mob");
                    }
                }
                ChooseFromElementsForm.ShowDialog(this);
            }
        }
        private void ResourceSearchId_DoubleClick(object sender, EventArgs e)
        {
            if (ChooseFromElementsForm != null)
            {
                ChooseFromElementsForm.SetAction = 2;
                ChooseFromElementsForm.SetWindow = 2;
                int.TryParse(ResourceSearchId.Text, out int b);
                int ind = Element.ResourcesList.FindIndex(z => z.Id == b);
                if (ind != -1)
                {
                    ChooseFromElementsForm.FindRow(ind, "Resource");
                }
                ChooseFromElementsForm.ShowDialog(this);
            }
        }
        private void DynamicSearchId_DoubleClick(object sender, EventArgs e)
        {
            DynamicForm.SetWindow = 2;
            DynamicForm.ShowDialog(this);
        }

        private void ExistenceToEnd_Click(object sender, EventArgs e)
        {
            if(Read!=null && NpcMobsGrid.CurrentRow!=null)
            {
                if(NpcMobsGrid.CurrentRow.Index!=-1)
                {
                    var t = Read.NpcMobList[NpcMobsGrid.CurrentRow.Index];
                    Read.NpcMobList.RemoveAt(NpcMobsGrid.CurrentRow.Index);
                    Read.NpcMobList.Insert(Read.NpcMobList.Count, t);
                    DataGridViewRow dg = NpcMobsGrid.Rows[NpcMobsGrid.CurrentRow.Index];
                    NpcMobsGrid.Rows.Remove(dg);
                    NpcMobsGrid.Rows.Insert(NpcMobsGrid.Rows.Count,dg);
                    NpcMobsGrid.CurrentCell = NpcMobsGrid.Rows[NpcMobsGrid.Rows.Count-1].Cells[1];
                }
            }
        }
    }
}
