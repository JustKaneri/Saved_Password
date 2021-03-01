using System;
using Android.Graphics;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Хранилище_паролей.Resources.BL;

namespace Хранилище_паролей
{
    [Activity(Label = "PropertiPage")]
    public class PropertiPage : Activity
    {
        private Button btnEdt;
        private Button btnCansel;
        private Button btnInfo;

        private RadioButton rbWhite;
        private RadioButton rbDark;

        private EditText edLogin;
        private EditText edPass;
        private EditText edtEmail;

        #region Другие части формы
        private LinearLayout l;
        private TextView t1;
        private TextView t2;
        private TextView t3;
        private TextView t4;
        private TextView t5;
        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PageProperti);
            this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            // Create your application here

            btnEdt = FindViewById<Button>(Resource.Id.BtnEdtUser);
            btnCansel = FindViewById<Button>(Resource.Id.BtnCanselProper);
            btnInfo = FindViewById<Button>(Resource.Id.BtnInformation);

            rbWhite = FindViewById<RadioButton>(Resource.Id.RbWhiteThem);
            rbDark = FindViewById<RadioButton>(Resource.Id.RbDarkThem);

            edLogin = FindViewById<EditText>(Resource.Id.EdtLoginProper);
            edPass = FindViewById<EditText>(Resource.Id.EdtPassProper);
            edtEmail = FindViewById<EditText>(Resource.Id.EdtEmail);

            #region Подключение других частей
            l = FindViewById<LinearLayout>(Resource.Id.LProper);
            t1 = FindViewById<TextView>(Resource.Id.textView1);
            t2 = FindViewById<TextView>(Resource.Id.textView2);
            t3 = FindViewById<TextView>(Resource.Id.textView3);
            t4 = FindViewById<TextView>(Resource.Id.textView4);
            t5 = FindViewById<TextView>(Resource.Id.TxtVEma);
            #endregion

            if (StatesTheme.IsDark())
            {
                rbDark.Checked = true;
                SetDarkThem();
            }
               

            edLogin.Text = Intent.GetStringExtra("Log");
            edPass.Text = Intent.GetStringExtra("Pass");
            edtEmail.Text = Intent.GetStringExtra("Email");

            btnCansel.Click += BtnCansel_Click;
            btnEdt.Click += BtnEdt_Click;
            btnInfo.Click += BtnInfo_Click;

