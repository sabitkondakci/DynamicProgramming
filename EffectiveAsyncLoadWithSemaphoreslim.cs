using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using namespace ConsoleDemo;

class EffectiveLoad
{
	static async Task Main()
	{
		#region string[] list
		string[] list = {
						"www.youtube.com",
						"www.facebook.com		",
						"www.baidu.com			",
						"www.yahoo.com			",
						"www.amazon.com			",
						"www.wikipedia.org		",
						"www.qq.com				",
						"www.google.co.in		",
						"www.twitter.com		 ",
						"www.live.com			",
						"www.taobao.com			",
						"www.bing.com			",
						"www.instagram.com		",
						"www.weibo.com			",
						"www.sina.com.cn		 ",
						"www.linkedin.com		",
						"www.yahoo.co.jp		 ",
						"www.msn.com			 ",
						"www.vk.com				",
						"www.google.de			",
						"www.yandex.ru			",
						"www.hao123.com			",
						"www.google.co.uk		",
						"www.reddit.com			",
						"www.ebay.com			",
						"www.google.fr			",
						"www.t.co				",
						"www.tmall.com			",
						"www.google.com.br		",
						"www.360.cn				",
						"www.sohu.com			",
						"www.amazon.co.jp		",
						"www.pinterest.com		",
						"www.netflix.com		 ",
						"www.google.it			",
						"www.google.ru			",
						"www.microsoft.com		",
						"www.google.es			",
						"www.wordpress.com		",
						"www.gmw.cn				",
						"www.tumblr.com			",
						"www.paypal.com			",
						"www.blogspot.com		",
						"www.imgur.com			",
						"www.stackoverflow.com	",
						"www.aliexpress.com		",
						"www.naver.com			",
						"www.ok.ru				",
						"www.apple.com			",
						"www.github.com			",
						"www.chinadaily.com.cn	",
						"www.imdb.com			",
						"www.google.co.kr		",
						"www.fc2.com			 ",
						"www.jd.com				",
						"www.blogger.com		 ",
						"www.163.com			 ",
						"www.google.ca			",
						"www.whatsapp.com		",
						"www.amazon.in			",
						"www.office.com			",
						"www.tianya.cn			",
						"www.google.co.id		",
						"www.youku.com			",
						"www.rakuten.co.jp		",
						"www.craigslist.org		",
						"www.amazon.de			",
						"www.nicovideo.jp		",
						"www.google.pl			",
						"www.soso.com			",
						"www.bilibili.com		",
						"www.dropbox.com		 ",
						"www.xinhuanet.com		",
						"www.outbrain.com		",
						"www.pixnet.net			",
						"www.alibaba.com		 ",
						"www.alipay.com			",
						"www.microsoftonline.com	 ",
						"www.booking.com			 ",
						"www.googleusercontent.com",
						"www.google.com.au		 ",
						"www.popads.net			 ",
						"www.cntv.cn				 ",
						"www.zhihu.com			 ",
						"www.amazon.co.uk		 ",
						"www.diply.com			 ",
						"www.coccoc.com			 ",
						"www.cnn.com				 ",
						"www.bbc.co.uk			 ",
						"www.twitch.tv			 ",
						"www.wikia.com			 ",
						"www.google.co.th		 ",
						"www.go.com				 ",
						"www.google.com.ph		 ",
						"www.doubleclick.net		 ",
						"www.onet.pl				 ",
						"www.googleadservices.com ",
						"www.accuweather.com		 ",
						"www.googleweblight.com	 ",
						"www.answers.yahoo.com    "
															};

		list = list.Select(l => l.Trim()).ToArray();
		list = list.Concat(list).ToArray();
		#endregion

		// pass a timeout accordingly
		await PrintPingList(list);
	}

	public readonly struct NetworkInfo
	{
		public string Website { get; init; }
		public IPAddress Address { get; init; }
		public IPStatus Status { get; init; }
		public long RoundTripTime { get; init; }
	}

	public static async Task PrintPingList(string[] sites, int timeout = -1)
	{
		using CancellationTokenSource source = new(timeout);

		try
		{
			// PingAsync(sites,source.Token) is to cancel running tasks
			// pingResultList.WithCancellation(source.Token) is to cancel IAsyncEnumerable's enumerator ==> IAsyncEnumerable <out T>.GetAsyncEnumerator(CancellationToken)

			var pingResultList = PingAsync(sites, source.Token);
			await Print(pingResultList, source.Token);
		}
		catch (OperationCanceledException)
		{
			Console.WriteLine("Operation has been cancelled!");
			// log it!
		}
	}

	public static async Task Print(IAsyncEnumerable<NetworkInfo> pingResultList,
	CancellationToken token = default)
	{
		await foreach (var pingResult in pingResultList
			.WithCancellation(token).ConfigureAwait(false))
		{
			Console.WriteLine($"Website:{pingResult.Website}\n"
		+ $"Address:{pingResult.Address}\nStatus:{pingResult.Status}\n"
		+ $"RoundTripTime: {pingResult.RoundTripTime}");

			Console.WriteLine();

			// catch the cancellation quicker
			if (token.IsCancellationRequested)
				throw new OperationCanceledException();
		}
	}
	public static async IAsyncEnumerable<NetworkInfo> PingAsync(string[] websites, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var length = websites.Length;
		var taskNetworkInfo = new Queue<Task<NetworkInfo>>();

		// allow 10 worker thread in at a time, this will prevent the thread exhaustion.
		using (var semaphoreTen = new SemaphoreSlim(10))
		{
			for (int i = 0; i < length; i++)
			{
				var k = i;
				NetworkInfo info = new();
				await semaphoreTen.WaitAsync(cancellationToken);

				taskNetworkInfo.Enqueue(Task.Run(async () =>
				{
					// semaphoreTen will let only 10 worker thread to get their job done at a time.
					Ping ping = new();

					try
					{
						var pingResult = await ping.SendPingAsync(websites[k], 500);

						info = new NetworkInfo
						{
							Address = pingResult.Address,
							Website = websites[k],
							RoundTripTime = pingResult.RoundtripTime,
							Status = pingResult.Status
						};
					}
					catch { /* log the errors */}
					finally
					{
						ping.Dispose();
					}

					return info;

				}, cancellationToken));

				// flush 10 items to IAsyncEnumerable at a time
				int qFlush = (i + 1) % 10;
				if (qFlush == 0 || i >= length - 10) // for the last remaining elements in taskNetworkInfo Queue
				{
					while (taskNetworkInfo.TryDequeue(out var networkInfo))
					{
						if (!cancellationToken.IsCancellationRequested)
							semaphoreTen.Release();

						yield return await networkInfo;
					}
				}
			}
		}
	}
}

