using System.ComponentModel;
using System.Drawing;
using AgeUnexpectedUnits_MVC.Models;

namespace AgeUnexpectedUnits_MVC.Views;

public sealed class MainForm : Form
{
    private readonly Label _birthDateValue;
    private readonly Label _yearsValue;
    private readonly Label _monthsValue;
    private readonly Label _daysValue;
    private readonly Label _minutesValue;
    private readonly Label _coffeeValue;
    private readonly Label _seriesValue;
    private readonly Label _socialValue;
    private readonly Label _statusValue;
    private readonly Button _inputButton;

    public event EventHandler? InputRequested;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DateTime? LastDate { get; set; }

    public MainForm()
    {
        Text = "Возраст в неожиданных единицах — MVC";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(720, 560);
        ClientSize = new Size(720, 560);

        var titleLabel = new Label
        {
            Text = "ВОЗРАСТ В НЕОЖИДАННЫХ ЕДИНИЦАХ",
            AutoSize = true,
            Location = new Point(30, 25),
            Font = new Font("Segoe UI", 16, FontStyle.Bold)
        };

        var descriptionLabel = new Label
        {
            Text = "Дата вводится вручную, без стандартного элемента DateTimePicker.",
            AutoSize = true,
            Location = new Point(30, 62),
            ForeColor = Color.DimGray
        };

        _inputButton = new Button
        {
            Text = "Ввести данные",
            Location = new Point(30, 95),
            Width = 150,
            Height = 38
        };

        _inputButton.Click += (_, _) =>
            InputRequested?.Invoke(this, EventArgs.Empty);

        var group = new GroupBox
        {
            Text = "Результат",
            Location = new Point(30, 155),
            Size = new Size(650, 310)
        };

        _birthDateValue = CreateValueLabel(205, 30, group);
        _yearsValue = CreateValueLabel(205, 65, group);
        _monthsValue = CreateValueLabel(205, 100, group);
        _daysValue = CreateValueLabel(205, 135, group);
        _minutesValue = CreateValueLabel(205, 170, group);
        _coffeeValue = CreateValueLabel(205, 205, group);
        _seriesValue = CreateValueLabel(205, 240, group);
        _socialValue = CreateValueLabel(205, 275, group);

        AddCaption(group, "Дата рождения:", 25, 30);
        AddCaption(group, "Полных лет:", 25, 65);
        AddCaption(group, "Всего месяцев:", 25, 100);
        AddCaption(group, "Всего дней:", 25, 135);
        AddCaption(group, "Всего минут:", 25, 170);
        AddCaption(group, "Чашек кофе:", 25, 205);
        AddCaption(group, "Серий по 45 минут:", 25, 240);
        AddCaption(group, "Километров ленты:", 25, 275);

        _statusValue = new Label
        {
            Text = "Дата рождения не введена.",
            AutoSize = false,
            Location = new Point(30, 490),
            Size = new Size(650, 45),
            ForeColor = Color.DimGray
        };

        Controls.AddRange(
        [
            titleLabel,
            descriptionLabel,
            _inputButton,
            group,
            _statusValue
        ]);

        ClearResult();
    }

    public void SetResult(DateTime birthDate, AgeResult result)
    {
        _birthDateValue.Text = birthDate.ToString("dd.MM.yyyy");
        _yearsValue.Text = result.Years.ToString();
        _monthsValue.Text = result.TotalMonths.ToString("N0");
        _daysValue.Text = result.TotalDays.ToString("N0");
        _minutesValue.Text = result.Minutes.ToString("N0");
        _coffeeValue.Text = result.CoffeeCups.ToString("N1");
        _seriesValue.Text = result.SeriesCount.ToString("N1");
        _socialValue.Text = result.SocialMediaKilometers.ToString("N1");

        _statusValue.Text =
            "Расчёт выполнен моделью. Активная модель уведомила View об изменении состояния.";
    }

    public void ClearResult()
    {
        _birthDateValue.Text = "—";
        _yearsValue.Text = "—";
        _monthsValue.Text = "—";
        _daysValue.Text = "—";
        _minutesValue.Text = "—";
        _coffeeValue.Text = "—";
        _seriesValue.Text = "—";
        _socialValue.Text = "—";
    }

    public void SetStatus(string text)
    {
        _statusValue.Text = text;
    }

    public DateInputForm CreateDateInputDialog()
    {
        var dialog = new DateInputForm();
        return dialog;
    }

    private static Label CreateValueLabel(int x, int y, Control parent)
    {
        var label = new Label
        {
            AutoSize = true,
            Location = new Point(x, y),
            Font = new Font("Segoe UI", 10, FontStyle.Bold)
        };

        parent.Controls.Add(label);

        return label;
    }

    private static void AddCaption(Control parent, string text, int x, int y)
    {
        parent.Controls.Add(new Label
        {
            Text = text,
            AutoSize = true,
            Location = new Point(x, y)
        });
    }
}
