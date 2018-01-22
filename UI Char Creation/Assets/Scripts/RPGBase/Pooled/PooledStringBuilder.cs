using System;
using System.Text;

namespace RPGBase.Pooled
{
    public sealed class PooledStringBuilder : PoolableObject
    {
        /// <summary>
        ///  the initial capacity.
        /// </summary>
        private const int capacity = 1000;
        /// <summary>
        ///  the pooled object's index.
        /// </summary>
        private readonly int poolIndex;
        /// <summary>
        ///  the internal <see cref="StringBuilder"/> instance.
        /// </summary>
        private readonly StringBuilder stringBuilder = new StringBuilder(capacity);
        /// <summary>
        ///  Creates a new instance of <see cref="PooledStringBuilder"/>.
        /// <param name="index">the object's index</param>       
        /// </summary>
        public PooledStringBuilder(int index)
        {
            poolIndex = index;
        }
        /// <summary>
        /// Appends the string representation of the <c>char</c> argument to this
        /// sequence.
        /// <p>
        /// The argument is appended to the contents of this sequence.The length of
        /// this sequence increases by <c>1</c>.
        /// <p></p>
        /// The overall effect is exactly as if the argument were converted to a
        /// string by the method <see cref="Object.ToString"/>, and the character in
        /// that string were then appended <see cref="StringBuilder.Append(string)"/> to this character
        /// sequence.
        /// <paramref name="c"/>a <c>char</c>
        /// <exception cref="">if the item was not locked</exception>
        /// </summary>
        public void Append(char c)
        {
            if (StringBuilderPool.Instance.IsItemLocked(this))
            {
                stringBuilder.Append(c);
            }
            else
            {
                throw new PooledException("Item not locked for use!");
            }
        }
        /// <summary>
        /// Appends the string representation of the <c>char</c> array argument to
        /// this sequence.
        /// < p >
        /// The characters of the array argument are appended, in order, to the
        /// contents of this sequence.The length of this sequence increases by the
        /// length of the argument.
        /// < p >
        /// The overall effect is exactly as if the argument were converted to a
        /// string by the method <see cref="Object.ToString"/>, and the characters
        /// of that string were then <see cref="StringBuilder.Append(string)"/> appended to this
        /// character sequence.
        /// <paramref name="str"/> the characters to be appended.
        /// </summary>
        public void Append(char[] str)
        {
            if (StringBuilderPool.Instance.IsItemLocked(this))
            {
                stringBuilder.Append(new String(str));
            }
            else
            {
                throw new PooledException("Item not locked for use!");
            }
        }
        /// <summary>
        /// Appends the string representation of the <c>float</c> argument to
        /// this sequence.
        /// < p >
        /// The characters of the array argument are appended, in order, to the
        /// contents of this sequence.The length of this sequence increases by the
        /// length of the argument.
        /// < p >
        /// The overall effect is exactly as if the argument were converted to a
        /// string by the method <see cref="Object.ToString"/>, and the characters
        /// of that string were then <see cref="StringBuilder.Append(string)"/> appended to this
        /// character sequence.
        /// <paramref name="f"/> the floating-point value to be appended.
        /// </summary>
        public void Append(float f)
        {
            if (StringBuilderPool.Instance.IsItemLocked(this))
            {
                stringBuilder.Append(f);
            }
            else
            {
                throw new PooledException("Item not locked for use!");
            }
        }
        /// <summary>
        /// Appends the string representation of the <c>int</c> argument to
        /// this sequence.
        /// < p >
        /// The characters of the array argument are appended, in order, to the
        /// contents of this sequence.The length of this sequence increases by the
        /// length of the argument.
        /// < p >
        /// The overall effect is exactly as if the argument were converted to a
        /// string by the method <see cref="Object.ToString"/>, and the characters
        /// of that string were then <see cref="StringBuilder.Append(string)"/> appended to this
        /// character sequence.
        /// <paramref name="i"/> the integer value to be appended.
        /// </summary>
        public void Append(int i)
        {
            if (StringBuilderPool.Instance.IsItemLocked(this))
            {
                stringBuilder.Append(i);
            }
            else
            {
                throw new PooledException("Item not locked for use!");
            }
        }
        /// <summary>
        /// Appends the string representation of the <c>object</c> argument to
        /// this sequence.
        /// < p >
        /// The characters of the array argument are appended, in order, to the
        /// contents of this sequence.The length of this sequence increases by the
        /// length of the argument.
        /// < p >
        /// The overall effect is exactly as if the argument were converted to a
        /// string by the method <see cref="Object.ToString"/>, and the characters
        /// of that string were then <see cref="StringBuilder.Append(string)"/> appended to this
        /// character sequence.
        /// <paramref name="o"/> the object value to be appended.
        /// </summary>
        public void Append(object o)
        {
            if (StringBuilderPool.Instance.IsItemLocked(this))
            {
                stringBuilder.Append(o);
            }
            else
            {
                throw new PooledException("Item not locked for use!");
            }
        }
        /// <summary>
        /// Appends the string representation of the <c>string</c> argument to
        /// this sequence.
        /// < p >
        /// The characters of the array argument are appended, in order, to the
        /// contents of this sequence.The length of this sequence increases by the
        /// length of the argument.
        /// < p >
        /// The overall effect is exactly as if the argument were converted to a
        /// string by the method <see cref="Object.ToString"/>, and the characters
        /// of that string were then <see cref="StringBuilder.Append(string)"/> appended to this
        /// character sequence.
        /// <paramref name="str"/> the string value to be appended.
        /// </summary>
        public void Append(string str)
        {
            if (StringBuilderPool.Instance.IsItemLocked(this))
            {
                stringBuilder.Append(str);
            }
            else
            {
                throw new PooledException("Item not locked for use!");
            }
        }
        /// <summary>
        /// 
        /// Gets the value for the object's index.
        /// @return {@link int}

