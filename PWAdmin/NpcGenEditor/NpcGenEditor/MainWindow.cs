using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.Xpo.DB;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting.Export.Pdf;
using DevExpress.XtraPrinting.Native;
using NpcGenEditor.Classes;
using NpcGenEditor.DDSReader;
using NpcGenEditor.Elements;
using NpcGenEditor.PCKEngine;
using NpcGenEditor.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static DevExpress.Utils.Frames.FrameHelper;
using static System.Collections.Specialized.BitVector32;

namespace NpcGenEditor
{
    public partial class MainWindow : DevExpress.XtraEditors.XtraForm
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        NpcGen npcGen = new NpcGen();
        public static eListCollection eLC;
        public static List<GameMapInfo> Maps;
        MapWindow MapForm = null;
        public static List<MapLoadedInformation> LoadedMapConfigs = new List<MapLoadedInformation>();
        public static SortedList<int, string> imagesx_m;
        public static SortedList<int, ImagePosition> imageposition_m;
        public static SortedDictionary<string, Bitmap> imagesCache_m = new SortedDictionary<string, Bitmap>();
        public SortedList<int, string> listMonster = new SortedList<int, string>();
        public static Bitmap iconlist_ivtrm { get; set; }
        public static bool loadedData = false;
        public static bool loadedPck = false;

        void ReadDatas()
        {
            loadedData = false;
            loadedPck = false;
            try
            {
                npcGen = new NpcGen();
                listMonster = new SortedList<int, string>();
                listnpcmob.Items.Clear();
                listnpcmob.DataSource = null;
                eLC = null;
                BinaryReader b = new BinaryReader(File.Open(npcgen.Text, FileMode.Open));
                ReadElements();
                Task t = Task.Factory.StartNew(() => npcGen.ReadNpcgen(b));
                t.Wait();
                PopulateListBoxNpcMob();
            }
            catch (Exception e)
            {
                XtraMessageBox.Show($"{e.Message}\n{e.StackTrace}");
            }
        }

        void PopulateListBoxNpcMob()
        {
            if (!loadedData) return;
            if (npcGen != null)
            {
                UpdateTable();
            }
        }

        public void ReadElements()
        {
            try
            {
                eLC = new eListCollection(this, elements.Text, ref pb1);
                if (eLC.ConfigFile != null)
                {
                    string[] files = Directory.GetFiles(Application.StartupPath + "\\rules", "PW_v" + eLC.Version.ToString() + "*.rules");
                    for (int i = 0; i < files.Length; i++)
                    {
                        files[i] = files[i].Replace("=", "=>");
                        files[i] = files[i].Replace(".rules", string.Empty);
                        files[i] = files[i].Replace(Application.StartupPath + "\\rules\\", string.Empty);
                    }
                }
            }
            catch (Exception e)
            {
                XtraMessageBox.Show(e.Message);
                loadedData = false;
            }
            finally
            {
                loadedData = true;
                if (eLC.Lists[38].elementFields[2] == "Name")
                {
                    for (int i = 0; i < eLC.Lists[38].elementValues.Length; i++)
                    {
                        int id = int.Parse(eLC.GetValue(38, i, 0));
                        string name = eLC.GetValue(38, i, 2);
                        if (!listMonster.ContainsKey(id))
                        {
                            listMonster.Add(id, name);
                        }
                        else
                        {
                            MessageBox.Show("Error: Found duplicate Monster ID:" + id);
                            listMonster[id] = name;
                        }
                    }
                }
            }
        }

        public static PCKFileEntry[] ReadFileTable(PCKStream stream)
        {
            stream.Seek(-8, SeekOrigin.End);
            int FilesCount = stream.ReadInt32();
            stream.Seek(-272, SeekOrigin.End);
            long FileTableOffset = (long)((ulong)((uint)(stream.ReadUInt32() ^ (ulong)stream.key.KEY_1)));
            PCKFileEntry[] entrys = new PCKFileEntry[FilesCount];
            stream.Seek(FileTableOffset, SeekOrigin.Begin);
            for (int i = 0; i < entrys.Length; ++i)
            {
                int EntrySize = stream.ReadInt32() ^ stream.key.KEY_1;
                stream.ReadInt32();
                entrys[i] = new PCKFileEntry(stream.ReadBytes(EntrySize));
            }
            return entrys;
        }

        public static byte[] ReadFile(PCKStream stream, PCKFileEntry file)
        {
            stream.Seek(file.Offset, SeekOrigin.Begin);
            byte[] bytes = stream.ReadBytes(file.CompressedSize);
            return file.CompressedSize < file.Size ? PCKZlib.Decompress(bytes, file.Size) : bytes;
        }

