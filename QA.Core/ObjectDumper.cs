// Owners: Karlov Nikolay, Abretov Alexey
//
// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//
// Copyright (C) Microsoft Corporation.  All rights reserved.
//
// edited by Karlov Nikolay

using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;

namespace QA.Core
{
	/// <summary>
	/// Выставляется у полей и пропертей что бы ObjectDumper их игнорировал
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class IgnoreWhileDumpingAttribute : Attribute
	{
	}

	/// <summary>
    /// Сериализация текущего состояния объекта в поток/строку
    /// <remarks>
    /// Используется для журналирования состояния объектов. 
    /// Взято из примеров к VisualStudio 2010
    /// </remarks>
    /// </summary>
    public class ObjectDumper
    {
        const string EqualityString = "=";
        const string EtcString = "...";
        const string ComplexObjectString = "[ ]";
        const string MemberDeclarationString = ": ";
        const string ObjectIsNullString = "null";

        /// <summary>
        /// Записать объект (в консоль).
        /// Анализировать объект до 5го уровня вложенности
        /// </summary>
        /// <param name="element">объект</param>
        public static void Write(object element)
        {
            Write(element, 5);
        }

        /// <summary>
        /// Записать объект (в консоль) с указанием уровня вложенности
        /// </summary>
        /// <param name="element">Уровень вложенности, до которого анализируется объект</param>
        /// <param name="depth">объект</param>
        public static void Write(object element, int depth)
        {
            Write(element, depth, Console.Out);
        }

        /// <summary>
        /// Записать объект в поток
        /// </summary>
        /// <param name="element">Уровень вложенности, до которого анализируется объект</param>
        /// <param name="depth">объект</param>
        public static void Write(object element, int depth, TextWriter log)
        {
            ObjectDumper dumper = new ObjectDumper(depth);
            dumper._writer = log;
            dumper.WriteObject(null, element);
        }

        /// <summary>
        /// Непосредственная сериализация объекта в строковое представление
        /// </summary>
        /// <param name="element"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        public static string DumpObject(object element, int depth)
        {
            using (var writer = new StringWriter())
            {
                ObjectDumper.Write(element, depth, writer);

                return writer.GetStringBuilder().ToString();
            }
        }

        /// <summary>
        /// Непосредственная сериализация объекта в строковое представление c глубиной вложенности 100
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string DumpObject(object element)
        {
            return DumpObject(element, 100);
        }

        /// <summary>
        /// External code
        /// </summary>
        #region Private members
        TextWriter _writer;
        int _pos;
        int _level;
        int _depth;

        private ObjectDumper(int depth)
        {
            this._depth = depth;
        }

        private void Write(string s)
        {
            if (s != null)
            {
                _writer.Write(s);
                _pos += s.Length;
            }
        }

        private void WriteIndent()
        {
            for (int i = 0; i < _level; i++) _writer.Write("  ");
        }

        private void WriteLine()
        {
            _writer.WriteLine();
            _pos = 0;
        }

        private void WriteTab()
        {
            Write("  ");
            while (_pos % 8 != 0) Write(" ");
        }

        private void WriteObject(string prefix, object element)
        {
            if (element == null || element is ValueType || element is string)
            {
                WriteIndent();
                Write(prefix);
                WriteValue(element);
                WriteLine();
            }
            else
            {
                IEnumerable enumerableElement = element as IEnumerable;
                if (enumerableElement != null)
                {
                    foreach (object item in enumerableElement)
                    {
                        if (item is IEnumerable && !(item is string))
                        {
                            WriteIndent();
                            Write(prefix);
                            Write(EtcString);
                            WriteLine();
                            if (_level < _depth)
                            {
                                _level++;
                                WriteObject(prefix, item);
                                _level--;
                            }
                        }
                        else
                        {
                            WriteObject(prefix, item);
                        }
                    }
                }
                else
                {
					MemberInfo[] members = element.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance)
						.Where(x => !Attribute.IsDefined(x, typeof(IgnoreWhileDumpingAttribute)))
						.ToArray();

                    WriteIndent();
                    Write(prefix);
                    bool propWritten = false;
                    foreach (MemberInfo m in members)
                    {
                        FieldInfo f = m as FieldInfo;
                        PropertyInfo p = m as PropertyInfo;
                        if (f != null || p != null)
                        {
                            if (propWritten)
                            {
                                WriteTab();
                            }
                            else
                            {
                                propWritten = true;
                            }
                            Write(m.Name);
                            Write(EqualityString);
                            Type t = f != null ? f.FieldType : p.PropertyType;
                            if ((t.IsValueType || t == typeof(string)) && (p == null || p.CanRead))
                            {
                                WriteValue(f != null ? f.GetValue(element) : p.GetValue(element, null));
                            }
                            else
                            {
                                if (typeof(IEnumerable).IsAssignableFrom(t))
                                {
                                    Write(EtcString);
                                }
                                else
                                {
                                    Write(ComplexObjectString);
                                }
                            }
                        }
                    }
                    if (propWritten) WriteLine();
                    if (_level < _depth)
                    {
                        foreach (MemberInfo m in members)
                        {
                            FieldInfo f = m as FieldInfo;
                            PropertyInfo p = m as PropertyInfo;
                            if (f != null || p != null)
                            {
                                Type t = f != null ? f.FieldType : p.PropertyType;
                                if (!(t.IsValueType || t == typeof(string)))
                                {
                                    object value = f != null ? f.GetValue(element) : p.GetValue(element, null);
                                    if (value != null)
                                    {
                                        _level++;
                                        WriteObject(m.Name + MemberDeclarationString, value);
                                        _level--;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void WriteValue(object o)
        {
            if (o == null)
            {
                Write(ObjectIsNullString);
            }
            else if (o is DateTime)
            {
                Write(((DateTime)o).ToShortDateString());
            }
            else if (o is ValueType || o is string)
            {
                Write(o.ToString());
            }
            else if (o is IEnumerable)
            {
                Write(EtcString);
            }
            else
            {
                Write(ComplexObjectString);
            }
        }
        #endregion
    }

}