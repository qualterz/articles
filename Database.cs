using LiteDB;

namespace Articles
{
    public static class Database
    {
        public static LiteDatabase Get()
        {
            return new LiteDatabase(Config.Instance.DatabasePath);
        }
    }
}
