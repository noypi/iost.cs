namespace IOSTSdk.Helpers
{
    public class DataHelper
    {
        public static void DestroyData(byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = 0;
            }
        }
    }
}
