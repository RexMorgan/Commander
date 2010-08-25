using Commander.Registration.Dsl;

namespace Commander.Registration
{
    public interface IRegistryModification
    {
        void Modify(CommandRegistry registry);
    }
}