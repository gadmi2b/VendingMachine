namespace VendingMachine.BLL.Infrastructure
{
    /// <summary>
    /// The exception that is thrown on BLL layer when Presentation layer
    /// sends data which violates business rules
    /// </summary>
    public class VendingMachineException : Exception
    {
        public string Property { get; protected set; } = string.Empty;

        public VendingMachineException() { }
        public VendingMachineException(string message) : base(message) { }
        public VendingMachineException(string message, string property) : base(message)
        {
            Property = property;
        }
        public VendingMachineException(string message, string property, Exception innerException) : base(message, innerException)
        {
            Property = property;
        }
    }
}
