using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Flyweights
{
    class ScriptVariable
    {
        /** the floating-point array value the {@link ScriptVariable} references. */
        private float[] faval;
        /** the floating-point value the {@link ScriptVariable} references. */
        private float fval;
        /** the integer array value the {@link ScriptVariable} references. */
        private int[] iaval;
        /** the integer value the {@link ScriptVariable} references. */
        private int ival;
        /** the long array value the {@link ScriptVariable} references. */
        private long[] laval;
        /** the long value the {@link ScriptVariable} references. */
        private long lval;
        /** the {@link ScriptVariable}'s name. */
        private byte[] name;
        /** the string value the {@link ScriptVariable} references. */
        private byte[] text;
        /** the string array value the {@link ScriptVariable} references. */
        private byte[][] textaval;
        /** the {@link ScriptVariable}'s type. */
        private int type;
        /**
         * Creates a new instance of {@link ScriptVariable}.
         * @param newName the variable name
         * @param newType the variable type
         * @param value the variable value
         * @ if the type is invalid
         */
        public ScriptVariable( String newName,  int newType,
                 Object value) 
        {
		if (newName == null) {
			throw new RPGException(
                    ErrorMessage.BAD_PARAMETERS, "Name field is null.");
    }
		if (newName.trim().Length() == 0) {
			throw new RPGException(
                    ErrorMessage.BAD_PARAMETERS, "Name field is empty.");
}
name = newName.getBytes();
		type = newType;

        validateType();

        set(value);
	}
	/**
	 * Creates a new instance of {@link ScriptVariable}.
	 * @param newName the variable name
	 * @param newType the variable type
	 * @param value the variable value
	 * @ if the type is invalid
	 */
	public ScriptVariable( ScriptVariable clone) 
{
		if (clone == null) {
        throw new RPGException(ErrorMessage.BAD_PARAMETERS,
                "Cannot clone from null!");
    }
    name = clone.name;
    type = clone.type;
		if (clone.faval != null) {
        faval = new float[clone.faval.Length];
        System.arraycopy(clone.faval, 0, faval, 0, clone.faval.Length);
    }
    fval = clone.fval;
		if (clone.iaval != null) {
        iaval = new int[clone.iaval.Length];
        System.arraycopy(clone.iaval, 0, iaval, 0, clone.iaval.Length);
    }
    ival = clone.ival;
		if (clone.laval != null) {
        laval = new long[clone.laval.Length];
        System.arraycopy(clone.laval, 0, laval, 0, clone.laval.Length);
    }
    lval = clone.lval;
		if (clone.text != null) {
        text = new byte[clone.text.Length];
        System.arraycopy(clone.text, 0, text, 0, clone.text.Length);
    }
		if (clone.textaval != null) {
        textaval = new byte[clone.textaval.Length][];
        System.arraycopy(clone.textaval, 0, textaval, 0, clone.textaval.Length);
    }
}
/** Clears up member fields, releasing their memory. */
public void clear()
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
 * Gets the floating-point array value the {@link ScriptVariable}
 * references.
 * @return <code>float[]</code>
 * @ if the type is invalid
 */
public float[] getFloatArrayVal() 
{
		if (type != ScriptConstants.TYPE_G_03_FLOAT_ARR
				&& type != ScriptConstants.TYPE_L_11_FLOAT_ARR) {
        throw new RPGException(
                ErrorMessage.INTERNAL_ERROR, "Not a floating-point array");
    }
		return faval;
}
/**
 * Gets a floating-point value from the floating-point array the
 * {@link ScriptVariable} references.
 * @param index the value's index
 * @return <code>float</code>
 * @ if the type is invalid
 */
public float getFloatArrayVal( int index) 
{
		if (type != ScriptConstants.TYPE_G_03_FLOAT_ARR
				&& type != ScriptConstants.TYPE_L_11_FLOAT_ARR) {
        throw new RPGException(
                ErrorMessage.INTERNAL_ERROR, "Not a floating-point array");
    }
		return faval [index];
}
/**
 * Gets the floating-point value the {@link ScriptVariable} references.
 * @return <code>float</code>
 * @ if the type is invalid
 */
public float getFloatVal() 
{
		if (type != ScriptConstants.TYPE_G_02_FLOAT
				&& type != ScriptConstants.TYPE_L_10_FLOAT) {
        throw new RPGException(
                ErrorMessage.INTERNAL_ERROR, "Not a floating-point value");
    }
		return fval;
}
/**
 * Gets the integer array value the {@link ScriptVariable} references.
 * @return <code>int[]</code>
 * @ if the type is invalid
 */
public int[] getIntArrayVal() 
{
		if (type != ScriptConstants.TYPE_G_05_INT_ARR
				&& type != ScriptConstants.TYPE_L_13_INT_ARR) {
        throw new RPGException(
                ErrorMessage.INTERNAL_ERROR, "Not an integer array");
    }
		return iaval;
}
/**
 * Gets an integer value from the integer array the {@link ScriptVariable}
 * references.
 * @param index the value's index
 * @return <code>int</code>
 * @ if the type is invalid
 */
public int getIntArrayVal( int index) 
{
		if (type != ScriptConstants.TYPE_G_05_INT_ARR
				&& type != ScriptConstants.TYPE_L_13_INT_ARR) {
        throw new RPGException(
                ErrorMessage.INTERNAL_ERROR, "Not an integer array");
    }
		return iaval [index];
}
/**
 * Gets the integer value the {@link ScriptVariable} references.
 * @return <code>long</code>
 * @ if the type is invalid
 */
public int getIntVal() 
{
		if (type != ScriptConstants.TYPE_G_04_INT
				&& type != ScriptConstants.TYPE_L_12_INT) {
        throw new RPGException(
                ErrorMessage.INTERNAL_ERROR, "Not an integer variable");
    }
		return ival;
}
/**
 * Gets the long integer array value the {@link ScriptVariable} references.
 * @return <code>long[]</code>
 * @ if the type is invalid
 */
public long[] getLongArrayVal() 
{
		if (type != ScriptConstants.TYPE_G_07_LONG_ARR
				&& type != ScriptConstants.TYPE_L_15_LONG_ARR) {
        throw new RPGException(
                ErrorMessage.INTERNAL_ERROR, "Not an long integer array");
    }
		return laval;
}
/**
 * Gets a long integer value from the long integer array the
 * {@link ScriptVariable} references.
 * @param index the value's index
 * @return <code>long</code>
 * @ if the type is invalid
 */
public long getLongArrayVal( int index) 
{
		if (type != ScriptConstants.TYPE_G_07_LONG_ARR
				&& type != ScriptConstants.TYPE_L_15_LONG_ARR) {
        throw new RPGException(
                ErrorMessage.INTERNAL_ERROR, "Not an long integer array");
    }
		return laval [index];
}
/**
 * Gets the long integer value the {@link ScriptVariable} references.
 * @return <code>long</code>
 * @ if the type is invalid
 */
public long getLongVal() 
{
		if (type != ScriptConstants.TYPE_G_06_LONG
				&& type != ScriptConstants.TYPE_L_14_LONG) {
        throw new RPGException(
                ErrorMessage.INTERNAL_ERROR,
                "Not an long integer variable");
    }
		return lval;
}
/**
 * Gets the {@link ScriptVariable} 's name.
 * @return {@link String}
 */
public String getName()
{
    String s = null;
    if (name != null)
    {
        s = new String(name);
    }
    return s;
}
/**
 * Gets the text value the {@link ScriptVariable} references.
 * @return {@link String}
 * @ if the type is invalid
 */
public String getText() 
{
		if (type != ScriptConstants.TYPE_G_00_TEXT
				&& type != ScriptConstants.TYPE_L_08_TEXT) {
        throw new RPGException(
                ErrorMessage.INTERNAL_ERROR, "Not a String variable");
    }
    String s = null;
		if (text != null) {
        s = new String(text);
    }
		return s;
}
/**
 * Gets the {@link String} array value the {@link ScriptVariable} references.
 * @return {@link String}[]
 * @ if the type is invalid
 */
public String[] getTextArrayVal() 
{
		if (type != ScriptConstants.TYPE_G_01_TEXT_ARR
				&& type != ScriptConstants.TYPE_L_09_TEXT_ARR) {
        throw new RPGException(
                ErrorMessage.INTERNAL_ERROR, "Not a string array");
    }
    String []
    arr = new String[0];
		for (int i = 0, len = textaval.Length; i<len; i++) {
			arr = ArrayUtilities.GetInstance().extendArray(

                    new String(textaval[i]), arr);
		}
		return arr;
	}
	/**
	 * Gets a {@link String} value from the {@link String} array the
	 * {@link ScriptVariable} references.
	 * @param index the value's index
	 * @return {@link String}
	 * @ if the type is invalid
	 */
	public String getTextArrayVal( int index) 
{
		if (type != ScriptConstants.TYPE_G_01_TEXT_ARR
				&& type != ScriptConstants.TYPE_L_09_TEXT_ARR) {
        throw new RPGException(
                ErrorMessage.INTERNAL_ERROR, "Not a string array");
    }
		if (index >= textaval.Length) {
        throw new RPGException(
                ErrorMessage.INTERNAL_ERROR, "Invalid index");
    }
		return new String(textaval[index]);
	}
	/**
	 * Gets the {@link ScriptVariable}'s type.
	 * @return <code>int</code>
	 */
	public int getType()
{
    return type;
}
/**
 * Sets a value in the array the {@link ScriptVariable} references.
 * @param index the array index
 * @param value the value to set
 * @ if the type is invalid
 */
public void set( int index,  Object value)

            
{
    bool throwException = false;
		switch (type) {
		case ScriptConstants.TYPE_G_01_TEXT_ARR:
		case ScriptConstants.TYPE_L_09_TEXT_ARR:
			if (textaval == null)
        {
            textaval = new byte[0][];
        }
        if (index >= textaval.Length)
        {
            // add a new value
            byte[][] dest = new byte[index + 1][];
            System.arraycopy(textaval, 0, dest, 0, textaval.Length);
            textaval = dest;
            dest = null;
        }
        if (value == null)
        {
            textaval[index] = null;
        }
        else if (value is String) {
            textaval[index] = ((String)value).getBytes();
        } else if (value is char[]) {
            textaval[index] = new String((char[])value).getBytes();
        } else {
            textaval[index] = value.toString().getBytes();
        }
        break;
		case ScriptConstants.TYPE_G_03_FLOAT_ARR:
		case ScriptConstants.TYPE_L_11_FLOAT_ARR:
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
        else if (value is Float) {
            faval[index] = (float)value;
        } else if (value is Double) {
            faval[index] = Double.valueOf((double)value).floatValue();
        } else if (value is Integer) {
            faval[index] = (int)value;
        } else if (value is String) {
            try
            {
                faval[index] = Float.valueOf((String)value);
            }
            catch (Exception ex)
            {
                throwException = true;
            }
        } else {
            throwException = true;
        }
        break;
		case ScriptConstants.TYPE_G_05_INT_ARR:
		case ScriptConstants.TYPE_L_13_INT_ARR:
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
        else if (value is Float) {
            iaval[index] = Float.valueOf((float)value).intValue();
        } else if (value is Double) {
            iaval[index] = Double.valueOf((double)value).intValue();
        } else if (value is Integer) {
            iaval[index] = (int)value;
        } else if (value is String) {
            try
            {
                iaval[index] = Integer.valueOf((String)value);
            }
            catch (Exception ex)
            {
                throwException = true;
            }
        } else {
            throwException = true;
        }
        break;
		case ScriptConstants.TYPE_G_07_LONG_ARR:
		case ScriptConstants.TYPE_L_15_LONG_ARR:
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
        else if (value is Float) {
            laval[index] = Float.valueOf((float)value).longValue();
        } else if (value is Double) {
            laval[index] = Double.valueOf((double)value).longValue();
        } else if (value is Integer) {
            laval[index] = (int)value;
        } else if (value is Long) {
            laval[index] = (long)value;
        } else if (value is String) {
            try
            {
                laval[index] = Long.valueOf((String)value);
            }
            catch (Exception ex)
            {
                throwException = true;
            }
        } else {
            throwException = true;
        }
        break;
        default:
			throwException = true;
    }
		if (throwException) {
        PooledStringBuilder sb =
                StringBuilderPool.GetInstance().GetStringBuilder();
        try
        {
            sb.append("Invalid ScriptVariable type - ");
            sb.append(type);
            sb.append(".");
        }
        catch (PooledException e)
        {
            sb.ReturnToPool();
            sb = null;
            throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
        }
        RPGException ex = new RPGException(
                ErrorMessage.BAD_PARAMETERS, sb.toString());
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
public void set( Object value) 
{
    bool throwException = false;
		switch (type) {
		case ScriptConstants.TYPE_G_00_TEXT:
		case ScriptConstants.TYPE_L_08_TEXT:
			if (value == null)
        {
            text = null;
        }
        else if (value is String) {
            text = ((String)value).getBytes();
        } else if (value is char[]) {
            text = new String((char[])value).getBytes();
        } else {
            text = value.toString().getBytes();
        }
        break;
		case ScriptConstants.TYPE_G_01_TEXT_ARR:
		case ScriptConstants.TYPE_L_09_TEXT_ARR:
			if (value == null)
        {
            textaval = new byte[0][];
        }
        else if (value is String) {
            textaval = new byte[][] { ((String)value).getBytes() };
        } else if (value is char[]) {
            text = new String((char[])value).getBytes();
            textaval = new byte[][] {
                    new String((char[]) value).getBytes() };
        } else if (value is String[]) {
            textaval = new byte[((String[])value).Length][];
            for (int i = 0, len = ((String[])value).Length; i < len; i++)
            {
                textaval[i] = ((String[])value)[i].getBytes();
            }
        } else if (value is char[][]) {
            textaval = new byte[((char[][])value).Length][];
            for (int i = 0, len = ((char[][])value).Length; i < len; i++)
            {
                textaval[i] = new String(((char[][])value)[i]).getBytes();
            }
        } else {
            throwException = true;
        }
        break;
		case ScriptConstants.TYPE_G_02_FLOAT:
		case ScriptConstants.TYPE_L_10_FLOAT:
			if (value == null)
        {
            fval = 0;
        }
        else if (value is Float) {
            fval = (float)value;
        } else if (value is Double) {
            fval = Double.valueOf((double)value).floatValue();
        } else if (value is Integer) {
            fval = Integer.valueOf((int)value);
        } else if (value is Integer) {
            fval = Integer.valueOf((int)value);
        } else if (value is String) {
            try
            {
                fval = Float.valueOf((String)value);
            }
            catch (Exception ex)
            {
                throwException = true;
            }
        } else {
            throwException = true;
        }
        break;
		case ScriptConstants.TYPE_G_03_FLOAT_ARR:
		case ScriptConstants.TYPE_L_11_FLOAT_ARR:
			if (value == null)
        {
            faval = new float[0];
        }
        else if (value is float[]) {
            faval = (float[])value;
        } else {
            if (faval == null)
            {
                faval = new float[0];
            }
            if (value is Float) {
                faval = ArrayUtilities.GetInstance().extendArray(
                        (Float)value, faval);
            } else if (value is Double) {
                faval = ArrayUtilities.GetInstance().extendArray(
                        Double.valueOf((double)value).floatValue(), faval);
            } else if (value is Integer) {
                faval = ArrayUtilities.GetInstance().extendArray(
                        Integer.valueOf((int)value), faval);
            } else if (value is String) {
                try
                {
                    faval = ArrayUtilities.GetInstance().extendArray(
                            Float.valueOf((String)value), faval);
                }
                catch (Exception ex)
                {
                    throwException = true;
                }
            } else {
                throwException = true;
            }
        }
        break;
		case ScriptConstants.TYPE_G_04_INT:
		case ScriptConstants.TYPE_L_12_INT:
			if (value == null)
        {
            ival = 0;
        }
        else if (value is Float) {
            ival = Float.valueOf((float)value).intValue();
        } else if (value is Double) {
            ival = Double.valueOf((double)value).intValue();
        } else if (value is Integer) {
            ival = (int)value;
        } else if (value is String) {
            try
            {
                ival = Integer.valueOf((String)value);
            }
            catch (Exception ex)
            {
                throwException = true;
            }
        } else {
            throwException = true;
        }
        break;
		case ScriptConstants.TYPE_G_05_INT_ARR:
		case ScriptConstants.TYPE_L_13_INT_ARR:
			if (value == null)
        {
            iaval = new int[0];
        }
        else if (value is int[]) {
            iaval = (int[])value;
        } else {
            if (iaval == null)
            {
                iaval = new int[0];
            }
            if (value is Float) {
                iaval = ArrayUtilities.GetInstance().extendArray(
                        Float.valueOf((float)value).intValue(), iaval);
            } else if (value is Double) {
                iaval = ArrayUtilities.GetInstance().extendArray(
                        Double.valueOf((double)value).intValue(), iaval);
            } else if (value is Integer) {
                iaval = ArrayUtilities.GetInstance().extendArray(
                        (int)value, iaval);
            } else if (value is String) {
                try
                {
                    iaval = ArrayUtilities.GetInstance().extendArray(
                            Integer.valueOf((String)value), iaval);
                }
                catch (Exception ex)
                {
                    throwException = true;
                }
            } else {
                throwException = true;
            }
        }
        break;
		case ScriptConstants.TYPE_G_06_LONG:
		case ScriptConstants.TYPE_L_14_LONG:
			if (value == null)
        {
            lval = 0L;
        }
        else if (value is Float) {
            lval = Float.valueOf((float)value).longValue();
        } else if (value is Double) {
            lval = Double.valueOf((double)value).longValue();
        } else if (value is Integer) {
            lval = (int)value;
        } else if (value is Long) {
            lval = (long)value;
        } else if (value is String) {
            try
            {
                lval = Long.valueOf((String)value);
            }
            catch (Exception ex)
            {
                throwException = true;
            }
        } else {
            throwException = true;
        }
        break;
		case ScriptConstants.TYPE_G_07_LONG_ARR:
		case ScriptConstants.TYPE_L_15_LONG_ARR:
		default:
			if (value == null)
        {
            laval = new long[0];
        }
        else if (value is long[]) {
            laval = (long[])value;
        } else {
            if (laval == null)
            {
                laval = new long[0];
            }
            if (value is Float) {
                laval = ArrayUtilities.GetInstance().extendArray(
                        Float.valueOf((float)value).longValue(), laval);
            } else if (value is Double) {
                laval = ArrayUtilities.GetInstance().extendArray(
                        Double.valueOf((double)value).longValue(), laval);
            } else if (value is Integer) {
                laval = ArrayUtilities.GetInstance().extendArray(
                        (int)value, laval);
            } else if (value is Long) {
                laval = ArrayUtilities.GetInstance().extendArray(
                        (long)value, laval);
            } else if (value is String) {
                try
                {
                    laval = ArrayUtilities.GetInstance().extendArray(
                            Long.valueOf((String)value), laval);
                }
                catch (Exception ex)
                {
                    throwException = true;
                }
            } else {
                throwException = true;
            }
        }
        break;
    }
		if (throwException) {
        PooledStringBuilder sb =
                StringBuilderPool.GetInstance().GetStringBuilder();
        try
        {
            sb.append("Invalid value ");
            sb.append(value);
            sb.append(" for ScriptVariable type - ");
            sb.append(type);
            sb.append(".");
        }
        catch (PooledException e)
        {
            throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
        }
        RPGException ex = new RPGException(
                ErrorMessage.BAD_PARAMETERS, sb.toString());
        sb.ReturnToPool();
        throw ex;
    }
}
/**
 * Sets the {@link ScriptVariable}'s name.
 * @param value the name to set
 * @ if the value field is invalid
 */
public void setName( String value) 
{
		if (value == null) {
        throw new RPGException(
                ErrorMessage.BAD_PARAMETERS, "Name field is null.");
    }
		if (value.trim().Length() == 0) {
        throw new RPGException(
                ErrorMessage.BAD_PARAMETERS, "Name field is empty.");
    }
    name = value.getBytes();
}
/**
 * Sets the {@link ScriptVariable}'s type.
 * @param val the type to set
 * @throws PooledException if one occurs
 * @ if the type is invalid
 */
public void setType( int val)  {
		type = val;

        validateType();

        clear();
	}
	/**
	 * Validates the variable type.
	 * @ if the type is invalid
	 */
	private void validateType() 
{
		if (type < ScriptConstants.TYPE_G_00_TEXT
				|| type > ScriptConstants.TYPE_L_15_LONG_ARR) {
        PooledStringBuilder sb =
                StringBuilderPool.GetInstance().GetStringBuilder();
        try
        {
            sb.append("Invalid ScriptVariable type - ");
            sb.append(type);
            sb.append(".");
        }
        catch (PooledException ex)
        {
            throw new RPGException(ErrorMessage.INTERNAL_ERROR, ex);
        }
        RPGException ex = new RPGException(
                ErrorMessage.BAD_PARAMETERS, sb.toString());
        sb.ReturnToPool();
        throw ex;
    }
}
    }
}
