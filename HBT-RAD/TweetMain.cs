using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoreTweet;
using static HBT_RAD.OAuth;

namespace HBT_RAD
{
    public class TweetMain
    {
        private Keys keys = OAuth.Tokens;
        private Tokens Tokens => CoreTweet.Tokens.Create(keys.ConsumerKey, keys.ConsumerSecret, keys.AccessToken, keys.AccessSecret);

        private int Count = 0;

        private Random rand = new Random();

        public async Task Retweeted()
        {
            try
            {
                var tl2 = Tokens.Statuses.HomeTimeline(include_rts => true);
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
                            if (Count < 2400)
                            {
                                //ツイートIDを使ってリツイートする
                                await Tokens.Statuses.RetweetAsync(id => rStatus.Id);
                                Console.WriteLine(rStatus.Id + "をリツイートしました。");
                                Count++;
                            }
                        }

                        //いいねする
                        if (rStatus.IsFavorited == false)
                        {
                            if (Count < 2400)
                            {
                                //ツイートIDを使っていいねする
                                await Tokens.Favorites.CreateAsync(id => rStatus.Id);
                                Console.WriteLine(rStatus.Id + "をいいねしました。");
                                Count++;
                            }
                        }
                    }

                    Thread.Sleep(rand.Next(100, 1000));
                }
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
