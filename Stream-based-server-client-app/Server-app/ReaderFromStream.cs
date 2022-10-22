using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server_app
{
    internal class ReaderFromStream
    {
        protected StreamReader r;
        protected string line = null;

        protected void DoRead()
        {
            try
            {
                this.line = r.ReadLine();
            }
            catch (Exception)
            {

            }
            
        }

        public string ReadLine(StreamReader r, int timeoutMillisec)
        {
            this.r = r;
            this.line = null;
            Thread t = new Thread(DoRead);
            t.Start();

            if (t.Join(timeoutMillisec) == false)
            {
                t.Abort();
                return null;
            }

            return this.line;
        }
    }
}
