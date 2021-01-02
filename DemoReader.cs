using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// CUSTOM SYSTEM IMPORTS
using System.IO;

namespace SeriousTTT_Demo_Reviewer
{
    public class DemoReader
    {
        private const string SOURCE_ENGINE_DEMO_HEADER = "GMODEMO";
        private const int SOURCE_ENGINE_DEMO_PROTOCOL = 3;
        private readonly BinaryReader _binaryReader;
        public DemoHeader Header;

        public DemoReader(BinaryReader binaryReader)
        {
            _binaryReader = binaryReader;
        }

        public DemoReader(string path)
        {
            var input = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            _binaryReader = new BinaryReader(input);
        }

        public void ReadHeader()
        {
            Header = new DemoHeader();
            Header.Header = StringUtils.ReadNullTerminatedString(_binaryReader);
            if (Header.Header != SOURCE_ENGINE_DEMO_HEADER)
            {
                throw new FileLoadException($"Demo header '{Header.Header}' is not equal to '{SOURCE_ENGINE_DEMO_HEADER}'!");
            }

            Header.DemoProtocol = _binaryReader.ReadInt32();
            if (Header.DemoProtocol != SOURCE_ENGINE_DEMO_PROTOCOL)
            {
                throw new FileLoadException($"Demo protocol '{Header.DemoProtocol}' is not equal to '{SOURCE_ENGINE_DEMO_PROTOCOL}'!");
            }

            Header.NetworkProtocol = _binaryReader.ReadInt32();
            Header.ServerName = StringUtils.ReadSizedNullString(_binaryReader, 260);
            Header.ClientName = StringUtils.ReadSizedNullString(_binaryReader, 260);
            Header.MapName = StringUtils.ReadSizedNullString(_binaryReader, 260);
            Header.GameDirectory = StringUtils.ReadSizedNullString(_binaryReader, 260);
            Header.PlaybackTime = _binaryReader.ReadSingle();
            Header.Ticks = _binaryReader.ReadInt32();
            Header.Frames = _binaryReader.ReadInt32();
            Header.SignonDataLength = _binaryReader.ReadInt32();
        }

        public class DemoHeader
        {
            public string Header { get; set; }
            public int DemoProtocol { get; set; }
            public int NetworkProtocol { get; set; }
            public string ServerName { get; set; }
            public string ClientName { get; set; }
            public string MapName { get; set; }
            public string GameDirectory { get; set; }
            public float PlaybackTime { get; set; }
            public int Ticks { get; set; }
            public int Frames { get; set; }
            public int SignonDataLength { get; set; }
            public int TickRate => (int)Math.Floor(Ticks / PlaybackTime);
        }
    }
}
