using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Reflection;
using System.IO;

namespace Spinpreach.MonsterGirlsPlayer
{
    /// <summary>
    /// 暗号化クラス
    /// </summary>
    public static class Cipher
    {

        #region EncryptString（暗号化）

        public static string EncryptString(string source)
        {
            if (string.Empty.Equals(source)) return string.Empty;

            // 暗号化オブジェクトの作成
            RijndaelManaged rijndael = GetRmInstance();

            // 文字列をバイト型配列に変換する
            byte[] strBytes = Encoding.UTF8.GetBytes(source);

            // 対称暗号化オブジェクトの作成
            ICryptoTransform encryptor = rijndael.CreateEncryptor();

            // バイト型配列を暗号化する
            byte[] encBytes = encryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);

            // 閉じる
            encryptor.Dispose();

            // バイト型配列を文字列に変換して返す
            string ret = Convert.ToBase64String(encBytes);

            return (ret);
        }

        #endregion

        #region DecryptString（複合化）

        public static string DecryptString(string source)
        {

            if (string.Empty.Equals(source)) return string.Empty;

            try
            {
                // 暗号化オブジェクトの作成
                RijndaelManaged rijndael = GetRmInstance();

                //文字列をバイト型配列に戻す
                byte[] strBytes = Convert.FromBase64String(source);

                //対称暗号化オブジェクトの作成
                ICryptoTransform decryptor = rijndael.CreateDecryptor();

                //バイト型配列を復号化する
                //復号化に失敗すると例外CryptographicExceptionが発生
                byte[] decBytes = decryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);

                //閉じる
                decryptor.Dispose();

                //バイト型配列を文字列に戻して返す
                string ret = Encoding.UTF8.GetString(decBytes);

                return (ret);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        #endregion

        /// <summary>
        /// パスワードから共有キーと初期化ベクタを生成する
        /// </summary>
        /// <returns></returns>
        private static RijndaelManaged GetRmInstance()
        {
            //var password = Environment.UserName;
            var password = "spinpreach";
            var salt = Encoding.UTF8.GetBytes(Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().ManifestModule.Name));
            var deriveBytes = new Rfc2898DeriveBytes(password, salt);

            //反復処理回数を指定する デフォルトで1000回
            deriveBytes.IterationCount = 1000;

            //共有キーと初期化ベクタを生成する
            var ret = new RijndaelManaged();
            ret.Key = deriveBytes.GetBytes(ret.BlockSize / 8);
            ret.IV = deriveBytes.GetBytes(ret.BlockSize / 8);
            return (ret);
        }

    }
}