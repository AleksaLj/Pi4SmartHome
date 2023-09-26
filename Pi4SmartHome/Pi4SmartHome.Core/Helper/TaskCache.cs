
namespace Pi4SmartHome.Core.Helper
{
    public static class TaskCache
    {
        public static readonly Task<bool> True = Task.FromResult(true);
        public static readonly Task<bool> False = Task.FromResult(false);
        public static readonly Task<int> Zero = Task.FromResult(0);
        public static Task<T?> ObjectValue<T>(T? objectValue) => Task.FromResult(objectValue);
    }
}
