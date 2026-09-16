namespace AgeUnexpectedUnits_MVC.Views;

public interface IDateInputView
{
    event EventHandler? InputRequested;

    string DateText { get; }

    void SetDateText(string value);
    void ShowValidationError(string message);
    void CloseDialog();
}
