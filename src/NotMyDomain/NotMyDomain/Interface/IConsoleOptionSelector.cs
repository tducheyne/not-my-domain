namespace NotMyDomain.Interface
{
    internal interface IConsoleOptionSelector<T>
    {
        T Execute(bool defaultFirstOption = true);
    }
}