using FastFoodPOS.Forms;

namespace FastFoodPOS;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        while (true)
        {
            using (var login = new LoginForm())
            {
                if (login.ShowDialog() != DialogResult.OK) break;
            }

            using var main = new MainForm();
            Application.Run(main);
            if (!main.LoggedOut) break;
        }
    }
}
