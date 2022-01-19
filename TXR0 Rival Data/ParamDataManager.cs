using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace TXR0_Rival_Data
{
    public class ParamDataManager
    {
        public enum VarType
        {
            Data,
            DataLength,
            Index,
            Index1
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
        #endregion Structures
        #region Variables
        ParamData paramData = new ParamData("Rival Data");
        public DataSet dsParamData = null;
        Byte[] ELFData = null;
        #endregion Variables
        #region File Structures
        public readonly List<FileStructure> fsRivalData = new List<FileStructure>() {
            new FileStructure(VarType.Index1, typeof(Int32), "Rival Number"), // 0x00
            new FileStructure(VarType.Data, typeof(Int16), "Team Member ID"), // 0x00
            new FileStructure(VarType.Data, typeof(Int16), "Appear Flag?"), // 0x02
            new FileStructure(VarType.Data, typeof(Int16), "Car ID"), // 0x04
            new FileStructure(VarType.Data, typeof(Int16), "Team ID"), // 0x06
            new FileStructure(VarType.Data, typeof(Int16), "Difficulty Table?"), // 0x08
            new FileStructure(VarType.Data, typeof(Int16), "Unknown 1"), // 0x0a
            new FileStructure(VarType.Data, typeof(Int16), "Flag 1"), // 0x0c
            new FileStructure(VarType.Data, typeof(Int16), "Unknown 2"), // 0x0e
            new FileStructure(VarType.Data, typeof(Int32), "Increment"), // 0x10
            new FileStructure(VarType.Data, typeof(String), "Rival Name 1", 0x11), // 0x14
            new FileStructure(VarType.Data, typeof(String), "Rival Name 2", 0x11), // 0x25
            new FileStructure(VarType.Data, typeof(Int16), "Rival Type"), // 0x36
            new FileStructure(VarType.Data, typeof(Int16), "Area?"), // 0x36
            new FileStructure(VarType.Data, typeof(Byte), "Flag 2"),  // 0x3a
            new FileStructure(VarType.Data, typeof(Byte), "Flag 3"),  // 0x3b
            new FileStructure(VarType.Data, typeof(Byte), "Flag 4"),  // 0x3c
            new FileStructure(VarType.Data, typeof(Byte), "Flag 5"), // 0x3d
            new FileStructure(VarType.Data, typeof(Byte), "Flag 6"), // 0x3e
            new FileStructure(VarType.Data, typeof(Byte), "Flag 7"), // 0x3f
            new FileStructure(VarType.Data, typeof(Byte), "Flag 8"), // 0x40
            new FileStructure(VarType.Data, typeof(Byte), "Flag 9"), // 0x41
            new FileStructure(VarType.Data, typeof(Byte), "Flag 10"), // 0x42
            new FileStructure(VarType.Data, typeof(Byte), "Flag 11"), // 0x43
            new FileStructure(VarType.Data, typeof(Int16), "Sticker"), // 0x44
            new FileStructure(VarType.Data, typeof(Byte), "Fender"), // 0x46
            new FileStructure(VarType.Data, typeof(Byte), "Front Bumper"), // 0x48
            new FileStructure(VarType.Data, typeof(Byte), "Bonnet"), // 0x49
            new FileStructure(VarType.Data, typeof(Byte), "Mirrors"), // 0x4a
            new FileStructure(VarType.Data, typeof(Byte), "Side Skirts"), // 0x4b
            new FileStructure(VarType.Data, typeof(Byte), "Rear Bumper"), // 0x4c
            new FileStructure(VarType.Data, typeof(Byte), "Wing"), // 0x4d
            new FileStructure(VarType.Data, typeof(Byte), "Grill"), // 0x4e
            new FileStructure(VarType.Data, typeof(Byte), "Lights"), // 0x4f
            new FileStructure(VarType.Data, typeof(Byte), "Wheels?"), // 0x50
            new FileStructure(VarType.Data, typeof(Byte), "Wheel Colour 1"), // 0x51
            new FileStructure(VarType.Data, typeof(Byte), "Wheel Colour 2"), // 0x52
            new FileStructure(VarType.Data, typeof(Byte), "Unknown 3"), // 0x53
            new FileStructure(VarType.Data, typeof(Byte), "Unknown 4"), // 0x54
            new FileStructure(VarType.Data, typeof(Byte), "Engine"), // 0x55
            new FileStructure(VarType.Data, typeof(Byte), "Exhaust"), // 0x56
            new FileStructure(VarType.Data, typeof(Byte), "Transmission"), // 0x57
            new FileStructure(VarType.Data, typeof(Byte), "Differential"), // 0x58
            new FileStructure(VarType.Data, typeof(Byte), "Tyres"), // 0x59
            new FileStructure(VarType.Data, typeof(Byte), "Brakes"), // 0x5a
            new FileStructure(VarType.Data, typeof(Byte), "Suspension"), // 0x5b
            new FileStructure(VarType.Data, typeof(Byte), "Body"), // 0x5c
            new FileStructure(VarType.Data, typeof(SByte), "Handling"), // 0x5d
            new FileStructure(VarType.Data, typeof(SByte), "Acceleration"), // 0x5e
            new FileStructure(VarType.Data, typeof(SByte), "Braking"), // 0x5f
            new FileStructure(VarType.Data, typeof(SByte), "Brake Balance"), // 0x60
            new FileStructure(VarType.Data, typeof(SByte), "Spring Rate Front"), // 0x61
            new FileStructure(VarType.Data, typeof(SByte), "Spring Rate Rear"), // 0x62
            new FileStructure(VarType.Data, typeof(SByte), "Damper Rate Front"), // 0x63
            new FileStructure(VarType.Data, typeof(SByte), "Damper Rate Rear"), // 0x64
            new FileStructure(VarType.Data, typeof(SByte), "Gear Ratio 1"), // 0x65
            new FileStructure(VarType.Data, typeof(SByte), "Gear Ratio 2"), // 0x66
            new FileStructure(VarType.Data, typeof(SByte), "Gear Ratio 3"), // 0x67
            new FileStructure(VarType.Data, typeof(SByte), "Gear Ratio 4"), // 0x68
            new FileStructure(VarType.Data, typeof(SByte), "Gear Ratio 5"), // 0x69
            new FileStructure(VarType.Data, typeof(SByte), "Gear Ratio 6"), // 0x6a
            new FileStructure(VarType.Data, typeof(SByte), "Final Drive"), // 0x6b
            new FileStructure(VarType.Data, typeof(SByte), "Suspension Height Front"), // 0x6c
            new FileStructure(VarType.Data, typeof(SByte), "Suspension Height Rear"), // 0x6d
            new FileStructure(VarType.Data, typeof(Byte), "Unknown 5"), // 0x6e
            new FileStructure(VarType.Data, typeof(Byte), "Wheels"), // 0x6f
            new FileStructure(VarType.Data, typeof(Byte), "Unknown 6"), // 0x6f
            new FileStructure(VarType.Data, typeof(Single), "R1"), //
            new FileStructure(VarType.Data, typeof(Single), "G1"),
            new FileStructure(VarType.Data, typeof(Single), "B1"),
            new FileStructure(VarType.Data, typeof(Single), "R2"),
            new FileStructure(VarType.Data, typeof(Single), "G2"),
            new FileStructure(VarType.Data, typeof(Single), "B2"),
            new FileStructure(VarType.Data, typeof(Byte), "Unknown 7"),
            new FileStructure(VarType.Data, typeof(Byte), "Unknown 8"),
            new FileStructure(VarType.Data, typeof(Byte), "Unknown 9"),
            new FileStructure(VarType.Data, typeof(Byte), "Unknown 10"),
            new FileStructure(VarType.Data, typeof(Byte), "Unknown 11"),
            new FileStructure(VarType.Data, typeof(Byte), "Unknown 12"),
            new FileStructure(VarType.Data, typeof(Byte), "Unknown 13"),
            new FileStructure(VarType.Data, typeof(Byte), "Unknown 14"),
            new FileStructure(VarType.Data, typeof(Byte), "Unknown 15"),
            new FileStructure(VarType.Data, typeof(Byte), "Unknown 16"),
            new FileStructure(VarType.Data, typeof(Byte), "Unknown 17"),
            new FileStructure(VarType.Data, typeof(Byte), "Unknown 18")
        };                                                         
        #endregion File Structures
        #region Data Loading
        public DataSet OpenFile(String TableName, String DataFileName, List<FileStructure> FileStructure)
        {
            //try
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
            //catch(Exception ex)
            //{
            //    MessageBox.Show(null, ex.Message, "Error loading data files", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return null;
            //}
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
                        reader.BaseStream.Position = 1464464; // Offset in SLUS_201.89
                        data = reader.ReadBytes(59200);
                        if (data.Length != 59200)
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
            //try
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
                                newRow[structure.name] = GetValue(data, dataOffset, structure.dataType);
                                dataOffset += GetTypeLength(structure.dataType);
                            }
                        }
                        dtTable.Rows.Add(newRow);
                    }
                    return dsParamData;
                }
                else
                    return null;
            }
            //catch (Exception)
            //{
            //    return null;
            //}
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
                            if (structure.varType != VarType.Index && structure.varType != VarType.Index1) // Ignore index var types as those are used by this app not game
                            {
                                var cell = row[structure.name];
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
                                    Byte[] cellbytes = Encoding.ASCII.GetBytes(cell.ToString());
                                    for (Int32 i = 0; i < cell.ToString().Length; i++)
                                    {
                                        tempstringbuf[i] = cellbytes[i];
                                    }
                                    datawriter.Write(tempstringbuf);
                                }
                                else if (structure.dataType == typeof(Single))
                                {
                                    datawriter.Write(Convert.ToSingle(cell));
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
                BinaryWriter datawriter = new BinaryWriter(datastream);
                using (FileStream stream = new FileStream(FileName, FileMode.Create, FileAccess.Write))
                {
                    datawriter.Seek(1464464, SeekOrigin.Begin);
                    datawriter.Write(data);
                    stream.Write(ELFData, 0, ELFData.Length);
                    ELFData = null; // Clear ELF Data once written
                    return true;
                }
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
                String dataString = System.Text.ASCIIEncoding.ASCII.GetString(data, offset, length);
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
        #endregion Data Processing
    }
}
