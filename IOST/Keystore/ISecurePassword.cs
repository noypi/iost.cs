namespace IOSTSdk.Keystore
{
    public interface SecurePassword
    {
        void Append(char c);
        void Backspace();
        void InsertAt(char c);
    }
}