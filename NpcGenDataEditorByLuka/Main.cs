using DevExpress.XtraEditors;
using NpcGenDataEditorByLuka.Properties;
using pwAdminLocal.DDSReader;
using pwAdminLocal.Elements;
using pwAdminLocal.PCKEngine;
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
using static NpcGenDataEditorByLuka.Main;

namespace NpcGenDataEditorByLuka
{
    public partial class Main : DevExpress.XtraEditors.XtraForm
    {
        NpcGen npcGen = new NpcGen();
        public static eListCollection eLC;
        public static SortedList<int, string> imagesx_m;
        public static SortedList<int, ImagePosition> imageposition_m;
        public static SortedDictionary<string, Bitmap> imagesCache_m = new SortedDictionary<string, Bitmap>();
        public static Bitmap iconlist_ivtrm { get; set; }
        public static bool loadedData;
        public static bool loadedPck;

        public Main()
        {
            InitializeComponent();
        }

        void ReadDatas()
        {
            BinaryReader b = new BinaryReader(npcgenofd.OpenFile());
            Task t = Task.Factory.StartNew(() => npcGen.ReadNpcgen(b));
            t.Wait();
            t = Task.Factory.StartNew(() => ReadElements());
            t.Wait();            
        }

        public void ReadElements()
        {
            try
            {
                eLC = new eListCollection(this, elementsofd.FileName, ref progressBarControl1);
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
            catch
            {
                Console.WriteLine("Erro ao ler elements.data");
                loadedData = false;
            }
            finally
            {
                loadedData = true;
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
                var streamSurfaces = new PCKStream(surfacesofd.FileName);
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
            if (npcgenofd.ShowDialog() == DialogResult.OK)
            {
                npcgen.Text = npcgenofd.FileName;
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
            iconlist_ivtrm = (Bitmap)Image.FromFile("icon\\iconlist_ivtrm.png");
        }
    }
}