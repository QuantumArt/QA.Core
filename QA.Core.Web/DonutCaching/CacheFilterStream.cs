// Owners: Karlov Nikolay, Abretov Alexey

using System;
using System.IO;
using System.Text;
using System.Web;

namespace QA.Core.Web
{
    /// <summary>
    /// Response.OutputStream фильтр для замены некешируемых частей разметки
    /// </summary>
    internal class CacheFilterStream : Stream
    {
        private static object _o = new object();
        private Stream _shrink;
        private IReplacementStorage _storage;
        private bool _isEnabled;
        private DonutCacheItem _item;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }

        private static Encoding StreamEncoding { get { return HttpContext.Current.Response.Output.Encoding; } }

        public CacheFilterStream(Stream shrink, IReplacementStorage storage)
        {
            _shrink = shrink;
            _storage = storage;
            _isEnabled = true;
        }

        #region Properties
        public override bool CanRead { get { return true; } }
        public override bool CanSeek { get { return true; } }
        public override bool CanWrite { get { return true; } }
        public override void Flush() { _shrink.Flush(); }
        public override long Length { get { return 0; } }
        public override long Position { get; set; }
        #endregion

        #region Stream members
        public override int Read(byte[] buffer, int offset, int count)
        {
            return _shrink.Read(buffer, offset, count);
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            return _shrink.Seek(offset, origin);
        }
        public override void SetLength(long value)
        {
            _shrink.SetLength(value);
        }
        public override void Close()
        {
            _shrink.Close();
        }
        #endregion

        public override void Write(byte[] buffer, int offset, int count)
        {
            var replacements = _storage.GetReplacements();

            if (_isEnabled)
            {
                // если не надо ничего заменять и класть в кеш,
                if (replacements == null || replacements.Count == 0 
                    // замены уже производились
                    && CacheExtension.IsCachingApplied)
                {
                    
                    // то пишем в поток как есть
                    _shrink.Write(buffer, offset, count);
                    return;
                }

                byte[] data = new byte[count];
                Buffer.BlockCopy(buffer, offset, data, 0, count);
                string s = StreamEncoding.GetString(buffer);
                var result = s;

                // Делаем замены
                if (replacements != null && replacements.Count > 0)
                {
                    foreach (var replacement in replacements)
                    {
                        var sb = new StringBuilder();
                        using (var writer = new StringWriter(sb))
                        {
                            if (replacement is MarkupReplacement)
                            {
                                ((MarkupReplacement)replacement).Action(_o).WriteTo(writer);
                            }
                            else if (replacement is ActionReplacement)
                            {
                                // TODO: render action
                                var actionReplacement = (ActionReplacement)replacement;
                                _storage.RenderAction(actionReplacement, writer);
                            }
                        }

                        result = result.Replace(replacement.Key, sb.ToString());
                    }
                }

                if (!CacheExtension.IsCachingApplied)
                {
                    // кешируем
                    if (_item != null)
                    {
                        _item.Result += s;
                    }
                    else
                    {
                        _item = new DonutCacheItem(s, replacements);                        
                    }

                    _storage.SetCache(_item);
                }

                data = StreamEncoding.GetBytes(result);

                _shrink.Write(data, 0, data.Length);
                data = null;
            }
            else
            {
                _shrink.Write(buffer, offset, count);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _item = null;
            _storage = null;

            base.Dispose(disposing);
        }
    }
}