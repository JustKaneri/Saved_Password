using DLL_Shifrated;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Хранилище_паролей.Resources.BL
{
    public class ControlerUser
    {
        public User user { get; private set; }

        /// <summary>
        /// Регистрация.
        /// </summary>
        public ControlerUser(string login,string passw)
        {
            user = new User();
            user.Login = login;
            user.Passw = Shifrator.Shifrated(passw);

            FillFile(login);
        }

        /// <summary>
        /// Вход.
        /// </summary>
        /// <param name="login"></param>
        public ControlerUser(string login)
        {
           user =  ReadInFile(login);
        }

        /// <summary>
        /// Чтение из файла.
        /// </summary>
        /// <param name="login"></param>
        private static User ReadInFile(string login)
        {
            string fileName = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/" + login + ".xml";

            XmlSerializer serializer = new XmlSerializer(typeof(User));

            using (FileStream fs = File.OpenRead(fileName))
            {
                return (User)serializer.Deserialize(fs);
            }
        }

        /// <summary>
        /// Запись в файл.
        /// </summary>
        /// <param name="Name">имя файла.</param>
        private void FillFile(string Name)
        {
            string fileName = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/" + Name + ".xml";

            XmlSerializer serializer = new XmlSerializer(typeof(User));

            if (File.Exists(fileName))
                File.Delete(fileName);

            using (FileStream fs = File.OpenWrite(fileName))
            {
                serializer.Serialize(fs, user);
            }
        }

        /// <summary>
        /// Проверка на подлиность.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static bool IsLogIn(string login,string pass)
        {
            User testUser = ReadInFile(login);

            if (testUser.Login == login && Shifrator.DeShifrat(testUser.Passw) == pass)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Редактирование профиля.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="passw"></param>
        public void EditUser(string login,string passw,string em)
        {
            string fileName = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/" + login + ".xml";
            if (File.Exists(fileName))
                File.Delete(fileName);

            user.Login = login;
            user.Passw = passw;
            user.E_mail = em;
            FillFile(login);
        }

        /// <summary>
        /// Добавление пароля.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        public void AddPassw(string name,string login,string password)
        {
            Password newPassw = new Password();
            newPassw.Name = name;
            newPassw.Login = Shifrator.Shifrated(login);
            newPassw.Passwd = Shifrator.Shifrated(password);

            user.ListPassword.Add(newPassw);

            FillFile(user.Login);
        }

        /// <summary>
        /// Редактирование пароля.
        /// </summary>
        /// <param name="idPassw"></param>
        /// <param name="name"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        public void EditPassw(int idPassw,string name,string login,string password)
        {
            user.ListPassword[idPassw].Name = name;
            user.ListPassword[idPassw].Login = Shifrator.Shifrated(login);
            user.ListPassword[idPassw].Passwd = Shifrator.Shifrated(password);

            FillFile(user.Login);
        }

        /// <summary>
        /// Удаление пароля.
        /// </summary>
        /// <param name="idPassw"></param>
        public void DeletePassword(int idPassw)
        {
            user.ListPassword.RemoveAt(idPassw);
            if (user.ListPassword.Count == 0)
                user.ListPassword = new List<Password>();
            FillFile(user.Login);
        }

        /// <summary>
        /// Получить все пароли.
        /// </summary>
        /// <returns></returns>
        public List<string> GetPassw()
        {
            List<string> lst = new List<string>();

            for (int i = 0; i < user.ListPassword.Count; i++)
            {
                string t = user.ListPassword[i].Name 
                            + "#" + Shifrator.DeShifrat(user.ListPassword[i].Login) 
                            + "#" + Shifrator.DeShifrat(user.ListPassword[i].Passwd);
                lst.Add(t);
            }

            return lst;
        }

        /// <summary>
        /// Получить электроную почту выбраного пользователя.
        /// </summary>
        /// <returns></returns>
        public static string GetEmail(string login)
        {
            User tst = ReadInFile(login);

            if (!string.IsNullOrWhiteSpace(tst.E_mail))
                return Shifrator.DeShifrat(tst.E_mail);
            else
                return "";
        }
    }
}