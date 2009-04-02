using Rhino.Mocks;

namespace FluentMvc.Spec
{
    public abstract class TestBase
    {
        protected T CreateStub<T>() where T : class
        {
            return MockRepository.GenerateStub<T>();
        }
    }
}