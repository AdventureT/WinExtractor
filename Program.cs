var f = new ExtendedFileReader(File.Open(args[0], FileMode.Open));

var header = new Header(f);

Console.WriteLine($"===============================================\nVersion: {header.Version[0]}.{header.Version[1]}.{BitConverter.ToInt16( new byte[] { header.Version[2], header.Version[3] })}");
Console.WriteLine($"Filename: {header.FileName}");
Console.WriteLine($"Configuration: {header.Configuration}");
Console.WriteLine($"Internal gamename: {header.GameName}");
Console.WriteLine($"File built on: {header.CreationTime}\n===============================================");

f.BaseStream.Seek(0x40, SeekOrigin.Begin); //just in case i fuck something up with the header, it still extracts normally

var Groups = new List<Group>();

for (int i = 0; i < 256; i++)
{
    var group = new Group(f, i);
    Groups.Add(group);
}

var Files = new List<FileEntry>();

foreach (var group in Groups)
{
    
    if (group.Offset != -1)
    {
        //Console.WriteLine($"{group.GroupName}: {group.FileCount}");
        f.BaseStream.Seek(group.Offset, SeekOrigin.Begin);
        for (int i = 0; i < group.FileCount; i++)
        {
            f.BaseStream.Seek(group.Offset + i * 8, SeekOrigin.Begin);
            var file = new FileEntry(f);
            var last = Files.LastOrDefault();
            if (last != null)
            {
                Files[Files.Count - 1].Size = file.FileHeaderOffset - last.FileHeaderOffset;
                //Console.WriteLine($"\t - {file.GroupInfo.FileIndex}: {file.FileHeader.FileName} Size: {Files[Files.Count - 1].Size}");
            }
            Files.Add(file);
            group.FileEntries.Add(file);
        }
    }
    
}

foreach (var group in Groups)
{
    if (group.Offset != -1)
    {
        f.BaseStream.Seek(group.Offset, SeekOrigin.Begin);
        Console.WriteLine($"{group.GroupName}: {group.FileCount}");
        f.BaseStream.Seek(group.Offset, SeekOrigin.Begin);
        for (int i = 0; i < group.FileCount; i++)
        {
            Console.WriteLine($"\t - {group.FileEntries[i].GroupInfo.FileIndex}: {group.FileEntries[i].FileHeader.FileName}");
        }
    }
}
Console.WriteLine("Sum of Files: " + Groups.Sum(x => x.FileCount));

Console.ReadLine();
class Header
{
    public byte[] Version { get; set; } = new byte[4];

    public uint Unknown { get; set; }

    public uint Unknown2 { get; set; }

    public ushort Unknown3 { get; set; }

    public ushort Unknown4 { get; set; }

    public string FileName { get; set; }

    public string GameName { get; set; }

    public string Configuration { get; set; }

    public DateTime CreationTime { get; set; }

    public Header(ExtendedFileReader f)
    {
        Version = f.ReadBytes(4);
        Unknown = f.ReadUInt32();
        Unknown2 = f.ReadUInt32();
        Unknown3 = f.ReadUInt16();
        Unknown4 = f.ReadUInt16();
        FileName = f.ReadString();
        f.BaseStream.Seek(12 - FileName.Length - 1, SeekOrigin.Current);
        GameName = f.ReadString();
        f.BaseStream.Seek(16 - GameName.Length - 1, SeekOrigin.Current);
        CreationTime = new System.DateTime(1970, 1, 1).AddSeconds(f.ReadInt32());
        f.BaseStream.Seek(8, SeekOrigin.Current);
        Configuration = f.ReadString();
        f.BaseStream.Seek(8 - GameName.Length - 3, SeekOrigin.Current);
        
    }
}

class Group
{
    public GroupName GroupName { get; set; }

    public uint FileCount { get; set; }

    public List<FileEntry> FileEntries { get; set; } = new();

    public int Offset { get; set; }

    public Group(ExtendedFileReader f, int index)
    {
        GroupName = (GroupName)index;
        FileCount = f.ReadUInt32();
        Offset = f.ReadInt32();
    }
}

class FileEntry
{
    public GroupInfo GroupInfo { get; set; }

    public uint FileHeaderOffset { get; set; }

    public uint Size { get; set; }

    public FileHeader FileHeader { get; set; }

    public byte[] RawFile { get; set; }

    public FileEntry(ExtendedFileReader f)
    {
        var t = f.BaseStream.Position;
        GroupInfo = new GroupInfo(f);
        FileHeaderOffset = f.ReadUInt32();
        f.BaseStream.Seek(FileHeaderOffset, SeekOrigin.Begin);
        FileHeader = new FileHeader(f);
    }
}

class GroupInfo
{
    public byte FileIndex { get; set; }

    public ushort Unknown { get; set; }

    public GroupName GroupName { get; set; }

    public GroupInfo(ExtendedFileReader f)
    {
        FileIndex = f.ReadByte();
        Unknown = f.ReadUInt16();
        GroupName = (GroupName)f.ReadByte();
    }
}

