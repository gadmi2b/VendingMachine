namespace VendingMachine.BLL.Identity
{
    internal static class UserIdentity
    {
        static long _userId = 1;

        public static long UserId
        {
            get { return _userId; }
        }
    }
}
