namespace CurrenciesSERVICE_2._0
{
    public class ConnStr
    {
        private static string _ConnStr { get; set; }
        private static ConnStr Conn { get; set; }

        private ConnStr(string connectionString)
        {
            _ConnStr = connectionString;
        }
        public static void SetConnStr(string connectionString)
        {
            if (Conn == null)
                Conn = new ConnStr(connectionString);
        }
        public static string GetConnStr()
        {
            return _ConnStr;
        }
    }
}
