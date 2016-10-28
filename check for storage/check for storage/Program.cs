using System;
using System.Threading;
using System.Windows.Forms;

namespace check_for_storage
{
    class Program
    {
        private static String TRUECRYPT_FILE = "link to disk file";
        private static String TRUECRYPT_KEY_FILE = "link to disk keyfile";
        private static String TRUECRYPT_PASSWORD = "password";
        private static String TRUECRYPT_DISK = "disk";

        private static String CHECK_FILE = "link to keyfile";
        private static String CHECK_KEY = "key in keyfile";
        private static System.Threading.Timer timer;

        static void Main(string[] args)
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
                mount("/C del link to keyfile");

                MessageBox.Show("Код ошибки: 2", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Функция проверки файла-ключа и его содержимого
        private static void checkAndMount()
        {
            while (true)
            {
                if (System.IO.File.Exists(CHECK_FILE))
                {
                    using (System.IO.StreamReader sr = System.IO.File.OpenText(CHECK_FILE))
                    {
                        String checkText = "";

                        while ((checkText = sr.ReadLine()) != null)
                        {
                            if (checkText.Contains(CHECK_KEY))
                            {
                                // Монтирование
                                mount("/C truecrypt /v " + TRUECRYPT_FILE + " /k " + TRUECRYPT_KEY_FILE + " /h n /c n /l " + TRUECRYPT_DISK + " /p " + TRUECRYPT_PASSWORD + " /a /q");

                                timer = new System.Threading.Timer(timerTick, null, 0, 5000);

                                Thread.Sleep(5000);

                                break;
                            }
                            else
                            {
                                MessageBox.Show("Код ошибки: 3", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }

                        }
                    }
                    System.IO.File.Delete(@"link to keyfile");
                    break;
                }
                else
                {
                    Thread.Sleep(300000);
                }
            }
        }

        // Функция запуска командной строки
        private static void mount(String cmd)
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
        private static void timerTick(object o)
        {
            mount("/C link to share script");
            Thread.Sleep(3000);

            GC.Collect();
        }
    }
}
