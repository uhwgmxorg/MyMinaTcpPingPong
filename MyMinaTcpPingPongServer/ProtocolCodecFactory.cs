using Mina.Filter.Codec.Demux;
using System;

namespace MyMinaTcpPingPongServer
{
    class ProtocolCodecFactory : DemuxingProtocolCodecFactory
    {
        public ProtocolCodecFactory()
        {
            AddMessageDecoder<TCPServerProtocolManager>();
            AddMessageEncoder<byte[], TCPServerProtocolManager>();
        }
    }
}
