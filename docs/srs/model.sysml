package LabReportValidator {

    // ─── Определения элементов ───

    part def WebServer;
    part def Browser;
    part def DatabaseServer;

    // ─── Роли пользователей ───

    actor def Student {
        doc /* Студент: загружает отчёты и просматривает результаты проверки */
    }

    actor def Teacher {
        doc /* Преподаватель: создаёт лабораторные, просматривает отчёты и статистику */
    }

    actor def Administrator {
        doc /* Администратор: управляет предметами, шаблонами, группами и пользователями */
    }

    // ─── Основные сущности ───

    item def Report {
        attribute fileName : String;
        attribute fileFormat : String;      // PDF или DOCX
        attribute fileSizeBytes : Integer;
        attribute uploadDate : String;
        attribute status : String;          // Принят, Требует доработки, Подозрение на плагиат, Просрочен
        attribute version : Integer;
    }

    item def LabWork {
        attribute number : Integer;
        attribute title : String;
        attribute subject : String;
        attribute deadline : String;
    }

    item def ValidationResult {
        attribute sectionsFound : Integer;
        attribute sectionsMissing : Integer;
        attribute remarks : String;
    }

    item def PlagiarismResult {
        attribute matchPercentage : Integer;
        attribute threshold : Integer;      // по умолчанию 50
        attribute isSuspicious : Boolean;
    }

    item def Subject {
        attribute name : String;
    }

    item def CheckTemplate {
        attribute requiredSections : String; // Титульный лист, Цель, Ход работы, Выводы, Приложение
    }

    item def UserGroup {
        attribute groupName : String;
    }

    // ─── Структура системы ───

    part labReportSystem : WebServer {
        part authModule {
            doc /* Модуль аутентификации и авторизации */
        }
        part uploadModule {
            doc /* Модуль загрузки файлов отчётов */
        }
        part validationModule {
            doc /* Модуль валидации по шаблону */
        }
        part plagiarismModule {
            doc /* Модуль проверки на плагиат */
        }
        part teacherPanel {
            doc /* Панель преподавателя */
        }
        part adminPanel {
            doc /* Панель администратора */
        }
    }

    part database : DatabaseServer {
        doc /* PostgreSQL: пользователи, предметы, лабораторные, отчёты, результаты проверок */
    }

    part browser : Browser {
        doc /* Браузер пользователя: Chrome, Firefox, Edge */
    }

    // ─── Варианты использования ───

    use case def Register {
        doc /* Пользователь регистрируется по электронной почте и паролю */
        subject = labReportSystem;
        actor :>> Student;
    }

    use case def Login {
        doc /* Пользователь входит в систему */
        subject = labReportSystem;
        actor :>> Student;
        actor :>> Teacher;
        actor :>> Administrator;
    }

    use case def UploadReport {
        doc /* Студент загружает файл отчёта с привязкой к лабораторной работе */
        subject = labReportSystem;
        actor :>> Student;
        include ValidateTemplate;
        include CheckPlagiarism;
    }

    use case def ValidateTemplate {
        doc /* Система проверяет наличие обязательных разделов */
        subject = labReportSystem;
    }

    use case def CheckPlagiarism {
        doc /* Система сравнивает текст с ранее загруженными отчётами */
        subject = labReportSystem;
    }

    use case def ViewResults {
        doc /* Студент просматривает результат проверки */
        subject = labReportSystem;
        actor :>> Student;
    }

    use case def ReuploadReport {
        doc /* Студент повторно загружает исправленный отчёт */
        subject = labReportSystem;
        actor :>> Student;
        include ValidateTemplate;
        include CheckPlagiarism;
    }

    use case def CreateLabWork {
        doc /* Преподаватель создаёт лабораторную работу */
        subject = labReportSystem;
        actor :>> Teacher;
    }

    use case def ViewReports {
        doc /* Преподаватель просматривает загруженные отчёты */
        subject = labReportSystem;
        actor :>> Teacher;
    }

    use case def ViewStatistics {
        doc /* Преподаватель просматривает статистику по лабораторной */
        subject = labReportSystem;
        actor :>> Teacher;
    }

    use case def ManageSubjects {
        doc /* Администратор управляет списком предметов */
        subject = labReportSystem;
        actor :>> Administrator;
    }

    use case def ManageTemplates {
        doc /* Администратор редактирует шаблоны проверки */
        subject = labReportSystem;
        actor :>> Administrator;
    }

    use case def ManageUsers {
        doc /* Администратор управляет пользователями и группами */
        subject = labReportSystem;
        actor :>> Administrator;
    }

    // ─── Требования ───

    requirement def <'REQ-FUNC-001'> AuthRequirement {
        doc /* Система должна позволять зарегистрироваться по электронной почте и паролю, входить и выходить, различая роли студента, преподавателя и администратора. */
    }

    requirement def <'REQ-FUNC-002'> UploadRequirement {
        doc /* Система должна позволять студенту загружать файл отчёта (PDF, DOCX) с привязкой к конкретной лабораторной работе. */
    }

    requirement def <'REQ-FUNC-003'> ReuploadRequirement {
        doc /* Система должна позволять студенту повторно загрузить исправленный отчёт с сохранением предыдущей версии в истории. */
    }

    requirement def <'REQ-FUNC-004'> FileSizeLimitRequirement {
        doc /* Система не должна принимать файлы размером более 10 МБ. */
    }

    requirement def <'REQ-FUNC-005'> TemplateValidationRequirement {
        doc /* Система должна автоматически проверять загруженный отчёт на наличие обязательных разделов. */
    }

    requirement def <'REQ-FUNC-006'> DisplayResultsRequirement {
        doc /* Система должна отображать студенту статус и замечания по шаблону. */
    }

    requirement def <'REQ-FUNC-007'> PlagiarismCheckRequirement {
        doc /* Система должна выполнять базовую проверку на плагиат путём сравнения текста с ранее загруженными отчётами. */
    }

    requirement def <'REQ-FUNC-008'> PlagiarismThresholdRequirement {
        doc /* Система должна помечать отчёт как «Подозрение на плагиат» при совпадении выше 50 %. */
    }

    requirement def <'REQ-FUNC-009'> CreateLabRequirement {
        doc /* Преподаватель должен иметь возможность создавать лабораторные работы. */
    }

    requirement def <'REQ-FUNC-010'> ViewReportsRequirement {
        doc /* Преподаватель должен видеть загруженные отчёты с фильтрацией по группе и статусу. */
    }

    requirement def <'REQ-SEC-001'> PasswordHashingRequirement {
        doc /* Система не должна хранить пароли в открытом виде. */
    }

    requirement def <'REQ-SEC-002'> RoleAccessRequirement {
        doc /* Система должна разграничивать доступ по ролям. */
    }

    requirement def <'REQ-REL-001'> DataPersistenceRequirement {
        doc /* Система должна сохранять данные между перезапусками. */
    }
}
