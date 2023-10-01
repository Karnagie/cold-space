using CodeBase.Infrastructure.Services.Binding;

namespace CodeBase.Infrastructure.Factories
{
    public class BinderFactory
    {
        private BinderService _binderService;

        public BinderFactory(BinderService binderService)
        {
            _binderService = binderService;
        }

        public Binder Create()
        {
            var binder = Binder.Factory.Create(_binderService);
            return binder;
        }
    }
}