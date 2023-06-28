namespace VendingMachine.BLL.Identity
{
    /// <summary>
    /// Represents an application user.
    /// </summary>
    internal static class UserIdentity
    {
        static long _userId = 1;

        public static long UserId
        {
            get { return _userId; }
        }
    }
}
