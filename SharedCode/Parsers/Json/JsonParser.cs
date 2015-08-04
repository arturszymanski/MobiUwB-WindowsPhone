using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using SharedCode.Parsers.Json.Model;

namespace SharedCode.Parsers.Json
{
    public class JsonParser
    {
        public List<Feed> ParseFeedsJson(String jsonContent)
        {
            DataContractJsonSerializer serializer =
                new DataContractJsonSerializer(typeof(List<Feed>));

            List<Feed> jsonRoot;
            using (Stream jsonStream = GenerateStreamFromString(jsonContent))
            {
                jsonRoot = (List<Feed>)serializer.ReadObject(jsonStream);
            }
            return jsonRoot;
        }

        private Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
