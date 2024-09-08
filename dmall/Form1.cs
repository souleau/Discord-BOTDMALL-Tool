using DSharpPlus;
using DSharpPlus.Entities;
using KeyAuth;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dmall
{
    public partial class Form1 : Form
    {

        public static api KeyAuthApp = new api(
        name: "", // Application Name
        ownerid: "", // Owner ID
        secret: "", // Application Secret
        version: "" // Application Version /*
                       //path: @"Your_Path_Here" // (OPTIONAL) see tutorial here https://www.youtube.com/watch?v=I9rxt821gMk&t=1s

         );

        private DiscordClient discord;
        private CancellationTokenSource cts;
        private Random random;
        public Form1()
        {
            random = new Random();
            InitializeComponent();
        }

        private void Log(string message)
        {
            if (this.logTextBox.InvokeRequired)
            {
                this.logTextBox.Invoke(new Action(() => this.logTextBox.AppendText(message + Environment.NewLine)));
            }
            else
            {
                this.logTextBox.AppendText(message + Environment.NewLine);
            }
        }

        private async void startdm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tokentextbox.Text))
            {
                Log("Erreur : le token est vide.");
                return;
            }

            cts = new CancellationTokenSource();

            discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = tokentextbox.Text,
                TokenType = TokenType.Bot
            });

            try
            {
                await discord.ConnectAsync();
                Log("Bot link to Discord");

                var guildId = ulong.Parse(guiIDtextbox.Text);
                var guild = await discord.GetGuildAsync(guildId);
                Log($"Connecté au serveur : {guild.Name}");

                var members = await guild.GetAllMembersAsync();
                Log($"Nombre total de membres : {members.Count}");

                foreach (var member in members)
                {
                    if (cts.Token.IsCancellationRequested)
                    {
                        Log("Envoi des messages fini");
                        break;
                    }

                    if (!member.IsBot)
                    {
                        try
                        {
                            var mentionMessage = $"<@{member.Id}> {messagetextbox.Text}";
                            await member.SendMessageAsync(mentionMessage);
                            Log($"Message envoyé à {member.Username}#{member.Discriminator} + mention fdp");
                        }
                        catch (Exception ex)
                        {
                            Log($"Échec de l'envoi du message à {member.Username}#{member.Discriminator} : {ex.Message}");
                        }
                    }
                    int delay = random.Next(10, 501);
                    await Task.Delay(delay);
                }

                Log("Envoi des messages fini");
            }
            catch (Exception ex)
            {
                Log($"Erreur de connexion : {ex.Message}");
            }
        }


        private void stopdm_Click(object sender, EventArgs e)
        {
            cts?.Cancel();
            Log("sending messages canceled.");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           // KeyAuthApp.init();
           // KeyAuthApp.check();
        }

        private void label2_Click(object sender, EventArgs e)
        {
           
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            /**
            KeyAuthApp.license(licensetextbox.Text);
            if (KeyAuthApp.response.success)
            {
                MessageBox.Show("License", "Login", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                guna2Panel2.Hide();
            }
            else
            {
                Application.Exit();
            }
            **/
        }
    }
}