        public void ReadItemIcon()
        {
            try
            {
                var streamSurfaces = new PCKStream(surfaces.Text);
                var rftSurfaces = ReadFileTable(streamSurfaces);
                string iconlist_ivtrm = "icon\\iconlist_ivtrm.png";
                FileInfo fileInfo_m = new FileInfo(iconlist_ivtrm);
                string iconlist_ivtrf = "icon\\iconlist_ivtrf.png";
                FileInfo fileInfo_f = new FileInfo(iconlist_ivtrf);
                string iconlist_skill = "icon\\iconlist_skill.png";
                FileInfo fileInfo_skill = new FileInfo(iconlist_skill);
                DirectoryInfo dirInfo = new DirectoryInfo("icon");
                if (!dirInfo.Exists)
                    Directory.CreateDirectory("icon");
                if (fileInfo_m.Exists)
                    fileInfo_m.Delete();
                if (fileInfo_f.Exists)
                    fileInfo_f.Delete();
                if (fileInfo_skill.Exists)
                    fileInfo_skill.Delete();
                for (int i = 0; i < rftSurfaces.Length; i++)
                {
                    if (rftSurfaces[i].Path == "surfaces\\iconset\\iconlist_ivtrm.dds")
                    {
                        var read = ReadFile(streamSurfaces, rftSurfaces[i]);
                        File.WriteAllBytes("icon\\iconlist_ivtrm.dds", read);
                        DDSImagem dds = new DDSImagem("icon\\iconlist_ivtrm.dds");
                        dds.Save("icon\\iconlist_ivtrm.png");
                        File.Delete("icon\\iconlist_ivtrm.dds");
                    }
                    else if (rftSurfaces[i].Path == "surfaces\\iconset\\iconlist_ivtrm.txt")
                    {
                        var read = ReadFile(streamSurfaces, rftSurfaces[i]);
                        StreamReader file = new StreamReader(new MemoryStream(read), Encoding.GetEncoding("GBK"));
                        string line;
                        List<string> fileNames = new List<string>();
                        imagesx_m = new SortedList<int, string>();
                        int w = 0;
                        int h = 0;
                        int rows = 0;
                        int cols = 0;
                        int counter = 0;
                        while ((line = file.ReadLine()) != null)
                        {
                            switch (counter)
                            {
                                case 0:
                                    w = int.Parse(line);
                                    break;
                                case 1:
                                    h = int.Parse(line);
                                    break;
                                case 2:
                                    rows = int.Parse(line);
                                    break;
                                case 3:
                                    cols = int.Parse(line);
                                    break;
                                default:
                                    fileNames.Add(line);
                                    break;
                            }
                            counter++;
                        }
                        file.Close();
                        imageposition_m = new SortedList<int, ImagePosition>();
                        int x, y = 0;
                        for (int a = 0; a < fileNames.Count; a++)
                        {
                            Application.DoEvents();
                            y = a / cols;
                            x = a - y * cols;
                            x = x * w;
                            y = y * h;
                            imagesx_m.Add(a, fileNames[a]);
                            ImagePosition newpos = new ImagePosition();
                            newpos.name = fileNames[a];
                            newpos.pos = new Point(x, y);
                            imageposition_m.Add(a, newpos);
                        }
                    }
                    else continue;
                }
                streamSurfaces.Dispose();
                GC.Collect();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                loadedPck = false;
            }
            finally
            {
                loadedPck = true;
            }
        }

        public static Bitmap images(string name, int gender)
        {
            if (iconlist_ivtrm != null)
            {
                if (imagesCache_m != null && imagesCache_m.ContainsKey(name))
                {
                    return imagesCache_m[name];
                }
                else
                {
                    int w = 32;
                    int h = 32;
                    Point d = imageposition_m.FirstOrDefault(x => x.Value.name.Equals(name)).Value.pos;
                    Bitmap pageBitmap = new Bitmap(w, h, PixelFormat.Format32bppArgb);
                    using (Graphics graphics = Graphics.FromImage(pageBitmap))
                    {
                        graphics.DrawImage(iconlist_ivtrm, new Rectangle(0, 0, w, h), new Rectangle(d.X, d.Y, w, h), GraphicsUnit.Pixel);
                    }
                    if (imagesCache_m == null || imagesCache_m != null && !imagesCache_m.ContainsKey(name))
                    {
                        imagesCache_m[name] = pageBitmap;
                    }
                    return pageBitmap;
                }
            }
            return Resources.blank1;
        }

        public struct ImagePosition
        {
            public string name;
            public Point pos;

            public ImagePosition(string name, Point pos)
            {
                this.name = name;
                this.pos = pos;
            }
        }

