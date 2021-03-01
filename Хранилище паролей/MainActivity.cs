using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Collections.Generic;
using Android.Graphics;
using Хранилище_паролей.Resources.BL;
using Android.Content;
using DLL_Shifrated;

namespace Хранилище_паролей
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        #region Переменые.
        /// <summary>
        /// Меню выбора пароля.
        /// </summary>
        private Spinner spinPass;
        /// <summary>
        /// Кнопка показа пароля.
        /// </summary>
        private Button btnShowPass;
        /// <summary>
        /// Кнопка добавление пароля.
        /// </summary>
        private Button btnAddPass;
        /// <summary>
        /// Кнопка редактирования пароля.
        /// </summary>
        private Button btnEditPass;
        /// <summary>
        /// Кнопка удаления пароля.
        /// </summary>
        private Button btnDelPass;
        /// <summary>
        /// Кнопка выхода.
        /// </summary>
        private Button btnExit;
        /// <summary>
        /// Кнопка настройки.
        /// </summary>
        private Button btnPropert;

        /// <summary>
        /// Текстовое поле с логином.
        /// </summary>
        private EditText edtLogin;
        /// <summary>
        /// Текстовое поле с паролем.
        /// </summary>
        private EditText edtPass;

        /// <summary>
        /// Экземпляр класса контролер.
        /// </summary>
        private static ControlerUser controlerUser;

        /// <summary>
        /// Список паролей.
        /// </summary>
        private List<string> listPassw = new List<string>();
        #endregion

        #region Для смена темы.
        private LinearLayout l;
        private TextView t1;
        private TextView t2;
        private TextView t3;
        private Toolbar tb;
        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            spinPass = FindViewById<Spinner>(Resource.Id.SpinPass);
            btnShowPass = FindViewById<Button>(Resource.Id.BtnShowPass);
            btnAddPass = FindViewById<Button>(Resource.Id.BtnAddPas);
            btnEditPass = FindViewById<Button>(Resource.Id.BtnEditPass);
            btnDelPass = FindViewById<Button>(Resource.Id.BtnDelPass);
            btnPropert = FindViewById<Button>(Resource.Id.BtnPropert);
            btnExit = FindViewById<Button>(Resource.Id.BtnExit);

            edtLogin = FindViewById<EditText>(Resource.Id.EdtLogin);
            edtPass = FindViewById<EditText>(Resource.Id.EdtPass);


            l = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
            t1 = FindViewById<TextView>(Resource.Id.textM);
            t2 = FindViewById<TextView>(Resource.Id.textM2);
            t3 = FindViewById<TextView>(Resource.Id.textM3);
            tb = FindViewById<Toolbar>(Resource.Id.toolbar1);

            //Вызов авторизации.
            Intent LogOn = new Intent(this, typeof(PeregestPage));
            StartActivityForResult(LogOn, 0);

            spinPass.ItemSelected += SpinPass_ItemSelected;
            SetPassword();

            if (StatesTheme.IsDark())
                SetDarkThem();
            else
                SetWhiteThem();

            btnShowPass.Click += BtnShowPass_Click;
            btnExit.Click += BtnExit_Click;
            btnDelPass.Click += BtnDelPass_Click;
            btnAddPass.Click += BtnAddPass_Click;
            btnEditPass.Click += BtnEditPass_Click;
            btnPropert.Click += BtnPropert_Click;
            
        }

        /// <summary>
        /// Установка темной темы.
        /// </summary>
        private void SetDarkThem()
        {
            t1.SetTextColor(Color.White);
            t2.SetTextColor(Color.White);
            t3.SetTextColor(Color.White);
            edtLogin.SetTextColor(Color.White);
            edtPass.SetTextColor(Color.White);
            try
            {
                ((TextView)spinPass.GetChildAt(0)).SetTextColor(Color.White);
            }
            catch 
            {
            }
                
            l.SetBackgroundColor(Color.Rgb(53, 55, 61));
            tb.SetBackgroundColor(Color.Rgb(19, 32, 97));
            btnShowPass.SetBackgroundColor(Color.Rgb(19, 32, 97));
            
        }

        /// <summary>
        /// Установка светлой темы.
        /// </summary>
        private void SetWhiteThem()
        {
            t1.SetTextColor(Color.Black);
            t2.SetTextColor(Color.Black);
            t3.SetTextColor(Color.Black);
            edtLogin.SetTextColor(Color.Black);
            edtPass.SetTextColor(Color.Black);
            try
            {
                ((TextView)spinPass.GetChildAt(0)).SetTextColor(Color.Black);
            }
            catch 
            {
            }
                
            l.SetBackgroundColor(Color.White);
            tb.SetBackgroundColor(Color.Rgb(70, 130, 180));
            btnShowPass.SetBackgroundColor(Color.Rgb(70, 130, 180));
        }

        /// <summary>
        /// Настройки.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPropert_Click(object sender, System.EventArgs e)
        {
            Intent proper = new Intent(this, typeof(PropertiPage));
            proper.PutExtra("Log", controlerUser.user.Login);
            proper.PutExtra("Pass", Shifrator.DeShifrat(controlerUser.user.Passw));
            if(!string.IsNullOrWhiteSpace(controlerUser.user.E_mail))
                proper.PutExtra("Email", Shifrator.DeShifrat(controlerUser.user.E_mail));
            StartActivityForResult(proper, 3);
        }

        /// <summary>
        /// Редактирование пароля.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEditPass_Click(object sender, System.EventArgs e)
        {
            if(listPassw.Count >0)
            {
 
                Intent add = new Intent(this, typeof(PaswAddPage));
                add.PutExtra("Edt", true);
                add.PutExtra("Name", spinPass.SelectedItem.ToString());
                add.PutExtra("Log", edtLogin.Text);
                add.PutExtra("Pass", edtPass.Text);
                StartActivityForResult(add, 2);
            }
            else
            {
                Toast.MakeText(this, "Не выбран пароль для редактирования.", ToastLength.Short).Show();
            }

           
        }

        /// <summary>
        /// Добавление пароля.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddPass_Click(object sender, System.EventArgs e)
        {
            Intent add = new Intent(this, typeof(PaswAddPage));
            StartActivityForResult(add, 1);
        }

        /// <summary>
        /// Удаление пароля.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelPass_Click(object sender, System.EventArgs e)
        {
            if(spinPass.SelectedItemPosition > -1)
            {
                new Android.App.AlertDialog.Builder(this).
              SetTitle("Удаление пароля").
              SetMessage("Удалить пароль для " + spinPass.SelectedItem.ToString()).
              SetPositiveButton("Да", delegate {

                  controlerUser.DeletePassword(spinPass.SelectedItemPosition);
                  edtLogin.Text = "";
                  edtPass.Text = "";
                  SetPassword();

              }).
              SetNegativeButton("Нет", delegate { }).
              Show();
                
            }
            else
            {
                Toast.MakeText(this, "Выберите пароль.", ToastLength.Short).Show();
            }
        }

        /// <summary>
        /// Заполнение спинера паролями пользователя.
        /// </summary>
        private void SetPassword()
        {
            try
            {
                listPassw = controlerUser.GetPassw();
            }
            catch 
            {
               
                return;
            }
            
            var items = new List<string>();

            for (int i = 0; i < listPassw.Count; i++)
            {
                string item = listPassw[i].ToString().Split('#')[0];
                items.Add(item);
            }

            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, items);
            spinPass.Adapter = adapter;
        }

        /// <summary>
        /// Кнопка выход.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExit_Click(object sender, System.EventArgs e)
        {
            new Android.App.AlertDialog.Builder(this).
                SetTitle("Выход").
                SetMessage("Уже уходите?").
                SetIcon(Resource.Drawable.information_info_1565).
                SetPositiveButton("Да", delegate { Finish(); }).
                SetNegativeButton("Нет", delegate { }).
                Show();
        }

        /// <summary>
        /// Выбор пароля для просмотра.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpinPass_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            edtPass.InputType = Android.Text.InputTypes.TextVariationPassword | Android.Text.InputTypes.ClassText;
            if (StatesTheme.IsDark())
            {
                if (listPassw.Count != 0)
                    ((TextView)spinPass.GetChildAt(0)).SetTextColor(Color.White);
                edtPass.SetTextColor(Color.White);
            }  
            else
            {
                if (listPassw.Count != 0)
                    ((TextView)spinPass.GetChildAt(0)).SetTextColor(Color.Black);
                edtPass.SetTextColor(Color.Black);
            }

            
            btnShowPass.Text = "Показать пароль";

            int i = spinPass.SelectedItemPosition;

            edtLogin.Text = listPassw[i].ToString().Split('#')[1];
            edtPass.Text = listPassw[i].ToString().Split('#')[2];

        }

        /// <summary>
        /// Скрытие и показ пароля.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnShowPass_Click(object sender, System.EventArgs e)
        {
            if (StatesTheme.IsDark())
            {
                edtPass.SetTextColor(Color.White);
            }
            else
            {
                edtPass.SetTextColor(Color.Black);
            }

            if (btnShowPass.Text == "Показать пароль")
            {
                edtPass.InputType = Android.Text.InputTypes.ClassText | Android.Text.InputTypes.Null;
                btnShowPass.Text = "Скрыть пароль";
            }
            else
            {
                edtPass.InputType = Android.Text.InputTypes.TextVariationPassword | Android.Text.InputTypes.ClassText;
                
                btnShowPass.Text = "Показать пароль";
            }

            
        }

        /// <summary>
        /// При закрытии одной из форм.
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="resultCode"></param>
        /// <param name="data"></param>
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            //Закрылась форма регистрации.
            if(requestCode == 0)
            {
                if (resultCode == Result.Ok)
                {
                    //Регистрация
                    if(data.GetStringExtra("LogIn") == null)
                    {
                        string login = data.GetStringExtra("Rlogin");
                        string pass = data.GetStringExtra("Rpass");

                        new Android.App.AlertDialog.Builder(this).
                           SetTitle("Добро пожаловать").
                           SetMessage("Сверху находятся все необходимые кнопки\n" +
                           "-Добавить пароль(Плюс)\n" +
                           "-Удалить пароль(Корзина)\n" +
                           "-Редактировать пароль(Карандаш)\n" +
                           "-Настройки(Шестеренка)\n" +
                           "-Выход\n\n" +
                           "Рекомендация:\n" +
                           "Укажите свою почту во вкладке Настройки " +
                           "и тогда, если вы забудете пароль от аккаунты," +
                           "вы сможете его восстановить.").
                           SetIcon(Resource.Drawable.information_info_1565).
                           SetPositiveButton("Ок", delegate { }).
                           Show();

                        try
                        {
                            controlerUser = new ControlerUser(login, pass);
                        }
                        catch 
                        {
                            Toast.MakeText(this, "Не удалось создать учетную запись.", ToastLength.Short).Show();
                            Finish();
                        }
                    }
                    else//Вход
                    {
                       
                        controlerUser = new ControlerUser(data.GetStringExtra("LogIn"));
                    }

                    SetPassword();
                }
                else
                    Finish();

            }

            //Закрылась форма добавления пароля.
            if(requestCode == 1)
            {
                if(resultCode == Result.Ok)
                {
                    string name = data.GetStringExtra("Name");
                    string login = data.GetStringExtra("Login");
                    string pass = data.GetStringExtra("Pass");

                    controlerUser.AddPassw(name, login, pass);
                    SetPassword();

                   
                }
            }

            //Закрылась форма редактирования пароля.
            if(requestCode == 2)
            {
                if (resultCode == Result.Ok)
                {
                    string name = data.GetStringExtra("Name");
                    string login = data.GetStringExtra("Login");
                    string pass = data.GetStringExtra("Pass");

                    controlerUser.EditPassw(spinPass.SelectedItemPosition, name, login, pass);
                    SetPassword();
                }
            }

            //Закрылась форма с настройками.
            if(requestCode == 3)
            {
                if(resultCode == Result.Ok)
                {
                    string name = data.GetStringExtra("Login");
                    string pass = data.GetStringExtra("Pass");
                    string em = data.GetStringExtra("email");

                    controlerUser.EditUser(name, Shifrator.Shifrated(pass),Shifrator.Shifrated(em));
                }

                if (StatesTheme.IsDark())
                    SetDarkThem();
                else
                    SetWhiteThem();
            }
        }
    }
}