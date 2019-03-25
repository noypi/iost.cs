namespace IOSTSdk
{
    using Grpc.Core;

    internal class Channels
    {
        internal static Channel NewAustrialia() => new Channel(Servers.Australia, ChannelCredentials.Insecure);
        internal static Channel NewCanada() => new Channel(Servers.Canada, ChannelCredentials.Insecure);
        internal static Channel NewFrance() => new Channel(Servers.France, ChannelCredentials.Insecure);
        internal static Channel NewGermany() => new Channel(Servers.Germany, ChannelCredentials.Insecure);
        internal static Channel NewJapan() => new Channel(Servers.Japan, ChannelCredentials.Insecure);
        internal static Channel NewKorea() => new Channel(Servers.Korea, ChannelCredentials.Insecure);
        internal static Channel NewUK() => new Channel(Servers.UK, ChannelCredentials.Insecure);
        internal static Channel NewUS() => new Channel(Servers.US, ChannelCredentials.Insecure);
    }
}
