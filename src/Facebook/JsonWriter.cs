// --------------------------------
// <copyright file="JsonWriter.cs" company="Nikhil Kothari">
//     Copyright (c) 2010 Nikhil Kothari
// </copyright>
// <author>Nikhil Kothari (nikhilk.net)</author>
// <license>Included in this library with permission. Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://github.com/NikhilK/dynamicrest</website>
// ---------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Text;

namespace Facebook
{

    // TODO: Add date serialization options
    //       ScriptDate, Ticks, Formatted, Object

    internal sealed class JsonWriter : IDisposable
    {

        private StringWriter _internalWriter;
        private IndentedTextWriter _writer;
        private Stack<Scope> _scopes;

        public JsonWriter()
            : this(/* minimizeWhitespace */ false)
        {
        }

        public JsonWriter(bool minimizeWhitespace)
        {
            _writer = new IndentedTextWriter(minimizeWhitespace);
            _scopes = new Stack<Scope>();
            _internalWriter = (StringWriter)_writer.Target;
        }

        public JsonWriter(TextWriter writer)
            : this(writer, /* minimizeWhitespace */ false)
        {
        }

        public JsonWriter(TextWriter writer, bool minimizeWhitespace)
        {
            _writer = new IndentedTextWriter(writer, minimizeWhitespace);
            _scopes = new Stack<Scope>();
            _internalWriter = (StringWriter)_writer.Target;
        }

        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private void InvarientObject()
        {
            Contract.Invariant(_writer != null);
            Contract.Invariant(_internalWriter != null);
            Contract.Invariant(_scopes != null);
        }

        public string Json
        {
            get
            {
                if (_internalWriter != null)
                {
                    return _internalWriter.ToString();
                }
                throw new InvalidOperationException("Only available when you create JsonWriter without passing in your own TextWriter.");
            }
        }

        public void EndScope()
        {
            if (_scopes.Count == 0)
            {
                throw new InvalidOperationException("No active scope to end.");
            }

            _writer.WriteLine();
            _writer.Indent--;

            Scope scope = _scopes.Pop();
            if (scope.Type == ScopeType.Array)
            {
                _writer.Write("]");
            }
            else
            {
                _writer.Write("}");
            }
        }

        public void StartArrayScope()
        {
            StartScope(ScopeType.Array);
        }

