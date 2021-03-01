using System;
using System.Net;
using System.Net.Mail;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using DLL_Shifrated;
using Хранилище_паролей.Resources.BL;

namespace Хранилище_паролей
{
    [Activity(Label = "PagePasRegen")]
    public class PagePasRegen : Activity
    {
        private Button btn;
        private EditText edt;
        private LinearLayout l;

        private string email;
        private int Key;
        private string Log ="";

        private const string Login = "bOz9KK6KCYz|yVcbjfdyhgp";
        private const string Password = "TwoQRDny6OPD?$po}$CKc$C^=";
        private const string EmailName = "STE4BUEP6Gz|yVcbjfdyhgpIdh`e'{|";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            SetContentView(Resource.Layout.RegenPasPage);
            // Create your application here

            btn = FindViewById<Button>(Resource.Id.BtnNext);
            edt = FindViewById<EditText>(Resource.Id.EdtEmailRegen);
            l = FindViewById<LinearLayout>(Resource.Id.LRegen);

            if (StatesTheme.IsDark())
            {
                SetDarkThem();
            }

            email = Intent.GetStringExtra("e-mail");
            Log = Intent.GetStringExtra("Log");

            btn.Click += Btn_Click;
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            
            if (btn.Text == "Дальше")
            {
                if (edt.Text == "")
                {
                    Toast.MakeText(this, "Введите адрес электроной почты.", ToastLength.Short).Show();
                    return;
                }

                if (edt.Text == email)
                {
                    if(!IsConnetInternet())
                    {
                        Toast.MakeText(this, "Отсутствует интернет подключение.", ToastLength.Short).Show();
                        return;

                    }

                    SendKey();

                    btn.Text = "Восстановить";
                    edt.Text = "";
                    edt.Hint = "Введите код восстановления";
                    Toast.MakeText(this, "Код восстановления отправлен вам на почту(Возможно в разделе спам).", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this, "Не правильный адрес электроной почты.", ToastLength.Short).Show();
                    return;
                }
            }

            if(btn.Text == "Восстановить")
            {
                if(edt.Text == Key.ToString())
                {
                    Intent m = new Intent(this, typeof(PeregestPage));
                    SetResult(Result.Ok, m);
                    Finish();
                }
            } 
        }

        /// <summary>
        /// Установка темной темы.
        /// </summary>
        private void SetDarkThem()
        {
            l.SetBackgroundColor(Color.Rgb(53, 55, 61));
            btn.SetBackgroundColor(Color.Rgb(19, 32, 97));
            edt.SetTextColor(Color.White);
        }

        /// <summary>
        /// Отправка кода на почту.
        /// </summary>
        private void SendKey()
        {
            MailAddress from = new MailAddress(Shifrator.DeShifrat(EmailName), "Восстановление пароля");
            MailAddress to = new MailAddress(edt.Text);

            MailMessage message = new MailMessage(from, to);
            message.Subject = "Kод восстановления доступа в приложении Хранилище паролей.";

            Random rnd = new Random();
            Key = rnd.Next(1000, 10000);

            message.Body = "Ваш код для восстановления доступа: " + Key + " для аккаунта " + Log +"\n" +
                           "Если вы не пытаетесь восстановить доступ просто поигнорируйте это сообщение и не сообщайте никому код.";

            SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(Shifrator.DeShifrat(Login), Shifrator.DeShifrat(Password));

            try
            {
                smtp.Send(message);
            }
            catch (Exception)
            {
                Toast.MakeText(this, "Не удалось отправить код восстановления.", ToastLength.Short).Show();
                return;
            }
        }

        private bool IsConnetInternet()
        {
            using (var client = new WebClient())
            {
                try
                {
                    using (var stream = client.OpenRead("http://www.google.com"))
                    {
                        return true;
                    }
                }
                catch 
                {
                    return false;
                }
            }
        }
    }
}