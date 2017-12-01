using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGBase.Constants;
using RPGBase.Pooled;

namespace RPGBase.Flyweights
{
    public sealed class ScriptVariable
    {
        /** the floating-point array value the {@link ScriptVariable} references. */
        private float[] faval;
        public float[] Faval
        {
            get
            {
                if (type != ScriptConsts.TYPE_G_03_FLOAT_ARR
                        && type != ScriptConsts.TYPE_L_11_FLOAT_ARR)
                {
                    throw new RPGException(ErrorMessage.INTERNAL_ERROR, "Not a floating-point array");
                }
                return faval;
            }
        }
        /** the floating-point value the {@link ScriptVariable} references. */
        private float fval;
        public float Fval
        {
            get
            {
                if (type != ScriptConsts.TYPE_G_02_FLOAT
                        && type != ScriptConsts.TYPE_L_10_FLOAT)
                {
                    throw new RPGException(ErrorMessage.INTERNAL_ERROR, "Not a floating-point value");
                }
                return fval;
            }
        }
        /** the integer array value the {@link ScriptVariable} references. */
        private int[] iaval;
        public int[] Iaval
        {
            get
            {
                if (type != ScriptConsts.TYPE_G_05_INT_ARR
                        && type != ScriptConsts.TYPE_L_13_INT_ARR)
                {
                    throw new RPGException(ErrorMessage.INTERNAL_ERROR, "Not an integer array");
                }
                return iaval;
            }
        }
        /** the integer value the {@link ScriptVariable} references. */
        private int ival;
        public int Ival
        {
            get
            {
                if (type != ScriptConsts.TYPE_G_04_INT
                        && type != ScriptConsts.TYPE_L_12_INT)
                {
                    throw new RPGException(ErrorMessage.INTERNAL_ERROR, "Not an integer variable");
                }
                return ival;
            }
        }
        /** the long array value the {@link ScriptVariable} references. */
        private long[] laval;
        public long[] Laval
        {
            get
            {
                if (type != ScriptConsts.TYPE_G_07_LONG_ARR
                        && type != ScriptConsts.TYPE_L_15_LONG_ARR)
                {
                    throw new RPGException(ErrorMessage.INTERNAL_ERROR, "Not an long integer array");
                }
                return laval;
            }
        }
        /** the long value the {@link ScriptVariable} references. */
        private long lval;
        public long Lval
        {
            get
            {
                if (type != ScriptConsts.TYPE_G_06_LONG
                        && type != ScriptConsts.TYPE_L_14_LONG)
                {
                    throw new RPGException(ErrorMessage.INTERNAL_ERROR, "Not an long integer variable");
                }
                return lval;
            }
        }
        /** the {@link ScriptVariable}'s name. */
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (value == null)
                {
                    throw new RPGException(ErrorMessage.BAD_PARAMETERS, "Name field is null.");
                }
                if (value.Trim().Length == 0)
                {
                    throw new RPGException(ErrorMessage.BAD_PARAMETERS, "Name field is empty.");
                }
                name = value;
            }
        }

        /** the string value the {@link ScriptVariable} references. */
        private string text;
        /// <summary>
        /// the string value the {@link ScriptVariable} references.
        /// </summary>
        public string Text
        {
            get
            {
                if (type != ScriptConsts.TYPE_G_00_TEXT
                        && type != ScriptConsts.TYPE_L_08_TEXT)
                {
                    throw new RPGException(
                            ErrorMessage.INTERNAL_ERROR, "Not a String variable");
                }
                return text;
            }
        }
        /** the string array value the {@link ScriptVariable} references. */
        private string[] textaval;
        public string[] Textaval
        {
            get
            {
                if (type != ScriptConsts.TYPE_G_01_TEXT_ARR
                        && type != ScriptConsts.TYPE_L_09_TEXT_ARR)
                {
                    throw new RPGException(
                            ErrorMessage.INTERNAL_ERROR, "Not a string array");
                }
                return textaval;
            }
        }
        /** the {@link ScriptVariable}'s type. */
        private int type;
        /** the {@link ScriptVariable}'s type. */
        public int Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
                ValidateType();
                Clear();
            }
        }
        /**
         * Creates a new instance of {@link ScriptVariable}.
         * @param newName the variable name
         * @param newType the variable type
         * @param value the variable value
         * @ if the type is invalid
         */
        public ScriptVariable(string newName, int newType, object value)
        {
            if (newName == null)
            {
                throw new RPGException(ErrorMessage.BAD_PARAMETERS, "Name field is null.");
            }
            if (newName.Trim().Length == 0)
            {
                throw new RPGException(ErrorMessage.BAD_PARAMETERS, "Name field is empty.");
            }
            name = newName;
            type = newType;
            ValidateType();
            Set(value);
        }
        /**
         * Creates a new instance of {@link ScriptVariable}.
         * @param newName the variable name
         * @param newType the variable type
         * @param value the variable value
         * @ if the type is invalid
         */
        public ScriptVariable(ScriptVariable clone)
        {
            if (clone == null)
            {
                throw new RPGException(ErrorMessage.BAD_PARAMETERS, "Cannot clone from null!");
            }
            name = clone.name;
            type = clone.type;
            if (clone.faval != null)
            {
                faval = new float[clone.faval.Length];
                Array.Copy(clone.faval, faval, clone.faval.Length);
            }
            fval = clone.fval;
            if (clone.iaval != null)
            {
                iaval = new int[clone.iaval.Length];
                Array.Copy(clone.iaval, iaval, clone.iaval.Length);
            }
            ival = clone.ival;
            if (clone.laval != null)
            {
                laval = new long[clone.laval.Length];
                Array.Copy(clone.laval, laval, clone.laval.Length);
            }
            lval = clone.lval;
            if (clone.text != null)
            {
                text = new string(clone.text.ToCharArray());
            }
            if (clone.textaval != null)
            {
                textaval = new string[clone.textaval.Length];
                Array.Copy(clone.textaval, textaval, clone.textaval.Length);
            }
        }
        /** Clears up member fields, releasing their memory. */
        public void Clear()
        {
            faval = null;
            fval = 0f;
            iaval = null;
            ival = 0;
            laval = null;
            lval = 0;
            name = null;
            text = null;
            textaval = null;
        }
        /**
         * Sets a value in the array the {@link ScriptVariable} references.
         * @param index the array index
         * @param value the value to set
         * @ if the type is invalid
         */
        public void Set(int index, object value)
        {
            bool throwException = false;
            switch (type)
            {
                case ScriptConsts.TYPE_G_01_TEXT_ARR:
                case ScriptConsts.TYPE_L_09_TEXT_ARR:
                    if (textaval == null)
                    {
                        textaval = new string[0];
                    }
                    if (index >= textaval.Length)
                    {
                        Array.Resize(ref textaval, index + 1);
                    }
                    textaval[index] = value.ToString();
                    break;
                case ScriptConsts.TYPE_G_03_FLOAT_ARR:
                case ScriptConsts.TYPE_L_11_FLOAT_ARR:
                    if (faval == null)
                    {
                        faval = new float[0];
                    }
                    if (index >= faval.Length)
                    {
                        Array.Resize(ref faval, index + 1);
                    }
                    if (value == null)
                    {
                        throwException = true;
                    }
                    else if (value is float
                        || value is double
                        || value is int)
                    {
                        faval[index] = Convert.ToSingle(value);
                    }
                    else if (value is string)
                    {
                        try
                        {
                            faval[index] = float.Parse((string)value);
                        }
                        catch (Exception ex)
                        {
                            throwException = true;
                        }
                    }
                    else
                    {
                        throwException = true;
                    }
                    break;
                case ScriptConsts.TYPE_G_05_INT_ARR:
                case ScriptConsts.TYPE_L_13_INT_ARR:
                    if (iaval == null)
                    {
                        iaval = new int[0];
                    }
                    if (index >= iaval.Length)
                    {
                        Array.Resize(ref iaval, index + 1);
                    }
                    if (value == null)
                    {
                        throwException = true;
                    }
                    else if (value is float
                        || value is double
                        || value is int)
                    {
                        iaval[index] = Convert.ToInt32(value);
                    }
                    else if (value is string)
                    {
                        try
                        {
                            iaval[index] = int.Parse((string)value);
                        }
                        catch (Exception ex)
                        {
                            throwException = true;
                        }
                    }
                    else
                    {
                        throwException = true;
                    }
                    break;
                case ScriptConsts.TYPE_G_07_LONG_ARR:
                case ScriptConsts.TYPE_L_15_LONG_ARR:
                    if (laval == null)
                    {
                        laval = new long[0];
                    }
                    if (index >= laval.Length)
                    {
                        Array.Resize(ref laval, index + 1);
                    }
                    if (value == null)
                    {
                        throwException = true;
                    }
                    else if (value is float
                        || value is double
                        || value is int
                        || value is long)
                    {
                        laval[index] = Convert.ToInt64(value);
                    }
                    else if (value is String)
                    {
                        try
                        {
                            laval[index] = long.Parse((string)value);
                        }
                        catch (Exception ex)
                        {
                            throwException = true;
                        }
                    }
                    else
                    {
                        throwException = true;
                    }
                    break;
                default:
                    throwException = true;
                    break;
            }
            if (throwException)
            {
                PooledStringBuilder sb =
                        StringBuilderPool.GetInstance().GetStringBuilder();
                try
                {
                    sb.Append("Invalid array ScriptVariable type - ");
                    sb.Append(type);
                    sb.Append(".");
                }
                catch (PooledException e)
                {
                    sb.ReturnToPool();
                    sb = null;
                    throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
                }
                RPGException ex = new RPGException(ErrorMessage.BAD_PARAMETERS, sb.ToString());
                sb.ReturnToPool();
                sb = null;
                throw ex;
            }
        }
        /**
         * Sets the value the {@link ScriptVariable} references.
         * @param value the floating-point array value to set
         * @ if the type is invalid
         */
        public void Set(Object value)
        {
            bool throwException = false;
            switch (type)
            {
                case ScriptConsts.TYPE_G_00_TEXT:
                case ScriptConsts.TYPE_L_08_TEXT:
                    if (value == null)
                    {
                        text = null;
                    }
                    else
                    {
                        text = value.ToString();
                    }
                    break;
                case ScriptConsts.TYPE_G_01_TEXT_ARR:
                case ScriptConsts.TYPE_L_09_TEXT_ARR:
                    if (value == null)
                    {
                        textaval = new string[0];
                    }
                    else if (value is string)
                    {
                        textaval = new string[] { value.ToString() };
                    }
                    else if (value is string[])
                    {
                        textaval = new string[((String[])value).Length];
                        Array.Copy((String[])value, textaval, textaval.Length);
                    }
                    else
                    {
                        throwException = true;
                    }
                    break;
                case ScriptConsts.TYPE_G_02_FLOAT:
                case ScriptConsts.TYPE_L_10_FLOAT:
                    if (value == null)
                    {
                        fval = 0;
                    }
                    else if (value is float
                        || value is double
                        || value is int)
                    {
                        fval = Convert.ToSingle(value);
                    }
                    else if (value is string)
                    {
                        try
                        {
                            fval = float.Parse((String)value);
                        }
                        catch (Exception ex)
                        {
                            throwException = true;
                        }
                    }
                    else
                    {
                        throwException = true;
                    }
                    break;
                case ScriptConsts.TYPE_G_03_FLOAT_ARR:
                case ScriptConsts.TYPE_L_11_FLOAT_ARR:
                    if (value == null)
                    {
                        faval = new float[0];
                    }
                    else if (value is float[])
                    {
                        faval = (float[])value;
                    }
                    else
                    {
                        if (faval == null)
                        {
                            faval = new float[0];
                        }
                        Array.Resize(ref faval, faval.Length + 1);
                        if (value is float
                            || value is double
                            || value is int)
                        {
                            faval[faval.Length - 1] = Convert.ToSingle(value);
                        }
                        else if (value is String)
                        {
                            try
                            {

                                faval[faval.Length - 1] = float.Parse((string)value);
                            }
                            catch (Exception ex)
                            {
                                throwException = true;
                            }
                        }
                        else
                        {
                            throwException = true;
                        }
                    }
                    break;
                case ScriptConsts.TYPE_G_04_INT:
                case ScriptConsts.TYPE_L_12_INT:
                    if (value == null)
                    {
                        ival = 0;
                    }
                    else if (value is float
                        || value is double
                        || value is int)
                    {
                        ival = Convert.ToInt32(value);
                    }
                    else if (value is String)
                    {
                        try
                        {
                            ival = int.Parse((String)value);
                        }
                        catch (Exception ex)
                        {
                            throwException = true;
                        }
                    }
                    else
                    {
                        throwException = true;
                    }
                    break;
                case ScriptConsts.TYPE_G_05_INT_ARR:
                case ScriptConsts.TYPE_L_13_INT_ARR:
                    if (value == null)
                    {
                        iaval = new int[0];
                    }
                    else if (value is int[])
                    {
                        iaval = (int[])value;
                    }
                    else
                    {
                        if (iaval == null)
                        {
                            iaval = new int[0];
                        }
                        Array.Resize(ref iaval, iaval.Length + 1);
                        if (value is float
                            || value is double
                            || value is int)
                        {
                            iaval[iaval.Length - 1] = Convert.ToInt32(value);
                        }
                        else if (value is String)
                        {
                            try
                            {

                                iaval[iaval.Length - 1] = int.Parse((string)value);
                            }
                            catch (Exception ex)
                            {
                                throwException = true;
                            }
                        }
                        else
                        {
                            throwException = true;
                        }
                    }
                    break;
                case ScriptConsts.TYPE_G_06_LONG:
                case ScriptConsts.TYPE_L_14_LONG:
                    if (value == null)
                    {
                        lval = 0L;
                    }
                    else if (value is float
                        || value is double
                        || value is int
                        || value is long)
                    {
                        lval = Convert.ToInt64(value);
                    }
                    else if (value is String)
                    {
                        try
                        {
                            lval = long.Parse((String)value);
                        }
                        catch (Exception ex)
                        {
                            throwException = true;
                        }
                    }
                    else
                    {
                        throwException = true;
                    }
                    break;
                case ScriptConsts.TYPE_G_07_LONG_ARR:
                case ScriptConsts.TYPE_L_15_LONG_ARR:
                default:
                    if (value == null)
                    {
                        laval = new long[0];
                    }
                    else if (value is long[])
                    {
                        laval = (long[])value;
                    }
                    else
                    {
                        if (laval == null)
                        {
                            laval = new long[0];
                        }
                        Array.Resize(ref laval, laval.Length + 1);
                        if (value is float
                            || value is double
                            || value is int
                            || value is long)
                        {
                            laval[laval.Length - 1] = Convert.ToInt64(value);
                        }
                        else if (value is String)
                        {
                            try
                            {

                                laval[laval.Length - 1] = long.Parse((string)value);
                            }
                            catch (Exception ex)
                            {
                                throwException = true;
                            }
                        }
                        else
                        {
                            throwException = true;
                        }
                    }
                    break;
            }
            if (throwException)
            {
                PooledStringBuilder sb =
                        StringBuilderPool.GetInstance().GetStringBuilder();
                try
                {
                    sb.Append("Invalid value ");
                    sb.Append(value);
                    sb.Append(" for ScriptVariable type - ");
                    sb.Append(type);
                    sb.Append(".");
                }
                catch (PooledException e)
                {
                    throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
                }
                RPGException ex = new RPGException(ErrorMessage.BAD_PARAMETERS, sb.ToString());
                sb.ReturnToPool();
                throw ex;
            }
        }
        /**
         * Validates the variable type.
         * @ if the type is invalid
         */
        private void ValidateType()
        {
            if (type < ScriptConsts.TYPE_G_00_TEXT
                    || type > ScriptConsts.TYPE_L_15_LONG_ARR)
            {
                PooledStringBuilder sb =
                        StringBuilderPool.GetInstance().GetStringBuilder();
                try
                {
                    sb.Append("Invalid ScriptVariable type - ");
                    sb.Append(type);
                    sb.Append(".");
                }
                catch (PooledException ex)
                {
                    throw new RPGException(ErrorMessage.INTERNAL_ERROR, ex);
                }
                RPGException re = new RPGException(ErrorMessage.BAD_PARAMETERS, sb.ToString());
                sb.ReturnToPool();
                throw re;
            }
        }
    }
}
