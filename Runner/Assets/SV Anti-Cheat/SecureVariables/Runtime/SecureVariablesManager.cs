namespace SecureVariables
{
    using System;

    public static class SecureVariablesManager
    {
        public static ICryptographer Cryptographer => _cryptographer ?? (_cryptographer = new KeyedCryptographer());

        private static ICryptographer _cryptographer;
    }
}
