using System;

namespace kassasysteem.Classes
{
    public class ExactError : Exception
    {
        public ExactError() { }

        public ExactError(string s) : base(s) { }
    }
}
