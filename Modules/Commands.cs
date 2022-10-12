using Discord.Commands;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Red_Alert.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        /*
        [Command("Test")]
        public async Task Test()
        {
            await ReplyAsync("Test Completed!");
        }
        */

        [Command("ping")]
        public Task ping()
        {
            System.Diagnostics.Process procces = System.Diagnostics.Process.GetCurrentProcess();
            System.Diagnostics.ProcessThreadCollection threadCollection = procces.Threads;

            string threads = string.Empty;

            foreach (System.Diagnostics.ProcessThread proccessThread in threadCollection)
            {
                threads += string.Format("Thread Id: {0}, ThreadState: {1}\r\n", proccessThread.Id, proccessThread.ThreadState);
            }

            ReplyAsync(threads);
            return Task.CompletedTask;
        }

        [Command("shut_down")]
        public Task shut_down()
        {
            _ = Task.Run(async () =>
            {
                await ReplyAsync("Shuting down...");
                Environment.Exit(1);
            });
            return Task.CompletedTask;
        }

        [Command("restart")]
        public Task restart()
        {
            _ = Task.Run(async () =>
           {
               Console.WriteLine("Restarting...\n");

               Task.Delay(1000).Wait(1000);
               await ReplyAsync("Restarting...");
               //Start process, friendly name is something like MyApp.exe (from current bin directory)
               System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.FriendlyName);

               //Close the current process
               Environment.Exit(0);
           });
            return Task.CompletedTask;
        }

        [Command("log")]
        public Task log()
        {
            Program program = new Program();
            _ = Task.Run(async () =>
           {
               string date = Convert.ToString(DateTime.Now);
               date = date.Replace('/', '_').Replace(':', '_').Replace(' ', '_');

               using (StreamWriter file = File.CreateText(@"logs/log_" + date + ".txt"))
               {
                   file.Write(program.log_);
                   Console.WriteLine("Saved log at: " + @"logs/log" + date + ".txt");
               }
               await ReplyAsync("*Saved log at: " + @"logs/log" + date + ".txt*\n\n" + program.log_);
           });
            return Task.CompletedTask;
        }
    }
}