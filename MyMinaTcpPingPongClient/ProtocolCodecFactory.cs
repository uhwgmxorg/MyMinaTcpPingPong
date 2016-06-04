using Mina.Filter.Codec.Demux;
using System;

namespace MyMinaTcpPingPongClient
{
    class ProtocolCodecFactory : DemuxingProtocolCodecFactory
    {
        public ProtocolCodecFactory()
        {
            AddMessageDecoder<TCPClientProtocolManager>();
            AddMessageEncoder<byte[], TCPClientProtocolManager>();
        }
    }
}
