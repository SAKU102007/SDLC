package LabReportValidatorValidation {
    action def UploadReport;
    action def ValidateStructure;
    action def CheckGlobalPlagiarism;
    action def EvaluateThreshold;
    action def NotifyTeacher;
    action def RouteToReview;

    action processPipeline {
        action step1 : UploadReport;
        action step2 : ValidateStructure;
        action step3 : CheckGlobalPlagiarism;
        action step4 : EvaluateThreshold;
        action step5 : NotifyTeacher;
        action step6 : RouteToReview;

        flow from step1 to step2;
        flow from step2 to step3;
        flow from step3 to step4;
        flow from step4 to step5;
        flow from step4 to step6;
    }
}
