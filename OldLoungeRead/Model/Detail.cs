using System;
using System.Collections.Generic;
using System.Text;

namespace OldLoungeRead
{
    public class Detail
    {
        public Detail()
        {
            UserId = "0"; // 匿名ユーザーを表す「0」
            Name = "";
        }
        public string Title { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string EMail { get; set; }
        public bool Solved { get; set; }
        public string CreateDatetime { get; set; }
        public string ModifiedDatetime { get; set; }
        public string HomePage { get; set; }

        public string Body { get; set; }
    }

}
