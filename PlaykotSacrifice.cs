using System;
using System.Text.RegularExpressions;

class PlaykotSacrifice {
    
    public static class Binary {
        
        /// <summary> 
        /// This is a recursive implementation of decimal-to-binary conversion
        /// via shift operator.
        /// It should behave exactly like Convert.ToString(value, 2).
        /// </summary>
        /// <param name="value">
        /// Decimal value to convert. 
        /// </param> 
        /// <returns> 
        /// String which holds binary representation of the specified value.
        /// The returned representation does not include leading zeros.
        /// </returns>
        public static string DecToBin(int value) {
            if (value == 0) {
                return "0";
            }
            
            int len = sizeof(int) * 8; // 32 bits;
            char[] bits = new char[len];
            SetBits(ref bits, len - 1, value);
            return GetTrimmedRepresentation(bits, len);
        }
        
        private static void SetBits(ref char[] bits, int position, int value) {
            if (position >= 0) {
                bits[position] = "01"[value & 1];
                SetBits(ref bits, position - 1, value >> 1);
            }
        }
        
        private static string GetTrimmedRepresentation(char[] bits, int len) {
            for (int i = 0; i < len; i++) {
                if (bits[i] == '1') {
                    return new string(bits, i, len - i);
                }
            }
            return string.Empty;
        }
    }
    
    public static class Trim {
        
        private static Regex trimmingRegex; 
        
        static Trim() {
            trimmingRegex = new Regex("(.)(?<=\\1\\1)", RegexOptions.Compiled);      
        }
        
        /// <summary> 
        /// This is a simple solution for trimming repeated characters 
        /// in an arbitrary string using regular expressions.
        /// </summary>
        /// <param name="value">
        /// String to trim.
        /// </param> 
        /// <returns> 
        /// New string which has repeated characters removed.
        /// </returns>
        public static string TrimRepeated(string value) {
            return trimmingRegex.Replace(value, String.Empty);
        }
    }
}
