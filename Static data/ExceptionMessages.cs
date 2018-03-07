using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft
{
    public static class ExceptionMessages
    {
        public const string UnauthorizedAccessExceptionMessage =
            "The folder/file you are trying to get access needs a higher level of rights than you currently have.";
        public const string UnableToGoToHigherInPartitionHierrarchy =
            "You can't go so high in the hierrarchy because there are no more folders up";
        public const string UnableToParseNumber =
            "The sequence you've written is not a valid number.";
        public const string InvalidTakeCommand =
            "The take command expected does not match the format wanted!";
        public const string NotEnrolledInCourse = "Student must be enrolled in a course before you set his mark.";
    }
}
