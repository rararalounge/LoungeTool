using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace OldLoungeRead
{
    class WpForoWriter
    {
        private int topicId;
        private int postId;
        private string forumId;
        private string topicFilePath;
        private string postFilePath;
        private string tags;

        private List<string> titleList = new List<string>();

        public WpForoWriter(int topicId, int postId, string forumId, string tags)
        {
            this.topicId = topicId;
            this.postId = postId;
            this.forumId = forumId;
            this.topicFilePath = Path.Combine(Environment.CurrentDirectory, "topic.csv");
            this.postFilePath = Path.Combine(Environment.CurrentDirectory, "post.csv");
            this.tags = tags;
            File.Delete(this.topicFilePath);
            File.Delete(this.postFilePath);
        }

        /// <summary>
        /// トピック毎に、topic1件postｎ件ファイルを作成する。
        /// </summary>
        /// <param name="list"></param>
        public void Write(List<Detail> list)
        {
            this.WriteTopicFile(list);
            this.WritePostFile(list);
        }

        public string GetMultiSlug(string title)
        {
            // 指定のタイトルが既に存在するか
            int count = this.titleList.Where(n => n == title).Count();
            // 既に存在していたらslugが被らないように「-n」をつける
            return count == 1 ? "" : string.Format("-{0}", count);
        }

        private void WriteTopicFile(List<Detail> list)
        {
            this.titleList.Add(list[0].Title);

            // topicファイル作成
            using (StreamWriter topicFile = new StreamWriter(topicFilePath, true, new UTF8Encoding(false)))
            {
                try
                {
                    // 区切り文字は「`」
                    string line = string.Format("\"{0}\"`\"{1}\"`\"{2}\"`\"{3}\"`\"{4}\"`\"{5}\"`\"{6}\"`\"{7}\"`\"{8}\"`\"{9}\"`\"{10}\"`\"{11}\"`\"{12}\"`\"{13}\"`\"{14}\"`\"{15}\"`\"{16}\"`\"{17}\"`\"{18}\"`\"{19}\"`\"{20}\"`\"{21}\"`\"{22}\"`\"{23}\"`\"{24}\"",

                    this.topicId, // 0:topicid
                    this.forumId, // 1:forumid 
                    this.postId, // 2:first_postid
                    list[0].UserId, // 3:userid
                    list[0].Title, // 4:title
                    Uri.EscapeDataString(list[0].Title) + this.GetMultiSlug(list[0].Title), // 5:slug
                    list[0].CreateDatetime, // 6:created
                    list[list.Count - 1].CreateDatetime, // 7:modified
                    this.postId + list.Count - 1, // 8:last_post
                    list.Count, // 9:posts
                    "0", // 10:votes
                    "0", // 11:answers
                    "0", // 12:views
                    "", // 13:meta_key
                    "", // 14:meta_desc
                    "0", // 15:type
                    IsSolved(list) ? "1" : "0", // 16:solved 未解決 0 解決 1
                    "0", // 17:closed 初期 0 クローズ 1
                    "0", // 18:has_attach
                    "0", // 19:private
                    "0", // 20:status
                    list[0].Name, // 21:name
                    list[0].EMail, // 22:email
                    "", // 23:prefix
                    this.tags // 24:tags
                    );

                    topicFile.WriteLine(line);
                }
                finally
                {
                    if (topicFile != null)
                    {
                        topicFile.Close();
                    }
                }
            }
        }

        private void WritePostFile(List<Detail> list)
        {
            // postファイル作成
            using (StreamWriter postFile = new StreamWriter(postFilePath, true, new UTF8Encoding(false)))
            {
                try
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        var detail = list[i];

                        // 区切り文字は「`」
                        string line = string.Format("\"{0}\"`\"{1}\"`\"{2}\"`\"{3}\"`\"{4}\"`\"{5}\"`\"{6}\"`\"{7}\"`\"{8}\"`\"{9}\"`\"{10}\"`\"{11}\"`\"{12}\"`\"{13}\"`\"{14}\"`\"{15}\"`\"{16}\"`\"{17}\"",

                        this.postId,   // 0:postid
                        "0", // 1:parentid
                        this.forumId, // 2:forumid
                        this.topicId, // 3:topicid
                        "0", // 4:userid 匿名ユーザーは0
                        detail.Title, // 5:title
                        "<p>" + detail.Body + "<p>", // 6:body
                        detail.CreateDatetime, // 7:created
                        detail.ModifiedDatetime, // 8:modified
                        "0", // 9:likes
                        "0", // 10:votes
                        "0", // 11:is_answer
                        i == 0 ? "1" : "0", // 12:is_first_post
                        "0", // 13:status
                        detail.Name, // 14:name
                        detail.EMail, // 15:email
                        "0", // 16:private
                        "-1" // 17:root
                        );

                        postFile.WriteLine(line);
                        this.postId++;
                    }
                }
                finally
                {
                    if (postFile != null)
                    {
                        postFile.Close();
                    }
                }
                this.topicId++;
            }
        }

        private bool IsSolved(List<Detail> list)
        {
            return list.Where(n => n.Solved).Count<Detail>() > 0;
        }
    }
}
