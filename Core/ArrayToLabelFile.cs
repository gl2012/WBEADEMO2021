/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */
using System.Collections.Generic;
using System.Text;
namespace WBEADMS
{
    public class ArrayToLabelFile
    {
        public static byte[] ByteArray(IEnumerable<string> labels, int columns)
        {
            StringBuilder csv = new StringBuilder();

            foreach (var label in labels)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < columns; i++)
                {
                    if (i > 0) { sb.Append(","); }
                    sb.Append("\"" + label + "\"");
                }

                csv.Append(sb);
                csv.Append("\n");
            }

            var encoding = new System.Text.ASCIIEncoding();
            byte[] csvBytes = encoding.GetBytes(csv.ToString());
            return csvBytes;
        }
    }
}