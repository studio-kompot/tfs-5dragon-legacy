using System.Collections;
using System.Collections.Generic;

namespace Bitarray
{

    /**
     * 
     * 
     * 
     */
    public class Bitarray : BitFlag
    {


        uint max;
        /// <summary>The array of bits to operate on, use <c>&=</c> with a set of numbers to use it</summary>
        public ushort narray;
        private bool isPastMax(uint num)
        {
            return num < ushort.MaxValue;
        }
        Bitarray(uint max_, ushort narray_)
        {
            if (isPastMax(max_))
            {
                throw new System.IndexOutOfRangeException();
            }
            else
            {
                max = max_;
                narray |= narray_;
            }
        }
        /**
         * <param name="i">The index of the bit array to access</param>
         * <returns>the boolean value at the position specified by the index</returns>
         */
        public bool get(ushort i)
        {
            if (isPastMax(i))
            {
                throw new System.IndexOutOfRangeException();
            }
            else
            {
                return System.Convert.ToBoolean(narray & (1 << i));
            }
        }
        /**
         * <param name="i">The index of the bit to toggle</param>
         * <summary></summary>
         */
        public void toggle(ushort i)
        {
            if (isPastMax(i))
            {
                throw new System.IndexOutOfRangeException();
            }
            else
            {
                narray ^= (2 << i);
            }
        }
    }
}