package LabReportValidatorStructure {
    part def WebUI;
    part def AuthService;
    part def ReportService;
    part def ValidationService;
    part def PlagiarismService;
    part def NotificationService;
    part def PostgreSQL;

    part app {
        part ui : WebUI;
        part auth : AuthService;
        part report : ReportService;
        part validation : ValidationService;
        part plagiarism : PlagiarismService;
        part notification : NotificationService;
        part db : PostgreSQL;

        connect ui to auth;
        connect ui to report;
        connect ui to validation;
        connect ui to plagiarism;
        connect ui to notification;
        connect plagiarism to notification;

        connect auth to db;
        connect report to db;
        connect validation to db;
        connect plagiarism to db;
        connect notification to db;
    }
}
