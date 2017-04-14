using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Security.Principal;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace ScoringEngine
{
    public class UserScoredItem : ScoredItem
    {
        [JsonProperty("username")]
        public string Username { get; protected set; }
        [JsonProperty("check")]
        public UserScoredItemType Check { get; protected set; }
        [JsonProperty("shouldBeTrue")]
        public bool ShouldBeTrue { get; protected set; } = true;
        [JsonProperty("groupName")]
        public string GroupName { get; protected set; } = null;

        public override bool CheckScored()
        {
            switch (ScoredItemParser.GetOperatingSystem())
            {
                case ScoredItemParser.OS.Windows:
                    return checkedScoredWindows();
                case ScoredItemParser.OS.Unix:
                    return checkedScoredWindows();
                default:
                    throw new PlatformNotSupportedException("This platform is not supported: " + Environment.OSVersion.Platform);
            }
        }

        private bool checkedScoredWindows()
        {
            SelectQuery query = new SelectQuery("Win32_UserAccount");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            foreach (ManagementObject envVar in searcher.Get())
            {
                if (envVar["Name"].ToString() == Username)
                {
                    switch (Check)
                    {
                        case UserScoredItemType.ShouldExist:
                            return ShouldBeTrue;
                        case UserScoredItemType.IsEnabled:
                            return ShouldBeTrue != bool.Parse(envVar["Disabled"].ToString()); // A true test in boolean logic
                        case UserScoredItemType.InGroup:
                            return ShouldBeTrue == windowsUserInGroup(envVar["Name"].ToString(), GroupName);
                        case UserScoredItemType.PasswordChanged:
                            throw new NotImplementedException();
                        case UserScoredItemType.PasswordChangeable:
                            return ShouldBeTrue == bool.Parse(envVar["PasswordChangeable"].ToString());
                        case UserScoredItemType.PasswordExpires:
                            return ShouldBeTrue == bool.Parse(envVar["PasswordExpires"].ToString());
                        default:
                            throw new InvalidProgramException("If you got here the program is invalid?");
                    }
                }
                Console.WriteLine("Username : {0}", envVar["Name"]);
            }

            if (Check == UserScoredItemType.ShouldExist) return !ShouldBeTrue;

            return false;
        }

        private bool windowsUserInGroup(string username, string group)
        {
            return listUserGroups(username).Contains(group);
        }

        private static List<string> listUserGroups(string username)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_UserAccount");
            foreach (ManagementObject user in searcher.Get())
            {
                if (user["Name"].ToString() == username)
                {
                    List<string> userGroups = new List<string>();
                    string domain;
                    if (string.IsNullOrEmpty(System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName))
                        domain = Environment.MachineName;
                    else
                        domain = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;

                    ManagementObjectSearcher groupSearcher = new ManagementObjectSearcher(string.Format("SELECT * FROM Win32_GroupUser WHERE PartComponent=\"Win32_UserAccount.Domain='{1}',Name='{2}'\"", domain, domain, ((string)user["Name"]).Replace("'", "\"")));
                    foreach (ManagementObject group in groupSearcher.Get())
                    {
                        string nameBegin = "Name=\"";
                        string groupString = (string)group["GroupComponent"];
                        groupString = groupString.Substring(groupString.IndexOf(nameBegin) + nameBegin.Length);
                        groupString = groupString.Substring(0, groupString.IndexOf("\""));
                        userGroups.Add(groupString);
                    }

                    return userGroups;
                }
            }

            return null;
        }

        private bool checkedScoredUnix()
        {
            switch (Check)
            {
                case UserScoredItemType.ShouldExist:
                    return ShouldBeTrue == (File.ReadAllLines("/etc/passwd").Where(x => x.StartsWith(Username + ":")).Count() == 1);
                case UserScoredItemType.InGroup:
                    return ShouldBeTrue == (File.ReadAllLines("/etc/group").Where(x => new Regex($"{Regex.Escape(GroupName)}:(.*,)?{Regex.Escape(Username)},?$").IsMatch(x)).Count() == 1);
                case UserScoredItemType.PasswordChanged:
                    throw new NotImplementedException();
                default:
                    throw new PlatformNotSupportedException("This function is not supported on this platform: " + Environment.OSVersion.Platform);
            }
        }

        public enum UserScoredItemType
        {
            ShouldExist,
            IsEnabled,
            InGroup,
            PasswordChanged,
            PasswordChangeable,
            PasswordExpires,
        }
    }
}
