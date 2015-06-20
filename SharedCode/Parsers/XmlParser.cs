#region

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using SharedCode.IO;

#endregion

namespace SharedCode.Parsers
{
    public class XmlParser
    {
        private readonly IIOManagment _ioManager;

        public XmlParser(IIOManagment ioManager)
        {
            _ioManager = ioManager;
        }


        public void Deserialize<T>(
            String xmlFilePath,
            out T rootElement)
        {
            String fileName = Path.GetFileName(xmlFilePath);

            Stream stream;
            _ioManager.GetFileStreamFromStorageFolder(fileName,StreamType.ForRead, out stream);
            using (stream)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                rootElement = (T)serializer.Deserialize(stream);
            }
        }

        public void Serialize<T>(T rootElement, String fileName)
        {
            Stream stream = null;
            try
            {
                if (_ioManager.CheckIfFileExists(fileName) == false)
                {
                    _ioManager.CreateNewFileInStorageFolder(fileName);
                }

                _ioManager.GetFileStreamFromStorageFolder(
                    fileName,
                    StreamType.ForWrite,
                    out stream);

                stream.Seek(0, SeekOrigin.Begin);
                stream.SetLength(0);

                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(
                    stream,
                    rootElement);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
            }
        }

        private static void DisplayStream(Stream stream)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int ascii;
            stream.Position = 0;
            while ((ascii = stream.ReadByte()) != -1)
            {
                stringBuilder.Append((char) ascii);
            }
            String variable = stringBuilder.ToString();
            Debug.WriteLine(variable);
        }
    }
}
