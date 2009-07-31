namespace FluentMvc.Spec
{
    using Rhino.Mocks;

    public abstract class TestBase
    {
        protected T CreateStub<T>() where T : class
        {
            return MockRepository.GenerateStub<T>();
        }

        protected T	 CreateStub<T>(params object[] args) where T : class
        {
            return MockRepository.GenerateStub<T>(args);
        }
    }
}