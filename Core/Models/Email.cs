/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System;
using System.Configuration;
using System.IO;
using System.Net.Mail;

namespace WBEADMS
{

    public class Email
    {
        #region private members
        private string _to;
        // private string _from = "info@airshedsystems.com"; // TODO: determine if this works and if it needs to be changed.
        private string _from = "donotreply@wbea.org";
        private string _subject;
        private string _body;
        private string _attachmentPath;
        private bool _htmlBody;
        #endregion

        /// <summary>Initializes a new instance of the Email class.</summary>
        /// <param name="to">semi-colon separated list of email addresses</param>
        /// <param name="subject">subject line of the email</param>
        /// <param name="body">static body text of the email</param>
        public Email(string to, string subject, string body)
        {
            if (String.IsNullOrEmpty(to))
            {
                throw new ArgumentException("To field cannot be empty.");
            }

            _to = to.Trim();
            _subject = (String.IsNullOrEmpty(subject)) ? String.Empty : subject.Trim();
            _body = (String.IsNullOrEmpty(body)) ? String.Empty : body.Trim();
        }

        public Email(string to, string subject, string body, string attachmentPath)
            : this(to, subject, body)
        {
            if (string.IsNullOrEmpty(attachmentPath))
            {
                throw new ArgumentException("The attachment field cannot be set to null or be an empty string");
            }

            if (!File.Exists(attachmentPath))
            {
                throw new FileNotFoundException("Could not find target attachment file " + attachmentPath + " Verify it exists and has proper permission access");
            }

            _attachmentPath = attachmentPath;
        }

        public void Send()
        {
            // set up email

            // TODO: Ensure that the email addresses are proper.
            var toList = _to.Split(';');
            MailMessage email = new MailMessage(_from, toList[0], _subject, _body);
            for (int i = 1; i < toList.Length; i++)
            {
                email.To.Add(toList[i]);
            }

            _htmlBody = false; // TODO: do a check for HTML tags
            email.IsBodyHtml = _htmlBody;
            ////email.Sender = new System.Net.Mail.MailAddress(_from);

            string alertBccEmail = ConfigurationManager.AppSettings["alertBccEmail"];
            if (!string.IsNullOrEmpty(alertBccEmail))
            {
                email.Bcc.Add(alertBccEmail); // TODO: Email Address Validation 
            }

            // attach target file.
            if (!String.IsNullOrEmpty(_attachmentPath))
            {
                email.Attachments.Add(new System.Net.Mail.Attachment(_attachmentPath));
            }

            // send email
            try
            {
                ////Common.Logger.Debug("SENDING EMAIL......");
                GetSmtpClient().Send(email);
            }
            catch
            { // (Exception e) {
                ////Common.Logger.Error("Unable to send email response. " + System.Environment.NewLine + e.ToString());
                throw;
            }
            finally
            {
                email.Dispose();
            }
        }

        private static SmtpClient GetSmtpClient()
        {
            // network credentials
            /* string _smtpUser = ConfigurationManager.AppSettings["smtpUser"];
             string _smtpPassword = ConfigurationManager.AppSettings["smtpPassword"];
             string _smtpHost = ConfigurationManager.AppSettings["smtpHost"];
             bool _smtpEnableSsl = ConfigurationManager.AppSettings["smtpEnableSsl"].ToLower().Contains("true");
             int _smtpPort = int.Parse(ConfigurationManager.AppSettings["smtpPort"]);
             */
            string _smtpUser = "donotreply@wbea.org";
            string _smtpPassword = "EmployPrevention7";
            string _smtpHost = "smtp.office365.com";
            bool _smtpEnableSsl = ConfigurationManager.AppSettings["smtpEnableSsl"].ToLower().Contains("true");
            int _smtpPort = 587;


            SmtpClient smtpClient = new SmtpClient(_smtpHost, _smtpPort);
            smtpClient.Credentials = new System.Net.NetworkCredential(_smtpUser, _smtpPassword);
            smtpClient.EnableSsl = _smtpEnableSsl;

            return smtpClient;
        }
    }
}