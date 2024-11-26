namespace CatechistHelper.Infrastructure.Utils
{
    public static class Validator
    {
        public static T EnsureNonNull<T>(T? value) where T : class
        {
            return value ?? throw new ArgumentNullException(nameof(value));
        }

        public static void EnsureNonExist<T>(T? value) where T : class
        {
            if (value != null)
            {
                throw new ArgumentException($"Exist", nameof(value));
            }
        }
    }
}
