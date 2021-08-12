/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System;
using System.Security.Cryptography;
using System.Text;

namespace WBEADMS
{

    public static class Authentication
    {

        public static readonly int MinPasswordLength = 4;
        public static readonly int DefaultPasswordLength = 10;

        public static string CreateUser(string userName, string password, string email) { return "No registration feature has been implemented. Please ask an administrator to create an account for you."; }

        public static bool ValidateUser(string userName, string password)
        {
            var user = WBEADMS.Models.User.FetchByName(userName);
            if (user == null) { return false; }

            return PasswordMatch(user, password);
        }

        public static bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            var user = WBEADMS.Models.User.FetchByName(userName);
            if (user == null) { return false; }

            return ChangePassword(user, oldPassword, newPassword);
        }

        public static bool ChangePassword(WBEADMS.Models.User user, string oldPassword, string newPassword)
        {
            if (!PasswordMatch(user, oldPassword)) { return false; }
            SetPassword(user, newPassword);
            user.Save();
            return true;
        }

        public static bool ForgottenPassword(string userName)
        {
            var user = WBEADMS.Models.User.FetchByName(userName);
            if (user == null) { return false; }

            RandomPasswordAndEmail(user, "You have requested a new password");
            return true;
        }

        public static void ResetPassword(WBEADMS.Models.User user)
        {
            RandomPasswordAndEmail(user, "An administrator has reset your password");
        }

        private static bool PasswordMatch(WBEADMS.Models.User user, string entered_password)
        {
            var pwd = new Password(entered_password, user.salt);
            return pwd.ComputeSaltedHash() == user.password;
        }

        public static void SetPassword(WBEADMS.Models.User user, string newPassword)
        {
            string salt;
            string hash;
            Password.GenerateSaltAndHash(newPassword, out salt, out hash);
            user.SetSaltAndHash(salt, hash);
        }

        public static string SetNewPassword(WBEADMS.Models.User user)
        {
            string newPassword = Password.CreateRandomPassword(DefaultPasswordLength);
            SetPassword(user, newPassword);
            return newPassword;
        }

        public static void SendNewPassword(WBEADMS.Models.User user, string newPassword)
        {
            string subject = "WBEADMS Account Creation";
            string body =
                "Hello " + user.display_name + ",\n\n" +
                "Use the following credentials to log on to your account:\n" +
                "user name: @username\n" +
                "password: @newpassword\n\n" +
                "Please log on with this temporary password, go into My Account,\n" +
                "and change to another strong password.\n";
            body = body
                .Replace("@newpassword", newPassword)
                .Replace("@username", user.user_name);
            var email = new Email(user.email, subject, body);
            email.Send();
        }

        private static void RandomPasswordAndEmail(WBEADMS.Models.User user, string heading)
        {
            string subject = "WBEADMS Password Reset";
            string body =
                "Hello " + user.display_name + ",\n\n" +
                "@heading. Use the following password to enter your account:\n" +
                "user name: @username\n" +
                "password: @newpassword\n\n" +
                "Please log on with this temporary password, go into My Account,\n" +
                "and change to another strong password.\n";
            string newPassword = Password.CreateRandomPassword(DefaultPasswordLength);
            body = body
                .Replace("@heading", heading)
                .Replace("@newpassword", newPassword)
                .Replace("@username", user.user_name);
            SetPassword(user, newPassword);
            user.Save();
            var email = new Email(user.email, subject, body);
            email.Send();
        }

        private class Password
        {  // modified from http://www.aspheute.com/english/20040105.asp
            private string _password;
            private string _salt;

            public Password(string password, string salt)
            {
                _password = password;
                _salt = salt;
            }

            public static void GenerateSaltAndHash(string password, out string salt, out string hash)
            {
                salt = Password.CreateRandomSalt();
                var pwd = new Password(password, salt);
                hash = pwd.ComputeSaltedHash();
            }

            public static string CreateRandomPassword(int passwordLength)
            {
                string _allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ23456789";
                byte[] randomBytes = new byte[passwordLength];
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                rng.GetBytes(randomBytes);
                char[] chars = new char[passwordLength];
                int allowedCharCount = _allowedChars.Length;

                for (int i = 0; i < passwordLength; i++)
                {
                    chars[i] = _allowedChars[(int)randomBytes[i] % allowedCharCount];
                }

                return new string(chars);
            }

            public static string CreateRandomSalt()
            {
                byte[] _saltBytes = new byte[4];
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                rng.GetBytes(_saltBytes);

                int newSalt = (((int)_saltBytes[0]) << 24) + (((int)_saltBytes[1]) << 16) +
                             (((int)_saltBytes[2]) << 8) + ((int)_saltBytes[3]);
                return newSalt.ToString();
            }

            public string ComputeSaltedHash()
            {
                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] toHash = encoder.GetBytes(_password + _salt);

                SHA1 sha1 = SHA1.Create();
                byte[] computedHash = sha1.ComputeHash(toHash);
                sha1.Clear();

                return Convert.ToBase64String(computedHash);
            }
        }
    }
}