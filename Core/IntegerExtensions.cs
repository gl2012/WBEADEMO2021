/* Copyright © 2010 Air Shed Systems Inc. All rights reserved.
 * www.airshedsystems.com <http://www.airshedsystems.com>
 * Notice of License
 * Provided to WBEA under license agreement dated 22 December 2010
 */

namespace WBEADMS
{
    public static class IntegerExtensions
    {
        public static bool IsEven(this int i) { return ((i & 1) == 0); }
        public static bool IsOdd(this int i) { return ((i & 1) == 1); }
    }
}