            rbWhite.CheckedChange += RbWhite_CheckedChange;
            rbDark.CheckedChange += RbWhite_CheckedChange;
        }

        /// <summary>
        /// Вывод информации о программе.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnInfo_Click(object sender, EventArgs e)
        {
            new Android.App.AlertDialog.Builder(this).
                          SetTitle("Информация о приложении:").
                          SetMessage("Название: Хранилище паролей\n" +
                          "Версия:1.0v\n" +
                          "Создатель: Тесновец Илья\n" +
                          "Описание:\n" +
                          "Приложение используется для хранения паролей от различных аккаунтов в одном месте " +
                          "и вам достаточно помнить только один пароль," +
                          " а не запоминать пару десятков.\n\n\n" +
                          "\t\t\t\t\tВсе права защищены.\n" 
                          ).
                          SetIcon(Resource.Drawable.information_info_1565).
                          SetPositiveButton("Ок", delegate { }).
                          Show();
        }

        /// <summary>
        /// Выбор темы.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RbWhite_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
           if(rbWhite.Checked)
           {
                SetWhiteThem();
                StatesTheme.SelectWhite();
           }
           else
           {
                StatesTheme.SelectDark();
                SetDarkThem();
           }
        }

        /// <summary>
        /// Установить светлую тему.
        /// </summary>
        private void SetWhiteThem()
        {
            t1.SetTextColor(Color.Black);
            t2.SetTextColor(Color.Black);
            t3.SetTextColor(Color.Black);
            t4.SetTextColor(Color.Black);
            t5.SetTextColor(Color.Black);
            rbDark.SetTextColor(Color.Black);
            rbWhite.SetTextColor(Color.Black);
            l.SetBackgroundColor(Color.White);
            btnEdt.SetBackgroundColor(Color.Rgb(70,130,180));
            btnCansel.SetBackgroundColor(Color.Rgb(70, 130, 180));
            edLogin.SetTextColor(Color.Black);
            edPass.SetTextColor(Color.Black);
            edtEmail.SetTextColor(Color.Black);

        }

        /// <summary>
        /// Установить темную тему.
        /// </summary>
        private void SetDarkThem()
        {
            t1.SetTextColor(Color.White);
            t2.SetTextColor(Color.White);
            t3.SetTextColor(Color.White);
            t4.SetTextColor(Color.White);
            t5.SetTextColor(Color.White);
            rbDark.SetTextColor(Color.White);
            rbWhite.SetTextColor(Color.White);
            l.SetBackgroundColor(Color.Rgb(53, 55, 61));
            btnEdt.SetBackgroundColor(Color.Rgb(19, 32, 97));
            btnCansel.SetBackgroundColor(Color.Rgb(19, 32, 97));
            edLogin.SetTextColor(Color.White);
            edPass.SetTextColor(Color.White);
            edtEmail.SetTextColor(Color.White);
        }

        private void BtnEdt_Click(object sender, EventArgs e)
        {
            if(btnEdt.Text == "Редактировать")
            {
                edPass.InputType = Android.Text.InputTypes.ClassText | Android.Text.InputTypes.Null;
                edLogin.Enabled = edPass.Enabled = edtEmail.Enabled =true;
                
                btnEdt.Text = "Сохранить";
                btnCansel.Visibility = ViewStates.Visible;
            }
            else
            {
                if(string.IsNullOrWhiteSpace(edLogin.Text) || string.IsNullOrWhiteSpace(edPass.Text))
                {
                    Toast.MakeText(this, "Поля не могут быть пустыми", ToastLength.Short).Show();
                    return;
                }

                if(edPass.Text.Contains("#"))
                {
                    Toast.MakeText(this, "Пароль содержит не допустимые поля", ToastLength.Short).Show();
                    return;
                }

                if (edPass.Text.Length < 4)
                {
                    Toast.MakeText(this, "Пароль слишком короткий", ToastLength.Short).Show();
                    return;
                }

                if(edtEmail.Text != "" && !CheckEmail())
                {
                    Toast.MakeText(this, "Не правильный электронный адрес.", ToastLength.Short).Show();
                    return;
                }

                Intent main = new Intent(this, typeof(MainActivity));
                main.PutExtra("Login",edLogin.Text);
                main.PutExtra("Pass", edPass.Text);
                main.PutExtra("email", edtEmail.Text);
                SetResult(Result.Ok, main);
                Finish();
            }
        }

        /// <summary>
        /// Отмена редактирования.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCansel_Click(object sender, EventArgs e)
        {

            edPass.InputType = Android.Text.InputTypes.TextVariationPassword | Android.Text.InputTypes.ClassText;
            edLogin.Enabled = edPass.Enabled = edtEmail.Enabled= false;
            btnEdt.Text = "Редактировать";
            btnCansel.Visibility = ViewStates.Invisible;
        }

        /// <summary>
        /// Проверка E-mail на коректность.
        /// </summary>
        /// <returns></returns>
        private bool CheckEmail()
        {
            string e = edtEmail.Text;

            if (e.StartsWith('@') || e.EndsWith('@'))
                return false;

            if (!e.Contains("@"))
                return false;

            e = e.Substring(e.IndexOf("@") + 1);

            if (e.IndexOf(".") == e.IndexOf("@") + 1)
                return false;

            int cnt = 0;
            for (int i = 0; i < e.Length; i++)
            {

                if (e[i] == '.')
                    cnt++;
            }

            if (cnt > 1)
                return false;


            return true;
        }
    }
}