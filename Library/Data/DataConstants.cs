namespace Library.Data
{
    public static class DataConstants
    {

        public const int BookTitleMinLength = 10;
        public const int BookTitleMaxLength = 50;

        public const int AuthorNameMinLength = 5;
        public const int AuthorNameMaxLength = 50;

        public const int BookDescriptionMinLength = 5;
        public const int BookDescriptionMaxLength = 5000;

        public const int CategoryNameMinLength = 5;
        public const int CategoryNameMaxLength = 50;

        public const double RatingMinValue = 0.00;
        public const double RatingMaxValue = 10.00;


        public const string StringLengthErrorMessage = "The field {0} must be between {2} and {1} characters long!";

    }
}
