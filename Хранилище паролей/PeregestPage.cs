using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Хранилище_паролей.Resources.BL;

namespace Хранилище_паролей
{
    [Activity(Label = "PeregestPage")]
    public class PeregestPage : Activity
    {
        private Button btnSelect;
        private Button btnRegORLogOn;
        private EditText edtLog;
        private EditText edtPass;

        private TextView t1;
        private TextView t2;
        private LinearLayout l;

        private string fileName;

        /// <summary>
        /// Кол-во неверного входа.
        /// </summary>
        private int CountNotExet = -1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PageRegest);
            this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            // Create your application here

            btnSelect = FindViewById<Button>(Resource.Id.BtnSelect);
            btnRegORLogOn = FindViewById<Button>(Resource.Id.BtnLogIn);

            edtLog = FindViewById<EditText>(Resource.Id.EdtExtLogin);
            edtPass = FindViewById<EditText>(Resource.Id.EdtExtPassw);

            t1 = FindViewById<TextView>(Resource.Id.textView2);
            t2 = FindViewById<TextView>(Resource.Id.textView3);
            l = FindViewById<LinearLayout>(Resource.Id.LRegest);

            if(StatesTheme.IsDark())
            {
                SetDarkThem();
            }

            btnSelect.Click += BtnSelect_Click;
            btnRegORLogOn.Click += BtnRegORLogOn_Click;

        }

        /// <summary>
        /// Установка темной темы.
        /// </summary>
        private void SetDarkThem()
        {
            t1.SetTextColor(Color.White);
            t2.SetTextColor(Color.White);
            l.SetBackgroundColor(Color.Rgb(53, 55, 61));
            btnSelect.SetBackgroundColor(Color.Rgb(19, 32, 97));
            btnRegORLogOn.SetBackgroundColor(Color.Rgb(19, 32, 97));
            edtLog.SetTextColor(Color.White);
            edtPass.SetTextColor(Color.White);
        }

        /// <summary>
        /// Регистрация или вход.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRegORLogOn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(edtLog.Text) || string.IsNullOrWhiteSpace(edtPass.Text))
            {
                Toast.MakeText(this, "Вы не ввели логин или пароль", ToastLength.Long).Show();
                return;
            }

            if(edtPass.Text.Contains("#"))
            {
                Toast.MakeText(this, "Пароль содержит не допустимый символ", ToastLength.Long).Show();
                return;
            }

            if(edtPass.Text.Length < 4)
            {
                Toast.MakeText(this, "Лучше использовать более длинный пароль: 4 или более символов.", ToastLength.Long).Show();
                return;
            }

            bool isExit = false;
            fileName = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/" + edtLog.Text.Trim() + ".xml";

            if (btnRegORLogOn.Text == "Регистрация")
            {
                if(File.Exists(fileName))
                {
                    Toast.MakeText(this, "Пользователь уже существует.", ToastLength.Long).Show();
                    return;
                }
                Intent mainPag = new Intent(this, typeof(MainActivity));
                mainPag.PutExtra("Rlogin", edtLog.Text.Trim());
                mainPag.PutExtra("Rpass", edtPass.Text.Trim());
                SetResult(Result.Ok, mainPag);
                Finish();
            }
            else
            {
                LogIn(ref isExit);

                if (isExit == false)
                    return;

                Intent mainPag = new Intent(this, typeof(MainActivity));
                mainPag.PutExtra("LogIn", edtLog.Text);
                SetResult(Result.Ok, mainPag);
                Finish();
            }
        }

        /// <summary>
        /// Вход.
        /// </summary>
        /// <param name="f"></param>
        private void LogIn(ref bool f)
        {
            
            if (File.Exists(fileName))
            {
                if (!ControlerUser.IsLogIn(edtLog.Text.Trim(), edtPass.Text.Trim()))
                {
                    Toast.MakeText(this, "Не верный пароль", ToastLength.Short).Show();
                    CountNotExet++;

                    if(CountNotExet == 3)
                    {
                        CountNotExet = -1;
                        new Android.App.AlertDialog.Builder(this).
                            SetTitle("Ошибка входа").
                            SetMessage("Похоже у вас возникли проблемы со входом.\nВосстановить пароль?").
                            SetIcon(Resource.Drawable.key_1564).
                            SetPositiveButton("Да", delegate { RegenPass(); }).
                            SetNegativeButton("Нет", delegate { }).
                            Show();
                    }
                    f = false;
                }
                else
                    f = true;

            }
            else
            {
                Toast.MakeText(this, "Даной учетной записи не существует.", ToastLength.Short).Show();
            }
        }

        /// <summary>
        /// Выбор операции
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSelect_Click(object sender, EventArgs e)
        {
            if(btnSelect.Text == "Регистрация")
            {
                btnSelect.Text = "Вход";
                btnRegORLogOn.Text = "Регистрация";
            }
            else
            {
                btnSelect.Text = "Регистрация";
                btnRegORLogOn.Text = "Вход";
            }
            
        }

        /// <summary>
        /// Восстановление пароля.
        /// </summary>
        private void RegenPass()
        {
            string em = ControlerUser.GetEmail(edtLog.Text);
            if (em == "")
            {
                new Android.App.AlertDialog.Builder(this).
                            SetTitle("Упс...").
                            SetMessage("К сожалению но вы не привязали почту к данному аккаунту(").
                            SetIcon(Resource.Drawable.Error).
                            SetPositiveButton("Ok", delegate {}).
                            Show();
                return;
            }

            Intent i = new Intent(this, typeof(PagePasRegen));
            i.PutExtra("e-mail", em);
            i.PutExtra("Log", edtLog.Text);
            StartActivityForResult(i, 0);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if(resultCode == Result.Ok)
            {
                Intent mainPag = new Intent(this, typeof(MainActivity));
                mainPag.PutExtra("LogIn", edtLog.Text);
                SetResult(Result.Ok, mainPag);
                Finish();
            }
        }
    }
}