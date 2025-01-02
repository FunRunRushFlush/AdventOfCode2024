//Thanks to: https://github.com/viceroypenguin
public record Input(byte[] Bytes, string Text, string[] Lines)
{
    public ReadOnlySpan<string> SpanLines => Lines;
    public ReadOnlySpan<byte> Span => Bytes;
}
