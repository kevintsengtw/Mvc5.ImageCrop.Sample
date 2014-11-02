using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCrop.Common
{
    public static class MiscUtility
    {
        #region -- ConvertToInt --

        /// <summary>
        /// Safes the convert to int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int ConvertToInt(this string value)
        {
            return ConvertToInt(value, 0);
        }

        /// <summary>
        /// Safes the convert to int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public static int ConvertToInt(this string value, int val)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return val;
            }
            var result = 0;
            return int.TryParse(value, out result) ? result : val;
        }

        #endregion

        /// <summary>
        /// Makes the GUID.
        /// </summary>
        /// <returns></returns>
        public static string MakeGuid()
        {
            string enc = Convert.ToBase64String(System.Guid.NewGuid().ToByteArray());
            enc = enc.Replace("/", "_");
            enc = enc.Replace("+", "-");
            return enc.Substring(0, 22).Trim();
        }

    }
}