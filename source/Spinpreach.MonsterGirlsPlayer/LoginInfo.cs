using System.IO;
using System.Xml.Serialization;

namespace Spinpreach.MonsterGirlsPlayer
{
    public class LoginInfo
    {

        private const string xmlname = "LoginInfo.xml";

        public string UserID { get; set; } = string.Empty;
        public string PassWord { get; set; } = string.Empty;

        public LoginInfo() { }
        public LoginInfo(string UserID, string PassWord)
        {
            this.UserID = UserID;
            this.PassWord = PassWord;
        }

        public bool IsExists()
        {
            if (string.Empty.Equals(this.UserID)) return (false);
            if (string.Empty.Equals(this.PassWord)) return (false);
            return (true);
        }

        public static void Save(LoginInfo value)
        {
            string sPath = Path.Combine(Directory.GetCurrentDirectory(), xmlname);
            var xs = new XmlSerializer(typeof(LoginInfo));
            var fs = new FileStream(sPath, FileMode.Create);
            var obj = new LoginInfo();
            obj.UserID = Cipher.EncryptString(value.UserID);
            obj.PassWord = Cipher.EncryptString(value.PassWord);
            xs.Serialize(fs, obj);
            fs.Close();
        }

        public static LoginInfo Load()
        {
            var obj = new LoginInfo();
            string sPath = Path.Combine(Directory.GetCurrentDirectory(), xmlname);
            if (File.Exists(sPath))
            {
                var xs = new XmlSerializer(typeof(LoginInfo));
                var fs = new FileStream(sPath, FileMode.Open);
                var value = (LoginInfo)xs.Deserialize(fs);
                obj.UserID = Cipher.DecryptString(value.UserID);
                obj.PassWord = Cipher.DecryptString(value.PassWord);
                fs.Close();
            }
            return (obj);
        }

    }
}
