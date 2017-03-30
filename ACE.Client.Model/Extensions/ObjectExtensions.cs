namespace ACE.Client.Model.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsInteger(this string testingValue)
        {
            int intValue;
            var isInteger = int.TryParse(testingValue, out intValue);
            return isInteger;
        }

        public static bool IsAlpha(this string testingValue)
        {
            var upperA = (int) 'A';
            var upperZ = (int) 'Z';
            var lowerA = (int)'a';
            var lowerZ = (int)'z';

            foreach (var testingChar in testingValue.ToCharArray())
            {
                if (lowerA <= testingChar && testingChar <= lowerZ) continue;
                if (upperA <= testingChar && testingChar <= upperZ) continue;
                return false;
            }

            return true;
        }
    }
}
