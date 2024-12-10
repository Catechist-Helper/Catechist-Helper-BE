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
                throw new ArgumentException($"Tồn tại", nameof(value));
            }
        }

        public static void CheckPageInput(params int[] values)
        {
            if (values.Length == 0)
            {
                throw new ArgumentException("At least one value must be provided.");
            }

            foreach (var value in values)
            {
                if (value < 1)
                {
                    throw new ArgumentException("All values must be greater than or equal to 1.");
                }
            }
        }




    }
}
