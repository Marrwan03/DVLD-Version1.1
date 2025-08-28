using DVLD.Applications.Controls;
using DVLD.Properties;
using DVLD.User.Your_Requests.Appointment_Part.Controls;
using DVLD_Buisness;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Speech.Synthesis;
using System.Speech.Synthesis.TtsEngine;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Classes
{
   public static class clsUtil
    {
        public static string GenerateGUID()
        {

            // Generate a new GUID
            Guid newGuid = Guid.NewGuid();

            // convert the GUID to a string
            return newGuid.ToString();
            
        }

        public static bool CreateFolderIfDoesNotExist(string FolderPath)
        {
         
            // Check if the folder exists
            if (!Directory.Exists(FolderPath))
            {
                try
                {
                    // If it doesn't exist, create the folder
                    Directory.CreateDirectory(FolderPath);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error creating folder: " + ex.Message);
                    return false;
                }
            }

            return true;
            
        }
     
        public static string ReplaceFileNameWithGUID(string sourceFile)
        {
            // Full file name. Change your file name   
            string fileName = sourceFile;
            FileInfo fi = new FileInfo(fileName);
            string extn = fi.Extension;
            return GenerateGUID() + extn;

        }
        public static  bool CopyImageToProjectImagesFolder(ref string  sourceFile, string DestinationFolder)
        {
            // this funciton will copy the image to the
            // project images foldr after renaming it
            // with GUID with the same extention, then it will update the sourceFileName with the new name.

           
           // string DestinationFolder = @"C:\DVLD-People-Images\";
            if (!CreateFolderIfDoesNotExist(DestinationFolder))
            {
                return false;
            }

            string destinationFile = DestinationFolder + ReplaceFileNameWithGUID(sourceFile);

            try
            {
                File.Copy(sourceFile, destinationFile, true);

            }
            catch (IOException iox)
            {
                MessageBox.Show (iox.Message,"Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            sourceFile= destinationFile;
            return true;
        }

        public static bool DeleteOldImageFrom(string ImagePath)
        {
            if (ImagePath != "")
            {
                //first we delete the old image from the folder in case there is any.

                try
                {
                    File.Delete(ImagePath);
                    return true;
                }
                catch (IOException)
                {
                    // We could not delete the file.
                    //log it later   
                    return false;
                }
            }
            return true;
        }

        public static void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e, int LicenseID, clsDetainedLicense.enLicenseType licenseType)
        {

            
           
            string Type, ImagePath;
            int _LicenseID;
            DateTime DateOfBirth,IssueDate, ExpirationDate;
            string Name, National, BloodType;
            Image LicenseType;
            short Gendor;
            if (licenseType == clsDetainedLicense.enLicenseType.International)
            {
                clsInternationalLicense internationalLicense = clsInternationalLicense.Find(LicenseID);
                Gendor = internationalLicense.DriverInfo.PersonInfo.Gendor;
                ImagePath = internationalLicense.DriverInfo.PersonInfo.ImagePath;
                _LicenseID = LicenseID;
                Type = "International";
                IssueDate = internationalLicense.IssueDate;
                ExpirationDate = internationalLicense.ExpirationDate;
                DateOfBirth = internationalLicense.DriverInfo.PersonInfo.DateOfBirth;
                Name = internationalLicense.DriverInfo.PersonInfo.FirstName + " " + internationalLicense.DriverInfo.PersonInfo.LastName;
                National = internationalLicense.DriverInfo.PersonInfo.CountryInfo.CountryName;
                BloodType = internationalLicense.DriverInfo.UserInfo.GetBloodType();
                LicenseType = Resources.International_32;
            }
            else
            {
                Type = "Local";
                clsLicense LicenseInfo = clsLicense.Find(LicenseID);
                ImagePath = LicenseInfo.DriverInfo.PersonInfo.ImagePath;
                Gendor = LicenseInfo.DriverInfo.PersonInfo.Gendor;
                _LicenseID = LicenseInfo.LicenseID.Value;
                IssueDate = LicenseInfo.IssueDate;
                ExpirationDate = LicenseInfo.ExpirationDate;
                DateOfBirth = LicenseInfo.DriverInfo.PersonInfo.DateOfBirth;
                Name = LicenseInfo.DriverInfo.PersonInfo.FirstName + " " + LicenseInfo.DriverInfo.PersonInfo.LastName;
                National = LicenseInfo.DriverInfo.PersonInfo.CountryInfo.CountryName;
                BloodType = LicenseInfo.DriverInfo.UserInfo.GetBloodType();
                LicenseType = Resources.Local_32;
            }

            

            Font font = new Font("Arial", 16, FontStyle.Bold);
            // إعداد اللون
            Brush brush = new SolidBrush(Color.DarkRed);
            Brush brushTopic = new SolidBrush(Color.Black);

            // إعداد الموقع

            // إعداد تنسيق النص
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center; // محاذاة وسط
            stringFormat.LineAlignment = StringAlignment.Center; // محاذاة وسط عمودياً

            //Handle if ImagePath == null
            Image image;
            if(string.IsNullOrEmpty(ImagePath))
            {
                if(Gendor == 0)
                    image = Resources.Male_512;
                else
                    image = Resources.Female_512;
            }
            else
                image = Image.FromFile(ImagePath);


            e.Graphics.DrawImage(Resources.CircleBlack, -310, -148, Resources.CircleBlack.Width + 1450, Resources.CircleBlack.Height + 1350);
            e.Graphics.DrawImage(Resources.License, 0, 0, Resources.License.Width - 400, Resources.License.Height - 250);
            e.Graphics.DrawImage(Resources.BackLicenseCard, 0, 570, Resources.BackLicenseCard.Width - 400, Resources.BackLicenseCard.Height - 250);
            e.Graphics.DrawImage(LicenseType, 235, 785, LicenseType.Width + 120, LicenseType.Height + 120);
            e.Graphics.DrawString(National, new Font("Arial", 16, FontStyle.Underline), brushTopic, new RectangleF(35, 85, 200, 50), stringFormat);

            e.Graphics.DrawImage(image, 50, 135, Resources.Heavy_Motorcycle.Width - 50, Resources.Heavy_Motorcycle.Height - 50);

            e.Graphics.DrawString(Name, font, brush, new RectangleF(200, 90, 500, 50), stringFormat);
            e.Graphics.DrawString(_LicenseID.ToString(), font, brush, new RectangleF(350, 135, 200, 50), stringFormat);
            e.Graphics.DrawString(DateOfBirth.ToString("yyy-MM-dd"), font, brush, new RectangleF(350, 185, 200, 50), stringFormat);
            e.Graphics.DrawString(IssueDate.ToString("yyy-MM-dd"), font, brush, new RectangleF(350, 230, 200, 50), stringFormat);
            e.Graphics.DrawString(ExpirationDate.ToString("yyy-MM-dd"), font, brush, new RectangleF(350, 270, 200, 50), stringFormat);
            e.Graphics.DrawString(Type, font, brush, new RectangleF(350, 310, 200, 50), stringFormat);
            e.Graphics.DrawString(National, font, brush, new RectangleF(350, 346, 200, 50), stringFormat);
            e.Graphics.DrawString(BloodType, font, brush, new RectangleF(350, 380, 200, 50), stringFormat);
        }

        public static bool IsLicensePDFExists(int LicenseID, clsDetainedLicense.enLicenseType licenseType)
        {
            string Path;
            if(licenseType == clsDetainedLicense.enLicenseType.Local)
            {
                Path = @"C:/DVLD-LocalLicense-PDF/" + LicenseID + ".pdf";
            }
            else
            {
                Path = @"C:/DVLD-InternationlLicense-PDF/" + LicenseID + ".pdf";
            }
            

            return File.Exists(Path);
        }

        public static bool DeleteLicensePDF(int LicenseID, clsDetainedLicense.enLicenseType licenseType)
        {
            try
            {
                string Path;
                if (licenseType == clsDetainedLicense.enLicenseType.Local)
                {
                    Path = @"C:/DVLD-LocalLicense-PDF/" + LicenseID + ".pdf";
                }
                else
                {
                    Path = @"C:/DVLD-InternationlLicense-PDF/" + LicenseID + ".pdf";
                }

                if (IsLicensePDFExists(LicenseID, licenseType))
                {
                    File.Delete(Path);
                }
            }
            catch 
            {
                return false;
            }
            return true;

            
        }

        public static bool RememberUsernameAndPasswordRegistry(string Username, string Password)
        {
            string KeyPath = @"HKEY_CURRENT_USER\SOFTWARE\Loggin";
            string ValueUsername = "Username";
            string ValuePassword = "Password";
            try
            {
                string UsernameData = Registry.GetValue(KeyPath, ValueUsername, null) as string;
                if (Username == "" && UsernameData != null)
                {
                    using (RegistryKey basekey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                    {
                        using (RegistryKey key = basekey.OpenSubKey(KeyPath, true))
                        {
                            if (key != null)
                            {
                                key.DeleteValue(ValueUsername);
                                key.DeleteValue(ValuePassword);
                                return true;
                            }
                        }
                    }


                }

                Registry.SetValue(KeyPath, ValueUsername, Username, RegistryValueKind.String);
                Registry.SetValue(KeyPath, ValuePassword, Password, RegistryValueKind.String);
                return true;
            }
            //catch(UnauthorizedAccessException)
            //{ return false; }
            catch { return false; }

        }
        public static bool RememberUsernameAndPassword(string Username, string Password)
        {

            try
            {
                //this will get the current project directory folder.
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();


                // Define the path to the text file where you want to save the data
                string filePath = currentDirectory + "\\data.txt";

                //incase the username is empty, delete the file
                if (Username == "" && File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;

                }

                // concatonate username and passwrod withe seperator.
                string dataToSave = Username + "#//#" + Password;

                // Create a StreamWriter to write to the file
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    // Write the data to the file
                    writer.WriteLine(dataToSave);

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }

        }
        public enum enOperationOrder { update, delete }
        public static bool ChangeOrderOfApplicationsToPaidFor(int UserID, int AppType,
            enOperationOrder operationOrder)
        { 
            List<ctrlAddNewApplication.NewOrderInfo> _AllOrders = GetAllNewOrdersOfApplications();
           
            if (_AllOrders.Count < 0)
            {
                return false;
            }

            try
            {
                //this will get the current project directory folder.
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();

                // Define the path to the text file where you want to save the data
                string filePath = currentDirectory + $"\\{ConfigurationManager.AppSettings["FileNameOfApplicationsOrder"]}";
                foreach (var Order in _AllOrders)
                {
                    if (Order.UserID == UserID &&
                        Order.ApplicationType == AppType)
                    {
                        if (operationOrder == enOperationOrder.update)
                        {
                            Order.IsPaid = true;
                            Order.Datetime = DateTime.Now;
                        }
                            
                        else
                        {
                            if (Order.IsPaid)
                                Order.UserID = -1;

                        }
                        break;
                    }
                }

                // Create a StreamWriter to write to the file
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var Order in _AllOrders)
                    {
                        if (Order.UserID != -1)
                            writer.WriteLine(Order.ToString());

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }
            return true;
        }
        public static bool ChangeOrderOfAppointmentsToPaidFor(int UserID, clsTestType.enTestType TestType, enOperationOrder operationOrder)
        {
            List<ctrlAddNewAppointment.clsRequestInfo> _AllOrders = GetAllNewOrdersOfAppointments();

            if (_AllOrders.Count < 0)
            {
                return false;
            }

            try
            {
                //this will get the current project directory folder.
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();

                // Define the path to the text file where you want to save the data
                string filePath = currentDirectory + $"\\{ConfigurationManager.AppSettings["FileNameOfAppointmentsOrder"]}";
                foreach (var Order in _AllOrders)
                {
                    if (Order.UserID == UserID && Order.TestType == TestType)
                    {
                        if (operationOrder == enOperationOrder.delete)
                            Order.UserID = -1;

                        break;
                    }
                }

                // Create a StreamWriter to write to the file
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var Order in _AllOrders)
                    {
                        if (Order.UserID != -1)
                            writer.WriteLine(Order.ToString());

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }
            return true;
        }

        public static bool DeleteIssueLicenseOrderBy(int UserID)
        {
            List<ctrlAllTestAppointmentsInfo.clsIssueLicenseOrder> _dtAllOrders = GetAllNewOrdersOfFirstIssueLicense();

            if (_dtAllOrders.Count < 0)
                return false;

            try
            {
                var DeletedUser = _dtAllOrders.Single(x => x.UserID == UserID);
                if (DeletedUser == null)
                {
                    return false;
                }

                DeletedUser.UserID = -1;

                //this will get the current project directory folder.
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();

                // Define the path to the text file where you want to save the data
                string filePath = currentDirectory + $"\\{ConfigurationManager.AppSettings["FileNameOfIssueLicenseOrder"]}";
                using (StreamWriter write = new StreamWriter(filePath))
                {
                    foreach (var item in _dtAllOrders)
                    {
                        if (item.UserID != -1)
                        {
                            write.WriteLine(item.ToString());
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }
            return true;
        }

        public static bool AddNewOrderInFile(string FileName, string Message)
        {
            try
            {
                //this will get the current project directory folder.
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();


                // Define the path to the text file where you want to save the data
                string filePath = currentDirectory + $"\\{FileName}";

                if (!File.Exists(filePath))
                {
                    using (File.Create(filePath)) { };
                }

                // Create a StreamWriter to write to the file
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    // Write the data to the file
                    writer.WriteLine(Message);

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }

        }

        public static bool IsUserHasIssueLicenseOrderBy(int UserID)
        {
            List<ctrlAllTestAppointmentsInfo.clsIssueLicenseOrder> _dtAllOrders = GetAllNewOrdersOfFirstIssueLicense();

            if (_dtAllOrders.Count < 0)
                return false;

            try
            {
                return _dtAllOrders.Where(x => x.UserID == UserID).Any();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }

            return false;
        }
        public static bool IsUserHasApplicationOrderBy(int UserID)
        {
            List<ctrlAddNewApplication.NewOrderInfo> _dtAllOrders = GetAllNewOrdersOfApplications();

            if (_dtAllOrders.Count < 0)
                return false;

            try
            {
                return _dtAllOrders.Where(x => x.UserID == UserID).Any();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }

            return false;
        }

        public static bool GetStoredCredentialRegistry(ref string Username, ref string Password)
        {
            string KeyPath = @"HKEY_CURRENT_USER\SOFTWARE\Loggin";
            string ValueUsername = "Username";
            string ValuePassword = "Password";
            try
            {
                string UsernameData = Registry.GetValue(KeyPath, ValueUsername, null) as string;
                string PasswordData = Registry.GetValue(KeyPath, ValuePassword, null) as string;

                if (UsernameData != null && PasswordData != null)
                {
                    Username = UsernameData;
                    Password = PasswordData;
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }

        }
        public static List<ctrlAddNewApplication.NewOrderInfo> GetAllNewOrdersOfApplications()
        {
            List<ctrlAddNewApplication.NewOrderInfo> AllNewOrders = new List<ctrlAddNewApplication.NewOrderInfo>();
            //this will get the stored username and password and will return true if found and false if not found.
            try
            {
                //gets the current project's directory
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();

                // Path for the file that contains the credential.
                string filePath = currentDirectory + $"\\{ConfigurationManager.AppSettings["FileNameOfApplicationsOrder"]}";

                // Check if the file exists before attempting to read it
                if (File.Exists(filePath))
                {
                    // Create a StreamReader to read from the file
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        // Read data line by line until the end of the file
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            Console.WriteLine(line); // Output each line of data to the console
                            AllNewOrders.Add(ctrlAddNewApplication.NewOrderInfo.FromString(line));
                        }
                    }

                    //File.Delete(filePath ); //after that you have to delete all orders in file to recive new different orders
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return null;
            }
            return AllNewOrders;
        }

        public static List<ctrlAddNewAppointment.clsRequestInfo> GetAllNewOrdersOfAppointments()
        {
            List<ctrlAddNewAppointment.clsRequestInfo> AllNewOrders = new List<ctrlAddNewAppointment.clsRequestInfo>();
            //this will get the stored username and password and will return true if found and false if not found.
            try
            {
                //gets the current project's directory
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();

                // Path for the file that contains the credential.
                string filePath = currentDirectory + $"\\{ConfigurationManager.AppSettings["FileNameOfAppointmentsOrder"]}";

                // Check if the file exists before attempting to read it
                if (File.Exists(filePath))
                {
                    // Create a StreamReader to read from the file
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        // Read data line by line until the end of the file
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            Console.WriteLine(line); // Output each line of data to the console
                            AllNewOrders.Add(ctrlAddNewAppointment.clsRequestInfo.FromString(line));
                        }
                    }

                    //File.Delete(filePath ); //after that you have to delete all orders in file to recive new different orders
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return null;
            }
            return AllNewOrders;
        }

        public static List<ctrlAllTestAppointmentsInfo.clsIssueLicenseOrder> GetAllNewOrdersOfFirstIssueLicense()
        {
            List<ctrlAllTestAppointmentsInfo.clsIssueLicenseOrder> AllNewOrders = new List<ctrlAllTestAppointmentsInfo.clsIssueLicenseOrder>();
            //this will get the stored username and password and will return true if found and false if not found.
            try
            {
                //gets the current project's directory
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();

                // Path for the file that contains the credential.
                string filePath = currentDirectory + $"\\{ConfigurationManager.AppSettings["FileNameOfIssueLicenseOrder"]}";

                // Check if the file exists before attempting to read it
                if (File.Exists(filePath))
                {
                    // Create a StreamReader to read from the file
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        // Read data line by line until the end of the file
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            Console.WriteLine(line); // Output each line of data to the console
                            AllNewOrders.Add(ctrlAllTestAppointmentsInfo.clsIssueLicenseOrder.FromString(line));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return null;
            }
            return AllNewOrders;
        }

        static void SpeakAloud(string text)
        {
            //Install System.Speech via NuGet(if not available in Assemblies):
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {
                synth.SetOutputToDefaultAudioDevice();
                synth.Speak(text);
                
            }
        }

        public static bool SendEmailSMTP(string fromEmail, string password,
                                  string toEmail, string subject,
                                  string body, string attachmentPath = null)
        {
            try
            {
                // تحديد إعدادات SMTP بناء على مزود البريد
                SmtpInfo smtpInfo = GetSmtpInfo(fromEmail);

                using (SmtpClient client = new SmtpClient(smtpInfo.Server, smtpInfo.Port))
                {
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(fromEmail, password);
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Timeout = 30000;

                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress(fromEmail);
                        message.To.Add(toEmail);
                        message.Subject = subject;
                        message.Body = body;
                        message.IsBodyHtml = false;

                        // إضافة المرفق
                        if (!string.IsNullOrEmpty(attachmentPath) && System.IO.File.Exists(attachmentPath))
                        {
                            Attachment attachment = new Attachment(attachmentPath);
                            message.Attachments.Add(attachment);
                        }

                        client.Send(message);

                        // تحرير المرفق
                        if (message.Attachments.Count > 0)
                        {
                            foreach (Attachment att in message.Attachments)
                            {
                                att.Dispose();
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static SmtpInfo GetSmtpInfo(string email)
        {
            if (email.Contains("gmail.com"))
            {
                return new SmtpInfo { Server = "smtp.gmail.com", Port = 587 };
            }
            else if (email.Contains("yahoo.com"))
            {
                return new SmtpInfo { Server = "smtp.mail.yahoo.com", Port = 587 };
            }
            else if (email.Contains("outlook.com") || email.Contains("hotmail.com"))
            {
                return new SmtpInfo { Server = "smtp-mail.outlook.com", Port = 587 };
            }
            else
            {
                // افتراضي
                return new SmtpInfo { Server = "smtp.gmail.com", Port = 587 };
            }
        }
    }

    public class SmtpInfo
    {
        public string Server { get; set; }
        public int Port { get; set; }
    }
}
