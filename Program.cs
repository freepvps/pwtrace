using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Magic;
using Magic.Net;
using Magic.Net.Packets;
using Magic.Net.Plugins;
using Magic.Net.Packets.Client;
using Magic.Net.Packets.Server;
using System.Diagnostics;
using System.Text;

namespace serverstatus
{
    public class MainClass
    {
        static Stopwatch sw = new Stopwatch();

        static string Name;
        static string Id;
        static string Host;
        static string Login;
        static string Password;
        static TextWriter Output = Console.Out;

        static int RoleIndex = 0;
        static bool RoleCreate = true;
        static bool Force = true;

        static bool PrintPackets = false;
        static bool PrintPacketsAll = false;

        static int p = 0;

        static string[] helps = { "-h", "--help", "--h", "-help" };
        static string GetHelp()
        {
            var sb = new StringBuilder();

            var name = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            sb.AppendLine($"{name} [host] [login] [password] [params...]");
            sb.AppendLine($"    -load PATH - init config from PATH");
            sb.AppendLine($"    -o PATH - output file");
            sb.AppendLine($"    -id VALUE - status ID");
            sb.AppendLine($"    -name VALUE - status name");
            sb.AppendLine($"    -ri VALUE - role index (default {RoleIndex}) (-1 - don't enter world)");
            sb.AppendLine($"    -nr - no role (don't create new role)");
            sb.AppendLine($"    -nf - no force auth");
            sb.AppendLine($"    -pp - print packets");
            sb.AppendLine($"    -ppa - print all packets info (content)");

            return sb.ToString();
        }
        static void Init(string[] args)
        {
            var q = new Queue<string>(args);
            if (args.Intersect(helps).Count() > 0)
            {
                Console.WriteLine(GetHelp());
                Environment.Exit(0);
            }

            while (q.Count > 0)
            {
                var cmd = q.Dequeue();
                switch (cmd)
                {
                    case "-load":
                        Init(File.ReadAllLines(q.Dequeue()));
                        continue;
                    case "-o":
                        Output = new StreamWriter(q.Dequeue());
                        continue;
                    case "-id":
                        Id = q.Dequeue();
                        continue;
                    case "-name":
                        Name = q.Dequeue();
                        continue;
                    case "-ri":
                        RoleIndex = int.Parse(q.Dequeue());
                        continue;
                    case "-nr":
                        RoleCreate = false;
                        continue;
                    case "-nf":
                        Force = false;
                        continue;
                    case "-pp":
                        PrintPackets = true;
                        continue;
                    case "-ppa":
                        PrintPackets = PrintPacketsAll = true;
                        continue;
                    default:
                        switch (p)
                        {
                            case 0: Host = cmd; p++; continue;
                            case 1: Login = cmd; p++; continue;
                            case 2: Password = cmd; p++; continue;
                        }
                        continue;
                }
            }
        }
        static void Print(string block, string message, ConsoleColor color, params object[] args)
        {
            Output.Write("[" + sw.ElapsedMilliseconds + "][");
            Console.ForegroundColor = color;
            Output.Write(block);
            Console.ResetColor();
            Output.WriteLine("] " + message, args);
        }
        static void Param(string name, object value)
        {
            if (value == null) 
                value = "null";
            Print("param", "{0}: {1}", ConsoleColor.White, name, value);
        }
        static void Trace(string msg, params object[] args)
        {
            Print("trace", msg, ConsoleColor.Gray, args);
        }
        static void Info(string msg, params object[] args)
        {
            Print("info", msg, ConsoleColor.Green, args);
        }
        static void Warn(string msg, params object[] args)
        {
            Print("warn", msg, ConsoleColor.Yellow, args);
        }
        static void Error(string msg, params object[] args)
        {
            Print("error", msg, ConsoleColor.Red, args);
        }

        static void PrintInfo()
        {
            var gs = GameServer.Parse(Host);

            Param("Time", DateTime.Now.ToString("u"));
            Param("Name", Name);
            Param("Id", Id);
            Param("Host", gs.ToString());
            Param("Login", Login);
            Param("Password", Password);
            Param("Version", "1.1");
            Param("Author", "freepvps");
        }

