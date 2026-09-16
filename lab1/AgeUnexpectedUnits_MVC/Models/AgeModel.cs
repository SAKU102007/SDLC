using System.Text.Json;

namespace AgeUnexpectedUnits_MVC.Models;

/// <summary>
/// Active MVC model. Contains business rules, persistent state and notifies
/// subscribers when its state changes.
/// </summary>
public sealed class AgeModel
{
    private const double CoffeeCupsPerDay = 3.0;
    private const double SeriesMinutes = 45.0;
    private const double SocialMediaKmPer30Minutes = 1.0;

    private readonly string _storagePath;
    private DateTime? _birthDate;

    public event EventHandler? StateChanged;

    public AgeModel()
    {
        _storagePath = Path.Combine(
            Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName,
            "settings.json");

        Load();
    }

    public bool HasData => _birthDate.HasValue;

    public DateTime? BirthDate => _birthDate;

    public AgeResult? CurrentResult => _birthDate is null ? null : Calculate(_birthDate.Value, DateTime.Now);

    public void SetBirthDate(DateTime birthDate)
    {
        birthDate = birthDate.Date;

        if (birthDate > DateTime.Today)
            throw new ArgumentException("Дата рождения не может быть в будущем.");

        if (birthDate.Year < 1900)
            throw new ArgumentException("Год рождения должен быть не меньше 1900.");

        _birthDate = birthDate;
        Save();
        OnStateChanged();
    }

    public AgeResult CalculateCurrent()
    {
        if (_birthDate is null)
            throw new InvalidOperationException("Дата рождения ещё не введена.");

        return Calculate(_birthDate.Value, DateTime.Now);
    }

    private static AgeResult Calculate(DateTime birthDate, DateTime now)
    {
        if (birthDate > now)
            throw new ArgumentException("Дата рождения не может быть в будущем.");

        var dateNow = now.Date;

        var years = dateNow.Year - birthDate.Year;
        if (birthDate.AddYears(years) > dateNow)
            years--;

        var afterYears = birthDate.AddYears(years);
        var months = dateNow.Month - afterYears.Month;
        if (months < 0)
            months += 12;
        if (afterYears.AddMonths(months) > dateNow)
            months--;

        var afterMonths = afterYears.AddMonths(months);
        var days = (dateNow - afterMonths).Days;

        var totalMinutes = Math.Max(0, (long)(now - birthDate).TotalMinutes);
        var totalDays = totalMinutes / (24L * 60L);
        var totalMonths = years * 12L + months;

        var coffeeCups = totalDays * CoffeeCupsPerDay;
        var seriesCount = totalMinutes / SeriesMinutes;
        var socialMediaKilometers = totalMinutes / 30.0 * SocialMediaKmPer30Minutes;

        return new AgeResult(
            years,
            totalMonths,
            totalDays,
            totalMinutes,
            coffeeCups,
            seriesCount,
            socialMediaKilometers);
    }

    private void Save()
    {
        var state = new StoredState
        {
            BirthDate = _birthDate
        };

        var json = JsonSerializer.Serialize(state, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(_storagePath, json);
    }

    private void Load()
    {
        try
        {
            if (!File.Exists(_storagePath))
                return;

            var json = File.ReadAllText(_storagePath);
            var state = JsonSerializer.Deserialize<StoredState>(json);
            var savedDate = state?.BirthDate?.Date;

            if (savedDate is not null && savedDate <= DateTime.Today && savedDate.Value.Year >= 1900)
                _birthDate = savedDate;
        }
        catch
        {
            // Broken preferences must not prevent the application from starting.
            _birthDate = null;
        }
    }

    private void OnStateChanged() => StateChanged?.Invoke(this, EventArgs.Empty);

    private sealed class StoredState
    {
        public DateTime? BirthDate { get; set; }
    }
}
