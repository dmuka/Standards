namespace Application.UseCases.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class TransactionScopeAttribute : Attribute
    {
    }
}
