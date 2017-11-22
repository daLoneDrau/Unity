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
         * @throws RPGException if the type is invalid
         */
        public ScriptVariable( String newName,  int newType,
                 Object value) throws RPGException
        {
		if (newName == null) {
			throw new RPGException(
                    ErrorMessage.BAD_PARAMETERS, "Name field is null.");
    }
		if (newName.trim().length() == 0) {
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
	 * @throws RPGException if the type is invalid
	 */
	public ScriptVariable( ScriptVariable clone) throws RPGException
{
		if (clone == null) {
        throw new RPGException(ErrorMessage.BAD_PARAMETERS,
                "Cannot clone from null!");
    }
    name = clone.name;
    type = clone.type;
		if (clone.faval != null) {
        faval = new float[clone.faval.length];
        System.arraycopy(clone.faval, 0, faval, 0, clone.faval.length);
    }
    fval = clone.fval;
		if (clone.iaval != null) {
        iaval = new int[clone.iaval.length];
        System.arraycopy(clone.iaval, 0, iaval, 0, clone.iaval.length);
    }
    ival = clone.ival;
		if (clone.laval != null) {
        laval = new long[clone.laval.length];
        System.arraycopy(clone.laval, 0, laval, 0, clone.laval.length);
    }
    lval = clone.lval;
		if (clone.text != null) {
        text = new byte[clone.text.length];
        System.arraycopy(clone.text, 0, text, 0, clone.text.length);
    }
		if (clone.textaval != null) {
        textaval = new byte[clone.textaval.length][];
        System.arraycopy(clone.textaval, 0, textaval, 0, clone.textaval.length);
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
 * @throws RPGException if the type is invalid
 */
public float[] getFloatArrayVal() throws RPGException
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
 * @throws RPGException if the type is invalid
 */
public float getFloatArrayVal( int index) throws RPGException
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
 * @throws RPGException if the type is invalid
 */
public float getFloatVal() throws RPGException
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
 * @throws RPGException if the type is invalid
 */
public int[] getIntArrayVal() throws RPGException
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
 * @throws RPGException if the type is invalid
 */
public int getIntArrayVal( int index) throws RPGException
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
 * @throws RPGException if the type is invalid
 */
public int getIntVal() throws RPGException
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
 * @throws RPGException if the type is invalid
 */
public long[] getLongArrayVal() throws RPGException
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
 * @throws RPGException if the type is invalid
 */
public long getLongArrayVal( int index) throws RPGException
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
 * @throws RPGException if the type is invalid
 */
public long getLongVal() throws RPGException
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
 * @throws RPGException if the type is invalid
 */
public String getText() throws RPGException
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
 * @throws RPGException if the type is invalid
 */
public String[] getTextArrayVal() throws RPGException
{
		if (type != ScriptConstants.TYPE_G_01_TEXT_ARR
				&& type != ScriptConstants.TYPE_L_09_TEXT_ARR) {
        throw new RPGException(
                ErrorMessage.INTERNAL_ERROR, "Not a string array");
    }
    String []
    arr = new String[0];
		for (int i = 0, len = textaval.length; i<len; i++) {
			arr = ArrayUtilities.getInstance().extendArray(

                    new String(textaval[i]), arr);
		}
		return arr;
	}
	/**
	 * Gets a {@link String} value from the {@link String} array the
	 * {@link ScriptVariable} references.
	 * @param index the value's index
	 * @return {@link String}
	 * @throws RPGException if the type is invalid
	 */
	public String getTextArrayVal( int index) throws RPGException
{
		if (type != ScriptConstants.TYPE_G_01_TEXT_ARR
				&& type != ScriptConstants.TYPE_L_09_TEXT_ARR) {
        throw new RPGException(
                ErrorMessage.INTERNAL_ERROR, "Not a string array");
    }
		if (index >= textaval.length) {
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
 * @throws RPGException if the type is invalid
 */
public void set( int index,  Object value)

            throws RPGException
{
    bool throwException = false;
		switch (type) {
		case ScriptConstants.TYPE_G_01_TEXT_ARR:
		case ScriptConstants.TYPE_L_09_TEXT_ARR:
			if (textaval == null)
        {
            textaval = new byte[0][];
        }
        if (index >= textaval.length)
        {
            // add a new value
            byte[][] dest = new byte[index + 1][];
            System.arraycopy(textaval, 0, dest, 0, textaval.length);
            textaval = dest;
            dest = null;
        }
        if (value == null)
        {
            textaval[index] = null;
        }
        else if (value instanceof String) {
            textaval[index] = ((String)value).getBytes();
        } else if (value instanceof char[]) {
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
        if (index >= faval.length)
        {
            // add a new value
            float[] dest = new float[index + 1];
            System.arraycopy(faval, 0, dest, 0, faval.length);
            faval = dest;
            dest = null;
        }
        if (value == null)
        {
            throwException = true;
        }
        else if (value instanceof Float) {
            faval[index] = (float)value;
        } else if (value instanceof Double) {
            faval[index] = Double.valueOf((double)value).floatValue();
        } else if (value instanceof Integer) {
            faval[index] = (int)value;
        } else if (value instanceof String) {
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
        if (index >= iaval.length)
        {
            // add a new value
            int[] dest = new int[index + 1];
            System.arraycopy(iaval, 0, dest, 0, iaval.length);
            iaval = dest;
            dest = null;
        }
        if (value == null)
        {
            throwException = true;
        }
        else if (value instanceof Float) {
            iaval[index] = Float.valueOf((float)value).intValue();
        } else if (value instanceof Double) {
            iaval[index] = Double.valueOf((double)value).intValue();
        } else if (value instanceof Integer) {
            iaval[index] = (int)value;
        } else if (value instanceof String) {
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
        if (index >= laval.length)
        {
            // add a new value
            long[] dest = new long[index + 1];
            System.arraycopy(laval, 0, dest, 0, laval.length);
            laval = dest;
            dest = null;
        }
        if (value == null)
        {
            throwException = true;
        }
        else if (value instanceof Float) {
            laval[index] = Float.valueOf((float)value).longValue();
        } else if (value instanceof Double) {
            laval[index] = Double.valueOf((double)value).longValue();
        } else if (value instanceof Integer) {
            laval[index] = (int)value;
        } else if (value instanceof Long) {
            laval[index] = (long)value;
        } else if (value instanceof String) {
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
                StringBuilderPool.getInstance().getStringBuilder();
        try
        {
            sb.append("Invalid ScriptVariable type - ");
            sb.append(type);
            sb.append(".");
        }
        catch (PooledException e)
        {
            sb.returnToPool();
            sb = null;
            throw new RPGException(ErrorMessage.INTERNAL_ERROR, e);
        }
        RPGException ex = new RPGException(
                ErrorMessage.BAD_PARAMETERS, sb.toString());
        sb.returnToPool();
        sb = null;
        throw ex;
    }
}
/**
 * Sets the value the {@link ScriptVariable} references.
 * @param value the floating-point array value to set
 * @throws RPGException if the type is invalid
 */
public void set( Object value) throws RPGException
{
    bool throwException = false;
		switch (type) {
		case ScriptConstants.TYPE_G_00_TEXT:
		case ScriptConstants.TYPE_L_08_TEXT:
			if (value == null)
        {
            text = null;
        }
        else if (value instanceof String) {
            text = ((String)value).getBytes();
        } else if (value instanceof char[]) {
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
        else if (value instanceof String) {
            textaval = new byte[][] { ((String)value).getBytes() };
        } else if (value instanceof char[]) {
            text = new String((char[])value).getBytes();
            textaval = new byte[][] {
                    new String((char[]) value).getBytes() };
        } else if (value instanceof String[]) {
            textaval = new byte[((String[])value).length][];
            for (int i = 0, len = ((String[])value).length; i < len; i++)
            {
                textaval[i] = ((String[])value)[i].getBytes();
            }
        } else if (value instanceof char[][]) {
            textaval = new byte[((char[][])value).length][];
            for (int i = 0, len = ((char[][])value).length; i < len; i++)
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
        else if (value instanceof Float) {
            fval = (float)value;
        } else if (value instanceof Double) {
            fval = Double.valueOf((double)value).floatValue();
        } else if (value instanceof Integer) {
            fval = Integer.valueOf((int)value);
        } else if (value instanceof Integer) {
            fval = Integer.valueOf((int)value);
        } else if (value instanceof String) {
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
        else if (value instanceof float[]) {
            faval = (float[])value;
        } else {
            if (faval == null)
            {
                faval = new float[0];
            }
            if (value instanceof Float) {
                faval = ArrayUtilities.getInstance().extendArray(
                        (Float)value, faval);
            } else if (value instanceof Double) {
                faval = ArrayUtilities.getInstance().extendArray(
                        Double.valueOf((double)value).floatValue(), faval);
            } else if (value instanceof Integer) {
                faval = ArrayUtilities.getInstance().extendArray(
                        Integer.valueOf((int)value), faval);
            } else if (value instanceof String) {
                try
                {
                    faval = ArrayUtilities.getInstance().extendArray(
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
        else if (value instanceof Float) {
            ival = Float.valueOf((float)value).intValue();
        } else if (value instanceof Double) {
            ival = Double.valueOf((double)value).intValue();
        } else if (value instanceof Integer) {
            ival = (int)value;
        } else if (value instanceof String) {
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
        else if (value instanceof int[]) {
            iaval = (int[])value;
        } else {
            if (iaval == null)
            {
                iaval = new int[0];
            }
            if (value instanceof Float) {
                iaval = ArrayUtilities.getInstance().extendArray(
                        Float.valueOf((float)value).intValue(), iaval);
            } else if (value instanceof Double) {
                iaval = ArrayUtilities.getInstance().extendArray(
                        Double.valueOf((double)value).intValue(), iaval);
            } else if (value instanceof Integer) {
                iaval = ArrayUtilities.getInstance().extendArray(
                        (int)value, iaval);
            } else if (value instanceof String) {
                try
                {
                    iaval = ArrayUtilities.getInstance().extendArray(
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
        else if (value instanceof Float) {
            lval = Float.valueOf((float)value).longValue();
        } else if (value instanceof Double) {
            lval = Double.valueOf((double)value).longValue();
        } else if (value instanceof Integer) {
            lval = (int)value;
        } else if (value instanceof Long) {
            lval = (long)value;
        } else if (value instanceof String) {
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
        else if (value instanceof long[]) {
            laval = (long[])value;
        } else {
            if (laval == null)
            {
                laval = new long[0];
            }
            if (value instanceof Float) {
                laval = ArrayUtilities.getInstance().extendArray(
                        Float.valueOf((float)value).longValue(), laval);
            } else if (value instanceof Double) {
                laval = ArrayUtilities.getInstance().extendArray(
                        Double.valueOf((double)value).longValue(), laval);
            } else if (value instanceof Integer) {
                laval = ArrayUtilities.getInstance().extendArray(
                        (int)value, laval);
            } else if (value instanceof Long) {
                laval = ArrayUtilities.getInstance().extendArray(
                        (long)value, laval);
            } else if (value instanceof String) {
                try
                {
                    laval = ArrayUtilities.getInstance().extendArray(
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
                StringBuilderPool.getInstance().getStringBuilder();
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
        sb.returnToPool();
        throw ex;
    }
}
/**
 * Sets the {@link ScriptVariable}'s name.
 * @param value the name to set
 * @throws RPGException if the value field is invalid
 */
public void setName( String value) throws RPGException
{
		if (value == null) {
        throw new RPGException(
                ErrorMessage.BAD_PARAMETERS, "Name field is null.");
    }
		if (value.trim().length() == 0) {
        throw new RPGException(
                ErrorMessage.BAD_PARAMETERS, "Name field is empty.");
    }
    name = value.getBytes();
}
/**
 * Sets the {@link ScriptVariable}'s type.
 * @param val the type to set
 * @throws PooledException if one occurs
 * @throws RPGException if the type is invalid
 */
public void setType( int val) throws PooledException, RPGException {
		type = val;

        validateType();

        clear();
	}
	/**
	 * Validates the variable type.
	 * @throws RPGException if the type is invalid
	 */
	private void validateType() throws RPGException
{
		if (type < ScriptConstants.TYPE_G_00_TEXT
				|| type > ScriptConstants.TYPE_L_15_LONG_ARR) {
        PooledStringBuilder sb =
                StringBuilderPool.getInstance().getStringBuilder();
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
        sb.returnToPool();
        throw ex;
    }
}
    }
}
