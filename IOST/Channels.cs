namespace IOSTSdk
{
    using Grpc.Core;

    internal class Channels
    {
        internal static Channel NewAustrialia() => NewServer(Servers.Australia);
        internal static Channel NewCanada() => NewServer(Servers.Canada);
        internal static Channel NewFrance() => NewServer(Servers.France);
        internal static Channel NewGermany() => NewServer(Servers.Germany);
        internal static Channel NewJapan() => NewServer(Servers.Japan);
        internal static Channel NewKorea() => NewServer(Servers.Korea);
        internal static Channel NewUK() => NewServer(Servers.UK);
        internal static Channel NewUS() => NewServer(Servers.US);

        internal static Channel NewServer(string loc) => new Channel(loc, ChannelCredentials.Insecure);
    }
}
