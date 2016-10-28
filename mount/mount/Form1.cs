using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace mount
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private static String PASSWORD = "password";
        private static String TRUECRYPT_FILE = "link to file";
        private static String TRUECRYPT_KEY_FILE = "link to keyfile";
        private static String TRUECRYPT_PASSWORD = "password";
        private static String TRUECRYPT_DISK = "disk";

        private void buttonMount_Click(object sender, EventArgs e)
        {
            // Размонтирование дисков
            mount("/C truecrypt /q /d /f");

            Thread.Sleep(5000);

            // Проверка на наличие файлов по ссылкам
            if (System.IO.File.Exists(TRUECRYPT_FILE) && System.IO.File.Exists(TRUECRYPT_KEY_FILE))
            {
                checkAndMount();
            }
            else
            {
                MessageBox.Show("Код ошибки: 2", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
                       
        }

        // Функция проверки пароля, монтирования диска и создание файлов-ключей на остальных серверах
        private void checkAndMount()
        {
            if (textBoxPassword.Text.Equals(PASSWORD))
            {
                mount("/C truecrypt /v " + TRUECRYPT_FILE + " /k " + TRUECRYPT_KEY_FILE + " /h n /c n /l " + TRUECRYPT_DISK + " /p " + TRUECRYPT_PASSWORD + " /a /q");
                mount(@"/K Echo key > add keyfile to server");

                timer.Tick += new EventHandler(timer_Tick);
                timer.Interval = 5000;
                timer.Enabled = true;
            }
            else
            {
                MessageBox.Show("Код ошибки: 1", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Функция запуска командной строки
        private void mount(String cmd)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = cmd;
            process.StartInfo = startInfo;
            process.Start();
        }

        // Функция запуска скрипта шары дисков
        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();

            mount("/C link to share script");

            MessageBox.Show("Диски подключенны", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
