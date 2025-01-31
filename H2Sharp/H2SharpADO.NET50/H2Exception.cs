#region MIT License
/*
 * Copyright � 2008 Jonathan Mark Porter.
 * H2Sharp is a wrapper for the H2 Database Engine. http://h2sharp.googlecode.com
 * 
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 */
#endregion

using System;
using System.Runtime.Serialization;

namespace System.Data.H2
{

    [Serializable]
    public class H2Exception : Exception
    {
        [NonSerialized]
        java.sql.SQLException sqlException;


        internal H2Exception(java.sql.SQLException sqlException)
            : base(sqlException.getMessage())
        {
            this.sqlException = sqlException;
        }
        public H2Exception() { }
        public H2Exception(string message) : base(message) { }
        public H2Exception(string message, Exception inner) : base(message, inner) { }
        protected H2Exception(SerializationInfo info, StreamingContext context)
            : base(info, context) 
        {
        
        }

        public java.sql.SQLException GetSqlException()
        { return sqlException; }
    }
}