        private void npcgen_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            npcgenofd.DefaultViewMode = DevExpress.Dialogs.Core.View.ViewMode.Details;
            npcgenofd.Filter = "npcgen.data|npcgen*.data|Data Files (*.data)|*.data|All Files|*.*";
            if (npcgenofd.ShowDialog() == DialogResult.OK)
            {
                npcgen.Text = npcgenofd.FileName;
            }
        }

        private void elements_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            elementsofd.DefaultViewMode = DevExpress.Dialogs.Core.View.ViewMode.Details;
            elementsofd.Filter = "elements.data|elements*.data|Data Files (*.data)|*.data|All Files|*.*";
            if (elementsofd.ShowDialog() == DialogResult.OK)
            {
                elements.Text = elementsofd.FileName;
            }
        }

        private void surfaces_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            surfacesofd.DefaultViewMode = DevExpress.Dialogs.Core.View.ViewMode.Details;
            surfacesofd.Filter = "surfaces.pck|surfaces*.pck|Pck Files (*.pck)|*.pck|All Files|*.*";
            if (surfacesofd.ShowDialog() == DialogResult.OK)
            {
                surfaces.Text = surfacesofd.FileName;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            ReadDatas();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Task t = Task.Factory.StartNew(() => ReadItemIcon());
            t.Wait();
            iconlist_ivtrm = File.Exists("icon\\iconlist_ivtrm.png") ? (Bitmap)Image.FromFile("icon\\iconlist_ivtrm.png") : null;
            // Maps
            try
            {
                Maps = new List<GameMapInfo>();
                var streamSurfaces = new PCKStream(surfaces.Text);
                var rftSurfaces = ReadFileTable(streamSurfaces);
                var AllFiles = rftSurfaces.Where(z => z.Path.Contains("minimaps") && !z.Path.Contains("surfaces\\minimaps\\world")).ToList();
                List<string> AlreadyExistion = new List<string>();
                foreach (PCKFileEntry fs in AllFiles)
                {
                    GameMapInfo map = new GameMapInfo();
                    List<string> st = fs.Path.Split('\\').ToList();
                    map.MapName = st.ElementAt(st.Count - 2);
                    st.RemoveAt(st.Count - 1);
                    map.MapPath = string.Join("\\", st);
                    if (AlreadyExistion.FindIndex(v => v == string.Join("\\", st)) == -1)
                    {
                        map.MapFragments = AllFiles.Where(z => z.Path.Contains(map.MapPath)).ToList().OrderBy(z => z.Path).ToList();
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
                    map1.MapFragments = rftSurfaces.Where(z => z.Path.Contains(map1.MapPath)).ToList();
                    Maps.Add(map1);
                    cbMaps.Properties.Items.Clear();
                    for (int i = 0; i < Maps.Count; i++)
                    {
                        cbMaps.Properties.Items.Add(Maps[i].MapName);
                    }
                    cbMaps.SelectedIndex = cbMaps.Properties.Items.Count - 1;
                }
            }
            catch
            {
                cbMaps.Properties.Items.Clear();
                Maps = null;
            }
        }

        void LinkMaps(List<PCKFileEntry> l, string MapName)
        {
            var streamSurfaces = new PCKStream(surfaces.Text);
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
                pb1.BeginInvoke(new MethodInvoker(delegate
                {
                    pb1.Properties.Maximum = l.Count;
                    pb1.Position = 0;
                }));
                l = l.OrderBy(t => t.Path).ToList();
                for (int i = 0; i < l.Count; i++)
                {
                    gr.DrawImage(new DDSImage(ReadFile(streamSurfaces, l[i])).BitmapImage, new Point(x, y));
                    x += val;
                    if (x == bm.Width)
                    {
                        y += val;
                        x = 0;
                    }
                    pb1.BeginInvoke(new MethodInvoker(delegate
                    {
                        pb1.Position++;
                    }));
                }
                this.BeginInvoke(new MethodInvoker(delegate
                {
                    pb1.Position = 0;
                    if (MapForm == null)
                    {
                        MapForm = new MapWindow();
                        Cursor = Cursors.AppStarting;
                        MapForm.pictureBox1.BackgroundImage = bm;
                        MapForm.pictureBox1.Width = MapForm.pictureBox1.BackgroundImage.Width;
                        MapForm.pictureBox1.Height = MapForm.pictureBox1.BackgroundImage.Height;
                        GC.Collect();
                        Cursor = Cursors.Default;
                        MapForm.Show(this);
                    }
                    else if (MapForm.Visible == false)
                    {
                        MapForm = new MapWindow();
                        Cursor = Cursors.AppStarting;
                        MapForm.pictureBox1.BackgroundImage = bm;
                        MapForm.pictureBox1.Width = MapForm.pictureBox1.BackgroundImage.Width;
                        MapForm.pictureBox1.Height = MapForm.pictureBox1.BackgroundImage.Height;
                        GC.Collect();
                        Cursor = Cursors.Default;
                        MapForm.Show(this);
                    }
                    else
                    {
                        Cursor = Cursors.AppStarting;
                        MapForm.pictureBox1.BackgroundImage = bm;
                        MapForm.pictureBox1.Width = MapForm.pictureBox1.BackgroundImage.Width;
                        MapForm.pictureBox1.Height = MapForm.pictureBox1.BackgroundImage.Height;
                        GC.Collect();
                        Cursor = Cursors.Default;
                    }
                }));
            }
            else
            {
                XtraMessageBox.Show("Map options haven't been found in maps.conf", "Npcgen Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void listnpcmob_CustomizeItem(object sender, CustomizeTemplatedItemEventArgs e)
        {
            var item = e.Value as ClassDefaultMonsters;
            e.TemplatedItem["id"].Text = item.MobDops[0].Id.ToString();
            e.TemplatedItem["name"].Text = listMonster.TryGetValue(item.MobDops[0].Id, out string value) ? value : "[?]";
        }

        void UpdateTable()
        {
            listnpcmob.Items.Clear();
            listnpcmob.DataSource = null;
            List<ClassDefaultMonsters> l = new List<ClassDefaultMonsters>();
            if (npcGen != null)
            {
                for (int i = 0; i < npcGen.NpcMobList.Count; i++)
                {
                    var moblist = npcGen.NpcMobList[i];
                    l.Add(moblist);
                }
                listnpcmob.DataSource = l;
            }
        }

        private void groupControl1_CustomButtonClick(object sender, DevExpress.XtraBars.Docking2010.BaseButtonEventArgs e)
        {
            var lista = listnpcmob.DataSource as List<ClassDefaultMonsters>;
            switch (e.Button.Properties.GroupIndex)
            {
                case 0:
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
                    lista.Add(mb);
                    break;
                case 1:
                    if (listnpcmob.SelectedIndices.Count >= 0)
                    {
                        for (int i = 0; i < listnpcmob.SelectedIndices.Count; i++)
                        {
                            int ind = listnpcmob.SelectedIndices[i] - i;
                            lista.RemoveAt(ind);
                        }
                    }
                    break;
            }
        }

        private void buttonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            List<ClassDefaultMonsters> filterlist = new List<ClassDefaultMonsters>();
            List<int> nl = new List<int>();
            if (txtSearchNpcMob.Text.Trim().Length == 0)
            {
                UpdateTable();
            }
            if (cbSearchTypeNpcMob.SelectedIndex == 0 && txtSearchNpcMob.Text.Trim().Length > 0)
            {
                int.TryParse(txtSearchNpcMob.Text, out int IdIndex);
                filterlist = npcGen.NpcMobList.Select((item, index) => new { Item = item, Index = index }).Where(o => o.Item.MobDops.Any(x => x.Id == IdIndex)).Select(o => o.Item).ToList();
            }
            else if (cbSearchTypeNpcMob.SelectedIndex == 1 && txtSearchNpcMob.Text.Trim().Length > 0)
            {
                nl = listMonster.Select(item => new { Item = item }).Where(o => o.Item.Value.Contains(txtSearchNpcMob.Text)).Select(o => o.Item.Key).ToList();
                foreach (var n in nl)
                {
                    filterlist.AddRange(npcGen.NpcMobList.Select((item, index) => new { Item = item, Index = index }).Where(o => o.Item.MobDops.Any(x => x.Id == n)).Select(o => o.Item).ToList());
                }
            }
            else if (cbSearchTypeNpcMob.SelectedIndex == 2 && txtSearchNpcMob.Text.Trim().Length > 0)
            {
                int.TryParse(txtSearchNpcMob.Text, out int IdIndex);
                filterlist = npcGen.NpcMobList.Select((item, index) => new { Item = item, Index = index }).Where(o => o.Item.Trigger_id == IdIndex).Select(o => o.Item).ToList();
            }
            listnpcmob.DataSource = filterlist;
        }

        private void listnpcmob_ParseSearchControlText(object sender, ParseSearchControlTextEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.SearchControlText))
                return;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            if (File.Exists("maps.conf"))
            {
                StreamReader sr = new StreamReader("maps.conf");
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
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (Maps != null)
            {
                int SelectedIndex = cbMaps.SelectedIndex;
                System.Threading.Thread th = new System.Threading.Thread(() => LinkMaps(Maps[SelectedIndex].MapFragments, Maps[SelectedIndex].MapName));
                th.Start();
            }
        }
    }
}