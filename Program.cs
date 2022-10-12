using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Red_Alert
{
    public class Program
    {
        public List<Alert> alert;
        public List<AlertHistory> json;
        public List<Image> image;
        private string location = "";
        private string date = "";
        private string title = "";
        private string location_ = "";
        private string date_ = "";
        private string title_ = "";
        private string website = "";
        private string website_ = "";
        private string alert_json = "";
        private string alert_json_ = "";
        private string desc_ = "";
        private ulong slash_command;
        private string id_string;
        private ulong user_id;
        public string log_;
        public string DMS = "OFF";
        public string image_url;
        public string api_key;
        public string image_json;
        public string lat = "";
        public string lng = "";

        private ulong my_id = 292770890792566784; //change this for control over the bot

        private static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        public DiscordSocketClient _client;
        public CommandService _commands;
        public IServiceProvider _services;
        private SocketGuild arg;

        public async Task RunBotAsync()
        {
            ////////////////////////////////////////////////////
            Console.Title = "Red Alert Discord Bot";
            ////////////////////////////////////////////////////
            await Commends();

            var socketConfig = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.All,
            };

            _client = new DiscordSocketClient(socketConfig);

            _commands = new CommandService();
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            string
                Token = "YOUR DISCORD BOT TOKEN GOES HERE";

            _client.ButtonExecuted += MyButtonHandler;

            _client.Ready += deadlock;

            _client.Ready += ServerNumber;

            _client.Ready += RedAlert_Id;

            //_client.Ready += GetImage_stage1;
            //_client.Ready += GetImage_stage2;

            _client.Ready += Client_Ready;

            //_client.Ready += deadlock;

            //_client.Ready += RedAlert_Invite;

            _client.SelectMenuExecuted += MyMenuHandler;

            _client.Ready += Azaka;

            _client.ModalSubmitted += BugFeature;

            _client.MessageReceived += _client_MessageReceived;

            _client.SlashCommandExecuted += SlashCommandHandler;

            _client.LeftGuild += _client_LeftGuild;

            _client.JoinedGuild += RedAlert_Role;

            //await Task.Run(Azaka);

            await RegisterCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, Token);

            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task<Task> _client_LeftGuild(SocketGuild arg)
        {
            await RedAlert_Id();
            return Task.CompletedTask;
        }

        private Task _client_MessageReceived(SocketMessage arg)
        {
            if (DMS == "ON")
            {
                if (arg.Author.IsBot == false)
                {
                    log_ = log_ + "\n" + (new LogMessage(LogSeverity.Info, "Message",
                        arg.Author.Username + " " + arg.Author.Id + " sent message: " + arg));
                    Console.WriteLine(new LogMessage(LogSeverity.Info, "Message",
                        arg.Author.Username + " sent message: " + arg));
                }
            }

            return Task.CompletedTask;
        }

        public Task log(LogMessage arg)
        {
            Console.WriteLine(arg);
            log_ = log_ + "\n" + arg;

            return Task.CompletedTask;
        }

        public Task deadlock()
        {
            _ = Task.Run(async () =>
            {
                ///////////////////////////////////////////
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                log_ = log_ + "\n" + (new LogMessage(LogSeverity.Info, "Gateway", "Started deadlock protection."));
                Console.WriteLine(new LogMessage(LogSeverity.Info, "Gateway", "Started deadlock protection."));
                ///////////////////////////////////////////

                var connTime = 0;
                while (true)
                {
                    if (_client.ConnectionState != ConnectionState.Connected)
                    {
                        connTime++;
                        if (connTime.Equals(30))
                        {
                            ///////////////////////////////////////////
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.ForegroundColor = ConsoleColor.White;
                            ///////////////////////////////////////////

                            // if only we could call the log event method ourselves without compile error.
                            log_ = log_ + "\n" + (new LogMessage(LogSeverity.Critical, "Gateway",
                                "Has not been connected for 30 seconds. Assuming deadlock in connector."));
                            Console.WriteLine(new LogMessage(LogSeverity.Critical, "Gateway",
                                "Has not been connected for 30 seconds. Assuming deadlock in connector."));
                            Task.Delay(1000).Wait(1000);
                            // add log function here
                            string date = Convert.ToString(DateTime.Now);
                            date = date.Replace('/', '_').Replace(':', '_').Replace(' ', '_');

                            using (StreamWriter file = File.CreateText(@"logs/log_" + date + ".txt"))
                            {
                                file.Write(log_);
                                Console.WriteLine("Saved log at: " + @"logs/log" + date + ".txt");
                            }

                            System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.FriendlyName);
                            //Close the current process

                            Environment.Exit(0);
                        }
                    }
                    else if (_client.ConnectionState == ConnectionState.Connected && !connTime.Equals(0))
                    {
                        connTime = 0;
                    }

                    await Task.Delay(1000).ConfigureAwait(false);
                }
            });
            ///////////////////////////////////////////
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            ///////////////////////////////////////////
            ///
            return Task.CompletedTask;
        }

        public Task Commends()
        {
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    var cm = Console.ReadLine();
                    if (cm != null)
                    {
                        log_ = log_ + "\n" + (new LogMessage(LogSeverity.Info, "Command", cm));
                        switch (cm)
                        {
                            case ("Remove Role"):
                                try
                                {
                                    Console.WriteLine(
                                        "Please notice! if the Red Alert bot role is lower in prority then the one you want to get you wont be able to remove the role! ");
                                    Console.WriteLine("Enter guild ID: ");
                                    UInt64 GUILD_ = Convert.ToUInt64(Console.ReadLine());
                                    var guild = _client.GetGuild(GUILD_);
                                    Console.WriteLine("Enter user ID: ");
                                    UInt64 ID_ = Convert.ToUInt64(Console.ReadLine());
                                    var user = guild.GetUser(ID_);
                                    Console.WriteLine(user.Username);
                                    Console.WriteLine("Would you like to serch role by ID?\n Yes/No");
                                    string choise = Console.ReadLine();

                                    if (choise == "Yes")
                                    {
                                        Console.WriteLine("Enter role ID: ");
                                        UInt64 RoleID = Convert.ToUInt64(Console.ReadLine());
                                        var role = guild.GetRole(RoleID);

                                        Console.WriteLine(role.Name);
                                        Console.WriteLine("If you agree to remove " + user.Username + " the role " +
                                                          role.Name + " please press enter.");
                                        Console.ReadKey();

                                        await ((SocketGuildUser)user).RemoveRoleAsync(role);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Enter role name: ");
                                        string RoleName = Console.ReadLine();
                                        var role = guild.Roles.FirstOrDefault(x => x.Name == RoleName);
                                        ;

                                        Console.WriteLine(role.Name);
                                        Console.WriteLine("If you agree to remove " + user.Username + " the role " +
                                                          role.Name + " please press enter.");
                                        Console.ReadKey();

                                        await ((SocketGuildUser)user).RemoveRoleAsync(role);
                                    }

                                    Console.WriteLine("Role removed!");
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Could Not add role :( ");
                                    Console.WriteLine(e);
                                }

                                break;

                            case ("Role"):
                                try
                                {
                                    Console.WriteLine(
                                        "Please notice! if the Red Alert bot role is lower in prority then the one you want to get you wont be able to add the role! ");
                                    Console.WriteLine("Enter guild ID: ");
                                    UInt64 GUILD_ = Convert.ToUInt64(Console.ReadLine());
                                    var guild = _client.GetGuild(GUILD_);
                                    Console.WriteLine("Enter user ID: ");
                                    UInt64 ID_ = Convert.ToUInt64(Console.ReadLine());
                                    var user = guild.GetUser(ID_);
                                    Console.WriteLine(user.Username);
                                    Console.WriteLine("Would you like to serch role by ID?\n Yes/No");
                                    string choise = Console.ReadLine();

                                    if (choise == "Yes")
                                    {
                                        Console.WriteLine("Enter role ID: ");
                                        UInt64 RoleID = Convert.ToUInt64(Console.ReadLine());
                                        var role = guild.GetRole(RoleID);

                                        Console.WriteLine(role.Name);
                                        Console.WriteLine("If you agree to give " + user.Username + " the role " +
                                                          role.Name + " please press enter.");
                                        Console.ReadKey();

                                        await ((SocketGuildUser)user).AddRoleAsync(role);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Enter role name: ");
                                        string RoleName = Console.ReadLine();
                                        var role = guild.Roles.FirstOrDefault(x => x.Name == RoleName);
                                        ;

                                        Console.WriteLine(role.Name);
                                        Console.WriteLine("If you agree to give " + user.Username + " the role " +
                                                          role.Name + " please press enter.");
                                        Console.ReadKey();

                                        await ((SocketGuildUser)user).AddRoleAsync(role);
                                    }

                                    Console.WriteLine("Role granted!");
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Could Not add role :( ");
                                    Console.WriteLine(e);
                                }

                                break;

                            case ("Exit"):
                                Environment.Exit(0);
                                break;

                            case ("Servers"):
                                for (int i = 0; i < _client.Guilds.Count; i++)
                                {
                                    var guild = _client.Guilds.ToList()[i] as SocketGuild;

                                    var user_ = _client.GetUserAsync(my_id).Result;

                                    await UserExtensions.SendMessageAsync(user_, "Name: " + guild.Name.ToString() + " - ID: " + guild.Id);
                                }

                                break;

                            case ("Create Role"):
                                await RedAlert_Role(arg);
                                break;

                            case ("User"):
                                Console.WriteLine("Please Enter User ID: ");
                                ulong user_id = Convert.ToUInt64(Console.ReadLine());
                                try
                                {
                                    var user_ = _client.GetUserAsync(user_id).Result;
                                    Console.WriteLine("Please Enter User Messgae: ");
                                    string MessageUser = Console.ReadLine();
                                    await UserExtensions.SendMessageAsync(user_, MessageUser);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                }

                                break;

                            case ("Log"):
                                string date = Convert.ToString(DateTime.Now);
                                date = date.Replace('/', '_').Replace(':', '_').Replace(' ', '_');

                                using (StreamWriter file = File.CreateText(@"logs/log_" + date + ".txt"))
                                {
                                    file.Write(log_);
                                    Console.WriteLine("Saved log at: " + @"logs/log" + date + ".txt");
                                }

                                break;

                            case ("Client_Ready()"):
                                await Client_Ready();
                                break;

                            case ("Role Reset"):
                                ///////////////////////////////////////////
                                Console.WriteLine();
                                Console.BackgroundColor = ConsoleColor.Green;
                                Console.ForegroundColor = ConsoleColor.White;
                                log_ = log_ + "\n" +
                                       (new LogMessage(LogSeverity.Info, "Role", "Reseting Roles!     \n"));
                                Console.WriteLine(new LogMessage(LogSeverity.Info, "Role", "Reseting Roles!     \n"));
                                ///////////////////////////////////////////
                                for (int i = 0; i < _client.Guilds.Count; i++)
                                {
                                    var guild = _client.Guilds.ToList()[i] as SocketGuild;
                                    try
                                    {
                                        var role_ = guild.Roles.FirstOrDefault(x =>
                                            x.Name == "Red Alert Notifications");
                                        var role = guild.GetRole(role_.Id);
                                        await role.DeleteAsync();

                                        log_ = log_ + "\n" + (new LogMessage(LogSeverity.Info, "Role",
                                            "Removed role for: " + guild.Id));
                                        Console.WriteLine(new LogMessage(LogSeverity.Info, "Role",
                                            "Removed role for: " + guild.Id));
                                    }
                                    catch (Exception)
                                    {
                                        log_ = log_ + "\n" + (new LogMessage(LogSeverity.Info, "Role",
                                            "Role problem exists at: " + guild.Id));
                                        Console.WriteLine(new LogMessage(LogSeverity.Info, "Role",
                                            "Role problem exists at: " + guild.Id));
                                    }
                                }

                                break;

                            case ("DMS"):
                                if (DMS == "OFF")
                                {
                                    DMS = "ON";
                                    Console.WriteLine("Now accepting dms!");
                                }
                                else
                                {
                                    DMS = "OFF";
                                    Console.WriteLine("Now dont accepting dms!");
                                }

                                break;

                            case ("Restart"):
                                Console.WriteLine("Restarting...\n");
                                Task.Delay(1000).Wait(1000);
                                //Start process, friendly name is something like MyApp.exe (from current bin directory)
                                System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.FriendlyName);

                                //Close the current process
                                Environment.Exit(0);
                                break;

                            case ("Message"):
                                Console.WriteLine("What Is The Message You Would Like To Send?");
                                string message = Console.ReadLine();
                                await RedAlert_SendMessage(message);
                                break;

                            case ("Channel"):
                                Console.WriteLine("What Is The Message You Would Like To Send?");
                                string message_ = Console.ReadLine();

                                Console.WriteLine("To Where? \n(Channel ID)");
                                ulong id__ = Convert.ToUInt64(Console.ReadLine());

                                var chnl_ = _client.GetChannel(id__) as IMessageChannel;
                                await chnl_.SendMessageAsync(message_);
                                Console.WriteLine("Done!");
                                break;

                            case ("ID"):
                                ///////////////////////////////////////////
                                Console.WriteLine();
                                Console.BackgroundColor = ConsoleColor.Green;
                                Console.ForegroundColor = ConsoleColor.White;
                                log_ = log_ + "\n" + (new LogMessage(LogSeverity.Info, "ID", "Checking IDs!     \n"));
                                Console.WriteLine(new LogMessage(LogSeverity.Info, "ID", "Checking IDs!     \n"));
                                ///////////////////////////////////////////

                                string id_string;
                                List<string> list = new List<string>();

                                using (StreamReader file = File.OpenText(@"id.txt"))
                                {
                                    id_string = file.ReadToEnd();
                                }

                                string[] ids = id_string.Split(',');

                                for (int id_ = 1; id_ < ids.Length;)
                                {
                                    ulong result = Convert.ToUInt64(ids[id_]);

                                    var chnl = _client.GetChannel(result) as IMessageChannel;

                                    try
                                    {
                                        if (chnl != null)
                                        {
                                            //append
                                            list.Add("," + result);
                                            log_ = log_ + "\n" + (new LogMessage(LogSeverity.Info, "ID",
                                                Convert.ToString(result)));
                                            Console.WriteLine(new LogMessage(LogSeverity.Info, "ID",
                                                Convert.ToString(result)));
                                        }
                                        else
                                        {
                                            log_ = log_ + "\n" + (new LogMessage(LogSeverity.Info, "ID",
                                                "Channel " + result +
                                                " Cant be accessed, \n deleting channel from database now!"));
                                            Console.WriteLine(new LogMessage(LogSeverity.Info, "ID",
                                                "Channel " + result +
                                                " Cant be accessed, \n deleting channel from database now!"));
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        log_ = log_ + "\n" + (new LogMessage(LogSeverity.Info, "ID",
                                            "Channel " + result + e));
                                        Console.WriteLine(new LogMessage(LogSeverity.Info, "ID",
                                            "Channel " + result + e));
                                    }

                                    id_++;
                                    //await Task.Delay(50);
                                }

                                using (StreamWriter file = File.CreateText(@"id.txt"))
                                {
                                    foreach (string id in list)
                                    {
                                        file.Write(id);
                                    }
                                }

                                log_ = log_ + "\n" + (new LogMessage(LogSeverity.Info, "ID", "Done checking IDs!\n"));
                                Console.WriteLine(new LogMessage(LogSeverity.Info, "ID", "Done checking IDs!\n"));
                                Console.WriteLine();
                                ///////////////////////////////////////////

                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.White;

                                ///////////////////////////////////////////
                                break;

                            default:
                                Console.WriteLine("Error! not a valid command.");
                                break;
                        }
                    }
                }
            });
            return Task.CompletedTask;
        }

        public async Task SendEmbedAsync()
        {
            string website_l;
            string image_url__ = "";
            using (WebClient clinet = new WebClient())
            {
                clinet.Headers.Add("user-agent",
                    "Mozilla / 5.0(Windows NT 10.0; Win64; x64; rv: 99.0) Gecko / 20100101 Firefox / 99.0");
                try
                {
                    ////////////////////////////////////////////
                    //Console.WriteLine("Still Looking....");
                    ////////////////////////////////////////////

                    json = JsonConvert.DeserializeObject<List<AlertHistory>>(
                        clinet.DownloadString("https://www.oref.org.il/WarningMessages/History/AlertsHistory.json"));

                    location = (json[0].data);
                    date = ("Date: " + json[0].alertDate);
                    title = ("Title: " + json[0].title);
                    website = (@"~~" + "https://www.google.com/maps/search/" + json[0].data.Replace(" ", "_") + "~~");
                }
                catch (Exception)
                {
                    try
                    {
                        var chnl = _client.GetChannel(slash_command) as IMessageChannel;
                        await chnl.SendMessageAsync("Could Not Find Last Alert.... :( ");
                    }
                    catch
                    {
                        string msg = "Could Not Find Last Alert.... :(";

                        // Get the user with the ID from your DiscordSocketClient
                        var user = _client.GetUserAsync(user_id).Result;

                        await UserExtensions.SendMessageAsync(user, msg);
                    }
                }
            }

            string newlocation_;

            if (json[0].data[0] == ' ')
            {
                newlocation_ = json[0].data.Remove(0, json[0].data.Length - 1);
            }
            else
            {
                newlocation_ = json[0].data;
            }

            string wae = newlocation_;

            foreach (var VARIABLE in image)
            {
                if (VARIABLE.name.Contains(newlocation_))
                {
                    lat = VARIABLE.lat;
                    lng = VARIABLE.lng;

                    website_l =
                       $"https://dev.virtualearth.net/REST/V1/Imagery/Metadata/Road/{lat},{lng}?zl=13&o=xml&key={api_key}";

                    string website_lo;
                    using (WebClient clinet = new WebClient())
                    {
                        clinet.Headers.Add("user-agent",
                            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_13_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.100 Safari/537.36");

                        website_lo = clinet.DownloadString(website_l);
                    }

                    //<ImageUrl>http://ecn.t3.tiles.virtualearth.net/tiles/a032010110123333.jpeg?g=12552</ImageUrl>
                    int one_ = website_lo.IndexOf(@"<ImageUrl>");
                    int two_ = website_lo.IndexOf(@"</ImageUrl>");

                    image_url__ = website_lo.Substring(one_ + 10, two_ - one_ - 10);
                }
            }

            if (image_url__ == "")
            {
                image_url__ =
                    "https://cdn.discordapp.com/attachments/965774763396042763/1005619855849947297/image.png";
            }

            var embed = new EmbedBuilder()
            {
                ImageUrl = image_url__
            }
                .WithTitle("*Red Alert*")
                .WithDescription("Most Recent Alert Was At:\n" + location + "\n" + date + "\n" + title +
                                 "\n 📢 <:MISSILES:945771593676779601>" + "\n" + website)
                .WithColor(Color.DarkRed);

            if (location != null && location != "")
            {
                try
                {
                    var chnl = _client.GetChannel(slash_command) as IMessageChannel;
                    await chnl.SendMessageAsync(embed: embed.Build());
                }
                catch
                {
                    // Get the user with the ID from your DiscordSocketClient
                    var user = _client.GetUserAsync(user_id).Result;

                    await UserExtensions.SendMessageAsync(user, embed: embed.Build());
                }
            }
            else
            {
                try
                {
                    var chnl = _client.GetChannel(slash_command) as IMessageChannel;
                    await chnl.SendMessageAsync("Could Not Find Last Alert.... :( ");
                }
                catch
                {
                    string msg = "Could Not Find Last Alert.... :(";

                    // Get the user with the ID from your DiscordSocketClient
                    var user = _client.GetUserAsync(user_id).Result;

                    await UserExtensions.SendMessageAsync(user, msg);
                }
            }
            //Your embed needs to be built before it is able to be sent
        }

        public async Task Client_Ready()
        {
            _ = Task.Run(async () =>
                {
                    var globalCommand = new SlashCommandBuilder();
                    globalCommand.WithName("set-up");
                    globalCommand.WithDescription(
                        "Preform This Command In Order To Set This Chanel As Alert Channel!📢 <:MISSILES:945771593676779601>");

                    try
                    {
                        // With global commands we don't need the guild.
                        await _client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
                        // Using the ready event is a simple implementation for the sake of the example. Suitable for testing and development.
                        // For a production bot, it is recommended to only run the CreateGlobalApplicationCommandAsync() once for each command.
                    }
                    catch (HttpException exception)
                    {
                        // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message. You can serialize the Error field in the exception to get a visual of where your error is.
                        var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);

                        // You can send this error somewhere or just print it to the console, for this example we're just going to print it.
                        Console.WriteLine(json);
                    }

                    globalCommand.WithName("get-notifications");
                    globalCommand.WithDescription(
                        "Preform This Command In Order To Get Notifications Every Time There Is A Red Alert.📢");

                    try
                    {
                        // With global commands we don't need the guild.
                        await _client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
                        // Using the ready event is a simple implementation for the sake of the example. Suitable for testing and development.
                        // For a production bot, it is recommended to only run the CreateGlobalApplicationCommandAsync() once for each command.
                    }
                    catch (HttpException exception)
                    {
                        // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message. You can serialize the Error field in the exception to get a visual of where your error is.
                        var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);

                        // You can send this error somewhere or just print it to the console, for this example we're just going to print it.
                        Console.WriteLine(json);
                    }

                    globalCommand.WithName("disable-notifications");
                    globalCommand.WithDescription(
                        "Preform This Command In Order To Remove Notifications On This Server.📢 ");

                    try
                    {
                        // With global commands we don't need the guild.
                        await _client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
                        // Using the ready event is a simple implementation for the sake of the example. Suitable for testing and development.
                        // For a production bot, it is recommended to only run the CreateGlobalApplicationCommandAsync() once for each command.
                    }
                    catch (HttpException exception)
                    {
                        // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message. You can serialize the Error field in the exception to get a visual of where your error is.
                        var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);

                        // You can send this error somewhere or just print it to the console, for this example we're just going to print it.
                        Console.WriteLine(json);
                    }

                    globalCommand.WithName("recent-alert");
                    globalCommand.WithDescription("Preform This Command In Order To Get The Most Recent Alert!");

                    try
                    {
                        // With global commands we don't need the guild.
                        await _client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
                        // Using the ready event is a simple implementation for the sake of the example. Suitable for testing and development.
                        // For a production bot, it is recommended to only run the CreateGlobalApplicationCommandAsync() once for each command.
                    }
                    catch (HttpException exception)
                    {
                        // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message. You can serialize the Error field in the exception to get a visual of where your error is.
                        var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);

                        // You can send this error somewhere or just print it to the console, for this example we're just going to print it.
                        Console.WriteLine(json);
                    }

                    globalCommand.WithName("all-alerts");
                    globalCommand.WithDescription(
                        "Preform This Command In Order To Get All Alerts In The Last 12 Hours!");

                    try
                    {
                        // With global commands we don't need the guild.
                        await _client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
                        // Using the ready event is a simple implementation for the sake of the example. Suitable for testing and development.
                        // For a production bot, it is recommended to only run the CreateGlobalApplicationCommandAsync() once for each command.
                    }
                    catch (HttpException exception)
                    {
                        // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message. You can serialize the Error field in the exception to get a visual of where your error is.
                        var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);

                        // You can send this error somewhere or just print it to the console, for this example we're just going to print it.
                        Console.WriteLine(json);
                    }

                    globalCommand.WithName("contact-us");
                    globalCommand.WithDescription("Report A Bug! Or Request A Feature!");

                    try
                    {
                        // With global commands we don't need the guild.
                        await _client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
                        // Using the ready event is a simple implementation for the sake of the example. Suitable for testing and development.
                        // For a production bot, it is recommended to only run the CreateGlobalApplicationCommandAsync() once for each command.
                    }
                    catch (HttpException exception)
                    {
                        // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message. You can serialize the Error field in the exception to get a visual of where your error is.
                        var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);

                        // You can send this error somewhere or just print it to the console, for this example we're just going to print it.
                        Console.WriteLine(json);
                    }

                    globalCommand.WithName("rate-us");
                    globalCommand.WithDescription("Rate Us <3");
                    globalCommand.AddOption(new SlashCommandOptionBuilder()

                        .WithName("rating")
                        .WithDescription("The rating youre willing to give our bot")
                        .WithRequired(true)
                        .AddChoice("Terrible", 1)
                        .AddChoice("Meh", 2)
                        .AddChoice("Good", 3)
                        .AddChoice("Lovely", 4)
                        .AddChoice("Excellent!", 5)
                        .WithType(ApplicationCommandOptionType.Integer)
                    );

                    try
                    {
                        // With global commands we don't need the guild.
                        await _client.CreateGlobalApplicationCommandAsync(globalCommand.Build());
                        // Using the ready event is a simple implementation for the sake of the example. Suitable for testing and development.
                        // For a production bot, it is recommended to only run the CreateGlobalApplicationCommandAsync() once for each command.
                    }
                    catch (HttpException exception)
                    {
                        // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message. You can serialize the Error field in the exception to get a visual of where your error is.
                        var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);

                        // You can send this error somewhere or just print it to the console, for this example we're just going to print it.
                        Console.WriteLine(json);
                    }

                    log_ = log_ + "\n" + (new LogMessage(LogSeverity.Info, "Client_Ready", "Completed"));
                    Console.WriteLine(new LogMessage(LogSeverity.Info, "Client_Ready", "Completed"));
                }
            );
        }

        public Task RedAlert_SendMessage(string message)
        {
            _ = Task.Run(() =>
            {
                log_ = log_ + "\n" + (new LogMessage(LogSeverity.Info, "RedAlert", "Send message started!"));
                Console.WriteLine(new LogMessage(LogSeverity.Info, "RedAlert", "Send message started!"));

                int i = 0;
                string id_string;

                using (StreamReader file = File.OpenText(@"id.txt"))
                {
                    id_string = file.ReadToEnd();
                }

                string[] ids = id_string.Split(',');
                try
                {
                    while (i < (ids.Length - 1))
                    {
                        i++;
                        ulong result = Convert.ToUInt64(ids[i]);

                        var chnl = _client.GetChannel(result) as IMessageChannel;
                        chnl.SendMessageAsync(message);
                        Task.Delay(50).Wait(50);
                    }

                    log_ = log_ + "\n" + (new LogMessage(LogSeverity.Info, "RedAlert", "Send message completed!"));
                    Console.WriteLine(new LogMessage(LogSeverity.Info, "RedAlert", "Send message completed!"));
                }
                catch (Exception e)
                {
                    log_ = log_ + "\n" +
                           (new LogMessage(LogSeverity.Info, "RedAlert", "Error in sending message!\n" + e));
                    Console.WriteLine(new LogMessage(LogSeverity.Info, "RedAlert", "Error in sending message!\n" + e));
                }
            });
            return Task.CompletedTask;
        }

        public async Task RedAlert_Role(SocketGuild arg)
        {
            ///////////////////////////////////////////
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.ForegroundColor = ConsoleColor.White;
            log_ = log_ + "\n" + (new LogMessage(LogSeverity.Info, "Role", "Checking Roles!     \n"));
            Console.WriteLine(new LogMessage(LogSeverity.Info, "Role", "Checking Roles!     \n"));
            ///////////////////////////////////////////
            for (int i = 0; i < _client.Guilds.Count; i++)
            {
                var guild = _client.Guilds.ToList()[i] as SocketGuild;
                try
                {
                    var role_ = guild.Roles.FirstOrDefault(x => x.Name == "Red Alert Notifications");
                    if (role_ == null)
                    {
                        var role = await guild.CreateRoleAsync($"Red Alert Notifications");
                        log_ = log_ + "\n" +
                               (new LogMessage(LogSeverity.Info, "Role", "Creating role for: " + guild.Id));
                        Console.WriteLine(new LogMessage(LogSeverity.Info, "Role", "Creating role for: " + guild.Id));
                        //Console.WriteLine(new LogMessage(LogSeverity.Info, "Role", guild.Name.ToString() + "\n"));
                    }
                    else
                    {
                        log_ = log_ + "\n" + (new LogMessage(LogSeverity.Info, "Role",
                            "Role already exists at: " + guild.Id));
                        Console.WriteLine(new LogMessage(LogSeverity.Info, "Role",
                            "Role already exists at: " + guild.Id));
                        //Console.WriteLine(new LogMessage(LogSeverity.Info, "Role", guild.Name.ToString() + "\n"));
                    }
                }
                catch (Exception)
                {
                    log_ = log_ + "\n" +
                           (new LogMessage(LogSeverity.Info, "Role", "Role problem exists at: " + guild.Id));
                    Console.WriteLine(new LogMessage(LogSeverity.Info, "Role", "Role problem exists at: " + guild.Id));
                    //Console.WriteLine(new LogMessage(LogSeverity.Info, "Role", guild.Name.ToString() + "\n"));
                }
            }

            ///////////////////////////////////////////
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            ///////////////////////////////////////////
        }

        public Task RedAlert_Id()
        {
            ///////////////////////////////////////////
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.White;
            log_ = log_ + "\n" + (new LogMessage(LogSeverity.Info, "ID", "Checking IDs!     \n"));
            Console.WriteLine(new LogMessage(LogSeverity.Info, "ID", "Checking IDs!     \n"));
            ///////////////////////////////////////////

            string id_string;
            List<string> list = new List<string>();

            using (StreamReader file = File.OpenText(@"id.txt"))
            {
                id_string = file.ReadToEnd();
            }

            string[] ids = id_string.Split(',');
            string[] ids_ = ids.Distinct().ToArray();

            for (int id_ = 1; id_ < ids_.Length;)
            {
                ulong result = Convert.ToUInt64(ids_[id_]);

                var chnl = _client.GetChannel(result) as IMessageChannel;

                try
                {
                    if (chnl != null)
                    {
                        //append
                        list.Add("," + result);
                        log_ = log_ + "\n" + (new LogMessage(LogSeverity.Info, "ID", Convert.ToString(result)));
                        Console.WriteLine(new LogMessage(LogSeverity.Info, "ID", Convert.ToString(result)));
                    }
                    else
                    {
                        log_ = log_ + "\n" + (new LogMessage(LogSeverity.Info, "ID",
                            "Channel " + result + " Cant be accessed, \n deleting channel from database now!"));
                        Console.WriteLine(new LogMessage(LogSeverity.Info, "ID",
                            "Channel " + result + " Cant be accessed, \n deleting channel from database now!"));
                    }
                }
                catch (Exception e)
                {
                    log_ = log_ + "\n" + (new LogMessage(LogSeverity.Info, "ID", "Channel " + result + e));
                    Console.WriteLine(new LogMessage(LogSeverity.Info, "ID", "Channel " + result + e));
                }

                id_++;
                //await Task.Delay(50);
            }

            using (StreamWriter file = File.CreateText(@"id.txt"))
            {
                foreach (string id in list)
                {
                    file.Write(id);
                }
            }

            log_ = log_ + "\n" + (new LogMessage(LogSeverity.Info, "ID", "Done checking IDs!\n"));
            Console.WriteLine(new LogMessage(LogSeverity.Info, "ID", "Done checking IDs!\n"));
            Console.WriteLine();
            _ = RedAlert_Role(arg);
            return Task.CompletedTask;
        }

        public Task RedAlert()
        {
            _ = Task.Run(() =>
            {
                log_ = log_ + "\n" + (new LogMessage(LogSeverity.Info, "RedAlert", "Send message started!"));
                Console.WriteLine(new LogMessage(LogSeverity.Info, "RedAlert", "Send message started!"));

                int i = 0;
                string id_string;

                using (StreamReader file = File.OpenText(@"id.txt"))
                {
                    id_string = file.ReadToEnd();
                }

                string[] ids = id_string.Split(',');
                try
                {
                    if (image_url == null)
                    {
                        image_url =
                            "https://cdn.discordapp.com/attachments/965774763396042763/1005619855849947297/image.png";
                    }

                    var embedBuilder = new EmbedBuilder()
                    {
                        ImageUrl = image_url
                    };

                    embedBuilder.AddField("Google Maps Link: ",
                            $"Click here for google maps link! [Google Maps]({website_})!")

                        .WithTitle("*Red Alert*")
                        .WithDescription("**צבע אדום ב:**\n" + "מיקום: " + location_ + "\n" + "תאריך: " +
                                         DateTime.Now.ToString() + "\n" + title_ + "\n" +
                                         "\n 📢 <:MISSILES:945771593676779601>" + "\n")
                        .WithFooter(footer => footer.WithText(desc_))
                        .WithColor(Color.DarkRed);

                    while (i < (ids.Length - 1))
                    {
                        i++;
                        ulong result = Convert.ToUInt64(ids[i]);

                        var chnl = _client.GetChannel(result) as IMessageChannel;

                        var chnl_ = _client.GetChannel(result) as SocketGuildChannel;
                        var Guild = chnl_.Guild.Id;
                        var guild = _client.GetGuild(Guild);

                        var role = guild.Roles.FirstOrDefault(x => x.Name == "Red Alert Notifications");
                        try
                        {
                            chnl.SendMessageAsync($"<@&{role.Id}>" + " <:MISSILES:945771593676779601>");
                            Task.Delay(50).Wait(50);
                            chnl.SendMessageAsync(embed: embedBuilder.Build(), isTTS: true);
                        }
                        catch (Exception)
                        {
                            chnl.SendMessageAsync(embed: embedBuilder.Build(), isTTS: true);
                        }

                        Task.Delay(50).Wait(50);
                    }

                    log_ = log_ + "\n" + (new LogMessage(LogSeverity.Info, "RedAlert", "Send message completed!"));
                    Console.WriteLine(new LogMessage(LogSeverity.Info, "RedAlert", "Send message completed!"));
                }
                catch (Exception e)
                {
                    log_ = log_ + "\n" +
                           (new LogMessage(LogSeverity.Info, "RedAlert", "Error in sending message!\n" + e));
                    Console.WriteLine(new LogMessage(LogSeverity.Info, "RedAlert", "Error in sending message!\n" + e));
                }
            });
            return Task.CompletedTask;
        }

        private Task SlashCommandHandler(SocketSlashCommand command)
        {
            _ = Task.Run(async () =>
            {
                slash_command = command.Channel.Id;
                //await command.RespondAsync($"You executed {command.Data.Name}");
                //command.User.
                if (command.Data.Name == "set-up" && command.IsDMInteraction == false)
                {
                    var builder = new ComponentBuilder()
                        .WithButton("Click Here!", "1");
                    //command.Channel.Id LOG THIS CHANNEL!!
                    await command.RespondAsync(
                        "Click The Button In Order To Set This Chanel As Alert Channel! \n You Have 30 Seconds! 📢 <:MISSILES:945771593676779601> ",
                        components: builder.Build());
                    await Task.Delay(30000);
                    await command.DeleteOriginalResponseAsync();
                }
                else if (command.Data.Name == "set-up" && command.IsDMInteraction == true)
                {
                    await command.RespondAsync("Oops... You can't set me up in a DM Channel! :( ");
                }

                if (command.Data.Name == "get-notifications")
                {
                    try
                    {
                        user_id = command.User.Id;
                        await command.RespondAsync("Trying To Add Role Alert...");
                        var chnl = command.Channel as SocketGuildChannel;
                        var Guild = chnl.Guild.Id;
                        var guild = _client.GetGuild(Guild);
                        var user = guild.GetUser(user_id);

                        var role = guild.Roles.FirstOrDefault(x => x.Name == "Red Alert Notifications");

                        //Console.WriteLine(role.Name);

                        await ((SocketGuildUser)user).AddRoleAsync(role);
                        await command.ModifyOriginalResponseAsync(m => m.Content = "Role granted!");

                        Task.Delay(5000).Wait(5000);
                        await command.DeleteOriginalResponseAsync();
                    }
                    catch (Exception)
                    {
                        await command.ModifyOriginalResponseAsync(m => m.Content = "Role Could Not Be Added!");
                    }
                }

                if (command.Data.Name == "disable-notifications")
                {
                    try
                    {
                        user_id = command.User.Id;
                        await command.RespondAsync("Trying To Remove Role Alert...");
                        var chnl = command.Channel as SocketGuildChannel;
                        var Guild = chnl.Guild.Id;
                        var guild = _client.GetGuild(Guild);
                        var user = guild.GetUser(user_id);

                        var role = guild.Roles.FirstOrDefault(x => x.Name == "Red Alert Notifications");

                        //Console.WriteLine(role.Name);

                        await ((SocketGuildUser)user).RemoveRoleAsync(role);
                        await command.ModifyOriginalResponseAsync(m => m.Content = "Role Removed!");
                        Task.Delay(5000).Wait(5000);
                        await command.DeleteOriginalResponseAsync();
                    }
                    catch (Exception)
                    {
                        await command.ModifyOriginalResponseAsync(m => m.Content = "Role Could Not Be Removed!");
                    }
                }

                if (command.Data.Name == "recent-alert")
                {
                    user_id = command.User.Id;
                    await command.RespondAsync("Trying To Retrive Last Alert...");
                    await SendEmbedAsync();
                }

                if (command.Data.Name == "all-alerts")
                {
                    user_id = command.User.Id;
                    await command.RespondAsync("Trying To Retrive All Alerts...");
                    await All_Alerts();
                }

                if (command.Data.Name == "rate-us")
                {
                    await HandleFeedbackCommand(command);
                }

                if (command.Data.Name == "contact-us")
                {
                    var menuBuilder = new SelectMenuBuilder()
                        .WithPlaceholder("Select an option")
                        .WithCustomId("menu-1")
                        .WithMinValues(1)
                        .WithMaxValues(1)
                        .AddOption("Report A Bug", "Report A Bug", "Report A Bug To The Developer!")
                        .AddOption("Request To Add A Feature", "Request To Add A Feature",
                            "Send The Developer A Request To Add A Feature!");

                    var builder = new ComponentBuilder()
                        .WithSelectMenu(menuBuilder);

                    await command.RespondAsync("Select An Option!", components: builder.Build());
                }
            });
            return Task.CompletedTask;
        }

        public Task MyMenuHandler(SocketMessageComponent arg)
        {
            _ = Task.Run(async () =>
            {
                var text = string.Join(", ", arg.Data.Values);
                //await arg.RespondAsync($"You have selected {text}");
                var option = string.Join("", arg.Data.Values);
                if (option == "Request To Add A Feature")
                {
                    var mb = new ModalBuilder()
                        .WithTitle("Request To Add A Feature:")
                        .WithCustomId("feature")
                        .AddTextInput("What is the feature you would like us to add?", "feature",
                            placeholder: "A feature.");

                    await arg.RespondWithModalAsync(mb.Build());
                    await Task.Delay(500);
                    await arg.DeleteOriginalResponseAsync();
                }

                if (option == "Report A Bug")
                {
                    var mb = new ModalBuilder()
                        .WithTitle("Report A Bug:")
                        .WithCustomId("bug")
                        .AddTextInput("What is the bug you would like to report?", "bug", placeholder: "A bug.");

                    await arg.RespondWithModalAsync(mb.Build());
                    await Task.Delay(500);
                    await arg.DeleteOriginalResponseAsync();
                }
            });
            return Task.CompletedTask;
        }

        public Task BugFeature(SocketModal modal)
        {
            _ = Task.Run(async () =>
            {
                List<SocketMessageComponentData> components = modal.Data.Components.ToList();

                string bug, feature = "";
                ulong wae = modal.Id;
                if (modal.Data.CustomId == "bug")
                {
                    bug = components
                        .First(x => x.CustomId == "bug").Value;

                    string message = $"hey {_client.GetUser(my_id)}; User " + $"{modal.User} sent a bug report: \n " +
                                     $"{bug}.";

                    ulong user_id = my_id; // me
                    var user = _client.GetUserAsync(user_id).Result;
                    await UserExtensions.SendMessageAsync(user, message);

                    var embedBuilder = new EmbedBuilder()
                        .WithAuthor(modal.User)
                        .WithTitle("Report A Bug")
                        .WithDescription($"Thanks for your bug report! We will review it soon!")
                        .WithColor(Color.Green)
                        .WithCurrentTimestamp();

                    AllowedMentions mentions = new AllowedMentions();
                    mentions.AllowedTypes = AllowedMentionTypes.Users;

                    await modal.RespondAsync(embed: embedBuilder.Build());
                    await Task.Delay(10000);
                    await modal.DeleteOriginalResponseAsync();
                }

                if (modal.Data.CustomId == "feature")
                {
                    feature = components
                        .First(x => x.CustomId == "feature").Value;

                    string message = $"hey {_client.GetUser(my_id)}; User " +
                                     $"{modal.User} sent a feature request: \n " + $"{feature}.";

                    ulong user_id = my_id; // me
                    var user = _client.GetUserAsync(user_id).Result;
                    await UserExtensions.SendMessageAsync(user, message);

                    var embedBuilder = new EmbedBuilder()
                        .WithAuthor(modal.User)
                        .WithTitle("Add A Feature")
                        .WithDescription($"Thanks for your suggestion! We will review it soon!")
                        .WithColor(Color.Green)
                        .WithCurrentTimestamp();

                    AllowedMentions mentions = new AllowedMentions();
                    mentions.AllowedTypes = AllowedMentionTypes.Users;

                    await modal.RespondAsync(embed: embedBuilder.Build());
                    await Task.Delay(10000);
                    await modal.DeleteOriginalResponseAsync();
                }
            });
            return Task.CompletedTask;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private Task HandleFeedbackCommand(SocketSlashCommand command)
        {
            _ = Task.Run(async () =>
            {
                var embedBuilder = new EmbedBuilder()
                    .WithAuthor(command.User)
                    .WithTitle("Feedback")
                    .WithDescription($"Thanks for your feedback! You rated us {command.Data.Options.First().Value}/5")
                    .WithColor(Color.Green)
                    .WithCurrentTimestamp();

                var embedBuilderuser = new EmbedBuilder()
                    .WithAuthor(command.User)
                    .WithTitle("Feedback")
                    .WithDescription(
                        $"The user rated the bot {command.Data.Options.First().Value}/5 \n Feedback by: {Convert.ToString(command.User) + "\n ID: " + Convert.ToString(command.User.Id)}")
                    .WithColor(Color.Green)
                    .WithCurrentTimestamp();

                ulong user_id = my_id; // me
                var user = _client.GetUserAsync(user_id).Result;

                await command.RespondAsync(embed: embedBuilder.Build());
                await UserExtensions.SendMessageAsync(user, embed: embedBuilderuser.Build());

                await Task.Delay(10000);
                await command.DeleteOriginalResponseAsync();
            });
            return Task.CompletedTask;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task All_Alerts()
        {
            using (WebClient clinet = new WebClient())
            {
                clinet.Headers.Add("user-agent",
                    "Mozilla / 5.0(Windows NT 10.0; Win64; x64; rv: 99.0) Gecko / 20100101 Firefox / 99.0");
                try
                {
                    ////////////////////////////////////////////
                    //Console.WriteLine("Still Looking....");
                    ////////////////////////////////////////////

                    json = JsonConvert.DeserializeObject<List<AlertHistory>>(
                        clinet.DownloadString("https://www.oref.org.il/WarningMessages/History/AlertsHistory.json"));

                    location = (json[0].data);
                    date = ("Date: " + json[0].alertDate);
                    title = ("Title: " + json[0].title);
                    website = (@"~~" + "https://www.google.com/maps/search/" + json[0].data.Replace(" ", "_") + "~~");
                }
                catch (Exception)
                {
                }
            }

            if (location != null && location != "")
            {
                int alert = 0;
                foreach (var alert1 in json)
                {
                    alert = alert + 1;
                }

                int i = 0;
                alert = alert - 1;
                string all_alerts = "";
                while (i < alert)
                {
                    i++;
                    //////
                    all_alerts = all_alerts + " ," + (json[i].data);
                }

                var embed = new EmbedBuilder()
                    .WithTitle("*Red Alert*")
                    .WithDescription("**All Alerts In The 12 Last Houers Were At:**\n" + all_alerts + "." +
                                     "\n\n*Last Alert At: " + json[0].alertDate + "*" +
                                     "  📢 <:MISSILES:945771593676779601>")
                    .WithColor(Color.DarkRed);

                try
                {
                    var chnl = _client.GetChannel(slash_command) as IMessageChannel;
                    await chnl.SendMessageAsync(embed: embed.Build());
                }
                catch
                {
                    // Get the user with the ID from your DiscordSocketClient
                    var user = _client.GetUserAsync(user_id).Result;

                    await UserExtensions.SendMessageAsync(user, embed: embed.Build());
                }
            }
            else
            {
                try
                {
                    var chnl = _client.GetChannel(slash_command) as IMessageChannel;
                    await chnl.SendMessageAsync("Could Not Find Alerts.... :( ");
                }
                catch
                {
                    string msg = "Could Not Find Alerts.... :(";

                    // Get the user with the ID from your DiscordSocketClient
                    var user = _client.GetUserAsync(user_id).Result;

                    await UserExtensions.SendMessageAsync(user, msg);
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandsAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandsAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            //if (message.Author.IsBot) return;
            if (message.Author.Id.Equals(my_id))
            {
                int argPos = 0;
                if (message.HasStringPrefix("Red Alert:", ref argPos))
                {
                    var result = await _commands.ExecuteAsync(context, argPos, _services);
                    if (!result.IsSuccess) Console.WriteLine("Result Failed!" + " " + result.ErrorReason);
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task MyButtonHandler(SocketMessageComponent component) //buttons
        {
            // We can now check for our custom id
            switch (component.Data.CustomId)
            {
                // Since we set our buttons custom id as 'custom-id', we can check for it like this:
                case "1":
                    // Lets respond by sending a message saying they clicked the button
                    await component.UpdateAsync(m =>
                    {
                        m.Components = null;
                        m.Content = "Done! [Edited.]";
                    });
                    await component.FollowupAsync(
                        $"{component.User.Mention} Ok!, Red Alerts Will Be Sent In this Channel Now! <:MISSILES:945771593676779601>");
                    ;

                    using (StreamReader file = File.OpenText(@"id.txt"))
                    {
                        id_string = file.ReadToEnd();
                    }

                    id_string = id_string + "," + component.ChannelId;

                    using (StreamWriter file = File.CreateText(@"id.txt"))
                    {
                        file.Write(id_string);
                    }

                    break;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Task ServerNumber()
        {
            _ = Task.Run(async () =>
            {
                int users = 0;
                while (true)
                {
                    await _client.SetActivityAsync(new Game(
                        "  Over " + Convert.ToString(_client.Guilds.Count) + " Servers! 📢", ActivityType.Watching,
                        ActivityProperties.None));

                    for (int i = 0; i < _client.Guilds.Count; i++)
                    {
                        users = users + _client.Guilds.ToList()[i].MemberCount;
                    }

                    //Console.WriteLine(users);
                    await Task.Delay(10000);
                    await _client.SetActivityAsync(new Game("  Over " + Convert.ToString(users) + " Users! 📢",
                        ActivityType.Watching, ActivityProperties.None));
                    users = 0;
                    await Task.Delay(10000);
                }
            });
            log_ = log_ + "\n" + (new LogMessage(LogSeverity.Info, "Status", "Setting Status..."));
            Console.WriteLine(new LogMessage(LogSeverity.Info, "Status", "Setting Status..."));
            return Task.CompletedTask;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public struct Alert
        {
            public string desc;
            public string title;
            public List<string> data;

            public Alert(string desc, string title, List<string> data)
            {
                this.desc = desc;
                this.title = title;
                this.data = data;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public struct AlertHistory
        {
            public string alertDate;
            public string title;
            public string data;

            public AlertHistory(string alertDate, string title, string data)
            {
                this.alertDate = alertDate;
                this.title = title;
                this.data = data;
            }
        }

        private static T GetNestedException<T>(Exception ex) where T : Exception
        {
            if (ex == null)
            {
                return null;
            }

            var tEx = ex as T;
            if (tEx != null)
            {
                return tEx;
            }

            return GetNestedException<T>(ex.InnerException);
        }

        private async Task Reconnect_ToServer()
        {
            while (true)
            {
                try
                {
                    Task.Delay(60000).Wait(60000);
                    using (WebClient clinet = new WebClient())
                    {
                        clinet.Headers.Add("user-agent",
                            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_13_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.100 Safari/537.36");
                        clinet.Headers.Add("X-Requested-With", "XMLHttpRequest");
                        clinet.Headers.Add("Referer", "https://www.oref.org.il/11226-he/pakar.aspx");

                        alert_json =
                            clinet.DownloadString("https://www.oref.org.il/WarningMessages/alert/alerts.json");
                    }

                    // if to here it works means server responding agian, should restart the task.
                    Azaka().Dispose();
                    ///////////////////////////////////////////
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.White;
                    log_ = log_ + "\n" + (new LogMessage(LogSeverity.Critical, "Red Alert",
                        "Reconnected to main server! - restarting task."));
                    Console.WriteLine();
                    Console.WriteLine(new LogMessage(LogSeverity.Critical, "Red Alert",
                        "Reconnected to main server! - restarting task."));
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    ///////////////////////////////////////////
                    await Azaka();
                    break;
                }
                catch (Exception)
                {
                    //continue running until recoonect.
                }
            }
        }

        public async Task Azaka() //need to add delay
        {
            Alert alert = JsonConvert.DeserializeObject<Alert>("{}");
            var old_alert = alert;

            /////////////////////////////////////////// create object
            await GetImage_stage1();
            Task.Delay(1500).Wait(1500);
            ///////////////////////////////////////////
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            log_ = log_ + "\n" + (new LogMessage(LogSeverity.Critical, "Red Alert", "Started Looking For Red Alerts!"));
            Console.WriteLine();
            Console.WriteLine(new LogMessage(LogSeverity.Critical, "Red Alert", "Started Looking For Red Alerts!"));
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            ///////////////////////////////////////////

            while (true)
            {
                try
                {
                    ////////////////////////////////////////////
                    //Console.WriteLine("Still Looking....");
                    ////////////////////////////////////////////
                    using (WebClient clinet = new WebClient())
                    {
                        clinet.Headers.Add("user-agent",
                            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_13_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.100 Safari/537.36");
                        clinet.Headers.Add("X-Requested-With", "XMLHttpRequest");
                        clinet.Headers.Add("Referer", "https://www.oref.org.il/11226-he/pakar.aspx");

                        alert_json = clinet.DownloadString("https://www.oref.org.il/WarningMessages/alert/alerts.json");
                    }

                    ///////////////////////////////////////////
                    Task.Delay(2500).Wait(2500);
                    ///////////////////////////////////////////
                    using (WebClient clinet = new WebClient())
                    {
                        clinet.Headers.Add("user-agent",
                            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_13_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.100 Safari/537.36");
                        clinet.Headers.Add("X-Requested-With", "XMLHttpRequest");
                        clinet.Headers.Add("Referer", "https://www.oref.org.il/11226-he/pakar.aspx");
                        alert_json_ = clinet.DownloadString("https://www.oref.org.il/WarningMessages/alert/alerts.json");
                    }
                }
                catch (Exception ex)
                {
                    log_ = log_ + (ex);
                    Console.WriteLine(ex);
                    var wex = GetNestedException<WebException>(ex);

                    // If there is no nested WebException, re-throw the exception.
                    //if (wex == null) { throw; }

                    // Get the response object.
                    var response = wex.Response as HttpWebResponse;

                    // If it's an HTTP response
                    if (response != null)
                    {
                        _ = Reconnect_ToServer();
                        break;
                    }

                    ///////////////////////////////////////////
                    Task.Delay(2500).Wait(2500);
                    ///////////////////////////////////////////
                }

                if (alert_json_ != alert_json && alert_json_ != null)
                {
                    location_ = "";
                    desc_ = "";
                    title_ = "";
                    website_ = "";

                    try
                    {
                        alert = JsonConvert.DeserializeObject<Alert>(alert_json_);
                        old_alert = JsonConvert.DeserializeObject<Alert>(alert_json);

                        alert.data = alert.data.Except(old_alert.data).ToList();
                    }
                    catch
                    {
                    }

                    try
                    {
                        foreach (string loc in alert.data)
                        {
                            location_ = location_ + " " + loc;
                        }

                        desc_ = ("הוראות: " + alert.desc);
                        title_ = ("אירוע: " + alert.title);
                        website_ = (@"https://www.google.com/maps/search/" + alert.data[0].Replace(" ", "_"));

                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;

                        log_ = log_ + "\n" + (new LogMessage(LogSeverity.Info, "Red Alert", "Red Alert Detected!"));
                        Console.WriteLine(new LogMessage(LogSeverity.Info, "Red Alert", "Red Alert Detected!"));

                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;

                        await GetImage_stage2();
                        await RedAlert();
                    }
                    catch
                    {
                    }
                }
            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// ------ OLD

            using (WebClient clinet = new WebClient())
            {
                clinet.Headers.Add("user-agent",
                    "Mozilla / 5.0(Windows NT 10.0; Win64; x64; rv: 99.0) Gecko / 20100101 Firefox / 99.0");
                //json = JsonConvert.DeserializeObject<List<Alert>>(clinet.DownloadString("https://www.oref.org.il/WarningMessages/History/AlertsHistory.json"));
                ///////////////////////////////////////////
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
                log_ = log_ + "\n" + (new LogMessage(LogSeverity.Critical, "Red Alert",
                    "Reverted to back-up systems! - Problem in server."));
                Console.WriteLine(new LogMessage(LogSeverity.Critical, "Red Alert",
                    "Reverted to back-up systems! - Problem in server."));
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                ///////////////////////////////////////////
                while (true)
                {
                    try
                    {
                        ////////////////////////////////////////////
                        //Console.WriteLine("Still Looking....");
                        ////////////////////////////////////////////

                        json = JsonConvert.DeserializeObject<List<AlertHistory>>(
                            clinet.DownloadString(
                                "https://www.oref.org.il/WarningMessages/History/AlertsHistory.json"));
                        date_ = ("תאריך: " + json[0].alertDate);

                        ///////////////////////////////////////////
                        Task.Delay(5500).Wait(5500);
                        ///////////////////////////////////////////

                        json = JsonConvert.DeserializeObject<List<AlertHistory>>(
                            clinet.DownloadString(
                                "https://www.oref.org.il/WarningMessages/History/AlertsHistory.json"));
                        location_ = (json[0].data);
                        title_ = ("אירוע: " + json[0].title);

                        string new_date = ("תאריך: " + json[0].alertDate);
                        if (new_date != date_)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.ForegroundColor = ConsoleColor.White;
                            log_ = log_ + "\n" + (new LogMessage(LogSeverity.Info, "Red Alert", "Red Alert Detected!"));
                            Console.WriteLine(new LogMessage(LogSeverity.Info, "Red Alert", "Red Alert Detected!"));
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.White;
                            website_ = (@"https://www.google.com/maps/search/" + json[0].data.Replace(" ", "_"));
                            //await RedAlert_Id();  --- ads quite a bit of delay should be better off. - should do on server leave
                            await GetImage_stage2();
                            await RedAlert();
                            //Console.WriteLine(location + "\n" + date + "\n" + title);
                        }
                    }
                    catch (Exception e)
                    {
                        ///////////////////////////////////////////
                        Task.Delay(5500).Wait(5500);
                        ///////////////////////////////////////////

                        //Console.BackgroundColor = ConsoleColor.Red;
                        //Console.ForegroundColor = ConsoleColor.White

                        //Console.WriteLine("Error!\n" + e);
                        log_ = log_ + (e);
                    }
                }
            }
        }

        public struct Image
        {
            public string lat;
            public string lng;
            public string name;

            public Image(string lat, string lng, string name)
            {
                this.lat = lat;
                this.lng = lng;
                this.name = name;
            }
        }

        public async Task GetImage_stage1()
        {
            //first of all get api key.
            using (StreamReader file = File.OpenText(@"api_key.txt"))
            {
                api_key = file.ReadToEnd();
            }

            using (StreamReader file = File.OpenText(@"citiesArchive.json"))
            {
                image_json = file.ReadToEnd();
            }

            image = JsonConvert.DeserializeObject<List<Image>>(image_json);
        }

        public async Task GetImage_stage2()
        {
            string newlocation_;

            if (location_[0] == ' ')
            {
                newlocation_ = location_.Remove(0, 1);
            }
            else
            {
                newlocation_ = location_;
            }

            image_url = null;
            lat = null;
            lng = null;

            foreach (var VARIABLE in image)
            {
                if (VARIABLE.name.Contains(newlocation_))
                {
                    lat = VARIABLE.lat;
                    lng = VARIABLE.lng;
                }
            }

            if (lat != null && lng != null)
            {
                image_url =
                    $"https://dev.virtualearth.net/REST/v1/Imagery/Map/Road/{lat},{lng}?ms=500,270&zl=12&&c=he-IL&he=0&&dc=c,55ff0000,FFFF5064,2,75;{lat},{lng}&fmt=png&key={api_key}";
            }
            else
            {
                image_url =
                    "https://cdn.discordapp.com/attachments/965774763396042763/1005619855849947297/image.png";
            }

            ///////////////////////////////////////////////////////////////////////

            //var user_ = _client.GetUserAsync(my_id).Result;
            //await UserExtensions.SendMessageAsync(user_, $"Cord {lat},{lng}.  Area: {newlocation_}");

            ///////////////////////////////////////////////////////////////////////

            //https://dev.virtualearth.net/REST/v1/Imagery/Map/Road/{lat},{lng}?ms=500,270&zl=12&&c=he-IL&he=0&&dc=c,55ff0000,FFFF5064,2,75;{lat},{lng}&fmt=png&key={api_key}

            /*
            if (lat != null && lng != null)
            {
                string website =
                    $"https://dev.virtualearth.net/REST/V1/Imagery/Metadata/Road/{lat},{lng}?zl=13&o=xml&key={api_key}";

                //https://dev.virtualearth.net/REST/V1/Imagery/Map/Road/{lat},{lng}/13?mapSize=350,350&dc=c,64009900,FF009900,2,100;{lat},{lng}&fmt=png&key={api_key}

                using (WebClient clinet = new WebClient())
                {
                    clinet.Headers.Add("user-agent",
                        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_13_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.100 Safari/537.36");

                    website__ = clinet.DownloadString(website);
                }

                //<ImageUrl>http://ecn.t3.tiles.virtualearth.net/tiles/a032010110123333.jpeg?g=12552</ImageUrl>
                one = website__.IndexOf(@"<ImageUrl>");
                two = website__.IndexOf(@"</ImageUrl>");

                image_url = website__.Substring(one + 10, two - one - 10);
            }
            */
        }
    }
}