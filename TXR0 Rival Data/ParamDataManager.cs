using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Memory;

namespace TXR0_Rival_Data
{
    public class ParamDataManager
    {
        public enum VarType
        {
            Data,
            DataLength,
            Index,
            Index1,
            Button
        };
        #region Structures
        public struct FileStructure
        {
            public FileStructure(VarType _varType, System.Type _dataType, String _name = null, Int32 _length = 0, DataGridViewColumn _columnType = null) { varType = _varType; dataType = _dataType; name = _name; length = _length; columnType = _columnType; }
            public VarType varType;
            public System.Type dataType;
            public String name;
            public Int32 length;
            public DataGridViewColumn columnType;
        };
        public struct ELFType
        {
            public ELFType(UInt32 _hash, UInt32 _address, String _type) { hash = _hash; address = _address; type = _type; }
            public UInt32 hash;
            public UInt32 address;
            public String type;
        }
        #endregion Structures
        #region Variables
        ParamData paramData = new ParamData("Rival Data");
        public DataSet dsParamData = null;
        Byte[] ELFData = null;
        public readonly List<ELFType> elfTypes = new List<ELFType>()
        {
            new ELFType(0x562BCFD0, 0x165890, "US"),
            new ELFType(0x765A0E92, 0x165E90, "EU"),
            new ELFType(0xC3859771, 0x16C390, "JP")
        };
        public String elfType = "Unknown";
        public Mem PCSX2 = new Mem();
        #endregion Variables
        #region File Structures
        public readonly List<FileStructure> fsRivalData = new List<FileStructure>() {
            new FileStructure(VarType.Index1, typeof(Int32), "Rival Number"),
            new FileStructure(VarType.Data, typeof(Int16), "Team Member ID"),
            new FileStructure(VarType.Data, typeof(Int16), "Appear Flag?"),
            new FileStructure(VarType.Data, typeof(Int16), "Car ID"),
            new FileStructure(VarType.Data, typeof(Int16), "Team ID"),
            new FileStructure(VarType.Data, typeof(Int16), "Battle Requirement"),
            new FileStructure(VarType.Data, typeof(Int16), "Unknown 1"),
            new FileStructure(VarType.Data, typeof(Int16), "Flag"),
            new FileStructure(VarType.Data, typeof(Int16), "Unknown 2"),
            new FileStructure(VarType.Data, typeof(Int32), "Increment"),
            new FileStructure(VarType.Data, typeof(String), "Rival Name 1", 0x11),
            new FileStructure(VarType.Data, typeof(String), "Rival Name 2", 0x11),
            new FileStructure(VarType.Data, typeof(Int16), "Rival Type"),
            new FileStructure(VarType.Data, typeof(Int16), "Area?"),
            new FileStructure(VarType.Data, typeof(Byte), "Flag ", 10),
            new FileStructure(VarType.Data, typeof(Int16), "Sticker"),
            new FileStructure(VarType.Data, typeof(Byte), "Fender"),
            new FileStructure(VarType.Data, typeof(Byte), "Front Bumper"),
            new FileStructure(VarType.Data, typeof(Byte), "Bonnet"),
            new FileStructure(VarType.Data, typeof(Byte), "Mirrors"),
            new FileStructure(VarType.Data, typeof(Byte), "Side Skirts"),
            new FileStructure(VarType.Data, typeof(Byte), "Rear Bumper"),
            new FileStructure(VarType.Data, typeof(Byte), "Wing"),
            new FileStructure(VarType.Data, typeof(Byte), "Grill"),
            new FileStructure(VarType.Data, typeof(Byte), "Lights"),
            new FileStructure(VarType.Data, typeof(Byte), "Wheels?"),
            new FileStructure(VarType.Data, typeof(Byte), "Wheel Colour 1"),
            new FileStructure(VarType.Data, typeof(Byte), "Wheel Colour 2"),
            new FileStructure(VarType.Data, typeof(Byte), "Unknown 3_", 2),
            new FileStructure(VarType.Data, typeof(Byte), "Engine"),
            new FileStructure(VarType.Data, typeof(Byte), "Exhaust"),
            new FileStructure(VarType.Data, typeof(Byte), "Transmission"),
            new FileStructure(VarType.Data, typeof(Byte), "Differential"),
            new FileStructure(VarType.Data, typeof(Byte), "Tyres"),
            new FileStructure(VarType.Data, typeof(Byte), "Brakes"),
            new FileStructure(VarType.Data, typeof(Byte), "Suspension"),
            new FileStructure(VarType.Data, typeof(Byte), "Body"),
            new FileStructure(VarType.Data, typeof(SByte), "Handling"),
            new FileStructure(VarType.Data, typeof(SByte), "Acceleration"),
            new FileStructure(VarType.Data, typeof(SByte), "Braking"),
            new FileStructure(VarType.Data, typeof(SByte), "Brake Balance"),
            new FileStructure(VarType.Data, typeof(SByte), "Spring Rate Front"),
            new FileStructure(VarType.Data, typeof(SByte), "Spring Rate Rear"),
            new FileStructure(VarType.Data, typeof(SByte), "Damper Rate Front"),
            new FileStructure(VarType.Data, typeof(SByte), "Damper Rate Rear"),
            new FileStructure(VarType.Data, typeof(SByte), "Gear Ratio ", 6),
            new FileStructure(VarType.Data, typeof(SByte), "Final Drive"),
            new FileStructure(VarType.Data, typeof(SByte), "Suspension Height Front"),
            new FileStructure(VarType.Data, typeof(SByte), "Suspension Height Rear"),
            new FileStructure(VarType.Data, typeof(Byte), "Unknown 5"),
            new FileStructure(VarType.Data, typeof(Byte), "Wheels"),
            new FileStructure(VarType.Data, typeof(Byte), "Unknown 6"),
            new FileStructure(VarType.Button, typeof(String), "Colour 1"),
            new FileStructure(VarType.Data, typeof(Single), "R1"),
            new FileStructure(VarType.Data, typeof(Single), "G1"),
            new FileStructure(VarType.Data, typeof(Single), "B1"),
            new FileStructure(VarType.Button, typeof(String), "Colour 2"),
            new FileStructure(VarType.Data, typeof(Single), "R2"),
            new FileStructure(VarType.Data, typeof(Single), "G2"),
            new FileStructure(VarType.Data, typeof(Single), "B2"),
            new FileStructure(VarType.Data, typeof(Byte), "Unknown 7_", 12)
        };                                                         
        #endregion File Structures
        #region Data Loading
        public DataSet OpenFile(String TableName, String DataFileName, List<FileStructure> FileStructure)
        {
            {
                Byte[] data = null;

                if (DataFileName != null)
                {
                    using (FileStream stream = new FileStream(DataFileName, FileMode.Open, FileAccess.Read))
                    {
                        BinaryReader reader = new BinaryReader(stream);
                        data = new byte[stream.Length];
                        stream.Read(data, 0, Convert.ToInt32(stream.Length));
                    }
                }
                if ((dsParamData = LoadTableData(data, TableName, FileStructure)) == null)
                    return null;
                return dsParamData;
            }
        }
        public DataSet OpenELFFile(String TableName, String DataFileName, List<FileStructure> FileStructure)
        {
            //try
            {
                Byte[] data = null;

                if (DataFileName != null)
                {
                    using (FileStream stream = new FileStream(DataFileName, FileMode.Open, FileAccess.Read))
                    {
                        BinaryReader reader = new BinaryReader(stream);
                        UInt32 hash = GetHashFromELF(reader, 64);
                        elfType = GetELFType(hash);
                        UInt32 dataOffset = GetELFDataOffset(hash);
                        if (dataOffset != 0)
                        {
                            reader.BaseStream.Position = dataOffset; //1464464; // Offset in SLUS_201.89
                            data = reader.ReadBytes(59200);
                            if (data.Length != 59200)
                                return null;
                        }
                        else
                            return null;
                    }
                }
                // Clear potential already loaded data
                dsParamData = null;
                if ((dsParamData = LoadTableData(data, TableName, FileStructure)) == null)
                    return null;
                return dsParamData;
            }
            //catch(Exception ex)
            //{
            //    MessageBox.Show(null, ex.Message, "Error loading data files", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return null;
            //}
        }

