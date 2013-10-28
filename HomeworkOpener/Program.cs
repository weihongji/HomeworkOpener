using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Homework
{
    static class Program
    {
        private static bool JumpOverWeekend = true; // If 'today' is in weekend, system will not show notification while homework is found in a day of the same weekend or in the last Friday.

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            var today = DateTime.Today;
            if (string.IsNullOrEmpty(GetFilePath(today))) {
                if (DateTime.Now.Hour < 18) {
                    today = DateTime.Today.AddDays(-1);
                }
            }
            var isInWeekend = today.DayOfWeek == DayOfWeek.Saturday || today.DayOfWeek == DayOfWeek.Sunday;
            for (int i = 0; i > -7; i--) {
                var date = today.AddDays(i);
                if (JumpOverWeekend && isInWeekend && i != 0) {
                    isInWeekend = date.AddDays(1).DayOfWeek == DayOfWeek.Saturday || date.AddDays(1).DayOfWeek == DayOfWeek.Sunday;
                }
                var file = GetFilePath(date);
                if (!string.IsNullOrEmpty(file)) {
                    if (i == 0 || (JumpOverWeekend && isInWeekend) || IsOpenPastHomework(date)) {
                        OpenHomework(file);
                    }
                    return;
                }
            }
            MessageBox.Show("没有找到最近7天以内的作业！", "没找到作业照片", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        static bool IsOpenPastHomework(DateTime date) {
            return MessageBox.Show(string.Format("没有找到今天的作业，是否打开{0}日的作业？", date.ToString("yyyy-M-d")),
                    "没找到今天作业的照片",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                ) == DialogResult.Yes;
        }

        static string GetFilePath(DateTime date) {
            var dateString = date.ToString("yyyy-M-d");
            string path = string.Format(@"C:\Users\Me\Pictures\{0}", dateString);
            if (Directory.Exists(path)) {
                return Directory.GetFiles(path).FirstOrDefault();
            }
            return null;
        }

        static void OpenHomework(string file) {
            Process p = new Process();
            p.StartInfo.FileName = "explorer";
            p.StartInfo.Arguments = file;
            p.Start();
            p.Close();
        }
    }
}
