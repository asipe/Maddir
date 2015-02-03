using System;
using System.Runtime.Serialization;

namespace Maddir.Core {
  [Serializable]
  public class MaddirException : Exception {
    public MaddirException() {}
    public MaddirException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    public MaddirException(string msg) : base(msg) {}
    public MaddirException(string msg, Exception e) : base(msg, e) {}
    public MaddirException(string msg, params object[] fmt) : base(string.Format(msg, fmt)) {}
  }
}