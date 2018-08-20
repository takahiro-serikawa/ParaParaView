// ShufflieList.cs - image shuffle class

/*
 * initialize
 *  shuffle = new ShuffleList(seed)
 *  if (shuffle.Count <= 0)
 *       shuffle.AddRange(filenames);

 * get next value
 *  shuffle.Get()

 * 前借り
 *  shuffle.Get(filename)

 * get back history
 *  shuffle.Back()
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParaParaView
{
    /// <summary>
    /// 
    /// </summary>
    public class ShuffleList: List<string>
    {
        Random random;
        List<string> history;
        int history_offset;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seed"></param>
        public ShuffleList(int seed)
        {
            random = new Random(seed > 0 ? seed : Environment.TickCount);
            history = new List<string>();
            history_offset = 0;
        }

        const int HISTORY_COUNT = 100;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Get()
        {
            if (history_offset > 0) {
                int i = history.Count - history_offset--;
                return history[i];
            } else {
                //if (stock.Count <= 0)
                //    stock.AddRange(files);

                if (this.Count > 0) {
                    int i = random.Next(this.Count);
                    string value = this[i];
                    this.RemoveAt(i);

                    if (history.Count >= HISTORY_COUNT)
                        history.RemoveAt(0);
                    history.Add(value);

                    return value;
                } else
                    return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Get(string value)
        {
            history_offset = 0;

            int i = this.IndexOf(value);
            if (i < 0)
                return null;

            this.RemoveAt(i);
            
            if (history.Count >= HISTORY_COUNT)
                history.RemoveAt(0);
            history.Add(value);

            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Back()
        {
            if (history.Count > 1+history_offset) {
                int i = history.Count - (1 + ++history_offset);
                return history[i];
            } else
                return null;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        public string Latest()
        {
            history_offset = 0;
            if (history.Count > 0)
                return history[history.Count-1];
            return null;
        }
    }
}
