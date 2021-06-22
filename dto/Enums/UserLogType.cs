using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace dto.Enums
{
    public enum UserLogType
    {
        [Description("Kullanıcı adı veya şifre yanlış.")]
        WrongLogIn = 1,

        [Description("Kullanıcı giriş yaptı.")]
        LogIn = 2,

        [Description("Çıkış yapıldı.")]
        LogOut = 3,
    }
}
