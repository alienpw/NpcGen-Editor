using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpcGenDataEditorByLuka
{
    class Elementsdata
    {
        List<int> ElementsDataVersion = new List<int>(new int[] { 6, 7, 10, 12, 17, 27, 29, 60,61,62,63, 66, 67, 68, 69, 70, 80, 84, 85, 88, 99, 100, 101, 102, 104, 105, 106, 108, 112, 145 });
        public short Version;
        ushort Signature;
        int Value;
        int TimeStamp;
        public int MonsterdAmount;
        public int NpcsAmount;
        public List<NpcMonster> ExistenceLists = new List<NpcMonster>();
        public List<NpcMonster> ResourcesList = new List<NpcMonster>();
        public Elementsdata(BinaryReader br)
        {
            Version = br.ReadInt16();
            Signature = br.ReadUInt16();
            if (ElementsDataVersion.Contains(Version))
            {
                if (Version >= 10)
                {
                    TimeStamp = br.ReadInt32();
                }
                #region 0
                Value = br.ReadInt32();
                br.ReadBytes(Value * 84);
                #endregion
                #region 1
                Value = br.ReadInt32();
                br.ReadBytes(Value * 68);
                #endregion
                #region 2
                Value = br.ReadInt32();
                br.ReadBytes(Value * 356);
                #endregion
                #region 3
                Value = br.ReadInt32();
                if (Version < 27)
                    br.ReadBytes(Value * 1404);
                else if (Version == 27 || Version == 29)
                    br.ReadBytes(Value * 1412);
                else if (Version == 145)
                    br.ReadBytes(Value * 1424);
                else
                    br.ReadBytes(Value * 1420);
                #endregion
                #region 4
                Value = br.ReadInt32();
                br.ReadBytes(Value * 68);
                #endregion
                #region 5
                Value = br.ReadInt32();
                br.ReadBytes(Value * 72);
                #endregion
                #region 6
                Value = br.ReadInt32();
                if (Version < 27)
                    br.ReadBytes(Value * 1104);
                else if (Version == 27 || Version == 29)
                    br.ReadBytes(Value * 1120);
                else if (Version == 145)
                    br.ReadBytes(Value * 1132);
                else
                    br.ReadBytes(Value * 1128);
                #endregion
                #region 7
                Value = br.ReadInt32();
                br.ReadBytes(Value * 68);
                #endregion
                #region 8
                Value = br.ReadInt32();
                br.ReadBytes(Value * 72);
                #endregion
                #region 9
                Value = br.ReadInt32();
                if (Version < 27)
                    br.ReadBytes(Value * 1156);
                else if (Version == 29 || Version == 27)
                    br.ReadBytes(Value * 1164);
                else if (Version == 145)
                    br.ReadBytes(Value * 1172);
                else
                    br.ReadBytes(Value * 1168);
                #endregion
                #region 10
                Value = br.ReadInt32();
                br.ReadBytes(Value * 68);
                #endregion
                #region 11
                Value = br.ReadInt32();
                br.ReadBytes(Value * 68);
                #endregion
                #region 12
                Value = br.ReadInt32();
                br.ReadBytes(Value * 376);
                #endregion
                #region 13
                Value = br.ReadInt32();
                br.ReadBytes(Value * 68);
                #endregion
                #region 14
                Value = br.ReadInt32();
                br.ReadBytes(Value * 68);
                #endregion
                #region 15
                Value = br.ReadInt32();
                br.ReadBytes(Value * 368);
                #endregion
                #region 16
                Value = br.ReadInt32();
                br.ReadBytes(Value * 68);
                #endregion
                #region 17
                Value = br.ReadInt32();
                br.ReadBytes(Value * 364);
                #endregion
                #region 18
                Value = br.ReadInt32();
                br.ReadBytes(Value * 68);
                #endregion
                #region 19
                Value = br.ReadInt32();
                br.ReadBytes(Value * 624);
                #endregion
                #region 19-20 bytes
                if (Version >= 10)
                {
                    #region Liss19-20Bytes
                    byte[] head = br.ReadBytes(4);
                    byte[] count = br.ReadBytes(4);
                    byte[] body = br.ReadBytes(BitConverter.ToInt32(count, 0));
                    byte[] tail = br.ReadBytes(4);
                    #endregion
                }
                #endregion
                #region 20
                Value = br.ReadInt32();
                br.ReadBytes(Value * 68);
                #endregion
                #region 21
                Value = br.ReadInt32();
                br.ReadBytes(Value * 348);
                #endregion
                #region 22
                Value = br.ReadInt32();
                if (Version < 27)
                    br.ReadBytes(Value * 516);
                else if (Version >= 27 && Version <= 101)
                    br.ReadBytes(Value * 524);
                else if (Version > 101 && Version < 145)
                    br.ReadBytes(Value * 648);
                else if (Version == 145)
                    br.ReadBytes(Value * 776);
                #endregion
                #region 23
                Value = br.ReadInt32();
                br.ReadBytes(Value * 488);
                #endregion
                #region 24
                Value = br.ReadInt32();
                br.ReadBytes(Value * 348);
                #endregion
                #region 25
                Value = br.ReadInt32();
                br.ReadBytes(Value * 348);
                #endregion
                #region 26
                Value = br.ReadInt32();
                br.ReadBytes(Value * 352);
                #endregion
                #region 27
                Value = br.ReadInt32();
                br.ReadBytes(Value * 348);
                #endregion
                #region 28
                Value = br.ReadInt32();
                br.ReadBytes(Value * 208);
                #endregion
                #region 29
                Value = br.ReadInt32();
                br.ReadBytes(Value * 888);
                #endregion
                #region 30
                Value = br.ReadInt32();
                br.ReadBytes(Value * 68);
                #endregion
                #region 31
                Value = br.ReadInt32();
                br.ReadBytes(Value * 892);
                #endregion
                #region 32
                Value = br.ReadInt32();
                br.ReadBytes(68 * Value);
                #endregion
                #region 33
                Value = br.ReadInt32();
                br.ReadBytes(Value * 340);
                #endregion
                #region 34
                Value = br.ReadInt32();
                br.ReadBytes(Value * 68);
                #endregion
                #region 35
                Value = br.ReadInt32();
                if (Version == 145)
                    br.ReadBytes(Value * 476);
                else
                    br.ReadBytes(Value * 436);
                #endregion
                #region 36
                Value = br.ReadInt32();
                br.ReadBytes(Value * 84);
                #endregion
                #region 37
                Value = br.ReadInt32();
                br.ReadBytes(Value * 196);
                #endregion
                #region 38 MonstersListReading
                Value = br.ReadInt32();
                MonsterdAmount = Value;
                for (int i = 0; i < Value; i++)
                {
                    NpcMonster np = new NpcMonster()
                    {
                        Id = br.ReadInt32()
                    };
                    br.ReadInt32();
                    np.Name = Encoding.Unicode.GetString(br.ReadBytes(64)).TrimEnd('\0');
                    #region BytesByVersion
                    if (Version <= 12)
                        br.ReadBytes(1428);
                    else if (Version == 17)
                        br.ReadBytes(1444);
                    else if (Version > 17 && Version <= 29)
                        br.ReadBytes(1456);
                    else if (Version >= 60 && Version < 66)
                        br.ReadBytes(1480);
                    else if (Version == 70 || Version == 66)
                        br.ReadBytes(1484);
                    else if (Version >= 80 &&Version<=88)
                        br.ReadBytes(1488);
                    else if (Version > 88 && Version <= 106)
                        br.ReadBytes(1544);
                    else if (Version == 108)
                        br.ReadBytes(1576);
                    else
                        br.ReadBytes(1592);
                    #endregion
                    ExistenceLists.Add(np);
                }
                #endregion
                #region 39
                Value = br.ReadInt32();
                br.ReadBytes(Value * 72);
                #endregion
                #region 40
                Value = br.ReadInt32();
                if (Version <= 29)
                    br.ReadBytes(Value * 1224);
                else if (Version == 145)
                    br.ReadBytes(Value * 4392);
                else if (Version == 60)
                    br.ReadBytes(Value * 2280);
                else
                    br.ReadBytes(Value * 3368);
                #endregion
                #region 41
                Value = br.ReadInt32();
                br.ReadBytes(Value * 72);
                #endregion
                #region 42
                Value = br.ReadInt32();
                br.ReadBytes(Value * 72);
                #endregion
                #region 43
                Value = br.ReadInt32();
                br.ReadBytes(Value * 200);
                #endregion
                #region 44
                Value = br.ReadInt32();
                br.ReadBytes(Value * 200);
                #endregion
                #region 45
                Value = br.ReadInt32();
                if (Version < 27)
                    br.ReadBytes(Value * 196);
                else
                {
                    br.ReadBytes(Value * 1092);
                }
                #endregion
                #region 46
                Value = br.ReadInt32();
                if (Version < 27)
                    br.ReadBytes(Value * 196);
                else if (Version == 29 || Version == 27)
                    br.ReadBytes(Value * 1092);
                else if (Version == 60)
                    br.ReadBytes(Value * 1116);
                else if (Version == 145)
                    br.ReadBytes(Value * 1124);
                else
                    br.ReadBytes(Value * 1120);
                #endregion
                #region 47
                Value = br.ReadInt32();
                br.ReadBytes(Value * 644);
                Value = br.ReadInt32();
                if (Version == 145)
                    br.ReadBytes(Value * 1096);
                else
                    br.ReadBytes(Value * 584);
                #endregion
                #region 48
                Value = br.ReadInt32();
                br.ReadBytes(Value * 72);
                #endregion
                #region 49
                Value = br.ReadInt32();
                br.ReadBytes(Value * 460);
                #endregion
                #region 50
                Value = br.ReadInt32();
                br.ReadBytes(Value * 328);
                #endregion
                #region 51
                Value = br.ReadInt32();
                br.ReadBytes(Value * 72);
                #endregion
                #region 52
                Value = br.ReadInt32();
                br.ReadBytes(Value * 68);
                #endregion
                #region 53
                Value = br.ReadInt32();
                if (Version < 27)
                    br.ReadBytes(Value * 1224);
                else
                    br.ReadBytes(Value * 1228);
                #endregion
                #region 54
                Value = br.ReadInt32();
                br.ReadBytes(Value * 72);
                #endregion
                #region 55
                Value = br.ReadInt32();
                br.ReadBytes(Value * 68);
                #endregion
                #region 56 NPCSListReading
                Value = br.ReadInt32();
                NpcsAmount = Value;
                for (int v = 0; v < Value; v++)
                {
                    NpcMonster np = new NpcMonster()
                    {
                        Id = br.ReadInt32(),
                        Name = Encoding.Unicode.GetString(br.ReadBytes(64)).TrimEnd('\0')
                    };
                    if (Version < 27)
                        br.ReadBytes(780);
                    else if (Version >= 27&&Version<=29)
                        br.ReadBytes(784);
                    else if (Version == 60)
                        br.ReadBytes(800);
                    else if (Version == 145)
                        br.ReadBytes(812);
                    else
                        br.ReadBytes(804);
                    ExistenceLists.Add(np);
                }
                #endregion
                #region DialogsReading
                byte[] pattern = (Encoding.GetEncoding("GBK")).GetBytes("facedata\\");
                long sourcePosition = br.BaseStream.Position;
                int listLength = -72 - pattern.Length;
                bool run = true;
                while (run)
                {
                    run = false;
                    for (int i = 0; i < pattern.Length; i++)
                    {
                        listLength++;
                        if (br.ReadByte() != pattern[i])
                        {
                            run = true;
                            break;
                        }
                    }
                }
                br.BaseStream.Position = sourcePosition;
                br.ReadBytes(listLength);
                #endregion
                #region 58
                Value = br.ReadInt32();
                if (Version <= 27)
                    br.ReadBytes(Value * 476);
                else
                    br.ReadBytes(Value * 480);
                #endregion
                #region 59
                Value = br.ReadInt32();
                br.ReadBytes(Value * 348);
                #endregion
                #region 60
                Value = br.ReadInt32();
                br.ReadBytes(Value * 196);
                #endregion
                #region 61
                Value = br.ReadInt32();
                br.ReadBytes(Value * 336);
                #endregion
                #region 62
                Value = br.ReadInt32();
                if (Version <= 27)
                    br.ReadBytes(Value * 468);
                else
                    br.ReadBytes(Value * 472);
                #endregion
                #region 63
                Value = br.ReadInt32();
                br.ReadBytes(Value * 340);
                #endregion
                #region 64
                Value = br.ReadInt32();
                br.ReadBytes(Value * 208);
                #endregion
                #region 65
                Value = br.ReadInt32();
                if (Version <= 88)
                    br.ReadBytes(Value * 204);
                else
                    br.ReadBytes(Value * 332);
                #endregion
                #region 66
                Value = br.ReadInt32();
                br.ReadBytes(Value * 68);
                #endregion
                #region 67
                Value = br.ReadInt32();
                br.ReadBytes(Value * 68);
                #endregion
                #region 68
                Value = br.ReadInt32();
                if (Version <= 10)
                    br.ReadBytes(Value * 400);
                else if (Version < 27)
                    br.ReadBytes(Value * 404);
                else if (Version >= 27 && Version <= 85)
                    br.ReadBytes(Value * 416);
                else if (Version >=88 && Version <145)
                    br.ReadBytes(Value * 420);
                else if (Version == 145)
                    br.ReadBytes(Value * 428);
                #endregion
                #region 69
                Value = br.ReadInt32();
                br.ReadBytes(Value * 196);
                #endregion
                #region 70
                Value = br.ReadInt32();
                if (Version <= 88)
                    br.ReadBytes(Value * 160);
                else
                    br.ReadBytes(Value * 208);
                #endregion
                #region 71
                Value = br.ReadInt32();
                if (Version <= 29)
                    br.ReadBytes(Value * 612);
                else if (Version == 60 || Version == 61)
                    br.ReadBytes(Value * 628);
                else if (Version > 61 && Version <= 88)
                {
                    br.ReadBytes(Value * 636);
                }
                else
                    br.ReadBytes(Value * 676);
                #endregion
                #region 72
                Value = br.ReadInt32();
                if (Version < 27)
                    br.ReadBytes(Value * 488);
                else if (Version == 145)
                    br.ReadBytes(Value * 616);
                else
                    br.ReadBytes(Value * 552);
                #endregion
                #region 73
                Value = br.ReadInt32();
                if (Version <= 60)
                    br.ReadBytes(Value * 404);
                else if (Version > 60 && Version <= 66)
                    br.ReadBytes(Value * 500);
                else
                    br.ReadBytes(Value * 504);
                #endregion
                #region 74
                Value = br.ReadInt32();
                br.ReadBytes(Value * 344);
                #endregion
                #region 75
                Value = br.ReadInt32();
                br.ReadBytes(Value * 340);
                #endregion
                #region 76
                Value = br.ReadInt32();
                br.ReadBytes(Value * 668);
                #endregion
                #region 77
                Value = br.ReadInt32();
                br.ReadBytes(Value * 68);
                #endregion
                #region 78
                Value = br.ReadInt32();
                for (int i = 0; i < Value; i++)
                {
                    NpcMonster rs = new NpcMonster()
                    {
                        Id = br.ReadInt32()
                    };
                    br.ReadInt32();
                    rs.Name = Encoding.Unicode.GetString(br.ReadBytes(64)).TrimEnd('\0');
                    ResourcesList.Add(rs);
                    if (Version <= 60)
                        br.ReadBytes(380);
                    else if (Version > 60 && Version <= 88)
                        br.ReadBytes(396);
                    else if (Version == 145)
                        br.ReadBytes(488);
                    else
                        br.ReadBytes(480);
                }
                #endregion
            }
            else
            {
                Version = -10;
            }
            br.Close();
        }
    }
    public class NpcMonster
    {
        public int Id;
        public string Name;
    }
}
