using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {

            static string RandomString(int length)
            {
                const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*";
                StringBuilder res = new StringBuilder();
                using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                {
                    byte[] uintBuffer = new byte[sizeof(uint)];

                    while (length-- > 0)
                    {
                        rng.GetBytes(uintBuffer);
                        uint num = BitConverter.ToUInt32(uintBuffer, 0);
                        res.Append(valid[(int)(num % (uint)valid.Length)]);
                    }
                }

                return res.ToString();
            }
            

            String ss = RandomString(6);
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("thewinderapp@gmail.com", "xltbqbsyderpqsxp"),
                EnableSsl = true,

            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress("thewinderapp@gmail.com"),
                Subject = "Authenticatie-code",
                Body = "<h1>Authenticatie-code voor Winder</h1>" +
                "De authenthenticatie-code voor het resetten van het wachtwoord van uw Winder account is: <b>" + $"{ss}</b>"+
                "<br>Met vriendelijke groet,     Het Winder team",
                IsBodyHtml = true,
            };
            mailMessage.To.Add("jannieandes@gmail.com");

            smtpClient.Send(mailMessage);



        }
    }
}