using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Goryokaku
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // 二重起動防止
            System.Threading.Mutex mutex
                = new System.Threading.Mutex(false, Application.ProductName);

            if (mutex.WaitOne(0, false) == false)
            {
                return;
            }
 
            GC.KeepAlive(mutex);
            mutex.Close();

            // メイン
            if (args.Length > 0)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                if (args[0].ToLower().Trim().Substring(0, 2) == "/s") //show
                {
                    // スクリーンセーバーを表示
                    ShowScreenSaver(); //
                    Application.Run();
                }
                else if (args[0].ToLower().Trim().Substring(0, 2) == "/p") //preview
                {
                    // プレビュー画面を表示
                    Application.Run(new Form1(new IntPtr(long.Parse(args[1]))));
                }
                else if (args[0].ToLower().Trim().Substring(0, 2) == "/c") //configure
                {
                    OptionMenu();
                }
            }
            else
            {
                // 引数なしの場合。
                OptionMenu();
            }


        }

        static void OptionMenu()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new OptionForm());
            MessageBox.Show("オプションなし"
           + "\nこのスクリーンセーバーには、設定できるオプションはありません。",
           "Goryokaku ScreenSaver",
           MessageBoxButtons.OK,
           MessageBoxIcon.Information);
        }

        static void ShowScreenSaver()
        {
            foreach (Screen screen in Screen.AllScreens)
            {
                Form1 screensaver = new Form1(screen.Bounds);
                screensaver.Show();
            }
        }
    }
}
