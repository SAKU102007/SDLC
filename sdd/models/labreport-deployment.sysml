package LabReportValidatorDeployment {
    part def Browser;
    part def ApplicationServer;
    part def DatabaseServer;

    part systemDeployment {
        part client : Browser;
        part appServer : ApplicationServer;
        part dbServer : DatabaseServer;

        connect client to appServer; // HTTPS
        connect appServer to dbServer; // TCP / Npgsql
    }
}
