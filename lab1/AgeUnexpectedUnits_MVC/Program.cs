using AgeUnexpectedUnits_MVC.Controllers;
using AgeUnexpectedUnits_MVC.Models;
using AgeUnexpectedUnits_MVC.Views;

namespace AgeUnexpectedUnits_MVC;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        var model = new AgeModel();
        var view = new MainForm();
        var controller = new AgeController(model, view);

        Application.Run(view);
    }
}
