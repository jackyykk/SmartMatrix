namespace SmartMatrix.Core.ErrorHandling
{
    public class Throw : IThrow
    {
        public static IThrow Exception { get; } = new Throw();
        private Throw()
        {
        }
    }
}