        /// </summary>
        public int GetPoolIndex()
        {
            return poolIndex;
        }
        public void Init()
        {
            // TODO Auto-generated method stub
        }
        /// <summary>
        /// Returns the length (character count).
        /// <returns>the length of the sequence of characters currently represented by this object</returns> 
        /// </summary>
        public int Length()
        {
            return stringBuilder.Length;
        }
        public void ReturnToPool()
        {
            stringBuilder.Length = 0;
            if (StringBuilderPool.Instance.IsItemLocked(this))
            {
                StringBuilderPool.Instance.UnlockItem(this);
            }
        }
        /// <summary>
        /// Sets the length of the character sequence. The sequence is changed to a
        /// new character sequence whose length is specified by the argument. For
        /// every nonnegative index <i>k</i> less than {@code newLength}, the
        /// character at index <i>k</i> in the new character sequence is the same as
        /// the character at index <i>k</i> in the old sequence if <i>k</i> is less
        /// than the length of the old character sequence; otherwise, it is the null
        /// character {@code '\u005Cu0000'}. In other words, if the {@code newLength}
        /// argument is less than the current length, the length is changed to the
        /// specified length.
        /// <p>
        /// If the {@code newLength} argument is greater than or equal to the current
        /// length, sufficient null characters ({@code '\u005Cu0000'}) are appended
        /// so that length becomes the {@code newLength} argument.
        /// <p>
        /// The {@code newLength} argument must be greater than or equal to {@code 0}.
        /// @param newLength the new length
        /// @throws IndexOutOfBoundsException if the {@code newLength} argument is
        ///             negative.

        /// </summary>
        public void SetLength(int newLength)
        {
            stringBuilder.Length = newLength;
        }
        /// <summary>
        /// 
        /// Returns a string representing the data in this sequence. A new
        /// {@code String} object is allocated and initialized to contain the
        /// character sequence currently represented by this object. This
        /// {@code String} is then returned. Subsequent changes to this sequence do
        /// not affect the contents of the {@code String}.
        /// @return a string representation of this sequence of characters.
        /// </summary>
        public override String ToString()
        {
            return stringBuilder.ToString();
        }
    }
}