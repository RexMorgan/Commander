using System;

namespace Commander.Registration.Dsl
{
    public interface IRegistrationConvention
    {
        void Process(Type type, CommandRegistry registry);
    }
}