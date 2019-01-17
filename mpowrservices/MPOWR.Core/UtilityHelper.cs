using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace MPOWR.Core
{
    public class UtilityHelper
    {
        public static string ObjectToJsonString(object obj)
        {
            if(obj==null)
                return null;
            return JsonConvert.SerializeObject(obj);
        }

        public static string ObjectToJsonString(object obj,int count)
        {
            if (count == 0)
                return null;
            return JsonConvert.SerializeObject(obj);
        }

        public static object JsonStringToObject(string str, Type type)
        {
            return JsonConvert.DeserializeObject(str,type);
        }
         

        public static byte[] ConvertDataSetToByteArray(DataSet dataTable)
        {
            byte[] binaryDataResult = null;
            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter brFormatter = new BinaryFormatter();
                dataTable.RemotingFormat = SerializationFormat.Binary;
                brFormatter.Serialize(memStream, dataTable);
                binaryDataResult = memStream.ToArray();
            }
            return binaryDataResult;
        }

        public static Stream ConvertTableToStream(DataTable table)
        {
            System.IO.MemoryStream xmlStream = new System.IO.MemoryStream();
            table.WriteXml(xmlStream, XmlWriteMode.WriteSchema);

            // Rewind the memory stream.
            xmlStream.Position = 0;
            return xmlStream;
        }
 
    }
}
