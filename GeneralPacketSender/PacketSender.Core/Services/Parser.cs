﻿using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PacketSender.Core
{
    public static class Parser
    {
        /// <summary>
        /// This is generic parser, it parses the data according to the provided parser info
        /// </summary>
        /// <param name="databytes"></param>
        /// <param name="parserInfoList"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static ParsedData[] Parse(ReadOnlyMemory<byte> databytes, IEnumerable<ParserInfo> parserInfoList)
        {
            if (parserInfoList is not null && databytes.Span.Length > 0 && parserInfoList.Any())
            {
                List<ParsedData> listOfValues = new();
                foreach (var parser in parserInfoList)
                {
                    IConvertible value = 0;
                    switch (parser.DataType) //Solve this type so that we can make it totally generic
                    {
                        case DataType.Bool:
                            if (parser.StartIndex >= databytes.Length)
                            {
                                throw new OverflowException($"Length of bytes is not enough Index: {parser.StartIndex} and Array Length: {databytes.Length}");
                            }
                            value = databytes.Span[parser.StartIndex] == 1;
                            break;

                        case DataType.Int8:
                            value = Get<sbyte>(databytes, parser.StartIndex);
                            break;

                        case DataType.UInt8:
                            value = Get<byte>(databytes, parser.StartIndex);
                            break;

                        case DataType.Int16:
                            value = Get<short>(databytes, parser.StartIndex);
                            break;

                        case DataType.UInt16:
                            value = Get<ushort>(databytes, parser.StartIndex);
                            break;

                        case DataType.Int32:
                            value = Get<int>(databytes, parser.StartIndex);
                            break;

                        case DataType.UInt32:
                            value = Get<uint>(databytes, parser.StartIndex);
                            break;

                        case DataType.Int64:
                            value = Get<long>(databytes, parser.StartIndex);
                            break;

                        case DataType.UInt64:
                            value = Get<ulong>(databytes, parser.StartIndex);
                            break;

                        case DataType.Float:
                            value = Get<float>(databytes, parser.StartIndex);
                            break;

                        case DataType.Double:
                            value = Get<double>(databytes, parser.StartIndex);
                            break;
                    }
                    listOfValues.Add(new(parser.Name, value));
                }
                return listOfValues.ToArray();
            }
            return null;
        }

        /// <summary>
        /// Gets the parsed value from memory of bytes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memory">Memory of bytes </param>
        /// <param name="startIndex">Start index of value</param>
        /// <returns>Parsed Value</returns>
        private static T Get<T>(ReadOnlyMemory<byte> memory, int startIndex) where T : struct
        {
            int length = Marshal.SizeOf(typeof(T));
            if (startIndex + length > memory.Length)
            {
                throw new OverflowException($"Length of bytes is not enough Index: {startIndex} with" +
                    $" DataLength: {length} and Array Length: {memory.Length}");
            }
            T type = Unsafe.ReadUnaligned<T>(ref MemoryMarshal.GetReference(memory.Slice(startIndex, length).Span));
            return type;
        }
    }
}