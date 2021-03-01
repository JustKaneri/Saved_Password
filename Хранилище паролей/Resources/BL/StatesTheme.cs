using System.IO;

namespace Хранилище_паролей.Resources.BL
{
    public class StatesTheme
    {
        public static string fileName = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/" + "DarkThem" + ".xml";

        public static void SelectDark()
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.Write((byte)0);
            }
        }

        public static void SelectWhite()
        {
            if (File.Exists(fileName))
                File.Delete(fileName);
        }

        public static bool IsDark()
        {
            if (File.Exists(fileName))
                return true;
            else
                return false;
        }
    }
}