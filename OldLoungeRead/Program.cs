using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OldLoungeRead
{
    class Program
    {
        static void Main(string[] args)
        {
            OldLoungeLogReader reader = new OldLoungeLogReader();
            //            reader.Read(@"U:\\lng\vc\vclng");
            //            reader.Read(@"U:\\lng");
            //            reader.Read(@"U:\\lng\sikaku");
            //            reader.Read(@"U:\\lng\vb");
            //            reader.Read(@"U:\\lng\web");
            //            reader.Read(@"U:\\lng\java");
            //            reader.Read(@"U:\\lng\dotnet");
            //            reader.Read(@"U:\\lng\vc");
                        reader.Read(@"U:\\lng\other");
        }
    }

}

