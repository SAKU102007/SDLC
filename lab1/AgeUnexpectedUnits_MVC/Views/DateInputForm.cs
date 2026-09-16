using System.Drawing;

namespace AgeUnexpectedUnits_MVC.Views;

public sealed class DateInputForm : Form, IDateInputView
{
    private readonly TextBox _dateTextBox;
    private readonly Button _okButton;
    private readonly Button _cancelButton;
    private readonly Label _hintLabel;

    public event EventHandler? InputRequested;

    public string DateText => _dateTextBox.Text.Trim();

    public DateInputForm()
    {
        Text = "Ввод даты рождения";
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ClientSize = new Size(430, 190);

        var titleLabel = new Label
        {
            Text = "Введите дату рождения",
            AutoSize = true,
            Location = new Point(25, 22),
            Font = new Font("Segoe UI", 12, FontStyle.Bold)
        };

        _hintLabel = new Label
        {
            Text = "Формат: ДД.ММ.ГГГГ",
            AutoSize = true,
            Location = new Point(25, 58)
        };

        _dateTextBox = new TextBox
        {
            Location = new Point(25, 83),
            Width = 250,
            MaxLength = 10,
            PlaceholderText = "например, 15.08.2007"
        };

        _okButton = new Button
        {
            Text = "Рассчитать",
            Location = new Point(25, 125),
            Width = 115
        };

        _cancelButton = new Button
        {
            Text = "Отмена",
            Location = new Point(150, 125),
            Width = 95,
            DialogResult = DialogResult.Cancel
        };

        _okButton.Click += (_, _) => InputRequested?.Invoke(this, EventArgs.Empty);
        AcceptButton = _okButton;
        CancelButton = _cancelButton;

        Controls.AddRange([
            titleLabel,
            _hintLabel,
            _dateTextBox,
            _okButton,
            _cancelButton
        ]);
    }

    public void SetDateText(string value)
    {
        _dateTextBox.Text = value;
        _dateTextBox.SelectionStart = _dateTextBox.TextLength;
    }

    public void ShowValidationError(string message)
    {
        MessageBox.Show(this, message, "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        _dateTextBox.Focus();
        _dateTextBox.SelectAll();
    }

    public void CloseDialog() => DialogResult = DialogResult.OK;
}
