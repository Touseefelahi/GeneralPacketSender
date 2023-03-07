using Avalonia.Data.Converters;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace PacketParser.Converters
{

    public class BytesToHexStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is Memory<byte> memory)
                {
                    StringBuilder sb = new();

                    for (int i = 0; i < memory.Length; i++)
                    {
                        sb.Append(memory.Span[i].ToString("X2")).Append(' ');
                    }

                    if (sb.Length > 2)
                        sb.Remove(sb.Length - 1, 1);
                    return sb.ToString();
                }
            }
            catch (Exception)
            {
                Trace.WriteLine("Failed to convert Str2Hex");
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is string inputValue)
                {
                    inputValue = inputValue.Replace(" ", "");
                    if (inputValue.Length > 0 && inputValue.Length % 2 != 0)
                    {
                        //Adding 0 to second last hex value
                        var lastChar = inputValue[inputValue.Length - 1];
                        var firstSegment = inputValue.Substring(0, inputValue.Length - 1);
                        inputValue = firstSegment + "0" + lastChar;
                    }
                    return new Memory<byte>(System.Convert.FromHexString(inputValue));
                }
            }
            catch (Exception)
            {
                Trace.WriteLine("Failed to convert back Str2Hex");
            }
            return value;
        }
    }
}