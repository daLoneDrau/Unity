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
            Name = newName;
            Type = newType;
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
            Name = clone.Name;
            Type = clone.Type;
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
                text = String.Copy(clone.text);
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
                        // add a new value
                    }
                    textaval[index] = value;
                    if (value == null)
                    {
                        textaval[index] = null;
                    }
                    else if (value is String)
                    {
                        textaval[index] = ((String)value).getBytes();
                    }
                    else if (value is char[])
                    {
                        textaval[index] = new String(value);
                    }
                    else
                    {
                        textaval[index] = value.ToString().getBytes();
                    }
                    break;
                case ScriptConsts.TYPE_G_03_FLOAT_ARR:
                case ScriptConsts.TYPE_L_11_FLOAT_ARR:
                    if (faval == null)
                    {
                        faval = new float[0];
                    }
                    if (index >= faval.Length)
                    {
                        // add a new value
                        float[] dest = new float[index + 1];
                        System.arraycopy(faval, 0, dest, 0, faval.Length);
                        faval = dest;
                        dest = null;
                    }
                    if (value == null)
                    {
                        throwException = true;
                    }
                    else if (value is Float)
                    {
                        faval[index] = (float)value;
                    }
                    else if (value is Double)
                    {
                        faval[index] = Double.valueOf((double)value).floatValue();
                    }
                    else if (value is Integer)
                    {
                        faval[index] = (int)value;
                    }
                    else if (value is String)
                    {
                        try
                        {
                            faval[index] = Float.valueOf((String)value);
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
                        // add a new value
                        int[] dest = new int[index + 1];
                        System.arraycopy(iaval, 0, dest, 0, iaval.Length);
                        iaval = dest;
                        dest = null;
                    }
                    if (value == null)
                    {
                        throwException = true;
                    }
                    else if (value is Float)
                    {
                        iaval[index] = Float.valueOf((float)value).intValue();
                    }
                    else if (value is Double)
                    {
                        iaval[index] = Double.valueOf((double)value).intValue();
                    }
                    else if (value is Integer)
                    {
                        iaval[index] = (int)value;
                    }
                    else if (value is String)
                    {
                        try
                        {
                            iaval[index] = Integer.valueOf((String)value);
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
                        // add a new value
                        long[] dest = new long[index + 1];
                        System.arraycopy(laval, 0, dest, 0, laval.Length);
                        laval = dest;
                        dest = null;
                    }
                    if (value == null)
                    {
                        throwException = true;
                    }
                    else if (value is Float)
                    {
                        laval[index] = Float.valueOf((float)value).longValue();
                    }
                    else if (value is Double)
                    {
                        laval[index] = Double.valueOf((double)value).longValue();
                    }
                    else if (value is Integer)
                    {
                        laval[index] = (int)value;
                    }
                    else if (value is Long)
                    {
                        laval[index] = (long)value;
                    }
                    else if (value is String)
                    {
                        try
                        {
                            laval[index] = Long.valueOf((String)value);
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
            }
            if (throwException)
            {
                PooledStringBuilder sb =
                        StringBuilderPool.GetInstance().GetStringBuilder();
                try
                {
                    sb.Append("Invalid ScriptVariable type - ");
                    sb.Append(type);
                    sb.Append(".");
                }
                catch (PooledException e)
                {
                    sb.ReturnToPool();
                    sb = null;
                    throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
                }
                RPGException ex = new RPGException(
                        ErrorMessage.BAD_PARAMETERS, sb.ToString());
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
        public void set(Object value)
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
                    else if (value is String)
                    {
                        text = ((String)value).getBytes();
                    }
                    else if (value is char[])
                    {
                        text = new String((char[])value).getBytes();
                    }
                    else
                    {
                        text = value.ToString().getBytes();
                    }
                    break;
                case ScriptConsts.TYPE_G_01_TEXT_ARR:
                case ScriptConsts.TYPE_L_09_TEXT_ARR:
                    if (value == null)
                    {
                        textaval = new byte[0][];
                    }
                    else if (value is String)
                    {
                        textaval = new byte[][] { ((String)value).getBytes() };
                    }
                    else if (value is char[])
                    {
                        text = new String((char[])value).getBytes();
                        textaval = new byte[][] {
                    new String((char[]) value).getBytes() };
                    }
                    else if (value is String[])
                    {
                        textaval = new byte[((String[])value).Length][];
                        for (int i = 0, len = ((String[])value).Length; i < len; i++)
                        {
                            textaval[i] = ((String[])value)[i].getBytes();
                        }
                    }
                    else if (value is char[][])
                    {
                        textaval = new byte[((char[][])value).Length][];
                        for (int i = 0, len = ((char[][])value).Length; i < len; i++)
                        {
                            textaval[i] = new String(((char[][])value)[i]).getBytes();
                        }
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
                    else if (value is Float)
                    {
                        fval = (float)value;
                    }
                    else if (value is Double)
                    {
                        fval = Double.valueOf((double)value).floatValue();
                    }
                    else if (value is Integer)
                    {
                        fval = Integer.valueOf((int)value);
                    }
                    else if (value is Integer)
                    {
                        fval = Integer.valueOf((int)value);
                    }
                    else if (value is String)
                    {
                        try
                        {
                            fval = Float.valueOf((String)value);
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
                        if (value is Float)
                        {
                            faval = ArrayUtilities.GetInstance().extendArray(
                                    (Float)value, faval);
                        }
                        else if (value is Double)
                        {
                            faval = ArrayUtilities.GetInstance().extendArray(
                                    Double.valueOf((double)value).floatValue(), faval);
                        }
                        else if (value is Integer)
                        {
                            faval = ArrayUtilities.GetInstance().extendArray(
                                    Integer.valueOf((int)value), faval);
                        }
                        else if (value is String)
                        {
                            try
                            {
                                faval = ArrayUtilities.GetInstance().extendArray(
                                        Float.valueOf((String)value), faval);
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
                    else if (value is Float)
                    {
                        ival = Float.valueOf((float)value).intValue();
                    }
                    else if (value is Double)
                    {
                        ival = Double.valueOf((double)value).intValue();
                    }
                    else if (value is Integer)
                    {
                        ival = (int)value;
                    }
                    else if (value is String)
                    {
                        try
                        {
                            ival = Integer.valueOf((String)value);
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
                        if (value is Float)
                        {
                            iaval = ArrayUtilities.GetInstance().extendArray(
                                    Float.valueOf((float)value).intValue(), iaval);
                        }
                        else if (value is Double)
                        {
                            iaval = ArrayUtilities.GetInstance().extendArray(
                                    Double.valueOf((double)value).intValue(), iaval);
                        }
                        else if (value is Integer)
                        {
                            iaval = ArrayUtilities.GetInstance().extendArray(
                                    (int)value, iaval);
                        }
                        else if (value is String)
                        {
                            try
                            {
                                iaval = ArrayUtilities.GetInstance().extendArray(
                                        Integer.valueOf((String)value), iaval);
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
                    else if (value is Float)
                    {
                        lval = Float.valueOf((float)value).longValue();
                    }
                    else if (value is Double)
                    {
                        lval = Double.valueOf((double)value).longValue();
                    }
                    else if (value is Integer)
                    {
                        lval = (int)value;
                    }
                    else if (value is Long)
                    {
                        lval = (long)value;
                    }
                    else if (value is String)
                    {
                        try
                        {
                            lval = Long.valueOf((String)value);
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
                        if (value is Float)
                        {
                            laval = ArrayUtilities.GetInstance().extendArray(
                                    Float.valueOf((float)value).longValue(), laval);
                        }
                        else if (value is Double)
                        {
                            laval = ArrayUtilities.GetInstance().extendArray(
                                    Double.valueOf((double)value).longValue(), laval);
                        }
                        else if (value is Integer)
                        {
                            laval = ArrayUtilities.GetInstance().extendArray(
                                    (int)value, laval);
                        }
                        else if (value is Long)
                        {
                            laval = ArrayUtilities.GetInstance().extendArray(
                                    (long)value, laval);
                        }
                        else if (value is String)
                        {
                            try
                            {
                                laval = ArrayUtilities.GetInstance().extendArray(
                                        Long.valueOf((String)value), laval);
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
                RPGException ex = new RPGException(
                        ErrorMessage.BAD_PARAMETERS, sb.ToString());
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
                RPGException ex = new RPGException(ErrorMessage.BAD_PARAMETERS, sb.ToString());
                sb.ReturnToPool();
                throw ex;
            }
        }
    }
}