class FileHeader
{
    public GroupInfo GroupInfo { get; set; }

    public string FileName { get; set; }

    public FileHeader(ExtendedFileReader f)
    {
        f.BaseStream.Seek(12, SeekOrigin.Current);
        GroupInfo = new GroupInfo(f);
        FileName = f.ReadString();
        f.BaseStream.Seek(48 - FileName.Length - 1, SeekOrigin.Current);
    }
}

enum GroupName
{
    Unk1,
    Unk2,
    Unk3,
    Unk4,
    Unk5,
    Unk6,
    Unk7,
    Unk8,
    Unk9,
    Unk10,
    Unk11,
    Unk12,
    Unk13,
    Unk14,
    Unk15,
    Unk16,
    Unk17,
    Unk18,
    Unk19,
    Unk20,
    Unk21,
    Unk22,
    Unk23,
    FX,
    Unk25,
    Unk26,
    Unk27,
    Unk28,
    Unk29,
    Unk30,
    Unk31,
    Unk32,
    Unk33,
    Unk34,
    Textures,
    Unk36,
    Unk37,
    Unk38,
    Unk39,
    Unk40,
    Proxy,
    Unk42,
    Unk43,
    Unk44,
    Unk45,
    Unk46,
    Unk47,
    Unk48,
    Unk49,
    Unk50,
    Unk51,
    Unk52,
    Unk53,
    Unk54,
    Unk55,
    Unk56,
    Unk57,
    Unk58,
    Unk59,
    Unk60,
    Unk61,
    Unk62,
    Unk63,
    Unk64,
    Unk65,
    Unk66,
    Unk67,
    Unk68,
    Unk69,
    Unk70,
    Unk71,
    Unk72,
    Unk73,
    Unk74,
    Unk75,
    Unk76,
    Unk77,
    Unk78,
    Unk79,
    Unk80,
    Unk81,
    Unk82,
    Unk83,
    Unk84,
    Unk85,
    Unk86,
    Unk87,
    Unk88,
    Unk89,
    Unk90,
    Unk91,
    Models,
    Unk93,
    Unk94,
    Unk95,
    Unk96,
    Unk97,
    Unk98,
    Unk99,
    Unk100,
    Unk101,
    Unk102,
    Unk103,
    Unk104,
    Unk105,
    Unk106,
    Unk107,
    Unk108,
    Unk109,
    Unk110,
    Unk111,
    Unk112,
    Unk113,
    Unk114,
    Unk115,
    Unk116,
    Unk117,
    Unk118,
    Unk119,
    Unk120,
    Unk121,
    Unk122,
    Unk123,
    Unk124,
    Unk125,
    Unk126,
    Unk127,
    Unk128,
    Unk129,
    Unk130,
    Unk131,
    Unk132,
    Unk133,
    Unk134,
    Unk135,
    Unk136,
    Unk137,
    Unk138,
    Unk139,
    Unk140,
    Unk141,
    Unk142,
    Unk143,
    Unk144,
    Unk145,
    Unk146,
    Unk147,
    Unk148,
    Unk149,
    Unk150,
    Unk151,
    Unk152,
    Unk153,
    Unk154,
    Unk155,
    Unk156,
    Unk157,
    Unk158,
    Unk159,
    Unk160,
    Unk161,
    Unk162,
    Unk163,
    Unk164,
    Unk165,
    Unk166,
    Unk167,
    Unk168,
    Unk169,
    Unk170,
    Unk171,
    Unk172,
    Unk173,
    Unk174,
    Unk175,
    Unk176,
    Unk177,
    Unk178,
    Unk179,
    Unk180,
    Unk181,
    Unk182,
    Unk183,
    Unk184,
    Unk185,
    Unk186,
    Unk187,
    Unk188,
    Unk189,
    Unk190,
    Unk191,
    Unk192,
    Unk193,
    Unk194,
    Unk195,
    Unk196,
    Unk197,
    Unk198,
    Unk199,
    Unk200,
    Unk201,
    Unk202,
    Unk203,
    Unk204,
    Unk205,
    Unk206,
    Unk207,
    Unk208,
    Unk209,
    Unk210,
    Unk211,
    Unk212,
    Unk213,
    Unk214,
    Unk215,
    Unk216,
    Unk217,
    Unk218,
    Unk219,
    Unk220,
    Unk221,
    Unk222,
    Unk223,
    Unk224,
    Unk225,
    Unk226,
    Unk227,
    Unk228,
    Unk229,
    Unk230,
    Unk231,
    Unk232,
    Unk233,
    Unk234,
    Unk235,
    Unk236,
    Unk237,
    Unk238,
    Unk239,
    Unk240,
    Unk241,
    Unk242,
    Unk243,
    Unk244,
    Unk245,
    Unk246,
    Unk247,
    Unk248,
    Unk249,
    Unk250,
    Unk251,
    Unk252,
    Unk253,
    Unk254,
    Unk255,
    Unk256
}