using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QA.Core.Data
{
    public abstract class CacheItemTracker
    {
        /// <summary>
        /// Метод, который вызывается для проверки
        /// </summary>
        ///<param name="changes">словарь, в котрый добавляются изменения </param>
        protected abstract void OnTrackChanges(List<TableModification> changes);

        public void TrackChanges(Dictionary<string, TableModification> changes)
        {
            var list = new List<TableModification>();
            try
            {
                OnTrackChanges(list);

                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        changes[item.TableName] = item;
                    }
                }
            }
            catch (Exception ex)
            {
                ObjectFactoryBase.Logger.ErrorException("Error in " + this.GetType(), ex);
                throw;
            }
        }
    }
}
