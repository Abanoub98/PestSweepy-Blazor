namespace Bookify.Web.Core.Consts
{
    public static class Errors
    {

        public const string MaxLengthError = "{0} Can Not Contain More Than {1} letter!";
        public const string MaxMinLengthError = "The {0} must be at least {2} and at max {1} characters long.";
        public const string IsExistError = "This {0} Already Exist!";
        public const string IsValueValid = "{0} Value Not Acceptable!";
        public const string BookIsExistError = "The Author Already Has This Book";
        public const string PasswordMatchError = "The password and confirmation password do not match";
        public const string PasswordError = "Password Must Contain lowercase letters and digits";
        public const string PasswordLowerUppercaseAndDigitsAndSpecialCharacterError = "Password must contain at least 8 characters, one uppercase letter, one lowercase letter, one digit, and one special character.";
        public const string UserNameFormate = "Username must start with an alphabet and other characters Must be alphabets, numbers or an underscore";
        public const string EngCharactersOnly = "{0} Must Contain Only English Letters";
        public const string InvalidNumber = "That is Invalid Number!";
        public const string NotAllowedExtension = "Only .png, .jpg, .jpeg files are allowed!";
        public const string MaxSize = "File cannot be more that 2 MB!";
    }
}
