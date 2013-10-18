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
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            for (int i = 0; i > -7; i--) {
                var date = DateTime.Today.AddDays(i);
                var file = GetFilePath(date);
                if (!string.IsNullOrEmpty(file)) {
                    if (i == 0 || IsOpenPastHomework(date)) {
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
