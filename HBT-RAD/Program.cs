using System;
using System.Threading;
using CoreTweet;
using System.Windows.Forms;


namespace HBT_RAD
{
    internal class Program
    {
        public static readonly DateTime StartTime = DateTime.Now;
        static private System.Timers.Timer timer;

        private static void Main(string[] args)
        {
            //var threadA = new Thread(new ThreadStart(TweetMain));
            //threadA.Start();
            ////MainAsync().Wait();

            TweetMain();
            var time = (DateTime.Now - StartTime).TotalMilliseconds;
            while (time <= 86400000.0) { Console.Write(""); }
            //Application.Run();

            //Test();
        }

        private static void TweetMain()
        {
            var tm = new TweetMain();
            timer = new System.Timers.Timer { Interval = 3600000, AutoReset = true };
            timer.Elapsed += (sender, e) =>
            {
                tm.Retweeted();
                var nnow = DateTime.Now;
                Console.Write((nnow.AddMilliseconds(3600000)).ToShortTimeString());
            };
            var a = tm.Retweeted();
            timer.Start();
        }

        //        private static async Task MainAsync()
        //        {
        //            var keys = OAuth.Tokens;
        //            var tokens = Tokens.Create(keys.ConsumerKey, keys.ConsumerSecret, keys.AccessToken, keys.AccessSecret);

        //            var message = TweetText();

        //#if DEBUG
        //            var keywords = new List<string> { "JXUG" };
        //            var connpass = new ConnpassStudyMeeting();
        //            var test = await connpass.GetStudyMeetingList(keywords);
        //#endif
        //            //Console.WriteLine(message);
        //            await tokens.Statuses.UpdateAsync(new { status = message });
        //        }

        private static void Test()
        {
            try
            {
                var keys = OAuth.Tokens;
                var tokens = Tokens.Create(keys.ConsumerKey, keys.ConsumerSecret, keys.AccessToken, keys.AccessSecret);
                //①タイムライン（自分のアカウントのツイート）を取得
                Console.WriteLine("タイムライン（自分のアカウントのツイート）を取得");
                var tl = tokens.Statuses.UserTimeline();
                foreach (var item in tl) Console.WriteLine(item.Text + "\n");

                //間を空ける
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();

                //②タイムライン（フォローしているユーザのツイート）を取得
                Console.WriteLine("タイムライン（フォローしているユーザのツイート）を取得");
                var tl2 = tokens.Statuses.HomeTimeline(include_rts => true);
                foreach (var item in tl2)
                {
                    Console.WriteLine(item.Text + "\n");
                    //Console.WriteLine(item.User.Id + "\n");
                    //Console.WriteLine(item.User.Description + "\n");

                    var rStatus = item.RetweetedStatus;

                    if (rStatus != null)
                    {
                        if (rStatus.IsRetweeted == false)
                        {
                            //ツイートIDを使ってリツイートする
                            tokens.Statuses.Retweet(id => rStatus.Id);
                            Console.WriteLine(rStatus.Id + "をリツイートしました。");
                        }

                        //いいねする
                        if (rStatus.IsFavorited == false)
                        {
                            //ツイートIDを使っていいねする
                            tokens.Favorites.Create(id => rStatus.Id);
                            Console.WriteLine(rStatus.Id + "をいいねしました。");
                        }
                    }

                    Thread.Sleep(10);
                }

                //フォローする
                //tokens.Friendships.Create(screen_name => followTarget); //followTargetに入っているIDのユーザをフォロー
                //Console.WriteLine(followTarget + "をフォローしました。");
                //フォローリストの読み込み
                var fl = tokens.Friends.List(screen_name => tokens.ScreenName); //上から20人を読みこむ
                                                                                //fl = tokens.Friends.List(user_screename => tokens.ScreenName, cursor => fl.NextCursor); //次の20人を読みこむ
                                                                                //fl = tokens.Friends.List(user_screename => tokens.ScreenName, cursor => fl.NextCursor); //さらに20人を読みこむ
                                                                                //Console.WriteLine("フォローリストの一番上のユーザ:" + fl[0].ScreenName); //フォローしたユーザを表示する

                //間を空ける
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();

                //③自分に来たリプライを取得
                Console.WriteLine("自分に来たリプライを取得");
                var reps = tokens.Statuses.MentionsTimeline(include_rts => true, count => 30);
                foreach (var item in reps)
                {
                    Console.WriteLine(item.User.ScreenName);
                    Console.WriteLine(item.Text + "\n");
                }

                Console.ReadKey();
            }
            catch (TwitterException e)
            {
                //CoreTweetに関するエラー。
                Console.WriteLine(e.Message); //メッセージを表示する
                Console.ReadKey();
            }
            catch (System.Net.WebException e)
            {
                //インターネット接続に関するエラー。
                Console.WriteLine(e.Message); //メッセージを表示する
                Console.ReadKey();
            }
        }
    }
}