// Stephen Toub

using System;

namespace MsdnMag
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class PreviewHandlerAttribute : Attribute
    {
        private string _name, _extension, _appId;

        public PreviewHandlerAttribute(string name, string extension, string appId)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (extension == null) throw new ArgumentNullException("extension");
            if (appId == null) throw new ArgumentNullException("appId");
            _name = name;
            _extension = extension;
            _appId = appId;
        }

        public string Name { get { return _name; } }
        public string Extension { get { return _extension; } }
        public string AppId { get { return _appId; } }
    }
}