        public void StartObjectScope()
        {
            StartScope(ScopeType.Object);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Facebook.JsonWriter+IndentedTextWriter.WriteTrimmed(System.String)")]
        private void StartScope(ScopeType type)
        {
            if (_scopes.Count != 0)
            {
                Scope currentScope = _scopes.Peek();
                if ((currentScope.Type == ScopeType.Array) &&
                    (currentScope.ObjectCount != 0))
                {
                    _writer.WriteTrimmed(", ");
                }

                currentScope.ObjectCount++;
            }

            Scope scope = new Scope(type);
            _scopes.Push(scope);

            if (type == ScopeType.Array)
            {
                _writer.Write("[");
            }
            else
            {
                _writer.Write("{");
            }
            _writer.Indent++;
            _writer.WriteLine();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Facebook.JsonWriter+IndentedTextWriter.WriteTrimmed(System.String)")]
        public void WriteName(string name)
        {
            Contract.Requires(!String.IsNullOrEmpty(name));

            if (_scopes.Count == 0)
            {
                throw new InvalidOperationException("No active scope to write into.");
            }
            if (_scopes.Peek() != null && _scopes.Peek().Type != ScopeType.Object)
            {
                throw new InvalidOperationException("Names can only be written into Object scopes.");
            }

            Scope currentScope = _scopes.Peek();
            if (currentScope.Type == ScopeType.Object)
            {
                if (currentScope.ObjectCount != 0)
                {
                    _writer.WriteTrimmed(", ");
                }

                currentScope.ObjectCount++;
            }

            _writer.Write("\"");
            _writer.Write(name);
            _writer.WriteTrimmed("\": ");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Facebook.JsonWriter+IndentedTextWriter.WriteTrimmed(System.String)")]
        private void WriteCore(string text, bool quotes)
        {
            if (_scopes.Count != 0)
            {
                Scope currentScope = _scopes.Peek();
                if (currentScope == null)
                {
                    throw new InvalidOperationException("The json write is in an invalid state. The current scope is null.");
                }
                if (currentScope.Type == ScopeType.Array)
                {
                    if (currentScope.ObjectCount != 0)
                    {
                        _writer.WriteTrimmed(", ");
                    }

                    currentScope.ObjectCount++;
                }
            }

            if (quotes)
            {
                _writer.Write('"');
            }
            _writer.Write(text);
            if (quotes)
            {
                _writer.Write('"');
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Facebook.JsonWriter.WriteCore(System.String,System.Boolean)")]
        public void WriteValue(bool value)
        {
            WriteCore(value ? "true" : "false", /* quotes */ false);
        }

        public void WriteValue(int value)
        {
            WriteCore(value.ToString(CultureInfo.InvariantCulture), /* quotes */ false);
        }

        public void WriteValue(float value)
        {
            WriteCore(value.ToString(CultureInfo.InvariantCulture), /* quotes */ false);
        }

        public void WriteValue(double value)
        {
            WriteCore(value.ToString(CultureInfo.InvariantCulture), /* quotes */ false);
        }

        public void WriteValue(DateTime dateTime)
        {
            // Uses ISO 8601 format for the dates such as "2010-04-13T15:29:40+0000"
            string value = dateTime.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture);
            WriteCore(value, /* quotes */ true);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Facebook.JsonWriter.WriteCore(System.String,System.Boolean)")]
        public void WriteValue(string s)
        {
            if (s == null)
            {
                WriteCore("null", /* quotes */ false);
            }
            else
            {
                WriteCore(s, /* quotes */ true);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Facebook.JsonWriter.WriteCore(System.String,System.Boolean)")]
        public void WriteValue(ICollection items)
        {
            if ((items == null) || (items.Count == 0))
            {
                WriteCore("[]", /* quotes */ false);
            }
            else
            {
                StartArrayScope();

                foreach (object o in items)
                {
                    WriteValue(o);
                }

                EndScope();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Facebook.JsonWriter.WriteCore(System.String,System.Boolean)")]
        public void WriteValue(IDictionary record)
        {
            if ((record == null) || (record.Count == 0))
            {
                WriteCore("{}", /* quotes */ false);
            }
            else
            {
                StartObjectScope();

                foreach (DictionaryEntry entry in record)
                {
                    string name = entry.Key as string;
                    if (String.IsNullOrEmpty(name))
                    {
                        throw new ArgumentException("Key of unsupported type contained in Hashtable.");
                    }

                    WriteName(name);
                    WriteValue(entry.Value);
                }

                EndScope();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Facebook.JsonWriter.WriteCore(System.String,System.Boolean)")]
        public void WriteValue(IDictionary<string, object> record)
        {
            if ((record == null) || (record.Count == 0))
            {
                WriteCore("{}", /* quotes */ false);
            }
            else
            {
                StartObjectScope();

                foreach (var entry in record)
                {
                    string name = entry.Key;
                    if (String.IsNullOrEmpty(name))
                    {
                        throw new ArgumentException("Key of unsupported type contained in Hashtable.");
                    }

                    WriteName(name);
                    WriteValue(entry.Value);
                }

                EndScope();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Justification = "It is impractical to do this.")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Facebook.JsonWriter.WriteCore(System.String,System.Boolean)")]
        public void WriteValue(object o)
        {
            if (o == null)
            {
                WriteCore("null", /* quotes */ false);
            }
            else if (o is bool)
            {
                WriteValue((bool)o);
            }
            else if (o is int)
            {
                WriteValue((int)o);
            }
            else if (o is float)
            {
                WriteValue((float)o);
            }
            else if (o is double)
            {
                WriteValue((double)o);
            }
            else if (o is DateTime)
            {
                WriteValue((DateTime)o);
            }
            else if (o is string)
            {
                WriteValue((string)o);
            }
            else if (o is IDictionary)
            {
                WriteValue((IDictionary)o);
            }
            else if (o is IDictionary<string, object>)
            {
                WriteValue((IDictionary<string, object>)o);
            }
            else if (o is ICollection)
            {
                WriteValue((ICollection)o);
            }
            else
            {
                StartObjectScope();
#if SILVERLIGHT
                var propDescs = o.GetType().GetProperties();
                foreach (PropertyInfo propDesc in propDescs)
                {
                    WriteName(propDesc.Name);
                    WriteValue(propDesc.GetValue(o, null));
                }
#else
                PropertyDescriptorCollection propDescs = TypeDescriptor.GetProperties(o);
                foreach (PropertyDescriptor propDesc in propDescs)
                {
                    WriteName(propDesc.Name);
                    WriteValue(propDesc.GetValue(o));
                }
#endif
                EndScope();
            }
        }


        private enum ScopeType
        {

            Array = 0,

            Object = 1
        }

        private sealed class Scope
        {

            private int _objectCount;
            private ScopeType _type;

            public Scope(ScopeType type)
            {
                _type = type;
            }

            public int ObjectCount
            {
                get
                {
                    return _objectCount;
                }
                set
                {
                    _objectCount = value;
                }
            }

            public ScopeType Type
            {
                get
                {
                    return _type;
                }
            }
        }


        private sealed class IndentedTextWriter : TextWriter
        {

            private TextWriter _writer;
            private bool _minimize;

            private int _indentLevel = 0;
            private bool _tabsPending = false;
            private string _tabString = "  ";

            public IndentedTextWriter(bool minimize)
                : base(CultureInfo.InvariantCulture)
            {
                _writer = new StringWriter(CultureInfo.InvariantCulture);
                _minimize = minimize;

                if (_minimize)
                {
                    NewLine = "\r";
                }
            }

            public IndentedTextWriter(TextWriter writer, bool minimize)
                : base(CultureInfo.InvariantCulture)
            {
                // <pex>
                Debug.Assert(writer != (TextWriter)null, "writer");
                // </pex>
                _writer = writer;
                _minimize = minimize;

                if (_minimize)
                {
                    NewLine = "\r";
                }
            }

            public override Encoding Encoding
            {
                get
                {
                    return _writer.Encoding;
                }
            }

            public override string NewLine
            {
                get
                {
                    return _writer.NewLine;
                }
                set
                {
                    _writer.NewLine = value;
                }
            }

            public int Indent
            {
                get
                {
                    return _indentLevel;
                }
                set
                {
                    Debug.Assert(value >= 0);
                    if (value < 0)
                    {
                        value = 0;
                    }
                    _indentLevel = value;
                }
            }

            public TextWriter Target
            {
                get
                {
                    return _writer;
                }
            }

            public override void Close()
            {
                _writer.Close();
            }

            public override void Flush()
            {
                _writer.Flush();
            }

            private void OutputTabs()
            {
                if (_tabsPending)
                {
                    if (_minimize == false)
                    {
                        for (int i = 0; i < _indentLevel; i++)
                        {
                            _writer.Write(_tabString);
                        }
                    }
                    _tabsPending = false;
                }
            }

            public override void Write(string s)
            {
                OutputTabs();
                _writer.Write(s);
            }

            public override void Write(bool value)
            {
                OutputTabs();
                _writer.Write(value);
            }

            public override void Write(char value)
            {
                OutputTabs();
                _writer.Write(value);
            }

            public override void Write(char[] buffer)
            {
                OutputTabs();
                _writer.Write(buffer);
            }

            public override void Write(char[] buffer, int index, int count)
            {
                OutputTabs();
                _writer.Write(buffer, index, count);
            }

            public override void Write(double value)
            {
                OutputTabs();
                _writer.Write(value);
            }

            public override void Write(float value)
            {
                OutputTabs();
                _writer.Write(value);
            }

            public override void Write(int value)
            {
                OutputTabs();
                _writer.Write(value);
            }

            public override void Write(long value)
            {
                OutputTabs();
                _writer.Write(value);
            }

            public override void Write(object value)
            {
                OutputTabs();
                _writer.Write(value);
            }

            public override void Write(string format, object arg0)
            {
                OutputTabs();
                _writer.Write(format, arg0);
            }

            public override void Write(string format, object arg0, object arg1)
            {
                OutputTabs();
                _writer.Write(format, arg0, arg1);
            }

            public override void Write(string format, params object[] arg)
            {
                OutputTabs();
                _writer.Write(format, arg);
            }

            //public void WriteLineNoTabs(string s)
            //{
            //    _writer.WriteLine(s);
            //}

            public override void WriteLine(string s)
            {
                OutputTabs();
                _writer.WriteLine(s);
                _tabsPending = true;
            }

            public override void WriteLine()
            {
                OutputTabs();
                _writer.WriteLine();
                _tabsPending = true;
            }

            public override void WriteLine(bool value)
            {
                OutputTabs();
                _writer.WriteLine(value);
                _tabsPending = true;
            }

            public override void WriteLine(char value)
            {
                OutputTabs();
                _writer.WriteLine(value);
                _tabsPending = true;
            }

            public override void WriteLine(char[] buffer)
            {
                OutputTabs();
                _writer.WriteLine(buffer);
                _tabsPending = true;
            }

            public override void WriteLine(char[] buffer, int index, int count)
            {
                OutputTabs();
                _writer.WriteLine(buffer, index, count);
                _tabsPending = true;
            }

            public override void WriteLine(double value)
            {
                OutputTabs();
                _writer.WriteLine(value);
                _tabsPending = true;
            }

            public override void WriteLine(float value)
            {
                OutputTabs();
                _writer.WriteLine(value);
                _tabsPending = true;
            }

            public override void WriteLine(int value)
            {
                OutputTabs();
                _writer.WriteLine(value);
                _tabsPending = true;
            }

            public override void WriteLine(long value)
            {
                OutputTabs();
                _writer.WriteLine(value);
                _tabsPending = true;
            }

            public override void WriteLine(object value)
            {
                OutputTabs();
                _writer.WriteLine(value);
                _tabsPending = true;
            }

            public override void WriteLine(string format, object arg0)
            {
                OutputTabs();
                _writer.WriteLine(format, arg0);
                _tabsPending = true;
            }

            public override void WriteLine(string format, object arg0, object arg1)
            {
                OutputTabs();
                _writer.WriteLine(format, arg0, arg1);
                _tabsPending = true;
            }

            public override void WriteLine(string format, params object[] arg)
            {
                OutputTabs();
                _writer.WriteLine(format, arg);
                _tabsPending = true;
            }

            public override void WriteLine(UInt32 value)
            {
                OutputTabs();
                _writer.WriteLine(value);
                _tabsPending = true;
            }

            //public void WriteSignificantNewLine()
            //{
            //    WriteLine();
            //}

            //public void WriteNewLine()
            //{
            //    if (_minimize == false)
            //    {
            //        WriteLine();
            //    }
            //}

            public void WriteTrimmed(string text)
            {
                Contract.Requires(!String.IsNullOrEmpty(text));

                if (_minimize == false)
                {
                    Write(text);
                }
                else
                {
                    Write(text.Trim());
                }
            }

            [ContractInvariantMethod]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
            private void InvarientObject()
            {
                Contract.Invariant(_writer != null);
                Contract.Invariant(!String.IsNullOrEmpty(_tabString));
            }
        }

        public void Dispose()
        {
            if (_internalWriter != null)
            {
                _internalWriter.Dispose();
            }
            if (_writer != null)
            {
                _writer.Dispose();
            }
        }
    }
}