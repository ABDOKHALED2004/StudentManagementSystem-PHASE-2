using System;
using System.Windows.Forms;
using StudentManagementSystem.Data;
using StudentManagementSystem.Forms;

namespace StudentManagementSystem
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Keep this project self-contained (no ApplicationConfiguration class)
            // to avoid duplicate-type issues when users accidentally merge projects.
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Replace InMemoryStudentRepository with your friend's SQL repository later.
            // Start empty by default.
            IStudentRepository repo = new InMemoryStudentRepository(seedDemoData: false);

            Application.Run(new MainForm(repo));
        }
    }
}
