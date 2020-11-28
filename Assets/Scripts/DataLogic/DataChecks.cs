using System.Linq;

namespace DataLogic
{
    public static class DataChecks
    {
        public static bool CheckForIdExists(string[] list, string stringToCheck)
        {
            return list.Contains(stringToCheck);
        }

        public static string EnsureUnique(string[] list, string id)
        {
            int append = 1;
            while (CheckForIdExists(list, id))
            {
                if (id.EndsWith($"_{ append }"))
                {
                    id = id.Remove(id.Length - $"_{ append }".Length, $"_{ append }".Length);
                }

                append++;
                id += $"_{ append }";
            }
            return id;
        }

        public static int GetMax(int first, int second)
        {
            return first >= second ? first : second;
        }

    }
}