        static bool Complete;
        static OOGHost host;
        static AuthPlugin Auth;
        static RolesPoolPlugin Pool;
        static WorldEnteringPlugin WorldEntering;
        public static void Main(string[] args)
        {
            sw.Start();
            Trace("Started");
            #if DEBUG
            if (args.Length == 0)
            {
                //args = new string[] { "144.76.156.12" };
                //args = new string[] { "193.107.128.236", "Defjam", "1" };
            }
            #endif
            try
            {
                Init(args);
            }
            catch (Exception ex)
            {
                Error("Config load error {0}", ex.Message);
                return;
            }
            finally
            {
                PrintInfo();
            }

            host = new OOGHost(Host);
            host.Handler.OnPacket += Host_Handler_OnPacket;

            if (!string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password))
            {
                Trace("Init auth");
                Auth = host.Plugins.Register<AuthPlugin>();
                Pool = host.Plugins.Register<RolesPoolPlugin>();
                WorldEntering = host.Plugins.Register<WorldEnteringPlugin>();
                if (RoleIndex == -1)
                {
                    WorldEntering.Enabled = false;
                    Pool.Enabled = false;
                }

                Pool.Enabled &= RoleCreate;

                Auth.Account.EnteredWorldChanged += Account_EnteredWorldChanged;
                Auth.Account.RolesLoadedChanged += Account_RolesLoadedChanged;
                Auth.SetAuthData(Login, Password, Force);
            }

            host.ConnectionInfo.StatusChanged += ConnectionInfo_StatusChanged;


            host.Handler.AddHandler<ServerInfoS01>(ServerInfo);

            host.Handler.AddHandler<OnlineAnnounceS04>(OnlineAnnounce);
            host.Handler.AddHandler<AnnounceForbidInfoS7B>(ForbidInfo);
            host.Handler.AddHandler<RoleList_ReS53>(OnRoleList_Receive);
            host.Handler.AddHandler<CreateRole_ReS55>(OnCreateRole_Receive);
            host.Handler.AddHandler<ErrorInfoS05>(ErrorInfo);
            host.Handler.AddHandler<LastLoginInfoS8F>(LastLogin);

            //5host.Handler.Logging = true;
            host.Handler.Log = Console.Error;

            host.Start();

            while (!Complete)
                System.Threading.Thread.Sleep(10);
        }