        public bool LoadELFData(String ELFFileName)
        {
            if (ELFFileName != null)
            {
                using (FileStream stream = new FileStream(ELFFileName, FileMode.Open, FileAccess.Read))
                {
                    BinaryReader reader = new BinaryReader(stream);
                    ELFData = reader.ReadBytes(Convert.ToInt32(stream.Length));
                    if (ELFData.Length == Convert.ToInt32(stream.Length))
                        return true;
                    else
                        ELFData = null;
                }
            }
            return false;
        }
        private DataSet LoadTableData(Byte[] data, String TableName, List<FileStructure> structures)
        {
            {
                Int32 dataOffset = 0;
                Int32 dataCount = 0;
                object length = 0;
                if(data != null)
                {
                    if (data.Length % 0x94 != 0) // Check if Rival Data
                        return null;
                    dataCount = data.Length / 0x94;
                    dsParamData = paramData.CreateDataStructure(TableName, structures);
                    DataTable dtTable = dsParamData.Tables[TableName];
                    for (Int32 i = 0; i < dataCount; i++)
                    {
                        DataRow newRow = dtTable.NewRow();
                        length = 0;
                        foreach (FileStructure structure in structures)
                        {
                            if (structure.varType == VarType.DataLength)
                            {
                                length = GetValue(data, dataOffset, structure.dataType);
                                dataOffset += GetTypeLength(structure.dataType);
                            }
                            else if (structure.varType == VarType.Button)
                            {
                                newRow[structure.name] = "btn:" + structure.name;
                            }
                            else if (structure.varType == VarType.Index || structure.varType == VarType.Index1)
                            {
                                newRow[structure.name] = (structure.varType == VarType.Index) ? i : i + 1;
                            }
                            else if (structure.dataType == typeof(System.String) && ((Int32)length > 0 || structure.length > 0))
                            {
                                if (structure.length > 0)
                                    length = structure.length;
                                newRow[structure.name] = GetValue(data, dataOffset, structure.dataType, (Int32)length);
                                dataOffset += (Int32)length;
                            }
                            else if (structure.dataType != typeof(System.String))
                            {
                                if (structure.length != 0)
                                {
                                    for (Int32 j = 0; j < structure.length; j++)
                                    {
                                        String name = structure.name + Convert.ToString(j + 1);
                                        newRow[name] = GetValue(data, dataOffset, structure.dataType);
                                        dataOffset += GetTypeLength(structure.dataType);
                                    }
                                }
                                else
                                {
                                    newRow[structure.name] = GetValue(data, dataOffset, structure.dataType);
                                    dataOffset += GetTypeLength(structure.dataType);
                                }
                            }
                        }
                        dtTable.Rows.Add(newRow);
                    }
                    return dsParamData;
                }
                else
                    return null;
            }
        }
        private Int32 FindPCSX2Offset(String byteString)
        {
            if (PCSX2.mProc.Process != null)
            {
                Int32 offset;
                var find = PCSX2.AoBScan(byteString, true, true);
                find.Wait();
                var res = find.Result;
                offset = Convert.ToInt32(res.SingleOrDefault());
                return offset;
            }
            else
                return 0;
        }
        private Int32 FindPCSX2Offset(Byte[] bytes)
        {
            if (PCSX2.mProc.Process != null)
            {
                String byteString = "";
                Int32 offset;
                foreach (Byte thisByte in bytes)
                {
                    byteString += thisByte.ToString("X02") + " ";
                }
                byteString = byteString.Substring(0, byteString.Length - 1);
                var find = PCSX2.AoBScan(byteString, true, true);
                find.Wait();
                var res = find.Result;
                offset = Convert.ToInt32(res.SingleOrDefault());
                return offset;
            }
            else
                return 0;
        }
        public DataSet PullFromPCSX2(String TableName, List<FileStructure> FileStructure)
        {
            if (PCSX2.mProc.Process != null)
            {
                Byte[] data = null;
                Int32 LoadedGameDataPtr = FindPCSX2Offset(
                    "20 ?? 36 00 E0 ?? 27 00 00 00 00 00 30 ?? 36 00 " +
                    "40 ?? 26 00 01 00 00 00 40 ?? 36 00 80 ?? 26 00 " +
                    "01 00 00 00 60 ?? 36 00 ?? ?? 2D 00 00 00 00 00 " +
                    "80 ?? 36 00 ?? ?? 2D 00 00 00 00 00 A0 ?? 36 00 " +
                    "A0 ?? 26 00 00 00 00 00 C0 ?? 36 00 ?? ?? 2D 00 " +
                    "00 00 00 00 D0 ?? 36 00 B0 ?? 26 00 00 00 00 00 " +
                    "E0 ?? 36 00 F0 ?? 26 00 00 00 00 00 F8 ?? 36 00 " +
                    "D0 ?? 26 00 02 00 00 00 00 00 00 00 00 00 00 00"
                    );
                if (LoadedGameDataPtr != 0)
                {
                    LoadedGameDataPtr += 0x80; // Offset which adjusts to rival data
                    String dataOffset = LoadedGameDataPtr.ToString("X");
                    if (dataOffset != "")
                    {
                        data = PCSX2.ReadBytes(dataOffset, 59200);
                        if (data.Length != 59200)
                            return null;
                    }
                }
                else
                    return null;
                if (data != null)
                {
                    dsParamData = null;
                    return LoadTableData(data, TableName, FileStructure);
                }
                else
                    return null;
            }
            else
                return null;
        }
        #endregion Data Loading
        #region Data Saving
        private Byte[] GetByteDataFromTable(String TableName)
        {
            if(dsParamData != null && dsParamData.Tables.Contains(TableName))
            {
                using(DataTable table = dsParamData.Tables[TableName])
                {
                    Byte[] data = new Byte[table.Rows.Count * 0x94]; // Create Data buffer for Rival Data. Entry size is 0x94
                    MemoryStream datastream = new MemoryStream(data);
                    BinaryWriter datawriter = new BinaryWriter(datastream);
                    foreach (DataRow row in table.Rows)
                    {
                        foreach (FileStructure structure in fsRivalData)
                        {
                            if (structure.varType != VarType.Index && structure.varType != VarType.Index1 && structure.varType != VarType.Button) // Ignore index and button var types as those are used by this app not game
                            {
                                Func<String, Int32> writedata = (String name) =>
                                {
                                    var cell = row[name];
                                    if (structure.dataType == typeof(Int32))
                                    {
                                        datawriter.Write(Convert.ToInt32(cell));
                                    }
                                    else if (structure.dataType == typeof(UInt32))
                                    {
                                        datawriter.Write(Convert.ToUInt32(cell));
                                    }
                                    else if (structure.dataType == typeof(Int16))
                                    {
                                        datawriter.Write(Convert.ToInt16(cell));
                                    }
                                    else if (structure.dataType == typeof(UInt16))
                                    {
                                        datawriter.Write(Convert.ToUInt16(cell));
                                    }
                                    else if (structure.dataType == typeof(SByte))
                                    {
                                        datawriter.Write(Convert.ToSByte(cell));
                                    }
                                    else if (structure.dataType == typeof(Byte))
                                    {
                                        datawriter.Write(Convert.ToByte(cell));
                                    }
                                    else if (structure.dataType == typeof(String))
                                    {
                                        Byte[] tempstringbuf = new Byte[structure.length];
                                        //Byte[] cellbytes = Encoding.ASCII.GetBytes(cell.ToString());
                                        Byte[] cellbytes = System.Text.Encoding.GetEncoding(932).GetBytes(cell.ToString());
                                        for (Int32 i = 0; i < cellbytes.Count(); i++)
                                        {
                                            tempstringbuf[i] = cellbytes[i];
                                        }
                                        datawriter.Write(tempstringbuf);
                                    }
                                    else if (structure.dataType == typeof(Single))
                                    {
                                        datawriter.Write(Convert.ToSingle(cell));
                                    }
                                    return 0;
                                };

                                if (structure.dataType != typeof(String) && structure.length > 0)
                                {
                                    for (Int32 i = 0; i < structure.length; i++)
                                    {
                                        writedata(structure.name + Convert.ToString(i + 1));
                                    }
                                }
                                else
                                {
                                    writedata(structure.name);
                                }
                            }
                        }
                    }
                    return data;
                }
            }
            return null;
        }
        public bool SaveELF(String FileName)
        {
            Byte[] data = GetByteDataFromTable("Rival Data");
            if (FileName != null && ELFData != null)
            {
                MemoryStream datastream = new MemoryStream(ELFData);
                BinaryReader datareader = new BinaryReader(datastream);

                UInt32 hash = GetHashFromELF(datareader, 64);
                Int32 dataOffset = Convert.ToInt32(GetELFDataOffset(hash));
                elfType = GetELFType(hash);

                if (dataOffset != 0)
                {
                    BinaryWriter datawriter = new BinaryWriter(datastream);
                    using (FileStream stream = new FileStream(FileName, FileMode.Create, FileAccess.Write))
                    {
                        datawriter.Seek(dataOffset, SeekOrigin.Begin);
                        datawriter.Write(data);
                        stream.Write(ELFData, 0, ELFData.Length);
                        ELFData = null; // Clear ELF Data once written
                        return true;
                    }
                }
                else
                    return false;
            }
            return false;
        }
        public bool SaveRivalData(String FileName)
        {
            Byte[] data = GetByteDataFromTable("Rival Data");
            if (FileName != null)
            {
                using (FileStream stream = new FileStream(FileName, FileMode.Create, FileAccess.Write))
                {
                    stream.Write(data, 0, data.Length);
                    return true;
                }
            }
            else
                return false;
        }
        public bool PushToPCSX2()
        {
            if (PCSX2.mProc.Process != null)
            {
                Int32 LoadedGameDataPtr = FindPCSX2Offset(
                    "20 ?? 36 00 E0 ?? 27 00 00 00 00 00 30 ?? 36 00 " +
                    "40 ?? 26 00 01 00 00 00 40 ?? 36 00 80 ?? 26 00 " +
                    "01 00 00 00 60 ?? 36 00 ?? ?? 2D 00 00 00 00 00 " +
                    "80 ?? 36 00 ?? ?? 2D 00 00 00 00 00 A0 ?? 36 00 " +
                    "A0 ?? 26 00 00 00 00 00 C0 ?? 36 00 ?? ?? 2D 00 " +
                    "00 00 00 00 D0 ?? 36 00 B0 ?? 26 00 00 00 00 00 " +
                    "E0 ?? 36 00 F0 ?? 26 00 00 00 00 00 F8 ?? 36 00 " +
                    "D0 ?? 26 00 02 00 00 00 00 00 00 00 00 00 00 00"
                    );
                if (LoadedGameDataPtr != 0)
                {
                    LoadedGameDataPtr += 0x80; // Offset which adjusts to rival data
                    String dataOffset = LoadedGameDataPtr.ToString("X");
                    if (dataOffset != "")
                    {
                        Byte[] data = GetByteDataFromTable("Rival Data");
                        PCSX2.WriteBytes(dataOffset, data);
                        return true;
                    }
                    else
                        return false;
                }
                return false;
            }
            else
                return false;
        }
        #endregion Data Saving
        #region Data Processing
        private object GetValue(Byte[] data, Int32 offset, System.Type datatype, Int32 length = 0)
        {
            object value = null;
            if(datatype == typeof(System.Byte))
            {
                value = Convert.ToByte(data[offset]);
            }
            else if (datatype == typeof(System.SByte))
            {
                value = Convert.ToSByte((SByte)data[offset]);
            }
            else if (datatype == typeof(System.UInt16))
            {
                value = BitConverter.ToUInt16(data, offset);
            }
            else if (datatype == typeof(System.Int16))
            {
                value = BitConverter.ToInt16(data, offset);
            }
            else if (datatype == typeof(System.UInt32))
            {
                value = BitConverter.ToUInt32(data, offset);
            }
            else if (datatype == typeof(System.Int32))
            {
                value = BitConverter.ToInt32(data, offset);
            }
            else if (datatype == typeof(System.UInt64))
            {
                value = BitConverter.ToUInt64(data, offset);
            }
            else if (datatype == typeof(System.Int64))
            {
                value = BitConverter.ToInt64(data, offset);
            }
            else if (datatype == typeof(System.Single))
            {
                value = BitConverter.ToSingle(data, offset);
            }
            else if (datatype == typeof(System.Double))
            {
                value = BitConverter.ToDouble(data, offset);
            }
            else if (datatype == typeof(System.String))
            {
                if (length == 0)
                {
                    length = GetStringLength(data, offset);
                }
                //String dataString = System.Text.ASCIIEncoding.ASCII.GetString(data, offset, length);
                String dataString = System.Text.Encoding.GetEncoding(932).GetString(data, offset, length);
                //String dataString = System.Text.Encoding.GetEncoding(51932).GetString(data, offset, length);
                value = dataString;
            }
            return value;
        }
        private Int32 GetStringLength(byte[] data, Int32 offset)
        {
            for(Int32 i = 0; i < data.Length - offset; i++)
            {
                if (data[offset + i] == 0) return i;
            }
            return 0;
        }
        private Int32 GetTypeLength(System.Type type)
        {
            if (type == typeof(System.Byte) || type == typeof(System.SByte))
                return sizeof(Byte);
            else if (type == typeof(System.Int16) || type == typeof(System.UInt16))
                return sizeof(Int16);
            else if (type == typeof(System.Single) || type == typeof(System.Int32) || type == typeof(System.UInt32))
                return sizeof(Int32);
            else if (type == typeof(System.Double) || type == typeof(System.Int64) || type == typeof(System.UInt64))
                return sizeof(Double);
            else
                return 0;
        }
        private Int32 GetLanguageRow(DataTable dt, Int32 id)
        {
            if(dt.Columns.Contains("ID"))
            {
                String select = "ID = " + Convert.ToString(id);
                DataRow[] rows = dt.Select(select);
                if (rows.Length > 0)
                {
                    Int32 index = dt.Rows.IndexOf(rows[0]);
                    return index;
                }
                else
                    return -1;
            }
            return -1;
        }
        private UInt32 GetHashFromELF(BinaryReader reader, UInt32 count)
        {
            UInt32 hash = 0;
            for (UInt32 i = 0; i < count; i++)
            {
                hash = (hash >> 1) | ((hash & 1) << 31);
                hash ^= reader.ReadUInt32();
            }
            return hash;
        }
        private UInt32 GetELFDataOffset(UInt32 hash)
        {
            for(Int32 i = 0; i < elfTypes.Count; i++)
            {
                if (elfTypes[i].hash == hash)
                    return elfTypes[i].address;
            }
            return 0;
        }
        private String GetELFType(UInt32 hash)
        {
            for (Int32 i = 0; i < elfTypes.Count; i++)
            {
                if (elfTypes[i].hash == hash)
                    return elfTypes[i].type;
            }
            return "Unknown";
        }
        #endregion Data Processing
    }
}
