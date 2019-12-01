using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace OldLoungeRead
{
    class OldLoungeLogReader
    {
        int topicId = 9365;
        int postId = 59216;
        string forumId = "23";
        string tags = "C#";

        /* forumId
        プログラミング = 23
        情報処理技術者試験 = 25
        */

        public void Read(string targetDir)
        {
            // 指定フォルダに存在するファイル名を取得
            string[] files = Directory.GetFiles(targetDir, "*.txt", System.IO.SearchOption.AllDirectories);

            WpForoWriter wpForoWriter = new WpForoWriter(topicId, postId, forumId, tags);

            foreach (string filePath in files)
            {
                Console.WriteLine(filePath);
                // 旧ラウンジファイルの読み込み
                var list = ReadLog(filePath);
                if (list.Count == 0) continue;
                // wpForoインポートファイル書き込み
                wpForoWriter.Write(list);
            }
        }

        // readLogでTopic１つとPostがｎ個作成される。
        private List<Detail> ReadLog(string filePath)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var detail = new Detail();
            var list = new List<Detail>();
            using (StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding("shift_jis")))
            {
                try
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        // スレットタイトル
                        if (line.StartsWith("Subject: "))
                        {
                            detail.Title = line.Remove(0, "Subject: ".Length);
                        }
                        // 投稿者名
                        else if (line.StartsWith("From: "))
                        {
                            detail.Name = line.Remove(0, "From: ".Length);
                        }
                        // メール
                        else if (line.StartsWith("E-Mail: "))
                        {
                            detail.EMail = line.Remove(0, "E-Mail: ".Length);
                        }
                        // ホームページ
                        else if (line.StartsWith("HomePage: "))
                        {
                            detail.HomePage = line.Remove(0, "HomePage: ".Length);
                        }
                        // 投稿日
                        else if (line.StartsWith("Date: "))
                        {
                            detail.CreateDatetime = TimeZoneInfo.ConvertTimeToUtc(DateTime.Parse(line.Remove(0, "Date: ".Length))).ToString();
                            detail.ModifiedDatetime = detail.CreateDatetime;
                        }
                        // 解決
                        else if (line.StartsWith("Solved: on"))
                        {
                            detail.Solved = true;
                        }
                        // コメント開始前の空行
                        else if (line == "")
                        {
                            if (detail.Body != null)
                            {
                                detail.Body += "<br/ >";
                            }
                        }
                        // コメント終了
                        else if (line == "========================================")
                        {
                            // コメントが存在して線がある場合は１投稿終了
                            if (detail.Body != null)
                            {
                                if (string.IsNullOrEmpty(detail.EMail))
                                {
                                    // emailが空の場合は、投稿者名をいれる。（wpforoのバグ対応。メールに差がないと投稿者とレスの名前が同じ表示されてしまう）
                                    detail.EMail = detail.Name;
                                }

                                list.Add(detail);
                                detail = new Detail();
                            }
                        }
                        else
                        {
                            detail.Body += line + "<br/ >";
                        }
                    }
                }
                finally
                {
                    if (sr != null)
                    {
                        sr.Close();
                    }
                }
            }
            list.Add(detail);
            return list;
        }
    }
}