        static void Host_Handler_OnPacket (PacketIdentifier packetId, Magic.IO.DataStream ds)
        {
            if (PrintPackets)
            {
                if (PrintPacketsAll)
                {
                    Info("{0} ({1} bytes) : {2}", packetId, ds.Count, BitConverter.ToString(ds.Buffer, 0, ds.Count));
                }
                else
                {
                    Info("{0} ({1} bytes)", packetId, ds.Count);
                }
            }
        }
        static void ServerInfo(object sender, PacketEventArgs e)
        {
            var serverInfo = e.Packet as ServerInfoS01;
            if (serverInfo.ServerStatus > 0.2)
            {
                Warn("Server status: {0}%", serverInfo.ServerStatus * 100);
            }
            else
            {
                Info("Server status: {0}%", serverInfo.ServerStatus * 100);
            }
            Info("Free creation time: {0}", serverInfo.FreeCreationTime);
            Info("Nonce: {0}", BitConverter.ToString(serverInfo.Key).Replace('-', ' '));
            var proto = "MD5";
            if (serverInfo.AuthType == 1) proto = "SHA256";
            else if (serverInfo.AuthType == 2) proto = "Token?";

            Info("Version: {0}", serverInfo.ServerVersion);
            Info("S01Version: {0}", serverInfo.S01Version);
            Info("Auth type: {0} ({1})", proto, serverInfo.AuthType);
            if (serverInfo.CRC != null)
            {
                if (serverInfo.CRC.Length == 0)
                {
                    Error("CRC is empty");
                }
                else
                {
                    Info("CRC: {0}", serverInfo.CRC);
                }
            }
            if (serverInfo.Bonus != null)
            {
                Info("Bonus mask: {0}", (serverInfo.Bonus ?? 0).ToString("X2"));
            }
            if (serverInfo.UnknownId1 != null)
            {
                Info("S01 unknown id 1: {0}", serverInfo.UnknownId1 ?? 0);
            }
            if (serverInfo.UnknownId2 != null)
            {
                Info("S01 unknown id 2: {0}", serverInfo.UnknownId2 ?? 0);
            }
            if (Auth == null)
            {
                Complete = true;
                host.Close();
            }
        }
        static string IpToString(byte[] ipBytes)
        {
            return string.Format("{3}.{2}.{1}.{0}", ipBytes[0], ipBytes[1], ipBytes[2], ipBytes[3]);
        }
        static void LastLogin(object sender, PacketEventArgs e)
        {
            var lastLogin = e.Packet as LastLoginInfoS8F;
            Info("Last login time: {0}", lastLogin.LastLoginTime.Time.ToString("u"));
            Info("Last login ip: {0}", IpToString(lastLogin.LastLoginIP));
            Info("Current ip: {0}", IpToString(lastLogin.CurrentIP));
        }
        static void Account_EnteredWorldChanged(object sender, EventArgs e)
        {
            Info("Entered world: {0}", Auth.Account.EnteredWorld);
            if (Auth.Account.EnteredWorld)
            {
                Complete = true;
            }
        }
        static void Account_RolesLoadedChanged(object sender, EventArgs e)
        {
            Info("Roles loaded: {0}", Auth.Account.RolesLoaded);
            if (Auth.Account.RolesLoaded)
            {
                if (!Pool.Enabled && !WorldEntering.Enabled)
                {
                    Complete = true;
                }
                if (Pool.Enabled && WorldEntering.Enabled)
                {
                    if (RoleIndex >= Pool.PoolSize && Auth.Account.Roles.Count <= RoleIndex)
                    {
                        Error("RoleIndex out of range");
                        Complete = true;
                    }
                }
                if (!Pool.Enabled && WorldEntering.Enabled)
                {
                    if (Auth.Account.Roles.Count <= RoleIndex)
                    {
                        Error("RoleIndex out of range");
                        Complete = true;
                    }
                }
            }
        }
        static void ConnectionInfo_StatusChanged(object sender, EventArgs e)
        {
            if (host.ConnectionInfo.Status == Magic.Data.ConnectionStatus.Disconnected)
            {
                if (!Complete)
                {
                    Error("Disconnected");
                }
                else
                {
                    Trace("Disconnected");
                }
                Complete = true;
            }
            else
            {
                if (host.ConnectionInfo.Status == Magic.Data.ConnectionStatus.Connected)
                {
                    Info("Connected");
                }
                else
                {
                    Trace("Connecting");
                }
            }
        }
        static void OnRoleList_Receive(object sender, PacketEventArgs e)
        {
            var role = e.Packet as RoleList_ReS53;
            if (role.IsChar)
            {
                Info("Role {0} level={1} occupation={2} gender={3} create={4} lastenter={5}", 
                    role.Role.Name,
                    role.Role.Level,
                    role.Role.Occupation.OccupationId,
                    role.Role.Gender.GenderId,
                    role.Role.CreateTime.Time.ToString("u"),
                    role.Role.LastOnline.Time.ToString("u")
                );
            }
        }
        static void OnCreateRole_Receive(object sender, PacketEventArgs e)
        {
            var role = e.Packet as CreateRole_ReS55;
            if (role.ResultCode.ResultCode == 0)
            {
                Info("Role {0} level={1} occupation={2} gender={3} create={4} lastenter={5}", 
                    role.Role.Name,
                    role.Role.Level,
                    role.Role.Occupation.OccupationId,
                    role.Role.Gender.GenderId,
                    role.Role.CreateTime.Time.ToString("u"),
                    role.Role.LastOnline.Time.ToString("u")
                );
            }
            else
            {
                Warn("Create role errorcode: {0}", role.ResultCode.ResultCode);
                switch (role.ResultCode.ResultCode)
                {
                    case 25: Warn("Create role error: you can not use this nickname"); break;
                    case 45: Warn("Create role error: nickname is already used"); break;
                    default: Warn("Create role error: unknown error"); break;
                }
            }
        }
        static void OnlineAnnounce(object sender, PacketEventArgs e)
        {
            var online = e.Packet as OnlineAnnounceS04;
            Info("Account id: {0}", online.AccountId);
            Info("Local id: {0}", online.UnkId);
            Info("S04 Remain time: {0}", online.RemainTime.Time.ToString("u"));
            Info("S04 Zone id: {0}", online.ZoneId);
            if (online.FreeTimeLeft != null) Info("S04 Free time left: {0}", online.FreeTimeLeft);
            if (online.FreeTimeEnd != null) Info("S04 Free time end: {0}", online.FreeTimeEnd);
            if (online.CreateTime != null) Info("S04 Create time: {0}", online.CreateTime);
            if (online.RefererFlag != null) Info("S04 Referer flag: {0}", online.RefererFlag);
            if (online.PasswordFlag != null) Info("S04 Password flag: {0}", online.PasswordFlag);
            if (online.Usbbind != null) Info("S04 Usbbind flag: {0}", online.Usbbind);
            if (online.AccountInfoFlag != null) Info("S04 Account info flag: {0}", online.AccountInfoFlag);
        }
        static void ForbidInfo(object sender, PacketEventArgs e)
        {
            var forbid = e.Packet as AnnounceForbidInfoS7B;
            Error("Forbid reason: {0}", forbid.Forbid.Reason);
            Error("Forbid time: {0} ({1})", forbid.Forbid.Time, DateTime.Now.AddSeconds(forbid.Forbid.Time).ToString("u"));
            Error("Forbid accoundId: {0}", forbid.AccountId);
            Error("Forbid disconnect: {0}", forbid.Disconnect);
            Error("Forbid create time: {0}", forbid.Forbid.Createtime.Time.ToString("u"));
            Error("Forbid type: {0}", forbid.Forbid.Type);
        }
        static void ErrorInfo(object sender, PacketEventArgs e)
        {
            var error = e.Packet as ErrorInfoS05;
            Error("s05 error code: {0}", error.ResultCode);
            Error("s05 error message: {0}", error.Message);
            host.Close();
        }
    }
}
