package LabReportValidatorInformation {
    enum def StatusCategory {
        enum Uploaded;
        enum Validating;
        enum StructureRejected;
        enum PlagiarismRejected;
        enum UnderReview;
        enum RevisionRequired;
        enum Accepted;
        enum Graded;
    }

    part def LabWork {
        attribute number : Natural;
        attribute title : String;
        attribute deadline : String;
    }

    part def Report {
        attribute fileName : String;
        attribute format : String;
        attribute version : Natural;
        attribute uploadDate : String;
    }

    part def ValidationResult {
        attribute status : StatusCategory;
        attribute sectionsFound : String;
        attribute remarks : String;
    }

    part def PlagiarismMatch {
        attribute matchScore : Natural;
        attribute threshold : Natural; // 30%
        attribute isReturned : Boolean;
    }

    part def User {
        attribute email : String;
        attribute role : String;
    }

    part def Subject {
        attribute title : String;
        attribute course : Natural;
    }

    part def Notification {
        attribute recipientId : String;
        attribute message : String;
        attribute createdAt : String;
    }
}
