﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NpcGenDataEditorByLuka
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            UIExceptionHandlerWinForms.UIException.Start("SmtpServer",
                26,
                "Password",
                "User",
                "bugreportaccountlb",
                "user@gmail.com",
                "Exception"
                );
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

    }
}
