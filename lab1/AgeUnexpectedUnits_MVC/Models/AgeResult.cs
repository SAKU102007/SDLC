namespace AgeUnexpectedUnits_MVC.Models;

public sealed record AgeResult(
    int Years,
    long TotalMonths,
    long TotalDays,
    long Minutes,
    double CoffeeCups,
    double SeriesCount,
    double SocialMediaKilometers);
