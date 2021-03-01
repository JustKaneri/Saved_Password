using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using Хранилище_паролей.Resources.BL;

namespace Хранилище_паролей
{
    [Activity(Label = "PaswAddPage")]
    public class PaswAddPage : Activity
    {
        private Button btnSave;
        private Button btnCancel;
        private EditText edtName;
        private EditText edtLogin;
        private EditText edtPass;
        private TextView h;

        private LinearLayout l;
        private TextView t1;
        private TextView t2;
        private TextView t3;

        private Button btnShow;

        private bool IsEdit;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.PageAddPasw);
            this.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            // Create your application here

            btnSave = FindViewById<Button>(Resource.Id.BtnSave);
            btnCancel = FindViewById<Button>(Resource.Id.BtnCansel);
            btnShow = FindViewById<Button>(Resource.Id.BtnShowPassAddEdtit);

            edtName = FindViewById<EditText>(Resource.Id.EdtNameAdd);
            edtLogin = FindViewById<EditText>(Resource.Id.EdtLoginAdd);
            edtPass = FindViewById<EditText>(Resource.Id.EdtPassAdd);

            h = FindViewById<TextView>(Resource.Id.TxtViewH);

            t1 = FindViewById<TextView>(Resource.Id.textView0);
            t2 = FindViewById<TextView>(Resource.Id.textView00);
            t3 = FindViewById<TextView>(Resource.Id.textView000);
            l = FindViewById<LinearLayout>(Resource.Id.LAdded);

            if (StatesTheme.IsDark())
                SetDarkThem();

            IsEdit = Intent.GetBooleanExtra("Edt", false);

            if (IsEdit)
            {
                h.Text = "Редактирование пароля:";
                edtName.Text = Intent.GetStringExtra("Name");
                edtLogin.Text = Intent.GetStringExtra("Log");
                edtPass.Text = Intent.GetStringExtra("Pass");
            }
           
            btnCancel.Click += BtnCancel_Click;
            btnSave.Click += BtnSave_Click;
            btnShow.Click += BtnShow_Click;
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
           //Показать пароль.
           if(btnShow.Tag.ToString() == "1")
           {
                btnShow.Tag = "2";
                edtPass.InputType = Android.Text.InputTypes.ClassText | Android.Text.InputTypes.Null;
                btnShow.SetBackgroundResource(Resource.Drawable.NotShow1);
           }
           else//Скрыть пароль.
           {
                btnShow.SetBackgroundResource(Resource.Drawable.Show2);
                btnShow.Tag = "1";
                edtPass.InputType = Android.Text.InputTypes.ClassText | Android.Text.InputTypes.TextVariationPassword;
           }
        }

        private void SetDarkThem()
        {
            t1.SetTextColor(Color.White);
            t2.SetTextColor(Color.White);
            t3.SetTextColor(Color.White);
            h.SetTextColor(Color.White);
            l.SetBackgroundColor(Color.Rgb(53, 55, 61));
            btnSave.SetBackgroundColor(Color.Rgb(19, 32, 97));
            btnCancel.SetBackgroundColor(Color.Rgb(19, 32, 97));
            edtLogin.SetTextColor(Color.White);
            edtPass.SetTextColor(Color.White);
            edtName.SetTextColor(Color.White);
        }

        /// <summary>
        /// Сохранить.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
           if(string.IsNullOrWhiteSpace(edtName.Text) || string.IsNullOrWhiteSpace(edtLogin.Text) || string.IsNullOrWhiteSpace(edtPass.Text))
           {
                Toast.MakeText(this, "Вы не заполнили все поля.", ToastLength.Short).Show();
                return;
           }

            Intent main = new Intent(this, typeof(MainActivity));
            main.PutExtra("Name", edtName.Text);
            main.PutExtra("Login", edtLogin.Text);
            main.PutExtra("Pass", edtPass.Text);
            SetResult(Result.Ok, main);

            Finish();
        }

        /// <summary>
        /// Отмена.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Finish();
        }
    }
}