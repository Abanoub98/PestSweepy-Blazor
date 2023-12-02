namespace Bookify.Web.Core.Consts
{
    public static class RegexPatterns
    {
        public const string UserName = "^[A-Za-z][A-Za-z0-9_]{1,30}$";
        public const string Password_LowercaseAndDigits = "^(?=.*[0-9])(?=.*[a-z]).{0,}$";
        public const string Password_LowerUppercaseAndDigitsAndSpecialCharacter = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
        public const string CharactersOnly_Eng = "^[a-zA-Z-_ ]*$";
        public const string CharactersOnly_Ar = "^[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FF ]*$";
        public const string NumbersAndChrOnly_ArEng = "^(?=.*[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z])[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z0-9 _-]+$";
        public const string DenySpecialCharacters = "^[^<>!#%$]*$";
        public const string ValidNumber_Eg = "^01[0,1,2,5]{1}[0-9]{8}$";
    }
}
