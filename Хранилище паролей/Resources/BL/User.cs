using System;
using System.Collections.Generic;

namespace Хранилище_паролей.Resources.BL
{
    [Serializable]
    public class User
    {
        /// <summary>
        /// Логин пользователя.
        /// </summary>
        public string Login { get;set; }
        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        public string Passw{ get; set; }
        /// <summary>
        /// Список паролей пользователя.
        /// </summary>
        public List<Password> ListPassword { get; set; } = new List<Password>();
        /// <summary>
        /// Адрес электроной почты для восстановления доступа.
        /// </summary>
        public string E_mail { get; set; }
    }
}