using Infrastructure;

namespace Barebone.ViewModels
{
    public class ViewModelFactoryBase
    {
         protected IRequestHandler RequestHandler { get; set; }

        public ViewModelFactoryBase(IRequestHandler requestHandler)
        {
            this.RequestHandler = requestHandler;
        }
    }
}