using CodeBase.Infrastructure.Services.System;

namespace CodeBase.Infrastructure.Services.Binding
{
    public interface IFilter
    {
        // bool Met(Binder linker);
        bool Met(SystemLinker linker);
    }
}