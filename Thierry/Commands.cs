﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace Thierry
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        private static readonly List<HoekUser> Hoek = new List<HoekUser>();

        [Command("anti9gag")]
        [RequireHatminRole]
        public async Task Anti9Gag()
        {
            Program.anti9Gag = !Program.anti9Gag;
        }

        [Command("beepboop")]
        public async Task Beepboop()
        {
            await ReplyAsync("Ik ben een robot!");
        }

        [Command("givehat")]
        [RequireHatminRole]
        public async Task GiveHat(SocketGuildUser user)
        {
            await Program.Prog.GiveHat(user);
        }

        [Command("help")]
        public async Task Help()
        {
            //print helptext
            //json help file maken
            await ReplyAsync("Ze heeft weeral gemorst, film het maar.");
        }

        [Command("indenhoek")]
        [RequireHatminRole]
        public async Task InDenHoek(SocketGuildUser user)
        {
            if (user.Roles.Contains(Program.Guild.MutedRole))
            {
                await ReplyAsync($"{user.Mention} staat al in den hoek.");
                return;
            }

            await user.AddRoleAsync(Program.Guild.MutedRole);
            Hoek.Add(new HoekUser(user));
            await ReplyAsync($"{user.Mention} staat nu in den hoek.");
        }

        [Command("lindsey")]
        public async Task Lindsey()
        {
            await ReplyAsync("https://www.youtube.com/watch?v=v39Xh4hk8Jw");
        }

        [Command("takehat")]
        [Alias("removehat")]
        [RequireHatminRole]
        public async Task RemoveHat()
        {
            if (Program.Guild.SocketGuild.GetRole(Program.Guild.HatRole.Id).Members.Any())
            {
                await ReplyAsync($"{Program.Guild.LastHat.Mention} The Lord giveth, and the Lord taketh away.");
                Program.Prog.RemoveHat();
            }
        }

        [Command("Thierry")]
        public async Task Thierry(SocketGuildUser user)
        {
            await ReplyAsync($"{user.Mention}! {user.Mention}! {user.Mention}!");
        }

        [Command("TRUT")]
        public async Task Trut()
        {
            await ReplyAsync(
                "Als er al spanningen zijn geweest, dan zijn die nu allemaal weg, daarmee hebben we die prijs gewonnen eh.");
        }

        [Command("uitdenhoek")]
        [RequireHatminRole]
        public async Task UitDenHoek(SocketGuildUser user)
        {
            HoekUser temp = null;
            var voted = Context.User as SocketGuildUser;

            if (user.Roles.Contains(Program.Guild.MutedRole))
                temp = Hoek.FirstOrDefault(x => x.User == user);

            if (temp == null) return;

            if (temp.Voted.Contains(voted))
            {
                await ReplyAsync("Gij hebt al ne keer gevote trut!");
                return;
            }

            temp.Votes++;
            temp.Voted.Add(voted);


            if (temp.Votes >= 2)
            {
                await user.RemoveRoleAsync(Program.Guild.MutedRole);
                Hoek.RemoveAll(x => x.User == user);
                await ReplyAsync($"{temp.User.Mention} has been released.");
                return;
            }

            await ReplyAsync($"{temp.User.Mention} currently has {temp.Votes} votes to be released.");
        }

        [Command("version")]
        [Alias("Version")]
        public async Task Version()
        {
            string version;
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream("Thierry.commithash.txt"))
            {
                using (var reader = new StreamReader(stream))
                {
                    version = reader.ReadToEnd();
                }
            }

            await ReplyAsync($"Running on git commit {version}");
        }
    }
}