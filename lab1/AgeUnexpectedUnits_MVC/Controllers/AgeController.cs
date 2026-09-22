using System.Globalization;
using AgeUnexpectedUnits_MVC.Models;
using AgeUnexpectedUnits_MVC.Views;

namespace AgeUnexpectedUnits_MVC.Controllers;

/// <summary>
/// MVC controller: translates user actions into model operations and creates views.
/// It does not contain the age calculation algorithm.
/// </summary>
public sealed class AgeController
{
    private readonly AgeModel _model;
    private readonly MainForm _view;

    public AgeController(AgeModel model, MainForm view)
    {
        _model = model;
        _view = view;

        _view.InputRequested += OnInputRequested;
        _model.StateChanged += OnModelStateChanged;

        ShowCurrentModelState();
    }

    private void OnInputRequested(object? sender, EventArgs e)
    {
        using var dialog = _view.CreateDateInputDialog();

        if (_model.BirthDate is not null)
            dialog.SetDateText(_model.BirthDate.Value.ToString("dd.MM.yyyy"));

        dialog.InputRequested += (_, _) => TryApplyDialog(dialog);
        dialog.ShowDialog(_view);
    }

    private void TryApplyDialog(DateInputForm dialog)
    {
        if (!TryParseDate(dialog.DateText, out var birthDate, out var error))
        {
            dialog.ShowValidationError(error);
            return;
        }

        try
        {
            _model.SetBirthDate(birthDate);
            dialog.CloseDialog();
        }
        catch (ArgumentException ex)
        {
            dialog.ShowValidationError(ex.Message);
        }
    }

    private void OnModelStateChanged(object? sender, EventArgs e)
    {
        // Active model pushes its new state to the controller, which refreshes the view.
        ShowCurrentModelState();
    }

    private void ShowCurrentModelState()
    {
        if (!_model.HasData || _model.BirthDate is null)
        {
            _view.ClearResult();
            _view.SetStatus("Дата рождения не введена.");
            return;
        }

        var result = _model.CalculateCurrent();
        _view.SetResult(_model.BirthDate.Value, result);
    }

    private static bool TryParseDate(string text, out DateTime birthDate, out string error)
    {
        birthDate = default;
        error = string.Empty;

        var normalized = text.Trim();
        var formats = new[] { "dd.MM.yyyy", "d.M.yyyy", "dd/MM/yyyy", "d/M/yyyy" };

        if (!DateTime.TryParseExact(
                normalized,
                formats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out birthDate))
        {
            error = "Некорректная дата. Используйте формат ДД.ММ.ГГГГ, например 15.08.2007.";
            return false;
        }

        birthDate = birthDate.Date;

        if (birthDate > DateTime.Today)
        {
            error = "Дата рождения не может быть в будущем.";
            return false;
        }

        if (birthDate.Year < 1900)
        {
            error = "Год рождения должен быть не меньше 1900.";
            return false;
        }

        return true;
    }
}
