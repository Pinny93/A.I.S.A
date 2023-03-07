namespace A.I.S.A.Utils
{
    public static class StreamExtensions
    {
        public static void Clear(this MemoryStream ms)
        {
            var buffer = ms.GetBuffer();
            Array.Clear(buffer, 0, buffer.Length);
            ms.Position = 0;
            ms.SetLength(0);
            ms.Capacity = 0;
        }
    }
}