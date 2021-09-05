using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VitalyNovitskyLotteryPOC.Common.DTOs;

namespace LotteryTester
{
    class Program
    {
        const string BaseUrl = "http://localhost:11270/";

        static Random _randomizer;
        static HttpClient _httpClient;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            _randomizer = new Random();
            _httpClient = new HttpClient();

            Console.ReadKey();

            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            var averagePlay1Task = TestPlayLottery(500, token);
            var averagePlay2Task = TestPlayLottery(500, token);
            var averagePlay3Task = TestPlayLottery(500, token);
            var averagePlay4Task = TestPlayLottery(500, token);
            var averagePlay5Task = TestPlayLottery(500, token);
            var getWinnerTask = TestGetWinner(50, token);
            var deleteWinnerTask = TestDeleteWinner(50, token);

            await Task.Run(() => { Thread.Sleep(60 * 1000); });

            tokenSource.Cancel();

            Task.WaitAll(new[]
            {
                averagePlay1Task,
                averagePlay2Task,
                averagePlay3Task,
                averagePlay4Task,
                averagePlay5Task,
                getWinnerTask,
                deleteWinnerTask
            });

            var averagePlay = (averagePlay1Task.Result
                + averagePlay2Task.Result
                + averagePlay3Task.Result
                + averagePlay4Task.Result
                + averagePlay5Task.Result) / 5;

            var averageGet = getWinnerTask.Result;

            var averageDelete = deleteWinnerTask.Result;

            Console.WriteLine($"Finished, averagePlay = {averagePlay}, averageGet = {averageGet}, averageDelete = {averageDelete}");

            Console.ReadKey();
        }

        static async Task<int> TestPlayLottery(int iterationsNumber, CancellationToken token)
        {
            var sleepTime = 0;
            var stopWatch = new Stopwatch();

            var i = 0;

            for (i = 0; i < iterationsNumber && !token.IsCancellationRequested; i++)
            {
                stopWatch.Start();
                Console.WriteLine(await PlayLottery());
                stopWatch.Stop();

                lock (_randomizer)
                    sleepTime = _randomizer.Next(0, 100);

                Thread.Sleep(sleepTime);
            }

            return (int)stopWatch.ElapsedMilliseconds / i;
        }

        private async static Task<string> PlayLottery()
        {
            var playLotteryRequest = GeneratePlayLotteryRequest();

            var content = new StringContent(JsonConvert.SerializeObject(playLotteryRequest), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{BaseUrl}LotteryUser/PlayLottery", content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else return $"Failed, code {response.StatusCode}";
        }

        private static PlayLotteryAttemptRequestDTO GeneratePlayLotteryRequest()
        {
            int ticketsNumber;
            int firstLetterIndex;
            int secondLetterIndex;

            lock (_randomizer)
            {
                ticketsNumber = _randomizer.Next(1, 10);
                firstLetterIndex = _randomizer.Next(65, 90);
                secondLetterIndex = _randomizer.Next(97, 122);
            }

            return new PlayLotteryAttemptRequestDTO
            {
                NumberOfTickets = ticketsNumber,
                UserName = $"{(char)firstLetterIndex}{(char)secondLetterIndex}"
            };
        }

        static async Task<int> TestGetWinner(int iterationsNumber, CancellationToken token)
        {
            var sleepTime = 0;
            var stopWatch = new Stopwatch();

            var i = 0;

            for (i = 0; i < iterationsNumber && !token.IsCancellationRequested; i++)
            {
                stopWatch.Start();
                Console.WriteLine(await GetWinner());
                stopWatch.Stop();

                lock (_randomizer)
                    sleepTime = _randomizer.Next(2000, 5000);

                Thread.Sleep(sleepTime);
            }

            return (int)stopWatch.ElapsedMilliseconds / i;
        }

        private async static Task<string> GetWinner()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}LotteryAdministrator/GetWinner");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();
            else return $"Failed, code {response.StatusCode}";
        }

        static async Task<int> TestDeleteWinner(int iterationsNumber, CancellationToken token)
        {
            var sleepTime = 0;
            var stopWatch = new Stopwatch();

            var i = 0;

            for (i = 0; i < iterationsNumber && !token.IsCancellationRequested; i++)
            {
                stopWatch.Start();
                Console.WriteLine(await DeleteWinner());
                stopWatch.Stop();

                lock (_randomizer)
                    sleepTime = _randomizer.Next(5000, 10000);

                Thread.Sleep(sleepTime);
            }

            return (int)stopWatch.ElapsedMilliseconds / i;
        }

        private async static Task<string> DeleteWinner()
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}LotteryAdministrator/RemoveWinner");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();
            else return $"Failed, code {response.StatusCode}";
        }
    }
}
