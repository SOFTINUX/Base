namespace Infrastructure
{
    public interface IDatabaseInitializer
    {
        void CheckAndInitialize(IRequestHandler context_);
    }
}
